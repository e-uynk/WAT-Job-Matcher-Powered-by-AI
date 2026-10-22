namespace WatJobMatcher.Api.Models
{
    public class JobEvaluationResultDto
    {
        public string? FileName { get; set; }
        public string? HostName { get; set; }
        public string? City { get; set; }
        public string? Positions { get; set; }
        public string? Status { get; set; }      // "Uygun", "Elendi", "Doğrulama Gerekiyor"
        public string? Reason { get; set; }      // Değerlendirme gerekçesi
        public double Score { get; set; }        // 10 üzerinden puan (örn: 8.5)
    }
}