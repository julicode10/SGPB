using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SGPB.Web.Data.Entities;

namespace SGPB.Web.Data
{
        public class ApplicationDbContext : IdentityDbContext<User>
        {
                public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
                {
                }

                public DbSet<Category> Categories { get; set; }

                public DbSet<Editorial> Editoriales { get; set; }


                public DbSet<DocumentType> DocumentTypes { get; set; }

                public DbSet<Lending> Lendings { get; set; }

                public DbSet<Book> Books { get; set; }
                public DbSet<BookImage> BookImages { get; set; }


                protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                        base.OnModelCreating(modelBuilder);
                        modelBuilder.Entity<Category>()
                                         .HasIndex(c => c.Name)
                                         .IsUnique();

                        modelBuilder.Entity<Editorial>()
                                         .HasIndex(e => e.Name)
                                         .IsUnique();

                        modelBuilder.Entity<DocumentType>()
                                         .HasIndex(d => d.Name)
                                         .IsUnique();

                        modelBuilder.Entity<Book>()
                                         .HasIndex(b => b.Serial)
                                         .IsUnique();
                }
        }

}
