

























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
using static Inspinia_MVC5_SeedProject.Notificaciones;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    [Authorize]
    public class ReclamosController : Controller
    {
        private SeguroBD db = new SeguroBD();
        private SeguridadUserBD dbs = new SeguridadUserBD();
        private static int ReclamoActual = 0;
        List<string> TiposReclamo = new List<string> { "Asegurado", "Perjudicado" };

        public ActionResult SetValue(int? id)
        {
            try {
                if ((int)id == 0)
                    id = db.Reclamos.Max(y => y.Id);
                ReclamoActual = (int)id;
            } catch (Exception) {
                id = 0;
            }
            
            return Json(new { id }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetValue2()
        {
            ReclamoActual = 0;
            return Json(new { ReclamoActual }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetValue()
        {
            return Json(new { ReclamoActual }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReclamo(int? id)
        {
            var x = db.Reclamos.ToList();
            var lista = from item in x
                        where item.Id == (int)id
                        select new
                        {
                            IdReclamo= item.Id,
                            NumerodeReclamo = item.NumReclamo,
                            TipodeReclamo = item.TipoReclamo,
                            Dependiente = item.Dependiente
                        };
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }


        // GET: /Reclamos/

        public ActionResult Index()

        {


            var reclamos = db.Reclamos.Include(r => r.BienAsegurado).Include(r => r.CoberturaReclamo).Include(r => r.Poliza);

            return View(reclamos.ToList());


        }

        // GET: /Reclamos/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reclamo reclamo = db.Reclamos.Find(id);

            if (reclamo == null)
            {
                return HttpNotFound();
            }

            ViewBag.Archivos = db.ArchivosReclamos.Where(x => x.ReclamoId == (int)id).ToList();

            return View(reclamo);
        }

        // GET: /Reclamos/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador,Reclamos")]
        public ActionResult Create()
        {
            ViewBag.TiposReclamo = new SelectList(TiposReclamo);

            ViewBag.DocumentoId = new SelectList(db.Documentos, "Id", "Descripcion");

            ViewBag.TipoPago = new SelectList(db.TipoDepagos, "Id", "Descripcion");

            ViewBag.BienAseguradoId = new SelectList(db.BienesAsegurados, "Id", "Observacion");

            ViewBag.CoberturaReclamoId = new SelectList(db.CoberturaReclamos, "Id", "Descripcion");

            ViewBag.PolizaId = new SelectList(db.Polizas, "Id", "NumPoliza");

            return View();
        }

        // POST: /Reclamos/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administración,Gerencia,Digitador,Reclamos")]
        public ActionResult Create([Bind(Include = "Id,NumReclamo,TipoReclamo,Dependiente,Ajustador,FechaAviso,FechaSiniestro,LugarOcurrencia,MontoReclamado,Responsable,PolizaId,BienAseguradoId,CoberturaReclamoId")] Reclamo reclamo, string detalleDocs, string detallePagos, int? PolizaId, int? BienAseguradoId)

        {
            int d = -3;
            var x = Request[""];
            var detDocs = JsonConvert.DeserializeObject<List<DetalleDocumento>>(detalleDocs);
            var detPagos = JsonConvert.DeserializeObject<List<DetallePago>>(detallePagos);

            if (ModelState.IsValid)
            {

                using (var transact = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (db.Reclamos.FirstOrDefault(p => p.NumReclamo == reclamo.NumReclamo) != null)
                            ModelState.AddModelError("NumReclamo", "El número de reclamo que desea ingresar ya existe");
                        else
                        {
                            reclamo.PolizaId = (int)PolizaId;
                            reclamo.BienAseguradoId = (int)BienAseguradoId;
                            db.Reclamos.Add(reclamo);
                            db.SaveChanges();

                            foreach (var item in detDocs)
                            {
                                item.ReclamoId = reclamo.Id;
                                db.DetalleDocumentos.Add(item);
                            }
                            db.SaveChanges();

                            foreach (var item in detPagos)
                            {
                                item.ReclamoId = reclamo.Id;
                                db.DetallePagos.Add(item);
                            }
                            d= db.SaveChanges();
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

            }

            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult getReclamo()
        {
            var list = from item in db.Reclamos
                       select new
                       {
                           IdReclamo = item.Id,
                           NumerodeReclamo = item.NumReclamo,
                           TipodeReclamo = item.TipoReclamo,
                           Dependiente= item.Dependiente
                       };
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecuperarReclamos(int? id)
        {
            var r = db.Reclamos.ToList();
            var lista = from item in r
                        where item.PolizaId == (int)id
                        select new
                        {
                            IdReclamo= item.Id,
                            NumReclamo = item.NumReclamo,
                            Tipo = item.TipoReclamo,
                            Fecha = item.FechaSiniestro.ToString("yyyy-MM-dd"),
                            Monto = item.MontoReclamado
                        };
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------ARCHIVO------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult RecuperarArchivos(int? id)
        {
            var list = from item in db.ArchivosReclamos
                       where item.ReclamoId == (int)id
                       select new
                       {
                           Id = item.Id,
                           Foto = item.Foto
                       };

            return Json(new { list }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador,Reclamos")]
        public ActionResult EliminarArchivo(int? id)
        {
            //var ar = db.ArchivosPolizas.Where(x => x.Id == (int)id);
            //ArchivosPólizas archi = new ArchivosPólizas();
            ArchivosReclamos arreclamo = db.ArchivosReclamos.Find(id);
            string url = Path.Combine(Server.MapPath(arreclamo.Foto));
            System.IO.File.Delete(url);
            db.ArchivosReclamos.Remove(arreclamo);
            int d = db.SaveChanges();
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [Authorize(Roles = "Administración,Gerencia,Digitador,Reclamos")]
        public ActionResult GuardarARExtra(int? id, HttpPostedFileBase fileUpload)
        {
            string path = Server.MapPath("~/Content/Imagen");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var file = Request.Files[0];
            var fileName = Path.GetFileName(file.FileName);
            path = Path.Combine(Server.MapPath("~/Content/Imagen"), fileName);
            file.SaveAs(path);
            string url = Path.Combine("/Content/Imagen", fileName);
            ArchivosReclamos obj = new ArchivosReclamos();
            obj.Foto = url;
            obj.ReclamoId = (int)id;
            db.ArchivosReclamos.Add(obj);
            int d = db.SaveChanges();
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------ARCHIVO------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------------

        // GET: /Reclamos/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador,Reclamos")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reclamo reclamo = db.Reclamos.Find(id);

            if (reclamo == null)
            {
                return HttpNotFound();
            }

            ViewBag.DocumentoId = new SelectList(db.Documentos, "Id", "Descripcion");

            ViewBag.TipoPago = new SelectList(db.TipoDepagos, "Id", "Descripcion");

            ViewBag.TiposReclamo = new SelectList(TiposReclamo, reclamo.TipoReclamo);

            ViewBag.BienAseguradoId = new SelectList(db.BienesAsegurados, "Id", "Observacion", reclamo.BienAseguradoId);

            ViewBag.CoberturaReclamoId = new SelectList(db.CoberturaReclamos, "Id", "Descripcion", reclamo.CoberturaReclamoId);

            ViewBag.PolizaId = new SelectList(db.Polizas, "Id", "NumPoliza", reclamo.PolizaId);

            return View(reclamo);
        }

        // POST: /Reclamos/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,NumReclamo,TipoReclamo,Dependiente,Ajustador,FechaAviso,FechaSiniestro,LugarOcurrencia,MontoReclamado,Responsable,BienAseguradoId,PolizaId,CoberturaReclamoId")] Reclamo reclamo)

        {
            if (ModelState.IsValid)
            {
                db.Entry(reclamo).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.TiposReclamo = new SelectList(TiposReclamo, reclamo.TipoReclamo);

            ViewBag.DocumentoId = new SelectList(db.Documentos, "Id", "Descripcion");

            ViewBag.TipoPago = new SelectList(db.TipoDepagos, "Id", "Descripcion");

            ViewBag.BienAseguradoId = new SelectList(db.BienesAsegurados, "Id", "Observacion", reclamo.BienAseguradoId);

            ViewBag.CoberturaReclamoId = new SelectList(db.CoberturaReclamos, "Id", "Descripcion", reclamo.CoberturaReclamoId);

            ViewBag.PolizaId = new SelectList(db.Polizas, "Id", "NumPoliza", reclamo.PolizaId);

            return View(reclamo);
        }

        // GET: /Reclamos/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador,Reclamos")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reclamo reclamo = db.Reclamos.Find(id);

            if (reclamo == null)
            {
                return HttpNotFound();
            }
            return View(reclamo);
        }

        // POST: /Reclamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            Reclamo reclamo = db.Reclamos.Find(id);

            db.Reclamos.Remove(reclamo);

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

        [HttpPost]
        public JsonResult SearchBienes(int Id)
        {
            List<BienAsegurado> data1 = db.BienesAsegurados.ToList();
            List<int> data2 = db.DetalleBienesAsegurados.Where(x => x.PolizaId == Id).Select(y => y.BienAseguradoId).ToList();
            List<BienAsegurado> nuevo = new List<BienAsegurado>();
            foreach (var item in data1)
            {
                if (data2.Contains(item.Id))
                    nuevo.Add(item);
            }
            var list = from item in nuevo
                       select new
                       {
                           Id = item.Id,
                           Num = item.NumCertificado,
                           Observacion = item.Observacion
                       };
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RecuperarDocumentos(int id)
        {
            var a = db.DetalleDocumentos.Where(x => x.ReclamoId == id).ToList();
            var list = from item in a
                       select new
                       {
                           IdDetalle = item.Id,
                           Doc = item.Documento.Descripcion,
                           Fecha = item.Fecha.ToString("yyyy-MM-dd"),
                           Emisor = item.Emisor,
                           Num = item.Numero,
                           Valor = item.Valor,
                           Comentarios = item.Comentarios
                       };

            return Json(new { list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RecuperarPagos(int id)
        {
            var x = db.DetallePagos.Where(y => y.ReclamoId == id).ToList();
            var list = from item in x
                       select new
                       {
                           IdDetalle = item.Id,
                           Tipo = item.TipoDePago.Descripcion,
                           Fecha = item.Fecha.ToString("yyyy-MM-dd"),
                           Moneda = item.Moneda,
                           Num = item.NumDoc,
                           Valor = item.Valor,
                           Banc = item.BancoTaller,
                           Notas = item.Nota
                       };
            return Json(new { list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Generar()
        {
            int d = -3;
            try
            {
                ReclamosTemp t = new ReclamosTemp();
                //t.NombreCliente = "Alina María Ortiz";
                t.Fecha = DateTime.Today.ToString("dd/MM/yyyy");
                t.Lugar = "Managua, Nicaragua";
                t.Comentarios = "Ejecutando prueba";
                t.UsuarioId = 1;
                t.Tipo = "1";
                t.Visto = false;
                dbs.ReclamosTemps.Add(t);
                d = dbs.SaveChanges();
                AddNotification.AppendNotify(t);
            }
            catch (Exception)
            {
                d = -1;
            }
            return Json(new { d },JsonRequestBehavior.AllowGet);
        }

        public ActionResult ContarNoVistos()
        {
            var x = dbs.ReclamosTemps.Where(r => !r.Visto).ToList();
            var y = (from item in x
                    select new
                    {
                        NombreCliente = item.Usuario.Nombre,
                        Lugar = item.Lugar,
                        Comentarios = item.Comentarios,
                        Tipo = item.Tipo
                    }).ToList();
            return Json(new { y }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CambiarEstado(int i)
        {
            var d = dbs.ReclamosTemps.Where(s=> s.Id == i).ToList();
            d[0].Visto = true;
            dbs.Entry(d[0]).State = EntityState.Modified;
            var w = dbs.SaveChanges();
            var t = (from item in d
                    select new
                    {
                        Id = item.Usuario.IdNUser,
                        Nombre = item.Usuario.Nombre + " " + item.Usuario.Apellido,
                        Fecha = item.Fecha,
                        Lugar = item.Lugar,
                        Comentarios = item.Comentarios,
                        Tipo = item.Tipo
                    }).ToList();
            return Json(new { t }, JsonRequestBehavior.AllowGet);
        }
        
    }
}
