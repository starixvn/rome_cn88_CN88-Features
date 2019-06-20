"use strict";

class ViewResources {
    $: JQueryStatic;
    public NoInformationIsAvailable: string;
    public Suspended: string;
    public Page: string;
    public Of: string;
    public Pager_DisplayItems: string;
    public ResetSecurityCodeDisabled: string;
    public Disabled: string;
    public NewMember: string;
    public NewAgent: string;
    public EditMember: string;
    public EditAgent: string;
    public EditMasterAgent: string;
    public ResetPassword: string;
    public ResetSecurityCode: string;
    public Active: string;
    public ViewOnly: string;
    public Inactive: string;
    public UnlockTooltip: string;
    public UnlockConfirmTitle: string;
    public UnlockConfirmMessage: string;

    constructor($: JQueryStatic) {
        this.$ = $;

        this.NoInformationIsAvailable = this.$("#NoInformationIsAvailable").val();

        this.Active = this.$("#Active").val();
        this.ViewOnly = this.$("#ViewOnly").val();
        this.Inactive = this.$("#Inactive").val();
        this.Suspended = this.$("#Suspended").val();

        this.Page = this.$("#Page").val();
        this.Of = this.$("#of").val();
        this.Pager_DisplayItems = this.$("#Pager_DisplayItems").val();
        this.Disabled = this.$("#Disabled").val();

        this.NewMember = this.$("#NewMember").val();
        this.NewAgent = this.$("#NewAgent").val();
        this.EditMember = this.$("#EditMember").val();
        this.EditAgent = this.$("#EditAgent").val();
        this.EditMasterAgent = this.$("#EditMasterAgent").val();

        this.ResetPassword = this.$("#ResetPassword").val();
        this.ResetSecurityCode = this.$("#ResetSecurityCode").val();
        this.UnlockTooltip = this.$("#UnlockTooltip").val();
        this.UnlockConfirmTitle = this.$("#UnlockConfirmTitle").val();
        this.UnlockConfirmMessage = this.$("#UnlockConfirmMessage").val();
    }
}

class Attribute {
    public Name: string;
    public Value: string;

    constructor(name: string, value: string) {
        this.Name = name;
        this.Value = value;
    }
}

class WrapperElement {
    private Attributes: Attribute[];
    private Content: string;
    private Tag: string;

    constructor(tag: string, content: string, attributes: Attribute[] = []) {
        this.Tag = tag;
        this.Attributes = attributes;
        this.Content = content;
    }

    ToString = () => {
        let attrs = '';
        this.Attributes.forEach(attr => {
            const { Name, Value } = attr;
            attrs += `${Name}='${Value} '`;
        });

        return `<${this.Tag} ${attrs}>${this.Content}</${this.Tag}>`;
    }
}

class WrapperSpan extends WrapperElement {
    constructor(attributes: Attribute[] = [], content: string = '') {
        super('span', content, attributes);
    }
}

class ColumnContent extends WrapperElement{
    constructor(content: string, attributes: Attribute[] = []) {
        super('td', content, attributes);
    }
}

class RowContent {
    private Attributes: Attribute[];
    private Columns: ColumnContent[];

    constructor(columns: ColumnContent[], attributes: Attribute[] = []) {
        this.Attributes = attributes;
        this.Columns = columns;
    }

    ToString = () => {
        let attrs = '', content = '';
        this.Attributes.forEach(attr => {
            const { Name, Value } = attr;
            attrs += `${Name}='${Value} '`;
        });

        this.Columns.forEach(column => {
            if (column !== null) {
                content += column.ToString();
            }
        });

        return `<tr ${attrs}>${content}</tr>`;
    }
}