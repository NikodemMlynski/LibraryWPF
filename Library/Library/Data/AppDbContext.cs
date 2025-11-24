using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(
                "Host=ep-winter-star-agwyqo9p-pooler.c-2.eu-central-1.aws.neon.tech;" +
                "Port=5432;" +
                "Database=neondb;" +
                "Username=neondb_owner;" +
                "Password=npg_3Bflw2pFNeDP;" +
                "SslMode=Require;" +
                "Trust Server Certificate=true;");
        }
    }
}
