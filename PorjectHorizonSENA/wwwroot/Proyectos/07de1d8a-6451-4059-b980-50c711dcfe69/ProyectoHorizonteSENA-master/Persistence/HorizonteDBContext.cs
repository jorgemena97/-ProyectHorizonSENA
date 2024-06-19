using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence;

public class HorizonteDBContext : DbContext
{
    public HorizonteDBContext(DbContextOptions<HorizonteDBContext> options)
    : base(options) { }

    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Rol> Rol { get; set; }
    public DbSet<Proyectos> Proyectos { get; set; }
}
