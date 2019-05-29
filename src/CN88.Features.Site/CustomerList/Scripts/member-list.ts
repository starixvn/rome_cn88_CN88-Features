/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="base-list.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />

class MemberList extends BaseList {
    $viewedAgent: JQuery;
    $errMember: JQuery;
    directDownline: number;

    $output: JQuery;
    usernameAutoSuggestionPath: string;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        bootstrapDropdownComponent: BootstrapDropdownComponent,
        numeral: any,
        pager: PagerComponent) {
        super($, statusComponent, bootstrapDropdownComponent, numeral, pager, "Member");
    }

    public init(viewResources: any, indexManager: any) {
        super.init(
            viewResources,
            indexManager,
            this.renderMemberList,
            siteInfrastructure.roleId.member,
            null,
            indexManager.addNewDownline);

        this.cacheDOMElements();

        let self = this;
        this.$("#btn-submit-member").off("click").on("click", function () {
            self.reportParam.PageIndex = 1;
            self.viewMembers(self.reportParam.CustId);
        });
        if (this.directDownline >= siteInfrastructure.roleId.agent) {
            //this.initAutoComplete();
        }
    }

    public cacheDOMElements() {
        super.cacheDOMElements();

        this.$viewedAgent = this.$("#txt-viewed-agent");
        this.directDownline = parseInt(this.$("#directDownline").val());
        this.$errMember = this.$("#msg-err-member");
        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    }
    viewMembers = (custId: number) => {
        this.$.blockUI();
        this.reportParam = {
            DownlineUsername: this.$("#txt-viewed-agent").val(),
            Username: this.$username.val(),
            SearchRoleId: siteInfrastructure.roleId.member,
            CustId: custId,
            Status: this.$dropdownStatus.find("option:selected").val(),
            PageSize: this.reportParam.PageSize,
            PageIndex: this.reportParam.PageIndex
        };

        if (this.directDownline != siteInfrastructure.roleId.member &&
            (this.reportParam.DownlineUsername == "" ||
                this.reportParam.DownlineUsername == undefined)) {
            this.$errMember.removeClass("display-none");
            this.$.unblockUI();
            return;
        }

        this.$errMember.addClass("display-none");

        if (this.indexManager.isDevice) {
            this.indexManager.memberToggleHandler.hideFilter();
        }

        this.loadData();
    };
    renderMemberList = (rs: any) => {
        if (rs.SearchAccountId > 0) {
            var buttonAddNew = this.$(".add-directdownline-member");
            buttonAddNew.attr("cust-id", rs.SearchAccountId);
            buttonAddNew.attr("username", rs.Username);
            buttonAddNew.attr("downline-username", rs.DownlineUsername);
        }

        if (!rs.Customers) {
            return;
        }

        this.$tblReport.removeClass("display-none");

        let html = "";
        let customerIndex = this.reportParam.PageSize * (this.reportParam.PageIndex - 1) + 1;

        if (this.indexManager.isDevice) {
            if (rs.Customers.length == 0) {
                html = "<tr><td colspan='9' align='center' class='no-info'>" + this.viewResources.NoInformationIsAvailable + "</td></tr>";
            } else {
                for (let i = 0; i < rs.Customers.length; i++ , customerIndex++) {
                    let customer = rs.Customers[i],
                        custId = customer.CustId,
                        username = customer.Username,
                        nickname = customer.Nickname ? customer.Nickname : "---",
                        name = customer.Name ? customer.Name : "---";

                    html += `<tr class='row-customer ${this.indexManager.getCustomerColor(customer)}' cust-id='${custId}'>` +
                        `<td class='link view-statement' cust-id='${custId}' username='${username}'>` +
                        `<div class='username-wrapper'>${username}
                                <div class="cust-nickname">${nickname}</div>
                                <div class="cust-name">${name}</div>
                            </div>
                        </td>` +
                        "<td>" + customer.Group + "</td>" +
                        `<td class='text-right link col-credit' cust-id='${custId}' username='${username}'>` + this.numeral(customer.Credit).format("###,0.00") + "</td>" +
                        `<td class='col-status text-center link' cust-id='${custId}' username='${username}' current-status='` + customer.Status + "'>" + this.indexManager.getStatusName(customer.Status) + "</td>" +
                        "<td class='col-action text-center'>" +
                        `<div class="dropdown">` +
                        `<a id="${custId}" class="dropdown-toggle" type = "button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >` +
                        (username.Closed ? `<span class="icon-user-unlock"><span class="path1"></span><span class="path2"></span></span>` : `<span class="icon-more-action"></span>`) +
                        `</a>` +
                        `<ul class="dropdown-menu" aria-labelledby="${custId}" >` +
                        `<li class='icon-pass' cust-id='${custId}' username='${username}' cust-role='1'>${this.indexManager.viewResources.ResetPassword}</li>` +
                        `<li class='icon-edit' cust-id='${custId}' username='${username}'>${this.indexManager.viewResources.EditMember}</li>` +
                        `</ul>` +
                        `</div>` +
                        "</td>" +
                        "</tr>";
                }
            }
        } else {
            if (rs.Customers.length == 0) {
                html = "<tr><td colspan='9' align='center' class='no-info'>" + this.viewResources.NoInformationIsAvailable + "</td></tr>";
            } else {
                for (let i = 0; i < rs.Customers.length; i++ , customerIndex++) {
                    let customer = rs.Customers[i],
                        custId = customer.CustId,
                    username = customer.Username,
                    status = customer.Status,
                    closed = customer.Closed,
                    userAttr = `cust-id='${custId}' username='${username}'`,
                    iconAttr = `data-toggle='tooltip' data-placement='top' ${userAttr}`; 
                
                let actionColumn = '';

                if (this.viewerStatusId === siteInfrastructure.customerStatus.active) {
                    const resetPasswordIcon = `<span class='icon-pass' ${iconAttr} title='${this.indexManager.viewResources.ResetPassword}' cust-role='1'/>`;
                    const editIcon = `<span class='icon-edit' ${iconAttr} title='${this.indexManager.viewResources.EditMember}'/>`;
                    const unclockIcon = closed ? `<span class='icon-unlock' ${iconAttr} title='${this.indexManager.viewResources.UnlockTooltip}'/>` : '';
                    actionColumn = `${resetPasswordIcon} ${editIcon} ${unclockIcon}`;
                }

                html += `<tr class='row-customer' cust-id='${custId}'>` +
                    `<td class='col-no'> ${customerIndex} </td>` +
                        `<td class='link view-statement' username='${username}' cust-id='${custId}'>${username}</td>` +
                    `<td>${customer.Name}</td>` +
                    `<td>${customer.Nickname}</td>` +
                    `<td>${customer.Group}</td>` +
                    `<td class='text-right link col-credit' ${userAttr}> ${this.numeral(customer.Credit).format("###,0.00")} </td>` +
                    `<td class='col-date'> ${customer.CreationDateStr} </td>` +
                    `<td class='col-status text-center link' ${userAttr} current-status='${status}'> ${this.indexManager.getStatusName(status)}</td>` +
                    `<td class='col-action text-left'> ${actionColumn} </td></tr>`;
                }
            }
        }

        this.$tblReport.find("> tbody").empty().html(html);
        this.$(window).trigger('resize.stickyTableHeaders');
        super.bindReportEvents(rs);
    };
    viewMembersOfAgent = (custId: any, username: any) => {
        this.$viewedAgent.val(username);
        this.$username.val("");

        this.reportParam.PageIndex = 1;
        this.reportParam.PageSize = 100;

        this.statusComponent.changeSelection(siteInfrastructure.customerStatus.all);

        this.viewMembers(custId);
    };
    initAutoComplete = () => {
        let queryUsernameURL = this.usernameAutoSuggestionPath + "?" +
            this.$.param({ isDir: this.directDownline == siteInfrastructure.roleId.agent, issub: false, ismember: true });
        //initAutoComplete(this.$viewedAgent[0], queryUsernameURL);

        this.$output.css("top", this.$viewedAgent.offset().top + 30);
        this.$output.css("left", this.$viewedAgent.offset().left);
    };
    reloadState = (reportParam: any) => {
        this.$("#txt-viewed-agent").val(reportParam.DownlineUsername);
        this.$username.val(reportParam.Username);
        this.$dropdownStatus.val(this.$("#dropdown-status-member").val());
        this.reportParam.PageSize = reportParam.PageSize;
        this.reportParam.PageIndex = reportParam.PageIndex;

        this.$("#PageIndexMember").val(reportParam.PageIndex);
        this.$("#PageSizeMember").val(reportParam.PageIndex);

        this.viewMembers(reportParam.CustId);
    };
}