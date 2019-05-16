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
            const params = "?" + $.param({
                transDesc: transDesc,
                betCheck: betCheck,
                bettypeId: betTypeId,
                sportId: sportTypeId
            });

            var options = {
                url: this.ResultDetailsPath + params,
                title: "",
                width: 855,
                height: 850
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
            const params = "?" + $.param({
                gameId: refNo,
                roleId: this.ViewerLevelId,
                productId: 6
            });

            var options = {
                url: Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params,
                title: "",
                width: 855,
                height: 725
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
            const params = "?" + $.param({
                custId: this.CustId,
                roleId: this.ViewerLevelId,
                gameId: refNo,
                productId: 22
            });

            var options = {
                url: Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params,
                title: "",
                width: 855,
                height: 725
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
            const params = "?" + $.param({
                custId: this.CustId,
                roleId: this.ViewerLevelId,
                gameId: refNo,
                productId: 24
            });

            var options = {
                url: Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params,
                title: "",
                width: 855,
                height: 725
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
            const params = "?" + $.param({
                custId: this.CustId,
                roleId: this.ViewerLevelId,
                gameId: refNo,
                productId: 32
            });

            var options = {
                url: Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params,
                title: "",
                width: 855,
                height: 725
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
            const params = "?" + $.param({
                custId: this.CustId,
                roleId: this.ViewerLevelId,
                gameId: refNo,
                productId: 30
            });

            var options = {
                url: Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params,
                title: "",
                width: 855,
                height: 725
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        }
    }
}