using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Entities.Interfaces;

namespace Persistence
{
    public class HorizonteDBContext : DbContext
    {
        public HorizonteDBContext(DbContextOptions<HorizonteDBContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Proyectos> Proyectos { get; set; }
        public DbSet<ProyectosUsuario> ProyectosUsuario { get; set; }
        public DbSet<Mensajes> Mensaje { get; set; }
        public DbSet<Comentarios> Comentarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de las relaciones entre las entidades utilizando Fluent API


            modelBuilder.Entity<ProyectosUsuario>()
                .HasOne(pu => pu.Proyecto)
                .WithMany(p => p.ProyectosUsuarios)
                .HasForeignKey(pu => pu.ProyectoId)
                .IsRequired(); // Indica que la relación es obligatoria


            modelBuilder.Entity<Comentarios>()
           .HasOne(c => c.Proyectos)
           .WithMany(p => p.Comentarios)
           .HasForeignKey(c => c.IdProyecto);


        }

    }
}
