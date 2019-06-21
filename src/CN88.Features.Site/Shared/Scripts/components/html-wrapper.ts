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

    constructor(tag: string, content: string, attributes: Attribute[] = []) {
        this.Tag = tag;
        this.Attributes = attributes;
        this.Content = content;
    }

    ToString = () => {
        let attrs = '';
        this.Attributes.forEach(attr => {
            const { Name, Value } = attr;
            attrs += `${Name}='${Value}' `;
        });

        return `<${this.Tag} ${attrs}>${this.Content}</${this.Tag}>`;
    }
}

class WrapperSpan extends WrapperElement {
    constructor(content: string = '', attributes: Attribute[] = []) {
        super('span', content, attributes);
    }
}

class WrapperDiv extends WrapperElement{
    constructor(content: string, attributes: Attribute[] = []) {
        super('div', content, attributes);
    }
}

class WrapperUl extends WrapperElement {
    constructor(liArray: WrapperLi[] = [], attributes: Attribute[] = []) {
        let content = '';
        liArray.forEach(li => {
            content += li.ToString();
        });

        super('ul', content, attributes);
    }
}

class WrapperLi extends WrapperElement {
    constructor(content: string, attributes: Attribute[] = []) {
        super('li', content, attributes);
    }
}

class WrapperAnchor extends WrapperElement {
    constructor(content: string, attributes: Attribute[] = []) {
        super('a', content, attributes);
    }
}

class ColumnContent extends WrapperElement {
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