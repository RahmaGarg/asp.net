using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Atelier_2.Models;
using Microsoft.AspNetCore.Identity;

namespace Atelier_2.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        // Constructor to pass options to the base class (IdentityDbContext)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet for Product entity
        public DbSet<Product> Products { get; set; }

        // DbSet for Client entity
        public DbSet<Client> Clients { get; set; }

        // DbSet for Reclamation entity
        public DbSet<Reclamation> Reclamations { get; set; }

        // DbSet for Intervention entity
        public DbSet<Intervention> Interventions { get; set; }

        // DbSet for Piece entity
        public DbSet<Piece> Pieces { get; set; }

        // Overriding the OnModelCreating method to configure relationships and entities
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure base class configurations are applied

            // Configure the relationship between Intervention and IdentityUser
            modelBuilder.Entity<Intervention>()
                .HasOne(i => i.Technicien)  // Relationship with IdentityUser (Technicien)
                .WithMany()  // One technicien can have many interventions
                .HasForeignKey(i => i.TechnicienId)  // Foreign key property
                .OnDelete(DeleteBehavior.Restrict);  // Prevent deletion of Technicien if they have interventions

            // Add other model configurations here if needed (e.g., for Client, Product, etc.)
        }
    }
}
