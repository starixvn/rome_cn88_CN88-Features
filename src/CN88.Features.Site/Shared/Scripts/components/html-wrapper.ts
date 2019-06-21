"use strict";

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
    public Visible: boolean;

    constructor(tag: string, content: string, attributes: Attribute[] = [], visible: boolean = true) {
        this.Tag = tag;
        this.Attributes = attributes;
        this.Content = content;
        this.Visible = visible;
    }

    ToString = () => {
        let attrs = '';
        this.Attributes.forEach(attr => {
            const { Name, Value } = attr;
            attrs += `${Name}='${Value} '`;
        });

        return this.Visible ? `<${this.Tag} ${attrs}>${this.Content}</${this.Tag}>` : '';
    }
}

class WrapperSpan extends WrapperElement {
    constructor(attributes: Attribute[] = [], content: string = '', visible: boolean = true) {
        super('span', content, attributes, visible);
    }
}

class ColumnContent extends WrapperElement{
    constructor(content: string, attributes: Attribute[] = [], visible: boolean = true) {
        super('td', content, attributes, visible);
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