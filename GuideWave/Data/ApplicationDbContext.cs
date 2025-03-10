using GuideWave.Models;
using Microsoft.EntityFrameworkCore;

namespace GuideWave.Data
{
    public class ApplicationDbContext :DbContext

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

       public DbSet<Guide> Guides { get; set; }
       public DbSet<Tourists> Tourists { get; set; }
       public DbSet<Review> Reviews { get; set; }
       public DbSet<Place> Places { get; set; }







    }
}
