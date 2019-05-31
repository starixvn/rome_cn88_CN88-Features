class BetListOfStatement {
    $: JQueryStatic;
    pagerComponent: ClientPagerComponent;
    isMobile: boolean;
    reportTable: JQuery;

    constructor(
        $: JQueryStatic,
        pagerComponent: ClientPagerComponent) {
        this.$ = $;
        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
    }

    init = () => {
        this.reportTable = this.$("#tbl-report");
        this.isMobile = this.reportTable.data("ismobile");
        this.registerEvents();
    };

    registerEvents = () => {
        if (this.isMobile) {
            this.reportTable.stacktable();
        }
        else {
            this.pagerComponent.render();
        }

        this.$(".btn-back").off("click").on("click", function () {
            window.location.href = document.referrer;
        });
    };
}

$(document).ready(() => {
    const betListOfStatement = new BetListOfStatement($, new ClientPagerComponent($));
    betListOfStatement.init();
});