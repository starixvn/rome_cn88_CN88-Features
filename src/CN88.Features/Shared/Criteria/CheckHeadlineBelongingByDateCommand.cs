namespace CN88.Features.Shared.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class CheckHeadlineBelongingByDateCommand : NonQueryCommand
    {
        public CheckHeadlineBelongingByDateCommand(
            int headlineId,
            int custId,
            DateTime fromDate,
            DateTime toDate)
        {
            HeadlineId = headlineId;
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public int HeadlineId { get; }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool IsBelonging { get; set; }

        public override string GetSettingKey()
            => "CS_Customer_CheckBelongingHeadline_ByDate";

        public override bool IsValid()
            => HeadlineId > 0 && CustId > 0 && FromDate <= ToDate;
    }
}
