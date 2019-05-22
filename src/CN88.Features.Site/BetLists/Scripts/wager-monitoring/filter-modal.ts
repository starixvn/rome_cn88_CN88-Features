class FilterParam {
    constructor(
        public sportGroup = 0,
        public isSelectAllSports = true,
        public stakeMode = 1,
        public stakeLimit = 0,
        public sportStakeConditions = "",
        public market = "",
        public leagueIds = "",
        public matchIds = "",
        public betTypeGroupIds = "") {
    }
}

class FilterModal {
    $: JQueryStatic;
    $filterModalContainer: JQuery;
    $filterModalPath: JQuery;
    $clearSearchLeagueButton: JQuery;
    $searchLeagueTextbox: JQuery;
    $clearSearchEventButton: JQuery;
    $searchEventTextbox: JQuery;
    $perSportList: JQuery;
    onSubmit: Function;

    filterParam: FilterParam;
    marketMultiSelectionComponent: MultiSelectionCheckboxComponent;
    leagueMultiSelectionComponent: MultiSelectionCheckboxComponent;
    eventMultiSelectionComponent: MultiSelectionCheckboxComponent;
    betTypeMultiSelectionComponent: MultiSelectionCheckboxComponent;

    sportGroup: number;
    isOpeningFilter = false;
    enterKeyCode = 13;

    cleaveAllSportStake: any;
    cleavePerSportStake = {};

    constructor($: JQueryStatic) {
        this.$ = $;
        this.$filterModalContainer = $("#filter-modal");
        this.$filterModalPath = $("#FilterModalPath");

        this.marketMultiSelectionComponent = new MultiSelectionCheckboxComponent($);
        this.leagueMultiSelectionComponent = new MultiSelectionCheckboxComponent($);
        this.eventMultiSelectionComponent = new MultiSelectionCheckboxComponent($);
        this.betTypeMultiSelectionComponent = new MultiSelectionCheckboxComponent($);
    }

    show = (targetTabId: string, filterParam: FilterParam = new FilterParam(), onSubmit: Function = null) => {
        this.filterParam = this.$.extend({}, this.filterParam, filterParam) as FilterParam;
        this.onSubmit = onSubmit;

        this.$.blockUI();
        this.isOpeningFilter = true;
        this.$.ajax({
            method: "POST",
            url: this.$filterModalPath.val(),
            cache: false,
            data: this.filterParam,
            success: (result) => {
                this.showFilterModal(result, targetTabId);
            },
            complete: this.$.unblockUI
        });
    }

    hide(): void {
        this.$filterModalContainer.modal("hide");
    }

    showFilterModal(html: string, targetTab: string): void {
        this.$filterModalContainer.html(html);

        this.cachDOMElements();
        this.renderComponents();
        this.registerEvents();
        this.$(targetTab).click();
        this.$filterModalContainer.modal({ show: true, backdrop: true });
        this.isOpeningFilter = false;
    }

    cachDOMElements(): void {
        this.$clearSearchLeagueButton = $("#btnClearSearchLeague");
        this.$searchLeagueTextbox = $("#txtSearchLeague");
        this.$clearSearchEventButton = $("#btnClearSearchEvent");
        this.$searchEventTextbox = $("#txtSearchEvent");
        this.$perSportList = $("#list-persports");
    }

