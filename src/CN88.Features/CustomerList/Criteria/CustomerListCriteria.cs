namespace CN88.Features.CustomerList.Criteria
{
    using CustomerList.Params;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class CustomerListCriteria : CriteriaBase
    {
        public CustomerListCriteria(CustomerListParam param)
        {
            ViewerId = param.ViewerId;
            DownlineUsername = string.IsNullOrWhiteSpace(param.DownlineUsername) ? string.Empty : param.DownlineUsername.Trim();
            SearchRoleId = param.SearchRoleId;
            Username = string.IsNullOrWhiteSpace(param.Username) ? string.Empty : param.Username.Trim();
            Status = param.Status;
            PageSize = param.PageSize;
            PageIndex = param.PageIndex;
        }

        [SpParam(SqlParamName = "CustId")]
        public int ViewerId { get; set; }

        public string DownlineUsername { get; set; }

        public int SearchRoleId { get; set; }

        [SpParam(SqlParamName = "SearchUsername")]
        public string Username { get; set; }

        [SpParam(SqlParamName = "SearchStatusId")]
        public int Status { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        [SpParam(SqlParamName = "TotalRows")]
        public int TotalCustomers { get; set; }

        public int UplineId { get; set; }

        public override string GetSettingKey()
            => "CS_Customer_DownlineList_Sel";

        public override bool IsValid()
            => ViewerId > 0;
    }
}