let ViewResult: Function;
let showCombMPDetail: Function;
let showParlaySubBetsCN88: Function;
let showAllParlaySubBetsCN88: Function;

$(document).ready(() => {
    ViewResult = BetLists.Result.BaseResultDetails.Instance.ViewResultDetails;
    showCombMPDetail = BetList.Details.ParlayTicketDetails.Instance.ShowComboParlayDetails;
    showParlaySubBetsCN88 = BetList.SubTickets.ParlaySubTickets.Instance.ShowSubBetsCN88;
    showAllParlaySubBetsCN88 = BetList.SubTickets.ParlaySubTickets.Instance.ShowAllParlaySubBetsCN88;
});