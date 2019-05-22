namespace CN88.Features.CustomerList.Criteria
{
    using CustomerList.Params;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetOnlineMemberListCriteria : CriteriaBase
    {
        public GetOnlineMemberListCriteria(OnlineMembersParam param)
        {
            ViewerId = param.ViewerId;
            PageSize = param.PageSize;
            PageIndex = param.PageIndex;
        }

        [SpParam(SqlParamName = "CustId")]
        public int ViewerId { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "CS_Customer_OnlineUserList_Sel";

        public override bool IsValid()
            => ViewerId > 0 
            && PageIndex > 0
            && PageSize > 0;
    }
}