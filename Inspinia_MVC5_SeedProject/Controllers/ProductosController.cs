

























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
    public class ProductosController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /Productos/

        public ActionResult Index()

        {


            var productos = db.Productos.Include(p => p.Aseguradora).Include(p => p.Ramo);

            return View(productos.ToList());


        }

        // GET: /Productos/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Producto producto = db.Productos.Find(id);

            if (producto == null)
            {
                return HttpNotFound();
            }
            return PartialView(producto);
        }

        // GET: /Productos/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            ViewBag.AseguradoraId = new SelectList(db.Aseguradoras, "Id", "Descripcion");

            ViewBag.RamoId = new SelectList(db.Ramos, "Id", "Descripcion");

            return View();
        }

        // POST: /Productos/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,Codigo,Descripcion,Comision,AseguradoraId,RamoId")] Producto producto)

        {
            if (ModelState.IsValid)
            {
                if (db.Productos.FirstOrDefault(q => q.Codigo == producto.Codigo && q.Descripcion == producto.Descripcion && q.AseguradoraId == producto.AseguradoraId) != null)
                {
                    ModelState.AddModelError("Codigo", "El producto que desea ingresar ya existe");
                    ModelState.AddModelError("Descripcion", "El producto que desea ingresar ya existe");
                }
                else
                {
                    db.Productos.Add(producto);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }


            ViewBag.AseguradoraId = new SelectList(db.Aseguradoras, "Id", "Descripcion", producto.AseguradoraId);

            ViewBag.RamoId = new SelectList(db.Ramos, "Id", "Descripcion", producto.RamoId);

            return View(producto);
        }

        // GET: /Productos/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Producto producto = db.Productos.Find(id);

            if (producto == null)
            {
                return HttpNotFound();
            }

            ViewBag.AseguradoraId = new SelectList(db.Aseguradoras, "Id", "Descripcion", producto.AseguradoraId);

            ViewBag.RamoId = new SelectList(db.Ramos, "Id", "Descripcion", producto.RamoId);

            return View(producto);
        }

        // POST: /Productos/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,Codigo,Descripcion,Comision,AseguradoraId,RamoId")] Producto producto)

        {
            if (ModelState.IsValid)
            {
                if (db.Productos.FirstOrDefault(q => q.Codigo == producto.Codigo && q.Descripcion == producto.Descripcion && q.AseguradoraId == producto.AseguradoraId && producto.Id!=q.Id) != null)
                {
                    ModelState.AddModelError("Codigo", "El producto que desea ingresar ya existe");
                    ModelState.AddModelError("Descripcion", "El producto que desea ingresar ya existe");
                }
                else
                {
                    db.Entry(producto).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            ViewBag.AseguradoraId = new SelectList(db.Aseguradoras, "Id", "Descripcion", producto.AseguradoraId);

            ViewBag.RamoId = new SelectList(db.Ramos, "Id", "Descripcion", producto.RamoId);

            return View(producto);
        }

        // GET: /Productos/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Producto producto = db.Productos.Find(id);

            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: /Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            Producto producto = db.Productos.Find(id);

            db.Productos.Remove(producto);

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
