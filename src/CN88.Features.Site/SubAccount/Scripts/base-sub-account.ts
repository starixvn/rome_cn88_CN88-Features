/// <reference path="../../shared/scripts/components/message.ts" />

abstract class BaseSubAccount {
    $: JQueryStatic;
    top: any;
    elements: any;
    message: Message;

    protected constructor($: JQueryStatic, top: any) {
        this.$ = $;
        this.top = top;
        this.message = new Message();
    }

    init(): void {
        this.cacheElements();
        this.bindEvents();
    }

    protected cacheElements(): void {
        this.elements = {
            messageContainer: this.$("#message-container"),
            name: this.$("#name"),
            totalBets: this.$("#totalBets"),
            acManagementViewOnly: this.$("#acManagementViewOnly"),
            acManagementEditView: this.$("#acManagementEditView"),
            reports: this.$("#reports"),
            requiredNameError: this.$("#RequiredNameError").val()
        };
    }

    private bindEvents(): void {
        let self: any = this;

        this.elements.acManagementViewOnly.change(function (): void {
            if (!this.checked) {
                self.elements.acManagementEditView.prop("checked", false);
            }
        });

        this.elements.acManagementEditView.change(function (): void {
            if (this.checked) {
                self.elements.acManagementViewOnly.prop("checked", true);
            }
        });

        this.$("#formSubAccount").submit(function (e: any): void {
            e.preventDefault();
            let formData: any = self.getFormData();

            if (self.validateFormData(formData)) {
                self.submitForm(self.$(this), formData);
            }
        });

        this.$("#buttonCancel").click(function (): void {
            self.top.popupManager.close();
        });
    }

    protected getFormData(): any {
        return {
            name: this.$.trim(this.elements.name.val()),
            pageAccess: this.getPageAccess()
        };
    }

    protected validateFormData(formData: any): boolean {
        this.message.hideMessage(this.elements.messageContainer);

        return true;
    }

    protected showError(error: string): void {
        this.message.showError(this.elements.messageContainer, error);
    }

    protected getPageAccess(): string {
        let permissions: string[] = [];
        let pageAccesses: JQuery[] = [
            this.elements.totalBets,
            this.elements.acManagementViewOnly,
            this.elements.acManagementEditView,
            this.elements.reports];

        for (let item of pageAccesses) {
            if (item.is(":checked")) {
                permissions.push(item.val());
            }
        }

        return permissions.join(",");
    }

    private submitForm(formSubAccount: JQuery, formData: any): void {
        this.$.blockUI();
        let self: any = this;

        this.$.ajax({
            type: formSubAccount.attr("method"),
            url: formSubAccount.attr("action"),
            headers: this.setupAntiXSRF(),
            data: formData,
            dataType: "json",
            success: function (data: any): void {
                if (data.ErrCode) {
                    data = { Code: data.ErrCode, Message: data.ErrMsg };
                }

                let result: Result = new Result(data);
                self.message.showMessage(self.elements.messageContainer, result);

                if (result.IsSuccessful) {
                    self.top.indexManager.closePopupAndReloadMainIframe();
                }
            },
            error: function (xhr: any): void {
                if (xhr.status === 401 || xhr.status === 403) {
                    self.top.siteInfrastructure.logout();
                }
            },
            complete: self.$.unblockUI
        });
    }

    private setupAntiXSRF(): any {
        let token: string = this.$("input[name=__RequestVerificationToken]").val();
        let headers: any = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }
}

export { BaseSubAccount };