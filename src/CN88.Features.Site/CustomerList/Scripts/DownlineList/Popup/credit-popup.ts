class CreditPopup extends PopupBase {
    cleaveCredit: Cleave;
    constructor($: JQueryStatic, topWindow: Window) {
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
    };

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
        const minCredit = parseFloat(this.$("#MinCredit").val()),
            maxCredit = parseFloat(this.$("#MaxCredit").val()),
            credit = parseFloat(this.cleaveCredit.getRawValue()),
            custId = this.$("#CustId").val(),
            custLevelId = this.$("#CustLevelId").val(),
            custLevelName = this.$("#CustLevelName").val(),
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

        this.submit("UpdateCredit", data, custLevelName);
    };
}

$(() => {
    const creditPopup = new CreditPopup($, window.top);
    creditPopup.init();
});