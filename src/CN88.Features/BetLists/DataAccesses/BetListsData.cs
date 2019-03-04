namespace CN88.Features.BetLists.DataAccesses
{
    using Models;

    public static class BetListsDataSample
    {
        public static TicketDto Ticket1()
        {
            return new TicketDto
            {
                SportTypeId = 1,
                UserName = "U1",
                BetTypeId = 1,
                TransId = 6231231062,
                BetId = 0,
                Handicap1 = (decimal)0.25,
                Handicap2 = (decimal)0.0,
                MasterOdds = (decimal)0.75,
                OddsType = "4",
                Odds = (decimal)40.47,
                Stake = -100,
                ActualStake = -100,
                BetTeam = "a",
                Status = "Refund",
                StatusId = null,
                RefNo = "100000000000",
                RefCode = 0,
                WalletType = 0,
                TransDate = new System.DateTime(2014, 8, 26, 1, 52, 33),
                WinlostDate = new System.DateTime(2014, 8, 26),
                Winlost = 500,
                AgentWinlost = 465,
                AgentPositionTaking = (decimal)0.93,
                LiveHomeScore = 5,
                IP = "127.0.0.1",
                LeagueId = 23618,
                MatchId = 10262649,
                MatchCode = "B0826T04S01050",
                HomeId = 93486,
                AwayId = 107531,
                CustId = 14026704,
                OriginalStake = -1,
                UGroup = "A"
            };
        }
    }
}
