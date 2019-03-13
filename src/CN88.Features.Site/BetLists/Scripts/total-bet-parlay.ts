class TotalBetParlay {
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
        this.pagerComponent.render();
    };
}

$(document).ready(() => {
    const totalBetParlay = new TotalBetParlay($, new ClientPagerComponent($));
    totalBetParlay.init();
})