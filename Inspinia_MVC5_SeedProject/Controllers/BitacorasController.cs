using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using System.Net;
using System.Web;
using System.Web.Mvc;

using Inspinia_MVC5_SeedProject.Models;
using System.IO;
using System.Web.Helpers;
using System.Drawing;
using System.Drawing.Imaging;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    [Authorize]
    public class BitacorasController : Controller
    {
        private SeguroBD db = new SeguroBD();

        // GET: /Bitacoras/
        public ActionResult FileUpload()
        {
            return View();
        }
        public ActionResult Index()

        {

            var bitacoras = db.Bitacoras.Include(b => b.Poliza);

            return View(bitacoras.ToList());

        }

        // GET: /Bitacoras/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bitacora bitacora = db.Bitacoras.Find(id);

            if (bitacora == null)
            {
                return HttpNotFound();
            }
            return View(bitacora);
        }

        // GET: /Bitacoras/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {

            ViewBag.PolizaId = new SelectList(db.Polizas, "Id", "NumPoliza");

            return View();
        }

        // POST: /Bitacoras/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create([Bind(Include="Id,Fecha,Observacion,PolizaId")] Bitacora bitacora, int? IdPoliza)

        {

            var x = Request[""];
            int id = (int)IdPoliza;

            //var files = Request.Files;
            if (ModelState.IsValid)
            {
                ArchivosPólizas obj = new ArchivosPólizas();
                HttpPostedFileBase FileBase = Request.Files[0];
                WebImage imagen = new WebImage(FileBase.InputStream);
                //obj.Foto = imagen.GetBytes();
                obj.PolizaId = id;
                db.ArchivosPolizas.Add(obj);
                db.SaveChanges();



                db.Bitacoras.Add(bitacora);

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.PolizaId = new SelectList(db.Polizas, "Id", "NumPoliza", bitacora.PolizaId);

            return View(bitacora);
        }

        [HttpPost]
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Prueba(HttpPostedFileBase files)
        {

            if (ModelState.IsValid)
            {
                     if (Request["PolizaId"] != null)
                {
                    var w = Request["PolizaId"];
                    if (!string.IsNullOrEmpty(w))
                    {

                        using (var transact = db.Database.BeginTransaction())
                        {
                            try
                            {
                                if (Request.Files.Count > 0 && Request.Files[0].ContentLength> 0)//verifica que hay archivos, si no hay, no pasa por la codicion de archivos para ser guardado
                                {
                                    string path = Server.MapPath("~/Content/Imagen");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            if (Request.Files.Count > 0)
                            {
                                var filesCount = Request.Files.Count;
                                for (int i = 0; i < filesCount; i++)
                                {
                                    var file = Request.Files[i];
                                    var fileName = Path.GetFileName(file.FileName);
                                    path = Path.Combine(Server.MapPath("~/Content/Imagen"), fileName);

                                    file.SaveAs(path);
                                    string url = Path.Combine("/Content/Imagen", fileName);
                                    ArchivosPólizas obj = new ArchivosPólizas();
                                    obj.Foto = url;
                                    obj.PolizaId = int.Parse(Request["PolizaId"]);
                                    db.ArchivosPolizas.Add(obj);
                                    db.SaveChanges();
                                }
                            }
                                }
                                if (Request["Fecha"] != null)
                        {
                            var valor = Request["Fecha"];
                            if (!string.IsNullOrEmpty(valor))
                            {

                                Bitacora b = new Bitacora();
                                b.Observacion = Request["Observacion"];
                                b.Fecha = DateTime.Parse(Request["Fecha"]);
                                b.PolizaId = int.Parse(Request["PolizaId"]);

                                db.Bitacoras.Add(b);
                                db.SaveChanges();
                            }
                        }
                                //Si todo se hizo correctamente se guardan los datos definitivamente
                                transact.Commit();
                            }
                            catch (Exception)
                            {
                                //Si hubo algun error en el almacenamiento de los datos
                                //deshacemos todos lo que habiamos guardado
                                transact.Rollback();
                            }
                        }

                    }
                }

            }
            return RedirectToAction("Index", "Polizas", null);
        }

        //drop and drag------------------------------------------------------------------------------------
        //[HttpPost]
        //public void UploadFiles()
        //{
        //    if (Request.Files?.Count > 0)
        //    {
        //        var filesCount = Request.Files.Count;
        //        for (int i = 0; i < filesCount; i++)
        //        {
        //            var file = Request.Files[i];
        //            var fileName = Path.GetFileName(file.FileName);
        //            var path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);

        //            file.SaveAs(path);
        //        }
        //    }
        //}
        //drop and drag----------------------------------------------------------------------------------------
        // GET: /Bitacoras/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bitacora bitacora = db.Bitacoras.Find(id);

            if (bitacora == null)
            {
                return HttpNotFound();
            }

            ViewBag.PolizaId = new SelectList(db.Polizas, "Id", "NumPoliza", bitacora.PolizaId);

            return View(bitacora);
        }

        //public ActionResult GetImagen(int id)
        //{

        //    //ArchivosPólizas ar = new ArchivosPólizas();
        //    var imagenPo = db.ArchivosPolizas.Where(x => x.PolizaId == id).FirstOrDefault();
        //    //FileContentResult ima = new FileContentResult(imagenPo.Foto, "*.*");
        //    for (int i = 0; i < imagenPo.Foto.Count(); i++)
        //    {
        //       var formato = File(imagenPo.Foto, "image/jpg", "image/png");
        //        //ima = formato;
        //    }

        //    return ima;
        //byte[] byteImagen = ar.Foto;

        //MemoryStream memoryStream = new MemoryStream(byteImagen);
        //for (int i = 0; i <= memoryStream.Length; i++)
        //{
        //    Image image = Image.FromStream(memoryStream);

        //    memoryStream = new MemoryStream();
        //    image.Save(memoryStream, ImageFormat.Png);
        //    image.Save(memoryStream, ImageFormat.Jpeg);
        //    memoryStream.Position = 0;
        //}

        //}


        // POST: /Bitacoras/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit([Bind(Include="Id,Fecha,Observacion,PolizaId")] Bitacora bitacora)

        {
            if (ModelState.IsValid)
            {
                db.Entry(bitacora).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.PolizaId = new SelectList(db.Polizas, "Id", "NumPoliza", bitacora.PolizaId);

            return View(bitacora);
        }

        // GET: /Bitacoras/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bitacora bitacora = db.Bitacoras.Find(id);

            if (bitacora == null)
            {
                return HttpNotFound();
            }
            return View(bitacora);
        }

        // POST: /Bitacoras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            Bitacora bitacora = db.Bitacoras.Find(id);

            db.Bitacoras.Remove(bitacora);

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

        public ActionResult RecuperarBitacora(int? id)
        {
            var x = db.Bitacoras.ToList();
            var lista = from item in x
                        where item.PolizaId == (int)id
                        select new
                        {
                            Fecha = item.Fecha.ToString("yyyy-MM-dd"),
                            Observacion = item.Observacion
                        };
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }
    }
}
