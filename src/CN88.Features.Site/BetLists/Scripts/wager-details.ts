/// <reference path="./base-wager.ts" />

$(document).ready(() => {
    const wagerDetails = new BaseWager($, new ClientPagerComponent($));
    wagerDetails.init($("#lnk-bet-list-by-account"));
});