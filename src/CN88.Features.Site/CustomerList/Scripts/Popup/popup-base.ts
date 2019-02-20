class PopupBase {
	public $: JQueryStatic;
	public topWindow: any;
	resources = {
		UpdateSuccessfully: '',
		ExceededAgentCredit: '',
		InputNumberError: '',
		ThereIsNoChange: ''
	};

	constructor($: JQueryStatic, topWindow: any) {
		this.$ = $;
		this.topWindow = topWindow;
	}

	public registerEvents() {
		this.$(".btn-cancel").bind("click", this.closePopup);
	}

	public getResource() {
		this.resources.UpdateSuccessfully = this.$("#UpdateSuccessfully").val();
	}

	closePopup = () => {
		this.topWindow.main.$('#myModal .close').click();
	}

	public reloadCallback(custId: any, newValue: any) {
	}

	public submit(url: string, data: any, custId: any, newValue: any) {
		let self = this;
		this.$.blockUI();

		this.$.ajax({
			type: "POST",
			url: url,
			data: data,
			dataType: "json",
			beforeSend: function (xhr) {
				xhr.setRequestHeader("__RequestVerificationToken", $("input[name=\"__RequestVerificationToken\"]").val());
			}})
			.done(function (data) {
				data.IsCompleted ? self.updateSuccesfully(custId, newValue) : self.showFailedMessage(data.Message);
			})
			.always(function () { self.$.unblockUI(); });
	}

	updateSuccesfully(custId: any, newValue: any) {
		let self = this;
		this.showSuccesfullyMessage();

		setTimeout(function () {
			self.reloadCallback(custId, newValue);
			self.closePopup();
		}, 3000);
	}

	showSuccesfullyMessage() {
		this.$("#error-message").html('')
			.text(this.resources.UpdateSuccessfully)
			.removeClass("hidden msg-error")
			.addClass("msg-success");
	}

	public showFailedMessage(message: any) {
		this.$("#error-message").html('')
			.text(message)
			.removeClass("hidden msg-success")
			.addClass("msg-error");
	}
};