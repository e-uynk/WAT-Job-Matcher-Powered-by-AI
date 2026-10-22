using WatJobMatcher.Api.Models;

namespace WatJobMatcher.Api.Services
{
    public class JobMatcherService
    {
        public List<JobEvaluationResultDto> EvaluateAll(List<JobListing> listings, CriteriaProfile criteria)
        {
            var results = new List<JobEvaluationResultDto>();

            foreach (var job in listings)
            {
                var (status, score, reason) = EvaluateSingle(job, criteria);
                results.Add(new JobEvaluationResultDto
                {
                    HostName = job.HostName,
                    City = job.City,
                    State = job.State,
                    Positions = job.Positions,
                    MatchStatus = status,
                    MatchScore = score,
                    MatchReason = reason
                });
            }

            return results;
        }

        private (string status, double score, string reason) EvaluateSingle(JobListing job, CriteriaProfile criteria)
        {
            // Kontenjan kontrolü
            if (job.AvailableJobs < criteria.MinimumOpenings)
            {
                return ("eliminated", 0.0, $"Yetersiz kontenjan: {job.AvailableJobs} kişi (En az {criteria.MinimumOpenings} gerekli).");
            }

            // Tarih kontrolü
            if (job.EarliestStartDate < criteria.EarliestAcceptableStartDate)
            {
                return ("eliminated", 0.0, $"Başlangıç tarihi kriterden önce: {job.EarliestStartDate:yyyy-MM-dd}.");
            }

            // Konaklama kontrolü
            if (criteria.CoupleHousingRequired && job.CoupleHousingCompatibility == "no")
            {
                return ("eliminated", 0.0, "Çift konaklaması uygun değil.");
            }

            if (job.CoupleHousingCompatibility == "unknown")
            {
                return ("verification_required", 0.7, "Kriterler uyuyor ancak çift konaklama durumu belirsiz, acenteden doğrulanmalı.");
            }

            return ("suitable", 1.0, "Tüm kriterler başarıyla sağlandı.");
        }
    }

    public class JobEvaluationResultDto
    {
        public string? HostName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Positions { get; set; }
        public string MatchStatus { get; set; } = "unknown";
        public double MatchScore { get; set; }
        public string MatchReason { get; set; } = string.Empty;
    }
}