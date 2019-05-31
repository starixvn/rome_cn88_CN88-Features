class BaseReport {
    $: JQueryStatic;

    dateRangePickerComponent: DateRangePickerComponent;
    sportGroupSelectionComponent: SingleSelectionDropdownComponent;
    betTypeSelectionComponent: SingleSelectionDropdownComponent;
    liveStatusSelectionComponent: SingleSelectionDropdownComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sportGroupSelectionComponent = sportGroupSelectionComponent;
        this.betTypeSelectionComponent = betTypeSelectionComponent;
        this.liveStatusSelectionComponent = liveStatusSelectionComponent;
    }

    renderComponents(): void {
        this.dateRangePickerComponent.render();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId", this.onchangeSportType));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
    }

    onchangeSportType = () => {
        const sportType = this.sportGroupSelectionComponent.$dropdown.val();

        this.betTypeSelectionComponent.$dropdown.empty();
        const first = betTypes[0];
        this.betTypeSelectionComponent.$dropdown.append(`<option value="${first[0].Value}">${first[0].Text}</option>`);

        for (const betTypeGroups of betTypes) {
            if (betTypeGroups[0].SportTypeGroup.toString() === sportType) {
                for (const betTypeGroup of betTypeGroups) {
                    this.betTypeSelectionComponent.$dropdown.append(`<option value="${betTypeGroup.Value}">${betTypeGroup.Text}</option>`);
                }

                break;
            }
        }

        this.betTypeSelectionComponent.$dropdown.multiselect("destroy");
        this.betTypeSelectionComponent.$dropdown.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
        });
    };
}