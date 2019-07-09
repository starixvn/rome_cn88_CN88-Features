/// <reference path="../../shared/scripts/components/message.ts" />

abstract class BaseSubAccount {
    $: JQueryStatic;
    top: Window;
    elements: any;
    message: Message;

    protected constructor($: JQueryStatic, top: Window) {
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
            formSubAccount: this.$("#formSubAccount"),
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
        this.elements.acManagementViewOnly.change((e: JQueryEventObject) => {
            const checkbox = this.$(e.target);
            if (!checkbox.is(":checked")) {
                this.elements.acManagementEditView.prop("checked", false);
            }
        });

        this.elements.acManagementEditView.change((e: JQueryEventObject) => {
            const checkbox = this.$(e.target);
            if (checkbox.is(":checked")) {
                this.elements.acManagementViewOnly.prop("checked", true);
            }
        });

        this.elements.formSubAccount.submit((e: JQueryEventObject) => {
            e.preventDefault();
            const formData = this.getFormData();

            if (this.validateFormData(formData)) {
                this.submitForm(formData);
            }
        });

        this.$("#buttonCancel").click(() => {
            this.top["popupManager"].close();
        });
    }

    protected getFormData(): any {
        return {
            name: this.$.trim(this.elements.name.val()),
            pageAccess: this.getPageAccess()
        };
    }

    protected validateFormData(formData): boolean {
        this.message.hideMessage(this.elements.messageContainer);

        return true;
    }

    protected showError(error: string): void {
        this.message.showError(this.elements.messageContainer, error);
    }

    protected getPageAccess(): string {
        let permissions: string[] = [];
        const pageAccesses: JQuery[] = [
            this.elements.totalBets,
            this.elements.acManagementViewOnly,
            this.elements.acManagementEditView,
            this.elements.reports];

        for (const item of pageAccesses) {
            if (item.is(":checked")) {
                permissions.push(item.val());
            }
        }

        return permissions.join(",");
    }

    private submitForm(formData): void {
        this.$.blockUI();

        this.$.ajax({
            type: this.elements.formSubAccount.attr("method"),
            url: this.elements.formSubAccount.attr("action"),
            headers: this.setupAntiXSRF(),
            data: formData,
            dataType: "json",
            success: (data) => {
                if (data.ErrCode) {
                    data = { Code: data.ErrCode, Message: data.ErrMsg };
                }

                const result = new Result(data);
                this.message.showMessage(this.elements.messageContainer, result);

                if (result.IsSuccessful) {
                    this.top["indexManager"].closePopupAndReloadMainIframe();
                }
            },
            error: (xhr: JQueryXHR) => {
                if (xhr.status === 401 || xhr.status === 403) {
                    this.top["siteInfrastructure"].logout();
                }
            },
            complete: this.$.unblockUI
        });
    }

    private setupAntiXSRF() {
        const token = this.$("input[name=__RequestVerificationToken]").val();
        let headers = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }
}