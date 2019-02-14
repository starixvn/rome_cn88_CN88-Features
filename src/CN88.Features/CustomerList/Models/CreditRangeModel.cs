namespace CN88.Features.CustomerList.Models
{
    public class CreditRangeModel
    {
        public CreditRangeModel()
        {
        }

        public CreditRangeModel(
            decimal usedCredit,
            decimal betCredit,
            decimal minCredit,
            decimal maxCredit)
        {
            UsedCredit = usedCredit;
            BetCredit = betCredit;
            MinCredit = minCredit;
            MaxCredit = maxCredit;
        }

        public decimal UsedCredit { get; }

        public decimal BetCredit { get; }

        public decimal MinCredit { get; }

        public decimal MaxCredit { get; }
    }
}
