class Pause {
    $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        const $stackTableElement = this.$("#tbl-report-stack");
        $stackTableElement
            .find('.btn-collapse-expanse')
            .parent()
            .off('click.stack')
            .on('click.stack', function () {
                const btnIcon = $(this).find(".btn-collapse-expanse");
                const rowIndex = btnIcon.parent().attr('data-stack-row');
                const isCollapsed = btnIcon.hasClass('icon-add');
                const $nextRows = $stackTableElement.find('.stack-row-' + rowIndex);
                const $tr = btnIcon.parent().parent();
                const speedOfEffect = 200;

                if (isCollapsed) {
                    btnIcon.removeClass('icon-add')
                        .addClass('icon-minus');
                    $tr.removeClass('stack-collapsed').addClass('stack-expanded');
                    $nextRows.show(speedOfEffect);
                }
                else {
                    btnIcon.removeClass('icon-minus')
                        .addClass('icon-add');
                    $tr.removeClass('stack-expanded').addClass('stack-collapsed');
                    $nextRows.hide(speedOfEffect);
                }
            });
    }
}

$(document).ready(() => {
    const pause = new Pause($);
    pause.init();
});