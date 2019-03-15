namespace BetLists.Result {
    export class SportsbookResultDetails extends BaseResultDetails implements ResultDetails {
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
            const params = "?" + $.param({
                matchid: matchId,
                bettypeid: betTypeId,
                race: race,
                sportId: sportTypeId,
                leagueId: league,
                isOutright: isOutright,
                refnum: refNo,
                betid: betId,
                isViewResultFromBetList: true
            });

            var options = {
                url: this.ResultDetailsPath + params,
                title: "",
                width: 730,
                height: 300
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        };
    }

    export class MixParlayResultDetails extends SportsbookResultDetails implements ResultDetails {
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
            const params = "?" + $.param({
                refnum: refNoMixParlay,
                wldate: winLostDate,
                bettypeid: betTypeId,
                matchid: 1,
                isViewResultFromBetList: true
            });

            var options = {
                url: this.ResultDetailsPath + params,
                title: "",
                width: 800,
                height: 550
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        };
    }

    export class FastMarketsResultDetails extends BaseResultDetails implements ResultDetails {
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
            hdp2?: string) => {
            const params = "?" + $.param({
                matchid: matchId,
                bettypeid: betTypeId,
                race: race,
                sportId: sportTypeId,
                leagueId: league,
                isOutright: isOutright,
                refnum: refNo,
                betid: betId,
                isViewResultFromBetList: true,
                hdp2: hdp2
            });

            var options = {
                url: this.ResultDetailsPath + params,
                title: "",
                width: 730,
                height: 300
            };

            Common.PopupDetails.Instance(window.top).Show(options);
        }
    }
}
