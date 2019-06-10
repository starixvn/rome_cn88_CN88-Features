"use strict";

class BootstrapDropdownOption {
    constructor(public dropdownId: string = "dropdown-single") {}
}

class BootstrapDropdownComponent {
    $: JQueryStatic;
    $dropdown: JQuery;

    options = new BootstrapDropdownOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: BootstrapDropdownOption = new BootstrapDropdownOption()) => {
        this.options = <BootstrapDropdownOption>this.$.extend({}, this.options, options);

        let $dropdown = this.$(this.options.dropdownId);
        this.$dropdown = $dropdown;

        let this$ = this.$;

        $dropdown.on("shown.bs.dropdown", function () {
            var $window = this$(window);
            var $this = this$(this);
            var $button = $this.children(".dropdown-toggle");
            var $ul = $this.children(".dropdown-menu");
            var ulOffset = $ul.offset();

            var spaceUp = (ulOffset.top - $button.height() - $ul.height()) - $window.scrollTop();
            var spaceDown = $window.scrollTop() + $window.height() - (ulOffset.top + $ul.height());

            if (spaceUp > spaceDown) {
                $this.addClass("dropup");
            }
        }).on("hidden.bs.dropdown", function () {
            this$(this).removeClass("dropup");
        });
    };
}