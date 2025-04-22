using Microsoft.EntityFrameworkCore;

namespace ReceteSistemiAPI
{
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }

        public DbSet<Recete> Receteler { get; set; }
        public DbSet<Ilac> Ilaclar { get; set; }
        public DbSet<ReceteIlac> ReceteIlaclar { get; set; }
        public DbSet<Hasta> Hastalar { get; set; }
        public DbSet<Veteriner> Veterinerler { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReceteIlac>()
                .HasKey(ri => new { ri.ReceteId, ri.IlacId });

            modelBuilder.Entity<ReceteIlac>()
                .HasOne(ri => ri.Recete)
                .WithMany(r => r.ReceteIlaclar)
                .HasForeignKey(ri => ri.ReceteId);

            modelBuilder.Entity<ReceteIlac>()
                .HasOne(ri => ri.Ilac)
                .WithMany(i => i.ReceteIlaclar)
                .HasForeignKey(ri => ri.IlacId);
        }
    }
}