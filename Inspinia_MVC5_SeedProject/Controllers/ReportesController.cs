using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inspinia_MVC5_SeedProject.Models.DataSets;
using Microsoft.Reporting.WebForms;
using Inspinia_MVC5_SeedProject.Models;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        private SeguroBD db = new SeguroBD();        
        private static int CuotaTemp = 0;

        public class ClientesR
        {
            public string Nombre { get; set; }
            public string Identificacion { get; set; }
            public string Tipo { get; set; }
            public int Cantidad { get; set; }
            public string Titulo { get; set; }
        }

        public class PolizasC
        {
            public string Nombre { get; set; }
            public string NumPoliza { get; set; }
            public string Producto { get; set; }
            public string Ramo { get; set; }
            public string Aseguradora { get; set; }
            public string Vigencia { get; set; }
            public double Monto { get; set; }
            public string Agente { get; set; }
            public string Titulo { get; set; }
            public string Info { get; set; }
        }

        public class PolizasInd : PolizasC
        {
            public string Apellidos { get; set; }
            public string Ident { get; set; }
            public string TipoC { get; set; }
            public string Titulo2 { get; set; }
        }

        public class ReclamosC
        {
            public string Nombre { get; set; }
            public string NumPoliza { get; set; }
            public string Producto { get; set; }
            public string Aseguradora { get; set; }
            public string NumReclamo { get; set; }
            public string FechaSin { get; set; }
            public string Lugar { get; set; }
            public double Monto { get; set; }
            public string Titulo { get; set; }
            public string Info { get; set; }
        }

        public class ReclamosInd : ReclamosC
        {
            public string Apellidos { get; set; }
            public string Ident { get; set; }
            public string TipoC { get; set; }
            public string Titulo2 { get; set; }
        }

        public class Padre
        {
            public string Nombres { get; set; }
            public string Apellidos { get; set; }
            public string Ident { get; set; }
            public string NumPoliza { get; set; }
            public string Vigencia { get; set; }
            public string Titulo2 { get; set; }
            public string Titulo { get; set; }
            public string Info { get; set; }

        }

        public class BitacoraR : Padre
        {
            public string Fecha { get; set; }
            public string Observacion { get; set; }
        }

        public class AdendasR : Padre
        {
            public string NumAdenda { get; set; }
            public string Tipo { get; set; }
            public string VigenciaAd { get; set; }
            public double Suma { get; set; }
            public double Prima { get; set; }
        }

        public class ReclamosR : Padre
        {
            public string NumReclamo { get; set; }
            public string Fecha { get; set; }
            public string Lugar { get; set; }
        }

        public class Cuotas : Padre
        {
            public string Cuota { get; set; }
            public string Vence { get; set; }
            public double Monto { get; set; }
            public double Saldo { get; set; }
            public string Estado { get; set; }
            public string Titulo3 { get; set; }
        }

        public class Estado
        {
            public string Fecha { get; set; }
            public string NumRecibo { get; set; }
            public double Prima { get; set; }
            public double Abono { get; set; }
            public double Saldo { get; set; }
        }

        public class TramitesR
        {
            public string Tipo { get; set; }
            public string Modalidad { get; set; }
            public string Enviado { get; set; }
            public string Recibido { get; set; }
            public string RecibidoPor { get; set; }
            public string Descripcion { get; set; }
            public string Titulo { get; set; }
            public string Info { get; set; }
        }

        public class BienAseg : Padre
        {
            public int Certificado { get; set; }
            public string Observacion { get; set; }
            public string Titulo3 { get; set; }
            public string Titulo4 { get; set; }
        }

        public class CaracteristicasR
        {
            public string Caracteristica { get; set; }
            public string Valor { get; set; }
            public int NumC { get; set; }
        }

        public class CoberturasR
        {
            public string Cobertura { get; set; }
            public double Suma { get; set; }
            public double Deducible { get; set; }
            public double Prima { get; set; }
            public int NumC { get; set; }
        }

        public class Comisiones
        {
            public string TItulo { get; set; }
            public string Info { get; set; }
            public string NumPoliza { get; set; }
            public string Nombre { get; set; }
            public double Pago { get; set; }
            public double Porcentaje { get; set; }
            public double Comision { get; set; }
        }

        public class SIBOIF
        {
            public string Titulo { get; set; }
            public string Info { get; set; }
            public string Num { get; set; }
            public string Ramo { get; set; }
            public double Assa { get; set; }
            public double America { get; set; }
            public double INISER { get; set; }
            public double MAPFRE { get; set; }
            public double Lafise { get; set; }
        }

        // GET: Reportes
        public ActionResult IndexClientes()
        {
            return View();
        }
        
        public ActionResult TodosClientes()
        {
            var data = GetClientes(0);
            DataSet.ClientesDataTable c_data = new DataSet.ClientesDataTable();
            ReportViewer rtp = new ReportViewer();
            rtp.ProcessingMode = ProcessingMode.Local;
            rtp.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes/Clientes.rdlc";
            rtp.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rtp.SizeToReportContent = true;
            rtp.ShowPrintButton = true;
            rtp.ShowZoomControl = true;
            ViewBag.rpt = rtp;
            ViewBag.datos = data.Count;
            return View();
        }

        public ActionResult ClientesActivos() {
            var data = GetClientes(1);
            DataSet.ClientesDataTable c_data = new DataSet.ClientesDataTable();
            ReportViewer rtp = new ReportViewer();
            rtp.ProcessingMode = ProcessingMode.Local;
            rtp.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes/Clientes.rdlc";
            rtp.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rtp.SizeToReportContent = true;
            rtp.ShowPrintButton = true;
            rtp.ShowZoomControl = true;
            ViewBag.rpt = rtp;
            ViewBag.datos = data.Count;
            return View();
        }

        public ActionResult ClientesInactivos() {
            var data = GetClientes(2);
            DataSet.ClientesDataTable c_data = new DataSet.ClientesDataTable();
            ReportViewer rtp = new ReportViewer();
            rtp.ProcessingMode = ProcessingMode.Local;
            rtp.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes/Clientes.rdlc";
            rtp.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rtp.SizeToReportContent = true;
            rtp.ShowPrintButton = true;
            rtp.ShowZoomControl = true;
            ViewBag.rpt = rtp;
            ViewBag.datos = data.Count;
            return View();
        }

        public List<ClientesR> GetClientes(int id)
        {
            List<ClientesR> lista = new List<ClientesR>();
            List<int> lista_p = db.Polizas.Where(x => x.FechaDesde <= DateTime.Today && x.FechaHasta >= DateTime.Today).Select(y => y.ClienteId).ToList();
            List<int> lista_p2 = db.Polizas.Where(x => x.FechaHasta < DateTime.Today).Select(y => y.ClienteId).ToList();
            foreach (var item in db.Clientes.ToList())
            {
                ClientesR c = new ClientesR();
                c.Nombre = item.NombreCompleto;
                c.Identificacion = item.Identificacion;
                c.Tipo = item.TipoCliente;
                try
                {
                    c.Cantidad = db.Polizas.Where(x => x.ClienteId == item.Id).Count();
                }
                catch (Exception)
                {
                    c.Cantidad = 0;
                }

                switch (id)
                {
                    case 0:
                        {
                            c.Titulo = "LISTADO DE CLIENTES";
                            lista.Add(c);
                        }; break;
                    case 1:
                        {
                            c.Titulo = "CLIENTES ACTIVOS";
                            if (lista_p.Contains(item.Id))
                                lista.Add(c);
                        }; break;
                    case 2:
                        {
                            c.Titulo = "CLIENTES INACTIVOS";
                            if (lista_p2.Contains(item.Id))
                                lista.Add(c);
                        }; break;
                }
            }

            return lista;
        }

        //===========================================================================================
        //==========================REPORTES PÓLIZAS - CLIENTES =====================================
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public ActionResult TodasPolizas(DateTime desde, DateTime hasta)
        {
            var data = GetTodasPolizas(desde, hasta);
            DataSet.PolizasClientesDataTable p = new DataSet.PolizasClientesDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/PolizasClientes.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public ActionResult PolizasVigentes() {
            var data = GetPolizasVigentes();
            DataSet.PolizasClientesDataTable c_data = new DataSet.PolizasClientesDataTable();
            ReportViewer rtp = new ReportViewer();
            rtp.ProcessingMode = ProcessingMode.Local;
            rtp.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes/PolizasClientes.rdlc";
            rtp.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rtp.SizeToReportContent = true;
            rtp.ShowPrintButton = true;
            rtp.ShowZoomControl = true;
            ViewBag.rpt = rtp;
            ViewBag.datos = data.Count;
            return View();
        }

        public ActionResult PolizasPorCompanias(DateTime desde, DateTime hasta, int id)
        {
            var data = GetPolizasCompania(desde, hasta, id);
            DataSet.PolizasClientesDataTable p = new DataSet.PolizasClientesDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/PolizasClientes.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public ActionResult PolizasPorAgentes(DateTime desde, DateTime hasta, int id)
        {
            var data = GetPolizasAgentes(desde, hasta, id);
            DataSet.PolizasClientesDataTable p = new DataSet.PolizasClientesDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/PolizasClientes.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public ActionResult PolizasPorClientes(DateTime desde, DateTime hasta, int id)
        {
            var data = GetPolizasClientes(desde, hasta, id);
            DataSet.PolizasClientesDataTable p = new DataSet.PolizasClientesDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/Polizas.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public ActionResult PolizasV(DateTime desde, DateTime hasta)
        {
            var data = GetPolizasV(desde, hasta);
            DataSet.PolizasClientesDataTable p = new DataSet.PolizasClientesDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/PolizasClientes.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public List<PolizasC> GetPolizasV(DateTime desde, DateTime hasta)
        {
            List<Poliza> lista = lista = db.Polizas.Where(y => y.FechaHasta >= desde && y.FechaHasta <= hasta).ToList();
            var data = (from item in lista
                        select new PolizasC
                        {
                            Nombre = item.Cliente.NombreCompleto,
                            NumPoliza = item.NumPoliza,
                            Producto = item.Producto.Descripcion,
                            Ramo = item.Producto.Ramo.Descripcion,
                            Aseguradora = item.Producto.Aseguradora.Descripcion,
                            Vigencia = item.FechaDesde.ToString("dd/MM/yyyy") + " - " + item.FechaHasta.ToString("dd/MM/yyyy"),
                            Monto = item.PrimaNeta,
                            Agente = item.Agente.NombreCompleto,
                            Titulo = "PÓLIZAS PRONTAS A VENCERSE",
                            Info = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy")
                        }).ToList();
            return data;
        }

        public List<PolizasInd> GetPolizasClientes(DateTime desde, DateTime hasta, int id)
        {
            var obj = db.Clientes.Find(id);
            List<Poliza> lista = lista = db.Polizas.Where(y => y.FechaDesde >= desde && y.FechaHasta <= hasta && y.ClienteId == id).ToList();
            return (from item in lista
                    select new PolizasInd
                    {
                        Nombre = item.Cliente.Nombres,
                        Apellidos = item.Cliente.Apellidos,
                        Ident = item.Cliente.Identificacion,
                        TipoC = item.Cliente.TipoCliente,
                        NumPoliza = item.NumPoliza,
                        Producto = item.Producto.Descripcion,
                        Ramo = item.Producto.Ramo.Descripcion,
                        Aseguradora = item.Producto.Aseguradora.Descripcion,
                        Vigencia = item.FechaDesde.ToString("dd/MM/yyyy") + " - " + item.FechaHasta.ToString("dd/MM/yyyy"),
                        Monto = item.PrimaNeta,
                        Agente = item.Agente.NombreCompleto,
                        Titulo = "DATOS DEL CLIENTE",
                        Info = "",
                        Titulo2 = "DETALLE DE PÓLIZAS"
                    }).ToList();
        }

        public List<PolizasC> GetPolizasAgentes(DateTime desde, DateTime hasta, int id)
        {
            var obj = db.Agentes.Find(id);
            List<Poliza> lista = lista = db.Polizas.Where(y => y.FechaDesde >= desde && y.FechaHasta <= hasta && y.AgenteId == id).ToList();
            return (from item in lista
                    select new PolizasC
                    {
                        Nombre = item.Cliente.NombreCompleto,
                        NumPoliza = item.NumPoliza,
                        Producto = item.Producto.Descripcion,
                        Ramo = item.Producto.Ramo.Descripcion,
                        Aseguradora = item.Producto.Aseguradora.Descripcion,
                        Vigencia = item.FechaDesde.ToString("dd/MM/yyyy") + " - " + item.FechaHasta.ToString("dd/MM/yyyy"),
                        Monto = item.PrimaNeta,
                        Agente = item.Agente.NombreCompleto,
                        Titulo = "PÓLIZAS POR AGENTE",
                        Info = obj.NombreCompleto + "\n" + desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy")
                    }).ToList();
        }

        public List<PolizasC> GetPolizasCompania(DateTime desde, DateTime hasta, int id)
        {
            var obj = db.Aseguradoras.Find(id);
            var lista = db.Polizas.Where(y => y.FechaDesde >= desde && y.FechaHasta <= hasta && y.Producto.AseguradoraId == id).ToList();
            return (from item in lista
                    select new PolizasC
                    {
                        Nombre = item.Cliente.NombreCompleto,
                        NumPoliza = item.NumPoliza,
                        Producto = item.Producto.Descripcion,
                        Ramo = item.Producto.Ramo.Descripcion,
                        Aseguradora = item.Producto.Aseguradora.Descripcion,
                        Vigencia = item.FechaDesde.ToString("dd/MM/yyyy") + " - " + item.FechaHasta.ToString("dd/MM/yyyy"),
                        Monto = item.PrimaNeta,
                        Agente = item.Agente.NombreCompleto,
                        Titulo = "PÓLIZAS POR COMPAÑÍA",
                        Info = obj.Descripcion + "\n" + desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy")
                    }).ToList();
        }

        public List<PolizasC> GetTodasPolizas(DateTime desde, DateTime hasta)
        {
            return (from item in db.Polizas.Where(y => y.FechaDesde >= desde && y.FechaHasta <= hasta).ToList()
                    select new PolizasC
                    {
                        Nombre = item.Cliente.NombreCompleto,
                        NumPoliza = item.NumPoliza,
                        Producto = item.Producto.Descripcion,
                        Ramo = item.Producto.Ramo.Descripcion,
                        Aseguradora = item.Producto.Aseguradora.Descripcion,
                        Vigencia = item.FechaDesde.ToString("dd/MM/yyyy") + " - " + item.FechaHasta.ToString("dd/MM/yyyy"),
                        Monto = item.PrimaNeta,
                        Agente = item.Agente.NombreCompleto,
                        Titulo = "LISTADO DE PÓLIZAS",
                        Info = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy")
                    }).ToList();
        }

        public List<PolizasC> GetPolizasVigentes() {
            List<PolizasC> lista = new List<PolizasC>();
            List<int> lista_p = db.Polizas.Where(x => x.FechaDesde <= DateTime.Today && x.FechaHasta >= DateTime.Today).Select(y => y.ClienteId).ToList();
            foreach (var x in db.Polizas.ToList()) {
                if (x.FechaDesde <= DateTime.Today && x.FechaHasta >= DateTime.Today) {
                    PolizasC p = new PolizasC();
                    p.Nombre = x.Cliente.NombreCompleto;
                    p.NumPoliza = x.NumPoliza;
                    p.Producto = x.Producto.Descripcion;
                    p.Ramo = x.Producto.Ramo.Descripcion;
                    p.Aseguradora = x.Producto.Aseguradora.Descripcion;
                    p.Vigencia = x.FechaDesde.ToString("dd/MM/yyyy") + " - " + x.FechaHasta.ToString("dd/MM/yyyy");
                    p.Monto = x.PrimaNeta;
                    p.Agente = x.Agente.NombreCompleto;
                    p.Titulo = "PÓLIZAS VIGENTES";
                    p.Info = "";
                    lista.Add(p);
                }
            }
            return lista;
        }

        //===========================================================================================
        //==========================REPORTES RECLAMOS - CLIENTES ====================================
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public ActionResult ReclamosClientes(DateTime desde, DateTime hasta)
        {
            var data = GetReclamosTodos(desde, hasta);
            DataSet.ReclamosClientesDataTable p = new DataSet.ReclamosClientesDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/ReclamosClientes.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public ActionResult ReclamosPorCliente(DateTime desde, DateTime hasta, int id)
        {
            var data = GetReclamosPorCliente(desde, hasta, id);
            DataSet.ReclamosClientesDataTable p = new DataSet.ReclamosClientesDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/ReclamosIndiv.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public List<ReclamosInd> GetReclamosPorCliente(DateTime desde, DateTime hasta, int id)
        {
            var lista = db.Reclamos.Where(x => x.FechaSiniestro >= desde && x.FechaSiniestro <= hasta && x.Poliza.ClienteId == id).ToList();
            return (from item in lista
                    select new ReclamosInd
                    {
                        Nombre = item.Poliza.Cliente.Nombres,
                        Apellidos = item.Poliza.Cliente.Apellidos,
                        Ident = item.Poliza.Cliente.Identificacion,
                        TipoC = item.Poliza.Cliente.TipoCliente,
                        NumPoliza = item.Poliza.NumPoliza,
                        Producto = item.Poliza.Producto.Descripcion,
                        Aseguradora = item.Poliza.Producto.Aseguradora.Descripcion,
                        NumReclamo = item.NumReclamo,
                        FechaSin = item.FechaSiniestro.ToString("dd/MM/yyyy"),
                        Lugar = item.LugarOcurrencia,
                        Monto = item.MontoReclamado,
                        Titulo = "DATOS DEL CLIENTE",
                        Titulo2 = "DETALLE DE RECLAMOS",
                        Info = ""
                    }).ToList();
        }

        public List<ReclamosC> GetReclamosTodos(DateTime desde, DateTime hasta)
        {
            var lista = db.Reclamos.Where(x => x.FechaSiniestro >= desde && x.FechaSiniestro <= hasta).ToList();
            return (from item in lista
                    select new ReclamosC
                    {
                        Nombre = item.Poliza.Cliente.NombreCompleto,
                        NumPoliza = item.Poliza.NumPoliza,
                        Producto = item.Poliza.Producto.Descripcion,
                        Aseguradora = item.Poliza.Producto.Aseguradora.Descripcion,
                        NumReclamo = item.NumReclamo,
                        FechaSin = item.FechaSiniestro.ToString("dd/MM/yyyy"),
                        Lugar = item.LugarOcurrencia,
                        Monto = item.MontoReclamado,
                        Titulo = "LISTA DE RECLAMOS",
                        Info = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy")
                    }).ToList();
        }

        //===========================================================================================
        //======================================REPORTES PÓLIZAS ====================================
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public ActionResult IndexPolizas()
        {
            return View();
        }

        public ActionResult Bitacora(DateTime desde, DateTime hasta, int id)
        {
            var data = GetBitacora(desde, hasta, id);
            DataSet.BitacoraDataTable p = new DataSet.BitacoraDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/Bitacora.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public ActionResult Adendas(DateTime desde, DateTime hasta, int id)
        {
            var data = GetAdenda(desde, hasta, id);
            DataSet.BitacoraDataTable p = new DataSet.BitacoraDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/Adendas.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public ActionResult Reclamos(DateTime desde, DateTime hasta, int id)
        {
            var data = GetReclamo(desde, hasta, id);
            DataSet.BitacoraDataTable p = new DataSet.BitacoraDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/Reclamos.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public ActionResult EstadodeCuenta(int id)
        {
            var data = GetCuotas(id);
            var data2 = GetCuenta(id);
            DataSet.BitacoraDataTable p = new DataSet.BitacoraDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/EstadodeCuenta.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", data2));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public ActionResult BienesAsegurados(int id)
        {
            var data1 = GetBienes(id);
            var data2 = GetCaract(id);
            var data3 = GetCobert(id);
            DataSet.BitacoraDataTable p = new DataSet.BitacoraDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/BienesAsegurados.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data1));
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", data2));
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", data3));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data1.Count;
            return View();
        }

        public List<BienAseg> GetBienes(int id)
        {
            return (from item in db.BienesAsegurados.Where(x => !x.Estado).ToList()
                    join item2 in db.DetalleBienesAsegurados.Where(x => x.PolizaId == id).ToList()
                    on item.Id equals item2.BienAseguradoId
                    select new BienAseg
                    {
                        NumPoliza = item2.Poliza.NumPoliza,
                        Vigencia = item2.Poliza.FechaDesde.ToString("dd/MM/yyyy") + " - " + item2.Poliza.FechaHasta.ToString("dd/MM/yyyy"),
                        Apellidos = item2.Poliza.Cliente.Apellidos,
                        Nombres = item2.Poliza.Cliente.Nombres,
                        Ident = item2.Poliza.Cliente.Identificacion,
                        Certificado = item.NumCertificado,
                        Observacion = item.Observacion,
                        Info ="",
                        Titulo = "DATOS GENERALES",
                        Titulo2 = "BIENES ASEGURADOS",
                        Titulo3 = "DETALLE DE CARACTERISTICAS",
                        Titulo4 = "DETALLE DE COBERTURAS"
                    }).ToList();
        }

        public List<CaracteristicasR> GetCaract(int id)
        {
            return (from item in db.DetalleCaracteristicas.ToList()
                    join item2 in db.BienesAsegurados.ToList() on item.BienAseguradoId equals item2.Id
                    join item3 in db.DetalleBienesAsegurados.Where(x => x.PolizaId == id).ToList() on item2.Id equals item3.BienAseguradoId
                    select new CaracteristicasR
                    {
                        Caracteristica = item.Caracteristica.Descripcion,
                        Valor = item.Valor,
                        NumC = item2.NumCertificado
                    }).ToList();
        }

        public List<CoberturasR> GetCobert(int id)
        {
            return (from item in db.DetalleCoberturas.ToList()
                    join item2 in db.BienesAsegurados.ToList() on item.BienAseguradoId equals item2.Id
                    join item3 in db.DetalleBienesAsegurados.Where(x => x.PolizaId == id).ToList() on item2.Id equals item3.BienAseguradoId
                    select new CoberturasR
                    {
                        Cobertura = item.Coberturas.Descripcion,
                        Suma = item.SumaAsegurada,
                        Deducible = item.Deducible,
                        Prima = item.Prima,
                        NumC = item2.NumCertificado
                    }).ToList();
        }

        public List<Estado> GetCuenta(int id)
        {
            var obj = db.Polizas.Find(id);
            double prima = obj.PrimaNeta + obj.Iva + obj.Otros + obj.Emision;
            var lista = db.ReciboCuotas.Where(x => x.CuotaId == CuotaTemp).ToList();
            List<Estado> nueva = new List<Estado>();
            double suma = 0;
            foreach (var item in lista)
            {
                Estado e = new Estado();
                e.NumRecibo = item.NumRecibo;
                e.Abono = item.PagoNeto;
                suma += e.Abono;
                e.Fecha = item.Fecha.ToString("dd/MM/yyyyy");
                e.Prima = prima;
                e.Saldo = e.Prima - suma;
                nueva.Add(e);
            }
            return nueva;
        }
        
        public List<Cuotas> GetCuotas(int id)
        {
            var cuotas = db.DetalleCuotas.Where(x => x.PolizaId == id && x.Deshabilitar).ToList();
            CuotaTemp = cuotas[0].CuotaId;
            return (from item in cuotas
                    select new Cuotas
                    {
                        Apellidos = item.Poliza.Cliente.Apellidos,
                        Nombres = item.Poliza.Cliente.Nombres,
                        Ident = item.Poliza.Cliente.Identificacion,
                        NumPoliza = item.Poliza.NumPoliza,
                        Vigencia = item.Poliza.FechaDesde.ToString("dd/MM/yyyy") + " - " + item.Poliza.FechaHasta.ToString("dd/MM/yyyy"),
                        Cuota = item.Cuotas,
                        Vence = item.Vence.ToString("dd/MM/yyyy"),
                        Monto = item.Monto,
                        Saldo = item.Saldo,
                        Estado = item.Estado,
                        Info = "",
                        Titulo = "DATOS GENERALES",
                        Titulo2 = "DETALLE DE CUOTAS",
                        Titulo3 = "DETALLE DE ABONOS"
        }).ToList();
        }

        public List<ReclamosR> GetReclamo(DateTime desde, DateTime hasta, int id)
        {
            var lista = db.Reclamos.Where(x => x.FechaSiniestro>= desde && x.FechaSiniestro <= hasta && x.PolizaId == id).ToList();
            return (from item in lista
                    select new ReclamosR
                    {
                        Nombres = item.Poliza.Cliente.Nombres,
                        Apellidos = item.Poliza.Cliente.Apellidos,
                        Ident = item.Poliza.Cliente.Identificacion,
                        NumPoliza = item.Poliza.NumPoliza,
                        Vigencia = item.Poliza.FechaDesde.ToString("dd/MM/yyyy") + " - " + item.Poliza.FechaHasta.ToString("dd/MM/yyyy"),
                        NumReclamo = item.NumReclamo,
                        Fecha = item.FechaSiniestro.ToString("dd/MM/yyyy"),
                        Lugar = item.LugarOcurrencia,
                        Titulo = "DATOS GENERALES",
                        Info = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy"),
                        Titulo2 = "DETALLE DE RECLAMOS"
                    }).ToList();
        }

        public List<AdendasR> GetAdenda(DateTime desde, DateTime hasta, int id)
        {
            var lista = db.Adendas.Where(x => x.FechaEmision >= desde && x.FechaEmision <= hasta && x.PolizaId == id).ToList();
            return (from item in lista
                    select new AdendasR
                    {
                        Nombres = item.Poliza.Cliente.Nombres,
                        Apellidos = item.Poliza.Cliente.Apellidos,
                        Ident = item.Poliza.Cliente.Identificacion,
                        NumPoliza = item.Poliza.NumPoliza,
                        Vigencia = item.Poliza.FechaDesde.ToString("dd/MM/yyyy") + " - " + item.Poliza.FechaHasta.ToString("dd/MM/yyyy"),
                        VigenciaAd = item.FechaDesde.ToString("dd/MM/yyyy") + " - " + item.FechaHasta.ToString("dd/MM/yyyy"),
                        NumAdenda = item.NumAdenda,
                        Prima = item.PrimaNeta,
                        Suma = item.SumaAsegurada,
                        Tipo = item.TipoAdenda,
                        Titulo = "DATOS GENERALES",
                        Info = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy"),
                        Titulo2 = "DETALLE DE ADENDAS"
                    }).ToList();
        }

        public List<BitacoraR> GetBitacora(DateTime desde, DateTime hasta, int id)
        {
            var lista = db.Bitacoras.Where(x => x.Fecha >= desde && x.Fecha <= hasta && x.PolizaId == id).ToList();
            return (from item in lista
                    select new BitacoraR
                    {
                        Nombres = item.Poliza.Cliente.Nombres,
                        Apellidos = item.Poliza.Cliente.Apellidos,
                        Ident = item.Poliza.Cliente.Identificacion,
                        NumPoliza = item.Poliza.NumPoliza,
                        Vigencia = item.Poliza.FechaDesde.ToString("dd/MM/yyyy") + " - " + item.Poliza.FechaHasta.ToString("dd/MM/yyyy"),
                        Fecha = item.Fecha.ToString("dd/MM/yyyy"),
                        Observacion = item.Observacion,
                        Titulo = "DATOS GENERALES",
                        Info = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy"),
                        Titulo2 = "DETALLE DE BITÁCORA"
                    }).ToList();
        }

        public ActionResult IndexReclamos()
        {
            return View();
        }

        //===========================================================================================
        //======================================REPORTES TRÁMITES====================================
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public ActionResult IndexTramites()
        {
            return View();
        }

        public ActionResult Tramites(DateTime desde, DateTime hasta)
        {
            var data = GetTramites(desde, hasta);
            DataSet.BitacoraDataTable p = new DataSet.BitacoraDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/Tramites.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public ActionResult TramitesPendientes()
        {
            var data = GetTramitesPendientes();
            DataSet.BitacoraDataTable p = new DataSet.BitacoraDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/Tramites.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public List<TramitesR> GetTramites(DateTime desde, DateTime hasta)
        {
            var lista = db.Tramites.Where(x => x.FechaRecepcion >= desde && x.FechaRecepcion <= hasta).ToList();
            return (from item in lista
                    select new TramitesR
                    {
                        Tipo = item.Tipo,
                        Modalidad = item.Modalidad,
                        Enviado = item.FechaEnvio.ToString("dd/MM/yyyy"),
                        Recibido = item.FechaRecibido.ToString("dd/MM/yyyy"),
                        RecibidoPor = item.RecibidoPor,
                        Descripcion = item.Descripcion,
                        Titulo = "LISTADO DE TRÁMITES",
                        Info = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy")
                    }).ToList();
        }

        public List<TramitesR> GetTramitesPendientes()
        {
            var lista = db.Tramites.Where(x => !x.Finalizacion).ToList();
            return (from item in lista
                    select new TramitesR
                    {
                        Tipo = item.Tipo,
                        Modalidad = item.Modalidad,
                        Enviado = item.FechaEnvio.ToString("dd/MM/yyyy"),
                        Recibido = item.FechaRecibido.ToString("dd/MM/yyyy"),
                        RecibidoPor = item.RecibidoPor,
                        Descripcion = item.Descripcion,
                        Titulo = "LISTADO DE TRÁMITES PENDIENTES",
                        Info = ""
                    }).ToList();
        }

        //======================================================================
        //===========================OTROS REPORTES=============================
        public ActionResult IndexOtros()
        {
            return View();
        }

        public ActionResult ComisionesD(DateTime desde, DateTime hasta)
        {
            var data = GetComisionesD(desde,hasta);
            DataSet.ComisionesDataTable p = new DataSet.ComisionesDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/Comisiones.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public List<Comisiones> GetComisionesD(DateTime desde,DateTime hasta)
        {
            List<Comisiones> l = new List<Comisiones>();
            var pagos = db.ReciboCuotas.Where(x => x.Fecha >= desde && x.Fecha <= hasta).ToList();

            foreach (var item in pagos)
            {
                Comisiones d = new Comisiones();
                d.TItulo = "Comisiones en Dólares";
                d.Info = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy");
                var o = Busqueda(item.CuotaId);
                if (o.TipoMoneda == "Dólares")
                {
                    d.NumPoliza = o.NumPoliza;
                    d.Nombre = o.Cliente.NombreCompleto;
                    d.Pago = item.Pago;
                    d.Porcentaje = o.ComisionPorcentaje;
                    d.Comision = d.Pago;
                    if (o.Emision != 0)
                        d.Comision -= (d.Comision * 0.02);
                    if (o.Iva != 0)
                        d.Comision -= (d.Comision * 0.15);
                    d.Comision *= (d.Porcentaje / 100);
                    l.Add(d);
                }
            }

            return l;
        }

        public Poliza Busqueda(int id)
        {
            var s = db.DetalleCuotas.Where(d => d.CuotaId == id).FirstOrDefault().Poliza;
            return s;
        }

        public ActionResult ComisionesC(DateTime desde, DateTime hasta)
        {
            var data = GetComisionesC(desde, hasta);
            DataSet.ComisionesDataTable p = new DataSet.ComisionesDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/Comisiones.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public List<Comisiones> GetComisionesC(DateTime desde, DateTime hasta)
        {
            List<Comisiones> l = new List<Comisiones>();
            var pagos = db.ReciboCuotas.Where(x => x.Fecha >= desde && x.Fecha <= hasta).ToList();

            foreach (var item in pagos)
            {
                Comisiones d = new Comisiones();
                d.TItulo = "Comisiones en Dólares";
                d.Info = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy");
                var o = Busqueda(item.CuotaId);
                if (o.TipoMoneda == "Córdobas")
                {
                    d.NumPoliza = o.NumPoliza;
                    d.Nombre = o.Cliente.NombreCompleto;
                    d.Pago = item.Pago;
                    d.Porcentaje = o.ComisionPorcentaje;
                    d.Comision = d.Pago;
                    if (o.Emision != 0)
                        d.Comision -= (d.Comision * 0.02);
                    if (o.Iva != 0)
                        d.Comision -= (d.Comision * 0.15);
                    d.Comision *= (d.Porcentaje / 100);
                    l.Add(d);
                }
            }

            return l;
        }

        public ActionResult PrimasAcumuladas(DateTime desde, DateTime hasta)
        {
            var data = GetPrimasAcum(desde, hasta);
            DataSet.ComisionesDataTable p = new DataSet.ComisionesDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/SIBOIF.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public List<SIBOIF> GetPrimasAcum(DateTime desde, DateTime hasta)
        {
            List<SIBOIF> lista = new List<SIBOIF>();
            lista.Add(CrearM("I", "VIDA","Vida Individual", "Saldo Deudores", desde, hasta));
            lista.Add(Crear("1.1", "Vida Individual", desde, hasta));
            lista.Add(Crear("1.2", "Vida colectiva", desde, hasta));
            lista.Add(Crear("1.3", "Saldo Deudores", desde, hasta));
            lista.Add(CrearM("II", "ACCIDENTES PERSONALES", "Individuales", "Colectivo para viajeros", desde, hasta));
            lista.Add(Crear("2.1", "Individuales", desde, hasta));
            lista.Add(Crear("2.2", "Familiares", desde, hasta));
            lista.Add(Crear("2.3", "Colectivo", desde, hasta));
            lista.Add(Crear("2.4", "Escolares", desde, hasta));
            lista.Add(Crear("2.5", "Transporte Privado", desde, hasta));
            lista.Add(Crear("2.6", "Transporte Público", desde, hasta));
            lista.Add(Crear("2.7", "Para viajero", desde, hasta));
            lista.Add(Crear("2.8", "Colectivo para viajeros", desde, hasta));
            lista.Add(CrearM("III", "SALUD", "Gastos médicos individuales", "Gastos médicos colectivos", desde, hasta));
            lista.Add(Crear("3.1", "Gastos médicos individuales", desde, hasta));
            lista.Add(Crear("3.2", "Gastos médicos grupo familiar", desde, hasta));
            lista.Add(Crear("3.3", "Gastos médicos colectivos", desde, hasta));
            lista.Add(CrearM("IV", "SEGUROS PREVISIONALES", "Accidentes laborales", "Accidentes laborales", desde, hasta));
            lista.Add(Crear("4.1", "Accidentes laborales", desde, hasta));
            lista.Add(CrearM("V", "RENTAS","Rentas programadas","Pensión", desde, hasta));
            lista.Add(Crear("5.1", "Rentas programadas", desde, hasta));
            lista.Add(Crear("5.2", "Rentas vitalicias", desde, hasta));
            lista.Add(Crear("5.3", "Pensión", desde, hasta));
            lista.Add(CrearM("VI", "PATRIMONIALES", "Incendio", "Responsabilidad civil de licencia", desde, hasta));
            lista.Add(Crear("6.1", "Incendio", desde, hasta));
            lista.Add(Crear("6.2", "Líneas aliadas", desde, hasta));
            lista.Add(Crear("6.3", "Automóviles", desde, hasta));
            lista.Add(Crear("6.4", "Transporte", desde, hasta));
            lista.Add(Crear("6.5", "Robo y hurto", desde, hasta));
            lista.Add(Crear("6.6", "Marítimo", desde, hasta));
            lista.Add(Crear("6.7", "Aviación", desde, hasta));
            lista.Add(Crear("6.8", "Rotura de cristales", desde, hasta));
            lista.Add(Crear("6.9", "Agropecuario", desde, hasta));
            lista.Add(Crear("6.10", "Dinero y valores", desde, hasta));
            lista.Add(Crear("6.11", "Todo riesgo de construcción", desde, hasta));
            lista.Add(Crear("6.12", "Equipo de contratista", desde, hasta));
            lista.Add(Crear("6.13", "Todo riesgo de montaje", desde, hasta));
            lista.Add(Crear("6.14", "Caldera y maquinaria", desde, hasta));
            lista.Add(Crear("6.15", "Rotura de maquinaria", desde, hasta));
            lista.Add(Crear("6.16", "Seguro bancario", desde, hasta));
            lista.Add(Crear("6.17", "Equipo Electrónico", desde, hasta));
            lista.Add(Crear("6.18", "Crédito", desde, hasta));
            lista.Add(Crear("6.19", "Póliza de Asistencia", desde, hasta));
            lista.Add(Crear("6.20", "Seguro título de propiedad", desde, hasta));
            lista.Add(Crear("6.21", "Caución", desde, hasta));
            lista.Add(Crear("6.22", "Desempleo", desde, hasta));
            lista.Add(Crear("6.23", "Responsabilidad civil", desde, hasta));
            lista.Add(Crear("6.24", "Fidelidad", desde, hasta));
            lista.Add(Crear("6.25", "Responsabilidad civil de licencia", desde, hasta));
            lista.Add(CrearM("VII", "OBLIGATORIOS", "Responsabilidad civil daños a terceros", "Responsabilidad civil de accidentes personales", desde, hasta));
            lista.Add(Crear("7.1", "Responsabilidad civil daños a terceros", desde, hasta));
            lista.Add(Crear("7.2", "Responsabilidad civil de accidentes personales", desde, hasta));
            lista.Add(Crear("7.3", "Rc-Acc Personal de Transporte", desde, hasta));
            lista.Add(Crear("7.4", "Rc Licencia Profesional", desde, hasta));
            lista.Add(CrearM("VIII", "FIANZAS", "Fianza de contratista y proveedor", "Otras fianzas", desde, hasta));
            lista.Add(Crear("8.1", "Fianza de contratista y proveedor", desde, hasta));
            lista.Add(Crear("8.2", "Fianzas fiscales", desde, hasta));
            lista.Add(Crear("8.3", "Fianzas profesionales", desde, hasta));
            lista.Add(Crear("8.4", "Fianzas judiciales", desde, hasta));
            lista.Add(Crear("8.5", "Otras fianzas", desde, hasta));
            lista.Add(Crear("IX", "SEGUROS ESPECIALES", desde, hasta));
            lista.Add(Crear("X", "MICROSEGUROS", desde, hasta));
            return lista;
        }

        public SIBOIF Crear(string num, string ramo, DateTime desde, DateTime hasta)
        {
            SIBOIF s;
            s = new SIBOIF();
            s.Titulo = "Primas Acumuladas";
            s.Info = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy");
            s.Num = num;
            s.Ramo = ramo;
            var x = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && b.Producto.Ramo.Descripcion == ramo && b.Producto.Aseguradora.Descripcion.Contains("assa")).ToList();
            if (x != null)
                s.Assa = x.Sum(b => b.PrimaNeta);
            else
                s.Assa = 0;
            var x2 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && b.Producto.Ramo.Descripcion == ramo && b.Producto.Aseguradora.Descripcion.Contains("américa")).ToList();
            if (x2 != null)                                                                                                         
                s.America = x2.Sum(b => b.PrimaNeta);                                                                               
            else                                                                                                                    
                s.America = 0;                                                                                                      
            var x3 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && b.Producto.Ramo.Descripcion == ramo && b.Producto.Aseguradora.Descripcion.Contains("iniser")).ToList();
            if (x3 != null)                                                                                                         
                s.INISER = x3.Sum(b => b.PrimaNeta);                                                                                
            else                                                                                                                    
                s.INISER = 0;                                                                                                       
            var x4 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && b.Producto.Ramo.Descripcion == ramo && b.Producto.Aseguradora.Descripcion.Contains("lafise")).ToList();
            if (x4 != null)                                                                                                         
                s.Lafise = x4.Sum(b => b.PrimaNeta);                                                                                
            else                                                                                                                    
                s.Lafise = 0;                                                                                                       
            var x5 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && b.Producto.Ramo.Descripcion == ramo && b.Producto.Aseguradora.Descripcion.Contains("mapfre")).ToList();
            if (x5 != null)
                s.MAPFRE = x5.Sum(b => b.PrimaNeta);
            else
                s.MAPFRE = 0;
            return s;
        }

        public SIBOIF CrearM(string num, string ramo, string p, string u, DateTime desde, DateTime hasta)
        {
            SIBOIF s;
            s = new SIBOIF();
            var _p = db.Ramos.FirstOrDefault(q => q.Descripcion == p);
            var _u = db.Ramos.FirstOrDefault(q => q.Descripcion == u);
            if (_p != null && _u != null)
            {
                s.Titulo = "Primas Acumuladas";
                s.Info = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy");
                s.Num = num;
                s.Ramo = ramo;
                var x = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && (b.Producto.RamoId >= _p.Id && b.Producto.RamoId <= _u.Id) && b.Producto.Aseguradora.Descripcion.Contains("assa")).ToList();
                if (x != null)
                    s.Assa = x.Sum(b => b.PrimaNeta);
                else
                    s.Assa = 0;
                var x2 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && (b.Producto.RamoId >= _p.Id && b.Producto.RamoId <= _u.Id) && b.Producto.Aseguradora.Descripcion.Contains("américa")).ToList();
                if (x2 != null)
                    s.America = x2.Sum(b => b.PrimaNeta);
                else
                    s.America = 0;
                var x3 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && (b.Producto.RamoId >= _p.Id && b.Producto.RamoId <= _u.Id) && b.Producto.Aseguradora.Descripcion.Contains("iniser")).ToList();
                if (x3 != null)
                    s.INISER = x3.Sum(b => b.PrimaNeta);
                else
                    s.INISER = 0;
                var x4 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && (b.Producto.RamoId >= _p.Id && b.Producto.RamoId <= _u.Id) && b.Producto.Aseguradora.Descripcion.Contains("lafise")).ToList();
                if (x4 != null)
                    s.Lafise = x4.Sum(b => b.PrimaNeta);
                else
                    s.Lafise = 0;
                var x5 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && (b.Producto.RamoId >= _p.Id && b.Producto.RamoId <= _u.Id) && b.Producto.Aseguradora.Descripcion.Contains("mapfre")).ToList();
                if (x5 != null)
                    s.MAPFRE = x5.Sum(b => b.PrimaNeta);
                else
                    s.MAPFRE = 0;
            }
            return s;
        }        

        public ActionResult SumasAcumuladas(DateTime desde, DateTime hasta)
        {
            var data = GetSumasAcum(desde, hasta);
            DataSet.ComisionesDataTable p = new DataSet.ComisionesDataTable();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath + @"Reportes/SIBOIF.rdlc");
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            ViewBag.datos = data.Count;
            return View();
        }

        public List<SIBOIF> GetSumasAcum(DateTime desde, DateTime hasta)
        {
            List<SIBOIF> lista = new List<SIBOIF>();
            lista.Add(CrearSM("I", "VIDA", "Vida Individual", "Saldo Deudores", desde, hasta));
            lista.Add(CrearS("1.1", "Vida Individual", desde, hasta));
            lista.Add(CrearS("1.2", "Vida colectiva", desde, hasta));
            lista.Add(CrearS("1.3", "Saldo Deudores", desde, hasta));
            lista.Add(CrearSM("II", "ACCIDENTES PERSONALES", "Individuales", "Colectivo para viajeros", desde, hasta));
            lista.Add(CrearS("2.1", "Individuales", desde, hasta));
            lista.Add(CrearS("2.2", "Familiares", desde, hasta));
            lista.Add(CrearS("2.3", "Colectivo", desde, hasta));
            lista.Add(CrearS("2.4", "Escolares", desde, hasta));
            lista.Add(CrearS("2.5", "Transporte Privado", desde, hasta));
            lista.Add(CrearS("2.6", "Transporte Público", desde, hasta));
            lista.Add(CrearS("2.7", "Para viajero", desde, hasta));
            lista.Add(CrearS("2.8", "Colectivo para viajeros", desde, hasta));
            lista.Add(CrearSM("III", "SALUD", "Gastos médicos individuales", "Gastos médicos colectivos", desde, hasta));
            lista.Add(CrearS("3.1", "Gastos médicos individuales", desde, hasta));
            lista.Add(CrearS("3.2", "Gastos médicos grupo familiar", desde, hasta));
            lista.Add(CrearS("3.3", "Gastos médicos colectivos", desde, hasta));
            lista.Add(CrearSM("IV", "SEGUROS PREVISIONALES", "Accidentes laborales", "Accidentes laborales", desde, hasta));
            lista.Add(CrearS("4.1", "Accidentes laborales", desde, hasta));
            lista.Add(CrearSM("V", "RENTAS", "Rentas programadas", "Pensión", desde, hasta));
            lista.Add(CrearS("5.1", "Rentas programadas", desde, hasta));
            lista.Add(CrearS("5.2", "Rentas vitalicias", desde, hasta));
            lista.Add(CrearS("5.3", "Pensión", desde, hasta));
            lista.Add(CrearSM("VI", "PATRIMONIALES", "Incendio", "Responsabilidad civil de licencia", desde, hasta));
            lista.Add(CrearS("6.1", "Incendio", desde, hasta));
            lista.Add(CrearS("6.2", "Líneas aliadas", desde, hasta));
            lista.Add(CrearS("6.3", "Automóviles", desde, hasta));
            lista.Add(CrearS("6.4", "Transporte", desde, hasta));
            lista.Add(CrearS("6.5", "Robo y hurto", desde, hasta));
            lista.Add(CrearS("6.6", "Marítimo", desde, hasta));
            lista.Add(CrearS("6.7", "Aviación", desde, hasta));
            lista.Add(CrearS("6.8", "Rotura de cristales", desde, hasta));
            lista.Add(CrearS("6.9", "Agropecuario", desde, hasta));
            lista.Add(CrearS("6.10", "Dinero y valores", desde, hasta));
            lista.Add(CrearS("6.11", "Todo riesgo de construcción", desde, hasta));
            lista.Add(CrearS("6.12", "Equipo de contratista", desde, hasta));
            lista.Add(CrearS("6.13", "Todo riesgo de montaje", desde, hasta));
            lista.Add(CrearS("6.14", "Caldera y maquinaria", desde, hasta));
            lista.Add(CrearS("6.15", "Rotura de maquinaria", desde, hasta));
            lista.Add(CrearS("6.16", "Seguro bancario", desde, hasta));
            lista.Add(CrearS("6.17", "Equipo Electrónico", desde, hasta));
            lista.Add(CrearS("6.18", "Crédito", desde, hasta));
            lista.Add(CrearS("6.19", "Póliza de Asistencia", desde, hasta));
            lista.Add(CrearS("6.20", "Seguro título de propiedad", desde, hasta));
            lista.Add(CrearS("6.21", "Caución", desde, hasta));
            lista.Add(CrearS("6.22", "Desempleo", desde, hasta));
            lista.Add(CrearS("6.23", "Responsabilidad civil", desde, hasta));
            lista.Add(CrearS("6.24", "Fidelidad", desde, hasta));
            lista.Add(CrearS("6.25", "Responsabilidad civil de licencia", desde, hasta));
            lista.Add(CrearSM("VII", "OBLIGATORIOS", "Responsabilidad civil daños a terceros", "Responsabilidad civil de accidentes personales", desde, hasta));
            lista.Add(CrearS("7.1", "Responsabilidad civil daños a terceros", desde, hasta));
            lista.Add(CrearS("7.2", "Responsabilidad civil de accidentes personales", desde, hasta));
            lista.Add(CrearS("7.3", "Rc-Acc Personal de Transporte", desde, hasta));
            lista.Add(CrearS("7.4", "Rc Licencia Profesional", desde, hasta));
            lista.Add(CrearSM("VIII", "FIANZAS", "Fianza de contratista y proveedor", "Otras fianzas", desde, hasta));
            lista.Add(CrearS("8.1", "Fianza de contratista y proveedor", desde, hasta));
            lista.Add(CrearS("8.2", "Fianzas fiscales", desde, hasta));
            lista.Add(CrearS("8.3", "Fianzas profesionales", desde, hasta));
            lista.Add(CrearS("8.4", "Fianzas judiciales", desde, hasta));
            lista.Add(CrearS("8.5", "Otras fianzas", desde, hasta));
            lista.Add(CrearS("IX", "SEGUROS ESPECIALES", desde, hasta));
            lista.Add(CrearS("X", "MICROSEGUROS", desde, hasta));
            return lista;
        }

        public SIBOIF CrearSM(string num, string ramo, string p, string u, DateTime desde, DateTime hasta)
        {
            double suma = 0;
            SIBOIF s;
            s = new SIBOIF();
            var _p = db.Ramos.FirstOrDefault(q => q.Descripcion == p);
            var _u = db.Ramos.FirstOrDefault(q => q.Descripcion == u);
            if(_p!=null && _u != null)
            {
                s.Titulo = "Sumas Aseguradas Acumuladas";
                s.Info = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy");
                s.Num = num;
                s.Ramo = ramo;
                var x = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && (b.Producto.RamoId >= _p.Id && b.Producto.RamoId <= _u.Id) && b.Producto.Aseguradora.Descripcion.Contains("assa")).ToList();
                if (x != null)
                {
                    var i = x.Select(y => y.Id);
                    var b = db.DetalleBienesAsegurados.Where(r => i.Contains(r.PolizaId)).Select(y => y.BienAsegurado);
                    if (b != null)
                    {
                        suma = 0;
                        foreach (var item in b)
                        {
                            suma += db.DetalleCoberturas.Where(t => t.BienAseguradoId == item.Id).Max(r => r.SumaAsegurada);
                        }
                        s.Assa = suma;
                    }
                    else
                        s.Assa = 0;
                }
                else
                    s.Assa = 0;

                var x2 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && (b.Producto.RamoId >= _p.Id && b.Producto.RamoId <= _u.Id) && b.Producto.Aseguradora.Descripcion.Contains("américa")).ToList();
                if (x2 != null)
                {
                    var i = x2.Select(y => y.Id);
                    var b = db.DetalleBienesAsegurados.Where(r => i.Contains(r.PolizaId)).Select(y => y.BienAsegurado);
                    if (b != null)
                    {
                        suma = 0;
                        foreach (var item in b)
                        {
                            suma += db.DetalleCoberturas.Where(t => t.BienAseguradoId == item.Id).Max(r => r.SumaAsegurada);
                        }
                        s.America = suma;
                    }
                    else
                        s.America = 0;
                }
                else
                    s.America = 0;

                var x3 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && (b.Producto.RamoId >= _p.Id && b.Producto.RamoId <= _u.Id) && b.Producto.Aseguradora.Descripcion.Contains("iniser")).ToList();
                if (x3 != null)
                {
                    var i = x3.Select(y => y.Id);
                    var b = db.DetalleBienesAsegurados.Where(r => i.Contains(r.PolizaId)).Select(y => y.BienAsegurado);
                    if (b != null)
                    {
                        suma = 0;
                        foreach (var item in b)
                        {
                            suma += db.DetalleCoberturas.Where(t => t.BienAseguradoId == item.Id).Max(r => r.SumaAsegurada);
                        }
                        s.INISER = suma;
                    }
                    else
                        s.INISER = 0;
                }
                else
                    s.INISER = 0;

                var x4 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && (b.Producto.RamoId >= _p.Id && b.Producto.RamoId <= _u.Id) && b.Producto.Aseguradora.Descripcion.Contains("lafise")).ToList();
                if (x4 != null)
                {
                    var i = x4.Select(y => y.Id);
                    var b = db.DetalleBienesAsegurados.Where(r => i.Contains(r.PolizaId)).Select(y => y.BienAsegurado);
                    if (b != null)
                    {
                        suma = 0;
                        foreach (var item in b)
                        {
                            suma += db.DetalleCoberturas.Where(t => t.BienAseguradoId == item.Id).Max(r => r.SumaAsegurada);
                        }
                        s.Lafise = suma;
                    }
                    else
                        s.Lafise = 0;
                }
                else
                    s.Lafise = 0;

                var x5 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && (b.Producto.RamoId >= _p.Id && b.Producto.RamoId <= _u.Id) && b.Producto.Aseguradora.Descripcion.Contains("mapfre")).ToList();
                if (x5 != null)
                {
                    var i = x5.Select(y => y.Id);
                    var b = db.DetalleBienesAsegurados.Where(r => i.Contains(r.PolizaId)).Select(y => y.BienAsegurado);
                    if (b != null)
                    {
                        suma = 0;
                        foreach (var item in b)
                        {
                            suma += db.DetalleCoberturas.Where(t => t.BienAseguradoId == item.Id).Max(r => r.SumaAsegurada);
                        }
                        s.MAPFRE = suma;
                    }
                    else
                        s.MAPFRE = 0;
                }
                else
                    s.MAPFRE = 0;
            }
            return s;
        }

        public SIBOIF CrearS(string num, string ramo, DateTime desde, DateTime hasta)
        {
            double suma = 0;
            SIBOIF s;
            s = new SIBOIF();
            s.Titulo = "Sumas Aseguradas Acumuladas";
            s.Info = desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy");
            s.Num = num;
            s.Ramo = ramo;
            var x = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && b.Producto.Ramo.Descripcion == ramo && b.Producto.Aseguradora.Descripcion.Contains("assa")).ToList();
            if (x != null)
            {
                var i = x.Select(y => y.Id);
                var b = db.DetalleBienesAsegurados.Where(r => i.Contains(r.PolizaId)).Select(y => y.BienAsegurado);
                if (b != null)
                {
                    suma = 0;
                    foreach (var item in b)
                    {
                        suma += db.DetalleCoberturas.Where(t => t.BienAseguradoId == item.Id).Max(r => r.SumaAsegurada);
                    }
                    s.Assa = suma;
                }
                else
                    s.Assa = 0;
            }
            else
                s.Assa = 0;

            var x2 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && b.Producto.Ramo.Descripcion == ramo && b.Producto.Aseguradora.Descripcion.Contains("américa")).ToList();
            if (x2 != null)
            {
                var i = x2.Select(y => y.Id);
                var b = db.DetalleBienesAsegurados.Where(r => i.Contains(r.PolizaId)).Select(y => y.BienAsegurado);
                if (b != null)
                {
                    suma = 0;
                    foreach (var item in b)
                    {
                        suma += db.DetalleCoberturas.Where(t => t.BienAseguradoId == item.Id).Max(r => r.SumaAsegurada);
                    }
                    s.America = suma;
                }
                else
                    s.America = 0;
            }
            else
                s.America = 0;

            var x3 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && b.Producto.Ramo.Descripcion == ramo && b.Producto.Aseguradora.Descripcion.Contains("iniser")).ToList();
            if (x3 != null)
            {
                var i = x3.Select(y => y.Id);
                var b = db.DetalleBienesAsegurados.Where(r => i.Contains(r.PolizaId)).Select(y => y.BienAsegurado);
                if (b != null)
                {
                    suma = 0;
                    foreach (var item in b)
                    {
                        suma += db.DetalleCoberturas.Where(t => t.BienAseguradoId == item.Id).Max(r => r.SumaAsegurada);
                    }
                    s.INISER = suma;
                }
                else
                    s.INISER = 0;
            }
            else
                s.INISER = 0;

            var x4 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && b.Producto.Ramo.Descripcion == ramo && b.Producto.Aseguradora.Descripcion.Contains("lafise")).ToList();
            if (x4 != null)
            {
                var i = x4.Select(y => y.Id);
                var b = db.DetalleBienesAsegurados.Where(r => i.Contains(r.PolizaId)).Select(y => y.BienAsegurado);
                if (b != null)
                {
                    suma = 0;
                    foreach (var item in b)
                    {
                        suma += db.DetalleCoberturas.Where(t => t.BienAseguradoId == item.Id).Max(r => r.SumaAsegurada);
                    }
                    s.Lafise = suma;
                }
                else
                    s.Lafise = 0;
            }
            else
                s.Lafise = 0;

            var x5 = db.Polizas.Where(b => b.FechaEmision >= desde && b.FechaHasta <= hasta && b.Producto.Ramo.Descripcion == ramo && b.Producto.Aseguradora.Descripcion.Contains("mapfre")).ToList();
            if (x5 != null)
            {
                var i = x5.Select(y => y.Id);
                var b = db.DetalleBienesAsegurados.Where(r => i.Contains(r.PolizaId)).Select(y => y.BienAsegurado);
                if (b != null)
                {
                    suma = 0;
                    foreach (var item in b)
                    {
                        suma += db.DetalleCoberturas.Where(t => t.BienAseguradoId == item.Id).Max(r => r.SumaAsegurada);
                    }
                    s.MAPFRE = suma;
                }
                else
                    s.MAPFRE = 0;
            }
            else
                s.MAPFRE = 0;
            return s;
        }

        //============================AJAX======================================
        public ActionResult Compañias()
        {
            var data = (from item in db.Aseguradoras.ToList()
                        select new
                        {
                            Id = item.Id,
                            Nombre = item.Descripcion
                        }).ToList();

            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Agentes()
        {
            var data = (from item in db.Agentes.ToList()
                        select new
                        {
                            Id = item.Id,
                            Nombre = item.NombreCompleto
                        }).ToList();

            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Clientes()
        {
            var data = (from item in db.Clientes.ToList()
                        select new
                        {
                            Id = item.Id,
                            Ident = item.Identificacion,
                            Nombre = item.NombreCompleto
                        }).ToList();

            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Polizas()
        {
            var data = (from item in db.Polizas.ToList()
                        select new
                        {
                            Id = item.Id,
                            Nombre = item.Cliente.NombreCompleto,
                            Pol = item.NumPoliza
                        }).ToList();

            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
    }
}