using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatJobMatcher.Api.Data;
using WatJobMatcher.Api.Models;
using WatJobMatcher.Api.Services;

namespace WatJobMatcher.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobMatchController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly OllamaParserService _ollamaService;

        public JobMatchController(AppDbContext context, OllamaParserService ollamaService)
        {
            _context = context;
            _ollamaService = ollamaService;
        }

        [HttpPost("evaluate-files")]
        public async Task<IActionResult> EvaluateFiles([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest(new { message = "Lütfen en az bir PDF dosyası yükleyin." });

            // Veritabanından kullanıcı kriterlerini çekiyoruz
            var criteria = await _context.CriteriaProfiles.FirstOrDefaultAsync() ?? new CriteriaProfile
            {
                MinimumOpenings = 2,
                PreferredStates = "NY, FL",
                AcceptedPositions = "Housekeeping, Lifeguard",
                EarliestAcceptableStartDate = new DateTime(2026, 6, 1),
                LatestAcceptableStartDate = new DateTime(2026, 7, 1)
            };

            // PDF'leri sıralı değil, eşzamanlı (asenkron) işlemek için görev listesi oluşturuyoruz
            var evaluationTasks = files.Select(async file =>
            {
                string extractedText = string.Empty;
                try
                {
                    using var stream = file.OpenReadStream();
                    using var pdfDocument = UglyToad.PdfPig.PdfDocument.Open(stream);
                    var sb = new System.Text.StringBuilder();
                    foreach (var page in pdfDocument.GetPages())
                    {
                        sb.AppendLine(page.Text);
                    }
                    extractedText = sb.ToString();
                }
                catch (Exception)
                {
                    extractedText = "";
                }

                // AI servisine isteği gönder ve sonucu döndür
                return await _ollamaService.EvaluateJobWithCriteriaAsync(extractedText, file.FileName, criteria);
            });

            // Tüm görevleri aynı anda başlat ve hepsinin bitmesini bekle
            var results = await Task.WhenAll(evaluationTasks);

            return Ok(results);
        }
    }
}