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
            const params = "?" + $.param({ gambleId: matchId });

            const options = {
                url: `${Common.AppPath.ResultsAppPath()}ng2/TicketDetail${params}`,
                title: "",
                width: 520,
                height: 450
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
            const params = "?" + $.param({
                custId: custId,
                gameTypeId: betTypeId,
                transId3rd: betCheck
            });

            const options = {
                url: `${Common.AppPath.ResultsAppPath}pokergames/Index${params}`,
                title: "",
                width: 1099,
                height: 570,
                resizable: true
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        }
    }
}