

























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
    public class DepartamentosController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /Departamentos/

        public ActionResult Index()

        {



            return View(db.Departamentos.ToList());


        }

        // GET: /Departamentos/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Departamento departamento = db.Departamentos.Find(id);

            if (departamento == null)
            {
                return HttpNotFound();
            }
            return PartialView(departamento);
        }

        // GET: /Departamentos/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: /Departamentos/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,Descripcion")] Departamento departamento)

        {
            if (ModelState.IsValid)
            {
                if (db.Departamentos.FirstOrDefault(a => a.Descripcion == departamento.Descripcion) != null)
                    ModelState.AddModelError("Descripcion", "El departamento que desea ingresar ya existe");
                else
                {
                    db.Departamentos.Add(departamento);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }


            return View(departamento);
        }

        // GET: /Departamentos/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Departamento departamento = db.Departamentos.Find(id);

            if (departamento == null)
            {
                return HttpNotFound();
            }

            return View(departamento);
        }

        // POST: /Departamentos/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,Descripcion")] Departamento departamento)

        {
            if (ModelState.IsValid)
            {
                if (db.Departamentos.FirstOrDefault(a => a.Descripcion == departamento.Descripcion && a.Id != departamento.Id) != null)
                    ModelState.AddModelError("Descripcion", "El departamento que desea ingresar ya existe");
                else
                {
                    db.Entry(departamento).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(departamento);
        }

        // GET: /Departamentos/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Departamento departamento = db.Departamentos.Find(id);

            if (departamento == null)
            {
                return HttpNotFound();
            }
            return View(departamento);
        }

        // POST: /Departamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            Departamento departamento = db.Departamentos.Find(id);

            db.Departamentos.Remove(departamento);

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
