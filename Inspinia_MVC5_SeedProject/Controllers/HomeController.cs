using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    public class HomeController : Controller
    {

        
        [Authorize]
        public ActionResult Index()
        {
            ViewData["SubTitle"] = "Welcome in ASP.NET MVC 5 INSPINIA SeedProject ";
            ViewData["Message"] = "It is an application skeleton for a typical MVC 5 project. You can use it to quickly bootstrap your webapp projects.";

            return View();
        }

        public ActionResult Minor()
        {
            ViewData["SubTitle"] = "Simple example of second view";
            ViewData["Message"] = "Data are passing to view by ViewData from controller";

            return View();
        }

        [AllowAnonymous]
        public ActionResult LandingPage()
        {
            return View();
        }

       //public ActionResult App()
       // {
       //     var ruta = Server.MapPath("~/Content/apk/AppSeguros2.apk");
       //      File (ruta, "aplication/apk", "SegurosYMas.apk");
       //     return Json(new { lista }, JsonRequestBehavior.AllowGet);
       // }


        public ActionResult Manual()
        {
            return View();
        }
    }
}