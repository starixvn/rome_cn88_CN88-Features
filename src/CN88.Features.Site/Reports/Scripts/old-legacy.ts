"use strict";
class OldLegacyReport extends OldBaseReport {
    $tooltip: JQuery;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $excelIcon: JQuery;
    $tableReport: JQuery;
    $custLevelId: JQuery;
    $moreInfoIcon: JQuery;
    isMobile: boolean;

    sortComponent: SortComponent;
    linkBuilderComponent: LinkBuilderComponent;
    pagerComponent: ClientPagerComponent;

    constructor(
        $: JQueryStatic,
        accountingPeriodSelectionComponent: SingleSelectionDropdownComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        sortComponent: SortComponent,
        linkBuilderComponent: LinkBuilderComponent,
        pagerComponent: ClientPagerComponent) {
        super($, accountingPeriodSelectionComponent, dateRangePickerComponent, sportGroupSelectionComponent);

        this.sortComponent = sortComponent;
        this.linkBuilderComponent = linkBuilderComponent;
        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
        this.isMobile = cn88Features.convertAttributeToBool(this.$("#tbl-report").attr("data-isMobile"));
    }

    init(): void {
        this.$.blockUI();

        this.reCheckIn();
        this.cacheDOMElements();
        this.renderComponents();

        this.registerEvents();

        this.$subMenuLegacy.parent().addClass("active");

        this.$.unblockUI();
    }

    cacheDOMElements(): void {
        super.cacheDOMElements();
        this.$tooltip = this.$('[data-toggle="tooltip"]');
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$excelIcon = this.$("#icon-excel");
        this.$tableReport = this.$("#tbl-report");
        this.$custLevelId = this.$("#CustLevelId");
        this.$moreInfoIcon = this.$(".more-info");
    }

    renderComponents(): void {
        this.initTooltip();
        super.renderComponents();

        if (!this.isMobile) {
            const headers = this.$custLevelId.val() === "4"
                ? { 0: { sorter: "text" } }
                : { 0: { sorter: "text" }, 1: { sorter: "text" } };
            this.sortComponent.render(
                new SortOption(
                    this.$tableReport,
                    parseInt(this.$tableReport.data("totalrecords"), 0) > 1,
                    true,
                    this.sortComponent.afterSort,
                    headers,
                    this.$reportForm));

            this.linkBuilderComponent.render();
            this.pagerComponent.render(new ClientPagerOption(() => { this.onSelectPage(); }));
        }
    }

    initTooltip(): void {
        this.$tooltip.tooltip();
    }

    registerEvents(): void {
        this.$reportForm.submit(() => { this.onReportSubmit(); });
        this.$excelIcon.click(() => { this.onExportExcelEvent(); })

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
            this.$moreInfoIcon.removeClass("tooltip-show");
        });
    }

    onReportSubmit(): void {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.accountingPeriodSelectionComponent.updateSelectedValue();
    }

    onExportExcelEvent(): void {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };

    onSelectPage(): void {
        this.linkBuilderComponent.updateParam("PageSize", this.pagerComponent.pageSize.toString());
    }
}