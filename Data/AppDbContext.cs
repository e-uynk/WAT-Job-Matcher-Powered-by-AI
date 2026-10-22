using Microsoft.EntityFrameworkCore;
using WatJobMatcher.Api.Models;

namespace WatJobMatcher.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<CriteriaProfile> CriteriaProfiles { get; set; }
    }
}