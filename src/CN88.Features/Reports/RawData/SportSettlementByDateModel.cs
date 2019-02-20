namespace CN88.Features.Reports.RawData
{
    public class SportSettlementByDateModel
    {
        public string EventDate { get; set; }

        public string SportTypeGroup { get; set; }

        public long CompletedEvent { get; set; }

        public long IncompletedEvent { get; set; }
    }
}
