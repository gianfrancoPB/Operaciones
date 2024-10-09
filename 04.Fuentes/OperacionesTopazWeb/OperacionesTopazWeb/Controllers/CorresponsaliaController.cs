using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguridadCrkWCF;
using UtilitarioCrk;
using System.Xml;
using System.Reflection;
using OperacionesTopazWeb.Filters;
using OperacionesTopazWeb.wsGestion;
using System.Text;
using System.IO;
using System.Globalization;
using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Configuration;


namespace OperacionesTopazWeb.Controllers
{
    public class CorresponsaliaController : Controller
    {
        static string        sSessionArchivoExportarRuta     = "sSessionArchivoExportarRuta";
        static string        sSessionArchivoExportarNombre   = "sSessionArchivoExportarNombre";

        static string        sSessionDocumento  = "sSessionDocumento";
        static string        sSessionBytes      = "sSessionBytes";
        static string        sSessionBytesPagos = "sSessionBytes";
        private const string sSFileBytes        = "fileBytes";
        private const string sSFileBytesCV      = "fileBytes";
        private const string sSFileBytesPagos   = "fileBytes";
        [SeguridadIngreso]
        public ActionResult Index(string id)
        {
            return View();
        }

        [HttpPost]
        [SeguridadSession]
        public ActionResult BancoDeCredito()
        {
            return View();
        }

        [HttpPost]
        [SeguridadSession]
        public ActionResult CajaArequipa()
        {
            return View();
        }

