using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Inspinia_MVC5_SeedProject.Models;
using static Inspinia_MVC5_SeedProject.Notificaciones;

namespace Inspinia_MVC5_SeedProject
{
    public class Notifications:Hub
    {
        private readonly Notificaciones _not;

        public Notifications(Notificaciones not)
        {
            _not = not;
        }

        public Notifications() : this(Notificaciones.Instance) { }

        public async Task<IEnumerable<ReclamosView>> GetNotificaciones()
        {
            return await _not.GetNotificaciones();
        }
    }
}