

























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

namespace Inspinia_MVC5_SeedProject.Controllers
{
    [Authorize]
    public class AdendasController : Controller
    {
        private SeguroBD db = new SeguroBD();
        private static int AdendaActual = 0;

        // GET: /Adendas/

        List<string> TiposAdenda = new List<string> { "Cancelación", "Exclusión", "Inclusión", "Modificación", "Liquidación" };
        public ActionResult Index()
        {
            //List<int> lista1 = db.DetalleAdendas.Select(x=> x.PolizaId).ToList();
            //List<string> lista2 = db.Polizas.Where(y => lista1.Contains(y.Id)).Select(z=> z.NumPoliza).ToList();
            //ViewBag.NumPoliza = lista2;
            return View(db.Adendas.ToList());
        }

        public ActionResult SetValue(int? id) {
            try {
                if ((int)id == 0)
                    id = db.Adendas.Max(y => y.Id);
                AdendaActual = (int)id;
            } catch (Exception) {
                id = 0;
            }
            
            return Json(new { id },JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetValue2() {
            AdendaActual = 0;
            return Json(new { AdendaActual },JsonRequestBehavior.AllowGet);
           
        }

        public ActionResult GetValue() {
            return Json(new { AdendaActual },JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAdenda(int? id) {
            var x = db.Adendas.ToList();
            var lista = from item in x
                        where item.Id == (int)id
                        select new {
                            IdAdenda = item.Id,
                            Nombre = item.Poliza.Cliente.NombreCompleto,
                            Identificacion = item.Poliza.Cliente.Identificacion,
                            NumAdenda = item.NumAdenda,
                            Producto = item.Poliza.Producto.Descripcion,
                            Aseguradora = item.Poliza.Producto.Aseguradora.Descripcion,
                            IdPoliza = item.Poliza.Id,
                            TipodeAdenda = item.TipoAdenda,
                        };
            return Json(new { lista },JsonRequestBehavior.AllowGet);
        }

        // GET: /Adendas/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Adenda adenda = db.Adendas.Find(id);

            if (adenda == null)
            {
                return HttpNotFound();
            }
            return View(adenda);
        }

        // GET: /Adendas/Create
        [Authorize(Roles ="Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {
            ViewBag.TiposAdenda = new SelectList(TiposAdenda);
            
            return View();
        }

        // POST: /Adendas/Create

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 

        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,NumAdenda,TipoAdenda,FechaEmision,FechaDesde,FechaHasta,SumaAsegurada,PrimaNeta,Iva,Otros,ComisionEspecial,TipoDeCambio,Comentario,PolizaId")] Adenda adenda, int? PolizaId, int? ultimo, string detalleCuotas)

        {
            int d = -3;
            var x = Request[""];
            if (ModelState.IsValid)
            {
                if (db.Adendas.FirstOrDefault(t => t.NumAdenda == adenda.NumAdenda) != null)
                {
                    ModelState.AddModelError("NumAdenda", "El número de adenda que intenta ingresar ya existe.");
                }
                else
                {
                    using (var transact = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var det = JsonConvert.DeserializeObject<List<DetalleCuotasAdenda>>(detalleCuotas);
                            //Guardamos la adenda
                            adenda.PolizaId = (int)PolizaId;
                        db.Adendas.Add(adenda);
                        db.SaveChanges();

                        //Guardamos en la tabla cuota
                        Cuota u = new Cuota();
                        u.NumCoutas = (int)ultimo;
                        if (ultimo == 1)
                            u.TipoCuotas = "Número Único";
                        else
                            u.TipoCuotas = "Consecutivas";
                        u.ReciboDePrima = Request["ReciboPrima"];
                        db.Cuotas.Add(u);
                        db.SaveChanges();

                        //if(adenda.TipoAdenda=="Cancelación")
                        //{
                        //    List<int> lista = db.DetalleBienesAsegurados.Where(y => y.PolizaId == (int)PolizaId).Select(z => z.BienAseguradoId).ToList();
                        //    List<BienAsegurado> bienes = db.BienesAsegurados.Where(a => lista.Contains(a.Id)).ToList();
                        //    foreach (var item in bienes)
                        //    {
                        //        item.Estado = true;
                        //        db.Entry(item).State = EntityState.Modified;
                        //    }
                        //    db.SaveChanges();
                        //}

                        //Guardamos los detalles de cuotas
                        foreach (var item in det)
                        {
                            item.AdendaId = adenda.Id;
                            item.CuotaId = u.Id;
                            item.Saldo = item.Monto;
                            item.Deshabilitar = true;
                            db.DetalleCuotasAdenda.Add(item);
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
            else
                d = -2;
            
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult getAdenda()
        {
            var list = from item in db.Adendas
                       join poliza in db.Polizas on item.PolizaId equals poliza.Id
                       select new
                       {
                           IdAdenda = item.Id,
                           NumPoliza = poliza.NumPoliza,
                           IdPoliza = poliza.Id,
                           Cliente = poliza.Cliente.Nombres + " " + poliza.Cliente.Apellidos,
                           Identificacion = poliza.Cliente.Identificacion,
                           NumerodeAdenda = item.NumAdenda,
                           TipodeAdenda = item.TipoAdenda,
                           Producto = item.Poliza.Producto.Descripcion,
                           Aseguradora = item.Poliza.Producto.Aseguradora.Descripcion
                       };
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RecuperarAdendas(int? id)
        {
            var a = db.Adendas.ToList();
            var d = db.DetalleAdendas.ToList();
            var lista = from item in a
                       where item.PolizaId == (int)id
                       select new
                       {
                           IdAdenda = item.Id,
                           NumAdenda = item.NumAdenda,
                           Tipo = item.TipoAdenda,
                           Desde = item.FechaDesde.ToString("yyyy-MM-dd"),
                           Hasta = item.FechaHasta.ToString("yyyy-MM-dd"),
                           Suma = item.SumaAsegurada
                       };
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------ARCHIVO------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------------
        //public ActionResult RecuperarArchivos(int? id)
        //{
        //    var list = from item in db.ArchivosAdendas
        //               where item.AdendaId == (int)id
        //               select new
        //               {
        //                   Id = item.Id,
        //                   Foto = item.Foto
        //               };

        //    return Json(new { list }, JsonRequestBehavior.AllowGet);
        //}


        //public ActionResult EliminarArchivo(int? id)
        //{
        //    //var ar = db.ArchivosPolizas.Where(x => x.Id == (int)id);
        //    //ArchivosPólizas archi = new ArchivosPólizas();
        //    ArchivosAdendas aradenda = db.ArchivosAdendas.Find(id);
        //    string url = Path.Combine(Server.MapPath(aradenda.Foto));
        //    System.IO.File.Delete(url);
        //    db.ArchivosAdendas.Remove(aradenda);
        //    int d = db.SaveChanges();
        //    return Json(new { d }, JsonRequestBehavior.AllowGet);
        //}



        //[HttpPost]
        //public ActionResult GuardarARExtra(int? id, HttpPostedFileBase fileUpload)
        //{
        //    string path = Server.MapPath("~/Content/Imagen");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    var file = Request.Files[0];
        //    var fileName = Path.GetFileName(file.FileName);
        //    path = Path.Combine(Server.MapPath("~/Content/Imagen"), fileName);
        //    file.SaveAs(path);
        //    string url = Path.Combine("/Content/Imagen", fileName);
        //    ArchivosAdendas obj = new ArchivosAdendas();
        //    obj.Foto = url;
        //    obj.AdendaId = (int)id;
        //    db.ArchivosAdendas.Add(obj);
        //    int d = db.SaveChanges();
        //    return Json(new { d }, JsonRequestBehavior.AllowGet);
        //}
        //----------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------ARCHIVO------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------------


        // GET: /Adendas/Edit/5
        [Authorize(Roles ="Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Adenda adenda = db.Adendas.Find(id);

            if (adenda == null)
            {
                return HttpNotFound();
            }

            ViewBag.TiposAdenda = new SelectList(TiposAdenda, adenda.TipoAdenda);
            return View(adenda);
        }

        // POST: /Adendas/Edit/5

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 

        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,NumAdenda,TipoAdenda,FechaEmision,FechaDesde,FechaHasta,SumaAsegurada,PrimaNeta,Iva,Otros,ComisionEspecial,TipoDeCambio,Comentario,PolizaId")] Adenda adenda)

        {
            if (ModelState.IsValid)
            {
                db.Entry(adenda).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.TiposAdenda = new SelectList(TiposAdenda, adenda.TipoAdenda);
            return View(adenda);
        }

        // GET: /Adendas/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Adenda adenda = db.Adendas.Find(id);

            if (adenda == null)
            {
                return HttpNotFound();
            }
            return View(adenda);
        }

        // POST: /Adendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            Adenda adenda = db.Adendas.Find(id);

            db.Adendas.Remove(adenda);

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
            var datos = db.DetalleCuotasAdenda.ToList();
            var listado = from item in datos
                          where item.Id == IdDetalle
                          select new
                          {
                              Num = item.Couta,
                              Fecha = item.Vence.ToString("yyyy-MM-dd"),
                              Monto = item.Monto,
                              Estado = item.Estado
                          };
            return Json(new { list = listado }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCambioCuotas(int? idDetalle, string fecha, string monto)
        {
            DetalleCuotasAdenda obj = db.DetalleCuotasAdenda.Find(idDetalle);
            obj.Vence = DateTime.Parse(fecha);
            obj.Monto = double.Parse(monto);
            obj.Saldo = obj.Monto;
            db.Entry(obj).State = EntityState.Modified;
            int d = db.SaveChanges();
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecuperarCuotas(int? id)
        {
            var data = db.DetalleCuotasAdenda.ToList();
            var lista = from item in data
                        where item.AdendaId == (int)id && item.Deshabilitar
                        select new
                        {
                            IdDetalle = item.Id,
                            Cuotas = item.Couta,
                            Vence = item.Vence.ToString("yyyy-MM-dd"),
                            Monto = item.Monto,
                            Saldo = item.Saldo,
                            Estado = item.Estado,
                            Recibo = item.Cuota.ReciboDePrima,
                            CuotaId = item.CuotaId
                        };
            return Json(new { list = lista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarCuotas(int? IdAdenda, string numcuotas, string vence, string monto, string recibo)
        {

            int cantd = int.Parse(numcuotas), cuota = 1;
            using (var transact = db.Database.BeginTransaction())
            {
                try
                {


                    DateTime fecha = DateTime.Parse(vence);
            double _monto = double.Parse(monto);
            List<DetalleCuotasAdenda> lista = db.DetalleCuotasAdenda.Where(x => x.AdendaId == (int)IdAdenda).ToList();
            for (int i = 0; i < lista.Count; i++)
            {
                DetalleCuotasAdenda reemp = db.DetalleCuotasAdenda.Find(lista[i].Id);
                if (i < cantd)
                {
                    if (i != 0)
                        fecha = fecha.AddMonths(1);
                    reemp.Couta = (i + 1).ToString();
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

            if (cantd > lista.Count)
            {
                for (int a = cuota; a <= cantd; a++)
                {
                    fecha = fecha.AddMonths(1);
                    DetalleCuotasAdenda nuevo = new Models.DetalleCuotasAdenda();
                    nuevo.Couta = a.ToString();
                    nuevo.Vence = fecha;
                    nuevo.Monto = _monto;
                    nuevo.Saldo = nuevo.Monto;
                    nuevo.Estado = "Pendiente";
                    nuevo.AdendaId = (int)IdAdenda;
                    nuevo.CuotaId = lista[0].CuotaId;
                    nuevo.Deshabilitar = true;
                    db.DetalleCuotasAdenda.Add(nuevo);
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
    }
}
