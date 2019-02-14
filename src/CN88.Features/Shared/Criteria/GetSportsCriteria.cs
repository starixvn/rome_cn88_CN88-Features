namespace CN88.Features.Shared.Criteria
{
    using Fanex.Data.Repository;

    public class GetSportsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_SportType_Sel";

        public override bool IsValid() => true;
    }
}