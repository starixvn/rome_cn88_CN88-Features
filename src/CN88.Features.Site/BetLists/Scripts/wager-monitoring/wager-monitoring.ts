class WagerMonitoring {
    $: JQueryStatic;
    $tblBetList: JQuery;
    $filterForm: JQuery;
    $filterFormPath: JQuery;
    $wagerMonitoringPath: JQuery;
    ajaxRequest: JQueryXHR;

    filterModal: FilterModal;
    sportGroupSelectionComponent: SingleSelectionDropdownComponent;

    pageSize: number;
    sportGroup: number;
    getBetsUrl: string;
    intervalTime: number;
    isCalling: boolean = false;
    isOpeningFilter: boolean = false;

    constructor($: JQueryStatic) {
        this.$ = $;
        this.filterModal = new FilterModal($);
        this.sportGroupSelectionComponent = new SingleSelectionDropdownComponent($);
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
        this.$filterFormPath = this.$("#FilterFormPath");
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
            self.filterModal.show(link.data("target"), self.getParam(), self.updateFilterForm);
        });
    }

    getParam = () => {
        return new FilterParam(
            this.sportGroup,
            this.$("#IsSelectAllSports").val(),
            this.$("#StakeMode").val(),
            this.$("#StakeLimit").val(),
            this.$("#SportStakeConditions").val(),
            this.$("#Market").val(),
            this.$("#LeagueIds").val(),
            this.$("#MatchIds").val(),
            this.$("#BetTypeGroupIds").val()
        );
    }

    updateFilterForm(): void {
        this.$.blockUI();

        this.$.ajax({
            method: "GET",
            url: this.$filterFormPath.val(),
            cache: false,
            data: this.filterModal.getParameters(),
            success: (html) => {
                this.onSubmitSuccess(html);
            },
            complete: this.$.unblockUI
        });
    }

    onSubmitSuccess(html: string): void {
        this.$filterForm.html(html);
        this.registerEvents();

        this.filterModal.hide();
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
    const wagerMonitoring = new WagerMonitoring($);
    wagerMonitoring.init();
})