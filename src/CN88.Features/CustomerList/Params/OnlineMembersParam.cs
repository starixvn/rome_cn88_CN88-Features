namespace CN88.Features.CustomerList.Params
{
    public class OnlineMembersParam
    {
        public OnlineMembersParam(int viewerId)
        {
            ViewerId = viewerId;
        }

        public int ViewerId { get; }
    }
}
