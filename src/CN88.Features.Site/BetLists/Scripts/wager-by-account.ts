/// <reference path="../../shared/scripts/components/client-pager.ts" />

class WagerByAccount {
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

        const dataUrl = this.$("#lnk-wager-details").attr("data-url");
        this.$("#lnk-wager-details").click(function () {
            if (!dataUrl) {
                return;
            }

            window.location.href = dataUrl;
        });
    };
}

$(document).ready(() => {
    const betListByAccount = new WagerByAccount($, new ClientPagerComponent($));
    betListByAccount.init();
});