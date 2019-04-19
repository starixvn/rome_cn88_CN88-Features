namespace CN88.Features.Models
{
    using System;

    public interface IUser
    {
        int Id { get; }

        string Name { get; }

        string NickName { get; }

        string Language { get; }

        string LanguageKey { get; }

        byte LevelId { get; }

        bool IsInternal { get; }

        int SubAccountId { get; }

        string IP { get; }

        bool AccountManagementEditView { get; }

        bool AccountManagementViewOnly { get; }

        bool IsAdminCanViewVNIP { get; }

        int AdminId { get; }

        DateTime LimitedDateToShowInfo { get; }
    }
}