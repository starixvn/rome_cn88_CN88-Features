/// <reference path="./base-wager.ts" />

$(document).ready(() => {
    const betListByAccount = new BaseWager($, new ClientPagerComponent($));
    betListByAccount.init($("#lnk-wager-details"));
});