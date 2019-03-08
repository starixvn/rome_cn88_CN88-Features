class BetListOfStatement {
    $: JQueryStatic;
    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.registerEvents();
    };

    registerEvents = () => {
        this.$(".btn-back").off("click").on("click", function () {
            window.history.back();
        });
    };
}

$(document).ready(() => {
    const betListOfStatement = new BetListOfStatement($);
    betListOfStatement.init();
})