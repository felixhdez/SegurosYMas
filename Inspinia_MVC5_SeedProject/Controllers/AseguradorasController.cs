

























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
    public class AseguradorasController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /Aseguradoras/

        public ActionResult Index()

        {



            return View(db.Aseguradoras.ToList());


        }

        // GET: /Aseguradoras/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Aseguradora aseguradora = db.Aseguradoras.Find(id);

            if (aseguradora == null)
            {
                return HttpNotFound();
            }
            return PartialView(aseguradora);
        }

        // GET: /Aseguradoras/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: /Aseguradoras/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,Descripcion,Direccion,Telefono1,Telefono2,Asistencia,IR,IMI")] Aseguradora aseguradora)

        {
            if (ModelState.IsValid)
            {
                if (db.Aseguradoras.FirstOrDefault(x => x.Descripcion == aseguradora.Descripcion) != null)
                    ModelState.AddModelError("Descripcion", "La aseguradora que desea ingresar ya existe.");
                else
                {

                    db.Aseguradoras.Add(aseguradora);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }


            return View(aseguradora);
        }

        // GET: /Aseguradoras/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Aseguradora aseguradora = db.Aseguradoras.Find(id);

            if (aseguradora == null)
            {
                return HttpNotFound();
            }

            return View(aseguradora);
        }

        // POST: /Aseguradoras/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,Descripcion,Direccion,Telefono1,Telefono2,Asistencia,IR,IMI")] Aseguradora aseguradora)

        {
            if (ModelState.IsValid)
            {
                if (db.Aseguradoras.FirstOrDefault(x => x.Descripcion == aseguradora.Descripcion && x.Id != aseguradora.Id) != null)
                    ModelState.AddModelError("Descripcion", "La aseguradora que desea ingresar ya existe.");
                else
                {
                    db.Entry(aseguradora).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(aseguradora);
        }

        // GET: /Aseguradoras/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Aseguradora aseguradora = db.Aseguradoras.Find(id);

            if (aseguradora == null)
            {
                return HttpNotFound();
            }
            return View(aseguradora);
        }

        // POST: /Aseguradoras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            Aseguradora aseguradora = db.Aseguradoras.Find(id);

            db.Aseguradoras.Remove(aseguradora);

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
