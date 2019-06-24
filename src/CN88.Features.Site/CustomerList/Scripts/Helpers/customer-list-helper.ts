/// <reference path="../viewmodel/view-model.ts" />
/// <reference path="../../../shared/scripts/components/html-wrapper.ts" />

class CustomerListHelper {
    viewResources: ViewResources;
    getStatusName: Function;
    numeral: Function;

    constructor(viewResources: ViewResources, getStatusName: Function, numeral: Function) {
        this.viewResources = viewResources;
        this.getStatusName = getStatusName;
        this.numeral = numeral;
    }

    private buildCommonAttributesJson = (CustId: any, Username: string) =>
        [{ Name: 'cust-id', Value: CustId }, { Name: 'username', Value: Username }];

    private buildActionAttributeJson(
        levelId: number,
        isActivedViewer: boolean,
        isMember: boolean,
        status: number,
        closed: boolean) {
        return {
            Password: {
                Attributes: [{ Name: 'class', Value: 'icon-pass' }, { Name: 'cust-role', Value: levelId.toString() }],
                Visibile: isActivedViewer
            },
            Security: {
                Attributes: [{ Name: 'class', Value: 'icon-security' }],
                Visibile: isActivedViewer && !isMember
            },
            EditAccount: {
                Attributes: [{ Name: 'class', Value: 'icon-edit' }],
                Visibile: isActivedViewer
            },
            AddAccount: {
                Attributes: [{ Name: 'class', Value: 'icon-add' }],
                Visibile: isActivedViewer && status === siteInfrastructure.customerStatus.active && !isMember
            },
            UnlockAccount: {
                Attributes: [{ Name: 'class', Value: 'icon-unlock' }],
                Visibile: isActivedViewer && closed
            }
        };
    }

    private buildActionColumnOnDesktop(customer: any, viewerStatusId: number, levelId: number) {
        const { agent, member } = siteInfrastructure.roleId, isAgent = levelId === agent, isMember = levelId === member,
            { active } = siteInfrastructure.customerStatus,
            isActivedViewer = viewerStatusId === active,
            { ResetPassword, ResetSecurityCode, EditMember, EditAgent, EditMasterAgent, NewAgent, NewMember, UnlockTooltip } = this.viewResources,
            { CustId, Username, Status, Closed } = customer, emptyContent = '';

        const actionAttributeJson = this.buildActionAttributeJson(levelId, isActivedViewer, isMember, Status, Closed);
        const actionsJson = [
            {
                Content: emptyContent,
                Attributes: [{ Name: 'title', Value: ResetPassword }].concat(actionAttributeJson.Password.Attributes),
                Visibile: actionAttributeJson.Password.Visibile
            },
            {
                Content: emptyContent,
                Attributes: [{ Name: 'title', Value: ResetSecurityCode }].concat(actionAttributeJson.Security.Attributes),
                Visibile: actionAttributeJson.Security.Visibile
            },
            {
                Content: emptyContent,
                Attributes: [{ Name: 'title', Value: isMember ? EditMember : isAgent ? EditAgent : EditMasterAgent }]
                    .concat(actionAttributeJson.EditAccount.Attributes),
                Visibile: actionAttributeJson.EditAccount.Visibile
            },
            {
                Content: emptyContent,
                Attributes: [{ Name: 'title', Value: isMember ? EditMember : isAgent ? NewMember : NewAgent }]
                    .concat(actionAttributeJson.AddAccount.Attributes),
                Visibile: actionAttributeJson.AddAccount.Visibile
            },
            {
                Content: emptyContent,
                Attributes: [{ Name: 'title', Value: UnlockTooltip }].concat(actionAttributeJson.UnlockAccount.Attributes),
                Visibile: actionAttributeJson.UnlockAccount.Visibile
            }
        ];

        const iconAttributesJson = [{ Name: 'data-toggle', Value: 'tooltip' }, { Name: 'data-placement', Value: 'top' }]
            .concat(this.buildCommonAttributesJson(CustId, Username));

        return actionsJson.map(span => {
            if (span.Visibile) {
                return new SpanWrapper(span.Content,
                    span.Attributes.concat(iconAttributesJson)
                        .map(attr => new Attribute(attr.Name, attr.Value))).ToString();
            }
        }).join();
    }

