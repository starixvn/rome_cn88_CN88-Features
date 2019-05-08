class WagerMonitoring {
    $: JQueryStatic;
    $tblBetList: JQuery;
    $filterForm: JQuery;
    $wagerMonitoringPath: JQuery;

    $stakeLnk: JQuery;
    $maketLnk: JQuery;
    $leagueLnk: JQuery;
    $eventLnk: JQuery;
    $betTypeLnk: JQuery;

    $sportGroup: JQuery;
    $isSelectAllSports: JQuery;
    $stakeMode: JQuery;
    $stakeLimit: JQuery;
    $market: JQuery;
    $leagueIds: JQuery;
    $matchIds: JQuery;
    $betTypeGroupIds: JQuery;
    $sportStakeConditions: JQuery;
    $minTransId: JQuery;

    $iconPause: JQuery;
    $iconDelete: JQuery;
    $iconSpeaker: JQuery;
    $iconSpeakerText: JQuery;
    $sound: any;

    ajaxRequest: JQueryXHR;
    filterModal: FilterModal;
    sportGroupSelectionComponent: SingleSelectionDropdownComponent;

    pageSize: number;
    pauseUrl: string;
    getBetsUrl: string;
    intervalTime: number;
    isCalling: boolean = false;
    isSpeakOn: boolean = true;
    speakerOnText: string;
    speakerOffText: string;

    resources = {
        lblPerSports: '',
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
        lblAllBetTypes: '',
        lblOn: '',
        lblOff: ''
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
        this.filterModal.render(() => { this.updateFilterForm(); })

        this.pageSize = this.$("#PageSize").val();
        this.pauseUrl = this.$("#pause-url").val();
        this.getBetsUrl = this.$("#get-bets-url").val();
        this.intervalTime = this.$("#IntervalTime").val();

        setInterval(() => { this.getLastBets() }, this.intervalTime * 1000);

        this.$.unblockUI();
    }

    cacheElements(): void {
        this.$tblBetList = this.$("#tbl-betlist");
        this.$filterForm = this.$("#filter-form");
        this.$wagerMonitoringPath = this.$("#WagerMonitoringPath");

        this.$stakeLnk = $(".link[data-target='#stake-tab'");
        this.$maketLnk = $(".link[data-target='#market-tab'");
        this.$leagueLnk = $(".link[data-target='#league-tab'");
        this.$eventLnk = $(".link[data-target='#event-tab'");
        this.$betTypeLnk = $(".link[data-target='#bettype-tab'");

        this.$sportGroup = this.$("#SportGroup");
        this.$isSelectAllSports = this.$("#IsSelectAllSports");
        this.$stakeMode = this.$("#StakeMode");
        this.$stakeLimit = this.$("#StakeLimit");
        this.$market = this.$("#Market");
        this.$leagueIds = this.$("#LeagueIds");
        this.$matchIds = this.$("#MatchIds");
        this.$betTypeGroupIds = this.$("#BetTypeGroupIds");
        this.$sportStakeConditions = this.$("#SportStakeConditions");
        this.$minTransId = this.$("#MinTransId");

        this.$iconPause = this.$(".icon-pause");
        this.$iconDelete = this.$(".icon-delete");
        this.$iconSpeaker = this.$(".icon-audio-speaker");
        this.$iconSpeakerText = this.$("#icon-audio-text");
        this.$sound = document.getElementById("sound");

        this.resources = {
            lblPerSports: $("#lbl-PerSports").val(),
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
            lblAllBetTypes: $("#lbl-AllBetTypes").val(),
            lblOn: $("#lbl-On").val(),
            lblOff: $("#lbl-Off").val()
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

        this.$filterForm.find(".link").on("click touchend", function () {
            let link = $(this);
            self.filterModal.show(link.data("target"));
        });

        this.$iconPause.parent().on("click touchend", () => {
            this.onPauseClick();
        });

        this.$iconDelete.parent().on("click touchend", () => {
            this.$tblBetList.find("tbody").empty();
        });

        this.$iconSpeaker.parent().on("click touchend", () => {
            this.isSpeakOn = !this.isSpeakOn;

            if (this.isSpeakOn) {
                this.$iconSpeaker.removeClass("icon-audio-speaker-off").addClass("icon-audio-speaker-on");
                this.$iconSpeakerText.text(this.resources.lblOn);
            } else {
                this.$iconSpeaker.removeClass("icon-audio-speaker-on").addClass("icon-audio-speaker-off");
                this.$iconSpeakerText.text(this.resources.lblOff);
            }

            this.$sound.play();
        });
    }

    onPauseClick(): void {
        this.$.ajax({
            url: this.pauseUrl,
            success: (html) => {
                this.onPauseClickSuccess(html);
            },
            complete: this.$.unblockUI
        });
    }

    onPauseClickSuccess(html: string): void {
        let result = html.replace('<table></table>', this.$tblBetList.prop("outerHTML"));
        let w = window.open("", "Pause", "height=800,width=1280");
        w.document.open();
        w.document.write(result);
        w.document.close();
    }

    getParam = () => {
        return new FilterParam(
            this.$sportGroup.val(),
            this.$isSelectAllSports.val(),
            this.$stakeMode.val(),
            this.$stakeLimit.val(),
            this.$sportStakeConditions.val(),
            this.$market.val(),
            this.$leagueIds.val(),
            this.$matchIds.val(),
            this.$betTypeGroupIds.val()
        );
    }

    updateFilterForm(): void {
        this.$.blockUI();

        var param = this.filterModal.getParameters();

        this.$stakeLnk.html(param.isSelectAllSports ? `${this.resources["lblStakeMode_" + param.stakeMode]} ${param.stakeLimit}` : this.resources.lblPerSports);
        this.$maketLnk.html(param.markets.length ? param.markets : this.resources.lblAllMarkets);
        this.$leagueLnk.html(param.leagueIds.length ? `${this.resources.lblLeague} (${param.leagueIds.length})` : this.resources.lblAllLeagues);
        this.$eventLnk.html(param.matchIds.length ? `${this.resources.lblEvent} (${param.matchIds.length})` : this.resources.lblAllEvents);
        this.$betTypeLnk.html(param.betTypeGroupIds.length ? `${this.resources.lblBetType} (${param.betTypeGroupIds.length})` : this.resources.lblAllBetTypes);

        this.$isSelectAllSports.val(param.isSelectAllSports.toString());
        this.$stakeMode.val(param.stakeMode);
        this.$stakeLimit.val(param.stakeLimit);
        this.$market.val(param.marketIds);
        this.$leagueIds.val(param.leagueIds);
        this.$matchIds.val(param.matchIds);
        this.$betTypeGroupIds.val(param.betTypeGroupIds);
        this.$sportStakeConditions.val(param.sportStakeConditions);

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
            sportGroup: this.$sportGroup.val(),
            isSelectAllSports: this.$("#IsSelectAllSports").val(),
            stakeMode: this.$("#StakeMode").val(),
            stakeLimit: this.$("#StakeLimit").val(),
            sportStakeConditions: this.$("#SportStakeConditions").val(),
            market: this.$("#Market").val(),
            leagueIds: this.$("#LeagueIds").val(),
            matchIds: this.$("#MatchIds").val(),
            betTypeGroupIds: this.$("#BetTypeGroupIds").val(),
            minTransId: this.$minTransId.val()
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

        let $responseTable = this.$(responseHtml);
        let totalRecords = $responseTable.data("totalrecords");
        let minTransId = $responseTable.data("mintransid");
        this.$minTransId.val(minTransId);

        if (totalRecords > 0) {
            this.$tblBetList.find("tbody").prepend($responseTable.find("tbody").html());

            var numberOfRows = this.$tblBetList.find("tbody tr").length;

            if (numberOfRows > this.pageSize) {
                this.$tblBetList.find("tbody tr").slice(-(numberOfRows - this.pageSize)).remove();
            }

            if (this.isSpeakOn) {
                this.$sound.play();
            }
        }

        this.isCalling = false;
    }
}

$(document).ready(() => {
    const wagerMonitoring = new WagerMonitoring($);
    wagerMonitoring.init();
})