"use strict";
class MultiSelectionCheckboxOption {
    constructor(
        public checkboxAllId = "checkbox-all",
        public listItemId = "ListId",
        public hiddenInputId = "HiddenInputId") {
    }
}

class MultiSelectionCheckboxComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $checkboxAll: JQuery;
    $listItem: JQuery;
    $checkboxes: JQuery;

    options = new MultiSelectionCheckboxOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: MultiSelectionCheckboxOption = new MultiSelectionCheckboxOption()) => {
        this.options = this.$.extend({}, this.options, options) as MultiSelectionCheckboxOption;

        this.$checkboxAll = this.$("#" + this.options.checkboxAllId);
        this.$listItem = this.$("#" + this.options.listItemId);
        this.$hiddenInput = this.$("#" + this.options.hiddenInputId);

        let self = this;
        let $checkboxes = this.$listItem.children().not(".hide").find("input[type='checkbox']");
        this.$checkboxes = $checkboxes;
        this.$checkboxAll.change(function () {
            $checkboxes.prop("checked", this.checked);
        });

        $checkboxes.on("click touchend", function () {
            if (this.checked) {
                self.isCheckAll();
            } else {
                self.$checkboxAll.prop("checked", false);
            }
        });
    };

    isCheckAll = () => {
        let isAllChecked = true;

        this.$checkboxes.each(function () {
            if (!this.checked) {
                isAllChecked = false;
            }
        });

        if (isAllChecked) {
            this.$checkboxAll.prop("checked", true);
        }
    };

    getSelectedItems = () => {
        let selected = [];

        if (this.$checkboxAll.is(":checked")) {
            return selected;
        }

        const selectedCheckboxs = this.$listItem.find(".item").children("input:checked");

        const this$ = this.$;

        selectedCheckboxs.each(function () {
            selected.push([this$(this).val()]);
        });

        return selected;
    };

    getSelectedItemTexts = () => {
        let selected = [];

        if (this.$checkboxAll.is(":checked")) {
            return selected;
        }

        const selectedOptions = this.$listItem.find("input:checked");
        const this$ = this.$;

        selectedOptions.each(function () {
            selected.push([this$(this).attr("data-text")]);
        });

        return selected;
    };

    updateSelectedValues = () => {
        this.$hiddenInput.val(this.getSelectedItems().join(","));
    };

    search = (text) => {
        const lowerCaseText = text.toLowerCase();
        const this$ = this.$;
        this.$checkboxAll.prop("checked", false)

        this.$checkboxes.each(function () {
            const chk = this$(this);
            const chkValue = chk.attr("data-text").toLowerCase();
            if (chkValue.indexOf(lowerCaseText) === -1) {
                chk.prop("checked", false);
                chk.parent().addClass("hide");
            } else {
                chk.prop("checked", true);
                chk.parent().removeClass("hide");
            }
        });
    };

    clearSearch = () => {
        const this$ = this.$;
        this.$checkboxAll.prop("checked", true);
        this.$checkboxes.each(function () {
            const chk = this$(this);
            chk.prop("checked", true);
            chk.parent().removeClass("hide");
        });
    }
}