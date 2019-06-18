namespace BetLists.Result {
    const thirdPartyDetail = 'ThirdParty/Detail';

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

            const options = {
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

            openThirdPartyPopup(params);
        }
    }

    function openThirdPartyPopup(params: string) {

        const options = {
            url: `${Common.AppPath.ResultsAppPath()}${thirdPartyDetail}${params}`,
            title: "",
            width: 855,
            height: 725
        };

        Common.PopupDetails.Instance(window.top).Show(options);
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

            openThirdPartyPopup(params);
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

            openThirdPartyPopup(params);
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

            openThirdPartyPopup(params);
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

            openThirdPartyPopup(params);
        }
    }
}