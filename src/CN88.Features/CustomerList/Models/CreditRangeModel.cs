namespace CN88.Features.CustomerList.Models
{
    public class CreditRangeModel
    {
        public CreditRangeModel()
        {
        }

        public CreditRangeModel(
            decimal usedCredit,
            decimal uplineCredit,
            decimal uplineUsedCredit,
            decimal minCredit,
            decimal maxCredit)
        {
            UsedCredit = usedCredit;
            UplineCredit = uplineCredit;
            UplineUsedCredit = uplineUsedCredit;
            MinCredit = minCredit;
            MaxCredit = maxCredit;
        }

        public CreditRangeModel(
            decimal usedCredit,
            CreditRangeModel memberCredit)
        {
            UsedCredit = usedCredit;
            UplineCredit = memberCredit.UplineCredit;
            UplineUsedCredit = memberCredit.UplineUsedCredit;
            MinCredit = usedCredit;
            MaxCredit = memberCredit.UplineCredit - memberCredit.UplineUsedCredit + usedCredit;
        }

        public decimal UsedCredit { get; }

        public decimal UplineCredit { get; }

        public decimal UplineUsedCredit { get; }

        public decimal MinCredit { get; }

        public decimal MaxCredit { get; }
    }
}
