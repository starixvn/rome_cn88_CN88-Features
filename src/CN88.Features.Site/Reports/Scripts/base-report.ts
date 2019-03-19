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
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId", () => { this.onchangeSportType(); }));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
    }

    onchangeSportType(): void {
        let sportType = this.sportGroupSelectionComponent.$dropdown.val();
        this.betTypeSelectionComponent.$dropdown.empty();
        for (var i = 0; i < betTypes.length; i++) {
            var betTypeGroup = betTypes[i];
            if (betTypeGroup[0].SportTypeGroup == 0) {
                this.betTypeSelectionComponent.$dropdown.append('<option value=' + betTypeGroup[0].Value + '>' + betTypeGroup[0].Text + '</option>');

                if (sportType == 0)
                    break;
            }

            if (betTypeGroup[0].SportTypeGroup == sportType) {
                for (var j = 0; j < betTypeGroup.length; j++) {
                    this.betTypeSelectionComponent.$dropdown.append('<option value=' + betTypeGroup[j].Value + '>' + betTypeGroup[j].Text + '</option>');
                }

                break;
            }
        }

        this.betTypeSelectionComponent.$dropdown.multiselect('destroy')
        this.betTypeSelectionComponent.$dropdown.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
        });
    }
}