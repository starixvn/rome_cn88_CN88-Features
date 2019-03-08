class OnlineMember {
    $: JQueryStatic;
    pagerComponent: PagerComponent;

    constructor(
        $: JQueryStatic,
        pager: PagerComponent) {
        this.$ = $;
        this.pagerComponent = pager;
    }

    public init() {
        this.pagerComponent.render();
    }    
}

$(function () {
    const onlineMembers = new OnlineMember(
        $,
        new PagerComponent($));

    onlineMembers.init();
})

