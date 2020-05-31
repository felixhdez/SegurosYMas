

























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
    public class AgentesController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /Agentes/

        public ActionResult Index()

        {



            return View(db.Agentes.ToList());


        }

        // GET: /Agentes/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Agente agente = db.Agentes.Find(id);

            if (agente == null)
            {
                return HttpNotFound();
            }
            return PartialView(agente);
        }

        // GET: /Agentes/Create
        [Authorize(Roles="Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: /Agentes/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,NombreCompleto")] Agente agente)

        {
            if (ModelState.IsValid)
            {
                if (db.Agentes.FirstOrDefault(x => x.NombreCompleto == agente.NombreCompleto) != null)
                    ModelState.AddModelError("NombreCompleto", "El agente que desea ingresar ya existe.");
                else
                {
                    db.Agentes.Add(agente);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(agente);
        }

        // GET: /Agentes/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Agente agente = db.Agentes.Find(id);

            if (agente == null)
            {
                return HttpNotFound();
            }

            return View(agente);
        }

        // POST: /Agentes/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,NombreCompleto")] Agente agente)
        {
            if (ModelState.IsValid)
            {
                if (db.Agentes.FirstOrDefault(z => z.NombreCompleto == agente.NombreCompleto && agente.Id != z.Id) != null)
                    ModelState.AddModelError("NombreCompleto", "El agente que desea ingresar ya existe");
                else
                {
                    db.Entry(agente).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(agente);
        }

        // GET: /Agentes/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Agente agente = db.Agentes.Find(id);

            if (agente == null)
            {
                return HttpNotFound();
            }
            return View(agente);
        }

        // POST: /Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            Agente agente = db.Agentes.Find(id);

            db.Agentes.Remove(agente);

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
