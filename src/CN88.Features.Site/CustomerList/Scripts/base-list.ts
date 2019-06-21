/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="viewmodel/view-model.ts" />
/// <reference path="../../shared/scripts/components/html-wrapper.ts" />

class BaseList {
    $: JQueryStatic;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;
    bootstrapDropdownComponent: BootstrapDropdownComponent;

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

    viewResources: ViewResources;
    indexManager: any = null;

    levelName: string;
    levelNameLowerCase: string;

    levelId: number;
    viewDownlineFunction: Function;
    minPageSize: number = 10;
    viewerStatusId: number;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        bootstrapDropdownComponent: BootstrapDropdownComponent,
        numeral: any,
        pager: PagerComponent,
        levelName: string) {
        this.$ = $;
        this.statusComponent = statusComponent;
        this.bootstrapDropdownComponent = bootstrapDropdownComponent;
        this.numeral = numeral;
        this.pager = pager;
        this.levelName = levelName;
        this.levelNameLowerCase = levelName.toLowerCase();
    }

    public init(
        indexManager: CustomerList,
        levelId: number,
        viewDownlineFunction: Function) {
        this.indexManager = indexManager;
        this.viewResources = indexManager.viewResources;
        this.levelId = levelId;
        this.viewDownlineFunction = viewDownlineFunction;
        this.viewerStatusId = parseInt(this.$("#ViewerStatusId").val());
        this.cacheDOMElements();

        this.statusComponent.render(
            new SingleSelectionDropdownOption(`dropdown-status-${this.levelNameLowerCase}`, `UserSelectedStatusId${this.levelName}`));
        this.$dropdownStatus = this.$(`#dropdown-status-${this.levelNameLowerCase}`);
        this.$tblReport.stickyTableHeaders();
    }

    public cacheDOMElements() {
        this.$username = this.$(`#txt-username-${this.levelNameLowerCase}`);
        this.getCustomerListUrl = this.$("#getCustomerListUrl").val();
        this.$tblReport = this.$(`#tbl-${this.levelNameLowerCase}`);
        this.$formReport = this.$(`#form-${this.levelNameLowerCase}`);
    };

    loadData() {
        this.$.ajax({
            method: "GET",
            data: this.reportParam,
            url: this.getCustomerListUrl,
            cache: false,
            success: (rs) => {
                this.renderCustomerList(rs);
            }
        });
    }

    initTooltip = () => {
        this.$('[data-toggle="tooltip"]').tooltip();
    };

    pagingList = () => {
        this.reportParam.PageIndex = this.$(`#PageIndex${this.levelName}`).val();
        this.reportParam.PageSize = this.$(`#PageSize${this.levelName}`).val();
        this.loadData();
    };

    bindReportEvents(rs) {
        var self = this;
        this.$tblReport.find(".view-statement").click(function () {
            var $this = self.$(this);
            var viewStatementUrl = `${$("#viewStatementtUrl").val()}?CustName=${$this.attr("username")}&CustId=${$this.attr("cust-id")}`;
            self.indexManager.isDevice ?
                window.open(viewStatementUrl, "_blank") :
                window.open(viewStatementUrl, "MemberStatement", "scrollbars=1,height=500,width=1000");
        });

        this.$tblReport.find(".view-downline").click(function () {
            var $this = self.$(this);
            self.indexManager.isDevice ?
                self.viewDownlineFunction($this.attr("cust-id"), $this.attr("username"), self.reportParam.Status) :
                self.viewDownlineFunction($this.attr("cust-id"), $this.html(), self.reportParam.Status);
        });

        this.$tblReport.find(".col-credit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateCredit($this.attr("cust-id"), $this.attr("username"), self.levelId);
        });

        this.$tblReport.find(".col-status").click(function () {
            var $this = self.$(this);

            self.indexManager.updateStatus($this.attr("cust-id"), $this.attr("username"), $this.attr("current-status"), $this.attr("direct-upline-status"));
        });

        this.$tblReport.find(".icon-pass").click(function () {
            var $this = self.$(this);

            self.indexManager.resetPassword($this.attr("cust-id"), $this.attr("username"), $this.attr("cust-role"));
        });

        this.$tblReport.find(".icon-security").click(function () {
            var $this = self.$(this);

            self.indexManager.resetSecurityCode($this.attr("cust-id"), $this.attr("username"));
        });

        this.$tblReport.find(".icon-edit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateAccount($this.attr("cust-id"), $this.attr("username"), self.levelId, self.reportParam);
        });

        this.$tblReport.find(".icon-add").click(function () {
            var $this = self.$(this);
            self.indexManager.addNewDownline($this.attr("cust-id"), '', self.levelId - 1, self.reportParam, $this.attr("username"));
        });

        var buttonAddNew = this.$(`.add-directdownline-${this.levelNameLowerCase}`);
        this.$tblReport.find(".icon-unlock").click(function () {
            var $this = self.$(this);

            self.indexManager.updateUnlock($this.attr("cust-id"));
        });

        if (rs.SearchAccountId > 0 && parseInt(rs.SearchAccountStatus) === siteInfrastructure.customerStatus.active) {
            buttonAddNew.removeAttr("disabled");
            buttonAddNew.click(function () {
                var $this = self.$(this);

                self.indexManager.addNewDownline($this.attr("cust-id"), $this.attr("username"), self.levelId, self.reportParam, $this.attr("downline-username"));
            });
        }
        else {
            buttonAddNew.attr("disabled", "disabled");
        }

        const pageIndex = `PageIndex${this.levelName}`, pageSize = `PageSize${this.levelName}`;
        this.$tblReport.find("> tfoot").empty();
        this.$(`#${pageIndex}`).remove();
        this.$(`#${pageSize}`).remove();

        const pagerElement = this.$(`#pager-${this.levelNameLowerCase}`);
        if (rs.TotalCustomers >= this.minPageSize && pagerElement.length > 0) {
            var footer = `<div id='report-pager-${this.levelNameLowerCase}'` +
                `data-labelpage='${this.viewResources.Page}'` +
                `data-labelof='${this.viewResources.Of}'` +
                `data-labeldisplayitems='${this.viewResources.Pager_DisplayItems}'` +
                `data-pageindex='${this.reportParam.PageIndex}'` +
                `data-pagesize='${this.reportParam.PageSize}'` +
                `data-totalrecords='${rs.TotalCustomers}'></div>`;

            pagerElement.html(footer);
            this.pager.render(new PagerOption(this.pagingList, `report-pager-${this.levelNameLowerCase}`, `form-${this.levelNameLowerCase}`, pageIndex, pageSize));
        } else {
            pagerElement.html("");
        }

        this.bootstrapDropdownComponent.render(new BootstrapDropdownOption(".tbl-downlinelist .dropdown"));

        this.initTooltip();
        this.$.unblockUI();
    };

    renderCustomerList(rs: any) {
        if (rs.SearchAccountId > 0) {
            var buttonAddNew = this.$(`.add-directdownline-${this.levelNameLowerCase}`);
            buttonAddNew.attr("cust-id", rs.SearchAccountId);
            buttonAddNew.attr("username", rs.Username);
            buttonAddNew.attr("downline-username", rs.DownlineUsername);
        }

        if (!rs.Customers) {
            return;
        }

        this.$tblReport.removeClass("display-none");

        this.$tblReport.find("> tbody").empty().html(
            this.indexManager.isDevice ? this.buildBodyOnMobileMode(rs) : this.buildBodyOnDesktopMode(rs)
        );
        this.$(window).trigger('resize.stickyTableHeaders');
        this.bindReportEvents(rs);
    };

    buildBodyOnDesktopMode(rs: any) {
        const { agent, member } = siteInfrastructure.roleId, isMember = this.levelId === member;
        if (rs.Customers.length == 0) {
            return this.buildNoInfoContent(isMember ? 9 : 8);
        }

        let html = "";
        let customerIndex = this.reportParam.PageSize * (this.reportParam.PageIndex - 1) + 1;
        const { viewerStatusId } = this,
            { active } = siteInfrastructure.customerStatus,
            isActivedViewer = viewerStatusId === active;

        for (let i = 0; i < rs.Customers.length; i++ , customerIndex++) {
            const { CustId, Username, Status, Credit, Closed, Group, Name, Nickname, CreationDateStr } = rs.Customers[i];

            const commonAttributesJson = [
                {
                    Name: 'cust-id',
                    Value: CustId
                },
                {
                    Name: 'username',
                    Value: Username
                },
            ];

            let attributesJson = [
                {
                    Content: customerIndex.toString(),
                    Attributes: [{
                        Name: 'class',
                        Value: 'col-no'
                    }],
                    Visibile: true
                },
                {
                    Content: Username,
                    Attributes: [{
                        Name: 'class',
                        Value: `link ${isMember ? 'view-statement' : 'view-downline'}`
                    }],
                    Visibile: true
                },
                {
                    Content: Name,
                    Attributes: [],
                    Visibile: true
                },
                {
                    Content: Nickname,
                    Attributes: [],
                    Visibile: true
                },
                {
                    Content: Group,
                    Attributes: [],
                    Visibile: isMember
                },
                {
                    Content: this.numeral(Credit).format("###,0.00"),
                    Attributes: [{
                        Name: 'class',
                        Value: `text-right ${isActivedViewer ? 'link col-credit' : ''}`
                    }].concat(isActivedViewer ? commonAttributesJson : []),
                    Visibile: true
                },
                {
                    Content: CreationDateStr,
                    Attributes: [{
                        Name: 'class',
                        Value: 'col-date'
                    }],
                    Visibile: true
                },
                {
                    Content: this.indexManager.getStatusName(Status),
                    Attributes: [{
                        Name: 'class',
                        Value: `text-center ${isActivedViewer ? 'link col-status' : ''}`
                    }].concat(isActivedViewer ? commonAttributesJson.concat([{
                        Name: 'current-status',
                        Value: Status
                    }]) : []),
                    Visibile: true
                },
                {
                    Content: this.buildActionColumnOnDesktop(rs.Customers[i]),
                    Attributes: [{
                        Name: 'class',
                        Value: 'col-action text-left'
                    }],
                    Visibile: true
                },
            ];

            const columns1: ColumnContent[] = [];

            attributesJson.forEach(item => {
                if (item.Visibile) {
                    const attrs: Attribute[] = [];
                    item.Attributes.forEach(attr => attrs.push(new Attribute(attr.Name, attr.Value)));
                    columns1.push(new ColumnContent(item.Content, attrs));
                }
            });

            const rowContent = new RowContent(columns1, [new Attribute('class', 'row-customer'), new Attribute('cust-id', CustId)]);
            html += rowContent.ToString();
        }

        return html;
    };
    buildActionColumnOnDesktop(customer: any) {
        const { agent, member } = siteInfrastructure.roleId, isAgent = this.levelId === agent, isMember = this.levelId === member;
        const { viewerStatusId, levelId } = this,
            { active } = siteInfrastructure.customerStatus,
            isActivedViewer = viewerStatusId === active,
            { ResetPassword, ResetSecurityCode, EditMember, EditAgent, EditMasterAgent, NewAgent, NewMember, UnlockTooltip } = this.viewResources;
        const { CustId, Username, Status, Closed} = customer, emptyContent = '';
        let actionColumn = '';

        const commonAttributesJson = [
            {
                Name: 'cust-id',
                Value: CustId
            },
            {
                Name: 'username',
                Value: Username
            },
        ];
        const iconAttributesJson = [
            {
                Name: 'data-toggle',
                Value: `tooltip`
            },
            {
                Name: 'data-placement',
                Value: `top`
            }
        ].concat(commonAttributesJson);

        const actionsJson = [
            {
                Content: emptyContent,
                Attributes: [
                    {
                        Name: 'class',
                        Value: 'icon-pass'
                    },
                    {
                        Name: 'title',
                        Value: ResetPassword
                    },
                    {
                        Name: 'cust-role',
                        Value: levelId.toString()
                    }].concat(iconAttributesJson),
                Visibile: isActivedViewer
            },
            {
                Content: emptyContent,
                Attributes: [{
                    Name: 'class',
                    Value: 'icon-security'
                },
                {
                    Name: 'title',
                    Value: ResetSecurityCode
                }].concat(iconAttributesJson),
                Visibile: isActivedViewer && !isMember
            },
            {
                Content: emptyContent,
                Attributes: [{
                    Name: 'class',
                    Value: 'icon-edit'
                },
                {
                    Name: 'title',
                    Value: isMember ? EditMember : isAgent ? EditAgent : EditMasterAgent
                }].concat(iconAttributesJson),
                Visibile: isActivedViewer
            },
            {
                Content: emptyContent,
                Attributes: [{
                    Name: 'class',
                    Value: 'icon-add'
                },
                {
                    Name: 'title',
                    Value: isAgent ? NewMember : NewAgent
                }].concat(iconAttributesJson),
                Visibile: isActivedViewer && Status === active && !isMember
            },
            {
                Content: emptyContent,
                Attributes: [{
                    Name: 'class',
                    Value: 'icon-unlock'
                },
                {
                    Name: 'title',
                    Value: UnlockTooltip
                }].concat(iconAttributesJson),
                Visibile: isActivedViewer && Closed
            },
        ];

        const actionSpans: WrapperSpan[] = [];

        actionsJson.forEach(item => {
            if (item.Visibile) {
                const attrs: Attribute[] = [];
                item.Attributes.forEach(attr => attrs.push(new Attribute(attr.Name, attr.Value)));
                actionSpans.push(new WrapperSpan(item.Content, attrs));
            }
        });

        actionSpans.forEach(span => {
            actionColumn += span.ToString();
        });

        return actionColumn;
    }
    buildBodyOnMobileMode(rs: any) {
        const { viewerStatusId, levelId } = this,
            { agent, member } = siteInfrastructure.roleId,
            isAgent = levelId === agent, isMember = levelId === member;

        if (rs.Customers.length == 0) {
            return this.buildNoInfoContent(isMember ? 5 : 4);
        }

        let html = "";
        let customerIndex = this.reportParam.PageSize * (this.reportParam.PageIndex - 1) + 1;

        const { active } = siteInfrastructure.customerStatus,
            { ResetPassword, ResetSecurityCode, EditMember, EditAgent, EditMasterAgent, NewAgent, NewMember, UnlockConfirmTitle } = this.viewResources;

        for (let i = 0; i < rs.Customers.length; i++ , customerIndex++) {
            let { CustId, Username, Status, Credit, Closed, Group, Name, Nickname } = rs.Customers[i],
                emptyContent = '',
                isActivedViewer = viewerStatusId === active,
                statusName = this.indexManager.getStatusName(status),
                nickname = Nickname ? Nickname : "---",
                name = Name ? Name : "---",
                userAttr = `cust-id='${CustId}' username='${Username}'`,
                creditFormat = this.numeral(Credit).format("###,0.00"),
                usernameLink = isMember ? 'view-statement' : 'view-downline';

            let actionColumn = '';
            const commonAttributes: Attribute[] = [
                new Attribute('cust-id', `${CustId}`),
                new Attribute('username', Username),
            ];

            let creditAttrs = [new Attribute('class', `text-right ${isActivedViewer ? 'link col-credit' : ''}`)];
            let statusAttrs = [new Attribute('class', `text-center ${isActivedViewer ? 'link col-status' : ''}`)];

            if (isActivedViewer) {
                creditAttrs = creditAttrs.concat(commonAttributes);

                statusAttrs.push(new Attribute('current-status', Status));
                statusAttrs = statusAttrs.concat(commonAttributes);

                const unlockIcon = new WrapperSpan(emptyContent, [new Attribute('class', 'icon-user-unlock')]);
                const moreActionIcon = new WrapperSpan(emptyContent, [new Attribute('class', 'icon-more-action')]);
                const anchorWrapper = new WrapperAnchor(
                    (Closed ? unlockIcon : moreActionIcon).ToString(),
                    [
                        new Attribute('id', CustId),
                        new Attribute('class', 'dropdown-toggle'),
                        new Attribute('type', 'button'),
                        new Attribute('data-toggle', 'dropdown'),
                        new Attribute('aria-haspopup', 'true'),
                        new Attribute('aria-expanded', 'false'),
                    ]
                );

                const liArray: WrapperLi[] = [
                    new WrapperLi(ResetPassword, [
                        new Attribute('class', 'icon-pass'),
                        new Attribute('cust-role', levelId.toString())
                    ].concat(commonAttributes)),
                    //new WrapperLi(ResetSecurityCode,
                    //    [new Attribute('class', 'icon-security')].concat(commonAttributes), !isMember),
                    new WrapperLi(ResetSecurityCode,
                        [new Attribute('class', 'icon-security')].concat(commonAttributes)),
                    new WrapperLi(isMember ? EditMember : isAgent ? EditAgent : EditMasterAgent,
                        [new Attribute('class', 'icon-edit')].concat(commonAttributes)),
                    //new WrapperLi(isAgent ? NewMember : NewAgent,
                    //    [new Attribute('class', 'icon-add')].concat(commonAttributes), Status === active && !isMember),
                    //new WrapperLi(UnlockConfirmTitle,
                    //    [new Attribute('class', 'icon-unlock')].concat(commonAttributes), Closed),
                    new WrapperLi(isAgent ? NewMember : NewAgent,
                        [new Attribute('class', 'icon-add')].concat(commonAttributes)),
                    new WrapperLi(UnlockConfirmTitle,
                        [new Attribute('class', 'icon-unlock')].concat(commonAttributes))
                ];

                const ul = new WrapperUl(liArray, [
                    new Attribute('class', 'dropdown-menu'),
                    new Attribute('aria-labelledby', CustId)
                ]);

                actionColumn = new WrapperDiv(`${anchorWrapper.ToString()}${ul.ToString()}`, [new Attribute('class', 'dropdown')]).ToString();
            }

            const nickNameDiv = new WrapperDiv(nickname, [new Attribute('class', 'cust-nickname')]);
            const nameDiv = new WrapperDiv(name, [new Attribute('class', 'cust-name')]);
            const usernameDiv = new WrapperDiv(`${Username}${nickNameDiv.ToString()}${nameDiv.ToString()}`, [new Attribute('class', 'username-wrapper')]);

            const columns: ColumnContent[] = [
                new ColumnContent(usernameDiv.ToString(), [new Attribute('class', `link ${isMember ? 'view-statement' : 'view-downline'}`)].concat(commonAttributes)),
                //new ColumnContent(Group, [], isMember),
                new ColumnContent(this.numeral(Credit).format("###,0.00"), creditAttrs),
                new ColumnContent(this.indexManager.getStatusName(Status), statusAttrs),
                new ColumnContent(actionColumn, [new Attribute('class', 'col-action text-center')])
            ];

            const rowContent = new RowContent(columns, [new Attribute('class', 'row-customer'), new Attribute('cust-id', CustId)]);
            html += rowContent.ToString();
        }

        return html
    };
    buildNoInfoContent = (colspan: number) =>
        `<tr><td colspan='${colspan}' align='center' class='no-info'>${this.viewResources.NoInformationIsAvailable}</td></tr>`;
    baseReloadState(reportParam: any, custRoleName: string) {
        const { Username, PageSize, PageIndex } = reportParam;
        this.$username.val(Username);
        this.$dropdownStatus.val(this.$(`#dropdown-status-${custRoleName.toLowerCase()}`).val());
        this.reportParam.PageSize = PageSize;
        this.reportParam.PageIndex = PageIndex;

        this.$(`#PageIndex${custRoleName}`).val(PageIndex);
        this.$(`#PageSize${custRoleName}`).val(PageIndex);
    };
    setStateWhenViewDownlines(status: number) {
        this.$username.val("");
        this.reportParam.PageIndex = 1;
        this.reportParam.PageSize = 100;
        this.reportParam.Status = status;
        this.statusComponent.changeSelection(status);
    };
    viewList(downlineUsername: string = null, custId: number = 0) {
        this.$.blockUI();
        this.reportParam = {
            DownlineUsername: downlineUsername,
            Username: <string>this.$username.val(),
            SearchRoleId: this.levelId,
            CustId: custId,
            Status: <number>this.$dropdownStatus.find("option:selected").val(),
            PageSize: this.reportParam.PageSize,
            PageIndex: this.reportParam.PageIndex
        };

        const { masterToggleHandler, agentToggleHandler, memberToggleHandler } = this.indexManager,
            { master, agent } = siteInfrastructure.roleId;

        if (this.indexManager.isDevice) {
            this.levelId === master ?
                masterToggleHandler.hideFilter() :
                this.levelId === agent ? agentToggleHandler.hideFilter() :
                    memberToggleHandler.hideFilter();
        }

        this.loadData();
    }
}
