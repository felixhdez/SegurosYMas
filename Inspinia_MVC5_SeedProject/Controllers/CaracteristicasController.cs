

























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
    public class CaracteristicasController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /Caracteristicas/

        public ActionResult Index()

        {



            return View(db.Caracteristicass.ToList());


        }

        // GET: /Caracteristicas/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Caracteristica caracteristica = db.Caracteristicass.Find(id);

            if (caracteristica == null)
            {
                return HttpNotFound();
            }
            return PartialView(caracteristica);
        }

        // GET: /Caracteristicas/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: /Caracteristicas/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,Descripcion")] Caracteristica caracteristica)

        {
            if (ModelState.IsValid)
            {
                if (db.Caracteristicass.FirstOrDefault(z => z.Descripcion == caracteristica.Descripcion) != null)
                    ModelState.AddModelError("Descripcion", "La característica que desea ingresar ya existe");
                else
                {
                    db.Caracteristicass.Add(caracteristica);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }


            return View(caracteristica);
        }

        // GET: /Caracteristicas/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Caracteristica caracteristica = db.Caracteristicass.Find(id);

            if (caracteristica == null)
            {
                return HttpNotFound();
            }

            return View(caracteristica);
        }

        // POST: /Caracteristicas/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,Descripcion")] Caracteristica caracteristica)

        {
            if (ModelState.IsValid)
            {
                if (db.Caracteristicass.FirstOrDefault(z => z.Descripcion == caracteristica.Descripcion && z.Id !=caracteristica.Id) != null)
                    ModelState.AddModelError("Descripcion", "La caracteristica que desea ingresar ya existe");
                else
                {
                    db.Entry(caracteristica).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(caracteristica);
        }

        // GET: /Caracteristicas/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Caracteristica caracteristica = db.Caracteristicass.Find(id);

            if (caracteristica == null)
            {
                return HttpNotFound();
            }
            return View(caracteristica);
        }

        // POST: /Caracteristicas/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {
            int d = -3;
            try
            {
                DetalleCaracteristica caracteristica = db.DetalleCaracteristicas.Find(id);

                db.DetalleCaracteristicas.Remove(caracteristica);

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
        
        public ActionResult ListarCaracteristicas()
        {
            var listado = from item in db.Caracteristicass
                          select new
                          {
                              IdCaract = item.Id,
                              Descripcion = item.Descripcion
                          };
            return Json(new { list = listado }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetalleCaracteristicas(int? id)
        {
            var listado = from item in db.DetalleCaracteristicas
                          where item.Id == id
                          select new
                          {
                              IdDetalle = item.Id,
                              IdCaract = item.CaracteristicaId,
                              Valor = item.Valor
                          };
            return Json(new { listado }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult GuardarCambios(int? idDetalle, int? idCaracteristica, string valor)
        {
            DetalleCaracteristica obj = db.DetalleCaracteristicas.Find(idDetalle);
            obj.CaracteristicaId = (int)idCaracteristica;
            obj.Valor = valor;
            db.Entry(obj).State = EntityState.Modified;
            int a = db.SaveChanges();
            return Json(new {a}, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult AddNuevo(int? idBienAsegurado, int? idCaracteristica, string valor)
        {
            DetalleCaracteristica nuevo = new DetalleCaracteristica();
            nuevo.BienAseguradoId = (int)idBienAsegurado;
            nuevo.CaracteristicaId = (int)idCaracteristica;
            nuevo.Valor = valor;
            db.DetalleCaracteristicas.Add(nuevo);
            int x = db.SaveChanges();
            return Json(new { x }, JsonRequestBehavior.AllowGet);
        }
    }
}
