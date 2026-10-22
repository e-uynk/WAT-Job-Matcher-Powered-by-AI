namespace WatJobMatcher.Api.Models
{
    public class OllamaParsedJobDto
    {
        public string? HostName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Positions { get; set; }
        public int? AvailableJobs { get; set; }
        public string? EarliestStartDate { get; set; }
        public string? LatestStartDate { get; set; }
        public string? CoupleHousingCompatibility { get; set; }
        public string? Notes { get; set; }
    }
}
