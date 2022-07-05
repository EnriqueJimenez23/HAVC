using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CapaDatos.Repositorio.EF6;

namespace CapaDominio.EntidadesNegocio
{
    public class CsWebContext : DataContext
    {
        public CsWebContext() : base("CsWebContext")
        {
        }

        public DbSet<VariableConfiguracion> VariablesConfiguraciones { get; set; }
        public DbSet<RegistroOperacion> RegistroOperaciones { get; set; }
        public DbSet<Catalogo> Catalogos { get; set; }
        public DbSet<CatalogoT> CatalogosT { get; set; }
        public DbSet<Municipios> Municipios { get; set; }
        public DbSet<Perfil> Perfiles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<ContenidoSitio> ContenidoSitio { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<ProyectoAvance> ProyectosAvance { get; set; }
        public DbSet<Veedor> Veedor { get; set; }
        public DbSet<Pasos> Pasos { get; set; }
        public DbSet<Pasos3> Pasos3 { get; set; }
        public DbSet<Pasos4> Pasos4 { get; set; }
        public DbSet<Pasos5> Pasos5 { get; set; }
        public DbSet<Pasos6> Pasos6 { get; set; }
        public DbSet<Pasos8> Pasos8 { get; set; }
        public DbSet<Pasos9> Pasos9 { get; set; }
        public DbSet<DatosGeo> DatosGeo { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
