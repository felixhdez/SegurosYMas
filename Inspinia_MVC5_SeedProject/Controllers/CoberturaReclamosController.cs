

























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
    public class CoberturaReclamosController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /CoberturaReclamos/

        public ActionResult Index()

        {



            return View(db.CoberturaReclamos.ToList());


        }

        // GET: /CoberturaReclamos/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CoberturaReclamo coberturaReclamo = db.CoberturaReclamos.Find(id);

            if (coberturaReclamo == null)
            {
                return HttpNotFound();
            }
            return PartialView(coberturaReclamo);
        }

        // GET: /CoberturaReclamos/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: /CoberturaReclamos/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,Descripcion")] CoberturaReclamo coberturaReclamo)

        {
            if (ModelState.IsValid)
            {
                if (db.CoberturaReclamos.FirstOrDefault(s => s.Descripcion == coberturaReclamo.Descripcion) != null)
                    ModelState.AddModelError("Descripcion", "La cobertura que desea ingresar ya existe");
                else
                {
                    db.CoberturaReclamos.Add(coberturaReclamo);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }


            return View(coberturaReclamo);
        }

        // GET: /CoberturaReclamos/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CoberturaReclamo coberturaReclamo = db.CoberturaReclamos.Find(id);

            if (coberturaReclamo == null)
            {
                return HttpNotFound();
            }

            return View(coberturaReclamo);
        }

        // POST: /CoberturaReclamos/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,Descripcion")] CoberturaReclamo coberturaReclamo)

        {
            if (ModelState.IsValid)
            {
                if (db.CoberturaReclamos.FirstOrDefault(s => s.Descripcion == coberturaReclamo.Descripcion && s.Id !=coberturaReclamo.Id) != null)
                    ModelState.AddModelError("Descripcion", "La cobertura que desea ingresar ya existe");
                else
                {
                    db.Entry(coberturaReclamo).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(coberturaReclamo);
        }

        // GET: /CoberturaReclamos/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CoberturaReclamo coberturaReclamo = db.CoberturaReclamos.Find(id);

            if (coberturaReclamo == null)
            {
                return HttpNotFound();
            }
            return View(coberturaReclamo);
        }

        // POST: /CoberturaReclamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            CoberturaReclamo coberturaReclamo = db.CoberturaReclamos.Find(id);

            db.CoberturaReclamos.Remove(coberturaReclamo);

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
