

























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
    public class ContactoIntermediariosController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /ContactoIntermediarios/

        public ActionResult Index()

        {



            return View(db.ContactoIntermediarios.ToList());


        }

        // GET: /ContactoIntermediarios/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ContactoIntermediario contactoIntermediario = db.ContactoIntermediarios.Find(id);

            if (contactoIntermediario == null)
            {
                return HttpNotFound();
            }
            return PartialView(contactoIntermediario);
        }

        // GET: /ContactoIntermediarios/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: /ContactoIntermediarios/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,Nombres,Apellidos,Telefono,Email,Cargo")] ContactoIntermediario contactoIntermediario)

        {
            if (ModelState.IsValid)
            {
                if (db.ContactoIntermediarios.FirstOrDefault(q => q.Nombres == contactoIntermediario.Nombres && q.Apellidos == contactoIntermediario.Apellidos) != null)
                {
                    ModelState.AddModelError("Nombres", "El intermediario que desea ingresar ya existe");
                    ModelState.AddModelError("Apellidos", "El intermediario que desea ingresar ya existe");
                }
                else
                {
                    db.ContactoIntermediarios.Add(contactoIntermediario);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }


            return View(contactoIntermediario);
        }

        // GET: /ContactoIntermediarios/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ContactoIntermediario contactoIntermediario = db.ContactoIntermediarios.Find(id);

            if (contactoIntermediario == null)
            {
                return HttpNotFound();
            }

            return View(contactoIntermediario);
        }

        // POST: /ContactoIntermediarios/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,Nombres,Apellidos,Telefono,Email,Cargo")] ContactoIntermediario contactoIntermediario)

        {
            if (ModelState.IsValid)
            {
                if (db.ContactoIntermediarios.FirstOrDefault(q => q.Nombres == contactoIntermediario.Nombres && q.Apellidos == contactoIntermediario.Apellidos && q.Id!=contactoIntermediario.Id) != null)
                {
                    ModelState.AddModelError("Nombres", "El intermediario que desea ingresar ya existe");
                    ModelState.AddModelError("Apellidos", "El intermediario que desea ingresar ya existe");
                }
                else
                {
                    db.Entry(contactoIntermediario).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(contactoIntermediario);
        }

        // GET: /ContactoIntermediarios/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ContactoIntermediario contactoIntermediario = db.ContactoIntermediarios.Find(id);

            if (contactoIntermediario == null)
            {
                return HttpNotFound();
            }
            return View(contactoIntermediario);
        }

        // POST: /ContactoIntermediarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            ContactoIntermediario contactoIntermediario = db.ContactoIntermediarios.Find(id);

            db.ContactoIntermediarios.Remove(contactoIntermediario);

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
