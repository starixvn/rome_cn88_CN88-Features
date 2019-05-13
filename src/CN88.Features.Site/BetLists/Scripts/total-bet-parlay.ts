class TotalBetParlay {
    $: JQueryStatic;
    pagerComponent: ClientPagerComponent;

    sportGroupSelectionComponent: SingleSelectionDropdownComponent;
    stakeModeSelectionComponent: SingleSelectionDropdownComponent;
    viewModeSelectionComponent: SingleSelectionDropdownComponent;
    cleaveStakeLimit: any;
    $stakeLimit: JQuery;
    toggleHandler: ToggleHandler;
    isMobile: boolean;

    constructor(
        $: JQueryStatic,
        pagerComponent: ClientPagerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        stakeModeSelectionComponent: SingleSelectionDropdownComponent,
        viewModeSelectionComponent: SingleSelectionDropdownComponent,
        toggleHandler: ToggleHandler) {
        this.$ = $;
        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;

        this.sportGroupSelectionComponent = sportGroupSelectionComponent;
        this.stakeModeSelectionComponent = stakeModeSelectionComponent;
        this.viewModeSelectionComponent = viewModeSelectionComponent;
        this.toggleHandler = toggleHandler;
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

        this.$stakeLimit = this.$("#StakeLimit");
        this.$("#report-form").submit(this.onReportSubmit);
        this.$.unblockUI();

        this.toggleHandler.render(new ToggleHandlerOption("filter-toggle-icon", "report-form", "totalbet-parlay-report"));
        this.isMobile = this.$("#isMobile").val() == "True";
    };

    onReportSubmit = () => {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.stakeModeSelectionComponent.updateSelectedValue();
        this.viewModeSelectionComponent.updateSelectedValue();

        let stake = parseFloat(this.cleaveStakeLimit.getRawValue()) || 0;
        this.$stakeLimit.val(stake);
    };
}

$(document).ready(() => {
    const totalBetParlay = new TotalBetParlay(
        $,
        new ClientPagerComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new ToggleHandler($));
    totalBetParlay.init();
})