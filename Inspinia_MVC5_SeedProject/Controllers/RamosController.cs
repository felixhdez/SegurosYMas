

























using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using System.Net;
using System.Web;
using System.Web.Mvc;

using Inspinia_MVC5_SeedProject.Models;


namespace Inspinia_MVC5_SeedProject.Controllers
{
    [Authorize]
    public class RamosController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /Ramos/

        public ActionResult Index()

        {



            return View(db.Ramos.ToList());


        }

        // GET: /Ramos/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ramo ramo = db.Ramos.Find(id);

            if (ramo == null)
            {
                return HttpNotFound();
            }
            return PartialView(ramo);
        }

        // GET: /Ramos/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: /Ramos/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,Descripcion")] Ramo ramo)

        {
            if (ModelState.IsValid)
            {
                if (db.Ramos.FirstOrDefault(d => d.Descripcion == ramo.Descripcion) != null)
                    ModelState.AddModelError("Descripcion", "El ramo que desea ingresar ya existe");
                else
                {
                    db.Ramos.Add(ramo);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }


            return View(ramo);
        }

        // GET: /Ramos/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ramo ramo = db.Ramos.Find(id);

            if (ramo == null)
            {
                return HttpNotFound();
            }

            return View(ramo);
        }

        // POST: /Ramos/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,Descripcion")] Ramo ramo)

        {
            if (ModelState.IsValid)
            {
                if (db.Ramos.FirstOrDefault(d => d.Descripcion == ramo.Descripcion && ramo.Id!=d.Id) != null)
                    ModelState.AddModelError("Descripcion", "El ramo que desea ingresar ya existe");
                else
                {
                    db.Entry(ramo).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(ramo);
        }

        // GET: /Ramos/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ramo ramo = db.Ramos.Find(id);

            if (ramo == null)
            {
                return HttpNotFound();
            }
            return View(ramo);
        }

        // POST: /Ramos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            Ramo ramo = db.Ramos.Find(id);

            db.Ramos.Remove(ramo);

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
