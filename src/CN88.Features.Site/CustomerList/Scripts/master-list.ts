class MasterList extends BaseList {
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
	    $('[data-toggle="tooltip"]').tooltip();

    }

    renderMasterList = (rs: any) => {
        this.$tblReport.removeClass("display-none");

        var html = "";
        var customerIndex = this.reportParam.PageSize * (this.reportParam.PageIndex - 1) + 1;

        if (rs.Customers.length == 0) {
            html = "<tr><td colspan='7' align='center' class='no-info'>" + this.viewResources.NoInformationIsAvailable + "</td></tr>";
        } else {
            for (var i = 0; i < rs.Customers.length; i++ , customerIndex++) {
				let customer = rs.Customers[i],
					custId = customer.CustId,
					username = customer.Username,
					disabledTooltip = `disabled title=${this.indexManager.viewResources.Disabled}`;

                html += "<tr class='row-customer " + this.indexManager.getCustomerColor(customer) + "' cust-id='" + customer.CustId + "'>" +
                    "<td class='col-no'>" + customerIndex + "</td>" +
                    "<td class='link view-downline' cust-id='" + customer.CustId + "'>" + customer.Username + "</td>" +
                    "<td>" + customer.Name + "</td>" +
                    "<td class='text-right link col-credit' cust-id='" + customer.CustId + "' username='" + customer.Username + "'>" + this.numeral(customer.Credit).format("###,0.00") + "</td>" +
                    "<td class='col-date'>" + customer.CreationDateStr + "</td>" +
                    "<td class='col-status link' cust-id='" + customer.CustId + "' username='" + customer.Username + "' current-status='" + customer.Status + "'>" + this.indexManager.getStatusName(customer.Status) + "</td>" +
                    "<td class='col-action'>" +
                    "<span class='icon-pass' data-toggle='tooltip' data-placement='left' title='Reset Password'></span>" +
                    "<span class='icon-security' data-toggle='tooltip' data-placement='left' title='Reset Security Code'></span>" +
		    `<span class='icon-edit' cust-id=${custId} username=${username} ${this.HasUpdatePermission ? '' : `${disabledTooltip}`}></span>` +
		    `<span class='icon-add' cust-id=${custId} username=${username} ${this.HasAddNewPermission ? '' : `${disabledTooltip}`}></span>` +
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
        this.$dropdownStatus.val(reportParam.Status);
        this.reportParam.PageSize = reportParam.PageSize;
        this.reportParam.PageIndex = reportParam.PageIndex;

        this.$("#PageIndexMaster").val(reportParam.PageIndex);
        this.$("#PageSizeMaster").val(reportParam.PageIndex);

        this.viewList();
    };
}

