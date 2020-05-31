

























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
    public class ClientesController : Controller
    {
        private SeguroBD db = new SeguroBD();
        List<String> obj = new List<string> { "Natural", "Jurídico" };

        // GET: /Clientes/

        public ActionResult Index()

        {


            var clientes = db.Clientes.Include(c => c.Departamento);

            return View(clientes.ToList());


        }

        public JsonResult GetPolizas(int Id)
        {
            var d = db.Polizas.Where(x=> x.ClienteId == Id).ToList();
            var list = from i in d
                        select new
                        {
                            IdPoliza = i.Id,
                            NumPoliza = i.NumPoliza,
                            Tipo = i.Tipo,
                            FHasta = i.FechaHasta.ToString("yyyy-MM-dd")
                        };
            return Json(new { list }, JsonRequestBehavior.AllowGet);
        }
        
        //Listamos a los clientes
        public JsonResult SearchClientes(int Id)
        {
            var data = db.Clientes.ToList();
            var list = from item in data
                       where item.Id == Id
                       select new
                       {
                           Apellidos = item.Apellidos,
                           Nombres = item.Nombres,
                           Identificacion = item.Identificacion,
                           NumTelf1 = item.NumTelf1,
                           NumTelf2 = item.NumTelf2,
                           NumTelf3 = item.NumTelf3,
                           Celular = item.Celular,
                           Email = item.Email,
                           TipoCliente = item.TipoCliente,
                           FechaNacimiento = item.FechaNacimiento.ToString("yyyy-MM-dd"),
                           Depto = item.Departamento.Descripcion,
                           DeptoId = item.Departamento.Id,
                           Notas = item.Notas,
                           Direccion = item.Direccion
                       };
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getClientes()
        {
            var data = db.Clientes.ToList();
            var list = from item in data
                       select new
                       {
                           IdCliente = item.Id,
                           Apellidos = item.Apellidos,
                           Nombres = item.Nombres,
                           Identificacion = item.Identificacion,
                           TipoCliente = item.TipoCliente
                       };
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchContratantes(int Id)
        {
            var data = db.Personas.ToList();
            var list = from item in data
                       where item.Id == Id
                       select new
                       {
                           IdContratante = item.Id,
                           Apellidos = item.Apellidos,
                           Nombres = item.Nombres,
                           Identificacion = item.Identificacion,
                           NumTelf1 = item.NumTelf1,
                           NumTelf2 = item.NumTelf2,
                           NumTelf3 = item.NumTelf3,
                           Celular = item.Celular,
                           Email = item.Email,
                           Depto = item.Departamento.Descripcion,
                           DeptoId = item.Departamento.Id,
                           Notas = item.Notas,
                           Direccion = item.Direccion
                       };
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getContratantes()
        {
            var data = db.Personas.ToList();
            var list = from item in data
                       where !db.Clientes.Select(x=> x.Id).Contains(item.Id)
                       select new
                       {
                           IdContratante = item.Id,
                           Apellidos = item.Apellidos,
                           Nombres = item.Nombres,
                           Identificacion = item.Identificacion,
                           Email = item.Email
                       };
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        //json Intermediario
        [HttpPost]
        public JsonResult getIntermediarios()
        {
            var data = db.ContactoIntermediarios.ToList();
            var list = from item in data    
                       select new
                       {
                           IdContacto = item.Id,
                           Apellidos = item.Apellidos,
                           Nombres = item.Nombres,
                           Email = item.Email,
                           Cargo = item.Cargo
                       };
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchIntermediarios(int Id)
        {
            var data = db.ContactoIntermediarios.ToList();
            var list = from item in data
                       where item.Id == Id
                       select new
                       {
                           IdContacto = item.Id,
                           Apellidos = item.Apellidos,
                           Nombres = item.Nombres,
                           NumTelf1 = item.Telefono,
                           Email = item.Email,
                           Cargo = item.Cargo
                       };
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }


        // GET: /Clientes/Details/5

        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cliente cliente = db.Clientes.Find(id);

            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: /Clientes/Create
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Create()
        {
          

            ViewBag.DropDown = new SelectList(obj);

            ViewBag.DepartamentoId = new SelectList(db.Departamentos, "Id", "Descripcion");

            return View();
        }

        // POST: /Clientes/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include="Id,Apellidos,Nombres,Identificacion,Direccion,NumTelf1,NumTelf2,NumTelf3,Celular,Email,Notas,DepartamentoId,TipoCliente,FechaNacimiento")] Cliente cliente)

        {
            if (ModelState.IsValid)
            {
                //Validamos que el cliente a ingresar no exista
                if (db.Clientes.FirstOrDefault(a => a.Apellidos == cliente.Apellidos && a.Nombres == cliente.Nombres && a.Identificacion == cliente.Identificacion) != null)
                {
                    ModelState.AddModelError("Apellidos", "El cliente que desea ingresar ya existe");
                    ModelState.AddModelError("Nombres", "El cliente que desea ingresar ya existe");
                    ModelState.AddModelError("Identificacion", "El cliente que desea ingresar ya existe");
                }
                else
                {
                    if (cliente.TipoCliente == "Natural")
                    {
                        if (ValidarFechaCedula(cliente.Identificacion, cliente.FechaNacimiento.ToString("yyyy-MM-dd")))
                        {
                            ModelState.AddModelError("Identificacion", "La identificación del cliente y la fecha de nacimiento no coinciden.");
                            ModelState.AddModelError("FechaNacimiento", "La identificación del cliente y la fecha de nacimiento no coinciden.");
                        }
                        else
                        {
                            db.Personas.Add(cliente);

                            db.SaveChanges();

                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        db.Personas.Add(cliente);

                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
                    
            }

            ViewBag.DropDown = new SelectList(obj);
            ViewBag.DepartamentoId = new SelectList(db.Departamentos, "Id", "Descripcion", cliente.DepartamentoId);

            return View(cliente);
        }

        public bool ValidarFechaCedula(string ident, string fechanac)
        {
            string f = (ident.Split('-'))[1];
            if (f.Length == 6)
            {
                string fec = ("" + f[4] + f[5] + "-" + f[2] + f[3] + "-" + f[0] + f[1]).ToString();
                string fec2 = fechanac.Remove(0, 2);
                if (fec != fec2)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        // GET: /Clientes/Edit/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cliente cliente = db.Clientes.Find(id);

            if (cliente == null)
            {
                return HttpNotFound();
            }

            ViewBag.DropDown = new SelectList(obj);

            ViewBag.DepartamentoId = new SelectList(db.Departamentos, "Id", "Descripcion", cliente.DepartamentoId);

            return View(cliente);
        }

        // POST: /Clientes/Edit/5

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 

        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include="Id,Apellidos,Nombres,Identificacion,Direccion,NumTelf1,NumTelf2,NumTelf3,Celular,Email,Notas,DepartamentoId,TipoCliente,FechaNacimiento")] Cliente cliente)

        {
            if (ModelState.IsValid)
            {
                if (db.Clientes.FirstOrDefault(a => a.Apellidos == cliente.Apellidos && a.Nombres == cliente.Nombres && a.Identificacion == cliente.Identificacion && a.Id != cliente.Id) != null)
                {
                    ModelState.AddModelError("Apellidos","El cliente que desea ingresar ya existe");
                    ModelState.AddModelError("Nombres","El cliente que desea ingresar ya existe");
                    ModelState.AddModelError("Identificacion","El cliente que desea ingresar ya existe");
                }
                else
                if (cliente.TipoCliente == "Natural")
                {
                    if (ValidarFechaCedula(cliente.Identificacion,cliente.FechaNacimiento.ToString("yyyy-MM-dd")))
                    {
                        ModelState.AddModelError("Identificacion","La identificación del cliente y la fecha de nacimiento no coinciden.");
                        ModelState.AddModelError("FechaNacimiento","La identificación del cliente y la fecha de nacimiento no coinciden.");
                    }
                    else
                    {
                        db.Entry(cliente).State = EntityState.Modified;

                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    db.Entry(cliente).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            ViewBag.DropDown = new SelectList(obj);

            ViewBag.DepartamentoId = new SelectList(db.Departamentos, "Id", "Descripcion", cliente.DepartamentoId);

            return View(cliente);
        }

        // GET: /Clientes/Delete/5
        [Authorize(Roles = "Administración,Gerencia,Digitador")]
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cliente cliente = db.Clientes.Find(id);

            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: /Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            Cliente cliente = db.Clientes.Find(id);

            db.Personas.Remove(cliente);

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
