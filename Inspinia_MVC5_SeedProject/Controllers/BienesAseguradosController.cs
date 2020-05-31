

























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

namespace Inspinia_MVC5_SeedProject.Controllers
{
    [Authorize]
    public class BienesAseguradosController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /BienesAsegurados/

        public ActionResult Index()

        {
            return View(db.BienesAsegurados.ToList());
        }

        [HttpPost]
        public JsonResult getPolizas()
        {
            var list = from item in db.Polizas
                       from item2 in db.Clientes
                       from item3 in db.Productos
                       where item.ClienteId == item2.Id && item.ProductoId == item3.Id
                       select new
                       {
                           IdPoliza = item.Id,
                           NumPoliza = item.NumPoliza,
                           Apellidos = item2.Apellidos,
                           Nombres = item2.Nombres,
                           Identificacion = item2.Identificacion,
                           Producto = item3.Descripcion,
                           Aseguradora = item3.Aseguradora.Descripcion
                       };
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        // GET: /BienesAsegurados/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BienAsegurado bienAsegurado = db.BienesAsegurados.Find(id);

            if (bienAsegurado == null)
            {
                return HttpNotFound();
            }
            return View(bienAsegurado);
        }

        // GET: /BienesAsegurados/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {
            ViewBag.CaracteristicaId = new SelectList(db.Caracteristicass.ToList(), "Id", "Descripcion");
            ViewBag.CoberturaId = new SelectList(db.Coberturas.ToList(), "Id", "Descripcion");
            return View();
        }

        //GET
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult CreateAdendas()
        {
            ViewBag.CaracteristicaId = new SelectList(db.Caracteristicass.ToList(), "Id", "Descripcion");
            ViewBag.CoberturaId = new SelectList(db.Coberturas.ToList(), "Id", "Descripcion");
            return View();
        }

        // POST: /BienesAsegurados/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include= "Id,NumCertificado,Estado,Observacion")] BienAsegurado bienAsegurado, int? IdPoliza, string detalleCaract, string detalleCobert)
        {
            int d = -3;
            bool band = false;
            // Convertimos el parametro a objeto C#
            var det = JsonConvert.DeserializeObject<List<DetalleCaracteristica>>(detalleCaract);
            var det2 = JsonConvert.DeserializeObject<List<DetalleCobertura>>(detalleCobert);
            if (ModelState.IsValid)
            {
                int Num = bienAsegurado.NumCertificado/*int.Parse(Request["BienesAsegurados.NumCertificado"])*/;
                List<BienAsegurado> lista1 = db.BienesAsegurados.Where(x => x.NumCertificado == Num).ToList();
                if (lista1 != null)
                {
                    foreach (var item in db.DetalleBienesAsegurados.Where(y => y.PolizaId == (int)IdPoliza))
                    {
                        if (lista1.Select(a => a.Id).Contains(item.BienAseguradoId))
                            band = true;
                    }
                }
                if (band)
                    ModelState.AddModelError("NumCertificado", "El número de certificado ya existe para esta póliza.");
                else
                {
                    using (var transact = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.BienesAsegurados.Add(bienAsegurado);
                            db.SaveChanges();

                            //Almacenamos el detalle de Bienes Asegurados
                            DetalleBienAsegurado detallebien = new DetalleBienAsegurado();
                            detallebien.BienAseguradoId = bienAsegurado.Id;
                            detallebien.PolizaId = (int)IdPoliza;
                            db.DetalleBienesAsegurados.Add(detallebien);
                            db.SaveChanges();
                            // Con el id obtenido guardamos los nuevos detalles.
                            foreach (var item in det)
                            {
                                item.BienAseguradoId = bienAsegurado.Id;
                                db.DetalleCaracteristicas.Add(item);
                            }
                            db.SaveChanges();

                            foreach (var item2 in det2)
                            {
                                item2.BienAseguradoId = bienAsegurado.Id;
                                db.DetalleCoberturas.Add(item2);
                            }
                            d = db.SaveChanges();
                            transact.Commit();
                        }
                        catch (Exception)
                        {
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
        public ActionResult CreateAdendas(int? PolizaId, string tipo, string detalleAdenda)
        {
            int d = -3;
            using (var transact = db.Database.BeginTransaction())
            {
                try
                {
                    if (tipo == "Cancelación")
            {
                List<int> lista = db.DetalleBienesAsegurados.Where(x => x.PolizaId == (int)PolizaId).Select(y=> y.BienAseguradoId).ToList();
                foreach (var item in db.BienesAsegurados.ToList())
                {
                    if(lista.Contains(item.Id))
                    {
                        item.Estado = true;
                        db.Entry(item).State = EntityState.Modified;
                    }
                }
                d = db.SaveChanges();
            }
            else
            {
                var det = JsonConvert.DeserializeObject<List<DetalleAdenda>>(detalleAdenda);
                foreach (var item in det)
                {
                    var dat = db.DetalleAdendas.Where(s => s.AdendaId == item.AdendaId && s.BienAseguradoId == item.BienAseguradoId).ToList();
                    if (dat.Count==0)
                        db.DetalleAdendas.Add(item);
                }
                d = db.SaveChanges();

            }

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

            return Json(new { d }, JsonRequestBehavior.AllowGet);


        }

        // GET: /BienesAsegurados/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BienAsegurado bienAsegurado = db.BienesAsegurados.Find(id);

            if (bienAsegurado == null)
            {
                return HttpNotFound();
            }

            return View(bienAsegurado);
        }

        // POST: /BienesAsegurados/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,NumCertificado,Estado,Observacion")] BienAsegurado bienAsegurado)

        {
            if (ModelState.IsValid)
            {
                db.Entry(bienAsegurado).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(bienAsegurado);
        }

        // GET: /BienesAsegurados/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BienAsegurado bienAsegurado = db.BienesAsegurados.Find(id);

            if (bienAsegurado == null)
            {
                return HttpNotFound();
            }
            return View(bienAsegurado);
        }

        // POST: /BienesAsegurados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            BienAsegurado bienAsegurado = db.BienesAsegurados.Find(id);

            db.BienesAsegurados.Remove(bienAsegurado);

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

        public ActionResult GetBienesXPoliza(int? id)
        {
            var lista = from item in db.BienesAsegurados.ToList()
                        join item2 in db.DetalleBienesAsegurados.ToList() on item.Id equals item2.BienAseguradoId
                        where item2.PolizaId == (int)id
                        select new
                        {
                            IdBien = item.Id,
                            NumCertificado = item.NumCertificado,
                            Observacion = item.Observacion,
                            Excluido = item.Estado
                        };
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBienesXAdenda(int? id)
        {
            var lista = from item in db.BienesAsegurados.ToList()
                        join item2 in db.DetalleAdendas.ToList() on item.Id equals item2.BienAseguradoId
                        where item2.AdendaId == (int)id
                        select new
                        {
                            IdBien = item.Id,
                            NumCertificado = item.NumCertificado,
                            Observacion = item.Observacion,
                            Excluido = item.Estado
                        };
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult CambiarEx(int? id, string obs, bool estado)
        {
            BienAsegurado bien = db.BienesAsegurados.FirstOrDefault(x => x.Id == (int)id);
            bien.Observacion = obs;
            bien.Estado = estado;
            db.Entry(bien).State = EntityState.Modified;
            int d = db.SaveChanges();
            return Json(new {d}, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult NuevoB(int? poliza, string numcert, string observacion, string detalleCaract, string detalleCobert)
        {
            int id_ = -3;
            using (var transact = db.Database.BeginTransaction())
            {
                try
                {
                    BienAsegurado b = new BienAsegurado();
                    b.NumCertificado = int.Parse(numcert);
                    b.Observacion = observacion;
                    b.Estado = true;
                    db.BienesAsegurados.Add(b);
                    id_ = db.SaveChanges();

                    DetalleBienAsegurado d = new DetalleBienAsegurado();
                    d.PolizaId = (int)poliza;
                    d.BienAseguradoId = b.Id;
                    db.DetalleBienesAsegurados.Add(d);
                    db.SaveChanges();

                    var detCa = JsonConvert.DeserializeObject<List<DetalleCaracteristica>>(detalleCaract);
                    var detCo = JsonConvert.DeserializeObject<List<DetalleCobertura>>(detalleCobert);

                    foreach (var item in detCa)
                    {
                        item.BienAseguradoId = b.Id;
                        db.DetalleCaracteristicas.Add(item);
                    }
                    db.SaveChanges();

                    foreach (var item in detCo)
                    {
                        item.BienAseguradoId = b.Id;
                        db.DetalleCoberturas.Add(item);
                    }
                    db.SaveChanges();
                    transact.Commit();
                }
                catch (Exception)
                {
                    transact.Rollback();
                }
            }
            return Json(new { id_ }, JsonRequestBehavior.AllowGet);

        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Modificar(int id, string cadena)
        {
            int d = 0;
            BienAsegurado b = db.BienesAsegurados.Find(id);
            b.Observacion = cadena;
            db.Entry(b).State = EntityState.Modified;
            d = db.SaveChanges();
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult AñadirBienRenov(string observacion, int numcert, string detalleCaract, string detalleCobert) {
            int d = -3;
            using (var transact = db.Database.BeginTransaction()) {
                try {

                    //Agregamos el bien asegurado
                    BienAsegurado bien = new BienAsegurado();
                    bien.NumCertificado = numcert;
                    bien.Observacion = observacion;
                    bien.Estado = false;
                    db.BienesAsegurados.Add(bien);
                    db.SaveChanges();

                    //Asigamos el ID para regresarlo
                    d = bien.Id;

                    //Deseralizamos los detalles
                    var detCa = JsonConvert.DeserializeObject<List<DetalleCaracteristica>>(detalleCaract);
                    var detCo = JsonConvert.DeserializeObject<List<DetalleCobertura>>(detalleCobert);

                    foreach (var item in detCa) {
                        item.BienAseguradoId = bien.Id;
                        db.DetalleCaracteristicas.Add(item);
                    } db.SaveChanges();

                    foreach (var item in detCo) {
                        item.BienAseguradoId = bien.Id;
                        db.DetalleCoberturas.Add(item);
                    } db.SaveChanges();

                    transact.Commit();
                } catch (Exception) {
                    transact.Rollback();
                }
            }
            return Json(new { d },JsonRequestBehavior.AllowGet);
        }
    }
}
