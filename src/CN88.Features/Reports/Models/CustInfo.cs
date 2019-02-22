namespace CN88.Features.Reports.Models
{
    public class CustInfo
    {
        public CustInfo(
            int custId,
            string custName,
            byte custLevelId,
            bool isOwner)
        {
            CustId = custId;
            CustName = custName;
            CustLevelId = custLevelId;
            IsOwner = isOwner;
        }

        public int CustId { get; }

        public string CustName { get; }

        public byte CustLevelId { get; }

        public bool IsOwner { get; }
    }
}