    private buildActionColumnOnMobile(customer: any, viewerStatusId: number, levelId: number) {
        const { agent, member } = siteInfrastructure.roleId, isAgent = levelId === agent, isMember = levelId === member,
            { active } = siteInfrastructure.customerStatus,
            isActivedViewer = viewerStatusId === active,
            { ResetPassword, ResetSecurityCode, EditMember, EditAgent, EditMasterAgent, NewAgent, NewMember, UnlockConfirmTitle } = this.viewResources,
            { CustId, Username, Status, Closed } = customer, emptyContent = '';

        const anchorWrapper = new AnchorWrapper(
            new SpanWrapper(emptyContent, [new Attribute('class', `${Closed ? 'icon-user-unlock' : 'icon-more-action'}`)]).ToString(),
            [
                new Attribute('id', CustId),
                new Attribute('class', 'dropdown-toggle'),
                new Attribute('type', 'button'),
                new Attribute('data-toggle', 'dropdown'),
                new Attribute('aria-haspopup', 'true'),
                new Attribute('aria-expanded', 'false'),
            ]
        );

        const actionAttributeJson = this.buildActionAttributeJson(levelId, isActivedViewer, isMember, Status, Closed);
        const listItemsJson = [
            { ...{ Content: ResetPassword }, ...actionAttributeJson.Password },
            { ...{ Content: ResetSecurityCode }, ...actionAttributeJson.Security },
            { ...{ Content: isMember ? EditMember : isAgent ? EditAgent : EditMasterAgent }, ...actionAttributeJson.EditAccount },
            { ...{ Content: isAgent ? NewMember : NewAgent }, ...actionAttributeJson.AddAccount },
            { ...{ Content: UnlockConfirmTitle }, ...actionAttributeJson.UnlockAccount }
        ];

        const unorderContent = listItemsJson.map(listItem => {
            if (listItem.Visibile) {
                return new ListItemWrapper(listItem.Content,
                    listItem.Attributes.concat(this.buildCommonAttributesJson(CustId, Username))
                        .map(attr => new Attribute(attr.Name, attr.Value))).ToString();
            }
        }).join();

        const unorderListWrapper = new UnoderListWrapper(unorderContent, [
            new Attribute('class', 'dropdown-menu'),
            new Attribute('aria-labelledby', CustId)
        ]);

        return new DivWrapper(`${anchorWrapper.ToString()}${unorderListWrapper.ToString()}`, [new Attribute('class', 'dropdown')]).ToString();
    }

    private buildUsernameColumnContentOnMobile(Username: string, Nickname: string, Name: string) {
        const nickNameDiv = new DivWrapper(Nickname ? Nickname : "---", [new Attribute('class', 'cust-nickname')]);
        const nameDiv = new DivWrapper(Name ? Name : "---", [new Attribute('class', 'cust-name')]);
        return new DivWrapper(`${Username}${nickNameDiv.ToString()}${nameDiv.ToString()}`, [new Attribute('class', 'username-wrapper')]).ToString();
    }

    private buildNoInfoContent = (colspan: number) =>
        `<tr><td colspan='${colspan}' align='center' class='no-info'>${this.viewResources.NoInformationIsAvailable}</td></tr>`;

    public buildBody(rs: any, isDesktop: boolean, viewerStatusId: number, levelId: number, reportParam: any) {
        const { member } = siteInfrastructure.roleId, isMember = levelId === member,
            { active } = siteInfrastructure.customerStatus, isActivedViewer = viewerStatusId === active;

        if (rs.Customers.length == 0) {
            const noInfoColspans = isDesktop ? 9 : 5;
            return this.buildNoInfoContent(isMember ? noInfoColspans : noInfoColspans - 1);
        }

        let customerIndex = reportParam.PageSize * (reportParam.PageIndex - 1) + 1, bodyContent = '';

        for (let i = 0; i < rs.Customers.length; i++ , customerIndex++) {
            const { CustId, Username, Status, Credit, Group, Name, Nickname, CreationDateStr } = rs.Customers[i];

            const commonAttributesJson = this.buildCommonAttributesJson(CustId, Username);

            const columnsAttributesJson = [
                {
                    Content: customerIndex.toString(),
                    Attributes: [{ Name: 'class', Value: 'col-no' }],
                    Visibile: isDesktop
                },
                {
                    Content: isDesktop ? Username : this.buildUsernameColumnContentOnMobile(Username, Nickname, Name),
                    Attributes: [{ Name: 'class', Value: `link ${isMember ? 'view-statement' : 'view-downline'}` }]
                        .concat(isActivedViewer ? commonAttributesJson : []),
                    Visibile: true
                },
                {
                    Content: Name,
                    Attributes: [],
                    Visibile: isDesktop
                },
                {
                    Content: Nickname,
                    Attributes: [],
                    Visibile: isDesktop
                },
                {
                    Content: Group,
                    Attributes: [],
                    Visibile: isMember
                },
                {
                    Content: this.numeral(Credit).format("###,0.00"),
                    Attributes: [{ Name: 'class', Value: `text-right ${isActivedViewer ? 'link col-credit' : ''}` }]
                        .concat(isActivedViewer ? commonAttributesJson : []),
                    Visibile: true
                },
                {
                    Content: CreationDateStr,
                    Attributes: [{ Name: 'class', Value: 'col-date' }],
                    Visibile: isDesktop
                },
                {
                    Content: this.getStatusName(Status),
                    Attributes: [{ Name: 'class', Value: `text-center ${isActivedViewer ? 'link col-status' : ''}` }]
                        .concat(isActivedViewer ?
                            commonAttributesJson.concat([{ Name: 'current-status', Value: Status }]) : []),
                    Visibile: true
                },
                {
                    Content: isDesktop ?
                        this.buildActionColumnOnDesktop(rs.Customers[i], viewerStatusId, levelId) :
                        this.buildActionColumnOnMobile(rs.Customers[i], viewerStatusId, levelId),
                    Attributes: [{ Name: 'class', Value: 'col-action text-left' }],
                    Visibile: true
                }
            ];

            const rowContent = columnsAttributesJson.map(column => {
                if (column.Visibile) {
                    return new ColumnWrapper(column.Content,
                        column.Attributes.map(attr => new Attribute(attr.Name, attr.Value))).ToString();
                }
            }).join();

            bodyContent += new RowWrapper(rowContent,
                [new Attribute('class', 'row-customer'), new Attribute('cust-id', CustId)]).ToString();
        }

        return bodyContent;
    };
}