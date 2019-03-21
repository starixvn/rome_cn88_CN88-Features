class MemberList extends BaseList {
    $viewedAgent: JQuery;
    $errMember: JQuery;
    directDownline: number;

    $output: JQuery;
    usernameAutoSuggestionPath: string;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        numeral: any,
        pager: PagerComponent) {
        super($, statusComponent, numeral, pager, "Member");
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

        var self = this;
        this.$("#btn-submit-member").off("click").on("click", function () {
            self.reportParam.PageSize = 100;
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

        if (this.directDownline != siteInfrastructure.roleId.agent &&
            this.reportParam.DownlineUsername == "") {
            this.$errMember.removeClass("display-none");
            this.$.unblockUI();
            return;
        }

        this.$errMember.addClass("display-none");
        this.loadData();
    };
    renderMemberList = (rs: any) => {
        if (rs.SearchAccountId > 0) {
            this.$formReport.find("#add-directdownline").attr("cust-id", rs.SearchAccountId);
            this.$formReport.find("#add-directdownline").attr("username", rs.Username);
            this.$formReport.find("#add-directdownline").attr("downline-username", rs.DownlineUsername);
        }

        if (!rs.Customers) {
			return;
		}

        this.$tblReport.removeClass("display-none");

        var html = "";
        var customerIndex = this.reportParam.PageSize * (this.reportParam.PageIndex - 1) + 1;

        if (rs.Customers.length == 0) {
            html = "<tr><td colspan='9' align='center' class='no-info'>" + this.viewResources.NoInformationIsAvailable + "</td></tr>";
        } else {
            let disabledTooltip = `disabled title='${this.indexManager.viewResources.Disabled}'`,              
                editMemberTooltip = ` title='${this.indexManager.viewResources.EditMember}'`;
            for (var i = 0; i < rs.Customers.length; i++ , customerIndex++) {
                let customer = rs.Customers[i],
                    custId = customer.CustId,
                    username = customer.Username;

                html += `<tr class='row-customer ${this.indexManager.getCustomerColor(customer)}' cust-id='${custId}'>` +
                    "<td class='col-no'>" + customerIndex + "</td>" +
                    `<td class='link view-statement' username='${username}' cust-id='${custId}'>${username}</td>` +
                    "<td>" + customer.Name + "</td>" +
                    "<td>" + customer.Nickname + "</td>" +
                    "<td>" + customer.Group + "</td>" +
                    `<td class='text-right link col-credit' cust-id='${custId}' username='${username}'>` + this.numeral(customer.Credit).format("###,0.00") + "</td>" +
                    "<td class='col-date'>" + customer.CreationDateStr + "</td>" +
                    `<td class='col-status text-center link' cust-id='${custId}' username='${username}' current-status='` + customer.Status + "'>" + this.indexManager.getStatusName(customer.Status) + "</td>" +
                    "<td class='col-action text-center'>" +
                    `<span class='icon-pass' data-toggle='tooltip' data-placement='top' title='${this.indexManager.viewResources.ResetPassword}' cust-id='${custId}' username='${username}' cust-role='3'></span>` +
                    `<span class='icon-edit' data-toggle='tooltip' data-placement='top' cust-id=${custId} username=${username} ${this.HasUpdatePermission ? `${editMemberTooltip}` : `${disabledTooltip}`}></span>` +
                    "</td>" +
                    "</tr>";
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
        var queryUsernameURL = this.usernameAutoSuggestionPath + "?" +
            this.$.param({ isDir: this.directDownline == siteInfrastructure.roleId.agent, issub: false, ismember: true });
        initAutoComplete(this.$viewedAgent[0], queryUsernameURL);

        this.$output.css("top", this.$viewedAgent.offset().top + 30);
        this.$output.css("left", this.$viewedAgent.offset().left);
    };
    reloadState = (reportParam: any) => {
        this.$("#txt-viewed-agent").val(reportParam.DownlineUsername);
        this.$username.val(reportParam.Username);
        this.$dropdownStatus.val(reportParam.Status);
        this.reportParam.PageSize = reportParam.PageSize;
        this.reportParam.PageIndex = reportParam.PageIndex;

        this.$("#PageIndexMember").val(reportParam.PageIndex);
        this.$("#PageSizeMember").val(reportParam.PageIndex);

		this.viewMembers(reportParam.CustId);
    };
}

