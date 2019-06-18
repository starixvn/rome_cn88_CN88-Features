/// <reference path="globals/bootstrap/index.d.ts" />
/// <reference path="globals/jasmine-jquery/index.d.ts" />
/// <reference path="globals/jasmine/index.d.ts" />
/// <reference path="globals/jquery.blockui/index.d.ts" />
/// <reference path="globals/jquery.cookie/index.d.ts" />
/// <reference path="globals/jquery/index.d.ts" />
/// <reference path="globals/jqueryui/index.d.ts" />
/// <reference path="globals/moment/index.d.ts" />
/// <reference path="globals/storejs/index.d.ts" />

interface JQuery {
    multiselect(option: any): JQuery;
    multiselect(func: string, options: any): JQuery;
}

interface JQuery {
    stickyTableHeaders(): void;
}

interface JQuery {
    nexDatePicker(option: any): void;
}

interface JQuery {
    pagination(option: any): void;
}


interface JQuery {
    toggleHandler(option: any): void;
}

interface JQuery {
    tablesorter(option: any): void;
}

interface JQuery {
    multiselect(arg0: { multiple: boolean; maxHeight: number; numberDisplayed: number; includeSelectAllOption: boolean; onChange: () => void; });
    stickyTableHeaders(): void;
}