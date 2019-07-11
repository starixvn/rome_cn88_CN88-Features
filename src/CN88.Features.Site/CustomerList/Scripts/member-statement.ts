/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/filter-toggle.ts" />

class MemberStatement {
    $: JQueryStatic;
    $reportForm: JQuery;
    topWindow: Window;
    sportGroupSelectionComponent: SingleSelectionDropdownComponent;
    fromDateSelectionComponent: SingleSelectionDropdownComponent;
    toDateSelectionComponent: SingleSelectionDropdownComponent;
    warning: string;
    ok: string;
    invalidDateRange: string;
    isMobile: boolean;
    toggleHandler: ToggleHandler;

    constructor(
        $: JQueryStatic,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        fromDateSelectionComponent: SingleSelectionDropdownComponent,
        toDateSelectionComponent: SingleSelectionDropdownComponent,
        topWindow: Window,
        toggleHandler: ToggleHandler) {
        this.$ = $;
        this.topWindow = topWindow;
        this.sportGroupSelectionComponent = sportGroupSelectionComponent;
        this.fromDateSelectionComponent = fromDateSelectionComponent;
        this.toDateSelectionComponent = toDateSelectionComponent;
        this.toggleHandler = toggleHandler;
    }

    public init() {
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId"));
        this.fromDateSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-fromdate", "UserSelectedfromdate"));
        this.toDateSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-todate", "UserSelectedtodate"));
        this.$reportForm = this.$("#report-form");
        this.$reportForm.submit(this.onReportSubmit);
        this.warning = this.$("#Warning").val();
        this.ok = this.$("#OK").val();
        this.invalidDateRange = this.$("#InvalidDateRange").val();
        this.isMobile = this.$("#IsMobile").val() === "True";

        if (this.isMobile) {
            this.toggleHandler.render(new ToggleHandlerOption("filter-toggle-icon", "report-form", "tbl-member-statement"));
        }
    }

    public onReportSubmit = () => {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.fromDateSelectionComponent.updateSelectedValue();
        this.toDateSelectionComponent.updateSelectedValue();
        const fromValue = this.$("#UserSelectedfromdate").val();
        const toValue = this.$("#UserSelectedtodate").val();
        if (fromValue < toValue) {
            this.topWindow["popupManager"].bootboxAlert(this.warning, this.invalidDateRange, null, 400, this.ok);
            return false;
        }
    };
}

$(function () {
    const memberStatement = new MemberStatement(
        $,
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        window.top,
        new ToggleHandler($));
    memberStatement.init();
});