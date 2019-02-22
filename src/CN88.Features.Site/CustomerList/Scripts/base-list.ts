class BaseList {
    $: JQueryStatic;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;

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
    indexManager = null;
    HasUpdatePermission: boolean;
    HasAddNewPermission: boolean;

    levelName: string;
    levelNameLowerCase: string;
    renderList: Function;

    levelId: number;
    viewDownlineFunction: Function;
	addDownlineFunction: Function;
    reportColSpan: number;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        numeral: any,
        pager: PagerComponent,
        levelName: string) {
        this.$ = $;
        this.statusComponent = statusComponent;
        this.numeral = numeral;
        this.pager = pager;
        this.levelName = levelName;
        this.levelNameLowerCase = levelName.toLowerCase();
    }

    public init(
        viewResources: any,
        indexManager: any,
        renderList: Function,
        levelId: number,
        viewDownlineFunction: Function,
		addDownlineFunction: Function) {
        this.viewResources = viewResources;
        this.indexManager = indexManager;
        this.renderList = renderList;
        this.levelId = levelId;
        this.reportColSpan = levelId == siteInfrastructure.roleId.member ? 8 : 7;
        this.viewDownlineFunction = viewDownlineFunction;
		this.addDownlineFunction = addDownlineFunction;

        this.cacheDOMElements();

        this.statusComponent.render(
            new SingleSelectionDropdownOption("dropdown-status-" + this.levelNameLowerCase, "UserSelectedStatusId" + this.levelName));
        this.$dropdownStatus = this.$("#dropdown-status-" + this.levelNameLowerCase);
        this.$tblReport.stickyTableHeaders();

        var self = this;
        this.$("#btn-submit-" + this.levelName.toLowerCase()).click(function () {
            self.reportParam.PageSize = 100;
            self.reportParam.PageIndex = 1;
            self.viewList();
        });
    }

    public cacheDOMElements() {
        this.$username = this.$("#txt-username-" + this.levelNameLowerCase);
        this.getCustomerListUrl = this.$("#getCustomerListUrl").val();
		this.$tblReport = this.$("#tbl-" + this.levelNameLowerCase);
		this.$formReport = this.$("#form-" + this.levelNameLowerCase);
        this.HasUpdatePermission = this.$("#HasUpdatePermission").val() == "1";
        this.HasAddNewPermission = this.$("#HasAddNewPermission").val() == "1";
    };
    public loadData() {
        this.$.ajax({
            method: "GET",
            data: this.reportParam,
            url: this.getCustomerListUrl,
            cache: false,
            success: (rs) => {
                this.renderList(rs);
            }
        });
    }

    viewList = () => {
        this.$.blockUI();
        this.reportParam = {
            DownlineUsername: null,
            Username: this.$username.val(),
			SearchRoleId: this.levelId,
			CustId: 0,
            Status: this.$dropdownStatus.find("option:selected").val(),
            PageSize: this.reportParam.PageSize,
            PageIndex: this.reportParam.PageIndex
        };
        this.loadData();
    };
    pagingList = () => {
        this.reportParam.PageIndex = this.$("#PageIndex" + this.levelName).val();
        this.reportParam.PageSize = this.$("#PageSize" + this.levelName).val();
        this.loadData();
    };
    public bindReportEvents(rs) {
        var self = this;
        this.$tblReport.find(".view-downline").click(function () {
            var $this = self.$(this);

            self.viewDownlineFunction($this.attr("cust-id"), $this.html());
        });
        this.$tblReport.find(".col-credit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateCredit($this.attr("cust-id"), $this.attr("username"), self.levelId);
		});

		this.$tblReport.find(".col-status").click(function () {
            var $this = self.$(this);

            self.indexManager.updateStatus($this.attr("cust-id"), $this.attr("username"), $this.attr("current-status"));
        });

        if (this.HasUpdatePermission) {
            this.$tblReport.find(".icon-edit").click(function () {
                var $this = self.$(this);

                self.indexManager.updateAccount($this.attr("cust-id"), $this.attr("username"), self.levelId, self.reportParam);
			});
        }

        if (this.HasAddNewPermission) {
            this.$tblReport.find(".icon-add").click(function () {
                var $this = self.$(this);

				self.addDownlineFunction($this.attr("cust-id"), '', self.levelId - 1, self.reportParam, $this.attr("username"));
			});

			this.$formReport.find("#add-directdownline").click(function () {
				var $this = self.$(this);

				self.addDownlineFunction($this.attr("cust-id"), $this.attr("username"), self.levelId, self.reportParam, $this.attr("downline-username"));
			});
        }

        this.$tblReport.find("> tfoot").empty();
        this.$("#PageIndex" + this.levelName).remove();
        this.$("#PageSize" + this.levelName).remove();
        if (rs.TotalCustomers >= siteInfrastructure.pageList[0]) {
            var footer = "<tr>" +
                "<td colspan='" + this.reportColSpan + "'><div id='report-pager-" + this.levelNameLowerCase + "' " +
                "data-labelpage='" + this.viewResources.Page + "'" +
                "data-labelof='" + this.viewResources.Of + "'" +
                "data-labeldisplayitems='" + this.viewResources.Pager_DisplayItems + "'" +
                "data-pageindex='" + this.reportParam.PageIndex + "'" +
                "data-pagesize='" + this.reportParam.PageSize + "'" +
                "data-totalrecords='" + rs.TotalCustomers + "'" +
                "></div></td>" +
                "</tr>";

            this.$tblReport.find("> tfoot").html(footer);
            this.pager.render(new PagerOption(this.pagingList, "report-pager-" + this.levelNameLowerCase, "form-" + this.levelNameLowerCase, "PageIndex" + this.levelName, "PageSize" + this.levelName));
        }

        this.$.unblockUI();
    }
}

