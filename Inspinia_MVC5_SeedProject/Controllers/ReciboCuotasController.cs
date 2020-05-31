

























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
    public class ReciboCuotasController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /ReciboCuotas/

        public ActionResult Index()

        {


            var recibocuotas = db.ReciboCuotas.Include(r => r.Cuota);

            var recibospolizas = from item in db.ReciboCuotas
                                 join item2 in db.Cuotas on item.CuotaId equals item2.Id
                                 join item3 in db.DetalleCuotas on item2.Id equals item3.CuotaId
                                 select item;
                                 

            return View(recibospolizas.ToList().Distinct());


        }

        // GET: /ReciboCuotas/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ReciboCuota reciboCuota = db.ReciboCuotas.Find(id);

            if (reciboCuota == null)
            {
                return HttpNotFound();
            }
            return PartialView(reciboCuota);
        }

        // GET: /ReciboCuotas/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador,Cobranza")]
        public ActionResult Create()
        {

            ViewBag.DetalleCuotaId = new SelectList(db.DetalleCuotas, "Id", "Cuotas");

            return View();
        }

        // POST: /ReciboCuotas/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administración,Gerencia,Digitador,Cobranza")]
        public ActionResult Create([Bind(Include="Id,Fecha,NumRecibo,Pago,PagoNeto,DetalleCuotaId")] ReciboCuota reciboCuota)

        {
            if (ModelState.IsValid)
            {

                db.ReciboCuotas.Add(reciboCuota);

                db.SaveChanges();

                return RedirectToAction("Index");
            }


            ViewBag.DetalleCuotaId = new SelectList(db.DetalleCuotas, "Id", "Cuotas", reciboCuota.CuotaId);

            return View(reciboCuota);
        }

        [HttpPost]
        [Authorize(Roles = "Administración,Gerencia,Digitador,Cobranza")]
        public ActionResult CreateRecibo(string fecha, string numrecibo, string pago, string pagoneto, int? cuota)
        {
            int d = -3;
            using (var transac = db.Database.BeginTransaction())
            {
                try
                {
                    double pagnet = double.Parse(pagoneto);
                    List<DetalleCuota> det = db.DetalleCuotas.Where(t => t.CuotaId == (int)cuota).ToList();
                    double faltante = det.Sum(x=> x.Saldo);
                    if (faltante >= pagnet)
                    {
                        ReciboCuota r = new ReciboCuota();
                        r.Fecha = DateTime.Parse(fecha);
                        r.NumRecibo = numrecibo;
                        r.Pago = double.Parse(pago);
                        r.PagoNeto = double.Parse(pagoneto);
                        r.CuotaId = (int)cuota;
                        db.ReciboCuotas.Add(r);
                        db.SaveChanges();

                        bool band = false;
                        double pendiente = 0;

                        for (int i = 0; i < det.Count && !band; i++)
                        {
                            if (det[i].Saldo != 0)
                            {
                                if (pagnet <= det[i].Saldo)
                                {
                                    det[i].Saldo -= pagnet;
                                    det[i].Saldo = Math.Round(det[i].Saldo, 2);
                                    band = true;
                                }
                                else
                                {
                                    pendiente = pagnet - det[i].Saldo;
                                    det[i].Saldo -= det[i].Saldo;
                                    pagnet = pendiente;
                                }
                                if (det[i].Saldo != 0)
                                    det[i].Estado = "Abonado";
                                else
                                    det[i].Estado = "Pagado";

                                //finally
                                db.Entry(det[i]).State = EntityState.Modified;
                                d = db.SaveChanges();
                            }
                        }
                        transac.Commit();
                    }
                    else
                        d = -5;
                }
                catch (Exception)
                {
                    transac.Rollback();
                }
            }
            
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }

        // GET: /ReciboCuotas/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador,Cobranza")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ReciboCuota reciboCuota = db.ReciboCuotas.Find(id);

            if (reciboCuota == null)
            {
                return HttpNotFound();
            }

            ViewBag.DetalleCuotaId = new SelectList(db.DetalleCuotas, "Id", "Cuotas", reciboCuota.CuotaId);

            return View(reciboCuota);
        }

        // POST: /ReciboCuotas/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administración,Gerencia,Digitador,Cobranza")]
        public ActionResult Edit([Bind(Include="Id,Fecha,NumRecibo,Pago,PagoNeto,CuotaId")] ReciboCuota reciboCuota)

        {
            if (ModelState.IsValid)
            {
                using (var transact = db.Database.BeginTransaction())
                {
                    try
                    {
                        List<ReciboCuota> abonos = db.ReciboCuotas.Where(t=> t.CuotaId == reciboCuota.CuotaId).ToList();
                        ReciboCuota actual = abonos.FirstOrDefault(w=> w.Id == reciboCuota.Id);
                        List<DetalleCuota> cuotas = db.DetalleCuotas.Where(x => x.CuotaId == actual.CuotaId).ToList();
                        double pagnet = actual.PagoNeto, pendiente = 0;

                        //Recorrer todos los detalles existentes y reestablecerlos
                        foreach (var item in cuotas)
                        {
                            item.Saldo = item.Monto;
                            item.Estado = "Pendiente";
                        }

                        //Recorremos los abonos
                        for (int j = 0; j < abonos.Count; j++)
                        {
                            bool band = false;
                            //Recorremos las cuotas
                            for (int i = 0; i < cuotas.Count && !band; i++)
                            {
                                //Si encontramos el detalle actual, el que estamos editando.
                                if (actual.Id == abonos[j].Id)
                                    abonos[j].PagoNeto = reciboCuota.PagoNeto;

                                if (cuotas[i].Saldo != 0)
                                {
                                    if (abonos[j].PagoNeto <= cuotas[i].Saldo)
                                    {
                                        cuotas[i].Saldo -= abonos[j].PagoNeto;
                                        cuotas[i].Saldo = Math.Round(cuotas[i].Saldo, 2);
                                        band = true;
                                    }
                                    else
                                    {
                                        pendiente = pagnet - cuotas[i].Saldo;
                                        cuotas[i].Saldo -= cuotas[i].Saldo;
                                        pagnet = pendiente;
                                    }
                                    if (cuotas[i].Saldo != 0)
                                        cuotas[i].Estado = "Abonado";
                                    else
                                        cuotas[i].Estado = "Pagado";

                                    //finally
                                    db.Entry(cuotas[i]).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }

                        }

                        db.Entry(actual).State = EntityState.Detached;
                        db.Entry(reciboCuota).State = EntityState.Modified;
                        db.SaveChanges();
                        transact.Commit();
                    }
                    catch (Exception)
                    {
                        transact.Rollback();
                    }
                }

                return RedirectToAction("Index");
            }

            ViewBag.DetalleCuotaId = new SelectList(db.DetalleCuotas, "Id", "Cuotas", reciboCuota.CuotaId);

            return View(reciboCuota);
        }

        // GET: /ReciboCuotas/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador,Cobranza")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ReciboCuota reciboCuota = db.ReciboCuotas.Find(id);

            if (reciboCuota == null)
            {
                return HttpNotFound();
            }
            return View(reciboCuota);
        }

        // POST: /ReciboCuotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            ReciboCuota reciboCuota = db.ReciboCuotas.Find(id);

            db.ReciboCuotas.Remove(reciboCuota);

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

        public ActionResult GetRecibos(int? id)
        {
            var d = db.ReciboCuotas.Where(x => x.CuotaId == (int)id).ToList();
            var lista = from item in d
                        select new
                        {
                            IdR = item.Id,
                            Prima = item.Cuota.ReciboDePrima,
                            Fecha = item.Fecha.ToString("yyyy-MM-dd"),
                            Recibo = item.NumRecibo,
                            Pago = item.Pago,
                            PagoNeto = item.PagoNeto
                        };
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        //================================RECIBOS DE CUOTAS DE ADENDAS======================================
        public ActionResult IndexAdendas() {

            var recibosadendas = from item in db.ReciboCuotas
                                 join item2 in db.Cuotas on item.CuotaId equals item2.Id
                                 join item3 in db.DetalleCuotasAdenda on item2.Id equals item3.CuotaId
                                 select item;


            return View(recibosadendas.ToList().Distinct());
        }

        public ActionResult DetailsAdendas(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ReciboCuota reciboCuota = db.ReciboCuotas.Find(id);

            if (reciboCuota == null) {
                return HttpNotFound();
            }
            return PartialView(reciboCuota);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador,Cobranza")]
        public ActionResult CreateAdendas() {

            ViewBag.DetalleCuotaId = new SelectList(db.DetalleCuotas,"Id","Cuotas");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administración,Gerencia,Digitador,Cobranza")]
        public ActionResult CreateReciboAdendas(string fecha,string numrecibo,string pago,string pagoneto,int? cuota) {
            int d = -3;
            using (var transac = db.Database.BeginTransaction()) {
                try {
                    double pagnet = double.Parse(pagoneto);
                    List<DetalleCuotasAdenda> det = db.DetalleCuotasAdenda.Where(t => t.CuotaId == (int)cuota).ToList();
                    double faltante = det.Sum(x => x.Saldo);
                    if (faltante >= pagnet) {
                        ReciboCuota r = new ReciboCuota();
                        r.Fecha = DateTime.Parse(fecha);
                        r.NumRecibo = numrecibo;
                        r.Pago = double.Parse(pago);
                        r.PagoNeto = double.Parse(pagoneto);
                        r.CuotaId = (int)cuota;
                        db.ReciboCuotas.Add(r);
                        db.SaveChanges();

                        bool band = false;
                        double pendiente = 0;

                        for (int i = 0;i < det.Count && !band;i++) {
                            if (det[i].Saldo != 0) {
                                if (pagnet <= det[i].Saldo) {
                                    det[i].Saldo -= pagnet;
                                    det[i].Saldo = Math.Round(det[i].Saldo,2);
                                    band = true;
                                } else {
                                    pendiente = pagnet - det[i].Saldo;
                                    det[i].Saldo -= det[i].Saldo;
                                    pagnet = pendiente;
                                }
                                if (det[i].Saldo != 0)
                                    det[i].Estado = "Abonado";
                                else
                                    det[i].Estado = "Pagado";

                                //finally
                                db.Entry(det[i]).State = EntityState.Modified;
                                d = db.SaveChanges();
                            }
                        }
                        transac.Commit();
                    } else
                        d = -5;
                } catch (Exception) {
                    transac.Rollback();
                }
            }

            return Json(new { d },JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administración,Gerencia,Digitador,Cobranza")]
        public ActionResult EditAdendas(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ReciboCuota reciboCuota = db.ReciboCuotas.Find(id);

            if (reciboCuota == null) {
                return HttpNotFound();
            }

            ViewBag.DetalleCuotaId = new SelectList(db.DetalleCuotas,"Id","Cuotas",reciboCuota.CuotaId);

            return View(reciboCuota);
        }

        // POST: /ReciboCuotas/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administración,Gerencia,Digitador,Cobranza")]
        public ActionResult EditAdendas([Bind(Include = "Id,Fecha,NumRecibo,Pago,PagoNeto,CuotaId")] ReciboCuota reciboCuota) {
            if (ModelState.IsValid) {
                using (var transact = db.Database.BeginTransaction()) {
                    try {
                        List<ReciboCuota> abonos = db.ReciboCuotas.Where(t => t.CuotaId == reciboCuota.CuotaId).ToList();
                        ReciboCuota actual = abonos.FirstOrDefault(w => w.Id == reciboCuota.Id);
                        List<DetalleCuotasAdenda> cuotas = db.DetalleCuotasAdenda.Where(x => x.CuotaId == actual.CuotaId).ToList();
                        double pagnet = actual.PagoNeto, pendiente = 0;

                        //Recorrer todos los detalles existentes y reestablecerlos
                        foreach (var item in cuotas) {
                            item.Saldo = item.Monto;
                            item.Estado = "Pendiente";
                        }

                        //Recorremos los abonos
                        for (int j = 0;j < abonos.Count;j++) {
                            bool band = false;
                            //Recorremos las cuotas
                            for (int i = 0;i < cuotas.Count && !band;i++) {
                                //Si encontramos el detalle actual, el que estamos editando.
                                if (actual.Id == abonos[j].Id)
                                    abonos[j].PagoNeto = reciboCuota.PagoNeto;

                                if (cuotas[i].Saldo != 0) {
                                    if (abonos[j].PagoNeto <= cuotas[i].Saldo) {
                                        cuotas[i].Saldo -= abonos[j].PagoNeto;
                                        cuotas[i].Saldo = Math.Round(cuotas[i].Saldo,2);
                                        band = true;
                                    } else {
                                        pendiente = pagnet - cuotas[i].Saldo;
                                        cuotas[i].Saldo -= cuotas[i].Saldo;
                                        pagnet = pendiente;
                                    }
                                    if (cuotas[i].Saldo != 0)
                                        cuotas[i].Estado = "Abonado";
                                    else
                                        cuotas[i].Estado = "Pagado";

                                    //finally
                                    db.Entry(cuotas[i]).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }

                        }

                        db.Entry(actual).State = EntityState.Detached;
                        db.Entry(reciboCuota).State = EntityState.Modified;
                        db.SaveChanges();
                        transact.Commit();
                    } catch (Exception) {
                        transact.Rollback();
                    }
                }

                return RedirectToAction("IndexAdendas");
            }

            ViewBag.DetalleCuotaId = new SelectList(db.DetalleCuotas,"Id","Cuotas",reciboCuota.CuotaId);

            return View(reciboCuota);
        }
    }
}
