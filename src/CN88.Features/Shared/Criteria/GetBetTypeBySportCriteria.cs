using Fanex.Data;
using Fanex.Data.Repository;

namespace CN88.Features.Shared.Criteria
{
    public class GetBetTypeBySportCriteria : CriteriaBase
    {
        public GetBetTypeBySportCriteria(int sportTypeId)
        {
            SportTypeId = sportTypeId;
        }

        [SpParam(SqlParamName = "SportType")]
        public int SportTypeId { get; }

        public override string GetSettingKey() => "CS_BetType_Sel";

        public override bool IsValid() => true;
    }
}