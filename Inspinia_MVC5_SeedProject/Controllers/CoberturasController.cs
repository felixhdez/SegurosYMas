

























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
    public class CoberturasController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /Coberturas/

        public ActionResult Index()

        {



            return View(db.Coberturas.ToList());


        }

        // GET: /Coberturas/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cobertura cobertura = db.Coberturas.Find(id);

            if (cobertura == null)
            {
                return HttpNotFound();
            }
            return PartialView(cobertura);
        }

        // GET: /Coberturas/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: /Coberturas/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,Descripcion")] Cobertura cobertura)

        {
            if (ModelState.IsValid)
            {
                if (db.Coberturas.FirstOrDefault(s => s.Descripcion == cobertura.Descripcion) != null)
                    ModelState.AddModelError("Descripcion", "La cobertura que desea ingresar ya existe");
                else
                {
                    db.Coberturas.Add(cobertura);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }


            return View(cobertura);
        }

        // GET: /Coberturas/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cobertura cobertura = db.Coberturas.Find(id);

            if (cobertura == null)
            {
                return HttpNotFound();
            }

            return View(cobertura);
        }

        // POST: /Coberturas/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,Descripcion")] Cobertura cobertura)

        {
            if (ModelState.IsValid)
            {
                if (db.Coberturas.FirstOrDefault(s => s.Descripcion == cobertura.Descripcion && s.Id != cobertura.Id) != null)
                    ModelState.AddModelError("Descripcion", "La cobertura que desea ingresar ya existe");
                else
                {
                    db.Entry(cobertura).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(cobertura);
        }

        // GET: /Coberturas/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cobertura cobertura = db.Coberturas.Find(id);

            if (cobertura == null)
            {
                return HttpNotFound();
            }
            return View(cobertura);
        }

        // POST: /Coberturas/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {
            int d = -3;
            try
            {
                DetalleCobertura cobertura = db.DetalleCoberturas.Find(id);

                db.DetalleCoberturas.Remove(cobertura);

                d = db.SaveChanges();
            }
            catch (Exception)
            {
                d = -1;
            }

            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ListarCoberturas()
        {
            var listado = from item in db.Coberturas
                          select new
                          {
                              IdCob = item.Id,
                              Descripcion = item.Descripcion
                          };
            return Json(new { list = listado }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetalleCoberturas(int? id)
        {
            var listado = from item in db.DetalleCoberturas
                          where item.Id == id
                          select new
                          {
                              IdDetalle = item.Id,
                              IdCob = item.CoberturaId,
                              Suma = item.SumaAsegurada,
                              Deducible = item.Deducible,
                              Prima = item.Prima
                          };
            return Json(new { listado }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult GuardarCambios(int? idDetalle, int? idCobertura, string suma, string deducible, string prima)
        {
            DetalleCobertura obj = db.DetalleCoberturas.Find(idDetalle);
            obj.CoberturaId = (int)idCobertura;
            obj.SumaAsegurada = double.Parse(suma);
            obj.Deducible = double.Parse(deducible);
            obj.Prima = double.Parse(prima);
            db.Entry(obj).State = EntityState.Modified;
            int a = db.SaveChanges();
            return Json(new { a }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult AddNuevo(int? idBienAsegurado, int? idCobertura, string suma, string deduc, string prima)
        {
            DetalleCobertura nuevo = new DetalleCobertura();
            nuevo.BienAseguradoId = (int)idBienAsegurado;
            nuevo.CoberturaId = (int)idCobertura;
            nuevo.SumaAsegurada = double.Parse(suma);
            nuevo.Deducible = double.Parse(deduc);
            nuevo.Prima = double.Parse(prima);
            db.DetalleCoberturas.Add(nuevo);
            int x = db.SaveChanges();
            return Json(new { x }, JsonRequestBehavior.AllowGet);
        }
    }
}
