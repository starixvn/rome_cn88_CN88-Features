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
        let self: any = this;

        this.$tblSubAccount.stickyTableHeaders();

        this.$tblSubAccount.find('[data-toggle="tooltip"]').tooltip();

        this.$tblSubAccount.find(".icon-pass").click(function (): void {

        });

        this.$tblSubAccount.find(".icon-security").click(function (): void {
        });

        this.$tblSubAccount.find(".icon-edit").click(function (): void {
            //let parent = self.$(this).closest("td");
            //let custId = parent.data("custid");
            //confirm("Are you sure you want to delete sub account " + parent.data("username"));
            //self.top.popupManager.init(self.$, self.top);
            self.top.popupManager.bootboxDialog();
        });
    }
}

$(() => {
    const subAccount: SubAccount = new SubAccount($, top);
    subAccount.init();
});