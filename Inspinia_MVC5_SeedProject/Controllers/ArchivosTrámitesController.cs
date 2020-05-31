

























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
    public class ArchivosTrámitesController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /ArchivosTrámites/

        public ActionResult Index()

        {


            var archivostramites = db.ArchivosTramites.Include(a => a.Tramite);

            return View(archivostramites.ToList());


        }

        // GET: /ArchivosTrámites/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ArchivosTrámites archivosTrámites = db.ArchivosTramites.Find(id);

            if (archivosTrámites == null)
            {
                return HttpNotFound();
            }
            return View(archivosTrámites);
        }

        // GET: /ArchivosTrámites/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            ViewBag.TramiteId = new SelectList(db.Tramites, "Id", "Tipo");

            return View();
        }

        // POST: /ArchivosTrámites/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create([Bind(Include="Id,Foto,TramiteId")] ArchivosTrámites archivosTrámites)

        {
            if (ModelState.IsValid)
            {

                db.ArchivosTramites.Add(archivosTrámites);

                db.SaveChanges();

                return RedirectToAction("Index");
            }


            ViewBag.TramiteId = new SelectList(db.Tramites, "Id", "Tipo", archivosTrámites.TramiteId);

            return View(archivosTrámites);
        }


        [HttpPost]
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult CrearArTramite(HttpPostedFileBase files)
        {

            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)//verifica que hay archivos, si no hay, no pasa por la codicion de archivos para ser guardado
                {
                    if (Request.Files?.Count > 0)
                {
                    string path = Server.MapPath("~/Content/Imagen");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var filesCount = Request.Files.Count;
                        for (int i = 0; i < filesCount; i++)
                        {
                            var file = Request.Files[i];
                            var fileName = Path.GetFileName(file.FileName);
                            path = Path.Combine(Server.MapPath("~/Content/Imagen"), fileName);

                            file.SaveAs(path);
                            string url = Path.Combine("/Content/Imagen", fileName);
                            ArchivosTrámites obj = new ArchivosTrámites();
                            obj.Foto = url;
                            obj.TramiteId = int.Parse(Request["TramitesId"]);
                            db.ArchivosTramites.Add(obj);
                            db.SaveChanges();
                        }
                    }
                    //}

                    //Bitacora b = new Bitacora();
                    //b.Observacion = Request["Observacion"];
                    //b.Fecha = DateTime.Parse(Request["Fecha"]);
                    //b.PolizaId = int.Parse(Request["PolizaId"]);

                    //db.Bitacoras.Add(b);
                    //db.SaveChanges();


                }


            }

            return RedirectToAction("Index", "Tramites", null);
        }

        // GET: /ArchivosTrámites/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ArchivosTrámites archivosTrámites = db.ArchivosTramites.Find(id);

            if (archivosTrámites == null)
            {
                return HttpNotFound();
            }

            ViewBag.TramiteId = new SelectList(db.Tramites, "Id", "Tipo", archivosTrámites.TramiteId);

            return View(archivosTrámites);
        }

        // POST: /ArchivosTrámites/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit([Bind(Include="Id,Foto,TramiteId")] ArchivosTrámites archivosTrámites)

        {
            if (ModelState.IsValid)
            {
                db.Entry(archivosTrámites).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.TramiteId = new SelectList(db.Tramites, "Id", "Tipo", archivosTrámites.TramiteId);

            return View(archivosTrámites);
        }

        // GET: /ArchivosTrámites/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ArchivosTrámites archivosTrámites = db.ArchivosTramites.Find(id);

            if (archivosTrámites == null)
            {
                return HttpNotFound();
            }
            return View(archivosTrámites);
        }

        // POST: /ArchivosTrámites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            ArchivosTrámites archivosTrámites = db.ArchivosTramites.Find(id);

            db.ArchivosTramites.Remove(archivosTrámites);

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
