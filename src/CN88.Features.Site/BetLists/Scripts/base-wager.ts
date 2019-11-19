/// <reference path="../../shared/scripts/components/client-pager.ts" />

class BaseWager {
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

    public init(data: JQuery) {
        this.isMobile = this.$("#isMobile").val() === "True";

        if (this.isMobile) {
            this.$("#tbl-report").stacktable();
            this.$('body').backtop();
        }
        else {
            this.pagerComponent.render();
        }

        const dataUrl = data.attr("data-url");
        data.click(function () {
            if (!dataUrl) {
                return;
            }

            window.location.href = dataUrl;
        });
    }
}
