"use strict";
class SubAccount {
    $: JQueryStatic;
    top: Window;
    $tblSubAccount: JQuery;

    constructor($: JQueryStatic, top: Window) {
        this.$ = $;
        this.top = top;
    }

    init(): void {
        this.cacheElements();
        this.bindEvents();
    }

    cacheElements(): void {
        this.$tblSubAccount = this.$("#tbl-subaccount");
    }

    bindEvents(): void {
        this.$tblSubAccount.stickyTableHeaders();      
        this.$('[data-toggle="tooltip"]').tooltip();
    }
}

$(() => {
    const subAccount: SubAccount = new SubAccount($, top);
    subAccount.init();
});