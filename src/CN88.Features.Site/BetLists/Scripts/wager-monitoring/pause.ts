class Pause {
    $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        const $tblReport = this.$("#tbl-report");
        const isMobile = $tblReport.data("ismobile");

        if (isMobile) {
            $tblReport.stacktable();
            this.$('body').backtop();
        }
    }
}

$(document).ready(() => {
    const pause = new Pause($);
    pause.init();
});