class FilterParam {
    constructor(
        public sportGroup: number = 0,
        public isSelectAllSports: boolean = true,
        public stakeMode: number = 1,
        public stakeLimit: number = 0,
        public sportStakeConditions: string = "",
        public market: string = "",
        public leagueIds: string = "",
        public matchIds: string = "",
        public betTypeGroupIds: string = "") {
    }
}

class FilterModal {
    $: JQueryStatic;
    $filterModalContainer: JQuery;
    $filterModalPath: JQuery;
    $clearSearchLeagueButton: JQuery;
    $clearSearchEventButton: JQuery;
    onSubmit: Function;

    filterParam: FilterParam;
    marketMultiSelectionComponent: MultiSelectionCheckboxComponent;
    leagueMultiSelectionComponent: MultiSelectionCheckboxComponent;
    eventMultiSelectionComponent: MultiSelectionCheckboxComponent;
    betTypeMultiSelectionComponent: MultiSelectionCheckboxComponent;

    sportGroup: number;
    isOpeningFilter: boolean = false;

    cleaveAllSportStake: any;
    cleavePerSportStake: any = {};

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
        this.filterParam = <FilterParam>this.$.extend({}, this.filterParam, filterParam);
        this.onSubmit = onSubmit;

        this.$.blockUI();
        this.isOpeningFilter = true;
        this.$.ajax({
            method: "GET",
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
        this.$clearSearchEventButton = $("#btnClearSearchEvent");
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

        let $perSportsConditions = this.$("#list-persports").children();
        let self = this;
        $perSportsConditions.each(function () {
            let id = self.$(this).data("id");
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

        var self = this;
        var mainMarkets = self.$("[data-group-id='chk-main-markets']");
        var otherMarkets = self.$("[data-group-id='chk-other-markets']");
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
            let searchText = this.$("#txtSearchLeague").val();
            if (searchText) {
                this.leagueMultiSelectionComponent.search(this.$("#txtSearchLeague").val());
            } else {
                this.leagueMultiSelectionComponent.clearSearch();
            }
        });

        this.$clearSearchLeagueButton.on("click touchend", () => {
            this.$("#txtSearchLeague").val("");
            this.leagueMultiSelectionComponent.clearSearch();
        });

        this.$("#btnSearchEvent").on("click touchend", () => {
            let searchText = this.$("#txtSearchEvent").val();
            if (searchText) {
                this.eventMultiSelectionComponent.search(this.$("#txtSearchEvent").val());
            } else {
                this.eventMultiSelectionComponent.clearSearch();
            }
        });

        this.$clearSearchEventButton.on("click touchend", () => {
            this.$("#txtSearchEvent").val("");
            this.eventMultiSelectionComponent.clearSearch();
        });

        this.$("#btn-submit").on("click touchend", () => {
            this.onSubmit();
            this.hide();
        });

        this.$("#btn-reset").on("click touchend", this.resetFilter);
    }

    clickCheckboxAll = (groupItems, allCheckbox) => {
        if (allCheckbox.checked) {
            groupItems.attr("disabled", "disabled");
            groupItems.prop("checked", true);

            this.betTypeMultiSelectionComponent.isCheckAll();
            return;
        }

        groupItems.attr("disabled", null);
        groupItems.prop("checked", false);
    };

    isCheckAll = (groupItems, allCheckbox) => {
        let isAllChecked = true;

        groupItems.each(function () {
            if (!this.checked) {
                isAllChecked = false;
            }
        })

        if (isAllChecked) {
            allCheckbox.prop("checked", true);
            groupItems.attr("disabled", "disabled");
        }
    };

    getParameters = () => {
        let conditions = [];
        let isSelectAllSports = Common.Utils.ParseBoolean(this.$("input[name='stake-filter-option']:checked").val());
        if (!isSelectAllSports) {
            let $perSportsConditions = this.$("#list-persports").children();
            let self = this;
            $perSportsConditions.each(function () {
                let $this = self.$;
                let sportGroupId = $this(this).data("id");
                let stakeMode = $this(this).find("select").children("option:selected").val();
                let stakeLimit = self.cleavePerSportStake[sportGroupId].getRawValue();
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
        let listAllSportsId = "#list-allsports";
        let listPerSportsId = "#list-persports";
        let enabledElement: string;
        let disabledElement: string;

        let isSelectAllSports = Common.Utils.ParseBoolean(this.$("input[name='stake-filter-option']:checked").val());

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

        var $AllSportsStakeLimit = this.$("#AllSportsStakeLimit");
        $AllSportsStakeLimit.val(0);

        this.$(".StakeLimit").val(0);

        let $allMarketsCheckbox = this.$("#checkbox-all-markets");
        $allMarketsCheckbox.prop("checked", true);
        $allMarketsCheckbox.change();

        this.$clearSearchLeagueButton.click();
        this.$clearSearchEventButton.click();

        let $allBetTypesCheckbox = this.$("#checkbox-all-bettypes");
        $allBetTypesCheckbox.prop("checked", true);
        $allBetTypesCheckbox.change();
    }
}