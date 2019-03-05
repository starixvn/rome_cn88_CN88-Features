namespace BetList.SubTickets {
    export class ParlaySubTickets {
        private static instance: ParlaySubTickets;
        private isShowAllSubBets = false;

        public static get Instance(): ParlaySubTickets {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ParlaySubTickets() : this.instance;
            return this.instance;
        }

        public ShowSubBetsCN88 = (transId: string, index: any, refNo: any,
            handicap1: any, handicap2: any, betCheck: any, betTeam: any, custId: any, sourceElement: any) => {
            var $parent = $(sourceElement).parent();

            if ($parent.hasClass("cn88-mix-parlay-collapse")) {
                $parent.removeClass("cn88-mix-parlay-collapse");
            }

            else {
                $parent.addClass("cn88-mix-parlay-collapse");
            }

            // Place Your Code Here.
            var row = $(".tbl-bet-list tr[data-trans-id='" + transId + "']");

            const subTicketRows: JQuery = $(".sub-tickets-" + transId);
            if (subTicketRows.length == 0) {
                let moreInfo = $(".tbl-bet-list tr[data-trans-id='" + transId + "'] .cn88-bet-item");

                var params = $.param({
                    transId: transId,
                    custId: custId
                });

                $.ajax({
                    method: 'GET',
                    url: Common.AppPath.BetListAppPath() + "BetLists/SubTickets?" + params,
                    beforeSend: () => {
                        moreInfo.append("<div id='div-sub-tickets-" + transId + "' class='block-icon-small'></div>");
                    },
                    success: (responseHtml) => {
                        this.InsertRowsAfterRow(row, $(responseHtml).find(">tbody>tr"));
                        $("#div-sub-tickets-" + transId).remove();
                        row.attr("show-sub-tickets", "1");
                    },
                });

                return;
            }

            var isVisible = row.attr("show-sub-tickets") == "1";
            if (isVisible && !this.isShowAllSubBets) {
                row.attr("show-sub-tickets", "0");
                subTicketRows.addClass("display-none");
            } else {
                row.attr("show-sub-tickets", "1");
                subTicketRows.removeClass("display-none");
            }
        }

        public ShowAllParlaySubBetsCN88 = () => {
            this.isShowAllSubBets = true;
            $(".tbl-bet-list .cn88-mix-parlay > a").click();
            this.isShowAllSubBets = false;
        }

        private InsertRowsAfterRow = (row: JQuery, rows: any) => {
            var target = row[0];
            for (var i = rows.length - 1; i >= 0; i--) {
                target.parentNode.insertBefore(rows[i], target.nextSibling);
            }
        };
    }
}