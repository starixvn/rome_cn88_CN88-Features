class EditSubAccount extends BaseSubAccount {
    constructor($: JQueryStatic, top: any) {
        super($, top);
    }

    protected cacheElements(): void {
        super.cacheElements();
        this.elements.subAccountId = this.$("#subAccountId").val();
    }

    protected getFormData(): any {
        let formData: any = super.getFormData();
        formData.custId = this.elements.subAccountId;

        return formData;
    }
}

$(() => {
    new EditSubAccount($, top).init();
});