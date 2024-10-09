using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UtilitarioCrk;


namespace OperacionesTopazWeb.Filters
{
    public class SeguridadIngreso : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.Count == 0)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Accesos", action = "AccesoDenegado" }));
            }
            else
            {
                var urlParameter = filterContext.ActionParameters["id"];
                if (urlParameter == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Accesos", action = "AccesoDenegado" }));
                }
                else
                {
                   
                    bool bAgregarSession = Seguridad.ValidarCadenaConexion((string)urlParameter);
                    if (!bAgregarSession)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Accesos", action = "AccesoDenegado" }));
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }

    public class SeguridadSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool bValidar = Seguridad.ValidarSession();
            if (bValidar)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Accesos", action = "TimeOut" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }


    public class SeguridadSessionAjax : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool bValidar = Seguridad.ValidarSession();
            if (bValidar)
            {
                filterContext.Result = new JsonResult
                {
                    Data = new { iTipoResultado = -5, message = UtlConstantes.msgErrorSesion },
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                };
            }
            base.OnActionExecuting(filterContext);
        }
    }
}