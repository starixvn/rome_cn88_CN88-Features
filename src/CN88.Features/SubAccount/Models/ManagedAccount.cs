namespace CN88.Features.SubAccount.Models
{
    public class ManagedAccount
    {
        public ManagedAccount()
        {
        }

        public ManagedAccount(
            int custId,
            string username,
            string firstName,
            bool assigned)
        {
            CustId = custId;
            Username = username;
            FirstName = firstName;
            Assigned = assigned;
        }

        public int CustId { get; }

        public string Username { get; }

        public string FirstName { get; }

        public bool Assigned { get; }
    }
}