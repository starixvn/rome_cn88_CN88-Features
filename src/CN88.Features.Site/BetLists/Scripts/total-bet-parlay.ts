class TotalBetParlay {
    $: JQueryStatic;
    pagerComponent: ClientPagerComponent;

    sportGroupSelectionComponent: SingleSelectionDropdownComponent;
    stakeModeSelectionComponent: SingleSelectionDropdownComponent;
    viewModeSelectionComponent: SingleSelectionDropdownComponent;
    cleaveStakeLimit: any;

    constructor(
        $: JQueryStatic,
        pagerComponent: ClientPagerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        stakeModeSelectionComponent: SingleSelectionDropdownComponent,
        viewModeSelectionComponent: SingleSelectionDropdownComponent) {
        this.$ = $;
        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;

        this.sportGroupSelectionComponent = sportGroupSelectionComponent;
        this.stakeModeSelectionComponent = stakeModeSelectionComponent;
        this.viewModeSelectionComponent = viewModeSelectionComponent;
    }

    init = () => {
        this.$.blockUI();

        this.pagerComponent.render();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "SportGroupId"));
        this.stakeModeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-stake-mode", "StakeMode"));
        this.viewModeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-view-mode", "ViewMode"));

        this.cleaveStakeLimit = new Cleave('#StakeLimit', {
            numeral: true,
            numeralPositiveOnly: true,
            numeralDecimalMark: '.',
            delimiter: ','
        });

        this.$("#report-form").submit(this.onReportSubmit);
        this.$.unblockUI();
    };

    onReportSubmit = () => {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.stakeModeSelectionComponent.updateSelectedValue();
        this.viewModeSelectionComponent.updateSelectedValue();
    };
}

$(document).ready(() => {
    const totalBetParlay = new TotalBetParlay(
        $,
        new ClientPagerComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($));
    totalBetParlay.init();
})