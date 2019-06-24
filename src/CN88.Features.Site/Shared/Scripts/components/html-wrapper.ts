"use strict";

class Attribute {
    public Name: string;
    public Value: string;

    constructor(name: string, value: string) {
        this.Name = name;
        this.Value = value;
    }
}

class ElementWrapper {
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

class SpanWrapper extends ElementWrapper {
    constructor(content: string = '', attributes: Attribute[] = []) {
        super('span', content, attributes);
    }
}

class DivWrapper extends ElementWrapper{
    constructor(content: string, attributes: Attribute[] = []) {
        super('div', content, attributes);
    }
}

class UnoderListWrapper extends ElementWrapper {
    constructor(content: string, attributes: Attribute[] = []) {
        super('ul', content, attributes);
    }
}

class ListItemWrapper extends ElementWrapper {
    constructor(content: string, attributes: Attribute[] = []) {
        super('li', content, attributes);
    }
}

class AnchorWrapper extends ElementWrapper {
    constructor(content: string, attributes: Attribute[] = []) {
        super('a', content, attributes);
    }
}

class ColumnWrapper extends ElementWrapper {
    constructor(content: string, attributes: Attribute[] = []) {
        super('td', content, attributes);
    }
}

class RowWrapper extends ElementWrapper {
    constructor(content: string, attributes: Attribute[] = []) {
        super('tr', content, attributes);
    }
}