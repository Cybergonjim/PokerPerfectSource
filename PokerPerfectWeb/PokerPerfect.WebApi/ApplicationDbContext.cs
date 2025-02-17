using PokerPerfect.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace PokerPerfect.WebApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }

        public DbSet<Blind> Blinds { get; set; }
        public DbSet<Payout> Payouts { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Chipset> Chipsets { get; set; }
        public DbSet<Chip> Chips { get; set; }
    }
}
