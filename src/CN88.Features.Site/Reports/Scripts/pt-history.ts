class PTHistory {
    $: JQueryStatic;
    $ptHistoryUrl: JQuery;
    $FromDate: JQuery;
    $ToDate: JQuery;
    $ptHistory: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cacheDOMElements();
        this.registerEvents();
    }

    public cacheDOMElements() {
        this.$ptHistoryUrl = this.$("#PTHistoryUrl");
        this.$FromDate = this.$("#FromDate");
        this.$ToDate = this.$("#ToDate");
        this.$ptHistory = this.$("#pt-history");
    }

    public registerEvents() {
        this.$(".icon-history").click(this.loadPTHistory);
    }

    loadPTHistory = (e: JQueryEventObject) => {
        this.$.blockUI();

        const icon = this.$(e.target);
        const custId = icon.data("custid");
        const fromDate = this.$FromDate.val();
        const toDate = this.$ToDate.val();

        this.$.ajax({
            method: "GET",
            url: this.$ptHistoryUrl.val(),
            cache: false,
            data: { CustId: custId, FromDate: fromDate, ToDate: toDate },
            success: (result) => {
                this.renderPTHistory(result);
                this.showPTHistory(icon.position());
            },
            complete: this.$.unblockUI
        });
    };

    renderPTHistory = (html: string) => {
        if (this.$ptHistory) {
            this.$ptHistory.remove();
        }

        if (html === "") {
            return;
        }

        this.$("body").append(html);
        this.$ptHistory = this.$("#pt-history");
    }

    showPTHistory = (position: JQueryCoordinates) => {
        this.$ptHistory.modal({ show: true, backdrop: false });
        this.$ptHistory.css({
            position: "absolute",
            bottom: "auto",
            top: position.top + 18,
            left: "unset",
            right: 20
        });

        this.$ptHistory.find('.modal-dialog').css({
            transform: 'none',
            margin: 0,
            width: "100%"
        });
    };
}