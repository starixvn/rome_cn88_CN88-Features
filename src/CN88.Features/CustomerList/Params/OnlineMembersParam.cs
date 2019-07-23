namespace CN88.Features.CustomerList.Params
{
    public class OnlineMembersParam
    {
        public OnlineMembersParam(
            int viewerId,
            int subAccId,
            bool isAllDownline,
            int pageSize,
            int pageIndex)
        {
            ViewerId = viewerId;
            SubAccId = subAccId;
            IsAllDownline = isAllDownline;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int ViewerId { get; }

        public int SubAccId { get; }

        public bool IsAllDownline { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public int TotalRow { get; set; }
    }
}