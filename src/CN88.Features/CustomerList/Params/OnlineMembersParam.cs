namespace CN88.Features.CustomerList.Params
{
    public class OnlineMembersParam
    {
        public OnlineMembersParam(
            int viewerId, 
            int pageSize,
            int pageIndex)
        {
            ViewerId = viewerId;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int ViewerId { get; }

        public int PageSize { get; }

        public int PageIndex { get; }
    }
}
