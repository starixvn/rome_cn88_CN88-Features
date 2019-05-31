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
        const $checkboxes = this.$listItem.children().not(".hide").find("input[type='checkbox']");
        this.$checkboxes = $checkboxes;

        this.$checkboxAll.change(() => {
            $checkboxes.prop("checked", this.$checkboxAll.is(":checked"));
        });

        this.$checkboxes.on("click touchend", (eventObject: JQueryEventObject) => {
            if (this.$(eventObject.target).is(":checked")) {
                this.isCheckAll();
            } else {
                this.$checkboxAll.prop("checked", false);
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
        const selected = [];

        if (this.$checkboxAll.is(":checked")) {
            return selected;
        }

        const selectedCheckboxs = this.$listItem.find(".item").children("input:checked");
        selectedCheckboxs.each((index: number, elem: Element) => {
            selected.push([this.$(elem).val()]);
        });

        return selected;
    };

    getSelectedItemTexts = () => {
        const selected = [];

        if (this.$checkboxAll.is(":checked")) {
            return selected;
        }

        const selectedOptions = this.$listItem.find("input:checked");
        selectedOptions.each((index: number, elem: Element) => {
            selected.push([this.$(elem).attr("data-text")]);
        });

        return selected;
    };

    updateSelectedValues = () => {
        this.$hiddenInput.val(this.getSelectedItems().join(","));
    };

    search = (text) => {
        const lowerCaseText = text.toLowerCase();
        this.$checkboxAll.prop("checked", false);

        this.$checkboxes.each((index: number, elem: Element) => {
            const chk = this.$(elem);
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
        this.$checkboxAll.prop("checked", true);
        this.$checkboxes.each((index: number, elem: Element) => {
            const chk = this.$(elem);
            chk.prop("checked", true);
            chk.parent().removeClass("hide");
        });
    }
}