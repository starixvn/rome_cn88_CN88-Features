namespace CN88.Features.CustomerList.Params
{
    public class CustomerListParam
    {
        public CustomerListParam(
            int viewerId,
            int subAccountId,
            string downlineUsername,
            int searchRoleId,
            string username,
            int status,
            int pageSize,
            int pageIndex)
        {
            ViewerId = viewerId;
            SubAccountId = subAccountId;
            DownlineUsername = downlineUsername;
            SearchRoleId = searchRoleId;
            Username = username;
            Status = status;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int ViewerId { get; }

        public int SubAccountId { get; }

        public string DownlineUsername { get; }

        public int SearchRoleId { get; }

        public string Username { get; }

        public int Status { get; }

        public int PageSize { get; }

        public int PageIndex { get; }
    }
}