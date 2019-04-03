class BetListOfStatement {
    $: JQueryStatic;
    pagerComponent: ClientPagerComponent;

    constructor(
        $: JQueryStatic,
        pagerComponent: ClientPagerComponent) {
        this.$ = $;
        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
    }

    init = () => {
        this.registerEvents();
        this.pagerComponent.render();
    };

    registerEvents = () => {
        this.$(".btn-back").off("click").on("click", function () {
            window.location.href = document.referrer;
        });
    };
}

$(document).ready(() => {

    const betListOfStatement = new BetListOfStatement($, new ClientPagerComponent($));
    betListOfStatement.init();
})