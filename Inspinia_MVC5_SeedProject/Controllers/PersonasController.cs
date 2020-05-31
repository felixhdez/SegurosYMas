

























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
    public class PersonasController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /Personas/

        public ActionResult Index()

        {


            var personas = db.Personas.Include(p => p.Departamento).Where(y=> !db.Clientes.Select(x => x.Id).Contains(y.Id));

            return View(personas.ToList());


        }

        // GET: /Personas/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Persona persona = db.Personas.Find(id);

            if (persona == null)
            {
                return HttpNotFound();
            }
            return PartialView(persona);
        }

        // GET: /Personas/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            ViewBag.DepartamentoId = new SelectList(db.Departamentos, "Id", "Descripcion");

            return View();
        }

        // POST: /Personas/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,Apellidos,Nombres,Identificacion,Direccion,NumTelf1,NumTelf2,NumTelf3,Celular,Email,Notas,DepartamentoId")] Persona persona)

        {
            if (ModelState.IsValid)
            {
                var lista = db.Personas.Where(y => !db.Clientes.Select(x => x.Id).Contains(y.Id));
                if (lista.FirstOrDefault(r => r.Apellidos == persona.Apellidos && r.Nombres == persona.Nombres && r.Identificacion == persona.Identificacion) != null)
                {
                    ModelState.AddModelError("Nombres", "El contratante que desea ingresar ya existe");
                    ModelState.AddModelError("Apellidos", "El contratante que desea ingresar ya existe");
                    ModelState.AddModelError("Identificacion", "El contratante que desea ingresar ya existe");
                }
                else
                {
                    db.Personas.Add(persona);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }


            ViewBag.DepartamentoId = new SelectList(db.Departamentos, "Id", "Descripcion", persona.DepartamentoId);

            return View(persona);
        }

        // GET: /Personas/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Persona persona = db.Personas.Find(id);

            if (persona == null)
            {
                return HttpNotFound();
            }

            ViewBag.DepartamentoId = new SelectList(db.Departamentos, "Id", "Descripcion", persona.DepartamentoId);

            return View(persona);
        }

        // POST: /Personas/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,Apellidos,Nombres,Identificacion,Direccion,NumTelf1,NumTelf2,NumTelf3,Celular,Email,Notas,DepartamentoId")] Persona persona)

        {
            if (ModelState.IsValid)
            {
                var lista = db.Personas.Where(y => !db.Clientes.Select(x => x.Id).Contains(y.Id));
                if (lista.FirstOrDefault(r => r.Apellidos == persona.Apellidos && r.Nombres == persona.Nombres && r.Identificacion == persona.Identificacion && r.Id!= persona.Id) != null)
                {
                    ModelState.AddModelError("Nombres", "El contratante que desea ingresar ya existe");
                    ModelState.AddModelError("Apellidos", "El contratante que desea ingresar ya existe");
                    ModelState.AddModelError("Identificacion", "El contratante que desea ingresar ya existe");
                }
                else
                {
                    db.Entry(persona).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            ViewBag.DepartamentoId = new SelectList(db.Departamentos, "Id", "Descripcion", persona.DepartamentoId);

            return View(persona);
        }

        // GET: /Personas/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Persona persona = db.Personas.Find(id);

            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: /Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            Persona persona = db.Personas.Find(id);

            db.Personas.Remove(persona);

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
