using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperacionesTopazWeb.Controllers
{
    public class AccesosController : Controller
    {
        public ActionResult AccesoDenegado()
        {
            return View();
        }

        public ActionResult TimeOut()
        {
            return View();
        }


        // GET: /Error/
        public ActionResult Error(string Error = "0")
        {
            if (Error == "404")
            {
                ViewData["Tipo"] = 1;
            }
            else if (Error == "500")
            {
                ViewData["Tipo"] = 2;
            }
            else if (Error == "999")
            {
                ViewData["Tipo"] = 999;
            }
            else
            {
                ViewData["Tipo"] = 3;
            }
            return View();
        }
    }
}