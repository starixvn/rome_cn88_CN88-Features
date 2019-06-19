/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />

class BaseList {
    $: JQueryStatic;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;
    bootstrapDropdownComponent: BootstrapDropdownComponent;

    $tooltip: JQuery;
    $dropdownStatus: JQuery;
    $username: JQuery;
    $tblReport: JQuery;
    $formReport: JQuery;
    getCustomerListUrl: string;

    reportParam = {
        DownlineUsername: null,
        Username: "",
        SearchRoleId: 0,
        CustId: 0,
        Status: 0,
        PageSize: 100,
        PageIndex: 1
    };

    viewResources: any;
    indexManager: any = null;

    levelName: string;
    levelNameLowerCase: string;

    levelId: number;
    viewDownlineFunction: Function;
    minPageSize: number = 10;
    viewerStatusId: number;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        bootstrapDropdownComponent: BootstrapDropdownComponent,
        numeral: any,
        pager: PagerComponent,        
        levelName: string) {
        this.$ = $;
        this.statusComponent = statusComponent;
        this.bootstrapDropdownComponent = bootstrapDropdownComponent;
        this.numeral = numeral;
        this.pager = pager;
        this.levelName = levelName;
        this.levelNameLowerCase = levelName.toLowerCase();
    }

    public init(
        indexManager: any,
        levelId: number,
        viewDownlineFunction: Function) {
        this.indexManager = indexManager;
        this.viewResources = indexManager.viewResources;
        this.levelId = levelId;
        this.viewDownlineFunction = viewDownlineFunction;
        this.viewerStatusId = parseInt(this.$("#ViewerStatusId").val());
        this.cacheDOMElements();

        this.statusComponent.render(
            new SingleSelectionDropdownOption(`dropdown-status-${this.levelNameLowerCase}`, `UserSelectedStatusId${this.levelName}`));
        this.$dropdownStatus = this.$(`#dropdown-status-${this.levelNameLowerCase}`);
        this.$tblReport.stickyTableHeaders();
    }

    public cacheDOMElements() {
        this.$username = this.$(`#txt-username-${this.levelNameLowerCase}`);
        this.getCustomerListUrl = this.$("#getCustomerListUrl").val();
        this.$tblReport = this.$(`#tbl-${this.levelNameLowerCase}`);
        this.$formReport = this.$(`#form-${this.levelNameLowerCase}`);
    };

    loadData() {
        this.$.ajax({
            method: "GET",
            data: this.reportParam,
            url: this.getCustomerListUrl,
            cache: false,
            success: (rs) => {
                this.renderCustomerList(rs);
            }
        });
    }

    initTooltip = () => {
        this.$('[data-toggle="tooltip"]').tooltip();
    };

    pagingList = () => {
        this.reportParam.PageIndex = this.$(`#PageIndex${this.levelName}`).val();
        this.reportParam.PageSize = this.$(`#PageSize${this.levelName}`).val();
        this.loadData();
    };

    bindReportEvents(rs) {
        var self = this;
        this.$tblReport.find(".view-statement").click(function () {
            var $this = self.$(this);
            var viewStatementUrl = `${$("#viewStatementtUrl").val()}?CustName=${$this.attr("username")}&CustId=${$this.attr("cust-id")}`;
            self.indexManager.isDevice ?
                window.open(viewStatementUrl, "_blank") :
                window.open(viewStatementUrl, "MemberStatement", "scrollbars=1,height=500,width=1000");
        });

        this.$tblReport.find(".view-downline").click(function () {
            var $this = self.$(this);
            self.indexManager.isDevice ? 
                self.viewDownlineFunction($this.attr("cust-id"), $this.attr("username"), self.reportParam.Status) :
                self.viewDownlineFunction($this.attr("cust-id"), $this.html(), self.reportParam.Status);
        });

        this.$tblReport.find(".col-credit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateCredit($this.attr("cust-id"), $this.attr("username"), self.levelId);
        });

        this.$tblReport.find(".col-status").click(function () {
            var $this = self.$(this);

            self.indexManager.updateStatus($this.attr("cust-id"), $this.attr("username"), $this.attr("current-status"), $this.attr("direct-upline-status"));
        });

        this.$tblReport.find(".icon-pass").click(function () {
            var $this = self.$(this);

            self.indexManager.resetPassword($this.attr("cust-id"), $this.attr("username"), $this.attr("cust-role"));
        });

        this.$tblReport.find(".icon-security").click(function () {
            var $this = self.$(this);

            self.indexManager.resetSecurityCode($this.attr("cust-id"), $this.attr("username"));
        });

        this.$tblReport.find(".icon-edit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateAccount($this.attr("cust-id"), $this.attr("username"), self.levelId, self.reportParam);
        });

        this.$tblReport.find(".icon-add").click(function () {
            var $this = self.$(this);
            self.indexManager.addNewDownline($this.attr("cust-id"), '', self.levelId - 1, self.reportParam, $this.attr("username"));
        });

        var buttonAddNew = this.$(`.add-directdownline-${this.levelNameLowerCase}`);
        this.$tblReport.find(".icon-unlock").click(function () {
            var $this = self.$(this);

            self.indexManager.updateUnlock($this.attr("cust-id"));
        });

        if (rs.SearchAccountId > 0 && parseInt(rs.SearchAccountStatus) === siteInfrastructure.customerStatus.active) {
            buttonAddNew.removeAttr("disabled");
            buttonAddNew.click(function () {
                var $this = self.$(this);

                self.indexManager.addNewDownline($this.attr("cust-id"), $this.attr("username"), self.levelId, self.reportParam, $this.attr("downline-username"));
            });
        }
        else {
            buttonAddNew.attr("disabled", "disabled");
        }

        const pageIndex = `PageIndex${this.levelName}`, pageSize = `PageSize${this.levelName}`;
        this.$tblReport.find("> tfoot").empty();
        this.$(`#${pageIndex}`).remove();
        this.$(`#${pageSize}`).remove();

        const pagerElement = this.$(`#pager-${this.levelNameLowerCase}`);
        if (rs.TotalCustomers >= this.minPageSize && pagerElement.length > 0) {
            var footer = `<div id='report-pager-${this.levelNameLowerCase}'` +
                `data-labelpage='${this.viewResources.Page}'` +
                `data-labelof='${this.viewResources.Of}'` +
                `data-labeldisplayitems='${this.viewResources.Pager_DisplayItems}'` +
                `data-pageindex='${this.reportParam.PageIndex}'` +
                `data-pagesize='${this.reportParam.PageSize}'` +
                `data-totalrecords='${rs.TotalCustomers}'></div>`;

            pagerElement.html(footer);
            this.pager.render(new PagerOption(this.pagingList, `report-pager-${this.levelNameLowerCase}`, `form-${this.levelNameLowerCase}`, pageIndex, pageSize));
        } else {
            pagerElement.html("");
        }

        this.bootstrapDropdownComponent.render(new BootstrapDropdownOption(".tbl-downlinelist .dropdown"));

        this.initTooltip();
        this.$.unblockUI();
    };

    renderCustomerList(rs: any) {
        if (rs.SearchAccountId > 0) {
            var buttonAddNew = this.$(`.add-directdownline-${this.levelNameLowerCase}`);
            buttonAddNew.attr("cust-id", rs.SearchAccountId);
            buttonAddNew.attr("username", rs.Username);
            buttonAddNew.attr("downline-username", rs.DownlineUsername);
        }

        if (!rs.Customers) {
            return;
        }

        this.$tblReport.removeClass("display-none");

        this.$tblReport.find("> tbody").empty().html(
            this.indexManager.isDevice ? this.buildBodyOnMobileMode(rs) : this.buildBodyOnDesktopMode(rs)
        );
        this.$(window).trigger('resize.stickyTableHeaders');
        this.bindReportEvents(rs);
    };

    buildBodyOnDesktopMode(rs: any) {
        const isAgent = this.levelId === 2, isMember = this.levelId === 1;
        if (rs.Customers.length == 0) {
            return this.buildNoInfoContent(isMember ? 9 : 8);
        }

        let html = "";
        let customerIndex = this.reportParam.PageSize * (this.reportParam.PageIndex - 1) + 1;
        for (let i = 0; i < rs.Customers.length; i++ , customerIndex++) {
            const customer = rs.Customers[i],
                custId = customer.CustId,
                username = customer.Username,
                status = customer.Status,
                statusName = this.indexManager.getStatusName(status),
                closed = customer.Closed,
                userAttr = `cust-id='${custId}' username='${username}'`,
                iconAttr = `data-toggle='tooltip' data-placement='top' ${userAttr}`,
                creditFormat = this.numeral(customer.Credit).format("###,0.00"),
                usernameLink = isMember ? 'view-statement' : 'view-downline';

            let actionColumn = '',
                creditColumn = `<td class='text-right'> ${creditFormat} </td>`,
                statusColumn = `<td class='text-center'> ${statusName}</td>`;

            const groupColumn = isMember ? `<td>${customer.Group}</td>` : '';

            if (this.viewerStatusId === siteInfrastructure.customerStatus.active) {
                creditColumn = `<td class='text-right link col-credit' ${userAttr}> ${creditFormat} </td>`;
                statusColumn = `<td class='col-status text-center link' ${userAttr} current-status='${customer.Status}'> ${statusName}</td>`;

                const resetPasswordIcon = `<span class='icon-pass' ${iconAttr} title='${this.viewResources.ResetPassword}' cust-role=${this.levelId}/>`;

                const securityCodeIcon = isMember ? '' : `<span class='icon-security' ${iconAttr} title='${this.viewResources.ResetSecurityCode}'/>`;

                const editIconText = isMember ?
                    this.viewResources.EditMember :
                    isAgent ? this.viewResources.EditAgent :
                        this.viewResources.EditMasterAgent;
                const editIcon = `<span class='icon-edit' ${iconAttr} title='${editIconText}'/>`;

                const addDownlineIcon = status === siteInfrastructure.customerStatus.active && !isMember? `<span class='icon-add' ${iconAttr} title='${this.viewResources.NewAgent}'/>` : '';

                const unclockIcon = closed ? `<span class='icon-unlock' ${iconAttr} title='${this.viewResources.UnlockTooltip}'/>` : '';

                actionColumn = `<td class='col-action text-left'>${resetPasswordIcon} ${securityCodeIcon} ${editIcon} ${addDownlineIcon} ${unclockIcon}</td>`;
            }

            html += `<tr class='row-customer' cust-id='${custId}'>` +
                `<td class='col-no'> ${customerIndex} </td>` +
                `<td class='link ${usernameLink}' ${iconAttr}> ${username} </td>` +
                `<td>${customer.Name}</td>` +
                `<td>${customer.Nickname}</td>` +
                `${groupColumn}${creditColumn}<td class='col-date'> ${customer.CreationDateStr} </td>` +
                `${statusColumn}  ${actionColumn} </tr>`;
        }

        return html;
    };
    buildBodyOnMobileMode(rs: any) {
        const isAgent = this.levelId === siteInfrastructure.roleId.agent, isMember = this.levelId === siteInfrastructure.roleId.member;
        if (rs.Customers.length == 0) {
            return this.buildNoInfoContent(isMember ? 5: 4);
        }

        let html = "";
        let customerIndex = this.reportParam.PageSize * (this.reportParam.PageIndex - 1) + 1;
        for (let i = 0; i < rs.Customers.length; i++ , customerIndex++) {
            let customer = rs.Customers[i],
                status = customer.Status,
                statusName = this.indexManager.getStatusName(status),
                custId = customer.CustId,
                username = customer.Username,
                nickname = customer.Nickname ? customer.Nickname : "---",
                name = customer.Name ? customer.Name : "---",
                userAttr = `cust-id='${custId}' username='${username}'`,
                creditFormat = this.numeral(customer.Credit).format("###,0.00"),
                usernameLink = isMember ? 'view-statement' : 'view-downline';

            let actionColumn = '',
                creditColumn = `<td class='text-right'> ${creditFormat} </td>`,
                statusColumn = `<td class='text-center'> ${statusName}</td>`,
                viewDownlineColumn = `<td class='link ${usernameLink}' ${userAttr}>` +
                    `<div class='username-wrapper'>${username}
                                <div class="cust-nickname">${nickname}</div>
                                <div class="cust-name">${name}</div>
                            </div>
                    </td>`;

            const groupColumn = isMember ? `<td>${customer.Group}</td>` : '';

            if (this.viewerStatusId === siteInfrastructure.customerStatus.active) {
                creditColumn = `<td class='text-right link col-credit' ${userAttr}> ${creditFormat}</td>`;
                statusColumn = `<td class='col-status text-center link' ${userAttr} current-status='${customer.Status}'> ${statusName}</td>`;

                const resetPasswordIcon = `<li class='icon-pass' ${userAttr} cust-role=${this.levelId}>${this.viewResources.ResetPassword}</li>`;

                const securityCodeIcon = isMember ? '' : `<li class='icon-security' ${userAttr}>${this.viewResources.ResetSecurityCode}</li>`;

                const editIconText = isMember ?
                    this.viewResources.EditMember :
                    isAgent ? this.viewResources.EditAgent :
                        this.viewResources.EditMasterAgent;
                const editIcon = `<li class='icon-edit' ${userAttr}>${editIconText}</li>`;

                const addDownlineIcon = status === siteInfrastructure.customerStatus.active && !isMember ? `<li class='icon-add' ${userAttr}>${this.viewResources.NewAgent}</li>` : '';

                const unclockIcon = closed ? `<li class='icon-unlock' ${userAttr}>Unlock Trans</li>` : '';

                actionColumn = "<td class='col-action text-center'>" +
                    `<div class="dropdown">` +
                    `<a id="${custId}" class="dropdown-toggle" type = "button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >` +
                    (username.Closed ? `<span class="icon-user-unlock"><span class="path1"></span><span class="path2"></span></span>` : `<span class="icon-more-action"></span>`) +
                    `</a>` +
                    `<ul class="dropdown-menu" aria-labelledby="${custId}" >${resetPasswordIcon} ${securityCodeIcon} ${editIcon} ${addDownlineIcon} ${unclockIcon}</ul>` +
                    `</div>` +
                    "</td>" +
                    "";
            }

            html += `<tr class='row-customer' cust-id='${custId}'> ${viewDownlineColumn} ${groupColumn} ${creditColumn} ${statusColumn} ${actionColumn}</tr>`;
        }

        return html
    };
    buildNoInfoContent = (colspan: number) =>
        `<tr><td colspan='${colspan}' align='center' class='no-info'>${this.viewResources.NoInformationIsAvailable}</td></tr>`;
    baseReloadState(reportParam: any, custRoleName: string) {
        this.$username.val(reportParam.Username);
        this.$dropdownStatus.val(this.$(`#dropdown-status-${custRoleName.toLowerCase()}`).val());
        this.reportParam.PageSize = reportParam.PageSize;
        this.reportParam.PageIndex = reportParam.PageIndex;

        this.$(`#PageIndex${custRoleName}`).val(reportParam.PageIndex);
        this.$(`#PageSize${custRoleName}`).val(reportParam.PageIndex);
    };
    setStateWhenViewDownlines(status: number) {
        this.$username.val("");
        this.reportParam.PageIndex = 1;
        this.reportParam.PageSize = 100;
        this.reportParam.Status = status;
        this.statusComponent.changeSelection(status);
    };
    viewList(downlineUsername: string = null, custId: number = 0) {
        this.$.blockUI();
        this.reportParam = {
            DownlineUsername: downlineUsername,
            Username: <string>this.$username.val(),
            SearchRoleId: this.levelId,
            CustId: custId,
            Status: <number>this.$dropdownStatus.find("option:selected").val(),
            PageSize: this.reportParam.PageSize,
            PageIndex: this.reportParam.PageIndex
        };

        if (this.indexManager.isDevice) {
            this.levelId === siteInfrastructure.roleId.master ?
                this.indexManager.masterToggleHandler.hideFilter() :
                this.levelId === siteInfrastructure.roleId.agent ? this.indexManager.agentToggleHandler.hideFilter() :
                    this.indexManager.memberToggleHandler.hideFilter();
        }

        this.loadData();
    }
}
