namespace WatJobMatcher.Api.Models
{
    public class JobListing
    {
        public int Id { get; set; }

        // İşveren / Şirket Adı
        public string HostName { get; set; } = string.Empty;

        // Konum Bilgileri
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        // Kontenjan ve Pozisyon
        public int AvailableJobs { get; set; }
        public string Positions { get; set; } = string.Empty;

        // Tarih Aralıkları
        public DateTime EarliestStartDate { get; set; }
        public DateTime LatestStartDate { get; set; }

        // Ücret ve Çalışma Saatleri
        public decimal Wage { get; set; }
        public int HoursPerWeek { get; set; }

        // Konaklama ve Eşleşme Durumu
        public bool HousingAvailable { get; set; }
        public string CoupleHousingCompatibility { get; set; } = "unknown"; // yes / no / unknown

        // Kural Motoru Sonuçları
        public string MatchStatus { get; set; } = "pending"; // suitable, verification_required, eliminated
        public double MatchScore { get; set; } = 0.0;
        public string MatchReason { get; set; } = string.Empty;

        // Kaynak Belge Takibi
        public string SourceDocument { get; set; } = string.Empty;
    }
}