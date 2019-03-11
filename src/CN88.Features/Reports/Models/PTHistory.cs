namespace CN88.Features.Reports.Models
{
    using System;

    public class PTHistory
    {
        public PTHistory()
        {
        }

        public PTHistory(
            DateTime effectiveDate,
            int sportType,
            int betType,
            int productId,
            int newPT,
            int oldPT)
        {
            EffectiveDate = effectiveDate;
            SportType = sportType;
            BetType = betType;
            ProductId = productId;
            NewPT = newPT;
            OldPT = oldPT;
        }

        public DateTime EffectiveDate { get; }

        public int SportType { get; }

        public int BetType { get; }

        public int ProductId { get; }

        public int NewPT { get; }

        public int OldPT { get; }
    }
}