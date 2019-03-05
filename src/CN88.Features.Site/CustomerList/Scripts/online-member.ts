class OnlineMember {
    $: JQueryStatic;
    pager: PagerComponent;

	$tblReport: JQuery;
    getCustomerListUrl: string;

    reportParam = {      
        PageSize: 100,
        PageIndex: 1
    };

    viewResources: any;
   
    renderList: Function;    

    constructor(
        $: JQueryStatic,
        pager: PagerComponent) {
        this.$ = $;
        this.pager = pager;
    }

    public init(
        viewResources: any,
        renderList: Function) {
        this.viewResources = viewResources;
        this.renderList = renderList;
        this.cacheDOMElements();
        this.$tblReport.stickyTableHeaders();      
        var self = this;
    }

    public cacheDOMElements() {
		this.$tblReport = this.$("#tblReport");
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
            PageSize: this.reportParam.PageSize,
            PageIndex: this.reportParam.PageIndex
        };
        this.loadData();
    };
    pagingList = () => {
        this.reportParam.PageIndex = this.$("#PageIndex").val();
        this.reportParam.PageSize = this.$("#PageSize").val();
        this.loadData();
    };
    public bindReportEvents(rs) {
        var self = this;
        
        this.$tblReport.find("> tfoot").empty();
        this.$("#PageIndex").remove();
        this.$("#PageSize").remove();
        if (rs.TotalCustomers >= siteInfrastructure.pageList[0]) {
            var footer = "<tr>" +
                "<td colspan='3'><div id='report-pager'"+
                "data-labelpage='" + this.viewResources.Page + "'" +
                "data-labelof='" + this.viewResources.Of + "'" +
                "data-labeldisplayitems='" + this.viewResources.Pager_DisplayItems + "'" +
                "data-pageindex='" + this.reportParam.PageIndex + "'" +
                "data-pagesize='" + this.reportParam.PageSize + "'" +
                "data-totalrecords='" + rs.TotalCustomers + "'" +
                "></div></td>" +
                "</tr>";

            this.$tblReport.find("> tfoot").html(footer);
            this.pager.render(new PagerOption(this.pagingList, "report-pager", "form", "PageIndex", "PageSize"));
        }
        this.$.unblockUI();
    }
}

