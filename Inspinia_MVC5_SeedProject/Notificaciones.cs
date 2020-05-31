using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inspinia_MVC5_SeedProject.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspinia_MVC5_SeedProject
{
    public class Notificaciones
    {
        private readonly static Lazy<Notificaciones> obj = new Lazy<Notificaciones>(() => new Notificaciones
        (GlobalHost.ConnectionManager.GetHubContext<Notifications>().Clients));
        private Microsoft.AspNet.SignalR.Hubs.IHubConnectionContext<dynamic> clients;

        private Models.SeguridadUserBD db = new Models.SeguridadUserBD();
        private Models.SeguroBD db2 = new Models.SeguroBD();
        public Notificaciones(Microsoft.AspNet.SignalR.Hubs.IHubConnectionContext<dynamic> clients)
        {
            this.clients = clients;
        }

        public static Notificaciones Instance
        {
            get { return obj.Value; }
        }

        public async Task<IEnumerable<ReclamosView>> GetNotificaciones()
        {
            try
            {
                var x = await db.ReclamosTemps.ToListAsync();
                var y = await db2.Clientes.ToListAsync();
                string sql = @"select R.Id, U.Nombre + ' ' + u.Apellido as [NombreCliente], R.Fecha, R.Lugar, R.Comentarios, R.Tipo, R.Visto
                from mvc.ReclamosTemporales R inner join mvc.Usuarios U on R.UsuarioId = U.Id";

                var result = db.Database.SqlQuery<ReclamosView>(sql).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [NotMapped]
        public class ReclamosView : ReclamosTemp
        {
            public string NombreCliente { get; set; }
        }
        public void NuevaNotificacion(ReclamosView reclamos)
        {
            clients.All.nuevaNotificacion(reclamos);
        }
    }
}