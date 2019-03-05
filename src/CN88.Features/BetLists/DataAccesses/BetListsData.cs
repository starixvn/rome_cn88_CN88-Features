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

        public static TicketDto Ticket3()
        {
            return new TicketDto
            {
                SportTypeId = 1,
                UserName = "U1",
                BetTypeId = 1,
                TransId = 6231231065,
                BetId = 0,
                Handicap1 = (decimal)0.25,
                Handicap2 = (decimal)0.0,
                MasterOdds = (decimal)0.75,
                OddsType = "4",
                Odds = (decimal)40.47,
                Stake = -100,
                ActualStake = -100,
                BetTeam = "a",
                Status = "Win",
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

        public static TicketDto Ticket2()
        {
            return new TicketDto
            {
                SportTypeId = 0,
                UserName = "U1",
                BetTypeId = 29,
                TransId = 9799000826,
                Handicap1 = (decimal)1,
                Handicap2 = (decimal)2,
                MasterOdds = (decimal)0,
                OddsType = "1",
                Odds = (decimal)1.0201,
                Stake = -10,
                ActualStake = -10,
                BetTeam = "1",
                Status = "running",
                StatusId = null,
                RefNo = "1100000059",
                TransDate = new System.DateTime(2014, 8, 26, 1, 52, 33),
                WinlostDate = new System.DateTime(2014, 8, 26),
                Winlost = 500,
                AgentWinlost = 465,
                AgentPositionTaking = (decimal)0.3,
                LiveHomeScore = 5,
                IP = "127.0.0.1",
                LeagueId = 23618,
                MatchId = 10262649,
                MatchCode = "B0826T04S01050",
                HomeId = 93486,
                AwayId = 107531,
                CustId = 791054,
                OriginalStake = -1,
                UGroup = "A",
                BetCheck = "2",
                OriginalBetType = 29
            };
        }

        public static TicketDto Ticket4()
        {
            return new TicketDto
            {
                SportTypeId = 0,
                UserName = "U1",
                BetTypeId = 29,
                TransId = 9799000830,
                Handicap1 = (decimal)1,
                Handicap2 = (decimal)2,
                MasterOdds = (decimal)0,
                OddsType = "1",
                Odds = (decimal)1.0201,
                Stake = -10,
                ActualStake = -10,
                BetTeam = "1",
                Status = "running",
                StatusId = null,
                RefNo = "1100000060",
                TransDate = new System.DateTime(2014, 8, 26, 1, 52, 33),
                WinlostDate = new System.DateTime(2014, 8, 26),
                Winlost = 500,
                AgentWinlost = 465,
                AgentPositionTaking = (decimal)0.3,
                LiveHomeScore = 5,
                IP = "127.0.0.1",
                LeagueId = 23618,
                MatchId = 10262649,
                MatchCode = "B0826T04S01050",
                HomeId = 93486,
                AwayId = 107531,
                CustId = 791054,
                OriginalStake = -1,
                UGroup = "A",
                BetCheck = "2",
                OriginalBetType = 29
            };
        }
    }
}
