using OperacionesTopazWeb.Filters;
using OperacionesTopazWeb.wsCondicion;
using OperacionesTopazWeb.wsGestion;
using SeguridadCrkWCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
//using System.Web.Http;
using System.Web.Mvc;
using UtilitarioCrk;
//using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using System.Web;

namespace OperacionesTopazWeb.Controllers
{
    public class ReportesController : Controller           
    {
        private const string sSFileBytesPagos = "fileBytes";
        private const string sSFileBytesPagosAM = "fileBytesAM";

        [HttpPost]
        [SeguridadSession]
        public ActionResult Pago()
        {
            return View("Pago");
        }

        [HttpPost]
        [SeguridadSession]
        public ActionResult AfectacionesMasivas()
        {
            return View("AfectacionesMasivas");
        }

        #region ListarPagoReporte
        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult ListarPago(EnCorresponsaliaParametros Parametros, string fechaInicio, string fechaFin)
        {
            var objResultado = new object();
            try
            {
                List<EnCorresponsaliaDescargarPagos> lEnProducto = new List<EnCorresponsaliaDescargarPagos>();
 

                    using (IwsGestionClient oIwsProducto = new IwsGestionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsProducto.InnerChannel);
                    lEnProducto = oIwsProducto.WsListarPago(Parametros, fechaInicio, fechaFin).ToList();
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



        #region ExportarListaPago
        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult Exportar()
        {
            var oResultado = new object();
            byte[] fileBytes = null;
            Session[sSFileBytesPagos] = null;

            try
            {
                using (IwsGestionClient oIwsParametroClient = new IwsGestionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsParametroClient.InnerChannel);
                    fileBytes = oIwsParametroClient.wsExportarPagos();
                }

                if (fileBytes == null)          //  CUANDO NO HAY DATOS
                {
                    oResultado = new
                    {
                        iTipoResultado = 2,
                        sResultado = ""
                    };

                    Session[sSFileBytesPagos] = null;
                }
                else
                {
                    oResultado = new
                    {
                        iTipoResultado = 1,
                        sResultado = Convert.ToBase64String(fileBytes)
                    };
                    Session[sSFileBytesPagos] = fileBytes;
                }
            }

            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                                this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                                ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            var JsonResult = Json(oResultado);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;
        }
        #endregion



        #region DescargarExcelListaPago
        [SeguridadSessionAjax]
        public FileResult DownloadReportePagos()
        {

            string fileName = string.Empty;
            byte[] fileBytes = null;
            string sMascaraTiempo = "_yyyy_MM_dd_HH_mm_ss";
            try
            {
                fileName = "ReportesListaPagos" + DateTime.Now.ToString(sMascaraTiempo) + ".xls";
              
                fileBytes = (byte[])Session[sSFileBytesPagos];
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                                this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                                ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }
        #endregion

        #region ListarAfectacionesMasivasCabecera
        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult ReporteAfectacionesMasivasCabecera(EnAfectacionesMasivasReporteCabecera oParametros)
        {
            var objResultado = new object();
            int iTotalRegistros = 0;


            try
            {                
                List<EnAfectacionesMasivasReporteCabecera> lAfectacionesMasivasReportes = new List<EnAfectacionesMasivasReporteCabecera>();


                using (IwsGestionClient oIwsGestion = new IwsGestionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsGestion.InnerChannel);
                    lAfectacionesMasivasReportes = oIwsGestion.WsReporteAfectacionesMasivasCabecera(oParametros).ToList();
                }

                if (lAfectacionesMasivasReportes.Count != 0)
                {
                    iTotalRegistros = lAfectacionesMasivasReportes[0].TotalRegistros;
                }

                objResultado = new
                {
                    PageStart = oParametros.NumeroPagina,
                    PageSize = oParametros.NumeroRegistros,
                    SearchText = string.Empty,
                    ShowChildren = true,
                    iTotalRecords = lAfectacionesMasivasReportes.Count(),
                    iTotalDisplayRecords = iTotalRegistros,
                    aaData = lAfectacionesMasivasReportes,
                    success = true
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

        #region ExportarAfectacionesMasivas
        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult ExportarAfectacionesMasivas(EnAfectacionesMasivasReporteCabecera oParametros)
        {
            var oResultado = new object();
            byte[] fileBytes = null;
            Session[sSFileBytesPagosAM] = null;

            try
            {
                using (IwsGestionClient oIwsParametroClient = new IwsGestionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsParametroClient.InnerChannel);
                    fileBytes = oIwsParametroClient.wsExportarAfectacionesMasivas(oParametros);
                }

                if (fileBytes == null)          //  CUANDO NO HAY DATOS
                {
                    oResultado = new
                    {
                        iTipoResultado = 2,
                        sResultado = ""
                    };

                    Session[sSFileBytesPagosAM] = null;
                }
                else
                {
                    oResultado = new
                    {
                        iTipoResultado = 1,
                        sResultado = Convert.ToBase64String(fileBytes)
                    };
                    Session[sSFileBytesPagosAM] = fileBytes;
                }
            }

            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                                this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                                ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            var JsonResult = Json(oResultado);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;
        }
        #endregion

        #region DescargarExcelListarAfectacionesMasivas
        [SeguridadSessionAjax]
        public FileResult DownloadReporteAfectacionesMasivas()
        {

            string fileName = string.Empty;
            byte[] fileBytes = null;
            string sMascaraTiempo = "_yyyy_MM_dd_HH_mm_ss";
            try
            {
                fileName = "ReporteAfectacionesMasivas" + DateTime.Now.ToString(sMascaraTiempo) + ".xls";

                fileBytes = (byte[])Session[sSFileBytesPagosAM];
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                                this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                                ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }
        #endregion
    }

}