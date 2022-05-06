using Microsoft.EntityFrameworkCore;

namespace SGPB.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {



        }
    }
}
