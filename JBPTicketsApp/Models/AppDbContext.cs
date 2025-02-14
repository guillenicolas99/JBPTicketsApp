﻿using JBPTicketsApp.Models.Entities;
using JBPTicketsApp.Recursos;
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
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }


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

            modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Rol)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(u => u.IdRol);


            modelBuilder.Entity<Rol>().HasData(
                new Rol { IdRol = 1, NombreRol = "Admin"},
                new Rol { IdRol = 2, NombreRol = "User"},
                new Rol { IdRol = 3, NombreRol = "Manager"}
            );

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { IdUsuario = 1, NombreUsuario = "Admin", Correo = "admin@gmail.com" ,Clave = Utilidades.EncriptarClave("Admin123"), IdRol = 1},
                new Usuario { IdUsuario = 2, NombreUsuario = "User", Correo = "user@gmail.com", Clave = Utilidades.EncriptarClave("User123"), IdRol = 2},
                new Usuario { IdUsuario = 3, NombreUsuario = "Manager", Correo = "manager@gmail.com", Clave = Utilidades.EncriptarClave("Admin123"), IdRol = 3}
            );

            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { IdCategoria = 1, Nombre = "Premium"},
                new Categoria { IdCategoria = 2, Nombre = "VIP"},
                new Categoria { IdCategoria = 3, Nombre = "General"}
                );

            modelBuilder.Entity<Red>().HasData(
                new Red { IdRed = 1, Nombre = "Sin red" },
                new Red { IdRed = 2, Nombre = "Adonai" },
                new Red { IdRed = 3, Nombre = "El Elyon" },
                new Red { IdRed = 4, Nombre = "El Shaddai" },
                new Red { IdRed = 5, Nombre = "Emmanuel" },
                new Red { IdRed = 6, Nombre = "YAWHE" }
                );

            modelBuilder.Entity<Persona>().HasData(
                new Persona { IdPersona = 1, Nombre = "Sin asignar", IdRed = 1, IdNivelLiderazgo = 1 }
                );
            modelBuilder.Entity<NivelLiderazgo>().HasData(
                new NivelLiderazgo { IdNivelLiderazgo = 1, Nombre = "Sin Nivel" },
                new NivelLiderazgo { IdNivelLiderazgo = 2, Nombre = "Miembro" },
                new NivelLiderazgo { IdNivelLiderazgo = 3, Nombre = "Discipulo" },
                new NivelLiderazgo { IdNivelLiderazgo = 4, Nombre = "Líder de Casa de Paz" },
                new NivelLiderazgo { IdNivelLiderazgo = 5, Nombre = "Líder de Discípulo" },
                new NivelLiderazgo { IdNivelLiderazgo = 6, Nombre = "Diácono" },
                new NivelLiderazgo { IdNivelLiderazgo = 7, Nombre = "Anciano" },
                new NivelLiderazgo { IdNivelLiderazgo = 8, Nombre = "Efesio" }
                );
        }
    }
}
