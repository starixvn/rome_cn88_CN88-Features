namespace CN88.Features.Site.BetLists.Models
{
    public class SportStakeCondition
    {
        public int SportGroupId { get; set; }

        public string SportGroupName { get; set; }

        public int StakeMode { get; set; }

        public decimal StakeLimit { get; set; }
    }
}