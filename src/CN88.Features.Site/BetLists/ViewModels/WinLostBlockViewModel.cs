namespace CN88.Features.Site.BetLists.ViewModels
{
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Features.BetLists.Helpers;

    public class WinLostBlockViewModel
    {
        public WinLostBlockViewModel(ITicket ticket)
        {
            SetData(ticket);
            CommVisibility = DisplayComm(ticket);
        }

        public string WinLost { get; private set; }

        public bool CommVisibility { get; }

        public string Comm { get; private set; }

        private static bool DisplayComm(ITicket ticket) => false;

        private void SetData(ITicket ticket)
        {
            if (BetTypeHelper.IsJackpotContribution(ticket.BetTypeId))
            {
                WinLost = "-";
                Comm = string.Empty;

                return;
            }

            var status = ticket.Status.ToLowerInvariant();

            switch (status)
            {
                case BetStatus.Running:
                case BetStatus.Waiting:
                    WinLost = "-";
                    Comm = string.Empty;
                    break;

                case BetStatus.Void:
                    WinLost = ticket.BetTypeId == 500 ? "(NW) 0.00" : string.Empty;
                    Comm = "0.00";
                    break;

                case BetStatus.Reject:
                    WinLost = "0.00";
                    Comm = string.Empty;
                    break;

                default:
                    WinLost = ticket.BetTypeId == 500 ? "(NW) " + ConvertByBetType.Winloss(ticket.ActualStake) : ConvertByBetType.Winloss(ticket.Winlost);
                    Comm = ticket.BetTypeId == 500 ? ConvertByBetType.Winloss(ticket.PlayerComm) : ConvertByBetType.Comm(ticket.PlayerComm);
                    break;
            }
        }
    }
}