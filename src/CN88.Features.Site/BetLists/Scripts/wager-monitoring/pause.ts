class Pause {
    $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        var $stackTableElement = this.$("#tbl-report-stack");
        $stackTableElement
            .find('.btn-collapse-expanse')
            .parent()
            .off('click.stack')
            .on('click.stack', function () {
                var btnIcon = $(this).find(".btn-collapse-expanse");
                var rowIndex = btnIcon.parent().attr('data-stack-row');
                var isCollapsed = btnIcon.hasClass('icon-add');
                var $nextRows = $stackTableElement.find('.stack-row-' + rowIndex);
                var $tr = btnIcon.parent().parent();

                if (isCollapsed) {
                    btnIcon.removeClass('icon-add')
                        .addClass('icon-minus');
                    $tr.removeClass('stack-collapsed').addClass('stack-expanded');
                    $nextRows.show(200);
                }
                else {
                    btnIcon.removeClass('icon-minus')
                        .addClass('icon-add');
                    $tr.removeClass('stack-expanded').addClass('stack-collapsed');
                    $nextRows.hide(200);
                }
            });
    }
}

$(document).ready(() => {
    const pause = new Pause($);
    pause.init();
});