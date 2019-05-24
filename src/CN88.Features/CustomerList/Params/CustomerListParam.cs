namespace CN88.Features.CustomerList.Params
{
    public class CustomerListParam
    {
        public CustomerListParam(
            int viewerId,
            string downlineUsername,
            int searchRoleId,
            string username,
            int status,
            int pageSize,
            int pageIndex)
        {
            ViewerId = viewerId;
            DownlineUsername = downlineUsername;
            SearchRoleId = searchRoleId;
            Username = username;
            Status = status;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int ViewerId { get; }

        public string DownlineUsername { get; set; }

        public int SearchRoleId { get; set; }

        public string Username { get; }

        public int Status { get; }

        public int PageSize { get; }

        public int PageIndex { get; }
    }
}