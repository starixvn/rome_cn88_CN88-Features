class MasterList extends BaseList {
    searchSuggestionMaster: JQuery;
    usernameAutoSuggestionPath: string;
    $output: JQuery;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        numeral: any,
        pager: PagerComponent) {
        super($, statusComponent, numeral, pager, "Master");
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
                    `<span class='icon-pass' data-toggle='tooltip' data-placement='top' title='${this.indexManager.viewResources.ResetPassword}' cust-id='${custId}' username='${username}' cust-role='3'></span>` +
                    `<span class='icon-security' data-toggle='tooltip' data-placement='top' title='${this.indexManager.viewResources.ResetSecurityCode}' cust-id='${custId}' username='${username}'></span>` +
                    `<span class='icon-edit' data-toggle='tooltip' data-placement='top' cust-id=${custId} username=${username} title=${this.indexManager.viewResources.EditMasterAgent}></span>` +
                    `<span class='icon-add' data-toggle='tooltip' data-placement='top' cust-id=${custId} username=${username} title=${this.indexManager.viewResources.NewAgent}></span>` +
                    "</td>" +
                    "</tr>";
            }
        }

        this.$tblReport.find("> tbody").empty().html(html);
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
        initAutoComplete(this.searchSuggestionMaster[0], queryUsernameURL);
        this.$output.css("top", this.searchSuggestionMaster.offset().top + 30);
        this.$output.css("left", this.searchSuggestionMaster.offset().left);
    };
}