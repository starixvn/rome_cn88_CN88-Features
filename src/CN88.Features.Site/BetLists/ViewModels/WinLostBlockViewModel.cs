namespace CN88.Features.Site.BetLists.ViewModels
{
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Features.BetLists.Helpers;
    using Features.Extensions;

    public class WinLostBlockViewModel
    {
        public WinLostBlockViewModel(ITicket ticket, int divisor)
        {
            SetData(ticket, divisor);
        }

        public string WinLost { get; private set; }

        public bool CommVisibility { get; }

        public string Comm { get; private set; }

        private void SetData(ITicket ticket, int divisor)
        {
            if (BetTypeHelper.IsJackpotContribution(ticket.BetTypeId))
            {
                WinLost = "-";
                Comm = string.Empty;

                return;
            }

            switch (ticket.Status.ToLowerInvariant())
            {
                case BetStatus.Running:
                case BetStatus.Waiting:
                case BetStatus.Reject:
                    WinLost = "0.00";
                    Comm = string.Empty;
                    break;

                case BetStatus.Void:
                    WinLost = ticket.BetTypeId == 500 ? "(NW) 0.00" : "0.00";
                    Comm = "0.00";
                    break;

                default:
                    WinLost = ticket.BetTypeId == 500
                        ? "(NW) " + ConvertByBetType.Winloss((ticket.ActualStake / divisor).Round())
                        : ConvertByBetType.Winloss(((ticket.Winlost + ticket.PlayerComm) / divisor).Round());
                    Comm = ticket.BetTypeId == 500
                        ? ConvertByBetType.Winloss(ticket.PlayerComm)
                        : ConvertByBetType.Comm(ticket.PlayerComm);
                    break;
            }
        }
    }
}