namespace BetList.Details {
    export class ParlayTicketDetails extends BaseDetails {
        private static instance: ParlayTicketDetails;

        public static get Instance(): ParlayTicketDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ParlayTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowComboParlayDetails = (refNo: string, winlostDate: Date, transId: string,
            betId: string, custId: string, betTeam: string, sourceElement: any) => {
            const ticketsContent: JQuery = $("#div-details-" + transId);

            if (ticketsContent.length == 0) {
                let moreInfo = $(".tbl-bet-list tr[data-trans-id='" + transId + "'] .cn88-bet-item");

                var params = $.param({
                    refNo: refNo,
                    transId: transId,
                    betId: betId,
                    custId: custId
                });

                $.ajax({
                    method: 'GET',
                    url: Common.AppPath.BetListAppPath() + "BetLists/TicketDetail?" + params,
                    beforeSend: () => {
                        moreInfo.append("<div id='div-details-" + transId + "' class='block-icon-small'></div>");
                    },
                    success: (responseHtml) => {
                        var div = $("#div-details-" + transId);
                        div.removeClass("block-icon-small");
                        div.html(responseHtml);
                    },
                });

                return;
            }

            var isVisible = !ticketsContent.hasClass("display-none");
            if (isVisible) {
                ticketsContent.addClass("display-none");
            } else {
                ticketsContent.removeClass("display-none");
            }
        }
    }
}
