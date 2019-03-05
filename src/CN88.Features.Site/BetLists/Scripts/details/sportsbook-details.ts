namespace BetList.Details {
    export class ParlayTicketDetails extends BaseDetails {
        private static instance: ParlayTicketDetails;

        public static get Instance(): ParlayTicketDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ParlayTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowComboParlayDetails = (winlostDate: Date, transId: string, betId: string, custId: string, betTeam: string, sourceElement: any) => {
            const params = '?'
                + '&winlostdate=' + winlostDate
                + '&transid=' + transId
                + '&betid=' + betId
                + '&custid=' + custId
                + '&betteam=' + betTeam;

            var options = {
                url: Common.AppPath.BetListAppPath() + 'Parlay/ComboParlayDetail' + params,
                title: "",
                width: 800,
                height: 438
            };

            // Place Your Code Here.
            alert("Alert from showCombMPDetail");
        }
    }
}
