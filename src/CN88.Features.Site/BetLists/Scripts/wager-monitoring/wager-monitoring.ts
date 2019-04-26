class WagerMonitoring {
    $: JQueryStatic;
    $tblBetList: JQuery;
    $filterForm: JQuery;
    $filterFormPath: JQuery;
    $wagerMonitoringPath: JQuery;

    $stakeLnk: JQuery;
    $maketLnk: JQuery;
    $leagueLnk: JQuery;
    $eventLnk: JQuery;
    $betTypeLnk: JQuery;

    ajaxRequest: JQueryXHR;

    filterModal: FilterModal;
    sportGroupSelectionComponent: SingleSelectionDropdownComponent;

    pageSize: number;
    sportGroup: number;
    getBetsUrl: string;
    intervalTime: number;
    isCalling: boolean = false;
    
    resources = {
        lblStakeMode_0: '',
        lblStakeMode_1: '',
        lblStakeMode_2: '',
        lblStakeMode_3: '',
        lblStakeMode_4: '',
        lblMarkets: '',
        lblAllMarkets: '',
        lblLeague: '',
        lblAllLeagues: '',
        lblEvent: '',
        lblAllEvents: '',
        lblBetType: '',
        lblAllBetTypes: ''
    };

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

        this.$stakeLnk = $(".link[data-target='#stake-tab'");
        this.$maketLnk = $(".link[data-target='#market-tab'");
        this.$leagueLnk = $(".link[data-target='#league-tab'");
        this.$eventLnk = $(".link[data-target='#event-tab'");
        this.$betTypeLnk = $(".link[data-target='#bettype-tab'");

        this.resources = {
            lblStakeMode_0: $("#lbl-StakeMode_0").val(),
            lblStakeMode_1: $("#lbl-StakeMode_1").val(),
            lblStakeMode_2: $("#lbl-StakeMode_2").val(),
            lblStakeMode_3: $("#lbl-StakeMode_3").val(),
            lblStakeMode_4: $("#lbl-StakeMode_4").val(),
            lblMarkets: $("#lbl-Markets").val(),
            lblAllMarkets: $("#lbl-AllMarkets").val(),
            lblLeague: $("#lbl-League").val(),
            lblAllLeagues: $("#lbl-AllLeagues").val(),
            lblEvent: $("#lbl-Event").val(),
            lblAllEvents: $("#lbl-AllEvents").val(),
            lblBetType: $("#lbl-BetType").val(),
            lblAllBetTypes: $("#lbl-AllBetTypes").val()
        };
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
            self.filterModal.show(link.data("target"), self.getParam(), () => { self.updateFilterForm(); });
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

        var param = this.filterModal.getParameters();

        this.$stakeLnk.html(param.isSelectAllSports ? `${this.resources["lblStakeMode_" + param.stakeMode]} ${param.stakeLimit}` : "");
        this.$maketLnk.html(param.markets.length ? param.markets : this.resources.lblAllMarkets);
        this.$leagueLnk.html(param.leagueIds.length ? `${this.resources.lblLeague} (${param.leagueIds.length})` : this.resources.lblAllLeagues);
        this.$eventLnk.html(param.matchIds.length ? `${this.resources.lblEvent} (${param.matchIds.length})` : this.resources.lblAllEvents);
        this.$betTypeLnk.html(param.betTypeGroupIds.length ? `${this.resources.lblBetType} (${param.betTypeGroupIds.length})` : this.resources.lblAllBetTypes);

        this.filterModal.hide();
        this.$.unblockUI();
    }

    getLastBets(): void {
        if (this.filterModal.isOpeningFilter) {
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