/// <reference path="base-report.ts" />
/// <reference path="../../shared/scripts/components/sort.ts" />
/// <reference path="../../shared/scripts/components/link-builder.ts" />
/// <reference path="../../shared/scripts/components/client-pager.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />

class LegacyReport extends BaseReport {
    $tooltip: JQuery;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $excelIcon: JQuery;
    $tableReport: JQuery;
    isMobile: boolean;
    $custLevelId: JQuery;
    $moreInfoIcon: JQuery;

    sortComponent: SortComponent;
    linkBuilderComponent: LinkBuilderComponent;
    pagerComponent: ClientPagerComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        sortComponent: SortComponent,
        linkBuilderComponent: LinkBuilderComponent,
        pagerComponent: ClientPagerComponent) {
        super($, dateRangePickerComponent, sportGroupSelectionComponent, betTypeSelectionComponent, liveStatusSelectionComponent);

        this.sortComponent = sortComponent;
        this.linkBuilderComponent = linkBuilderComponent;
        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
        this.isMobile = cn88Features.convertAttributeToBool(this.$("#tbl-report").attr("data-isMobile"));
    }

    init = () => {
        this.$.blockUI();

        this.cacheDOMElements();
        this.renderComponents();
        this.registerEvents(); 

        this.$.unblockUI();
    };

    renderComponents(): void {
        this.initTooltip();
        super.renderComponents();

        const headers = this.$custLevelId.val() === "4"
            ? { 0: { sorter: "text" } }
            : { 0: { sorter: "text" }, 1: { sorter: "text" } };
        if (!this.isMobile) {
            this.sortComponent.render(
                new SortOption(
                    this.$tableReport,
                    parseInt(this.$tableReport.data("totalrecords"), 0) > 1,
                    true,
                    this.sortComponent.afterSort,
                    headers,
                    this.$reportForm));
        }

        this.linkBuilderComponent.render();
        this.pagerComponent.render(new ClientPagerOption(() => { this.onSelectPage(); }));
    }

    initTooltip = () => {
        this.$tooltip.tooltip();
    }

    public cacheDOMElements() {
        this.$tooltip = this.$('[data-toggle="tooltip"]');
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$excelIcon = this.$("#icon-excel");
        this.$tableReport = this.$("#tbl-report,#tbl-report-stack");
        this.$custLevelId = this.$("#CustLevelId");
        this.$moreInfoIcon = this.$(".more-info");
    }

    public registerEvents() {
        this.$reportForm.submit(this.onReportSubmit);
        this.$excelIcon.click(this.onExportExcelEvent);

        this.$("body").on("click touchend", (event) => {
            const tooltip = this.$(event.target);
            const showTooltipClass = "tooltip-show";
            if (tooltip.hasClass("more-info")) {
                const isShowing = tooltip.hasClass(showTooltipClass);

                const showingToolTips = this.$('.tooltip-show[data-toggle="tooltip"]');
                showingToolTips.tooltip("hide");
                showingToolTips.removeClass(showTooltipClass);

                if (isShowing) {
                    tooltip.tooltip("hide");
                    tooltip.removeClass(showTooltipClass);
                } else {
                    tooltip.tooltip("show");
                    tooltip.addClass(showTooltipClass);
                }

                event.stopPropagation();
                return false;
            }

            this.$moreInfoIcon.tooltip("hide");
            this.$moreInfoIcon.removeClass(showTooltipClass);
        });
    }

    onSelectPage(): void {
        this.linkBuilderComponent.updateParam("PageSize", this.pagerComponent.pageSize.toString());
    }

    onReportSubmit = () => {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
    };

    onExportExcelEvent = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };
}