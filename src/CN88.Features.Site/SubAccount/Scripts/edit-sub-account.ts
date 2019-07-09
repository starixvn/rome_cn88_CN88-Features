/// <reference path="base-sub-account.ts" />

class EditSubAccount extends BaseSubAccount {
    constructor($: JQueryStatic, top: Window) {
        super($, top);
    }

    protected cacheElements(): void {
        super.cacheElements();
        this.elements.subAccountId = this.$("#subAccountId").val();
    }

    protected getFormData() {
        let formData = super.getFormData();
        formData.custId = this.elements.subAccountId;

        return formData;
    }
}

$(() => {
    new EditSubAccount($, top).init();
});