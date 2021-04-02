using Microsoft.EntityFrameworkCore;
using Nutrimenez.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nutrimenez.Data
{
    public class NutrimenezContexto : DbContext
    {
        public NutrimenezContexto(DbContextOptions<NutrimenezContexto> options) : base(options)
        {
        }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoConsulta> TipoConsultas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Poner el nombre de las tablas en singular
            modelBuilder.Entity<Consulta>().ToTable("Consulta");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<TipoConsulta>().ToTable("TipoConsulta");

            // Deshabilitar la eliminación en cascada en todas las relaciones
            base.OnModelCreating(modelBuilder);
            foreach (var relationship in
            modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

    }
}
