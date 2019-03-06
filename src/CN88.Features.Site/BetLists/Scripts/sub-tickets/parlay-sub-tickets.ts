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

            var row = $(".tbl-bet-list tr[data-trans-id='" + transId + "']");

            const ticketsContent: JQuery = $("#cn88-sub-tickets-" + transId + "-" + index);
            const comboOfferName = $('#combinationOfferName_' + transId + "_" + index);

            if (ticketsContent.html() === "") {
                let moreInfo = $(".tbl-bet-list tr[data-trans-id='" + transId + "'] .cn88-mix-parlay");

                var params = $.param({
                    transId: transId,
                    custId: custId,
                    betCheck: betCheck,
                    betTeam: betTeam,
                    handicap1: handicap1,
                    handicap2: handicap2
                });

                $.ajax({
                    method: 'GET',
                    url: Common.AppPath.BetListAppPath() + "BetLists/ParlaySubTickets?" + params,
                    beforeSend: () => {
                        const blockIconContent = '<span class="block-icon-small"></span>';
                        ticketsContent.html(blockIconContent);
                        ticketsContent.show();
                    },
                    success: (responseHtml) => {
                        ticketsContent.html(responseHtml);
                        if (ticketsContent.find(".offer-name-list").length) {
                            comboOfferName.hide();
                        }

                        row.attr("show-sub-tickets", "1");
                    },
                });

                return;
            }

            if (ticketsContent.is(":visible") && !this.isShowAllSubBets) {
                ticketsContent.hide();
                comboOfferName.show();
                row.attr("show-sub-tickets", "0");
            }
            else {
                ticketsContent.show();
                if (ticketsContent.find(".offer-name-list").length) {
                    comboOfferName.hide();
                    row.attr("show-sub-tickets", "1");
                }
            }
        }

        public ShowAllParlaySubBetsCN88 = () => {
            this.isShowAllSubBets = true;
            $(".tbl-bet-list .cn88-mix-parlay > a").click();
            this.isShowAllSubBets = false;
        }
    }
}