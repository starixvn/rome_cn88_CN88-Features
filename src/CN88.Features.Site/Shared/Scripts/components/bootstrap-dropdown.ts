"use strict";

class BootstrapDropdownOption {
    constructor(public dropdownId = "dropdown-single") { }
}

class BootstrapDropdownComponent {
    $: JQueryStatic;
    topWindow: Window;
    $dropdown: JQuery;

    options = new BootstrapDropdownOption();

    constructor($: JQueryStatic, topWindow: Window) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    render = (options: BootstrapDropdownOption = new BootstrapDropdownOption()) => {
        this.options = this.$.extend({}, this.options, options) as BootstrapDropdownOption;

        const $dropdown = this.$(this.options.dropdownId);
        this.$dropdown = $dropdown;

        const this$ = this.$;
        const thisTopWindow = this.topWindow;

        $dropdown.on("shown.bs.dropdown", function () {
            const $window = this$(window);
            const $this = this$(this);
            const $button = $this.children(".dropdown-toggle");
            const $ul = $this.children(".dropdown-menu");
            const ulOffset = $ul.offset();
            const isiOS = /iPhone/i.test(navigator.userAgent);
            const scrollTop = isiOS ? thisTopWindow.$("body").scrollTop() : $window.scrollTop();

            const spaceUp = (ulOffset.top - $button.height() - $ul.height()) - scrollTop;
            const spaceDown = scrollTop + thisTopWindow.$("#content").height() - (ulOffset.top + $ul.height());

            if (spaceDown < 0 && (spaceUp >= 0 || spaceUp > spaceDown)) {
                $this.addClass("dropup");
            }
        }).on("hidden.bs.dropdown", function () {
            this$(this).removeClass("dropup");
        });
    };
}