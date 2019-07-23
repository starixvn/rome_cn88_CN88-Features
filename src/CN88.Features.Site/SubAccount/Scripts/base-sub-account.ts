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
        okLabel: '',
        managedAccountValidationMessage: ''
    };

    protected constructor($: JQueryStatic, top: Window) {
        this.$ = $;
        this.top = top;
        this.message = new Message();
    }

    init(): void {
        this.cacheElements();
        this.bindEvents();
        this.elements.tooltips.tooltip();
    }

    protected cacheElements(): void {
        this.elements = {
            formSubAccount: this.$("#formSubAccount"),
            messageContainer: this.$("#message-container"),
            name: this.$("#name"),
            totalBets: this.$("#totalBets"),
            reports: this.$("#reports"),
            requiredNameError: this.$("#RequiredNameError").val(),
            tooltips: this.$('[data-toggle="tooltip"]')
        };

        this.$checkboxSelectAll = this.$("#ckboxSelectAll");
        this.$downlineCheckboxs = this.$("#tblManagedAccount tr td input[type='checkbox']");

        this.resources.okLabel = this.$("#Ok").val();
        this.resources.successMessage = this.$("#SuccessMessage").val();
        this.resources.systemMessage = this.$("#SystemMessage").val();
        this.resources.selectAllDownlineMesage = this.$("#SelectAllDownlineMessage").val();
        this.resources.managedAccountValidationMessage = this.$("#ManagedAccountValidationMessage").val();
    }

    protected bindEvents(): void {
        this.elements.formSubAccount.submit((e: JQueryEventObject) => {
            e.preventDefault();
            const formData = this.getFormData();

            if (this.validateForm() && this.validateManagedAccount(formData)) {
                this.submitForm(formData);
            }
        });

        this.$("#buttonCancel").click(() => {
            this.top["popupManager"].close();
        });

        this.$checkboxSelectAll.change(() => {
            const isChecked = this.$checkboxSelectAll.is(":checked");
            if (isChecked) {
                const bootboxWidth = 350;
                this.top["popupManager"].bootboxAlert(
                    this.resources.systemMessage,
                    this.resources.selectAllDownlineMesage,
                    null,
                    bootboxWidth,
                    this.resources.okLabel);
            }

            this.$downlineCheckboxs.prop("checked", isChecked);
        });

        this.$downlineCheckboxs.on("click touchend", (e: JQueryEventObject) => {
            if (!this.$(e.target).is(":checked")) {
                this.$checkboxSelectAll.prop("checked", false);
            }
        });

        this.$(".head-title").click(this.collapseSection);

        this.$("body").on("click touchend", (event) => {
            const target = this.$(event.target);
            const showTooltipClass = "tooltip-show";

            if (target.hasClass("icon-info")) {
                const isShowing = target.hasClass(showTooltipClass);

                const showingToolTips = this.$('.tooltip-show[data-toggle="tooltip"]');
                showingToolTips.tooltip("hide");
                showingToolTips.removeClass(showTooltipClass);

                if (isShowing) {
                    target.tooltip("hide");
                    target.removeClass(showTooltipClass);
                } else {
                    target.tooltip("show");
                    target.addClass(showTooltipClass);
                }

                event.stopPropagation();
                return false;
            }

            this.elements.tooltips.tooltip("hide");
            this.elements.tooltips.removeClass(showTooltipClass);
        });
    }

    collapseSection = (e: JQueryEventObject) => {
        const collapsedClass = "collapsed";
        const $title = this.$(e.target);

        if ($title.hasClass(collapsedClass)) {
            this.$($title.data("target")).removeClass("hide");
            $title.removeClass(collapsedClass);
        } else {
            this.$($title.data("target")).addClass("hide");
            $title.addClass(collapsedClass);
        }
    };

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

    protected validateForm(): boolean {
        this.message.hideMessage(this.elements.messageContainer);

        return true;
    }

    protected validateManagedAccount(formData): boolean {
        if (!(formData.isSelectAllDownline || formData.downlineId)) {
            const bootboxWidth = 400;
            this.top["popupManager"].bootboxAlert(
                this.resources.systemMessage,
                this.resources.managedAccountValidationMessage,
                null,
                bootboxWidth,
                this.resources.okLabel);

            return false;
        }

        return true;
    }

    protected showError(error: string): void {
        this.message.showError(this.elements.messageContainer, error);
    }

    protected getPageAccess(): string {
        const permissions: string[] = [];
        const pageAccesses: JQuery[] = [this.elements.totalBets, this.elements.reports];

        for (const item of pageAccesses) {
            if (item.is(":checked")) {
                permissions.push(item.val());
            }
        }

        return permissions.join(",");
    }

    public submitForm(formData, isSaveAndNew = false): void {
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
                    if (isSaveAndNew) {
                        const timeOutInMiliseconds = 3000;
                        setTimeout(() => {
                            this.top.$("#main").attr("src", (index, attr) => { return attr; });
                            window.location.reload();
                        }, timeOutInMiliseconds);
                    } else {
                        this.top["indexManager"].closePopupAndReloadMainIframe();
                    }
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
        const headers = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }
}