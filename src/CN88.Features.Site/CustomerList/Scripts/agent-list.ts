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
            self.reportParam.PageSize = 100;
			self.reportParam.PageIndex = 1;
			self.viewAgents(self.reportParam.CustId);
		});

        if (this.directDownline >= siteInfrastructure.roleId.master) {
            this.initAutoComplete();
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
            this.reportParam.DownlineUsername == "") {
            this.$errAgent.removeClass("display-none");
            this.$.unblockUI();
            return;
        }

        this.$errAgent.addClass("display-none");
        this.loadData();
    };
    renderAgentList = (rs: any) => {
        if (!rs.Customers) {
            return;
        }

		this.$formReport.find("#add-directdownline").attr("cust-id", rs.SearchAccountId);
		this.$formReport.find("#add-directdownline").attr("username", rs.Username);
		this.$formReport.find("#add-directdownline").attr("downline-username", rs.DownlineUsername);
		
        this.$tblReport.removeClass("display-none");

        var html = "";
        var customerIndex = this.reportParam.PageSize * (this.reportParam.PageIndex - 1) + 1;

        if (rs.Customers.length == 0) {
            html = "<tr><td colspan='7' align='center' class='no-info'>" + this.viewResources.NoInformationIsAvailable + "</td></tr>";
        } else {
            for (var i = 0; i < rs.Customers.length; i++ , customerIndex++) {
                var customer = rs.Customers[i];
                html += "<tr class='row-customer " + this.indexManager.getCustomerColor(customer) + "' cust-id='" + customer.CustId + "'>" +
                    "<td class='text-center'>" + customerIndex + "</td>" +
                    "<td class='link view-downline' cust-id='" + customer.CustId + "'>" + customer.Username + "</td>" +
                    "<td>" + customer.Name + "</td>" +
                    "<td class='text-right col-credit link' cust-id='" + customer.CustId + "' username='" + customer.Username + "'>" + this.numeral(customer.Credit).format("###,0.00") + "</td>" +
                    "<td class='text-center'>" + customer.CreationDateStr + "</td>" +
                    "<td class='text-center col-status link' cust-id='" + customer.CustId + "' username='" + customer.Username + "' current-status='" + customer.Status + "'>" + this.indexManager.getStatusName(customer.Status) + "</td>" +
                    "<td class='text-center'>" +
                    "<span class='icon-edit' cust-id='" + customer.CustId + "' username='" + customer.Username + "' " + (this.HasUpdatePermission ? "" : "disabled='disabled' title='" + this.indexManager.viewResources.Disabled + "'") + "></span>" +
                    "<span class='icon-add' cust-id='" + customer.CustId + "' username='" + customer.Username + "' " + (this.HasAddNewPermission ? "" : "disabled='disabled' title='" + this.indexManager.viewResources.Disabled + "'") + "></span>" +
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
        var queryUsernameURL = this.usernameAutoSuggestionPath + "?" +
            this.$.param({ isDir: 1, issub: 0, ismember: 0 });
        initAutoComplete(this.$viewedMaster[0], queryUsernameURL);
        this.$output.css("top", this.$viewedMaster.offset().top + 30);
        this.$output.css("left", this.$viewedMaster.offset().left);
    };
    reloadState = (reportParam: any) => {
        this.$("#txt-viewed-master").val(reportParam.DownlineUsername);
        this.$username.val(reportParam.Username);
        this.$dropdownStatus.val(reportParam.Status);
        this.reportParam.PageSize = reportParam.PageSize;
        this.reportParam.PageIndex = reportParam.PageIndex;

        this.$("#PageIndexAgent").val(reportParam.PageIndex);
        this.$("#PageSizeAgent").val(reportParam.PageIndex);

		this.viewAgents(reportParam.CustId);
    };
}

