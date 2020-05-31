

























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
    public class ArchivosReclamosController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /ArchivosReclamos/

        public ActionResult Index()

        {


            var archivosreclamos = db.ArchivosReclamos.Include(a => a.Reclamo);

            return View(archivosreclamos.ToList());


        }

        // GET: /ArchivosReclamos/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ArchivosReclamos archivosReclamos = db.ArchivosReclamos.Find(id);

            if (archivosReclamos == null)
            {
                return HttpNotFound();
            }
            return View(archivosReclamos);
        }

        // GET: /ArchivosReclamos/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador,Reclamos")]
        public ActionResult Create()
        {

            ViewBag.ReclamoId = new SelectList(db.Reclamos, "Id", "NumReclamo");

            return View();
        }

        // POST: /ArchivosReclamos/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administración,Gerencia,Digitador,Reclamos")]
        public ActionResult Create([Bind(Include="Id,Foto,ReclamoId")] ArchivosReclamos archivosReclamos)

        {
            if (ModelState.IsValid)
            {

                db.ArchivosReclamos.Add(archivosReclamos);

                db.SaveChanges();

                return RedirectToAction("Index");
            }


            ViewBag.ReclamoId = new SelectList(db.Reclamos, "Id", "NumReclamo", archivosReclamos.ReclamoId);

            return View(archivosReclamos);
        }



        [HttpPost]
        [Authorize(Roles = "Administración,Gerencia,Digitador,Reclamos")]
        public ActionResult CrearArReclamo(HttpPostedFileBase files)
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
                            ArchivosReclamos obj = new ArchivosReclamos();
                            obj.Foto = url;
                            obj.ReclamoId = int.Parse(Request["ReclamoId"]);
                            db.ArchivosReclamos.Add(obj);
                            db.SaveChanges();
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


            }

            return RedirectToAction("Index", "Reclamos", null);
        }

        // GET: /ArchivosReclamos/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador,Reclamos")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ArchivosReclamos archivosReclamos = db.ArchivosReclamos.Find(id);

            if (archivosReclamos == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReclamoId = new SelectList(db.Reclamos, "Id", "NumReclamo", archivosReclamos.ReclamoId);

            return View(archivosReclamos);
        }

        // POST: /ArchivosReclamos/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administración,Gerencia,Digitador,Reclamos")]
        public ActionResult Edit([Bind(Include="Id,Foto,ReclamoId")] ArchivosReclamos archivosReclamos)

        {
            if (ModelState.IsValid)
            {
                db.Entry(archivosReclamos).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.ReclamoId = new SelectList(db.Reclamos, "Id", "NumReclamo", archivosReclamos.ReclamoId);

            return View(archivosReclamos);
        }

        // GET: /ArchivosReclamos/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador,Reclamos")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ArchivosReclamos archivosReclamos = db.ArchivosReclamos.Find(id);

            if (archivosReclamos == null)
            {
                return HttpNotFound();
            }
            return View(archivosReclamos);
        }

        // POST: /ArchivosReclamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            ArchivosReclamos archivosReclamos = db.ArchivosReclamos.Find(id);

            db.ArchivosReclamos.Remove(archivosReclamos);

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
