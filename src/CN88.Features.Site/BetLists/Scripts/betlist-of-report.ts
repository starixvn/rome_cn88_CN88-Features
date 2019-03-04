function showParlaySubBetsCN88(transId, index, refNo, handicap1, handicap2, betCheck, betTeam, custId, sourceElement) {
    var $parent = $(sourceElement).parent();

    if ($parent.hasClass("cn88-mix-parlay-collapse")) {
        $parent.removeClass("cn88-mix-parlay-collapse");
    }

    else {
        $parent.addClass("cn88-mix-parlay-collapse");
    }

    // Place Your Code Here.
    alert("Alert from showParlaySubBetsCN88");

}

function showCombMPDetail(winlostDate, transId, betId, custId, betTeam, sourceElement) {
    // Place Your Code Here.
    alert("Alert from showCombMPDetail");
}
