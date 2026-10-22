using System.Text;
using System.Text.Json;
using WatJobMatcher.Api.Models;

namespace WatJobMatcher.Api.Services
{
    public class OllamaParserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OllamaParserService> _logger;

        public OllamaParserService(HttpClient httpClient, ILogger<OllamaParserService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            // Eşzamanlı işlemlerde timeout yememek için süreyi 10 dakikaya çıkardık
            _httpClient.Timeout = TimeSpan.FromMinutes(10); 
        }

        public async Task<object> EvaluateJobWithCriteriaAsync(string jobText, string fileName, CriteriaProfile criteria)
        {
            var fallback = ParseFromFileName(fileName);

            try
            {
                var prompt = $@"
Sen bir Work and Travel iş analiz asistanısın. Aşağıdaki iş ilanı metnini kullanıcının kriterlerine göre incele.

KULLANICI KRİTERLERİ:
- İstenen Eyaletler: {criteria.PreferredStates}
- Kabul Edilen Pozisyonlar: {criteria.AcceptedPositions}
- En Erken Başlangıç: {criteria.EarliestAcceptableStartDate:yyyy-MM-dd}
- En Geç Başlangıç: {criteria.LatestAcceptableStartDate:yyyy-MM-dd}

İŞ İLANI METNİ:
{jobText}

GÖREVİN: İlanı analiz et ve SADECE şu JSON formatında yanıt ver. Puan verme, sadece eşleşme durumlarını (true/false) yaz:
{{
  ""HostName"": ""İş yerinin adı"",
  ""City"": ""Şehir ve Eyalet"",
  ""Positions"": ""İlandaki pozisyon"",
  ""IsStateMatch"": true veya false,
  ""IsPositionMatch"": true veya false,
  ""IsDateMatch"": true veya false,
  ""Reason"": ""Kriterlerin hangilerinin uyup uymadığına dair 1 cümlelik Türkçe açıklama""
}}
";

                var requestBody = new
                {
                    model = "qwen2.5:3b", // Doğru ve güncel model adımız
                    prompt = prompt,
                    stream = false
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://localhost:11434/api/generate", content);

                if (!response.IsSuccessStatusCode)
                {
                    return GetDefaultEvaluation(fallback);
                }

                var responseString = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseString);
                var root = doc.RootElement;

                if (root.TryGetProperty("response", out var aiProp))
                {
                    var aiText = aiProp.GetString();
                    if (!string.IsNullOrEmpty(aiText))
                    {
                        aiText = aiText.Replace("```json", "").Replace("```", "").Trim();
                        int start = aiText.IndexOf('{');
                        int end = aiText.LastIndexOf('}');
                        if (start != -1 && end != -1 && end > start)
                        {
                            var jsonClean = aiText.Substring(start, (end - start) + 1);
                            using var jsonDoc = JsonDocument.Parse(jsonClean);
                            var obj = jsonDoc.RootElement;

                            // 2. KATMAN: C# İLE KESİN VE HATA KABUL ETMEZ PUANLAMA MANTIĞI
int score = 1; 

bool isStateMatch = obj.TryGetProperty("IsStateMatch", out var sm) && sm.ValueKind == JsonValueKind.True;
bool isPosMatch = obj.TryGetProperty("IsPositionMatch", out var pm) && pm.ValueKind == JsonValueKind.True;
bool isDateMatch = obj.TryGetProperty("IsDateMatch", out var dm) && dm.ValueKind == JsonValueKind.True;

string reason = obj.TryGetProperty("Reason", out var r) ? r.GetString() : "Değerlendirildi.";

// GÜVENLİK DUVARI: Açıklamada olumsuzluk varsa true gelse bile puanı düşürüyoruz
if (reason.Contains("uyuşmuyor", StringComparison.OrdinalIgnoreCase) || 
    reason.Contains("sağlanmıyor", StringComparison.OrdinalIgnoreCase) ||
    reason.Contains("eşleşme bulunmamaktadır", StringComparison.OrdinalIgnoreCase) ||
    reason.Contains("eşleşmemektedir", StringComparison.OrdinalIgnoreCase))
{
    isStateMatch = false;
    isPosMatch = false;
    isDateMatch = false;
}

if (isStateMatch) score += 3;
if (isPosMatch) score += 3;
if (isDateMatch) score += 3;

string status = score >= 7 ? "Uygun" : "Elendi";

return new
{
    HostName = obj.TryGetProperty("HostName", out var h) ? h.GetString() : fallback.HostName,
    City = obj.TryGetProperty("City", out var c) ? c.GetString() : fallback.City,
    Positions = obj.TryGetProperty("Positions", out var p) ? p.GetString() : fallback.Positions,
    Score = score,
    Status = status,
    Reason = reason
};
                        }
                    }
                }

                return GetDefaultEvaluation(fallback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yapay Zeka veya JSON parse hatası");
                return GetDefaultEvaluation(fallback);
            }
        }

        private OllamaParsedJobDto ParseFromFileName(string fileName)
        {
            string cleanName = Path.GetFileNameWithoutExtension(fileName);
            string hostName = cleanName;
            string position = "General Worker";

            if (cleanName.Contains("-"))
            {
                var parts = cleanName.Split('-');
                if (parts.Length > 0) hostName = parts[0].Trim();
                if (parts.Length > 1) position = parts[1].Trim();
            }

            return new OllamaParsedJobDto
            {
                HostName = hostName,
                City = "ABD",
                Positions = position
            };
        }

        private object GetDefaultEvaluation(OllamaParsedJobDto fallback)
        {
            return new
            {
                HostName = fallback.HostName,
                City = "Bilinmiyor",
                Positions = fallback.Positions,
                Score = 5,
                Status = "Elendi",
                Reason = "Dosya yapay zeka tarafından tam okunamadı, manuel kontrol önerilir."
            };
        }
    }
}