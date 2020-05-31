using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inspinia_MVC5_SeedProject.Models;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    [Authorize]
    public class MetodosController : Controller
    {
        public class Grafico
        {
            public string Serie { get; set; }
            public double Valor { get; set; }
        }

        SeguroBD db = new SeguroBD();
        SeguridadUserBD dbs = new SeguridadUserBD();
        public ActionResult Valores()
        {
            var primasfaltantes = 0.00;
            var clientes = 0;
            var tramites = 0;
            var polizasactivas =0;
            var polizasvigentes = db.Polizas.Where(x => x.FechaDesde <= DateTime.Today && x.FechaHasta >= DateTime.Today).ToList();
            if (polizasvigentes != null)
            {
                var listaclientes = polizasvigentes.Select(y => y.ClienteId).ToList();
                clientes = db.Clientes.Where(x=> listaclientes.Contains(x.Id)).Count();
                polizasactivas = polizasvigentes.Count();
                tramites = db.Tramites.Where(s=> !s.Finalizacion).Count();
                var primas = db.DetalleCuotas.Where(y => y.Saldo != 0).ToList();
                if (primas != null)
                    primasfaltantes = primas.Sum(x => x.Saldo);
            }
            primasfaltantes = double.Parse(primasfaltantes.ToString());
            var concat = "" + primasfaltantes + "/" + clientes + "/" + tramites + "/" + polizasactivas;
            return Json(new { concat }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VentaAnual()
        {
            List<Grafico> ventas = new List<Grafico>();
            if(db.Polizas.Count() > 0)
            {
                var first = db.Polizas.Min(x => x.FechaEmision.Year);
                for (int i = first;i <= DateTime.Today.Year;i++)
                {
                    var v2 = 0.0;
                    var v = db.Polizas.Where(x => x.FechaEmision.Year == i).ToList();
                    if (v != null)
                        v2 = v.Sum(w => w.PrimaNeta);
                    Grafico g;
                    g = new Grafico();
                    g.Serie = i.ToString();
                    g.Valor = v2;
                    ventas.Add(g);
                }
            }

            return Json(new { ventas }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClientesAnual()
        {
            List<Grafico> clientes = new List<Grafico>();
            if(db.Polizas.Count() > 0)
            {
                var first = db.Polizas.Min(x => x.FechaEmision.Year);
                for (int i = first;i <= DateTime.Today.Year;i++)
                {
                    List<int> clie = new List<int>();
                    var v2 = 0.0;
                    var c = db.Polizas.Where(x => x.FechaEmision.Year == i).ToList();
                    if (c != null)
                        clie = c.Select(x => x.ClienteId).ToList();
                    var v = db.Clientes.Where(x => clie.Contains(x.Id)).ToList();
                    if (v != null)
                        v2 = v.Count();
                    Grafico g;
                    g = new Grafico();
                    g.Serie = i.ToString();
                    g.Valor = v2;
                    clientes.Add(g);
                }
            }

            return Json(new { clientes }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VentaMensual()
        {
            List<Grafico> ventas = new List<Grafico>();
            for (int i = 1;i <= DateTime.Today.Month;i++)
            {
                var v2 = 0.0;
                var v = db.Polizas.Where(x => x.FechaEmision.Year == DateTime.Today.Year && x.FechaEmision.Month==i).ToList();
                if (v != null)
                    v2 = v.Sum(w => w.PrimaNeta);
                Grafico g;
                g = new Grafico();
                DateTimeFormatInfo dtinfo = new CultureInfo("es-ES",false).DateTimeFormat;
                g.Serie = dtinfo.GetMonthName(i);
                g.Valor = v2;
                ventas.Add(g);
            }

            return Json(new { ventas },JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClientesMensual ()
        {
            List<Grafico> clientes = new List<Grafico>();
            for (int i = 1;i <= DateTime.Today.Month;i++)
            {
                List<int> clie = new List<int>();
                var v2 = 0.0;
                var c = db.Polizas.Where(x => x.FechaEmision.Year == DateTime.Today.Year && x.FechaEmision.Month == i).ToList();
                if (c != null)
                    clie = c.Select(x => x.ClienteId).ToList();
                var v = db.Clientes.Where(x => clie.Contains(x.Id)).ToList();
                if (v != null)
                    v2 = v.Count();
                Grafico g;
                g = new Grafico();
                DateTimeFormatInfo dtinfo = new CultureInfo("es-ES",false).DateTimeFormat;
                g.Serie = dtinfo.GetMonthName(i);
                g.Valor = v2;
                clientes.Add(g);
            }

            return Json(new { clientes },JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReclamos()
        {
            var x=  (from item in dbs.ReclamosTemps.ToList()
                    join item2 in db.Clientes.ToList() on item.UsuarioId equals item2.Id
                    orderby item.Fecha
                    select new
                    {
                        Cliente = item2.NombreCompleto,
                        Fecha = item.Fecha,
                        Lugar = item.Lugar,
                        Comentarios = item.Comentarios
                    }).ToList();
            return Json(new { list = x }, JsonRequestBehavior.AllowGet);
        }
    }
}