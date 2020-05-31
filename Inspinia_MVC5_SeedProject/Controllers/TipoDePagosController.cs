

























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
    public class TipoDePagosController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /TipoDePagos/

        public ActionResult Index()

        {



            return View(db.TipoDepagos.ToList());


        }

        // GET: /TipoDePagos/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TipoDePago tipoDePago = db.TipoDepagos.Find(id);

            if (tipoDePago == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipoDePago);
        }

        // GET: /TipoDePagos/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: /TipoDePagos/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,Descripcion")] TipoDePago tipoDePago)

        {
            if (ModelState.IsValid)
            {
                if (db.TipoDepagos.FirstOrDefault(p => p.Descripcion == tipoDePago.Descripcion) != null)
                    ModelState.AddModelError("Descripcion", "El tipo de pago que desea ingresar ya existe");
                else
                {
                    db.TipoDepagos.Add(tipoDePago);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }


            return View(tipoDePago);
        }

        // GET: /TipoDePagos/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TipoDePago tipoDePago = db.TipoDepagos.Find(id);

            if (tipoDePago == null)
            {
                return HttpNotFound();
            }

            return View(tipoDePago);
        }

        // POST: /TipoDePagos/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,Descripcion")] TipoDePago tipoDePago)

        {
            if (ModelState.IsValid)
            {
                if (db.TipoDepagos.FirstOrDefault(p => p.Descripcion == tipoDePago.Descripcion && p.Id != tipoDePago.Id) != null)
                    ModelState.AddModelError("Descripcion", "El tipo de pago que desea ingresar ya existe");
                else
                {
                    db.Entry(tipoDePago).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(tipoDePago);
        }

        // GET: /TipoDePagos/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TipoDePago tipoDePago = db.TipoDepagos.Find(id);

            if (tipoDePago == null)
            {
                return HttpNotFound();
            }
            return View(tipoDePago);
        }

        // POST: /TipoDePagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            TipoDePago tipoDePago = db.TipoDepagos.Find(id);

            db.TipoDepagos.Remove(tipoDePago);

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

        public ActionResult ListarPagos()
        {
            var listado = from item in db.TipoDepagos
                          select new
                          {
                              IdPag = item.Id,
                              Descripcion = item.Descripcion
                          };
            return Json(new { listado }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetallePagos(int id)
        {
            var t = db.DetallePagos.ToList();
            var listado = from item in t
                          where item.Id == id
                          select new
                          {
                              IdDetalle = item.Id,
                              IdPago = item.TipoDePagoId,
                              Fecha = item.Fecha.ToString("yyyy-MM-dd"),
                              Moneda= item.Moneda,
                              Num = item.NumDoc,
                              Valor = item.Valor,
                              Banco = item.BancoTaller,
                              Nota = item.Nota
                          };
            return Json(new { listado }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult GuardarCambios(int idDetalle, int IdPago, string fecha, string moneda, string numero, string valor, string banco, string nota)
        {
            DetallePago obj = db.DetallePagos.Find(idDetalle);
            obj.TipoDePagoId = IdPago;
            obj.Fecha = DateTime.Parse(fecha);
            obj.Moneda = moneda;
            obj.NumDoc= int.Parse(numero);
            obj.Valor = double.Parse(valor);
            obj.BancoTaller = banco;
            obj.Nota = nota;
            db.Entry(obj).State = EntityState.Modified;
            int a = db.SaveChanges();
            return Json(new { a }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult AddNuevo(int IdReclamo, int IdPago, string fecha, string moneda, string numero, string valor, string banco, string nota)
        {
            DetallePago nuevo = new DetallePago();
            nuevo.ReclamoId = IdReclamo;
            nuevo.TipoDePagoId = IdPago;
            nuevo.Fecha = DateTime.Parse(fecha);
            nuevo.Moneda = moneda;
            nuevo.NumDoc = int.Parse(numero);
            nuevo.Valor = double.Parse(valor);
            nuevo.BancoTaller = banco;
            nuevo.Nota = nota;
            db.DetallePagos.Add(nuevo);
            int x = db.SaveChanges();
            return Json(new { x }, JsonRequestBehavior.AllowGet);
        }
    }
}
