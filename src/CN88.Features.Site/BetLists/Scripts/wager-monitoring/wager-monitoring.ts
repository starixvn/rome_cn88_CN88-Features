class WagerMonitoring {
    $: JQueryStatic;
    $tblReport: JQuery;
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

    $btnPause: JQuery;
    $btnClear: JQuery;
    $btnSpeaker: JQuery;
    $iconSpeaker: JQuery;
    $iconSpeakerText: JQuery;
    $sound: JQuery;

    ajaxRequest: JQueryXHR;
    filterModal: FilterModal;
    sportGroupSelectionComponent: SingleSelectionDropdownComponent;
    toggleHandler: ToggleHandler;

    pageSize: number;
    pauseUrl: string;
    getBetsUrl: string;
    intervalTime: number;
    isCalling = false;
    isSpeakOn = true;
    speakerOnText: string;
    speakerOffText: string;
    isMobile: boolean;

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

    constructor(
        $: JQueryStatic,
        filterModal: FilterModal,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        toggleHandler: ToggleHandler) {
        this.$ = $;
        this.filterModal = filterModal;
        this.sportGroupSelectionComponent = sportGroupSelectionComponent;
        this.toggleHandler = toggleHandler;
    }

    init(): void {
        this.$.blockUI();

        this.cacheElements();
        this.renderComponents();
        this.registerEvents();

        this.pageSize = this.$("#PageSize").val();
        this.pauseUrl = this.$("#pause-url").val();
        this.getBetsUrl = this.$("#get-bets-url").val();
        this.intervalTime = this.$("#IntervalTime").val();

        setInterval(this.getLastBets, this.intervalTime * 1000);

        this.$.unblockUI();
    }

    cacheElements(): void {
        this.$tblReport = this.$("#tbl-report");
        this.isMobile = this.$tblReport.data("ismobile");

        this.$filterForm = this.$("#filter-form");
        this.$wagerMonitoringPath = this.$("#WagerMonitoringPath");

        this.$stakeLnk = $(".link[data-target='#stake-tab']");
        this.$maketLnk = $(".link[data-target='#market-tab']");
        this.$leagueLnk = $(".link[data-target='#league-tab']");
        this.$eventLnk = $(".link[data-target='#event-tab']");
        this.$betTypeLnk = $(".link[data-target='#bettype-tab']");

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

        this.$btnPause = this.$("#btnPause");
        this.$btnClear = this.$("#btnClear");
        this.$btnSpeaker = this.$("#btnSpeaker");
        this.$iconSpeaker = this.$(".icon-audio-speaker");
        this.$iconSpeakerText = this.$("#icon-audio-text");
        this.$sound = this.$("#sound");

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
        if (this.isMobile) {
            this.$tblReport.stacktable();
            this.toggleHandler.render(new ToggleHandlerOption("filter-toggle-icon", "filter-form", "tbl-report-stack"));
            this.$('body').backtop();
        }
        this.sportGroupSelectionComponent.render(
            new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId", this.onSportGroupChange));
    }

    onSportGroupChange = () => {
        const sportGroup = this.sportGroupSelectionComponent.$dropdown.val();
        window.location.href = `${this.$wagerMonitoringPath.val()}?SportGroup=${sportGroup}`;
    }

    registerEvents(): void {
        if (this.isMobile) {
            this.$filterForm.find(".link").parent().on("click touchend", this.openFilterModal);
        } else {
            this.$filterForm.find(".link").on("click touchend", this.openFilterModal);
        }

        this.$btnPause.on("click touchend", this.onPauseClick);

        this.$btnClear.on("click touchend", () => {
            this.$tblReport.find("tbody").empty();
        });

        this.$btnSpeaker.on("click touchend", (event: JQueryEventObject) => {
            if (event.type === "touchend") {
                this.$btnSpeaker.off("click");
            }

            this.isSpeakOn = !this.isSpeakOn;

            if (this.isSpeakOn) {
                this.$iconSpeaker.removeClass("icon-audio-speaker-off").addClass("icon-audio-speaker-on");
                this.$iconSpeakerText.text(this.resources.lblOn);
                this.$btnSpeaker.removeClass("off").addClass("on");
            } else {
                this.$iconSpeaker.removeClass("icon-audio-speaker-on").addClass("icon-audio-speaker-off");
                this.$iconSpeakerText.text(this.resources.lblOff);
                this.$btnSpeaker.removeClass("on").addClass("off");
            }

            this.playSound();
        });
    }

    openFilterModal = (e: JQueryEventObject) => {
        const link = this.$(e.target);
        this.filterModal.show(link.data("target"), this.getParam(), this.updateFilterForm);
    };

    onPauseClick = () => {
        this.$.ajax({
            url: this.pauseUrl,
            success: (html) => {
                this.onPauseClickSuccess(html);
            },
            complete: this.$.unblockUI
        });
    };

    onPauseClickSuccess(html: string): void {
        const result = html.replace('<table></table>', this.$tblReport.prop("outerHTML"));
        const features = this.isMobile ? "" : "height=800,width=1280";
        const pauseWindow = window.open("", "Pause", features);
        pauseWindow.document.open();
        pauseWindow.document.write(result);
        pauseWindow.document.close();
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
    };

    updateFilterForm = () => {
        this.$.blockUI();

        const param = this.filterModal.getParameters();

        const stakeMode = this.resources[`lblStakeMode_${param.stakeMode}`];
        this.$stakeLnk.html(param.isSelectAllSports ? `${stakeMode} ${param.stakeLimit}` : this.resources.lblPerSports);
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
    };

    getLastBets = () => {
        if (this.filterModal.isOpeningFilter) {
            return;
        }

        if (this.isCalling) {
            this.abortRequest();
        }

        this.$.blockUI();

        this.isCalling = true;
        const param = {
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
            method: "POST",
            data: param,
            url: this.getBetsUrl,
            success: (result) => { this.successGetLastBets(result); },
            complete: this.$.unblockUI
        });
    };

    abortRequest(): void {
        if (this.ajaxRequest) {
            this.ajaxRequest.abort();
        }
    }

    successGetLastBets(responseHtml: string): void {
        if (!this.isCalling) {
            return;
        }

        const $responseTable = this.$(responseHtml);
        const totalRecords = $responseTable.data("totalrecords");
        const minTransId = $responseTable.data("mintransid");
        this.$minTransId.val(minTransId);

        if (totalRecords > 0) {
            this.$tblReport.find("tbody").prepend($responseTable.find("tbody").html());

            const numberOfRows = this.$tblReport.find("tbody tr").length;

            if (numberOfRows > this.pageSize) {
                this.$tblReport.find("tbody tr").slice(-(numberOfRows - this.pageSize)).remove();
            }

            if (this.isSpeakOn) {
                this.playSound();
            }
        }

        this.isCalling = false;
    }

    playSound(): void {
        this.$sound.load();
        this.$sound.trigger("play");
    }
}

$(document).ready(() => {
    const wagerMonitoring = new WagerMonitoring(
        $,
        new FilterModal($),
        new SingleSelectionDropdownComponent($),
        new ToggleHandler($));

    wagerMonitoring.init();
});