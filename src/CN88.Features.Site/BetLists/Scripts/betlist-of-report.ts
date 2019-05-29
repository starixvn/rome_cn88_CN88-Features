/// <reference path="../../shared/scripts/core.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/link-builder.ts" />
/// <reference path="../../shared/scripts/components/filter-toggle.ts" />
/// <reference path="../../shared/scripts/components/client-pager.ts" />

class BetListOfReport {
    $: JQueryStatic;

    dateRangePickerComponent: DateRangePickerComponent;
    sportGroupSelectionComponent: SingleSelectionDropdownComponent;
    betTypeSelectionComponent: SingleSelectionDropdownComponent;
    liveStatusSelectionComponent: SingleSelectionDropdownComponent;
    linkBuilderComponent: LinkBuilderComponent;
    pagerComponent: ClientPagerComponent;
    toggleHandler: ToggleHandler;
    isMobile: boolean;

    elements = {
        excelIcon: null,
        exportExcelForm: null,
        reportForm: null,
        pageIndex: null,
        filterToggleIcon: null,
        reportTable: null,
        reportPager: null
    };

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        linkBuilderComponent: LinkBuilderComponent,
        pagerComponent: ClientPagerComponent,
        toggleHandler: ToggleHandler) {
        this.$ = $;

        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sportGroupSelectionComponent = sportGroupSelectionComponent;
        this.betTypeSelectionComponent = betTypeSelectionComponent;
        this.liveStatusSelectionComponent = liveStatusSelectionComponent;
        this.toggleHandler = toggleHandler;

        this.linkBuilderComponent = linkBuilderComponent;
        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
    }

    init = () => {
        this.$.blockUI();

        this.cacheDOMElements();
        this.isMobile = this.elements.reportTable.data("ismobile");
        if (this.isMobile) {
            this.elements.reportTable.stacktable();
            this.toggleHandler.render(new ToggleHandlerOption("filter-toggle-icon", "report-form", "tbl-report-stack"));
            this.$('body').backtop();
        }
        else {
            this.pagerComponent.render();
            this.elements.pageIndex = this.$("#PageIndex");
        }

        this.dateRangePickerComponent.render();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId", this.onchangeSportType));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
        this.linkBuilderComponent.render();

        this.registerEvents();

        this.$.unblockUI();
    };

    cacheDOMElements = () => {
        this.elements.excelIcon = this.$("#icon-excel");
        this.elements.exportExcelForm = this.$("#export-excel-form");
        this.elements.reportForm = this.$("#report-form");
        this.elements.filterToggleIcon = this.$("#filter-toggle-icon");
        this.elements.reportTable = this.$("#tbl-report");
        this.elements.reportPager = this.$("#report-pager");
    };

    registerEvents = () => {
        let self = this;

        this.elements.reportForm.submit(this.onReportSubmit);

        this.elements.excelIcon.click(this.onExportExcelEvent);

        let expiredSessionSeconds: number = this.$('#expiredSessionSeconds').val();
        if (!isNaN(expiredSessionSeconds)) {
            this.checkIn();

            let $this = this;

            setInterval(function () {
                $this.checkIn();
            }, expiredSessionSeconds * 1000);
        }
    };

    onReportSubmit = () => {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
        if (!this.isMobile) {
            this.elements.pageIndex.val(1);
        }
    };

    onExportExcelEvent = () => {
        this.$.blockUI();
        this.elements.exportExcelForm.submit();
        this.$.unblockUI();
    };

    onchangeSportType = () => {
        let sportType = this.sportGroupSelectionComponent.$dropdown.val();
        this.betTypeSelectionComponent.$dropdown.empty();
        let first = betTypes[0];
        this.betTypeSelectionComponent.$dropdown.append('<option value=' + first[0].Value + '>' + first[0].Text + '</option>');
        for (var i = 0; i < betTypes.length; i++) {
            var betTypeGroup = betTypes[i];

            if (betTypeGroup[0].SportTypeGroup == sportType) {
                for (var j = 0; j < betTypeGroup.length; j++) {
                    this.betTypeSelectionComponent.$dropdown.append('<option value=' + betTypeGroup[j].Value + '>' + betTypeGroup[j].Text + '</option>');
                }

                break;
            }
        }

        this.betTypeSelectionComponent.$dropdown.multiselect('destroy')
        this.betTypeSelectionComponent.$dropdown.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
        });
    };

    checkIn = () => {
        this.$.ajax({ url: '../OldReports/Checkin', type: 'POST' });
    }
}

$(document).ready(() => {
    const betListOfReport = new BetListOfReport(
        $,
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new LinkBuilderComponent($),
        new ClientPagerComponent($),
        new ToggleHandler($));
    betListOfReport.init();
})
