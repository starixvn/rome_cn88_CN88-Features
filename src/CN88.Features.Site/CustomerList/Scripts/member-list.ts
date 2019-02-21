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
            self.viewMembers(0);
        });
        if (this.directDownline >= siteInfrastructure.roleId.agent) {
            this.initAutoComplete();
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
        if (!rs.Customers) {
            return;
        }

		this.$formReport.find("#add-directdownline").attr("cust-id", rs.SearchAccountId);
        this.$tblReport.removeClass("display-none");

        var html = "";
        var customerIndex = this.reportParam.PageSize * (this.reportParam.PageIndex - 1) + 1;

        if (rs.Customers.length == 0) {
            html = "<tr><td colspan='8' align='center' class='no-info'>" + this.viewResources.NoInformationIsAvailable + "</td></tr>";
        } else {
            for (var i = 0; i < rs.Customers.length; i++ , customerIndex++) {
                var customer = rs.Customers[i];
                html += "<tr class='row-customer " + this.indexManager.getCustomerColor(customer) + "' cust-id='" + customer.CustId + "'>" +
                    "<td class='text-center'>" + customerIndex + "</td>" +
                    "<td>" + customer.Username + "</td>" +
                    "<td>" + customer.Name + "</td>" +
                    "<td class='text-center'>" + customer.Group + "</td>" +
                    "<td class='text-right col-credit link' cust-id='" + customer.CustId + "' username='" + customer.Username + "'>" + this.numeral(customer.Credit).format("###,0.00") + "</td>" +
                    "<td class='text-center'>" + customer.CreationDateStr + "</td>" +
                    "<td class='text-center col-status link' cust-id='" + customer.CustId + "' username='" + customer.Username + "' current-status='" + customer.Status + "'>" + this.indexManager.getStatusName(customer.Status) + "</td>" +
                    "<td class='text-center'>" +
                    "<span class='icon-edit' cust-id='" + customer.CustId + "' username='" + customer.Username + "' " + (this.HasUpdatePermission ? "" : "disabled='disabled' title='" + this.indexManager.viewResources.Disabled + "'") + "></span>" +
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
            this.$.param({ isDir: this.directDownline == siteInfrastructure.roleId.agent ? 1 : 0, issub: 0, ismember: 1 });
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

