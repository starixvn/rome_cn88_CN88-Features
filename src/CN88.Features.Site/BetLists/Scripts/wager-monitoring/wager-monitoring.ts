/// <reference path="../../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../../shared/scripts/components/filter-toggle.ts" />


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
    numberOfRowPerTicket: number;

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

        this.pageSize = <number>this.$("#PageSize").val();
        this.pauseUrl = <string>this.$("#pause-url").val();
        this.getBetsUrl = <string>this.$("#get-bets-url").val();
        this.intervalTime = <number>this.$("#IntervalTime").val();
        this.numberOfRowPerTicket = this.isMobile ? <number>this.$("#NumberOfMobileRowsPerTicket").val() : 1;

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
            lblPerSports: <string>$("#lbl-PerSports").val(),
            lblStakeMode_0: <string>$("#lbl-StakeMode_0").val(),
            lblStakeMode_1: <string>$("#lbl-StakeMode_1").val(),
            lblStakeMode_2: <string>$("#lbl-StakeMode_2").val(),
            lblStakeMode_3: <string>$("#lbl-StakeMode_3").val(),
            lblStakeMode_4: <string>$("#lbl-StakeMode_4").val(),
            lblMarkets: <string>$("#lbl-Markets").val(),
            lblAllMarkets: <string>$("#lbl-AllMarkets").val(),
            lblLeague: <string>$("#lbl-League").val(),
            lblAllLeagues: <string>$("#lbl-AllLeagues").val(),
            lblEvent: <string>$("#lbl-Event").val(),
            lblAllEvents: <string>$("#lbl-AllEvents").val(),
            lblBetType: <string>$("#lbl-BetType").val(),
            lblAllBetTypes: <string>$("#lbl-AllBetTypes").val(),
            lblOn: <string>$("#lbl-On").val(),
            lblOff: <string>$("#lbl-Off").val()
        };
    }

    renderComponents(): void {
        if (this.isMobile) {
            this.$tblReport.stacktable();
            this.toggleHandler.render(new ToggleHandlerOption("filter-toggle-icon", "filter-form", "tbl-report-stack"));
            this.$("body").backtop();
            this.$tblReport = this.$("#tbl-report-stack");
        }
        this.sportGroupSelectionComponent.render(
            new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId", this.onSportGroupChange));
    }

    onSportGroupChange = () => {
        const sportGroup = this.sportGroupSelectionComponent.$dropdown.val();
        window.location.href = `${this.$wagerMonitoringPath.val()}?SportGroup=${sportGroup}`;
    }

    registerEvents(): void {
        const clickTouchedEvent = "click touchend";

        if (this.isMobile) {
            this.$filterForm.find(".link").parent().on(clickTouchedEvent, (event: JQueryEventObject) => {
                const link = this.$(event.currentTarget).find("a.link");
                this.openFilterModal(link.data("target"));
            });
        } else {
            this.$filterForm.find(".link").on(clickTouchedEvent, (event: JQueryEventObject) => {
                const link = this.$(event.currentTarget);
                this.openFilterModal(link.data("target"));
            });
        }

        this.$btnPause.on(clickTouchedEvent, this.onPauseClick);

        this.$btnClear.on(clickTouchedEvent, () => {
            this.$tblReport.find("tbody").empty();
        });

        this.$btnSpeaker.on(clickTouchedEvent, (event: JQueryEventObject) => {
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

    openFilterModal(targetTab: string): void {
        this.filterModal.show(targetTab, this.getParam(), this.updateFilterForm);
    }

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
            this.removeRows();

            if (this.isMobile) {
                this.bindShowPossEvent();
            }

            if (this.isSpeakOn) {
                this.playSound();
            }
        }

        this.isCalling = false;
    }

    removeRows(): void {
        const selector = this.isMobile ? "tbody tr.fixed-row" : "tbody tr";
        const numberOfRows = this.$tblReport.find(selector).length;

        if (numberOfRows > this.pageSize) {
            this.$tblReport.find("tbody tr").slice(-(numberOfRows - this.pageSize) * this.numberOfRowPerTicket).remove();
        }
    }

    bindShowPossEvent(): void {
        const $stackTableElement = this.$("#tbl-report-stack");

        $stackTableElement
            .find('.btn-collapse-expanse')
            .parent()
            .off('click.stack')
            .on('click.stack', function () {
                const btnIcon = $(this).find(".btn-collapse-expanse");
                const rowIndex = btnIcon.parent().attr('data-stack-row');
                const isCollapsed = btnIcon.hasClass('icon-add');
                const $nextRows = $stackTableElement.find('.stack-row-' + rowIndex);
                const $tr = btnIcon.parent().parent();
                const speedOfEffect = 200;

                if (isCollapsed) {
                    btnIcon.removeClass('icon-add')
                        .addClass('icon-minus');
                    $tr.removeClass('stack-collapsed').addClass('stack-expanded');
                    $nextRows.show(speedOfEffect);
                }
                else {
                    btnIcon.removeClass('icon-minus')
                        .addClass('icon-add');
                    $tr.removeClass('stack-expanded').addClass('stack-collapsed');
                    $nextRows.hide(speedOfEffect);
                }
            });
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