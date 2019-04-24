declare var initAutoComplete: any;

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
			self.viewAgents(self.reportParam.CustId);
		});

        if (this.directDownline >= siteInfrastructure.roleId.master) {
            //this.initAutoComplete();
        }
    }

    public cacheDOMElements() {
        super.cacheDOMElements();

		this.$viewedMaster = this.$("#txt-viewed-master");
        this.directDownline = parseInt(this.$("#directDownline").val());
        this.$errAgent = this.$("#msg-err-agent");
        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
	}
	viewAgents = (custId: number) => {
        this.$.blockUI();
        this.reportParam = {
            DownlineUsername: this.$("#txt-viewed-master").val(),
            Username: this.$username.val(),
			SearchRoleId: siteInfrastructure.roleId.agent,
			CustId: custId,
            Status: this.$dropdownStatus.find("option:selected").val(),
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
            this.$formReport.find("#add-directdownline").attr("cust-id", rs.SearchAccountId);
            this.$formReport.find("#add-directdownline").attr("username", rs.Username);
            this.$formReport.find("#add-directdownline").attr("downline-username", rs.DownlineUsername);
        }

        if (!rs.Customers) {
            return;
        }
	
        this.$tblReport.removeClass("display-none");

        let html = "";
        let customerIndex = this.reportParam.PageSize * (this.reportParam.PageIndex - 1) + 1;

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

