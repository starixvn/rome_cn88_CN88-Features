namespace BetLists.Result {
    export class AllbetResultDetails extends BaseResultDetails implements ResultDetails {
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
            const params = '?transDesc=' + transDesc + '&betCheck=' + betCheck + '&bettypeId=' + betTypeId + '&sportId=' + sportTypeId;

            var options = {
                url: this.ResultDetailsPath + params,
                title: "",
                width: 835,
                height: 800
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        }
    }

    export class RnGCasinoResultDetails extends BaseResultDetails implements ResultDetails {
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
            betCheck?: string) => {
            var params = '?gameId=' + refNo + '&roleId=' + this.ViewerLevelId + '&productId=6';

            var options = {
                url: Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params,
                title: "",
                width: 835,
                height: 675
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        }
    }

    export class MacauGamesResultDetails extends BaseResultDetails implements ResultDetails {
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
            betCheck?: string) => {
            var params = '?custId=' + this.CustId +
                '&roleId=' + this.ViewerLevelId +
                '&gameId=' + refNo +
                '&productId=22';

            var options = {
                url: Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params,
                title: "",
                width: 835,
                height: 675
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        }
    }

    export class FishingWorldResultDetails extends BaseResultDetails implements ResultDetails {
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
            betCheck?: string) => {
            var params = '?custId=' + this.CustId +
                '&roleId=' + this.ViewerLevelId +
                '&gameId=' + betCheck +
                '&productId=24';

            var options = {
                url: Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params,
                title: "",
                width: 835,
                height: 675
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        }
    }

    export class PPResultDetails extends BaseResultDetails implements ResultDetails {
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
            betCheck?: string) => {
            var params = '?custId=' + this.CustId +
                '&roleId=' + this.ViewerLevelId +
                '&gameId=' + betCheck +
                '&productId=32';

            var options = {
                url: Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params,
                title: "",
                width: 835,
                height: 675
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        }
    }

    export class BullfightingResultDetails extends BaseResultDetails implements ResultDetails {
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
            betCheck?: string) => {
            var params = '?custId=' + this.CustId + '&roleId=' + this.ViewerLevelId + '&gameId=' + refNo + '&productId=30';

            var options = {
                url: Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params,
                title: "",
                width: 835,
                height: 675
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        }
    }
}
