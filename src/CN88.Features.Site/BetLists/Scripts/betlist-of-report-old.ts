class BetListOfReportOld {
    $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        var mode = this.$("#DisplayMode").val();

        if (mode === "1") {
            this.$("#sub-menu-wager").parent().addClass("active");
            return;
        }

        this.$("#sub-menu-legacy").parent().addClass("active");
    };
}

$(document).ready(() => {
    const betListOfReportOld = new BetListOfReportOld($);
    betListOfReportOld.init();
})