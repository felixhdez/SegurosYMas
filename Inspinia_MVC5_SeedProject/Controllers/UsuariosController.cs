using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inspinia_MVC5_SeedProject.Models;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    [Authorize(Roles="Administración,Gerencia")]
    public class UsuariosController : Controller
    {
        private SeguridadUserBD seg = new SeguridadUserBD();
        private ApplicationDbContext context = new ApplicationDbContext();
        private SeguroBD db = new SeguroBD();
        // GET: Usuarios
        public ActionResult Index()
        {
            var lista = seg.Usuarios.Where(x=> x.Estado).ToList();
            return View(lista);
        }

        public ActionResult Create(string nombre, string apellidos, string correo, string password, int idRol, int? IdCliente)
        {
            int d = -3;
            try
            {
                Usuarios obj = new Usuarios();
                obj.Nombre = nombre;
                obj.Apellido = apellidos;
                obj.User = correo;
                obj.Pass = Encriptar(password);
                obj.RolId = idRol;
                if (IdCliente != 0)
                    obj.IdNUser = IdCliente;
                else
                    obj.IdNUser = null;
                seg.Usuarios.Add(obj);
                d = seg.SaveChanges();
            }
            catch (Exception)
            {

            }

            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Roles()
        {
            var l = (from item in seg.Roles.ToList()
                     select new
                     {
                         IdRol = item.Id,
                         Descripcion = item.Descripcion
                     }).ToList();
            return Json(new { l }, JsonRequestBehavior.AllowGet);
        }

        /// Encripta una cadena
        public string Encriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        public ActionResult Editar(int id)
        {
            var AspId = seg.Usuarios.Where(x => x.Id == id).Select(y => y.AspUser).FirstOrDefault();
            return RedirectToAction("Edit", "Account", new { id = AspId });
        }

        public ActionResult Deshabilitar(int id)
        {
            var AspId = seg.Usuarios.Where(x => x.Id == id).Select(y => y.AspUser).FirstOrDefault();
            return RedirectToAction("Deshabilitar", "Account", new { id = AspId });
        }
    }
}
