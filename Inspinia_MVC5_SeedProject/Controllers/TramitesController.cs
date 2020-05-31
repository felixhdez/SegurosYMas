

























using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using System.Net;
using System.Web;
using System.Web.Mvc;

using Inspinia_MVC5_SeedProject.Models;
using System.IO;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    [Authorize]
    public class TramitesController : Controller
    {
        private SeguroBD db = new SeguroBD();
        private static int TramiteActual = 0;
        List<string> Tipos = new List<string> { "Liquidación", "Modificación", "Nueva", "Renovación", "Para firma" };

        public ActionResult SetValue(int? id)
        {
            try {
                if ((int)id == 0)
                    id = db.Tramites.Max(y => y.Id);
                TramiteActual = (int)id;
            } catch (Exception) {
                id = 0;
            }
            
            return Json(new { id }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetValue2()
        {
            TramiteActual = 0;
            return Json(new { TramiteActual }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetValue()
        {
            return Json(new { TramiteActual }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTramite(int? id)
        {
            var x = db.Tramites.ToList();
            var lista = from item in x
                        where item.Id == (int)id
                        select new
                        {
                            IdTramites = item.Id,
                            TipodeTramites = item.Tipo,
                            Modalidad = item.Modalidad,
                            Descripcion = item.Descripcion
                        };
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Tramites/

        public ActionResult Index()

        {


            var tramites = db.Tramites.Include(t => t.Poliza);

            return View(tramites.ToList());


        }

        // GET: /Tramites/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tramite tramite = db.Tramites.Find(id);

            if (tramite == null)
            {
                return HttpNotFound();
            }

            int d = tramite.Finalizacion ? 1 : 0;
            ViewBag.Fin = d;
            ViewBag.Archivos = db.ArchivosTramites.Where(x => x.TramiteId == (int)id).ToList();

            return View(tramite);
        }

        // GET: /Tramites/Create
        public ActionResult Create()
        {
            ViewBag.Tipos = new SelectList(Tipos);
            ViewBag.PolizaId = new SelectList(db.Polizas, "Id", "NumPoliza");

            return View();
        }

        // POST: /Tramites/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "Id,Tipo,Modalidad,Descripcion,FechaRecepcion,NombreEjecutivo,FechaEnvio,FechaRecibido,RecibidoPor,Finalizacion,PolizaId")] Tramite tramite, int? IdPoliza)

        {
            int d = -3;
            if (ModelState.IsValid)
            {

                tramite.PolizaId = (int)IdPoliza;
                db.Tramites.Add(tramite);

                d = db.SaveChanges();
            }

            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------ARCHIVO------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult RecuperarArchivos(int? id)
        {
            var list = from item in db.ArchivosTramites
                       where item.TramiteId == (int)id
                       select new
                       {
                           Id = item.Id,
                           Foto = item.Foto
                       };

            return Json(new { list }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EliminarArchivo(int? id)
        {
            //var ar = db.ArchivosPolizas.Where(x => x.Id == (int)id);
            //ArchivosPólizas archi = new ArchivosPólizas();
            ArchivosTrámites arreclamo = db.ArchivosTramites.Find(id);
            string url = Path.Combine(Server.MapPath(arreclamo.Foto));
            System.IO.File.Delete(url);
            db.ArchivosTramites.Remove(arreclamo);
            int d = db.SaveChanges();
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
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
            ArchivosTrámites obj = new ArchivosTrámites();
            obj.Foto = url;
            obj.TramiteId = (int)id;
            db.ArchivosTramites.Add(obj);
            int d = db.SaveChanges();
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------ARCHIVO------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------------


        // GET: /Tramites/Edit/5

        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tramite tramite = db.Tramites.Find(id);

            if (tramite == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tipos = new SelectList(Tipos, tramite.Tipo);
            ViewBag.PolizaId = new SelectList(db.Polizas, "Id", "NumPoliza", tramite.PolizaId);

            return View(tramite);
        }

        [HttpPost]
        public JsonResult getTramites()
        {
            var list = from item in db.Tramites
                       select new
                       {
                           IdTramites = item.Id,
                           TipodeTramites = item.Tipo,
                           Modalidad = item.Modalidad,
                           Descripcion = item.Descripcion
                       };
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecuperarTramites(int? id)
        {
            var x = db.Tramites.ToList();
            var lista = from item in x
                       where item.PolizaId == (int)id
                       select new
                       {
                           IdTramite = item.Id,
                           Tipo = item.Tipo,
                           Modalidad = item.Modalidad,
                           Fecha = item.FechaEnvio.ToString("yyy-MM-dd"),
                           Recibido = item.RecibidoPor
                       };
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        // POST: /Tramites/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,Tipo,Modalidad,Descripcion,FechaRecepcion,NombreEjecutivo,FechaEnvio,FechaRecibido,RecibidoPor,Finalizacion,PolizaId")] Tramite tramite)

        {
            if (ModelState.IsValid)
            {
                db.Entry(tramite).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.Tipos = new SelectList(Tipos, tramite.Tipo);
            ViewBag.PolizaId = new SelectList(db.Polizas, "Id", "NumPoliza", tramite.PolizaId);

            return View(tramite);
        }

        // GET: /Tramites/Delete/5

        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tramite tramite = db.Tramites.Find(id);

            if (tramite == null)
            {
                return HttpNotFound();
            }
            return View(tramite);
        }

        // POST: /Tramites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            Tramite tramite = db.Tramites.Find(id);

            db.Tramites.Remove(tramite);

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
    }
}
