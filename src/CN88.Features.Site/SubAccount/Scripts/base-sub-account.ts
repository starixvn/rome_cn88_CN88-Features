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
            name: this.$("#firstName"),
            totalBets: this.$("#totalBets"),
            acManagementViewOnly: this.$("#acManagementViewOnly"),
            acManagementEditView: this.$("#acManagementEditView"),
            reports: this.$("#reports"),
            viewLog: this.$("#viewLog")
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
                console.dir(formData);
                //self.submitForm(self.$(this), formData);
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

        if (formData.name.length === 0) {
            this.showError("This field is required.");
            this.elements.name.focus();
            return false;
        }

        return true;
    }

    protected showError(error: string): void {
        this.message.showError(this.elements.messageContainer, error);
    }

    private getPageAccess(): string {
        let permissions: string[] = [];

        if (this.elements.acManagementEditView.is(":checked")) {
            permissions.push(this.elements.acManagementEditView.val());
        } else {
            if (this.elements.acManagementViewOnly.is(":checked")) {
                permissions.push(this.elements.acManagementViewOnly.val());
            }
        }

        for (let item of [this.elements.totalBets, this.elements.reports, this.elements.viewLog]) {
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

                self.message.showMessage(self.elements.messageContainer, new Result(data));
            },
            error: function (xhr: any): void {
                if (xhr.status === 401) {
                    alert("Oops... you are here because of a system error, please notify the administrators.");
                    self.top.siteInfrastructure.logout();
                }
            },
            complete: self.$.unblockUI
        });
    }

    private setupAntiXSRF(): any {
        let token = this.$("input[name=__RequestVerificationToken]").val();
        let headers = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }
}
