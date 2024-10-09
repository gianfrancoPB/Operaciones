using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OperacionesTopazWeb.Filters;
using OperacionesTopazWeb.wsCondicion;
using SeguridadCrkWCF;
using UtilitarioCrk;

 

namespace OperacionesTopazWeb.Controllers
{
    public class ConfiguracionController : Controller
    {
        [HttpPost]
        [SeguridadSession]
        public ActionResult Bandeja()
        {
            return View();
        }

        [HttpPost]
        [SeguridadSession]
        public ActionResult BandejaCV()
        {
            return View();
        }


        #region Detalle1
        [HttpPost]
        [SeguridadSession]
        public ActionResult Detalle1(int iIdCondicion)
        {

            try
            {

                var oCondicion = new EnCondicion();
                using (IwsCondicionClient oIwsDescripcion = new IwsCondicionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsDescripcion.InnerChannel);
                    oCondicion = oIwsDescripcion.WsObtenerCondicionId(iIdCondicion);
                }

              
                if (oCondicion.iTipo == 1)
                {
                    return View("Detalle1", oCondicion);
                }
                if (oCondicion.iTipo == 2)
                {
                    return View("Detalle2", oCondicion);
                }
                else
                      {
                          return View("Condicion CV", oCondicion);
                      }

            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB , UtlConstantes.LogNamespace_OperacionesTopazWEB ,
                    this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                    ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }


        }
        #endregion


