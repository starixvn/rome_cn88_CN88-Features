class BetListOfReport {
    $: JQueryStatic;
    elements = {
        excelIcon: null,
        exportExcelForm: null
    };

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cacheDOMElements();
        this.registerEvents();
    };

    cacheDOMElements = () => {
        this.elements.excelIcon = this.$("#icon-excel");
        this.elements.exportExcelForm = this.$("#export-excel-form");
    };

    registerEvents = () => {
        this.elements.excelIcon.click(this.onExportExcelEvent);
    };

    onExportExcelEvent = () => {
        this.$.blockUI();
        this.elements.exportExcelForm.submit();
        this.$.unblockUI();
    };
}

$(document).ready(() => {
    const betListOfReport = new BetListOfReport($);
    betListOfReport.init();
})