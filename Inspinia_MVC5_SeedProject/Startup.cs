using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Inspinia_MVC5_SeedProject.Models;
using System;

[assembly: OwinStartupAttribute(typeof(Inspinia_MVC5_SeedProject.Startup))]
namespace Inspinia_MVC5_SeedProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CrearPermisos();
            app.MapSignalR();
        }

        private void CrearPermisos()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roles = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var usuarios = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            IdentityRole permiso = new IdentityRole();
            if(!roles.RoleExists("Administración"))
            {
                permiso.Name = "Administración";
                roles.Create(permiso);

                ApplicationUser user = new ApplicationUser();
                user.UserName = "Admon@gmail.com";
                user.Email = "ingefelix17@gmail.com";
                var r = usuarios.Create(user, "&depto_comp17");
                if (r.Succeeded)
                {
                    usuarios.AddToRole(user.Id, "Administración");
                }
            }

            if (!roles.RoleExists("Vendedor"))
            {
                permiso = new IdentityRole();
                permiso.Name = "Vendedor";
                roles.Create(permiso);
            }

            if (!roles.RoleExists("Gerencia"))
            {
                permiso = new IdentityRole();
                permiso.Name = "Gerencia";
                roles.Create(permiso);
            }

            if (!roles.RoleExists("Cobranza"))
            {
                permiso = new IdentityRole();
                permiso.Name = "Cobranza";
                roles.Create(permiso);
            }

            if (!roles.RoleExists("Reclamos"))
            {
                permiso = new IdentityRole();
                permiso.Name = "Reclamos";
                roles.Create(permiso);
            }

            if (!roles.RoleExists("Digitador"))
            {
                permiso = new IdentityRole();
                permiso.Name = "Digitador";
                roles.Create(permiso);
            }

            if (!roles.RoleExists("Recepción"))
            {
                permiso = new IdentityRole();
                permiso.Name = "Recepción";
                roles.Create(permiso);
            }

            if (!roles.RoleExists("Cliente"))
            {
                permiso = new IdentityRole();
                permiso.Name = "Cliente";
                roles.Create(permiso);
            }
        }
    }
}
