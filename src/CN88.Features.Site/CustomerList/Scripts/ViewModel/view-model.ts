"use strict";

class ViewResources {
    $: JQueryStatic;
    public NoInformationIsAvailable: string;
    public Suspended: string;
    public Page: string;
    public Of: string;
    public Pager_DisplayItems: string;
    public ResetSecurityCodeDisabled: string;
    public Disabled: string;
    public NewMember: string;
    public NewAgent: string;
    public EditMember: string;
    public EditAgent: string;
    public EditMasterAgent: string;
    public ResetPassword: string;
    public ResetSecurityCode: string;
    public Active: string;
    public ViewOnly: string;
    public Inactive: string;
    public UnlockTooltip: string;
    public UnlockConfirmTitle: string;
    public UnlockConfirmMessage: string;
    public Ok: string;
    public Cancel: string;

    constructor($: JQueryStatic) {
        this.$ = $;

        this.NoInformationIsAvailable = this.$("#NoInformationIsAvailable").val();

        this.Active = this.$("#Active").val();
        this.ViewOnly = this.$("#ViewOnly").val();
        this.Inactive = this.$("#Inactive").val();
        this.Suspended = this.$("#Suspended").val();

        this.Page = this.$("#Page").val();
        this.Of = this.$("#of").val();
        this.Pager_DisplayItems = this.$("#Pager_DisplayItems").val();
        this.Disabled = this.$("#Disabled").val();

        this.NewMember = this.$("#NewMember").val();
        this.NewAgent = this.$("#NewAgent").val();
        this.EditMember = this.$("#EditMember").val();
        this.EditAgent = this.$("#EditAgent").val();
        this.EditMasterAgent = this.$("#EditMasterAgent").val();

        this.ResetPassword = this.$("#ResetPassword").val();
        this.ResetSecurityCode = this.$("#ResetSecurityCode").val();
        this.UnlockTooltip = this.$("#UnlockTooltip").val();
        this.UnlockConfirmTitle = this.$("#UnlockConfirmTitle").val();
        this.UnlockConfirmMessage = this.$("#UnlockConfirmMessage").val();
        this.Ok = this.$("#OK").val();
        this.Cancel = this.$("#Cancel").val();
    }
}

class ReportParam {
    $: JQueryStatic;
    public DownlineUsername: string;
    public Username: string;
    public SearchRoleId: number;
    public CustId: number;
    public Status: number;
    public PageSize: number;
    public PageIndex: number;

    constructor($: JQueryStatic) {
        this.$ = $;

        this.DownlineUsername = this.$("#DownlineUsername").val();
        this.Username = this.$("#Username").val();
        this.SearchRoleId = parseInt(<string>this.$("#SearchRoleId").val());
        this.CustId = 0;
        this.Status = parseInt(<string>this.$("#Status").val());
        this.PageSize = parseInt(<string>this.$("#PageSize").val());
        this.SearchRoleId = 1;
    }
}