"use strict";
class SubAccount {
    $: JQueryStatic;
    top: any;
    $tblSubAccount: JQuery;
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
        this.$tblSubAccount = this.$("#tbl-subaccount");
        this.resources = {
            ResetPassword: this.$("#ResetPassword").val(),
            ResetSecurityCode: this.$("#ResetSecurityCode").val()
        };

        this.urls = {
            FeaturesAppPath: this.$("#FeaturesAppPath").val(),
            ResetPassword: this.$("#ResetPasswordLink").val(),
            ResetSecurityCode: this.$("#ResetSecurityCodeLink").val()
        };
    }

    bindEvents(): void {
        let self: any = this;

        this.$tblSubAccount.stickyTableHeaders();

        this.$tblSubAccount.find('[data-toggle="tooltip"]').tooltip();

        this.$tblSubAccount.find(".icon-pass").click(function (): void {
            let param: any = self.getSubAccountParam(this);
            let popupTitle: string = `${self.resources.ResetPassword} - ${param.subAccName}`;
            let url: string = `${self.urls.ResetPassword}?custId=${param.subAccId}`;

            self.openPopup(popupTitle, url, 490, 430);
        });

        this.$tblSubAccount.find(".icon-security").click(function (): void {
            let param: any = self.getSubAccountParam(this);
            let popupTitle: string = `${self.resources.ResetSecurityCode} - ${param.subAccName}`;
            let url: string = `${self.urls.ResetSecurityCode}?custId=${param.subAccId}`;

            self.openPopup(popupTitle, url, 490, 344);
        });

        this.$tblSubAccount.find(".icon-edit").click(function (): void {
            self.$.blockUI();
            let param: any = self.getSubAccountParam(this);
            let title: string = `Edit Sub Account - ${param.subAccName}`;
            let url: string = `${self.urls.FeaturesAppPath}SubAccount/Update?custId=${param.subAccId}`;

            self.$.get(url, function (data) {
                self.top.popupManager.init(self.$, self.top);
                self.top.popupManager.bootboxDialog(
                    title,
                    data,
                    function () {
                        const editSubAccount: EditSubAccount = new EditSubAccount(self.top.$, self.top);
                        editSubAccount.edit(param.subAccId);
                    });

                self.$.unblockUI();
            });
        });

        this.$("#btnCreateSubAccount").click(function (): void {
            self.$.blockUI();
            let title: string = "Create Sub Account";
            let url: string = `${self.urls.FeaturesAppPath}SubAccount/Create`;

            self.$.get(url, function (data) {
                self.top.popupManager.init(self.$, self.top);
                self.top.popupManager.bootboxDialog(
                    title,
                    data,
                    function () {
                        const createSubAccount: CreateSubAccount = new CreateSubAccount(self.top.$, self.top);
                        createSubAccount.create();
                    });

                self.$.unblockUI();
            });
        });

        this.$.ajaxSetup({
            beforeSend: function (xhr) {
                let antiXSRFToken = self.$('input[name="__RequestVerificationToken"]');
                if (antiXSRFToken.length) {
                    xhr.setRequestHeader("__RequestVerificationToken", antiXSRFToken.val());
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if (xhr.status === 401) {
                    alert("Oops... you are here because of a system error, please notify the administrators.");
                    self.top.siteInfrastructure.logout();
                }
            }
        });
    }

    getSubAccountParam(element: HTMLElement): any {
        let parent: JQuery = this.$(element).closest("td");

        return {
            subAccId: parent.data("subaccid"),
            subAccName: parent.data("subaccname")
        };
    }

    openPopup(popupTitle: string, url: string, width: number, height: number) {
        this.top.popupManager.init(this.$, this.top);
        this.top.popupManager.open({
            title: popupTitle,
            url: url,
            width: width,
            height: height
        });
    }
}

$(() => {
    const subAccount: SubAccount = new SubAccount($, top);
    subAccount.init();
});