using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Atelier_2.Models;
using Microsoft.AspNetCore.Identity;

namespace Atelier_2.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet for Product entity
        public DbSet<Product> Products { get; set; }

        // DbSet for Client entity
        public DbSet<Client> Clients { get; set; }
        // DbSet for Reclamation entity
        public DbSet<Reclamation> Reclamations { get; set; }
        // DbSet pour les interventions
        public DbSet<Intervention> Interventions { get; set; }

        // DbSet pour les pièces
        public DbSet<Piece> Pieces { get; set; }
    }
}
