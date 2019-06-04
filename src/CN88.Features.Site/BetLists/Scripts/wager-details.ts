/// <reference path="../../shared/scripts/components/client-pager.ts" />

class WagerDetailsBetList {
    $: JQueryStatic;
    pagerComponent: ClientPagerComponent;
    isMobile: boolean;

    constructor(
        $: JQueryStatic,
        pagerComponent: ClientPagerComponent) {
        this.$ = $;
        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
    }

    init = () => {
        this.isMobile = this.$("#isMobile").val() === "True";

        if (this.isMobile) {
            this.$("#tbl-report").stacktable();
            this.$('body').backtop();
        }
        else {
            this.pagerComponent.render();
        }

        const betListUrl = this.$("#lnk-bet-list-by-account").attr("data-url");
        this.$("#lnk-bet-list-by-account").click(function () {
            if (!betListUrl) {
                return;
            }

            window.location.href = betListUrl;
        });
    };
}

$(document).ready(() => {
    const wagerDetails = new WagerDetailsBetList($, new ClientPagerComponent($));
    wagerDetails.init();
});