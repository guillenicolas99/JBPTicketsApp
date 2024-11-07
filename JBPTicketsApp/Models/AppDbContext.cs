using JBPTicketsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JBPTicketsApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<NivelLiderazgo> NivelesLiderazgo { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Red> Redes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de relaciones
            modelBuilder.Entity<Evento>()
                .HasMany(e => e.Tickets)
                .WithOne(t => t.Evento)
                .HasForeignKey(t => t.IdEvento);

            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Tickets)
                .WithOne(t => t.Categoria)
                .HasForeignKey(t => t.IdCategoria);

            modelBuilder.Entity<Persona>()
                .HasMany(p => p.Tickets)
                .WithOne(t => t.Persona)
                .HasForeignKey(t => t.IdPersona);

            modelBuilder.Entity<Red>()
                .HasMany(r => r.Personas)
                .WithOne(p => p.Red)
                .HasForeignKey(p => p.IdRed);

            modelBuilder.Entity<NivelLiderazgo>()
                .HasMany(n => n.Personas)
                .WithOne(p => p.NivelLiderazgo)
                .HasForeignKey(p => p.IdNivelLiderazgo);


            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { IdCategoria = 1, Nombre = "Premium"},
                new Categoria { IdCategoria = 2, Nombre = "VIP"},
                new Categoria { IdCategoria = 3, Nombre = "General"}
                );

            modelBuilder.Entity<Red>().HasData(
                new Red { IdRed = 1, Nombre = "Adonai" },
                new Red { IdRed = 2, Nombre = "El Elyon" },
                new Red { IdRed = 3, Nombre = "El Shaddai" },
                new Red { IdRed = 4, Nombre = "Emmanuel" },
                new Red { IdRed = 5, Nombre = "YAWHE" }
                );

            modelBuilder.Entity<NivelLiderazgo>().HasData(
                new NivelLiderazgo { IdNivelLiderazgo = 1, Nombre = "Miembro" },
                new NivelLiderazgo { IdNivelLiderazgo = 2, Nombre = "Discipulo" },
                new NivelLiderazgo { IdNivelLiderazgo = 3, Nombre = "Líder de Casa de Paz" },
                new NivelLiderazgo { IdNivelLiderazgo = 4, Nombre = "Líder de Discípulo" },
                new NivelLiderazgo { IdNivelLiderazgo = 5, Nombre = "Diácono" },
                new NivelLiderazgo { IdNivelLiderazgo = 6, Nombre = "Anciano" },
                new NivelLiderazgo { IdNivelLiderazgo = 7, Nombre = "Efesio" }
                );
        }
    }
}
