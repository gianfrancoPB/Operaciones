using OperacionesTopazWeb.Filters;
using System;
using System.Reflection;
using System.Web.Mvc;
using UtilitarioCrk;

namespace OperacionesTopazWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [SeguridadIngreso]
        public ActionResult Index(string id)
        {
            return View();
        }
        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult CerrarSesion()
        {
            var objResultado = new object();
            try
            {
                //Cierra sesiones globales
                Seguridad.CerrarSesion();
                //Cierra sesiones locales
                CerrarSesionesLocales();
                objResultado = new
                {
                    iResultado = 1
                };
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_ExcepcionesTopazWEB, UtlConstantes.LogNamespace_ExcepcionesTopazWEB, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return Json(objResultado);
        }

        private void CerrarSesionesLocales()
        {
            //System.Web.HttpContext.Current.Session[sSMaximoFiadores] = null;
        }
    }
}