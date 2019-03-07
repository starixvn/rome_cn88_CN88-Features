let ViewResult: Function;
let showCombMPDetail: Function;
let showParlaySubBetsCN88: Function;
let showAllParlaySubBetsCN88: Function;

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
        // this.$.unblockUI();
    };
}


$(document).ready(() => {
    ViewResult = BetLists.Result.BaseResultDetails.Instance.ViewResultDetails;
    showCombMPDetail = BetList.Details.ParlayTicketDetails.Instance.ShowComboParlayDetails;
    showParlaySubBetsCN88 = BetList.SubTickets.ParlaySubTickets.Instance.ShowSubBetsCN88;
    showAllParlaySubBetsCN88 = BetList.SubTickets.ParlaySubTickets.Instance.ShowAllParlaySubBetsCN88;

    const betListOfReport = new BetListOfReport($);
    betListOfReport.init();
})