namespace Inspinia_MVC5_SeedProject.Migrations.SeguridadUserBD
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using Inspinia_MVC5_SeedProject.Models;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Inspinia_MVC5_SeedProject.Models.SeguridadUserBD>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Inspinia_MVC5_SeedProject.Models.SeguridadUserBD";
        }

        protected override void Seed(Inspinia_MVC5_SeedProject.Models.SeguridadUserBD context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Roles.AddOrUpdate(
                r => r.Descripcion, new Roles { Descripcion = "Administración" },
                new Roles { Descripcion = "Vendedor" },
                new Roles { Descripcion = "Gerencia" },
                new Roles { Descripcion = "Cobranza" },
                new Roles { Descripcion = "Reclamos" },
                new Roles { Descripcion = "Digitador" },
                new Roles { Descripcion = "Recepción" },
                new Roles { Descripcion = "Cliente" }
                );
            context.SaveChanges();
        }
    }
}