        #region DetalleCreditoVencidos
        [HttpPost]
        [SeguridadSession]
        public ActionResult DetalleCreditoVencidos(int iIdCondicion)
        {
            try
            {

                var oCondicion = new EnCondicion();
                using (IwsCondicionClient oIwsDescripcion = new IwsCondicionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsDescripcion.InnerChannel);
                    oCondicion = oIwsDescripcion.WsObtenerCondicionId_CV(iIdCondicion);
                }
                
                if (oCondicion.iTipo == 1)
                {
                    return View("DetalleCreditoVencidos", oCondicion);
                }
                if (oCondicion.iTipo == 2)
                {
                    return View("BandejaCV", oCondicion);
                }
                else
                {
                    return View("Condicion CV", oCondicion);
                }

            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                    this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                    ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }


        }
        #endregion


        #region ListarCondicionesParametros1
        [HttpPost]
        [SeguridadSessionAjax]
        public ActionResult ListarCondiciones(EnCondicion oCondicion)
        {
            var objResultado = new object();
            var lsCuadros = new List<EnCondicion>();
            try
            {

                using (IwsCondicionClient oIwsDescripcion = new IwsCondicionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsDescripcion.InnerChannel);
                    lsCuadros = oIwsDescripcion.WsListarCondiciones(oCondicion).ToList();
                }

                int iTotalRegistros = 0;
                if (lsCuadros.Count > 0)
                {
                    iTotalRegistros = lsCuadros[0].TotalRegistros;
                }

                objResultado = new
                {
                    PageStart = oCondicion.NumeroPagina,
                    pageSize = oCondicion.TotalRegistros,
                    SearchText = string.Empty,
                    ShowChildren = true,
                    iTotalRecords = lsCuadros.Count(),
                    iTotalDisplayRecords = iTotalRegistros,
                    aaData = lsCuadros
                };
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                    this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                    ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return Json(objResultado);
        }
        #endregion


        #region ListarCondicionesCreditosVencidosParametros2
        [HttpPost]
        [SeguridadSessionAjax]
        public ActionResult ListarCondicionesCreditosVencidos(EnCondicion oCondicion)
        {
            var objResultado = new object();
            var lsCuadros = new List<EnCondicion>();
            try
            {

                using (IwsCondicionClient oIwsDescripcion = new IwsCondicionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsDescripcion.InnerChannel);
                    lsCuadros = oIwsDescripcion.WsListarCondicionesCreditosVencidos(oCondicion).ToList();
                }

                int iTotalRegistros = 0;
                if (lsCuadros.Count > 0)
                {
                    iTotalRegistros = lsCuadros[0].TotalRegistros;
                }

                objResultado = new
                {
                    PageStart = oCondicion.NumeroPagina,
                    pageSize = oCondicion.TotalRegistros,
                    SearchText = string.Empty,
                    ShowChildren = true,
                    iTotalRecords = lsCuadros.Count(),
                    iTotalDisplayRecords = iTotalRegistros,
                    aaData = lsCuadros
                };
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                    this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                    ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return Json(objResultado);
        }
        #endregion


        #region ListarProductosCondicion
        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult ListarProductosCondicion(int idCuadro, int iiActivo)
        {
            var objResultado = new object();
            try
            {
                List<EnProducto> lEnProducto = new List<EnProducto>();
                EnProducto oEnProducto = new EnProducto();
                oEnProducto.iIdCuadro = idCuadro;
                oEnProducto.iActivo = iiActivo;

                using (IwsCondicionClient oIwsProducto = new IwsCondicionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsProducto.InnerChannel);
                    lEnProducto = oIwsProducto.WsListarProductoCondicion(oEnProducto).ToList();
                }

                int iTotalRegistros = 0;
                if (lEnProducto.Count != 0)
                {
                    iTotalRegistros = lEnProducto[0].iTotalRegistros;
                }

                objResultado = new
                {
                    SearchText = string.Empty,
                    ShowChildren = true,
                    iTotalRecords = lEnProducto.Count(),
                    iTotalDisplayRecords = iTotalRegistros,
                    aaData = lEnProducto
                };
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB , UtlConstantes.LogNamespace_OperacionesTopazWEB ,
                    this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                    ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return Json(objResultado);
        }
        #endregion


        #region ListaProductoCondicion
        [HttpPost]
        [SeguridadSessionAjax]
        public async Task<JsonResult> MantenerProductoCondicion(List<Int64> ploProductos, string psDescripcion, int piCodigoCartera, short psiTipoOperacion)
        {
            var objResultado = new object();

            var datosUser = "";
            var iResultado = 0;
            try
            {
                datosUser = string.Format("{0}|{1}|{2}", UtlAuditoria.ObtenerNombreUsuario(),
                    UtlAuditoria.ObtenerDireccionIP(), UtlAuditoria.ObtenerDireccionMAC());

                using (IwsCondicionClient oIwsCartera = new IwsCondicionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsCartera.InnerChannel);
                    iResultado = oIwsCartera.WsMantenerProductoCondicion(ploProductos.ToArray(), datosUser, piCodigoCartera);
                }

                objResultado = new
                {
                    iResultado = iResultado
                };
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB , UtlConstantes.LogNamespace_OperacionesTopazWEB ,
                    this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                    ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return Json(objResultado);
        }
        #endregion


        #region ListaCondicionesProductosCreditosVencidos
        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult ListarProductosCondicionCreditoVencidos(int idCuadro, int iiActivo)
        {
            var objResultado = new object();
            try
            {
                List<EnProducto> lEnProducto = new List<EnProducto>();
                EnProducto oEnProducto = new EnProducto();
                oEnProducto.iIdCuadro = idCuadro;
                oEnProducto.iActivo = iiActivo;

                using (IwsCondicionClient oIwsProducto = new IwsCondicionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsProducto.InnerChannel);
                    lEnProducto = oIwsProducto.WsListarProductoCondicionCreditoVencidos(oEnProducto).ToList();
                }

                int iTotalRegistros = 0;
                if (lEnProducto.Count != 0)
                {
                    iTotalRegistros = lEnProducto[0].iTotalRegistros;
                }

                objResultado = new
                {
                    SearchText = string.Empty,
                    ShowChildren = true,
                    iTotalRecords = lEnProducto.Count(),
                    iTotalDisplayRecords = iTotalRegistros,
                    aaData = lEnProducto
                };
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                    this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                    ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return Json(objResultado);
        }
        #endregion


        #region ListadoCreditosVencidos
        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult ListarProductosCondicionCreditoVencidosAhorro(int idCuadro, int iiActivo)
        {
            var objResultado = new object();
            try
            {
                List<EnProducto> lEnProducto = new List<EnProducto>();
                EnProducto oEnProducto = new EnProducto();
                oEnProducto.iIdCuadro = idCuadro;
                oEnProducto.iActivo = iiActivo;

                using (IwsCondicionClient oIwsProducto = new IwsCondicionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsProducto.InnerChannel);
                    lEnProducto = oIwsProducto.WsListarProductoCondicionCreditoVencidosAhorro(oEnProducto).ToList();
                }

                int iTotalRegistros = 0;
                if (lEnProducto.Count != 0)
                {
                    iTotalRegistros = lEnProducto[0].iTotalRegistros;
                }

                objResultado = new
                {
                    SearchText = string.Empty,
                    ShowChildren = true,
                    iTotalRecords = lEnProducto.Count(),
                    iTotalDisplayRecords = iTotalRegistros,
                    aaData = lEnProducto
                };
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                    this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                    ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return Json(objResultado);
        }
        #endregion


        #region ListaCondicionesProductosAhorro
        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult ListarProductosCondicionProductoAhorroOrden(int idCuadro, int iiActivo)
        {
            var objResultado = new object();
            try
            {
                List<EnCarteraProductoAhorroOrden> lEnProducto = new List<EnCarteraProductoAhorroOrden>();
                EnCarteraProductoAhorroOrden oEnProducto = new EnCarteraProductoAhorroOrden();
                oEnProducto.iIdCuadro = idCuadro;
                oEnProducto.iActivo = iiActivo;

                using (IwsCondicionClient oIwsProducto = new IwsCondicionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsProducto.InnerChannel);
                    lEnProducto = oIwsProducto.WsListarProductoCondicionProductoAhorrosOrden(oEnProducto).ToList();
                }
 
                objResultado = new
                {
                    SearchText = string.Empty,
                    ShowChildren = true,
                    iTotalRecords = lEnProducto.Count(),
              
                    aaData = lEnProducto
                };
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                    this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                    ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return Json(objResultado);
        }
        #endregion


        #region MantenerProductoCondicionCreditoVencido
        [HttpPost]
        [SeguridadSessionAjax]
        public async Task<JsonResult> MantenerProductoCondicionCreditoVencido(List<Int64> ploProductos, List<Int64> ploProductosAhorro, string psDescripcion, int piCodigoCartera, short psiTipoOperacion , int DiasAtraso,decimal MontoMinimo)
        {
            var objResultado = new object();

            var datosUser = "";
            var iResultado = 0;
            try
            {
                datosUser = string.Format("{0}|{1}|{2}", UtlAuditoria.ObtenerNombreUsuario(),
                    UtlAuditoria.ObtenerDireccionIP(), UtlAuditoria.ObtenerDireccionMAC());

                using (IwsCondicionClient oIwsCartera = new IwsCondicionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsCartera.InnerChannel);
                    iResultado = oIwsCartera.WsMantenerProductoCondicionCreditoVencido(ploProductos.ToArray(), ploProductosAhorro.ToArray() , datosUser, piCodigoCartera, DiasAtraso , MontoMinimo);
                }

                objResultado = new
                {
                    iResultado = iResultado
                };
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                    this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                    ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return Json(objResultado);
        }
        #endregion


        #region GuardarOrdenProductoAhorro
        [HttpPost]
        [SeguridadSessionAjax]
        public async Task<JsonResult> GuardarOrdenProductoAhorro( List<EnCarteraProductoAhorroOrden> loProductoAhorro, string psDescripcion, int piCodigoCartera, short psiTipoOperacion)
        {
            var objResultado = new object();
            EnActionProductoAhorro oenListaResult = new EnActionProductoAhorro();


            var datosUser = "";
            var iResultado = 0;
            try
            {
                datosUser = string.Format("{0}|{1}|{2}", UtlAuditoria.ObtenerNombreUsuario(),
                    UtlAuditoria.ObtenerDireccionIP(), UtlAuditoria.ObtenerDireccionMAC());

                using (IwsCondicionClient oIwsCartera = new IwsCondicionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsCartera.InnerChannel);
                    iResultado = oIwsCartera.WsGuardarOrdenProductoAhorro(loProductoAhorro.ToArray(), datosUser, piCodigoCartera);
                }

                objResultado = new
                {
                    iResultado = iResultado
                };
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                    this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                    ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return Json(objResultado);
        }
        #endregion


        #region ListaProductoAhorro
        [HttpPost] 
        public PartialViewResult ListaProductoAhorro()
        {
            return PartialView("ListaProductoAhorro");
            //return PartialView();
        }
        #endregion
    }
}

 
