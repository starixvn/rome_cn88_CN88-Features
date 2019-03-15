class WagerByAccount {
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

        var self = this;
        this.$("#lnk-wager-details").click(function () {
            var url = self.$(this).attr("data-url");
            if (!url) {
                return;
            }

            window.location.href = url;
        });
    };
}

$(document).ready(() => {
    const betListByAccount = new WagerByAccount($, new ClientPagerComponent($));
    betListByAccount.init();
})