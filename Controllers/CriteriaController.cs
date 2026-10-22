using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatJobMatcher.Api.Data;
using WatJobMatcher.Api.Models;

namespace WatJobMatcher.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CriteriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CriteriaController(AppDbContext context)
        {
            _context = context;
        }

       [HttpGet]
public async Task<IActionResult> GetCriteria()
{
    var criteria = await _context.CriteriaProfiles.FirstOrDefaultAsync();
    if (criteria == null)
    {
        // Kayıt yoksa boş dönmek yerine varsayılan bir nesne dönelim ki frontend patlamasın
        return Ok(new CriteriaProfile
        {
            ProfileName = "Varsayılan Profil",
            MinimumOpenings = 2,
            EarliestAcceptableStartDate = new DateTime(2026, 6, 20),
            AllowedPositions = "Housekeeping, Laundry Attendant",
            RejectedPositions = "Kitchen, Dishwasher, Lifeguard",
            PreferredStates = "NY, FL, CA, ME",
            CoupleHousingRequired = true
        });
    }
    return Ok(criteria);
}

        // POST: api/criteria (Kullanıcının değiştirdiği kriterleri kaydeder/günceller)
        [HttpPost]
        public async Task<IActionResult> SaveCriteria([FromBody] CriteriaProfile newCriteria)
        {
            var existing = await _context.CriteriaProfiles.FirstOrDefaultAsync();
            if (existing == null)
            {
                _context.CriteriaProfiles.Add(newCriteria);
            }
            else
            {
                existing.ProfileName = newCriteria.ProfileName;
                existing.MinimumOpenings = newCriteria.MinimumOpenings;
                existing.EarliestAcceptableStartDate = newCriteria.EarliestAcceptableStartDate;
                existing.LatestAcceptableStartDate = newCriteria.LatestAcceptableStartDate;
                existing.PreferredStates = newCriteria.PreferredStates;
                existing.AcceptedPositions = newCriteria.AcceptedPositions;
                existing.DoubleAccommodationRequired = newCriteria.DoubleAccommodationRequired;
                existing.CustomNotes = newCriteria.CustomNotes;
                existing.AllowedPositions = newCriteria.AllowedPositions;
                existing.RejectedPositions = newCriteria.RejectedPositions;
                existing.CoupleApplicationRequired = newCriteria.CoupleApplicationRequired;
                existing.CoupleHousingRequired = newCriteria.CoupleHousingRequired;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Kriterler başarıyla kaydedildi!", data = newCriteria });
        }
    }
}
