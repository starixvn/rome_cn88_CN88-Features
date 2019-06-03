class BaseList {
    $: JQueryStatic;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;

    $tooltip: JQuery;
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

    levelName: string;
    levelNameLowerCase: string;
    renderList: Function;

    levelId: number;
    viewDownlineFunction: Function;
    addDownlineFunction: Function;
    reportColSpan: number;
    minPageSize = 10;

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
        this.reportColSpan = levelId === siteInfrastructure.roleId.member ? 9 : 8;
        this.viewDownlineFunction = viewDownlineFunction;
        this.addDownlineFunction = addDownlineFunction;

        this.cacheDOMElements();

        this.statusComponent.render(
            new SingleSelectionDropdownOption("dropdown-status-" + this.levelNameLowerCase, "UserSelectedStatusId" + this.levelName));
        this.$dropdownStatus = this.$("#dropdown-status-" + this.levelNameLowerCase);
        this.$tblReport.stickyTableHeaders();
        this.$("#btn-submit-" + this.levelName.toLowerCase()).click(() => {
            this.reportParam.PageIndex = 1;

            if (this.indexManager.isDevice) {
                this.indexManager.masterToggleHandler.hideFilter();
            }

            this.viewList();
        });
    }

    public cacheDOMElements() {
        this.$username = this.$("#txt-username-" + this.levelNameLowerCase);
        this.getCustomerListUrl = this.$("#getCustomerListUrl").val();
        this.$tblReport = this.$("#tbl-" + this.levelNameLowerCase);
        this.$formReport = this.$("#form-" + this.levelNameLowerCase);
    }

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

    initTooltip = () => {
        this.$('[data-toggle="tooltip"]').tooltip();
    };

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
        this.$tblReport.find(".view-statement").click((e) => {
            const $this = this.$(e.currentTarget);
            const viewStatementUrl = `${this.$("#viewStatementtUrl").val()}?CustName=${$this.attr("username")}&CustId=${$this.attr("cust-id")}`;

            if (this.indexManager.isDevice) {
                window.open(viewStatementUrl, "_blank");
            } else {
                window.open(viewStatementUrl, "MemberStatement", "scrollbars=1,height=500,width=1000");
            }
        });

        this.$tblReport.find(".view-downline").click((e) => {
            const $this = this.$(e.currentTarget);
            if (this.indexManager.isDevice) {
                this.viewDownlineFunction($this.attr("cust-id"), $this.attr("username"));
            } else {
                this.viewDownlineFunction($this.attr("cust-id"), $this.html());
            }
        });

        this.$tblReport.find(".col-credit").click((e) => {
            const $this = this.$(e.currentTarget);

            this.indexManager.updateCredit($this.attr("cust-id"), $this.attr("username"), this.levelId);
        });

        this.$tblReport.find(".col-status").click((e) => {
            const $this = this.$(e.currentTarget);

            this.indexManager.updateStatus($this.attr("cust-id"), $this.attr("username"), $this.attr("current-status"));
        });

        this.$tblReport.find(".icon-pass").click((e) => {
            const $this = this.$(e.currentTarget);

            this.indexManager.resetPassword($this.attr("cust-id"), $this.attr("username"), $this.attr("cust-role"));
        });

        this.$tblReport.find(".icon-security").click((e) => {
            const $this = this.$(e.currentTarget);

            this.indexManager.resetSecurityCode($this.attr("cust-id"), $this.attr("username"));
        });

        this.$tblReport.find(".icon-edit").click((e) => {
            const $this = this.$(e.currentTarget);

            this.indexManager.updateAccount($this.attr("cust-id"), $this.attr("username"), this.levelId, this.reportParam);
        });

        this.$tblReport.find(".icon-add").click((e) => {
            const $this = this.$(e.currentTarget);

            this.addDownlineFunction($this.attr("cust-id"), '', this.levelId - 1, this.reportParam, $this.attr("username"));
        });

        const buttonAddNew = this.$(".add-directdownline-" + this.levelNameLowerCase);
        if (rs.SearchAccountId > 0) {
            buttonAddNew.removeAttr("disabled");
            buttonAddNew.click((e) => {
                const $this = this.$(e.currentTarget);

                this.addDownlineFunction($this.attr("cust-id"), $this.attr("username"), this.levelId, this.reportParam, $this.attr("downline-username"));
            });
        }
        else {
            buttonAddNew.attr("disabled", "disabled");
        }

        this.$tblReport.find("> tfoot").empty();
        this.$("#PageIndex" + this.levelName).remove();
        this.$("#PageSize" + this.levelName).remove();
        if (rs.TotalCustomers >= this.minPageSize) {
            const footer = `<div id='report-pager-${this.levelNameLowerCase}'` +
                `data-labelpage='${this.viewResources.Page}'` +
                `data-labelof='${this.viewResources.Of}'` +
                `data-labeldisplayitems='${this.viewResources.Pager_DisplayItems}'` +
                `data-pageindex='${this.reportParam.PageIndex}'` +
                `data-pagesize='${this.reportParam.PageSize}'` +
                `data-totalrecords='${rs.TotalCustomers}'></div>`;

            this.$("#pager-" + this.levelNameLowerCase).html(footer);
            this.pager.render(new PagerOption(
                this.pagingList,
                "report-pager-" + this.levelNameLowerCase,
                "form-" + this.levelNameLowerCase,
                "PageIndex" + this.levelName,
                "PageSize" + this.levelName));
        } else {
            this.$("#pager-" + this.levelNameLowerCase).html("");
        }

        this.initTooltip();
        this.$.unblockUI();
    }
}