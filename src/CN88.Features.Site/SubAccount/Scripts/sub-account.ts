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

    constructor($: JQueryStatic, top: any) {
        this.$ = $;
        this.top = top;
    }

    init(): void {
        this.cacheElements();
        this.bindEvents();
    }

    cacheElements(): void {
        this.tableSubAccount = this.$("#tbl-subaccount");
        this.resources = {
            newSubAccount: this.$("#NewSubAccount").val(),
            editSubAccount: this.$("#EditSubAccount").val(),
            deleteSubAccount: this.$("#DeleteSubAccount").val(),
            resetPassword: this.$("#ResetPassword").val(),
            resetSecurityCode: this.$("#ResetSecurityCode").val()
        };

        this.urls = {
            newSubAccount: this.$("#NewSubAccountLink").val(),
            editSubAccount: this.$("#EditSubAccountLink").val(),
            deleteSubAccount: this.$("#DeleteSubAccountLink").val(),
            resetPassword: this.$("#ResetPasswordLink").val(),
            resetSecurityCode: this.$("#ResetSecurityCodeLink").val()
        };
    }

    bindEvents(): void {
        let self: any = this;

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
                1000,
                350);
        });

        this.tableSubAccount.find(".icon-delete").click(function (): void {
            let param: ISubAccountParam = self.getSubAccountParam(this);
            self.top.popupManager.init(self.$, self.top);
            self.top.popupManager.bootboxConfirm(
                `${self.resources.deleteSubAccount} - ${param.subAccountName}`,
                "Are you sure you want to delete sub account " + param.subAccountName,
                function () { alert(1); }
            );
        });

        this.$("#btnNewSubAccount").click(function (): void {
            self.openPopup(self.resources.newSubAccount, self.urls.newSubAccount, 1000, 350);
        });

        //this.$.ajaxSetup({
        //    beforeSend: function (xhr) {
        //        let antiXSRFToken = self.$('input[name="__RequestVerificationToken"]');
        //        if (antiXSRFToken.length) {
        //            xhr.setRequestHeader("__RequestVerificationToken", antiXSRFToken.val());
        //        }
        //    },
        //    error: function (xhr, textStatus, errorThrown) {
        //        if (xhr.status === 401) {
        //            alert("Oops... you are here because of a system error, please notify the administrators.");
        //            self.top.siteInfrastructure.logout();
        //        }
        //    }
        //});
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
}

$(() => {
    new SubAccount($, top).init();
});