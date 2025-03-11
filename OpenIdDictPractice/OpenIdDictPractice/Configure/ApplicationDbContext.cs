using Microsoft.EntityFrameworkCore;

namespace OpenIdDictPractice.Configure
{
    /// <summary>
    /// default DBContext for OpenIddict
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="options"><see cref="DbContextOptions"/></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// On Model create.
        /// </summary>
        /// <param name="builder"><see cref="ModelBuilder"/></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.UseOpenIddict();

        }
    }
}