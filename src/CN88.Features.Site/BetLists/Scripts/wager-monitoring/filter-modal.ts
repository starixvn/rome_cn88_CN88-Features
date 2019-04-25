class FilterModal {
    $: JQueryStatic;
    $stakeFilterOption: JQuery;
    $filterFormPath: JQuery;

    marketMultiSelectionComponent: MultiSelectionCheckboxComponent;
    leagueMultiSelectionComponent: MultiSelectionCheckboxComponent;
    eventMultiSelectionComponent: MultiSelectionCheckboxComponent;
    betTypeMultiSelectionComponent: MultiSelectionCheckboxComponent;

    sportGroup: number;
    onSubmitSuccess: Function;

    constructor($: JQueryStatic) {
        this.$ = $;

        this.marketMultiSelectionComponent = new MultiSelectionCheckboxComponent($);
        this.leagueMultiSelectionComponent = new MultiSelectionCheckboxComponent($);
        this.eventMultiSelectionComponent = new MultiSelectionCheckboxComponent($);
        this.betTypeMultiSelectionComponent = new MultiSelectionCheckboxComponent($);
    }

    render(onSubmitSuccess: Function): void {
        this.onSubmitSuccess = onSubmitSuccess;

        this.cacheDOMElements();
        this.renderComponents();
        this.registerEvents();
    }

    cacheDOMElements(): void {
        this.$stakeFilterOption = this.$("input[name='stake-filter-option']");
        this.$filterFormPath = this.$("#FilterFormPath");
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
        this.$stakeFilterOption.change(() => {
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

        this.$("#btn-submit").click((): void => {
            this.$.blockUI();

            let conditions = [];
            let isSelectAllSports = Common.Utils.ParseBoolean(this.$("input[name='stake-filter-option']:checked").val());
            if (!isSelectAllSports) {
                let $perSportsConditions = this.$("#list-persports").children();
                $perSportsConditions.each(function () {
                    let sportGroupId = $(this).data("id");
                    let stakeMode = $(this).find("select").children("option:selected").val();
                    let stakeLimit = $(this).find(".StakeLimit").val();
                    conditions.push({
                        sportGroupId: sportGroupId,
                        stakeMode: stakeMode,
                        stakeLimit: stakeLimit
                    });
                });
            }

            let param = {
                sportGroup: this.sportGroup,
                isSelectAllSports: isSelectAllSports,
                stakeMode: this.$("#AllSportStakeMode").children("option:selected").val(),
                stakeLimit: this.$("#AllSportsStakeLimit").val(),
                sportStakeConditions: JSON.stringify(conditions),
                market: this.marketMultiSelectionComponent.getSelectedItems().join(","),
                leagueIds: this.leagueMultiSelectionComponent.getSelectedItems().join(","),
                matchIds: this.eventMultiSelectionComponent.getSelectedItems().join(","),
                betTypeGroupIds: this.betTypeMultiSelectionComponent.getSelectedItems().join(",")
            };

            this.$.ajax({
                method: "GET",
                url: this.$filterFormPath.val(),
                cache: false,
                data: param,
                success: (html) => {
                    this.onSubmitSuccess(html);
                },
                complete: this.$.unblockUI
            });
        });
    }

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