

























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
    public class DocumentosController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /Documentos/

        public ActionResult Index()

        {



            return View(db.Documentos.ToList());


        }

        // GET: /Documentos/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Documento documento = db.Documentos.Find(id);

            if (documento == null)
            {
                return HttpNotFound();
            }
            return PartialView(documento);
        }

        // GET: /Documentos/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: /Documentos/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,Descripcion")] Documento documento)

        {
            if (ModelState.IsValid)
            {
                if (db.Documentos.FirstOrDefault(a => a.Descripcion == documento.Descripcion) != null)
                    ModelState.AddModelError("Descripcion", "El documento que desea ingresar ya existe");
                else
                {
                    db.Documentos.Add(documento);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }


            return View(documento);
        }

        // GET: /Documentos/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Documento documento = db.Documentos.Find(id);

            if (documento == null)
            {
                return HttpNotFound();
            }

            return View(documento);
        }

        // POST: /Documentos/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,Descripcion")] Documento documento)

        {
            if (ModelState.IsValid)
            {
                if (db.Documentos.FirstOrDefault(a => a.Descripcion == documento.Descripcion && a.Id != documento.Id) != null)
                    ModelState.AddModelError("Descripcion", "El documento que desea ingresar ya existe");
                else
                {
                    db.Entry(documento).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(documento);
        }

        // GET: /Documentos/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Documento documento = db.Documentos.Find(id);

            if (documento == null)
            {
                return HttpNotFound();
            }
            return View(documento);
        }

        // POST: /Documentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            Documento documento = db.Documentos.Find(id);

            db.Documentos.Remove(documento);

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

        public ActionResult ListarDocumentos()
        {
            var listado = from item in db.Documentos
                          select new
                          {
                              IdDoc = item.Id,
                              Descripcion = item.Descripcion
                          };
            return Json(new { listado }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetalleDocumentos(int id)
        {
            var s = db.DetalleDocumentos.ToList();
            var listado = from item in s
                          where item.Id == id
                          select new
                          {
                              IdDetalle = item.Id,
                              IdDoc = item.DocumentoId,
                              Fecha = item.Fecha.ToString("yyyy-MM-dd"),
                              Emisor = item.Emisor,
                              Num = item.Numero,
                              Valor = item.Valor,
                              Comentarios = item.Comentarios
                          };
            return Json(new { listado }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult GuardarCambios(int idDetalle, int IdDocumento, string fecha, string emisor, string numero, string valor, string comentarios)
        {
            DetalleDocumento obj = db.DetalleDocumentos.Find(idDetalle);
            obj.DocumentoId = IdDocumento;
            obj.Fecha = DateTime.Parse(fecha);
            obj.Emisor = emisor;
            obj.Numero = int.Parse(numero);
            obj.Valor = double.Parse(valor);
            obj.Comentarios = comentarios;
            db.Entry(obj).State = EntityState.Modified;
            int a = db.SaveChanges();
            return Json(new { a }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult AddNuevo (int IdReclamo, int IdDocumento, string fecha, string emisor, string numero, string valor, string comentarios)
        {
            DetalleDocumento nuevo =  new DetalleDocumento();
            nuevo.ReclamoId = IdReclamo;
            nuevo.DocumentoId = IdDocumento;
            nuevo.Fecha = DateTime.Parse(fecha);
            nuevo.Emisor = emisor;
            nuevo.Numero = int.Parse(numero);
            nuevo.Valor = double.Parse(valor);
            nuevo.Comentarios = comentarios;
            db.DetalleDocumentos.Add(nuevo);
            int x = db.SaveChanges();
            return Json(new { x }, JsonRequestBehavior.AllowGet);
        }
    }
}
