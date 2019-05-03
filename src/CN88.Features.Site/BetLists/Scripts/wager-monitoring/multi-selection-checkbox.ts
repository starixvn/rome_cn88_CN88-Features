"use strict";
class MultiSelectionCheckboxOption {
    constructor(
        public checkboxAllId: string = "checkbox-all",
        public listItemId: string = "ListId",
        public hiddenInputId: string = "HiddenInputId") {
    }
}

class MultiSelectionCheckboxComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $checkboxAll: JQuery;
    $listItem: JQuery;
    $checkboxes: any;

    options = new MultiSelectionCheckboxOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: MultiSelectionCheckboxOption = new MultiSelectionCheckboxOption()) => {
        this.options = <MultiSelectionCheckboxOption>this.$.extend({}, this.options, options);

        let $checkboxAll = this.$("#" + this.options.checkboxAllId);
        this.$checkboxAll = $checkboxAll;
        this.$listItem = this.$("#" + this.options.listItemId);
        this.$hiddenInput = this.$("#" + this.options.hiddenInputId);

        let self = this;
        let $checkboxes = this.$listItem.children().not(".hide").find("input[type='checkbox']");
        this.$checkboxes = $checkboxes;
        this.$checkboxAll.change(function () {
            $checkboxes.prop("checked", this.checked);

            if (this.checked) {
                $checkboxes.attr("disabled", "disabled");
            } else {
                $checkboxes.removeAttr("disabled");
            }
        });

        $checkboxes.on("click touchend", function () {
            if (this.checked) {
                self.isCheckAll();
            } else {
                $checkboxAll.prop("checked", false);
            }
        });
    };

    isCheckAll = () => {
        let isAllChecked = true;

        this.$checkboxes.each(function () {
            if (!this.checked) {
                isAllChecked = false;
            }
        })

        if (isAllChecked) {
            this.$checkboxAll.prop("checked", true);
            this.$checkboxes.attr("disabled", "disabled");
        }
    };

    getSelectedItems = () => {
        let selected = [];

        if (this.$checkboxAll.is(":checked")) {
            return selected;
        }

        let selectedCheckboxs = this.$listItem.find(".item").children("input:checked");

        let this$ = this.$;

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

        let selectedOptions = this.$listItem.find("input:checked");
        let this$ = this.$;

        selectedOptions.each(function () {
            selected.push([this$(this).attr("data-text")]);
        });

        return selected;
    };

    updateSelectedValues = () => {
        this.$hiddenInput.val(this.getSelectedItems().join(","));
    };
    
    search = (text) => {
        var lowerCaseText = text.toLowerCase();
        var this$ = this.$;
        this.$checkboxAll.prop("checked", false)

        this.$checkboxes.each(function () {
            var chk = this$(this);
            var chkValue = chk.attr("data-text").toLowerCase();
            if (chkValue.indexOf(lowerCaseText) == -1) {
                chk.prop("checked", false);
                chk.parent().addClass("hide");
            } else {
                chk.prop("checked", true);
                chk.removeAttr("disabled");
                chk.parent().removeClass("hide");
            }
        });
    };

    clearSearch = () => {
        var this$ = this.$;
        this.$checkboxAll.prop("checked", true)
        this.$checkboxes.each(function () {
            let chk = this$(this);
            chk.prop("checked", true);
            chk.attr("disabled", "disabled");
            chk.parent().removeClass("hide");
        });
    }
}