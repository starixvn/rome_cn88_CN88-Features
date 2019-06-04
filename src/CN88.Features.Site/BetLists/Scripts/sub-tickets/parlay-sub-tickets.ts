namespace BetList.SubTickets {
    const showSubTickets = "show-sub-tickets";
    export class ParlaySubTickets {
        private static instance: ParlaySubTickets;
        private isShowAllSubBets = false;

        public static get Instance(): ParlaySubTickets {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ParlaySubTickets() : this.instance;
            return this.instance;
        }

        public ShowSubBetsCN88 = (transId: string, index, refNo,
            handicap1, handicap2, betCheck, betTeam, custId, sourceElement) => {
            const $parent = $(sourceElement).parent();

            const mixParlyCollapseClass = "cn88-mix-parlay-collapse";
            if ($parent.hasClass(mixParlyCollapseClass)) {
                $parent.removeClass(mixParlyCollapseClass);
                $parent.find(".cn88-offername").removeClass("hide");
            }
            else {
                $parent.addClass(mixParlyCollapseClass);
                $parent.find(".cn88-offername").addClass("hide");
            }

            const row = $(".tbl-bet-list tr[data-trans-id='" + transId + "']");

            const ticketsContent: JQuery = $(`#cn88-sub-tickets-${transId}-${index}`);
            const comboOfferName = $(`#combinationOfferName_-${transId}-${index}`);

            if (ticketsContent.html() === "") {
                const params = $.param({
                    transId: transId,
                    custId: custId,
                    betCheck: betCheck,
                    betTeam: betTeam,
                    handicap1: handicap1,
                    handicap2: handicap2
                });

                $.ajax({
                    method: 'GET',
                    url: `${Common.AppPath.BetListAppPath()}BetLists/ParlaySubTickets?${params}`,
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

                        row.attr(showSubTickets, "1");
                    },
                });

                return;
            }

            if (ticketsContent.is(":visible") && !this.isShowAllSubBets) {
                ticketsContent.hide();
                comboOfferName.show();
                row.attr(showSubTickets, "0");
            }
            else {
                ticketsContent.show();
                if (ticketsContent.find(".offer-name-list").length) {
                    comboOfferName.hide();
                    row.attr(showSubTickets, "1");
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