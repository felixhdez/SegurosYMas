using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;

namespace Inspinia_MVC5_SeedProject.Models
{
    public partial class SeguroBD : DbContext
    {
        public SeguroBD()
          : base("name=SeguroBD")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SeguroBD, Migrations.Configuration>()); //configuracion para el inicializador de la base de datos
        }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ContactoIntermediario> ContactoIntermediarios { get; set; }
        public DbSet<Ramo> Ramos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Aseguradora> Aseguradoras { get; set; }
        public DbSet<Agente> Agentes { get; set; }
        public DbSet<Tramite> Tramites { get; set; }
        public DbSet<Poliza> Polizas { get; set; }
        public DbSet<Bitacora> Bitacoras { get; set; }
        public DbSet<Caracteristica> Caracteristicass { get; set; }
        public DbSet<BienAsegurado> BienesAsegurados { get; set; }
        public DbSet<DetalleCaracteristica> DetalleCaracteristicas { get; set; }
        public DbSet<DetalleBienAsegurado> DetalleBienesAsegurados { get; set; }
        public DbSet<Cobertura> Coberturas { get; set; }
        public DbSet<DetalleCobertura> DetalleCoberturas { get; set; }
        public DbSet<CoberturaReclamo> CoberturaReclamos { get; set; }
        public DbSet<Reclamo> Reclamos { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<DetalleDocumento> DetalleDocumentos { get; set; }
        public DbSet <TipoDePago> TipoDepagos { get; set; }
        public DbSet<DetallePago> DetallePagos { get; set; }
        public DbSet<Adenda> Adendas { get; set; }
        public DbSet<DetalleAdenda> DetalleAdendas { get; set; }
        public DbSet<Cuota> Cuotas { get; set; }
        public DbSet<DetalleCuotasAdenda> DetalleCuotasAdenda { get; set; }
        public DbSet<DetalleCuota> DetalleCuotas { get; set; }
        public DbSet<ReciboCuota> ReciboCuotas { get; set; }
        //public DbSet<ArchivosAdendas> ArchivosAdendas { get; set; }
        public DbSet<ArchivosPólizas> ArchivosPolizas { get; set; }
        public DbSet<ArchivosReclamos> ArchivosReclamos { get; set; }
        public DbSet<ArchivosTrámites> ArchivosTramites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }

        //public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.Usuarios> Usuarios { get; set; }

        //public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.Roles> Roles { get; set; }
    }
}