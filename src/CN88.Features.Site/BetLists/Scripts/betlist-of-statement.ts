class BetListOfStatement {
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
    };

    registerEvents = () => {
    };
}

$(document).ready(() => {
    const betListOfStatement = new BetListOfStatement($);
    betListOfStatement.init();
})