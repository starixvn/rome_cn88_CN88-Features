/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="base-list.ts" />

class MasterList extends BaseList {
    searchSuggestionMaster: JQuery;
    usernameAutoSuggestionPath: string;
    $output: JQuery;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        bootstrapDropdownComponent: BootstrapDropdownComponent,
        numeral: any,
        pager: PagerComponent) {
        super($, statusComponent, bootstrapDropdownComponent, numeral, pager, "Master");
    }

    public init(viewResources: any, indexManager: any) {
        super.init(
            viewResources,
            indexManager,
            this.renderMasterList,
            siteInfrastructure.roleId.master,
            indexManager.viewMaster,
            indexManager.addNewDownline);

        this.cacheDOM();
    }

    cacheDOM = () => {
        this.searchSuggestionMaster = this.$("#txt-username-master");
        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    };

    renderMasterList = (rs: any) => {
        if (rs.SearchAccountId > 0) {
            var buttonAddNew = this.$(".add-directdownline-master");
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
        super.bindReportEvents(rs);
    };
    reloadState = (reportParam: any) => {
        this.$username.val(reportParam.Username);
        this.$dropdownStatus.val(this.$("#dropdown-status-master").val());
        this.reportParam.PageSize = reportParam.PageSize;
        this.reportParam.PageIndex = reportParam.PageIndex;

        this.$("#PageIndexMaster").val(reportParam.PageIndex);
        this.$("#PageSizeMaster").val(reportParam.PageIndex);

        this.viewList();
    };
    initAutoComplete = () => {
        let queryUsernameURL = this.usernameAutoSuggestionPath + "?" +
            this.$.param({ isDir: true, issub: false, ismember: false });
        //initAutoComplete(this.searchSuggestionMaster[0], queryUsernameURL);
        this.$output.css("top", this.searchSuggestionMaster.offset().top + 30);
        this.$output.css("left", this.searchSuggestionMaster.offset().left);
    };
    buildBodyOnDesktopMode(rs: any) {
        let html = "";
        let customerIndex = this.reportParam.PageSize * (this.reportParam.PageIndex - 1) + 1;
        if (rs.Customers.length == 0) {
            html = "<tr><td colspan='8' align='center' class='no-info'>" + this.viewResources.NoInformationIsAvailable + "</td></tr>";
        } else {
            for (let i = 0; i < rs.Customers.length; i++ , customerIndex++) {
                const customer = rs.Customers[i],
                    custId = customer.CustId,
                    username = customer.Username,
                    status = customer.Status,
                    closed = customer.Closed,
                    userAttr = `cust-id='${custId}' username='${username}'`,
                    iconAttr = `data-toggle='tooltip' data-placement='top' ${userAttr}`;

                let actionColumn = '',
                    creditColumn = `<td class='text-right'> ${this.numeral(customer.Credit).format("###,0.00")} </td>`,
                    statusColumn = `<td class='text-center'> ${this.indexManager.getStatusName(status)}</td>`;

                if (this.viewerStatusId === siteInfrastructure.customerStatus.active) {
                    const resetPasswordIcon = `<span class='icon-pass' ${iconAttr} title='${this.indexManager.viewResources.ResetPassword}' cust-role='3'/>`;
                    const securityCodeIcon = `<span class='icon-security' ${iconAttr} title='${this.indexManager.viewResources.ResetSecurityCode}'/>`;
                    const editIcon = `<span class='icon-edit' ${iconAttr} title='${this.indexManager.viewResources.EditMasterAgent}'/>`;
                    const addDownlineIcon = status == siteInfrastructure.customerStatus.active ? `<span class='icon-add' ${iconAttr} title='${this.indexManager.viewResources.NewAgent}'/>` : '';
                    const unclockIcon = closed ? `<span class='icon-unlock' ${iconAttr} title='${this.indexManager.viewResources.UnlockTooltip}'/>` : '';

                    actionColumn = `${resetPasswordIcon} ${securityCodeIcon} ${editIcon} ${addDownlineIcon} ${unclockIcon}`;
                    creditColumn = `<td class='text-right link col-credit' ${userAttr}> ${this.numeral(customer.Credit).format("###,0.00")} </td>`;
                    statusColumn = `<td class='col-status text-center link' ${userAttr} current-status='${customer.Status}'> ${this.indexManager.getStatusName(customer.Status)}</td>`;
                }

                html += `<tr class='row-customer' cust-id='${custId}'>` +
                    `<td class='col-no'> ${customerIndex} </td>` +
                    `<td class='link view-downline' cust-id='${custId}'> ${username} </td>` +
                    `<td>${customer.Name}</td>` +
                    `<td>${customer.Nickname}</td>` +
                    `${creditColumn}<td class='col-date'> ${customer.CreationDateStr} </td>` +
                    `${statusColumn}<td class='col-action text-left'> ${actionColumn} </td></tr>`;
            }
        }

        return html;
    };
    buildBodyOnMobileMode(rs: any) {
        let html = "";
        let customerIndex = this.reportParam.PageSize * (this.reportParam.PageIndex - 1) + 1;
        if (rs.Customers.length == 0) {
            html = "<tr><td colspan='4' align='center' class='no-info'>" + this.viewResources.NoInformationIsAvailable + "</td></tr>";
        } else {
            for (let i = 0; i < rs.Customers.length; i++ , customerIndex++) {
                let customer = rs.Customers[i],
                    status = customer.Status,
                    custId = customer.CustId,
                    username = customer.Username,
                    nickname = customer.Nickname ? customer.Nickname : "---",
                    name = customer.Name ? customer.Name : "---",
                    userAttr = `cust-id='${custId}' username='${username}'`;

                let actionColumn = '',
                    creditColumn = `<td class='text-right'> ${this.numeral(customer.Credit).format("###,0.00")} </td>`,
                    statusColumn = `<td class='text-center'> ${this.indexManager.getStatusName(status)}</td>`;

                if (this.viewerStatusId === siteInfrastructure.customerStatus.active) {
                    const resetPasswordIcon = `<li class='icon-pass' ${userAttr} cust-role='3'>${this.indexManager.viewResources.ResetPassword}</li>`;
                    const securityCodeIcon = `<li class='icon-security' ${userAttr}>${this.indexManager.viewResources.ResetSecurityCode}</li>`;
                    const editIcon = `<li class='icon-edit' ${userAttr}>${this.indexManager.viewResources.EditMasterAgent}</li>`;
                    const addDownlineIcon = status == siteInfrastructure.customerStatus.active ? `<li class='icon-add' ${userAttr}>${this.indexManager.viewResources.NewAgent}</li>` : '';
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

                    creditColumn = `<td class='text-right link col-credit' ${userAttr}> ${this.numeral(customer.Credit).format("###,0.00")}</td>`;
                    statusColumn = `<td class='col-status text-center link' ${userAttr} current-status='${customer.Status}'> ${this.indexManager.getStatusName(customer.Status)}</td>`;
                }


                html += `<tr class='row-customer' cust-id='${custId}'>` +
                    `<td class='link view-downline' ${userAttr}>` +
                        `<div class='username-wrapper'>${username}
                                <div class="cust-nickname">${nickname}</div>
                                <div class="cust-name">${name}</div>
                            </div>
                    </td>${creditColumn} ${statusColumn} ${actionColumn}</tr>`;
            }
        }

        return html
    };
}