    renderComponents(): void {
        this.$(".dropdown-stake-condition > select").multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
        });

        this.cleaveAllSportStake = new Cleave('#AllSportsStakeLimit', {
            numeral: true,
            numeralPositiveOnly: true,
            numeralDecimalMark: '.',
            delimiter: ','
        });

        const $perSportsConditions = this.$perSportList.children();
        const self = this;
        $perSportsConditions.each(function () {
            const id = self.$(this).data("id");
            self.cleavePerSportStake[id] = new Cleave('#StakeLimit-' + id, {
                numeral: true,
                numeralPositiveOnly: true,
                numeralDecimalMark: '.',
                delimiter: ','
            });
        });

        this.marketMultiSelectionComponent.render(new MultiSelectionCheckboxOption("checkbox-all-markets", "list-market", "Market"));
        this.leagueMultiSelectionComponent.render(new MultiSelectionCheckboxOption("check-all-leagues", "list-league", "LeagueIds"));
        this.eventMultiSelectionComponent.render(new MultiSelectionCheckboxOption("checkbox-all-events", "list-event", "MatchIds"));
        this.betTypeMultiSelectionComponent.render(new MultiSelectionCheckboxOption("checkbox-all-bettypes", "list-bettype", "BetTypeGroupIds"));
    }

    registerEvents(): void {
        this.$("input[name='stake-filter-option']").change(() => {
            this.onStakeFilterOptionChange();
        });

        const self = this;
        const mainMarkets = self.$("[data-group-id='chk-main-markets']");
        const otherMarkets = self.$("[data-group-id='chk-other-markets']");

        this.$("#chk-main-markets").on("click touchend", function () {
            self.clickCheckboxAll(mainMarkets, this);
        });

        this.$("#chk-other-markets").on("click touchend", function () {
            self.clickCheckboxAll(otherMarkets, this);
        });

        mainMarkets.on("click touchend", function () {
            if (this.checked) {
                self.isCheckAll(mainMarkets, self.$("#chk-main-markets"));
            }
        });

        otherMarkets.on("click touchend", function () {
            if (this.checked) {
                self.isCheckAll(otherMarkets, self.$("#chk-other-markets"));
            }
        });

        this.$("#btnSearchLeague").on("click touchend", () => {
            self.searchLeague();
        });

        this.$searchLeagueTextbox.keyup(function (e) {
            if (e.keyCode === self.enterKeyCode) {
                self.searchLeague();
            }
        });

        this.$clearSearchLeagueButton.on("click touchend", () => {
            this.$searchLeagueTextbox.val("");
            this.leagueMultiSelectionComponent.clearSearch();
        });

        this.$("#btnSearchEvent").on("click touchend", () => {
            self.searchEvent();
        });

        this.$searchEventTextbox.keyup(function (e) {
            if (e.keyCode === self.enterKeyCode) {
                self.searchEvent();
            }
        });

        this.$clearSearchEventButton.on("click touchend", () => {
            this.$searchEventTextbox.val("");
            this.eventMultiSelectionComponent.clearSearch();
        });

        this.$("#btn-submit").on("click touchend", () => {
            this.onSubmit();
            this.hide();
        });

        this.$("#btn-reset").on("click touchend", this.resetFilter);
    }

    searchLeague(): void {
        const searchText = this.$searchLeagueTextbox.val();
        if (searchText) {
            this.leagueMultiSelectionComponent.search(searchText);
        } else {
            this.leagueMultiSelectionComponent.clearSearch();
        }
    }

    searchEvent(): void {
        const searchText = this.$searchEventTextbox.val();
        if (searchText) {
            this.eventMultiSelectionComponent.search(searchText);
        } else {
            this.eventMultiSelectionComponent.clearSearch();
        }
    }

    clickCheckboxAll = (groupItems, allCheckbox) => {
        if (allCheckbox.checked) {
            groupItems.prop("checked", true);

            this.betTypeMultiSelectionComponent.isCheckAll();
            return;
        }

        groupItems.prop("checked", false);
    };

    isCheckAll = (groupItems, allCheckbox) => {
        let isAllChecked = true;

        groupItems.each(function () {
            if (!this.checked) {
                isAllChecked = false;
            }
        });

        if (isAllChecked) {
            allCheckbox.prop("checked", true);
        }
    };

    getParameters = () => {
        let conditions = [];
        const isSelectAllSports = Common.Utils.ParseBoolean(this.$("input[name='stake-filter-option']:checked").val());
        if (!isSelectAllSports) {
            const $perSportsConditions = this.$perSportList.children();
            const self = this;
            $perSportsConditions.each(function () {
                const $this = self.$;
                const sportGroupId = $this(this).data("id");
                const stakeMode = $this(this).find("select").children("option:selected").val();
                const stakeLimit = self.cleavePerSportStake[sportGroupId].getRawValue();
                conditions.push({
                    sportGroupId: sportGroupId,
                    stakeMode: stakeMode,
                    stakeLimit: stakeLimit
                });
            });
        }

        return {
            sportGroup: this.sportGroup,
            isSelectAllSports: isSelectAllSports,
            stakeMode: this.$("#AllSportStakeMode").children("option:selected").val(),
            stakeLimit: this.cleaveAllSportStake.getRawValue(),
            sportStakeConditions: JSON.stringify(conditions),
            markets: this.marketMultiSelectionComponent.getSelectedItemTexts().join(","),
            marketIds: this.marketMultiSelectionComponent.getSelectedItems().join(","),
            leagueIds: this.leagueMultiSelectionComponent.getSelectedItems(),
            matchIds: this.eventMultiSelectionComponent.getSelectedItems(),
            betTypeGroupIds: this.betTypeMultiSelectionComponent.getSelectedItems()
        };
    };

    onStakeFilterOptionChange(): void {
        const listAllSportsId = "#list-allsports";
        const listPerSportsId = "#list-persports";
        let enabledElement: string;
        let disabledElement: string;

        const isSelectAllSports = Common.Utils.ParseBoolean(this.$("input[name='stake-filter-option']:checked").val());

        if (isSelectAllSports) {
            enabledElement = listAllSportsId;
            disabledElement = listPerSportsId;
        } else {
            enabledElement = listPerSportsId;
            disabledElement = listAllSportsId;
        }

        this.$(enabledElement).each(function () {
            $(this).find("button")
                .removeAttr("disabled")
                .removeClass("disabled");

            $(this).find("input")
                .removeAttr("disabled");
        });

        this.$(disabledElement).each(function () {
            $(this).find("button")
                .attr("disabled", "disabled")
                .addClass("disabled");

            $(this).find("input")
                .attr("disabled", "disabled");
        });
    }

    resetFilter = () => {
        this.$("#rd-all-sports").click();

        this.$(".dropdown-stake-condition > select").multiselect("select", 1);

        const $allSportsStakeLimit = this.$("#AllSportsStakeLimit");
        $allSportsStakeLimit.val(0);

        this.$(".StakeLimit").val(0);

        const $allMarketsCheckbox = this.$("#checkbox-all-markets");
        $allMarketsCheckbox.prop("checked", true);
        $allMarketsCheckbox.change();

        this.$clearSearchLeagueButton.click();
        this.$clearSearchEventButton.click();

        const $allBetTypesCheckbox = this.$("#checkbox-all-bettypes");
        $allBetTypesCheckbox.prop("checked", true);
        $allBetTypesCheckbox.change();
    }
}