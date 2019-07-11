class StatusPopup extends PopupBase {
    constructor($: JQueryStatic, topWindow: Window) {
        super($, topWindow);
    }

    init = () => {
        this.registerEvents();
        this.getResource();
    };

    registerEvents() {
        super.registerEvents();
        this.$(".btn-submit").bind("click", this.updateStatus);
    }

    getResource() {
        super.getResource();
        this.resources.ThereIsNoChange = this.$("#ThereIsNoChange").val();
    }

    updateStatus = () => {
        const custId = this.$("#CustId").val(),
            oldStatusId = this.$("#CurrentStatusId").val(),
            newStatusId = this.$("#dropdown-status").val(),
            custLevelName = this.$("#CustLevelName").val(),
            data = {
                "CustId": custId,
                "OldStatusId": oldStatusId,
                "NewStatusId": newStatusId,
                "UplineStatusId": this.$("#direct-upline-status").val()
            };

        if (oldStatusId === newStatusId) {
            this.showFailedMessage(this.resources.ThereIsNoChange);
            return;
        }

        this.submit("UpdateCustomerStatus", data, custLevelName);
    };
}

$(() => {
    const statusPopup = new StatusPopup($, window.top);
    statusPopup.init();
});