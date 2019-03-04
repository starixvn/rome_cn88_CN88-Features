namespace CN88.Features.Models
{
    public interface IUser
    {
        int Id { get; }

        string Name { get; }

        string NickName { get; }

        string Language { get; }

        byte LevelId { get; }

        bool IsInternal { get; }

        int SubAccountId { get; }

        string IP { get; }

        bool HasUpdatePermission { get; }

        bool HasAddNewPermission { get; }

        bool IsAdminCanViewVNIP { get; }
    }
}