        [HttpPost]
        [SeguridadSession]
        public ActionResult CreditosVencidos()
        {
            return View();
        }

        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult GeneracionDeudaCV(int iIAccion)
        {
            string vMensaje = string.Empty;
            var objResultado = new object();

            EnCorresponsalia oEnCorresponsalia = new EnCorresponsalia();
            EnCorresponsaliaParametros Parametros = new EnCorresponsaliaParametros();


            int IdUsuario = UtlAuditoria.ObtenerIdUsuario();
            string vAudNombreUsuarioCreacion = UtlAuditoria.ObtenerNombreUsuario();
            string vAudIPCreacion = UtlAuditoria.ObtenerDireccionIP();
            string vAudMACCreacion = UtlAuditoria.ObtenerDireccionMAC();
            int iIdOficina = Convert.ToInt32(UtlAuditoria.ObtenerIdOficina());
            Parametros.iIAccion = iIAccion;
            // Obteniendo valores de Auditoria
            Parametros.iIdUsuario = IdUsuario;
            Parametros.sAudNombreUsuarioCreacion = vAudNombreUsuarioCreacion;
            Parametros.sAudIPCreacion = vAudIPCreacion;
            Parametros.sAudMACCreacion = vAudMACCreacion;
            Parametros.iCodigoOficinaUsuario = iIdOficina;

            try
            {                 
                using (IwsGestionClient oIwsGestion = new IwsGestionClient())
                {                    
                    AuthenticationInspectorBehavior.CrearConexion(oIwsGestion.InnerChannel);
                    oEnCorresponsalia = oIwsGestion.wsGeneracionDeudaCV(Parametros);
                    if (oEnCorresponsalia.iAccion == 1)
                    {
                        vMensaje = oEnCorresponsalia.sDescripcionAccion;

                        objResultado = new
                        {
                            oEnCorresponsalia = oEnCorresponsalia,
                            iError = 0,
                            sMensaje = vMensaje
                        };
                    }
                    else
                    {
                        string sMensajeRetornar = string.Empty;

                        objResultado = new
                        {
                            oEnCorresponsalia = new EnCorresponsalia(),
                            iError = 1,
                            sMensaje = sMensajeRetornar
                        };
                    }
                }
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

        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult GeneracionDeuda(int iIAccion)
        {
            string vMensaje = string.Empty;
            var objResultado = new object();

            EnCorresponsalia oEnCorresponsalia = new EnCorresponsalia();
            EnCorresponsaliaParametros Parametros = new EnCorresponsaliaParametros();


            int    IdUsuario                    = UtlAuditoria.ObtenerIdUsuario();
            string vAudNombreUsuarioCreacion    = UtlAuditoria.ObtenerNombreUsuario();
            string vAudIPCreacion               = UtlAuditoria.ObtenerDireccionIP();
            string vAudMACCreacion              = UtlAuditoria.ObtenerDireccionMAC();
            int    iIdOficina                   = Convert.ToInt32(UtlAuditoria.ObtenerIdOficina());
            Parametros.iIAccion                 = iIAccion;
            // Obteniendo valores de Auditoria
            Parametros.iIdUsuario                = IdUsuario;
            Parametros.sAudNombreUsuarioCreacion = vAudNombreUsuarioCreacion;
            Parametros.sAudIPCreacion            = vAudIPCreacion;
            Parametros.sAudMACCreacion           = vAudMACCreacion;
            Parametros.iCodigoOficinaUsuario     = iIdOficina;


            try
            {
                using (IwsGestionClient oIwsGestion = new IwsGestionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsGestion.InnerChannel);
                    oEnCorresponsalia = oIwsGestion.wsGeneracionDeuda(Parametros);
                    if (oEnCorresponsalia.iAccion == 1 || oEnCorresponsalia.iAccion == 2)
                    {
                        vMensaje = oEnCorresponsalia.sDescripcionAccion;

                        objResultado = new
                        {
                            oEnCorresponsalia = oEnCorresponsalia,
                            iError = 0,
                            sMensaje = vMensaje
                        };
                    }
                    else
                    {
                        string sMensajeRetornar = string.Empty;

                        objResultado = new
                        {
                            oEnCorresponsalia = new EnCorresponsalia(),
                            iError = 1,
                            sMensaje = sMensajeRetornar
                        };
                    }
                }
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

        [HttpPost]
        [SeguridadSessionAjax]
        public ActionResult SubirArchivosPagos(HttpPostedFileBase file)
        {
            EnCorresponsalia oEnCorresponsalia = new EnCorresponsalia();
            var objResultado = new object();

            try
            {

                if (file != null)
                { 
                    var sRutaTemp = ConfigurationManager.AppSettings["RutaFileServer2"];
                    

                    string vNombre = Path.GetFileName(file.FileName);
                   string Carpetapath = Path.Combine(sRutaTemp, vNombre);


                    #region File
                    //  ##  01  ::  Valida si no Existe el directorio ( De ser asi lo crea ) 
                    if (!Directory.Exists(sRutaTemp)) 
                        Directory.CreateDirectory(sRutaTemp);

                    //  ##  02  ::  Elimina el archivo si en caso existiera
                    if (System.IO.File.Exists(Carpetapath))
                        System.IO.File.Delete(Carpetapath);

                    //  ##  03  ::  Crea el archivo con la informacion del File Input
                    file.SaveAs(Carpetapath);
                    #endregion
                    Session[sSessionArchivoExportarNombre] = null;
                    //CargarDocumento(iaccion);
                    Session[sSessionArchivoExportarNombre] = file.FileName;
                }

            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.LogNamespace_SAUWEB, UtlConstantes.LogNamespace_SAUWEB, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }

            return Json(objResultado);
        }

        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult GeneracionPagos(int iIAccion)
        {

            //metodo que deveulva la ruta y archivo desde la bd 
           var sRutaTemp = ConfigurationManager.AppSettings["RutaFileServer2"];

     
            string vNombre = Session[sSessionArchivoExportarNombre].ToString();

            string vRuta = Path.Combine(sRutaTemp );


            string vMensaje = string.Empty;
            var objResultado = new object();

            EnCorresponsalia oEnCorresponsalia = new EnCorresponsalia();
            EnCorresponsaliaParametros Parametros = new EnCorresponsaliaParametros();


            int IdUsuario = UtlAuditoria.ObtenerIdUsuario();
            string vAudNombreUsuarioCreacion = UtlAuditoria.ObtenerNombreUsuario();
            string vAudIPCreacion = UtlAuditoria.ObtenerDireccionIP();
            string vAudMACCreacion = UtlAuditoria.ObtenerDireccionMAC();
            int iIdOficina = Convert.ToInt32(UtlAuditoria.ObtenerIdOficina());
            Parametros.iIAccion = iIAccion;
            Parametros.iIdUsuario = IdUsuario;
            Parametros.sAudNombreUsuarioCreacion = vAudNombreUsuarioCreacion;
            Parametros.sAudIPCreacion = vAudIPCreacion;
            Parametros.sAudMACCreacion = vAudMACCreacion;
            Parametros.iCodigoOficinaUsuario = iIdOficina;



            try
            {
                using (IwsGestionClient oIwsGestion = new IwsGestionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsGestion.InnerChannel);

                    oEnCorresponsalia = oIwsGestion.wsGeneracionPagos(vRuta, vNombre, Parametros);
                    objResultado = new
                    {
                        aData = oEnCorresponsalia
                    };
                }
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

        #region CargarDocumentoTxt
        [HttpPost]
        [SeguridadSession]
        public ActionResult CargarDocumento(HttpPostedFileBase file)
        {

            Dictionary<string, byte[]> dcArchivoBytes = new Dictionary<string, byte[]>();
          
            string dcArchivoBytesKeyAnterior = "";

            ///////////////


            string sMascaraTiempo = "_yyyyMMddHHmmss";


            string[] sFileArchivos  = file.FileName.Split('.');
            var sFileNameArchivo    = sFileArchivos[0];
            var sFileNameExtencion  = sFileArchivos[1];

            string sNombreArchioFinal = sFileNameArchivo + DateTime.Now.ToString(sMascaraTiempo) + "." +    sFileNameExtencion;


            ///////////////

            byte[] imageByte = null;
            BinaryReader rdr = new BinaryReader(file.InputStream);
            imageByte = rdr.ReadBytes((int)file.ContentLength);


            var iTotalDocumentos = 0;    ////// VARIABLE INICIAL
            var iTotalArchivoBytes = 0;


            try
            {
                dcArchivoBytes.Add(sNombreArchioFinal, imageByte);

                Session[sSessionBytes] = dcArchivoBytes;

                ////////// AÑADIENDO UN CONTADOR DE SESSIONES
                iTotalArchivoBytes = dcArchivoBytes.Count();

                ///////////////////////////////////////////////////////////////////////////////////
                if (Session[sSessionDocumento] != null)
                {
                     
                }
                ///////////////////////////////////////////////////////////////////////////////////
                if (iTotalArchivoBytes == (iTotalDocumentos + 2))
                {

                    dcArchivoBytes.Remove(dcArchivoBytesKeyAnterior);
                }

            }
            catch (System.Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                                this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                                ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return Json(string.Empty);
        }
        #endregion

        #region ExportarBytesPagos
        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult Exportar_Pagos()
        {
            var oResultado = new object();
            byte[] fileBytes = null;
            Session[sSFileBytesPagos] = null;

            try
            {
                using (IwsGestionClient oIwsParametroClient = new IwsGestionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsParametroClient.InnerChannel);
                    fileBytes = oIwsParametroClient.wsObtenerPagos();
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

        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult Exportar_PagosCA(int iIdPagosCA)
        {
            var oResultado = new object();
            byte[] fileBytes = null;
            Session[sSFileBytesPagos] = null;

            try
            {
                using (IwsGestionClient oIwsParametroClient = new IwsGestionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsParametroClient.InnerChannel);
                    fileBytes = oIwsParametroClient.wsObtenerPagosCA(iIdPagosCA);
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

        #region ExportarBytesPagoCV
        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult Exportar_PagosCV()
        {
            var oResultado = new object();
            byte[] fileBytes = null;
            Session[sSFileBytesCV] = null;

            try
            {
                using (IwsGestionClient oIwsParametroClient = new IwsGestionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsParametroClient.InnerChannel);
                    fileBytes = oIwsParametroClient.wsObtenerPagosCV();
                }

                if (fileBytes == null)          //  CUANDO NO HAY DATOS
                {
                    oResultado = new
                    {
                        iTipoResultado = 2,
                        sResultado = ""
                    };

                    Session[sSFileBytesCV] = null;
                }
                else
                {
                    oResultado = new
                    {
                        iTipoResultado = 1,
                        sResultado = Convert.ToBase64String(fileBytes)
                    };
                    Session[sSFileBytesCV] = fileBytes;
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

        #region ExportarArchivoTxtBytes
        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult Exportar_Archivos(int piiFlag)
        {
            var     oResultado      = new object();
            byte    [] fileBytes    = null;
            Session [sSFileBytes]   = null;

            try
            {
                using (IwsGestionClient oIwsParametroClient = new IwsGestionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsParametroClient.InnerChannel);
                    fileBytes = oIwsParametroClient.wsObtenerArchivo(piiFlag);
                }

                if (fileBytes == null)          //  CUANDO NO HAY DATOS
                {
                    oResultado = new
                    {
                        iTipoResultado = 2,
                        sResultado = ""
                    };

                    Session[sSFileBytes] = null;
                }
                else
                {
                    oResultado = new
                    {
                        iTipoResultado = 1,
                        sResultado = Convert.ToBase64String(fileBytes)
                    };
                    Session[sSFileBytes] = fileBytes;
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

        #region DescargaExcelPagos
        [SeguridadSessionAjax]
        public FileResult DownloadPagos(int piFlag)
        {

            string fileName = string.Empty;
            byte[] fileBytes = null;
            string sMascaraTiempo = "_yyyy_MM_dd_HH_mm_ss";
            try
            {
                fileName = "AMORTIZACIONES_MASIVAS" + DateTime.Now.ToString(sMascaraTiempo) + ".xls";
              
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

        #region DescargaExcelAfectaciones
        [SeguridadSessionAjax]
        public FileResult DownloadPagosCV(int piFlag)
        {

            string fileName = string.Empty;
            byte[] fileBytes = null;
            string sMascaraTiempo = "_yyyy_MM_dd_HH_mm_ss";
            try
            {
                //Modificado 1 Septiembre 2022
                //fileName = "AMORTIZACIONES_MASIVAS" + DateTime.Now.ToString(sMascaraTiempo) + ".xlsx";
                fileName = "AMORTIZACIONES_MASIVAS_AFECTACIONES" + DateTime.Now.ToString(sMascaraTiempo) + ".xls";
            
                //Modificado 1 Septiembre 2022
                fileBytes = (byte[])Session[sSFileBytesCV];
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

        #region DescargaArchivoTxt
        [SeguridadSessionAjax]
        public FileResult Download(int piFlag)
        {

            string  fileName = string.Empty;
            byte[]  fileBytes    = null;
            try
            {
                switch (piFlag)
                {
                    case 1:
                        fileName = "CREP1.txt";
                        break;
                    case 2:
                        fileName = "CREP2.txt";
                        break;
                }

                fileBytes = (byte[])Session[sSFileBytes];
            }
            catch(Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                                this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                                ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }
        #endregion

        #region Caja Arequipa
        [SeguridadSessionAjax]
        public FileResult DownloadDeudasCA()
        {

            EnExportarDeudasCA oResultado = new EnExportarDeudasCA();
            try
            {
                using (IwsGestionClient oIwsGestion = new IwsGestionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsGestion.InnerChannel);

                    oResultado = oIwsGestion.wsExportarDeudasCA();
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazWEB, UtlConstantes.LogNamespace_OperacionesTopazWEB,
                                this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                                ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }

            return File(oResultado.bFileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, oResultado.sFileName);

        }

        [HttpPost]
        [SeguridadSessionAjax]
        public ActionResult SubirArchivosPagosCA(HttpPostedFileBase file)
        {
            EnCorresponsalia oEnCorresponsalia = new EnCorresponsalia();
            var objResultado = new object();

            try
            {

                if (file != null)
                {
                    var sRutaTemp = ConfigurationManager.AppSettings["RutaFileServer2"];
                    sRutaTemp = Path.Combine(sRutaTemp, "CajaArequipa");


                    string vNombre = Path.GetFileName(file.FileName);
                    string Carpetapath = Path.Combine(sRutaTemp, vNombre);


                    #region File
                    //  ##  01  ::  Valida si no Existe el directorio ( De ser asi lo crea ) 
                    if (!Directory.Exists(sRutaTemp))
                        Directory.CreateDirectory(sRutaTemp);

                    //  ##  02  ::  Elimina el archivo si en caso existiera
                    if (System.IO.File.Exists(Carpetapath))
                        System.IO.File.Delete(Carpetapath);

                    //  ##  03  ::  Crea el archivo con la informacion del File Input
                    file.SaveAs(Carpetapath);
                    #endregion
                    Session[sSessionArchivoExportarNombre] = null;
                    //CargarDocumento(iaccion);
                    Session[sSessionArchivoExportarNombre] = file.FileName;
                }

            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.LogNamespace_SAUWEB, UtlConstantes.LogNamespace_SAUWEB, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }

            return Json(objResultado);
        }

        [HttpPost]
        [SeguridadSessionAjax]
        public JsonResult GeneracionPagosCA(int iIAccion)
        {

            //metodo que deveulva la ruta y archivo desde la bd 
            var sRutaTemp = ConfigurationManager.AppSettings["RutaFileServer2"];
            sRutaTemp = Path.Combine(sRutaTemp, "CajaArequipa\\");

            string vNombre = Session[sSessionArchivoExportarNombre].ToString();

            string vRuta = Path.Combine(sRutaTemp);


            string vMensaje = string.Empty;
            var objResultado = new object();

            EnCorresponsalia oEnCorresponsalia = new EnCorresponsalia();
            EnCorresponsaliaParametros Parametros = new EnCorresponsaliaParametros();


            int IdUsuario = UtlAuditoria.ObtenerIdUsuario();
            string vAudNombreUsuarioCreacion = UtlAuditoria.ObtenerNombreUsuario();
            string vAudIPCreacion = UtlAuditoria.ObtenerDireccionIP();
            string vAudMACCreacion = UtlAuditoria.ObtenerDireccionMAC();
            int iIdOficina = Convert.ToInt32(UtlAuditoria.ObtenerIdOficina());
            Parametros.iIAccion = iIAccion;
            Parametros.iIdUsuario = IdUsuario;
            Parametros.sAudNombreUsuarioCreacion = vAudNombreUsuarioCreacion;
            Parametros.sAudIPCreacion = vAudIPCreacion;
            Parametros.sAudMACCreacion = vAudMACCreacion;
            Parametros.iCodigoOficinaUsuario = iIdOficina;



            try
            {
                using (IwsGestionClient oIwsGestion = new IwsGestionClient())
                {
                    AuthenticationInspectorBehavior.CrearConexion(oIwsGestion.InnerChannel);

                    oEnCorresponsalia = oIwsGestion.wsGeneracionPagosCA(vRuta, vNombre, Parametros);
                    objResultado = new
                    {
                        aData = oEnCorresponsalia
                    };
                }
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
    }
}