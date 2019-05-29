"use strict";
interface ISubAccountParam {
    subAccountId: number;
    subAccountName: string;
}

class SubAccount {
    $: JQueryStatic;
    top: any;
    tableSubAccount: JQuery;
    resources: any;
    urls: any;
    sortComponent: SortComponent;

    constructor(
        $: JQueryStatic,
        top: any,
        sortComponent: SortComponent) {
        this.$ = $;
        this.top = top;
        this.sortComponent = sortComponent;
    }

    init(): void {
        this.cacheElements();
        this.bindEvents();

        this.sortComponent.render(
            new SortOption(
                this.tableSubAccount,
                this.tableSubAccount.data("totalrecords") > 1,
                true,
                this.sortComponent.afterSort,
                {
                    0: { sorter: "text" },
                    1: { sorter: "text" },
                    2: { sorter: "text" },
                    7: { sorter: "text" }
                }));
    }

    cacheElements(): void {
        this.tableSubAccount = this.$("#tbl-subaccount");

        this.resources = {
            newSubAccount: this.$("#NewSubAccount").val(),
            editSubAccount: this.$("#EditSubAccount").val(),
            resetPassword: this.$("#ResetPassword").val(),
            resetSecurityCode: this.$("#ResetSecurityCode").val(),
            unlockConfirmMessage: this.$("#UnlockAccountConfirmMessage").val(),
            unlockSubAccount: this.$("#UnlockSubAccount").val(),
            okLabel: this.$("#OK").val(),
            cancelLabel: this.$("#Cancel").val()
        };

        this.urls = {
            newSubAccount: this.$("#NewSubAccountLink").val(),
            editSubAccount: this.$("#EditSubAccountLink").val(),
            resetPassword: this.$("#ResetPasswordLink").val(),
            resetSecurityCode: this.$("#ResetSecurityCodeLink").val(),
            unlockSubAccount: this.$("#UnlockSubAccountLink").val(),
            verifySecurityCode: this.$("#VerifySecurityCodeLink").val()
        };
    }

    bindEvents(): void {
        let self = this;

        this.tableSubAccount.stickyTableHeaders();

        this.tableSubAccount.find("[data-toggle='tooltip']").tooltip();

        this.tableSubAccount.find(".icon-pass").click(function (): void {
            let param: ISubAccountParam = self.getSubAccountParam(this);
            self.openPopup(
                `${self.resources.resetPassword} - ${param.subAccountName}`,
                `${self.urls.resetPassword}?custId=${param.subAccountId}`,
                490,
                430);
        });

        this.tableSubAccount.find(".icon-security").click(function (): void {
            let param: ISubAccountParam = self.getSubAccountParam(this);
            self.openPopup(
                `${self.resources.resetSecurityCode} - ${param.subAccountName}`,
                `${self.urls.resetSecurityCode}?custId=${param.subAccountId}`,
                490,
                344);
        });

        this.tableSubAccount.find(".icon-edit").click(function (): void {
            let param: ISubAccountParam = self.getSubAccountParam(this);
            self.openPopup(
                `${self.resources.editSubAccount} - ${param.subAccountName}`,
                `${self.urls.editSubAccount}?custId=${param.subAccountId}`,
                948,
                470);
        });

        this.$("#btnNewSubAccount").click(function (): void {
            self.openPopup(self.resources.newSubAccount, self.urls.newSubAccount, 948, 470);
        });

        this.tableSubAccount.find(".icon-unlock").click(function (): void {
            const param: ISubAccountParam = self.getSubAccountParam(this);

            self.top.popupManager.bootboxConfirm(
                self.resources.unlockSubAccount + " - " + param.subAccountName,
                self.resources.unlockConfirmMessage,
                () => {
                    self.$.blockUI();
                    const options = {
                        url: self.urls.unlockSubAccount,
                        data: { custId: param.subAccountId }
                    };

                    self.$.ajax({
                        type: "POST",
                        url: options.url,
                        data: options.data,
                        headers: self.setupAntiXSRF(),
                        dataType: "json",
                        success: (result) => { self.checkSecurityCode(result, options); },
                        complete: self.$.unblockUI
                    });
                },
                350,
                self.resources.okLabel,
                self.resources.cancelLabel);
        });
    }

    checkSecurityCode(result: any, options: any): void {
        if (result.code === 999991) {
            this.top[options.url] = () => {
                this.$.ajax({
                    type: "POST",
                    url: options.url,
                    data: options.data,
                    headers: this.setupAntiXSRF(),
                    dataType: "json",
                    success: (result) => { location.reload(true); },
                    complete: options.onComplete,
                });
            };

            this.showSecCodePopup(options.url);
            return;
        }

        location.reload(true);
    }

    getSubAccountParam(element: HTMLElement): ISubAccountParam {
        let parent: JQuery = this.$(element).closest("td");

        return {
            subAccountId: parent.data("subaccid"),
            subAccountName: parent.data("subaccname")
        };
    }

    openPopup(title: string, url: string, width: number, height: number): void {
        this.top.popupManager.init(this.$, this.top);
        this.top.popupManager.open({
            title: title,
            url: url,
            width: width,
            height: height
        });
    }

    showSecCodePopup(preUrl: string): void {
        this.top.popupManager.init(this.$, this.top);
        this.top.popupManager.open({
            title: "",
            url: this.urls.verifySecurityCode,
            width: 500,
            height: 190,
            preUrl: preUrl
        });
    }

    private setupAntiXSRF(): any {
        let token: string = this.$("input[name=__RequestVerificationToken]").val();
        let headers: any = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }
}

$(() => {
    new SubAccount($, top, new SortComponent($, new LinkBuilderComponent($))).init();
});