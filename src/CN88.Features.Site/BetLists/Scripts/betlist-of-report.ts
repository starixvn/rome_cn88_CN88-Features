/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/link-builder.ts" />
/// <reference path="../../shared/scripts/components/client-pager.ts" />
/// <reference path="../../shared/scripts/components/filter-toggle.ts" />

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
        reportPager: null,
        iconBackTop: null,
        body: null
    };

    constructor(options: BetListReportOptions) {
        this.$ = options.$;

        this.dateRangePickerComponent = options.dateRangePickerComponent;
        this.sportGroupSelectionComponent = options.sportGroupSelectionComponent;
        this.betTypeSelectionComponent = options.betTypeSelectionComponent;
        this.liveStatusSelectionComponent = options.liveStatusSelectionComponent;
        this.toggleHandler = options.toggleHandler;

        this.linkBuilderComponent = options.linkBuilderComponent;
        this.pagerComponent = options.pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
    }

    init = () => {
        this.$.blockUI();

        this.cacheDOMElements();
        this.isMobile = this.elements.reportTable.data("ismobile");
        if (this.isMobile) {
            this.elements.reportTable.stacktable();
            this.toggleHandler.render(new ToggleHandlerOption("filter-toggle-icon", "report-form", "tbl-report-stack"));
            if (this.elements.iconBackTop.length === 0) {
                this.elements.body.backtop();
            }
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
        this.elements.iconBackTop = this.$("#back-top-component");
        this.elements.body = this.$("body");
    };

    registerEvents = () => {
        this.elements.reportForm.submit(this.onReportSubmit);

        this.elements.excelIcon.click(this.onExportExcelEvent);

        const expiredSessionSeconds: number = <number>this.$('#expiredSessionSeconds').val();
        if (!isNaN(expiredSessionSeconds)) {
            this.checkIn();
            const oneThousand = 1000;
            setInterval(this.checkIn, expiredSessionSeconds * oneThousand);
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
        const sportType = this.sportGroupSelectionComponent.$dropdown.val();
        this.betTypeSelectionComponent.$dropdown.empty();
        const betTypes = window["betTypes"];
        const first = betTypes[0];
        this.betTypeSelectionComponent.$dropdown.append(`<option value="${first[0].Value}">${first[0].Text}</option>`);

        for (const betTypeGroups of betTypes) {
            if (betTypeGroups[0].SportTypeGroup.toString() === sportType) {
                for (const betTypeGroup of betTypeGroups) {
                    this.betTypeSelectionComponent.$dropdown.append(`<option value="${betTypeGroup.Value}">${betTypeGroup.Text}</option>`);
                }

                break;
            }
        }

        this.betTypeSelectionComponent.$dropdown.multiselect("destroy");
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

class BetListReportOptions
{
    constructor(
        public $: JQueryStatic,
        public dateRangePickerComponent: DateRangePickerComponent,
        public sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        public betTypeSelectionComponent: SingleSelectionDropdownComponent,
        public liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        public linkBuilderComponent: LinkBuilderComponent,
        public pagerComponent: ClientPagerComponent,
        public toggleHandler: ToggleHandler
    ) {}
}

$(document).ready(() => {
    const betListOfReport = new BetListOfReport(new BetListReportOptions(
        $,
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new LinkBuilderComponent($),
        new ClientPagerComponent($),
        new ToggleHandler($)));

    betListOfReport.init();
});