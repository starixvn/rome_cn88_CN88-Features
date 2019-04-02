declare var Cleave: any;

class CreditPopup extends PopupBase {
	cleaveCredit: any;
	constructor($: JQueryStatic, topWindow: any) {
		super($, topWindow);
	}

	init = () => {
		this.registerEvents();
		this.getResource();
		this.cleaveCredit = new Cleave('#credit', {
			numeral: true,
			numeralPositiveOnly: true,
            numeralDecimalMark: '.',
			delimiter: ','
		});
	}

	registerEvents() {
		super.registerEvents();
		this.$(".btn-submit").bind("click", this.updateCredit);
	}

	getResource() {
		super.getResource();
        this.resources.InvalidCredit = this.$("#InvalidCredit").val();
		this.resources.InputNumberError = this.$("#InputNumberError").val();
	}

	updateCredit = () => {
		let minCredit = parseFloat(this.$("#MinCredit").val()),
			maxCredit = parseFloat(this.$("#MaxCredit").val()),
			credit = parseFloat(this.cleaveCredit.getRawValue()),
			custId = this.$("#CustId").val(),
			custLevelId = this.$("#CustLevelId").val(),
			data = {
				"CustId": custId,
				"CustLevelId": custLevelId,
				"CreditAmount": credit
			};

		if (isNaN(credit)) {
			this.showFailedMessage(this.resources.InputNumberError);
			return;
		}

		if (credit < minCredit || credit > maxCredit) {
            this.showFailedMessage(this.resources.InvalidCredit);
			return;
		}

		this.submit("UpdateCredit", data, custId, credit);
	}

	reloadCallback(custId: any, newValue: any) {
		this.topWindow.main.customerList.reloadCreditValue(custId, newValue);
	}
}

$(() => {
	const creditPopup: any = new CreditPopup($, window.top);
	creditPopup.init();
})
