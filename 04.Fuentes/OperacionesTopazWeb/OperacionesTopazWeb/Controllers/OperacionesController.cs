using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OperacionesTopazWeb.Filters;
using UtilitarioCrk;
using System.Reflection;

namespace OperacionesTopazWeb.Controllers
{
    public class OperacionesController : Controller
    {
        // GET: Operaciones
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