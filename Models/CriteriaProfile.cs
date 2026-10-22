namespace WatJobMatcher.Api.Models
{
    public class CriteriaProfile
    {
        public int Id { get; set; }
        public string? ProfileName { get; set; }
        public int MinimumOpenings { get; set; } = 2;
        public DateTime EarliestAcceptableStartDate { get; set; }
        public DateTime LatestAcceptableStartDate { get; set; }
        public string? PreferredStates { get; set; }
        public string? AcceptedPositions { get; set; }
        public string? AllowedPositions { get; set; }
        public string? RejectedPositions { get; set; }
        public bool DoubleAccommodationRequired { get; set; }
        public bool CoupleApplicationRequired { get; set; }
        public bool CoupleHousingRequired { get; set; }
        public string? CustomNotes { get; set; } // Yeni: İnsanların eklemek istediği diğer serbest kriterler
    }
}