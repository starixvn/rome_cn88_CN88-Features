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
    onSubmit: Function;

    filterParam: FilterParam;
    marketMultiSelectionComponent: MultiSelectionCheckboxComponent;
    leagueMultiSelectionComponent: MultiSelectionCheckboxComponent;
    eventMultiSelectionComponent: MultiSelectionCheckboxComponent;
    betTypeMultiSelectionComponent: MultiSelectionCheckboxComponent;

    sportGroup: number;
    isOpeningFilter: boolean = false;

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
        this.renderComponents();
        this.registerEvents();
        this.$(targetTab).click();
        this.$filterModalContainer.modal({ show: true, backdrop: false });
        this.isOpeningFilter = false;
    }

    renderComponents(): void {
        this.$(".dropdown-stake-condition > select").multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
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

        if (this.sportGroup === 0) {
            let $betTypeGroupCheckboxes = this.$("#list-bettype .group-name input[type='checkbox']");

            $betTypeGroupCheckboxes.change(function () {
                let $groupCheckbox = $(this);
                let $checkboxes = $groupCheckbox.parent().siblings(".group-details").find("input[type='checkbox']");
                $checkboxes.prop("checked", this.checked);

                if (this.checked) {
                    $checkboxes.attr("disabled", "disabled");
                } else {
                    $checkboxes.removeAttr("disabled");
                }

                $checkboxes.click(function () {
                    if (this.checked) {
                        let isAllChecked = true;

                        $checkboxes.each(function () {
                            if (!this.checked) {
                                isAllChecked = false;
                            }
                        })

                        if (isAllChecked) {
                            $groupCheckbox.prop("checked", true);
                            $checkboxes.attr("disabled", "disabled");
                        }
                    } else {
                        $groupCheckbox.prop("checked", false);
                    }
                });
            });

            $betTypeGroupCheckboxes.click(function () {
                let $selectAll = $("#checkbox-all-bettypes");
                if (this.checked) {
                    let isAllChecked = true;

                    $betTypeGroupCheckboxes.each(function () {
                        if (!this.checked) {
                            isAllChecked = false;
                        }
                    })

                    if (isAllChecked) {
                        $selectAll.prop("checked", true);
                        $betTypeGroupCheckboxes.attr("disabled", "disabled");
                    }
                } else {
                    $selectAll.prop("checked", false);
                }
            });
        }

        this.$("#btn-submit").click(() => {
            this.onSubmit();
        });
    }

    getParameters = () => {
        let conditions = [];
        let isSelectAllSports = Common.Utils.ParseBoolean(this.$("input[name='stake-filter-option']:checked").val());
        if (!isSelectAllSports) {
            let $perSportsConditions = this.$("#list-persports").children();
            let $this = this.$;
            $perSportsConditions.each(function () {
                let sportGroupId = $this(this).data("id");
                let stakeMode = $this(this).find("select").children("option:selected").val();
                let stakeLimit = $this(this).find(".StakeLimit").val();
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
            stakeLimit: this.$("#AllSportsStakeLimit").val(),
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
}