/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="base-list.ts" />

declare var initAutoComplete: Function;

class AgentList extends BaseList {
    $viewedMaster: JQuery;
    $errAgent: JQuery;
    directDownline: number;

    $output: JQuery;
    usernameAutoSuggestionPath: string;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        numeral: any,
        pager: PagerComponent) {
        super($, statusComponent, numeral, pager, "Agent");
    }

    public init(viewResources: any, indexManager: any) {
        super.init(
            viewResources,
            indexManager,
            this.renderAgentList,
            siteInfrastructure.roleId.agent,
            indexManager.viewAgent,
            indexManager.addNewDownline);

        this.cacheDOMElements();

        var self = this;
        this.$("#btn-submit-agent").off("click").on("click", function () {
            self.reportParam.PageIndex = 1;
            if (self.indexManager.isDevice) {
                self.indexManager.agentToggleHandler.hideFilter();
            }

            self.viewAgents(self.reportParam.CustId);
        });

        if (this.directDownline >= siteInfrastructure.roleId.master) {
            //this.initAutoComplete();
        }
    }

    public cacheDOMElements() {
        super.cacheDOMElements();

        this.$viewedMaster = this.$("#txt-viewed-master");
        this.directDownline = parseInt(<string>this.$("#directDownline").val());
        this.$errAgent = this.$("#msg-err-agent");
        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    }
    viewAgents = (custId: number) => {
        this.$.blockUI();
        this.reportParam = {
            DownlineUsername: this.$("#txt-viewed-master").val(),
            Username: <string>this.$username.val(),
            SearchRoleId: siteInfrastructure.roleId.agent,
            CustId: custId,
            Status: <number>this.$dropdownStatus.find("option:selected").val(),
            PageSize: this.reportParam.PageSize,
            PageIndex: this.reportParam.PageIndex
        };

        if (this.directDownline != siteInfrastructure.roleId.agent &&
            (this.reportParam.DownlineUsername == "" ||
                this.reportParam.DownlineUsername == undefined)) {
            this.$errAgent.removeClass("display-none");
            this.$.unblockUI();
            return;
        }

        this.$errAgent.addClass("display-none");
        this.loadData();
    };
    renderAgentList = (rs: any) => {
        if (rs.SearchAccountId > 0) {
            let buttonAddNew = this.$(".add-directdownline-agent");
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
                html = "<tr><td colspan='4' align='center' class='no-info'>" + this.viewResources.NoInformationIsAvailable + "</td></tr>";
            } else {
                for (let i = 0; i < rs.Customers.length; i++ , customerIndex++) {
                    let customer = rs.Customers[i],
                        custId = customer.CustId,
                        username = customer.Username;

                    html += `<tr class='row-customer ${this.indexManager.getCustomerColor(customer)}' cust-id='${custId}'>` +
                        `<td class='link view-downline' cust-id='${custId}' username='${username}'><div class='username-wrapper'>${username}/ <br /> <span class="cust-name">${customer.Nickname}</span></div></td>` +
                        `<td class='col-status text-center link' cust-id='${custId}' username='${username}' current-status='` + customer.Status + "'>" + this.indexManager.getStatusName(customer.Status) + "</td>" +
                        `<td class='text-right link col-credit' cust-id='${custId}' username='${username}'>` + this.numeral(customer.Credit).format("###,0.00") + "</td>" +
                        "<td class='col-action text-center'>" +
                        `<div class="dropdown">` +
                        `<a id="${custId}" type = "button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >` +
                        (username.Closed ? `<span class="icon-user-unlock"><span class="path1"></span><span class="path2"></span></span>` : `<span class="icon-more-action"></span>`) +
                        `</a>` +
                        `<ul class="dropdown-menu" aria-labelledby="${custId}" >` +
                        `<li class='icon-pass' cust-id='${custId}' username='${username}' cust-role='2'>${this.indexManager.viewResources.ResetPassword}</li>` +
                        `<li class='icon-security' cust-id='${custId}' username='${username}'>${this.indexManager.viewResources.ResetSecurityCode}</li>` +
                        `<li class='icon-edit' cust-id='${custId}' username='${username}'>${this.indexManager.viewResources.EditAgent}</li>` +
                        `<li class='icon-add' cust-id='${custId}' username='${username}'>${this.indexManager.viewResources.NewMember}</li>` +
                        `</ul>` +
                        `</div>` +
                        "</td>" +
                        "</tr>";
                }
            }
        } else {
            if (rs.Customers.length == 0) {
                html = "<tr><td colspan='8' align='center' class='no-info'>" + this.viewResources.NoInformationIsAvailable + "</td></tr>";
            } else {
                for (let i = 0; i < rs.Customers.length; i++ , customerIndex++) {
                    let customer = rs.Customers[i],
                        custId = customer.CustId,
                        username = customer.Username;

                    html += `<tr class='row-customer ${this.indexManager.getCustomerColor(customer)}' cust-id='${custId}'>` +
                        "<td class='col-no'>" + customerIndex + "</td>" +
                        `<td class='link view-downline' cust-id='${custId}'>${username}</td>` +
                        "<td>" + customer.Name + "</td>" +
                        "<td>" + customer.Nickname + "</td>" +
                        `<td class='text-right link col-credit' cust-id='${custId}' username='${username}'>` + this.numeral(customer.Credit).format("###,0.00") + "</td>" +
                        "<td class='col-date'>" + customer.CreationDateStr + "</td>" +
                        `<td class='col-status text-center link' cust-id='${custId}' username='${username}' current-status='` + customer.Status + "'>" + this.indexManager.getStatusName(customer.Status) + "</td>" +
                        "<td class='col-action text-center'>" +
                        `<span class='icon-pass' data-toggle='tooltip' data-placement='top' title='${this.indexManager.viewResources.ResetPassword}' cust-id='${custId}' username='${username}' cust-role='2'></span>` +
                        `<span class='icon-security' data-toggle='tooltip' data-placement='top' title='${this.indexManager.viewResources.ResetSecurityCode}' cust-id='${custId}' username='${username}'></span>` +
                        `<span class='icon-edit' data-toggle='tooltip' data-placement='top' cust-id='${custId}' username='${username}' title='${this.indexManager.viewResources.EditAgent}'></span>` +
                        `<span class='icon-add' data-toggle='tooltip' data-placement='top' cust-id='${custId}' username='${username}' title='${this.indexManager.viewResources.NewMember}'></span>` +
                        "</td>" +
                        "</tr>";
                }
            }
        }


        this.$tblReport.find("> tbody").empty().html(html);
        this.$(window).trigger('resize.stickyTableHeaders');
        super.bindReportEvents(rs);
    };
    viewAgentsOfMaster = (custId: any, username: any) => {
        this.$viewedMaster.val(username);
        this.$username.val("");

        this.reportParam.PageIndex = 1;
        this.reportParam.PageSize = 100;

        this.statusComponent.changeSelection(siteInfrastructure.customerStatus.all);

        this.viewAgents(custId);
    };
    initAutoComplete = () => {
        let queryUsernameURL = this.usernameAutoSuggestionPath + "?" +
            this.$.param({ isDir: true, issub: false, ismember: false });
        initAutoComplete(this.$viewedMaster[0], queryUsernameURL);
        this.$output.css("top", this.$viewedMaster.offset().top + 30);
        this.$output.css("left", this.$viewedMaster.offset().left);
    };
    reloadState = (reportParam: any) => {
        this.$("#txt-viewed-master").val(reportParam.DownlineUsername);
        this.$username.val(reportParam.Username);
        this.$dropdownStatus.val(this.$("#dropdown-status-agent").val());
        this.reportParam.PageSize = reportParam.PageSize;
        this.reportParam.PageIndex = reportParam.PageIndex;

        this.$("#PageIndexAgent").val(reportParam.PageIndex);
        this.$("#PageSizeAgent").val(reportParam.PageIndex);

        this.viewAgents(reportParam.CustId);
    };
}
