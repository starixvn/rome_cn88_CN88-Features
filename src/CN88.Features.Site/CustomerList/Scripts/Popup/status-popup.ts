class StatusPopup extends PopupBase {
	constructor($: JQueryStatic, topWindow: any) {
		super($, topWindow);
	}

	init = () => {
		this.registerEvents();
		this.getResource();
	}

	registerEvents() {
		super.registerEvents();
		this.$(".btn-submit").bind("click", this.updateStatus);
	}

	getResource() {
		super.getResource();
		this.resources.ThereIsNoChange = this.$("#ThereIsNoChange").val();
	}

	updateStatus = () => {
        let custId = this.$("#CustId").val(),
            oldStatusId = this.$("#CurrentStatusId").val(),
			newStatusId = this.$("#dropdown-status").val(),
            data = { "CustId": custId, "OldStatusId": oldStatusId, "NewStatusId": newStatusId };

        if (oldStatusId === newStatusId) {
			this.showFailedMessage(this.resources.ThereIsNoChange);
			return;
		}

        this.submit("UpdateCustomerStatus", data, custId, newStatusId);
	}

	reloadCallback(custId: any, newValue: any) {
		let mainFrame = this.topWindow.main;

		if (typeof mainFrame.customerList !== "undefined" && typeof mainFrame.customerList.reloadStatusValue === "function") {
			mainFrame.customerList.reloadStatusValue(custId, newValue);
		}
	}
};

$(() => {
	const statusPopup: any = new StatusPopup($, window.top);
	statusPopup.init();
});