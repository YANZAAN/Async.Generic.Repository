using Microsoft.EntityFrameworkCore;

using NET.Repository.Tests.Fakes.DTO;

namespace NET.Repository.Tests.Fakes
{
    public class CartoonContext : DbContext
    {
        public virtual DbSet<Episode> Episodes { get; set; }
        public virtual DbSet<Cartoon> Cartoons { get; set; }

        public CartoonContext(DbContextOptions<CartoonContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cartoon>(conf =>
            {
                // conf.ToTable("Cartoon");

                conf.HasData(
                    new Cartoon() { Id = 1, Name = "My Little Pony" },
                    new Cartoon() { Id = 2, Name = "Rick and Morty" });
            });
            modelBuilder.Entity<Episode>(conf =>
            {
                conf.HasOne(episode => episode.RelatedCartoon)
                    .WithMany(cartoon => cartoon.Episodes)
                    .HasForeignKey(key => key.CartoonId);

                conf.HasData(
                    new Episode() { Id = 1, CartoonId = 1, Name = "The Ticket Master" },
                    new Episode() { Id = 2, CartoonId = 1, Name = "Applebuck Season" },
                    new Episode() { Id = 3, CartoonId = 2, Name = "Pilot" },
                    new Episode() { Id = 4, CartoonId = 2, Name = "Lawnmower Dog" });
            });
        }
    }
}