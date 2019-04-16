class WagerMonitoring {
    $: JQueryStatic;
    $tblBetList: JQuery;
    $filterForm: JQuery;
    $filterModal: JQuery;
    $resetButton: JQuery;
    $submitButton: JQuery;

    $filterModalPath: JQuery;
    $filterFormPath: JQuery;
    $wagerMonitoringPath: JQuery;

    ajaxRequest: JQueryXHR;
    sportGroupSelectionComponent: SingleSelectionDropdownComponent;
    marketMultiSelectionComponent: MultiSelectionCheckboxComponent;
    leagueMultiSelectionComponent: MultiSelectionCheckboxComponent;
    eventMultiSelectionComponent: MultiSelectionCheckboxComponent;
    betTypeMultiSelectionComponent: MultiSelectionCheckboxComponent;

    pageSize: number;
    sportGroup: number;
    getBetsUrl: string;
    intervalTime: number;
    isCalling: boolean = false;
    isOpeningFilter: boolean = false;

    constructor(
        $: JQueryStatic,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        marketMultiSelectionComponent: MultiSelectionCheckboxComponent,
        leagueMultiSelectionComponent: MultiSelectionCheckboxComponent,
        eventMultiSelectionComponent: MultiSelectionCheckboxComponent,
        betTypeMultiSelectionComponent: MultiSelectionCheckboxComponent) {
        this.$ = $;
        this.sportGroupSelectionComponent = sportGroupSelectionComponent;
        this.marketMultiSelectionComponent = marketMultiSelectionComponent;
        this.leagueMultiSelectionComponent = leagueMultiSelectionComponent;
        this.eventMultiSelectionComponent = eventMultiSelectionComponent;
        this.betTypeMultiSelectionComponent = betTypeMultiSelectionComponent;
    }

    init(): void {
        this.$.blockUI();

        this.cacheElements();
        this.renderComponents();
        this.registerEvents();

        this.pageSize = this.$("#PageSize").val();
        this.getBetsUrl = this.$("#get-bets-url").val();
        this.intervalTime = this.$("#IntervalTime").val();
        this.sportGroup = parseInt(this.$("#SportGroup").val());

        setInterval(() => { this.getLastBets() }, this.intervalTime * 1000);

        this.$.unblockUI();
    }

    cacheElements(): void {
        this.$tblBetList = this.$("#tbl-betlist");
        this.$filterForm = this.$("#filter-form");
        this.$filterModal = this.$("#filter-modal");
        this.$filterFormPath = this.$("#FilterFormPath");
        this.$filterModalPath = this.$("#FilterModalPath");
        this.$wagerMonitoringPath = this.$("#WagerMonitoringPath");
    }

    renderComponents(): void {
        this.sportGroupSelectionComponent.render(
            new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId", () => { this.onSportGroupChange(); }));
    }

    onSportGroupChange(): void {
        var sportGroup = this.sportGroupSelectionComponent.$dropdown.val();
        window.location.href = this.$wagerMonitoringPath.val() + "?SportGroup=" + sportGroup;
    }

    registerEvents(): void {
        let self = this;

        this.$filterForm.find(".link").on("click", function () {
            let link = $(this);
            self.loadFilterModal(link.data("target"));
        });
    }

    loadFilterModal(targetTab: string): void {
        this.$.blockUI();
        this.isOpeningFilter = true;
        let param = {
            sportGroup: this.sportGroup,
            isSelectAllSports: this.$("#IsSelectAllSports").val(),
            stakeMode: this.$("#StakeMode").val(),
            stakeLimit: this.$("#StakeLimit").val(),
            sportStakeConditions: this.$("#SportStakeConditions").val(),
            market: this.$("#Market").val(),
            leagueIds: this.$("#LeagueIds").val(),
            matchIds: this.$("#MatchIds").val(),
            betTypeGroupIds: this.$("#BetTypeGroupIds").val()
        };

        this.$.ajax({
            method: "GET",
            url: this.$filterModalPath.val(),
            cache: false,
            data: param,
            success: (result) => {
                this.showFilterModal(result, targetTab);
            },
            complete: this.$.unblockUI
        });
    }

    showFilterModal(html: string, targetTab: string): void {
        this.$filterModal.html(html);
        this.initFilterModal();
        this.$(targetTab).click();
        this.$filterModal.modal({ show: true, backdrop: false });
        this.isOpeningFilter = false;
    }

    initFilterModal(): void {
        this.$(".dropdown-stake-condition > select").multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
        });

        this.$("input[name='stake-filter-option']").change(() => {
            this.onSelectAllSportsChange();
        });

        this.marketMultiSelectionComponent.render(new MultiSelectionCheckboxOption("checkbox-all-markets", "list-market", "Market"));
        this.leagueMultiSelectionComponent.render(new MultiSelectionCheckboxOption("check-all-leagues", "list-league", "LeagueIds"));
        this.eventMultiSelectionComponent.render(new MultiSelectionCheckboxOption("checkbox-all-events", "list-event", "MatchIds"));
        this.betTypeMultiSelectionComponent.render(new MultiSelectionCheckboxOption("checkbox-all-bettypes", "list-bettype", "BetTypeGroupIds"));

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
                    this.$filterForm.html(html);
                    this.$filterModal.modal("hide");
                    this.registerEvents();
                },
                complete: this.$.unblockUI
            });
        });
    }

    onSelectAllSportsChange(): void {
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

    getLastBets(): void {
        if (this.isOpeningFilter) {
            return;
        }

        if (this.isCalling) {
            this.abortRequest();
        }

        this.$.blockUI();

        this.isCalling = true;
        let param = {
            sportGroup: this.sportGroup,
            isSelectAllSports: this.$("#IsSelectAllSports").val(),
            stakeMode: this.$("#StakeMode").val(),
            stakeLimit: this.$("#StakeLimit").val(),
            sportStakeConditions: this.$("#SportStakeConditions").val(),
            market: this.$("#Market").val(),
            leagueIds: this.$("#LeagueIds").val(),
            matchIds: this.$("#MatchIds").val(),
            betTypeGroupIds: this.$("#BetTypeGroupIds").val()
        };

        this.ajaxRequest = this.$.ajax({
            method: "GET",
            data: param,
            url: this.getBetsUrl,
            success: (result) => { this.successGetLastBets(result) },
            complete: this.$.unblockUI
        });
    }

    abortRequest(): void {
        if (this.ajaxRequest) {
            this.ajaxRequest.abort();
        }
    }

    successGetLastBets(responseHtml: string): void {
        if (!this.isCalling) {
            return;
        }

        this.$tblBetList.find("tbody").prepend(responseHtml);

        var numberOfRows = this.$tblBetList.find("tbody tr").length;

        if (numberOfRows > this.pageSize) {
            this.$tblBetList.find("tbody tr").slice(-(numberOfRows - this.pageSize)).remove();
        }

        this.isCalling = false;
    }
}

$(document).ready(() => {
    const wagerMonitoring = new WagerMonitoring(
        $,
        new SingleSelectionDropdownComponent($),
        new MultiSelectionCheckboxComponent($),
        new MultiSelectionCheckboxComponent($),
        new MultiSelectionCheckboxComponent($),
        new MultiSelectionCheckboxComponent($));
    wagerMonitoring.init();
})