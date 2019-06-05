namespace BetLists.Result {
    export interface ResultDetails {
        ViewResultDetails(matchId?: number,
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
            transDesc?: string): void;
    }

    export class ResultFactory {
        private static sportsbookResult: ResultDetails;
        private static mixParlayResult: ResultDetails;
        private static ng2Result: ResultDetails;
        private static allbetResult: ResultDetails;
        private static rngCasinoResult: ResultDetails;
        private static fishingWorldResult: ResultDetails;
        private static bullfightingResult: ResultDetails;
        private static macauGamesResult: ResultDetails;
        private static fastMarketsResult: ResultDetails;
        private static ppResult: ResultDetails;
        private static pokerGamesResult: ResultDetails;

        private static readonly BullfightingBetTypeId = 1004;

        private static readonly ng2FromBetTypeId = 2501;

        private static readonly ng2ToBetTypeId = 2699;

        private static readonly mixParlay = 9;

        private static readonly systemParlay = 29;

        private static readonly otherMixParlay = 2799;

        private static readonly fishingWorldFromBetTypeId = 1011;

        private static readonly fishingWorldToBetTypeId = 1020;

        private static readonly rngCasinoFromBetTypeId1 = 2101;

        private static readonly rngCasinoToBetTypeId1 = 2399;

        private static readonly rngCasinoFromBetTypeId2 = 2901;

        private static readonly rngCasinoToBetTypeId2 = 2999;

        private static readonly macauGamesFromBetTypeId = 4001;

        private static readonly macauGamesToBetTypeId = 4100;

        private static readonly allBetFromBetTypeId = 2401;

        private static readonly allBetToBetTypeId = 2405;

        private static readonly fastMarketFromBetTypeId = 221;

        private static readonly fastMarketToBetTypeId = 227;

        private static readonly ppFromBetTypeId = 4101;

        private static readonly ppToBetTypeId = 4300;

        private static readonly pokerGamesFromBetTypeId = 1401;

        private static readonly pokerGamesToBetTypeId = 1499;

        public static CreateInstance(betTypeId: number): ResultDetails {
            if (betTypeId === ResultFactory.BullfightingBetTypeId) {
                this.bullfightingResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.bullfightingResult) ? new BullfightingResultDetails() : this.bullfightingResult;
                return this.bullfightingResult;
            }

            if (betTypeId >= ResultFactory.ng2FromBetTypeId && betTypeId <= ResultFactory.ng2ToBetTypeId) {
                this.ng2Result = Common.Utils.IsNullOrUndefinedOrEmpty(this.ng2Result) ? new NG2ResultDetails() : this.ng2Result;
                return this.ng2Result;
            }

            if (betTypeId === ResultFactory.mixParlay || betTypeId === ResultFactory.systemParlay || betTypeId === ResultFactory.otherMixParlay) {
                this.mixParlayResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.mixParlayResult) ? new MixParlayResultDetails() : this.mixParlayResult;
                return this.mixParlayResult;
            }

            if (betTypeId >= ResultFactory.fishingWorldFromBetTypeId && betTypeId <= ResultFactory.fishingWorldToBetTypeId) {
                this.fishingWorldResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.fishingWorldResult) ? new FishingWorldResultDetails() : this.fishingWorldResult;
                return this.fishingWorldResult;
            }

            if ((betTypeId >= ResultFactory.rngCasinoFromBetTypeId1 && betTypeId <= ResultFactory.rngCasinoToBetTypeId1) 
                || (betTypeId >= ResultFactory.rngCasinoFromBetTypeId2 && betTypeId <= ResultFactory.rngCasinoToBetTypeId2)) {
                this.rngCasinoResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.rngCasinoResult) ? new RnGCasinoResultDetails : this.rngCasinoResult;
                return this.rngCasinoResult;
            }

            if (betTypeId >= ResultFactory.macauGamesFromBetTypeId && betTypeId <= ResultFactory.macauGamesToBetTypeId) {
                this.macauGamesResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.macauGamesResult) ? new MacauGamesResultDetails() : this.macauGamesResult;
                return this.macauGamesResult;
            }

            if (betTypeId >= ResultFactory.allBetFromBetTypeId && betTypeId <= ResultFactory.allBetToBetTypeId) {
                this.allbetResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.allbetResult) ? new AllbetResultDetails() : this.allbetResult;
                return this.allbetResult;
            }

            if (ResultFactory.fastMarketFromBetTypeId <= betTypeId && betTypeId <= ResultFactory.fastMarketToBetTypeId) {
                this.fastMarketsResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.fastMarketsResult) ? new FastMarketsResultDetails() : this.fastMarketsResult;
                return this.fastMarketsResult;
            }

            if (betTypeId >= ResultFactory.ppFromBetTypeId && betTypeId <= ResultFactory.ppToBetTypeId) {
                this.ppResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.ppResult) ? new PPResultDetails() : this.ppResult;
                return this.ppResult;
            }

            if (ResultFactory.pokerGamesFromBetTypeId <= betTypeId && betTypeId <= ResultFactory.pokerGamesToBetTypeId) {
                this.pokerGamesResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.pokerGamesResult) ? new PokerGamesResultDetails() : this.pokerGamesResult;
                return this.pokerGamesResult;
            }

            this.sportsbookResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.sportsbookResult) ? new SportsbookResultDetails : this.sportsbookResult;
            return this.sportsbookResult;
        }
    }

    export class BaseResultDetails {
        protected readonly ResultDetailsPath: string = `${Common.AppPath.ResultsAppPath()}Normal/ResultDetails"`;
        protected readonly SportTypeGroup: string = $("#sport-type-group").val();
        protected readonly BetTypeGroup: string = $("#bet-type-group").val();
        protected readonly FromDate: string = $("#from-date").val();
        protected readonly ToDate: string = $("#to-date").val();
        protected readonly CustId: string = $("#cust-id").val();
        protected readonly ViewerLevelId: string = $("#viewer-level-id").val();
        protected readonly BetListType: string = $("#bet-list-type").val();
        private static instance: BaseResultDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new BaseResultDetails() : this.instance;
            return this.instance;
        }

        public ViewResultDetails = (
            matchId: number,
            race: number,
            betTypeId: number,
            sportTypeId: number,
            refNo: string,
            userName: string,
            winLostDate: Date,
            refNoMixParlay: number,
            league: number,
            isOutright: boolean,
            betId: number,
            betCheck?: string,
            custId?: number,
            transDesc?: string) => {
            ResultFactory.CreateInstance(betTypeId).ViewResultDetails(
                matchId, 
                race, 
                betTypeId, 
                sportTypeId, 
                refNo, 
                userName, 
                winLostDate, 
                refNoMixParlay, 
                league, 
                isOutright, 
                betId, 
                betCheck, 
                custId, 
                transDesc);
        }
    }
}
