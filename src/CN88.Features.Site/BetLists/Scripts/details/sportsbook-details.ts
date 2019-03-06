namespace BetList.Details {
    export class ParlayTicketDetails extends BaseDetails {
        private static instance: ParlayTicketDetails;

        public static get Instance(): ParlayTicketDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ParlayTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowComboParlayDetails = (refNo: string, winlostDate: Date, transId: string,
            betId: string, custId: string, betTeam: string, sourceElement: any) => {

            var params = $.param({
                refNo: refNo,
                transId: transId,
                betId: betId,
                custId: custId
            });

            var options = {
                url: Common.AppPath.BetListAppPath() + "BetLists/ParlayDetail?" + params,
                title: " ",
                width: 800,
                height: 438
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        }
    }
}
