namespace BetLists.Result {
    export class NG2ResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            betId?: number) => {
            const params = '?gambleId=' + matchId;

            var options = {
                url: Common.AppPath.ResultsAppPath() + 'ng2/TicketDetail' + params,
                title: "",
                width: 500,
                height: 400
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        }
    }

    export class PokerGamesResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            betId?: number,
            betCheck?: string,
            custId?: number,
            transDesc?: string) => {
            const params = '?custId=' + custId + '&gameTypeId=' + betTypeId + '&transId3rd=' + betCheck;

            var options = {
                url: Common.AppPath.ResultsAppPath() + 'pokergames/Index' + params,
                title: "",
                width: 1099,
                height: 500,
                resizable: true
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        }
    }
}