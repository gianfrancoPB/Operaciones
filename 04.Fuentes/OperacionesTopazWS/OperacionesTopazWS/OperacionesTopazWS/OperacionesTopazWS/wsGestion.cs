using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperacionesTopazWS.Interfaces;
using SatelitesEN;
using OperacionesTopazRN;
using SeguridadCrkWCF;
using UtilitarioCrk;
using System.Reflection;

namespace OperacionesTopazWS
{
    [AuthenticationInspectorBehavior]
    public class wsGestion : IwsGestion
    {
        #region GeneracionDeudaCreditosVencidos
        public EnCorresponsalia wsGeneracionDeudaCV(EnCorresponsaliaParametros Parametros)
        {
            EnCorresponsalia oCorresponsalia = new EnCorresponsalia();

            try
            {
                RnGestion owsGeneracionDeuda = new RnGestion();
                oCorresponsalia = owsGeneracionDeuda.rnGeneracionDeudaCV(Parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCorresponsalia;
        }
        #endregion

        #region GeneracionDeuda
        public EnCorresponsalia wsGeneracionDeuda(EnCorresponsaliaParametros Parametros)
        {
            EnCorresponsalia oCorresponsalia = new EnCorresponsalia();

            try
            {
                RnGestion owsGeneracionDeuda = new RnGestion();
                oCorresponsalia = owsGeneracionDeuda.rnGeneracionDeuda(Parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCorresponsalia;
        }
        #endregion

        #region GeneracionPagos
        public EnCorresponsalia wsGeneracionPagos(string vRuta, string vNombre, EnCorresponsaliaParametros Parametros)
        {
            EnCorresponsalia oCorresponsaliaPagos = new EnCorresponsalia();

            try
            {
                RnGestion owsGeneracionDeuda = new RnGestion();
                oCorresponsaliaPagos = owsGeneracionDeuda.rnGeneracionPagos(vRuta, vNombre, Parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCorresponsaliaPagos;
        }
        public EnCorresponsalia wsGeneracionPagosCA(string vRuta, string vNombre, EnCorresponsaliaParametros Parametros)
        {
            EnCorresponsalia oCorresponsaliaPagos = new EnCorresponsalia();

            try
            {
                RnGestion owsGeneracionDeuda = new RnGestion();
                oCorresponsaliaPagos = owsGeneracionDeuda.rnGeneracionPagosCA(vRuta, vNombre, Parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCorresponsaliaPagos;
        }
        #endregion

        #region ObtenerPagos 
        public byte [] wsObtenerPagos()
        {
            byte[] bPagos;

            try
            {
                RnGestion owsObtenerPagos = new RnGestion();
                bPagos = owsObtenerPagos.rnObtenerPagos();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return bPagos;
        }
        public byte[] wsObtenerPagosCA(int iIdPagosCA)
        {
            byte[] bPagos;

            try
            {
                RnGestion owsObtenerPagos = new RnGestion();
                bPagos = owsObtenerPagos.rnObtenerPagosCA(iIdPagosCA);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bPagos;
        }
        #endregion

        #region ObtenerPagosCV
        public byte[] wsObtenerPagosCV()
        {
            byte[] bPagos;

            try
            {
                RnGestion owsObtenerPagos = new RnGestion();
                bPagos = owsObtenerPagos.rnObtenerPagosCV();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bPagos;
        }
        #endregion

        #region ListarPagos
        public async Task<List<EnCorresponsaliaDescargarPagos>> WsListarPago(EnCorresponsaliaParametros Parametros, string fechaInicio, string fechaFin)
        {
            try
            {
                RnGestion ornReglasParametro = new RnGestion();
                return await ornReglasParametro.frn_ListarPago(Parametros, fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region Reporte - AfectacionesMasivas
        public List<EnAfectacionesMasivasReporteCabecera> WsReporteAfectacionesMasivasCabecera(EnAfectacionesMasivasReporteCabecera oParametros)
        {
            try
            {
                 RnGestion ornGestion = new RnGestion();
                return ornGestion.frn_ReporteAfectacionesMasivasCabecera(oParametros);
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion  

        #region ObtenerArchivo
        public byte[] wsObtenerArchivo(int piFlag)
        {
            byte[] bEnOficina = null;

            try
            {
                RnGestion ornReporte = new RnGestion();
                bEnOficina = ornReporte.rnObtenerArchivo(piFlag);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return bEnOficina;
        }
        #endregion

        #region ExportarPagos
        public byte[] wsExportarPagos()
        {
            byte[] bPagos;

            try
            {
                RnGestion owsObtenerPagos = new RnGestion();
                bPagos = owsObtenerPagos.rnExportarPagos();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bPagos;
        }
        #endregion

        #region ExportarPagos
        public byte[] wsExportarAfectacionesMasivas(EnAfectacionesMasivasReporteCabecera oParametros)
        {
            byte[] bAfectacionesMasivas;
            try
            {
                RnGestion rnGestion = new RnGestion();
                bAfectacionesMasivas = rnGestion.rnExportarAfectacionesMasivas(oParametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bAfectacionesMasivas;
        }
        #endregion

        #region Caja Arequipa
        public EnExportarDeudasCA wsExportarDeudasCA()
        { 

            EnExportarDeudasCA oResultado = new EnExportarDeudasCA();

            try
            {
                RnGestion owsGeneracionDeuda = new RnGestion();
                oResultado = owsGeneracionDeuda.rn_ExportarDeudasCA();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oResultado;
        }
        #endregion

    }
}
