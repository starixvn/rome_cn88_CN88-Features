/// <reference path="../../shared/scripts/components/message.ts" />

abstract class BaseSubAccount {
    $: JQueryStatic;
    top: Window;
    elements: any;
    message: Message;
    $checkboxSelectAll: JQuery;
    $downlineCheckboxs: JQuery;
    resources = {
        systemMessage: '',
        selectAllDownlineMesage: '',
        successMessage: '',
        okLabel: ''
    };

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
            reports: this.$("#reports"),
            requiredNameError: this.$("#RequiredNameError").val()
        };

        this.$checkboxSelectAll = this.$("#ckboxSelectAll");
        this.$downlineCheckboxs = this.$("#tblManagedAccount tr td input[type='checkbox']");

        this.resources.okLabel = this.$("#Ok").val();
        this.resources.successMessage = this.$("#SuccessMessage").val();
        this.resources.systemMessage = this.$("#SystemMessage").val();
        this.resources.selectAllDownlineMesage = this.$("#SelectAllDownlineMessage").val();
    }

    private bindEvents(): void {
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

        this.$checkboxSelectAll.change(() => {
            const isChecked = this.$checkboxSelectAll.is(":checked");
            if (isChecked) {
                this.top["popupManager"].bootboxAlert(
                    this.resources.systemMessage,
                    this.resources.selectAllDownlineMesage,
                    null,
                    350,
                    this.resources.okLabel);
            }

            this.$downlineCheckboxs.prop("checked", isChecked);
        });

        this.$downlineCheckboxs.on("click touchend", (e: JQueryEventObject) => {
            if (this.$(e.target).is(":checked")) {
                let isAllChecked = true;

                this.$downlineCheckboxs.each(function () {
                    if (!this.checked) {
                        isAllChecked = false;
                    }
                });

                if (isAllChecked) {
                    this.$checkboxSelectAll.prop("checked", true);
                }
            } else {
                this.$checkboxSelectAll.prop("checked", false);
            }
        });
    }

    protected getFormData(): any {
        let downlineId = "";
        const isSelectAllDownline = this.$checkboxSelectAll.is(":checked");

        if (!isSelectAllDownline) {
            downlineId = this.getSelectedDownlines();
        }

        return {
            name: this.$.trim(this.elements.name.val()),
            pageAccess: this.getPageAccess(),
            isSelectAllDownline: isSelectAllDownline,
            downlineId: downlineId
        };
    }

    private getSelectedDownlines(): string {
        const selected = [];
        this.$downlineCheckboxs.each((index: number, elem: Element) => {
            const $elem = this.$(elem);
            if ($elem.is(":checked")) {
                selected.push([$elem.val()]);
            }
        });

        return selected.join(",");
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
        const pageAccesses: JQuery[] = [this.elements.totalBets, this.elements.reports];

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

                if (result.IsSuccessful) {
                    this.message.showSuccess(this.elements.messageContainer, this.resources.successMessage);
                    this.top["indexManager"].closePopupAndReloadMainIframe();
                } else {
                    this.message.showMessage(this.elements.messageContainer, result);
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