"use strict";

/// <reference path="../../shared/scripts/components/sort.ts" />
/// <reference path="../../shared/scripts/components/link-builder.ts" />

interface ISubAccountParam {
    subAccountId: number;
    subAccountName: string;
}

class SubAccount {
    $: JQueryStatic;
    top: Window;
    tableSubAccount: JQuery;
    resources = {
        newSubAccount: '',
        editSubAccount: '',
        resetPassword: '',
        resetSecurityCode: '',
        unlockConfirmMessage: '',
        unlockSubAccount: '',
        deleteConfirmMessage: '',
        deleteSubAccount: '',
        okLabel: '',
        cancelLabel: ''
    };
    urls = {
        newSubAccount: '',
        editSubAccount: '',
        resetPassword: '',
        resetSecurityCode: '',
        unlockSubAccount: '',
        deleteSubAccount: '',
        verifySecurityCode: ''
    };
    isDevice: boolean;

    sortComponent: SortComponent;
    bootstrapDropdownComponent: BootstrapDropdownComponent;

    constructor(
        $: JQueryStatic,
        top: Window,
        sortComponent: SortComponent,
        bootstrapDropdownComponent: BootstrapDropdownComponent) {
        this.$ = $;
        this.top = top;
        this.sortComponent = sortComponent;
        this.bootstrapDropdownComponent = bootstrapDropdownComponent;
        this.isDevice = cn88Features.convertAttributeToBool(this.$("#IsDevice").val());
    }

    init(): void {
        this.cacheElements();
        this.bindEvents();

        if (!this.isDevice) {
            const headers = {
                0: { sorter: "text" },
                1: { sorter: "text" },
                2: { sorter: "text" }
            };
            headers[this.$("#createdDateColIndex").val()] = { sorter: "text" };
            this.sortComponent.render(
                new SortOption(
                    this.tableSubAccount,
                    this.tableSubAccount.data("totalrecords") > 1,
                    true,
                    this.sortComponent.afterSort,
                    headers));
        }

        this.bootstrapDropdownComponent.render(new BootstrapDropdownOption(".tbl-subaccount .dropdown"));
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
            deleteConfirmMessage: this.$("#DeleteSubAccountConfirmMessage").val(),
            deleteSubAccount: this.$("#DeleteSubAccount").val(),
            okLabel: this.$("#OK").val(),
            cancelLabel: this.$("#Cancel").val()
        };

        this.urls = {
            newSubAccount: this.$("#NewSubAccountLink").val(),
            editSubAccount: this.$("#EditSubAccountLink").val(),
            resetPassword: this.$("#ResetPasswordLink").val(),
            resetSecurityCode: this.$("#ResetSecurityCodeLink").val(),
            unlockSubAccount: this.$("#UnlockSubAccountLink").val(),
            deleteSubAccount: this.$("#DeleteSubAccountLink").val(),
            verifySecurityCode: this.$("#VerifySecurityCodeLink").val()
        };
    }

    bindEvents(): void {
        this.tableSubAccount.stickyTableHeaders();

        this.tableSubAccount.find("[data-toggle='tooltip']").tooltip();

        this.tableSubAccount.find(".icon-pass, .opt-pass").click((event: JQueryEventObject) => {
            const param = this.getSubAccountParam(this.$(event.currentTarget));
            const width = 490, height = 430;

            this.openPopup(
                `${this.resources.resetPassword} - ${param.subAccountName}`,
                `${this.urls.resetPassword}?custId=${param.subAccountId}`,
                width,
                height);
        });

        this.tableSubAccount.find(".icon-security, .opt-security").click((event: JQueryEventObject) => {
            const param = this.getSubAccountParam(this.$(event.currentTarget));
            const width = 490, height = 344;
            this.openPopup(
                `${this.resources.resetSecurityCode} - ${param.subAccountName}`,
                `${this.urls.resetSecurityCode}?custId=${param.subAccountId}`,
                width,
                height);
        });

        this.tableSubAccount.find(".icon-edit, .opt-edit").click((event: JQueryEventObject) => {
            const param = this.getSubAccountParam(this.$(event.currentTarget));
            const width = 948, height = 600;
            this.openPopup(
                `${this.resources.editSubAccount} - ${param.subAccountName}`,
                `${this.urls.editSubAccount}?custId=${param.subAccountId}`,
                width,
                height);
        });

        this.tableSubAccount.find(".icon-more-action").on("click touchend", () => {
            // Hide other dropdowns
        });

        this.$("#btnNewSubAccount").click(() => {
            const width = 948, height = 600;
            this.openPopup(this.resources.newSubAccount, this.urls.newSubAccount, width, height);
        });

        this.tableSubAccount.find(".icon-unlock, .opt-unlock").click((event: JQueryEventObject) => {
            this.unlockOrDeleteSubAccount(
                event,
                this.resources.unlockSubAccount,
                this.resources.unlockConfirmMessage,
                this.urls.unlockSubAccount);
        });

        this.tableSubAccount.find(".icon-delete, .opt-delete").click((event: JQueryEventObject) => {
            this.unlockOrDeleteSubAccount(
                event,
                this.resources.deleteSubAccount,
                this.resources.deleteConfirmMessage,
                this.urls.deleteSubAccount);
        });
    }

    getSubAccountParam(element: JQuery): ISubAccountParam {
        const parent = element.closest("td");

        return {
            subAccountId: parent.data("subaccid"),
            subAccountName: parent.data("subaccname")
        };
    }

    openPopup(title: string, url: string, width: number, height: number): void {
        this.top["popupManager"].init(this.$, this.top);
        this.top["popupManager"].open({
            title: title,
            url: url,
            width: width,
            height: height
        });
    }

    unlockOrDeleteSubAccount(event: JQueryEventObject, popupTitle: string, popupMessage: string, url: string) {
        const param = this.getSubAccountParam(this.$(event.currentTarget));
        const popupWidth = 350;

        this.top["popupManager"].bootboxConfirm(
            `${popupTitle} - ${param.subAccountName}`,
            popupMessage,
            () => {
                this.$.blockUI();
                const options = {
                    url: url,
                    data: { custId: param.subAccountId },
                    onComplete: () => {
                        location.reload();
                    }
                };
                const securityCodeChecker = new SecurityCodeChecker(this.$, this.top, popupTitle, this.resources.okLabel);

                this.$.ajax({
                    type: "POST",
                    url: options.url,
                    data: options.data,
                    headers: securityCodeChecker.setupAntiXSRF(),
                    dataType: "json",
                    success: (result) => { securityCodeChecker.checkSecurityCode(result, options); },
                    complete: self.$.unblockUI
                });
            },
            popupWidth,
            this.resources.okLabel,
            this.resources.cancelLabel);
    }
}

$(() => {
    new SubAccount(
        $,
        top,
        new SortComponent($, new LinkBuilderComponent($)),
        new BootstrapDropdownComponent($, top)).init();
});