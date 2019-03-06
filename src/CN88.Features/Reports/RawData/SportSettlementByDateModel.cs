namespace CN88.Features.Reports.RawData
{
    public class SportSettlementByDateModel
    {
        public string EventDate { get; set; }

        public int SportTypeGroup { get; set; }

        public long CompletedEvent { get; set; }

        public long IncompletedEvent { get; set; }
    }
}