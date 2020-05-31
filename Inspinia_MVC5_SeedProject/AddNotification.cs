using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inspinia_MVC5_SeedProject.Models;
using Microsoft.AspNet.SignalR;

namespace Inspinia_MVC5_SeedProject
{
    public static class AddNotification
    {
        public static void AppendNotify(ReclamosTemp reclamo)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<Notifications>();
            if (hubContext != null)
            {
                hubContext.Clients.All.nuevaNotificacion(reclamo);
            }
        }
    }
}