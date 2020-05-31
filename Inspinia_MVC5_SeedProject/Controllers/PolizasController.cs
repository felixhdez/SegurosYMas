

























using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using System.Net;
using System.Web;
using System.Web.Mvc;

using Inspinia_MVC5_SeedProject.Models;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    [Authorize]
    public class PolizasController : Controller
    {
        private SeguroBD db = new SeguroBD();
        private static int PolizaActual = 0;
        List<String> obj = new List<string> { "Natural", "Jurídico" };
        List<string> tiposmonedas = new List<string> { "Córdobas", "Dólares" };
        List<string> tipospolizas = new List<string> { "Nueva", "Renovación" };
        List<string> FormasdePago = new List<string> { "Contado", "Crédito" };

        public ActionResult SetValue(int? id)
        {
            try {
                if ((int)id == 0)
                    id = db.Polizas.Max(y => y.Id);
                PolizaActual = (int)id;
            } catch (Exception) { 
                id = 0;
            }
            return Json(new { id },JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetValue2()
        {
            PolizaActual = 0;
            return Json(new { PolizaActual }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetValue()
        {
            return Json(new { PolizaActual }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPoliza(int? id)
        {
            var x = db.Polizas.ToList();
            var lista = from item in x
                        where item.Id == (int)id
                        select new
                        {
                            IdPoliza = item.Id,
                            NumPoliza = item.NumPoliza,
                            Nombres = item.Cliente.Nombres,
                            Apellidos = item.Cliente.Apellidos,
                            Identificacion = item.Cliente.Identificacion,
                            Producto = item.Producto.Descripcion,
                            Aseguradora = item.Producto.Aseguradora.Descripcion
                        };
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Polizas/

        public ActionResult Index()

        {
            ViewBag.hoy = DateTime.Today;
            var polizas = db.Polizas.Include(p => p.Agente).Include(p => p.Cliente).Include(p => p.ContactoIntermediario).Include(p => p.Persona).Include(p => p.Producto);

            return View(polizas.ToList());


        }

        // GET: /Polizas/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Poliza poliza = db.Polizas.Find(id);

            if (poliza == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Archivos = db.ArchivosPolizas.Where(x => x.PolizaId == (int)id).ToList();

            return View(poliza);
        }

        // GET: /Polizas/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {
            ViewBag.FormadePago = new SelectList(FormasdePago);

            ViewBag.TipoPoliza = new SelectList(tipospolizas);

            ViewBag.TipoMoneda = new SelectList(tiposmonedas);

            ViewBag.DropDown = new SelectList(obj);

            ViewBag.AgenteId = new SelectList(db.Agentes, "Id", "NombreCompleto");

            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "Apellidos");

            ViewBag.ContactoIntermediarioId = new SelectList(db.ContactoIntermediarios, "Id", "Nombres");

            ViewBag.PersonaId = new SelectList(db.Personas, "Id", "Apellidos");

            ViewBag.AseguradoraId = new SelectList(db.Aseguradoras, "Id", "Descripcion");

            ViewBag.ProductoId = new SelectList(db.Productos, "Id", "Descripcion");

            ViewBag.DepartamentoId = new SelectList(db.Departamentos, "Id", "Descripcion");

            return View();
        }

        public ActionResult Filtro(int? Id)
        {
            var s = db.Productos.Where(x => x.AseguradoraId == (int)Id);
            var dato = from item in s
                       select new
                       {
                           IdProducto = item.Id,
                           Descripcion = item.Descripcion
                       };
            return Json(new { listProd = dato }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComision(int? Id)
        {
            var s = db.Productos.Where(x => x.Id == (int)Id).Select(z=> z.Comision).FirstOrDefault();
            return Json(new {s}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTipoDeCambio(DateTime fecha)
        {
            TipoDeCambioWS.Tipo_Cambio_BCNSoapClient servicio = new TipoDeCambioWS.Tipo_Cambio_BCNSoapClient();
            DateTime ultimodia = UltimoDia(fecha);
            double s = servicio.RecuperaTC_Dia(ultimodia.Year, ultimodia.Month, ultimodia.Day);
            return Json(new {s}, JsonRequestBehavior.AllowGet);
        }

        DateTime UltimoDia(DateTime fecha)
        {
            fecha = fecha.AddMonths(1);
            fecha = fecha.AddDays(-fecha.Day);
            return fecha;
        }

        public ActionResult RecuperarBien(int? id)
        {
            var data = db.BienesAsegurados.ToList();
            var lista = from item in data
                        where item.Id == id
                        select new
                        {
                            IdBien = item.Id,
                            Observacion = item.Observacion,
                            Excluido = item.Estado
                        };
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecuperarCuotas(int? id)
        {
            var data = db.DetalleCuotas.ToList();
            var data2 = db.ReciboCuotas.ToList();
            var lista = from item in data
                        where item.PolizaId == (int)id && item.Deshabilitar
                        select new
                        {
                            IdDetalle = item.Id,
                            Cuotas = item.Cuotas,
                            Vence = item.Vence.ToString("yyyy-MM-dd"),
                            Monto = item.Monto,
                            Estado = item.Estado,
                            Saldo = item.Saldo,
                            Recibo = item.Cuota.ReciboDePrima,
                            CuotaId = item.CuotaId
                        };

            //var lista = (from item in data
            //             join item2 in data2 on item.Id equals item2.DetalleCuotaId
            //             where item.PolizaId == (int)id && item.Deshabilitar
            //             select item2.PagoNeto).Sum();
                        

            return Json(new { list = lista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecuperarCaracteristicas(int? Id)
        {
            var listado = from item in db.DetalleCaracteristicas
                          from item2 in db.Caracteristicass
                          where item2.Id == item.CaracteristicaId && item.BienAseguradoId==Id
                          select new
                          {
                              IdCaracteristica = item2.Id,
                              IdDetalle = item.Id,
                              Descripcion = item2.Descripcion,
                              Valor = item.Valor
                          };
            return Json(new { list = listado }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecuperarCoberturas(int? Id)
        {
            var listado = from item in db.DetalleCoberturas
                          from item2 in db.Coberturas
                          where item2.Id == item.CoberturaId && item.BienAseguradoId == Id
                          select new
                          {
                              IdCobertura = item2.Id,
                              IdDetalle = item.Id,
                              Descripcion = item2.Descripcion,
                              Suma = item.SumaAsegurada,
                              Deducible = item.Deducible,
                              Prima = item.Prima
                          };
            return Json(new { list = listado }, JsonRequestBehavior.AllowGet);
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------ARCHIVO------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult RecuperarArchivos(int? id)
        {
            var list = from item in db.ArchivosPolizas
                       where item.PolizaId == (int)id
                       select new
                       {
                           Id = item.Id,
                           Foto = item.Foto
                       };

            return Json(new { list }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult EliminarArchivo(int? id)
        {
            //var ar = db.ArchivosPolizas.Where(x => x.Id == (int)id);
            //ArchivosPólizas archi = new ArchivosPólizas();
            ArchivosPólizas arpoliza = db.ArchivosPolizas.Find(id);
            string url = Path.Combine(Server.MapPath(arpoliza.Foto));
            System.IO.File.Delete(url);
            db.ArchivosPolizas.Remove(arpoliza);
            int d = db.SaveChanges();
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult GuardarARExtra(int? id, HttpPostedFileBase fileUpload)
        {
            int d = 0;
            string path = Server.MapPath("~/Content/Imagen");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                var file = Request.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("~/Content/Imagen"), fileName);
                file.SaveAs(path);
                string url = Path.Combine("/Content/Imagen", fileName);
                ArchivosPólizas obj = new ArchivosPólizas();
                obj.Foto = url;
                obj.PolizaId = (int)id;
                db.ArchivosPolizas.Add(obj);
                d = db.SaveChanges();
            }
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------ARCHIVO------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------------

        // POST: /Polizas/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create([Bind(Include="Id,NumPoliza,TipoMoneda,FechaEmision,FechaDesde,FechaHasta,Tipo,FormaDePago,Emision,Iva,Otros,ComisionPorcentaje,ComisionValor,TipoDeCambio,PrimaNeta,ProductoId,AgenteId")] Poliza poliza, int? IdCliente, int? idContratante, int? IdContacto, int? ultimo, string detalleCuotas)

        {
            int d = -3;
            var x = Request[""];
            
            if (ModelState.IsValid)
            {
                if (db.Polizas.FirstOrDefault(xy => xy.NumPoliza == poliza.NumPoliza && xy.ClienteId != (int)IdCliente) != null)
                    ModelState.AddModelError("NumPoliza", "La póliza que desea ingresar ya existe.");
                else
                {
                    using (var transact = db.Database.BeginTransaction())
                    {
                        try
                        {
                            // Convertimos el parametro a objeto C#
                            var det = JsonConvert.DeserializeObject<List<DetalleCuota>>(detalleCuotas);

                            //Guardamos al cliente
                            if (IdCliente == 0)
                            {
                                Cliente c = new Cliente();
                                c.Apellidos = Request["Cliente.Apellidos"];
                                c.Nombres = Request["Cliente.Nombres"];
                                c.Identificacion = Request["Cliente.Identificacion"];
                                c.NumTelf1 = Request["Cliente.Numtelf1"];
                                c.NumTelf2 = Request["Cliente.Numtelf2"];
                                c.NumTelf3 = Request["Cliente.Numtelf3"];
                                c.Celular = Request["Cliente.Celular"];
                                c.Email = Request["Cliente.Email"];
                                c.TipoCliente = Request["TipoCliente"];
                                c.FechaNacimiento = DateTime.Parse(Request["Cliente.FechaNacimiento"]);
                                c.DepartamentoId = int.Parse(Request["DepartamentoId"]);
                                c.Notas = Request["Cliente.Notas"];
                                c.Direccion = Request["Cliente.Direccion"];
                                db.Clientes.Add(c);
                                db.SaveChanges();
                                poliza.ClienteId = c.Id;
                            }
                            else
                                poliza.ClienteId = (int)IdCliente;

                            //Guardamos al contratante
                            if (idContratante == 0)
                            {
                                Persona p = new Persona();
                                p.Apellidos = Request["Persona.Apellidos"];
                                p.Nombres = Request["Persona.Nombres"];
                                p.Identificacion = Request["Persona.Identificacion"];
                                p.NumTelf1 = Request["Persona.Numtelf1"];
                                p.NumTelf2 = Request["Persona.Numtelf2"];
                                p.NumTelf3 = Request["Persona.Numtelf3"];
                                p.Celular = Request["Persona.Celular"];
                                p.Email = Request["Persona.Email"];
                                p.DepartamentoId = int.Parse(Request["DeptoId"]);
                                p.Notas = Request["Persona.Notas"];
                                p.Direccion = Request["Persona.Direccion"];
                                db.Personas.Add(p);
                                db.SaveChanges();
                                poliza.PersonaId = p.Id;
                            }
                            else
                                poliza.PersonaId = (int)idContratante;

                            //Guardamos al contacto intermediario
                            if (IdContacto == 0)
                            {
                                ContactoIntermediario i = new ContactoIntermediario();
                                i.Apellidos = Request["ContactoIntermediario.Apellidos"];
                                i.Nombres = Request["ContactoIntermediario.Nombres"];
                                i.Telefono = Request["ContactoIntermediario.Telefono"];
                                i.Email = Request["ContactoIntermediario.Email"];
                                i.Cargo = Request["ContactoIntermediario.Cargo"];
                                db.ContactoIntermediarios.Add(i);
                                db.SaveChanges();
                                poliza.ContactoIntermediarioId = i.Id;
                            }
                            else
                                poliza.ContactoIntermediarioId = (int)IdContacto;

                            //Guardamos la poliza
                            db.Polizas.Add(poliza);
                            db.SaveChanges();

                            Cuota u = new Cuota();
                            u.NumCoutas = (int)ultimo;
                            if (ultimo == 1)
                                u.TipoCuotas = "Número Único";
                            else
                                u.TipoCuotas = "Consecutivas";
                            u.ReciboDePrima = Request["ReciboPrima"];
                            db.Cuotas.Add(u);
                            db.SaveChanges();

                            //Guardamos los detalles de cuotas
                            foreach (var item in det)
                            {
                                item.PolizaId = poliza.Id;
                                item.CuotaId = u.Id;
                                item.Deshabilitar = true;
                                item.Saldo = item.Monto;
                                db.DetalleCuotas.Add(item);
                            }
                            d = db.SaveChanges();
                            
                            //Si todo se hizo correctamente se guardan los datos definitivamente
                            transact.Commit();
                        }
                        catch (Exception)
                        {
                            //Si hubo algun error en el almacenamiento de los datos
                            //deshacemos todos lo que habiamos guardado
                            transact.Rollback();
                        }
                    }
                }
            }

            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Polizas/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Poliza poliza = db.Polizas.Find(id);
            

            if (poliza == null)
            {
                return HttpNotFound();
            }


            ViewBag.AgenteId = new SelectList(db.Agentes, "Id", "NombreCompleto", poliza.AgenteId);

            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "NombreCompleto", poliza.ClienteId);

            ViewBag.ContactoIntermediarioId = new SelectList(db.ContactoIntermediarios, "Id", "NombreCompleto", poliza.ContactoIntermediarioId);

            //var lista = db.Personas.ToList();
            var lista_exclusion = db.Personas.Where(y => !db.Clientes.Select(x => x.Id).Contains(y.Id));
            ViewBag.PersonaId = new SelectList(lista_exclusion, "Id", "NombreCompleto", poliza.PersonaId);

            ViewBag.ProductoId = new SelectList(db.Productos, "Id", "Descripcion", poliza.ProductoId);

            //Propios
            ViewBag.IdAseguradora = new SelectList(db.Aseguradoras, "Id", "Descripcion", poliza.Producto.AseguradoraId);

            ViewBag.TipoMoneda = new SelectList(tiposmonedas, poliza.TipoMoneda);

            ViewBag.TipoPoliza = new SelectList(tipospolizas, poliza.Tipo);

            ViewBag.FormadePago = new SelectList(FormasdePago, poliza.FormaDePago);

            List<object> obj = (from item in db.BienesAsegurados
                                from item2 in db.DetalleBienesAsegurados
                                where item.Id == item2.BienAseguradoId && item2.PolizaId == poliza.Id
                                select new
                                {
                                    IdCert = item.Id,
                                    NumCertificado = item.NumCertificado,
                                    Excluido = item.Estado,
                                    Observacion = item.Observacion
                                }).ToList<object>();

            ViewBag.BienAseguradoId = new SelectList(obj, "IdCert", "NumCertificado");

            return View(poliza);
        }

        // POST: /Polizas/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit([Bind(Include="Id,NumPoliza,TipoMoneda,FechaEmision,FechaDesde,FechaHasta,Tipo,FormaDePago,PrimaNeta,Emision,Iva,Otros,ComisionPorcentaje,ComisionValor,TipoDeCambio,ProductoId,AgenteId,ClienteId,ContactoIntermediarioId,PersonaId")] Poliza poliza)

        {
            var x = Request[""];
            if (ModelState.IsValid)
            {
                using (var transact = db.Database.BeginTransaction())
                {
                    try
                    {  
                        if (db.Polizas.FirstOrDefault(xy => xy.NumPoliza == poliza.NumPoliza && xy.ClienteId == poliza.ClienteId && xy.ProductoId == poliza.ProductoId && xy.Id!=poliza.Id) != null)
                            ModelState.AddModelError("NumPoliza", "La poliza que desea ingresar ya existe.");
                        else
                        {
                            db.Entry(poliza).State = EntityState.Modified;

                            db.SaveChanges();
                            if (Request.Form.AllKeys.Contains("BienAseguradoId"))
                            {
                                BienAsegurado mod = db.BienesAsegurados.Find(int.Parse(Request["BienAseguradoId"]));
                                mod.Observacion = Request["Observacion"];
                                string valor = Request["Exclusion"];
                                if (valor == null)
                                    mod.Estado = false;
                                else
                                    mod.Estado = true;
                                db.Entry(mod).State = EntityState.Modified;
                                db.SaveChanges();
                            }

                            //Si todo se hizo correctamente se guardan los datos definitivamente
                            transact.Commit();
                            return RedirectToAction("Index");
                        } 
                    }
                    catch (Exception)
                    {
                        //Si hubo algun error en el almacenamiento de los datos
                        //deshacemos todos lo que habiamos guardado
                        transact.Rollback();
                    }
                }
            }

            //ViewBag.AgenteId = new SelectList(db.Agentes, "Id", "NombreCompleto", poliza.AgenteId);

            //ViewBag.ClienteId = new SelectList(db.Personas, "Id", "Apellidos", poliza.ClienteId);

            //ViewBag.ContactoIntermediarioId = new SelectList(db.ContactoIntermediarios, "Id", "Nombres", poliza.ContactoIntermediarioId);

            //ViewBag.PersonaId = new SelectList(db.Personas, "Id", "Apellidos", poliza.PersonaId);

            //ViewBag.ProductoId = new SelectList(db.Productos, "Id", "Codigo", poliza.ProductoId);

            ViewBag.AgenteId = new SelectList(db.Agentes, "Id", "NombreCompleto", poliza.AgenteId);

            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "NombreCompleto", poliza.ClienteId);

            ViewBag.ContactoIntermediarioId = new SelectList(db.ContactoIntermediarios, "Id", "NombreCompleto", poliza.ContactoIntermediarioId);

            //var lista_exclusion = db.Personas.Where(y => !db.Clientes.Select(x => x.Id).Contains(y.Id));
            ViewBag.PersonaId = new SelectList(db.Personas, "Id", "NombreCompleto", poliza.PersonaId);

            ViewBag.ProductoId = new SelectList(db.Productos, "Id", "Descripcion", poliza.ProductoId);

            //Propios
            ViewBag.IdAseguradora = new SelectList(db.Aseguradoras, "Id", "Descripcion", poliza.Producto.AseguradoraId);

            ViewBag.TipoMoneda = new SelectList(tiposmonedas, poliza.TipoMoneda);

            ViewBag.TipoPoliza = new SelectList(tipospolizas, poliza.Tipo);

            ViewBag.FormadePago = new SelectList(FormasdePago, poliza.FormaDePago);

            List<object> obj = (from item in db.BienesAsegurados
                                from item2 in db.DetalleBienesAsegurados
                                where item.Id == item2.BienAseguradoId && item2.PolizaId == poliza.Id
                                select new
                                {
                                    IdCert = item.Id,
                                    NumCertificado = item.NumCertificado,
                                    Excluido = item.Estado,
                                    Observacion = item.Observacion
                                }).ToList<object>();

            ViewBag.BienAseguradoId = new SelectList(obj, "IdCert", "NumCertificado");

            return View(poliza);
        }

        // GET: /Polizas/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Poliza poliza = db.Polizas.Find(id);

            if (poliza == null)
            {
                return HttpNotFound();
            }
            return View(poliza);
        }

        // POST: /Polizas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            Poliza poliza = db.Polizas.Find(id);

            db.Polizas.Remove(poliza);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult DetalleCuota(int? IdDetalle)
        {
            var datos = db.DetalleCuotas.ToList();
            var listado = from item in datos
                          where item.Id == IdDetalle
                          select new
                          {
                              Num = item.Cuotas,
                              Fecha = item.Vence.ToString("yyyy-MM-dd"),
                              Monto = item.Monto,
                              Estado = item.Estado
                          };
            return Json(new { list = listado }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult GuardarCambioCuotas(int? idDetalle, string fecha, string monto)
        {
            DetalleCuota obj = db.DetalleCuotas.Find(idDetalle);
            obj.Vence = DateTime.Parse(fecha);
            obj.Monto = double.Parse(monto);
            obj.Saldo = obj.Monto;
            db.Entry(obj).State = EntityState.Modified;
            int d = db.SaveChanges();
            return Json(new {d}, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult ActualizarCuotas(int? idPoliza, string numcuotas, string vence, string monto, string recibo)
        {
            int cantd = int.Parse(numcuotas), cuota = 1;
            using (var transact = db.Database.BeginTransaction())
            {
                try
                {

                    
            DateTime fecha = DateTime.Parse(vence);
            double _monto = double.Parse(monto);
            List<DetalleCuota> lista = db.DetalleCuotas.Where(x => x.PolizaId == (int)idPoliza).ToList();
            for (int i = 0; i < lista.Count; i++)
            {
                DetalleCuota reemp = db.DetalleCuotas.Find(lista[i].Id);
                if (i < cantd)
                {
                    if(i!=0)
                        fecha = fecha.AddMonths(1);
                    reemp.Cuotas = (i + 1).ToString();
                    reemp.Monto = _monto;
                    reemp.Saldo = reemp.Monto;
                    reemp.Vence = fecha;
                    reemp.Deshabilitar = true;
                    cuota++;
                }
                else
                    reemp.Deshabilitar = false;
                db.Entry(reemp).State = EntityState.Modified;
            }
            db.SaveChanges();

            if(cantd>lista.Count)
            {
                for (int a = cuota; a <= cantd; a++)
                {
                    fecha = fecha.AddMonths(1);
                    DetalleCuota nuevo = new Models.DetalleCuota();
                    nuevo.Cuotas = a.ToString();
                    nuevo.Vence = fecha;
                    nuevo.Monto = _monto;
                    nuevo.Saldo = nuevo.Monto;
                    nuevo.Estado = "Pendiente";
                    nuevo.PolizaId = (int)idPoliza;
                    nuevo.CuotaId = lista[0].CuotaId;
                    nuevo.Deshabilitar = true;
                    db.DetalleCuotas.Add(nuevo);
                }
                db.SaveChanges();
            }

            Cuota cambiar = db.Cuotas.Find(lista[0].CuotaId);
            cambiar.NumCoutas = cantd;
            if (cantd == 1)
                cambiar.TipoCuotas = "Número Único";
            else
                cambiar.TipoCuotas = "Consecutivas";
            cambiar.ReciboDePrima = recibo;
            db.Entry(cambiar).State = EntityState.Modified;
            db.SaveChanges();

                    //Si todo se hizo correctamente se guardan los datos definitivamente
                    transact.Commit();
                }
                catch (Exception)
                {
                    //Si hubo algun error en el almacenamiento de los datos
                    //deshacemos todos lo que habiamos guardado
                    transact.Rollback();
                }
            }
            return Json(new { cantd }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Objeto(int? id)
        {
            var obj = db.Polizas.Where(x=> x.Id == (int)id);
            var objeto = from item in obj
                         select new
                         {
                             FormaDePago = item.FormaDePago,
                             TipoPoliza = item.Tipo,
                             TipoMoneda = item.TipoMoneda
                         };
            return Json(new { objeto }, JsonRequestBehavior.AllowGet);
        }

        //================+++++++++++++++++++RENOVACIÓN+++++++++++++++++++================
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult CreateRenovacion() {
            ViewBag.AgenteId = new SelectList(db.Agentes,"Id","NombreCompleto");

            ViewBag.ClienteId = new SelectList(db.Clientes,"Id","NombreCompleto");

            ViewBag.ContactoIntermediarioId = new SelectList(db.ContactoIntermediarios,"Id","NombreCompleto");

            //var lista = db.Personas.ToList();
            var lista_exclusion = db.Personas.Where(y => !db.Clientes.Select(x => x.Id).Contains(y.Id));
            ViewBag.PersonaId = new SelectList(lista_exclusion,"Id","NombreCompleto");

            ViewBag.ProductoId = new SelectList(db.Productos,"Id","Descripcion");

            //Propios
            ViewBag.IdAseguradora = new SelectList(db.Aseguradoras,"Id","Descripcion");

            ViewBag.TipoMoneda = new SelectList(tiposmonedas);

            ViewBag.TipoPoliza = new SelectList(tipospolizas);

            ViewBag.FormadePago = new SelectList(FormasdePago);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult CreateRenovacion([Bind(Include = "Id,NumPoliza,TipoMoneda,FechaEmision,FechaDesde,FechaHasta,Tipo,FormaDePago,Emision,Iva,Otros,ComisionPorcentaje,ComisionValor,TipoDeCambio,PrimaNeta,ProductoId,AgenteId,ClienteId,PersonaId,ContactoIntermediarioId")] Poliza poliza, int? ultimo,string detalleCuotas, string detalleArchivos, string detalleBienes) {
            int d = -3;
            if (ModelState.IsValid) {
                using (var transact = db.Database.BeginTransaction()) {
                    try {
                        var cuotas = JsonConvert.DeserializeObject<List<DetalleCuota>>(detalleCuotas);
                        var bienes = JsonConvert.DeserializeObject<List<DetalleBienAsegurado>>(detalleBienes);
                        var archivos = JsonConvert.DeserializeObject<List<ArchivosPólizas>>(detalleArchivos);

                        //Almacemos lo principal, la póliza
                        db.Polizas.Add(poliza);
                        db.SaveChanges();

                        //Almacenamos el principal de las cuotas
                        Cuota u = new Cuota();
                        u.NumCoutas = (int)ultimo;
                        if (ultimo == 1)
                            u.TipoCuotas = "Número Único";
                        else
                            u.TipoCuotas = "Consecutivas";
                        u.ReciboDePrima = Request["ReciboPrima"];
                        db.Cuotas.Add(u);
                        db.SaveChanges();

                        //Guardamos los detalles de cuotas
                        foreach (var item in cuotas) {
                            item.PolizaId = poliza.Id;
                            item.CuotaId = u.Id;
                            item.Deshabilitar = true;
                            item.Saldo = item.Monto;
                            db.DetalleCuotas.Add(item);
                        }
                        d = db.SaveChanges();

                        //Almacenamos los biene asegurados
                        foreach (var item in bienes) {
                            item.PolizaId = poliza.Id;
                            db.DetalleBienesAsegurados.Add(item);
                        }
                        db.SaveChanges();

                        //Almacenamos los archivos
                        foreach (var item in archivos) {
                            item.PolizaId = poliza.Id;
                            db.ArchivosPolizas.Add(item);
                        }
                        d = db.SaveChanges();

                        transact.Commit();
                    } catch (Exception) {
                        transact.Rollback();
                    }
                }
            }

            return Json(new { d },JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult GetFull(int id) {

            var item = db.Polizas.Where(x=> x.Id == id).ToList();
            var objeto = from i in item
                         select new {
                             IdPoliza = i.Id,
                             Cliente = i.Cliente.NombreCompleto,
                             ClienteId= i.ClienteId,
                             Contratante = i.Persona.NombreCompleto,
                             ContratanteId = i.PersonaId,
                             Contacto = i.ContactoIntermediario.NombreCompleto,
                             ContactoId = i.ContactoIntermediarioId,
                             NumPoliza = i.NumPoliza,
                             Aseguradora = i.Producto.AseguradoraId,
                             Producto = i.ProductoId,
                             TipoMoneda = i.TipoMoneda,
                             FechaEmision = i.FechaEmision.ToString("yyyy-MM-dd"),
                             Desde = i.FechaHasta.ToString("yyyy-MM-dd"),
                             Hasta = i.FechaHasta.AddYears(1).ToString("yyyy-MM-dd"),
                             TipoPoliza = i.Tipo,
                             PrimaNeta = i.PrimaNeta,
                             Derecho = i.Emision,
                             IVA = i.Iva,
                             Otros = i.Otros,
                             ComisionP = i.ComisionPorcentaje,
                             ComisionV = i.ComisionValor,
                             FormaPago = i.FormaDePago,
                             Agente = i.AgenteId
                         };


            return Json(new { objeto },JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecuperarBienesNoExcluidos(int? id) {
            var data = db.BienesAsegurados.Where(x=> !x.Estado).ToList();
            var data2 = db.DetalleBienesAsegurados.Where(r => r.PolizaId == (int)id).ToList();
            var lista = from item in data
                        join det in data2 on item.Id equals det.BienAseguradoId
                        select new {
                            IdBien = item.Id,
                            NumCertf = item.NumCertificado,
                            Observacion = item.Observacion
                        };
            return Json(new { lista },JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult ArchivosRenovacion(int? id,HttpPostedFileBase fileUpload) {
            string d = "";
            string path = Server.MapPath("~/Content/Imagen");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            if (Request.Files.Count > 0) {
                var file = Request.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("~/Content/Imagen"),fileName);
                file.SaveAs(path);
                d = Path.Combine("/Content/Imagen",fileName);
            }
            return Json(new { d },JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult EliminarRenovacion(string url) {
            int d = 0;
            try {
                string eliminar = Path.Combine(Server.MapPath(url));
                System.IO.File.Delete(eliminar);
            } catch (Exception) {
                d = -3;
            }
            return Json(new { d },JsonRequestBehavior.AllowGet);
        }
    }
}
