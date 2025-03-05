using Microsoft.EntityFrameworkCore;

namespace OpenIdDictPractice.Configure
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.UseOpenIddict();

        }
    }
}