class MemberStatement {
    $: JQueryStatic;
    $reportForm: JQuery;
    topWindow: any;
    sportGroupSelectionComponent: SingleSelectionDropdownComponent;
    fromDateSelectionComponent: SingleSelectionDropdownComponent;
    toDateSelectionComponent: SingleSelectionDropdownComponent;
    warning: string;
    ok: string;
    invalidDateRange: string;

    constructor(
        $: JQueryStatic,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        fromDateSelectionComponent: SingleSelectionDropdownComponent,
        toDateSelectionComponent: SingleSelectionDropdownComponent,
        topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
        this.sportGroupSelectionComponent = sportGroupSelectionComponent;
        this.fromDateSelectionComponent = fromDateSelectionComponent;
        this.toDateSelectionComponent = toDateSelectionComponent;
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
    }

    public onReportSubmit = () => {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.fromDateSelectionComponent.updateSelectedValue();
        this.toDateSelectionComponent.updateSelectedValue();
        var fromValue = this.$("#UserSelectedfromdate").val();
        var toValue = this.$("#UserSelectedtodate").val();
        if (fromValue < toValue) {
            this.topWindow.popupManager.bootboxAlert(this.warning, this.invalidDateRange, null, 400, this.ok);
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
        window.top);
    memberStatement.init();
})