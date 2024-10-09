using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using SatelitesEN;
using UtilitarioCrk;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace OperacionesTopazAD
{
    public class AdGestion : AdGeneral
    {
        public AdGestion(SqlConnection con)
        {
            conexion = con;
        }

        #region GeneracionDeudaCreditoVencidos
        public EnCorresponsalia adGeneracionDeudaCV(EnCorresponsaliaParametros Parametros)
        {
            EnCorresponsalia oGeneracionDeuda = new EnCorresponsalia();
  
            try
            {
                using (SqlCommand cmd = new SqlCommand("AFE_MAS.USP_AfectacionMasiva", conexion)) 
                {
                    cmd.CommandType = CommandType.StoredProcedure;
            
                    cmd.Parameters.AddWithValue("@piiIndicadorDepuracion", 0);             
                    cmd.Parameters.AddWithValue("@pivNombreUsuario", string.IsNullOrEmpty(Parametros.sAudNombreUsuarioCreacion) ? "" : Parametros.sAudNombreUsuarioCreacion);
                    cmd.Parameters.AddWithValue("@pivNombreEquipo", string.IsNullOrEmpty(Parametros.sAudMACCreacion) ? "" : Parametros.sAudMACCreacion);
                    cmd.Parameters.AddWithValue("@pivNumeroIP", string.IsNullOrEmpty(Parametros.sAudIPCreacion) ? "" : Parametros.sAudIPCreacion);
                    cmd.Parameters.AddWithValue("@poiIndicadorProceso", 0);
                    cmd.Parameters.AddWithValue("@povObservaciones", "");
                    cmd.CommandTimeout = 0;
                    using (SqlDataReader drd = cmd.ExecuteReader())
                    {
                        if (drd != null)
                        {
                            int pos_CantidadRegistros = drd.GetOrdinal("iCantidadRegistros");
                            int pos_Accion = drd.GetOrdinal("iAccion");
                            int pos_DescripcionAccion = drd.GetOrdinal("vDescripcionAccion");
                            int pos_Nombres = drd.GetOrdinal("vNombres");
                            //Agregado 29 Agosto 2022
                            int pos_FechaProceso = drd.GetOrdinal("vFechaProceso");
                            //Agregado 29 Agosto 2022

                            while (drd.Read())
                            {
                                oGeneracionDeuda.iCantidadRegistros = drd.IsDBNull(pos_CantidadRegistros) ? 0 : drd.GetInt32(pos_CantidadRegistros);
                                oGeneracionDeuda.iAccion = drd.IsDBNull(pos_Accion) ? 0 : drd.GetInt32(pos_Accion);
                                oGeneracionDeuda.sDescripcionAccion = drd.IsDBNull(pos_DescripcionAccion) ? string.Empty : drd.GetString(pos_DescripcionAccion);
                                oGeneracionDeuda.sNombresCompleto = drd.IsDBNull(pos_Nombres) ? string.Empty : drd.GetString(pos_Nombres);
                                //Agregado 29 Agosto 2022
                                oGeneracionDeuda.sFechaProceso = drd.IsDBNull(pos_FechaProceso) ? string.Empty : drd.GetString(pos_FechaProceso);
                                //Agregado 29 Agosto 2022
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return oGeneracionDeuda;
        }
        #endregion

        #region GeneracionDeuda
        public EnCorresponsalia adGeneracionDeuda(EnCorresponsaliaParametros Parametros)
        {
            EnCorresponsalia oGeneracionDeuda = new EnCorresponsalia();

            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_GenerarData", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@piIAccion", Parametros.iIAccion);
                    cmd.Parameters.AddWithValue("@piIdUsuario", Parametros.iIdUsuario);
                    cmd.Parameters.AddWithValue("@pvAudNombreUsuarioCreacion", string.IsNullOrEmpty(Parametros.sAudNombreUsuarioCreacion) ? "" : Parametros.sAudNombreUsuarioCreacion);
                    cmd.Parameters.AddWithValue("@pvAudIPCreacion",            string.IsNullOrEmpty(Parametros.sAudIPCreacion)            ? "" : Parametros.sAudIPCreacion);
                    cmd.Parameters.AddWithValue("@pvAudMACCreacion",           string.IsNullOrEmpty(Parametros.sAudMACCreacion)           ? "" : Parametros.sAudMACCreacion);
                    cmd.Parameters.AddWithValue("@piCodigoOficinaUsuario", Parametros.iCodigoOficinaUsuario);
                    cmd.CommandTimeout = 0;
                    using (SqlDataReader drd = cmd.ExecuteReader())
                    {
                        if (drd != null)
                        {
                            int pos_CantidadRegistros   = drd.GetOrdinal("iCantidadRegistros");
                            int pos_Accion              = drd.GetOrdinal("iAccion");
                            int pos_DescripcionAccion   = drd.GetOrdinal("vDescripcionAccion");
                            int pos_Nombres             = drd.GetOrdinal("vNombres");
                            int pos_Fecha            = drd.GetOrdinal("vFechaProceso1");


                            while (drd.Read())
                            {
                                oGeneracionDeuda.iCantidadRegistros = drd.IsDBNull(pos_CantidadRegistros) ? 0 : drd.GetInt32(pos_CantidadRegistros);
                                oGeneracionDeuda.iAccion            = drd.IsDBNull(pos_Accion)            ? 0 : drd.GetInt32(pos_Accion);
                                oGeneracionDeuda.sDescripcionAccion = drd.IsDBNull(pos_DescripcionAccion) ? string.Empty : drd.GetString(pos_DescripcionAccion);
                                oGeneracionDeuda.sNombresCompleto   = drd.IsDBNull(pos_Nombres)           ? string.Empty : drd.GetString(pos_Nombres);
                                oGeneracionDeuda.sFechaProceso      = drd.IsDBNull(pos_Fecha)             ? string.Empty : drd.GetString(pos_Fecha);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return oGeneracionDeuda;
        }
        #endregion

        #region ObtenerPagos
        public List<EnCorresponsaliaDescargarPagos> adObtenerPagos()
        {
            List<EnCorresponsaliaDescargarPagos> loObtenerPagos = new List<EnCorresponsaliaDescargarPagos>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_DescargarData", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader drd = cmd.ExecuteReader())
                    {
                        if (drd != null)
                        {
                            int pos_FECHA_PROCESO       = drd.GetOrdinal("vFechaProceso");
                            int pos_NUMERO_CREDITO      = drd.GetOrdinal("vNumeroCredito");
                            int pos_DOI                 = drd.GetOrdinal("vDoi");
                            int pos_MONEDA              = drd.GetOrdinal("vCodigoMoneda");
                            int pos_MONTO_APLICAR       = drd.GetOrdinal("nMontoAplicar");
                            int pos_RUBRO_CUENTA        = drd.GetOrdinal("vRubroCuenta");
                            int pos_TIPO_MOVIMIENTO     = drd.GetOrdinal("vTipoMovimiento");


                            while (drd.Read())
                            {
                                EnCorresponsaliaDescargarPagos  oObtenerPagos = new EnCorresponsaliaDescargarPagos();
                                oObtenerPagos.sFecha_Proceso    = drd.IsDBNull(pos_FECHA_PROCESO)   ? string.Empty : drd.GetString(pos_FECHA_PROCESO);
                                oObtenerPagos.sNumero_Credito   = drd.IsDBNull(pos_NUMERO_CREDITO)  ? string.Empty : drd.GetString(pos_NUMERO_CREDITO);
                                oObtenerPagos.sDOI              = drd.IsDBNull(pos_DOI)             ? string.Empty : drd.GetString(pos_DOI);
                                oObtenerPagos.sMoneda           = drd.IsDBNull(pos_MONEDA)          ? string.Empty : drd.GetString(pos_MONEDA);
                                oObtenerPagos.dMonto_Aplicar    = drd.IsDBNull(pos_MONTO_APLICAR)   ? 0            : drd.GetDecimal(pos_MONTO_APLICAR);
                                oObtenerPagos.sRubro_Cuenta     = drd.IsDBNull(pos_RUBRO_CUENTA)    ? string.Empty : drd.GetString(pos_RUBRO_CUENTA);
                                oObtenerPagos.sTipo_Movimiento  = drd.IsDBNull(pos_TIPO_MOVIMIENTO) ? string.Empty : drd.GetString(pos_TIPO_MOVIMIENTO);
                                loObtenerPagos.Add(oObtenerPagos);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return loObtenerPagos;
        }
        public List<EnCorresponsaliaDescargarPagos> adObtenerPagosCA(int iIdPagosCA)
        {
            List<EnCorresponsaliaDescargarPagos> loObtenerPagos = new List<EnCorresponsaliaDescargarPagos>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_DescargarDeudasCA", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@iIdPagosCA", iIdPagosCA);
                    cmd.CommandTimeout = 0;
                    using (SqlDataReader drd = cmd.ExecuteReader())
                    {
                        if (drd != null)
                        {
                            int pos_FECHA_PROCESO       = drd.GetOrdinal("vFechaProceso");
                            int pos_NUMERO_CREDITO      = drd.GetOrdinal("vNumeroCredito");
                            int pos_DOI                 = drd.GetOrdinal("vDoi");
                            int pos_MONEDA              = drd.GetOrdinal("vCodigoMoneda");
                            int pos_MONTO_APLICAR       = drd.GetOrdinal("nMontoAplicar");
                            int pos_RUBRO_CUENTA        = drd.GetOrdinal("vRubroCuenta");
                            int pos_TIPO_MOVIMIENTO     = drd.GetOrdinal("vTipoMovimiento");


                            while (drd.Read())
                            {
                                EnCorresponsaliaDescargarPagos  oObtenerPagos = new EnCorresponsaliaDescargarPagos();
                                oObtenerPagos.sFecha_Proceso    = drd.IsDBNull(pos_FECHA_PROCESO)   ? string.Empty : drd.GetString(pos_FECHA_PROCESO);
                                oObtenerPagos.sNumero_Credito   = drd.IsDBNull(pos_NUMERO_CREDITO)  ? string.Empty : drd.GetString(pos_NUMERO_CREDITO);
                                oObtenerPagos.sDOI              = drd.IsDBNull(pos_DOI)             ? string.Empty : drd.GetString(pos_DOI);
                                oObtenerPagos.sMoneda           = drd.IsDBNull(pos_MONEDA)          ? string.Empty : drd.GetString(pos_MONEDA);
                                oObtenerPagos.dMonto_Aplicar    = drd.IsDBNull(pos_MONTO_APLICAR)   ? 0            : drd.GetDecimal(pos_MONTO_APLICAR);
                                oObtenerPagos.sRubro_Cuenta     = drd.IsDBNull(pos_RUBRO_CUENTA)    ? string.Empty : drd.GetString(pos_RUBRO_CUENTA);
                                oObtenerPagos.sTipo_Movimiento  = drd.IsDBNull(pos_TIPO_MOVIMIENTO) ? string.Empty : drd.GetString(pos_TIPO_MOVIMIENTO);
                                loObtenerPagos.Add(oObtenerPagos);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return loObtenerPagos;
        }
        #endregion

        #region ListadoPagos
        public List<EnCorresponsaliaDescargarPagos> adListadoPagos()
        {
            List<EnCorresponsaliaDescargarPagos> loObtenerPagos = new List<EnCorresponsaliaDescargarPagos>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_ListarReportePago", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader drd = cmd.ExecuteReader())
                    {
                        if (drd != null)
                        {
                            int pos_NroLinea = drd.GetOrdinal("NRO_LINEA");
                            int pos_NUMERO_Doi = drd.GetOrdinal("vDoi");
                            int pos_SNombre_Cliente = drd.GetOrdinal("vNombreCliente");
                            int pos_vDescripcion = drd.GetOrdinal("vDescripcion");
                            int pos_vRubroCuenta = drd.GetOrdinal("vRubroCuenta");
                            int pos_vCodigoEstado = drd.GetOrdinal("vCodigoEstado");
                            int pos_vAsiento_Contable = drd.GetOrdinal("vAsiento_Contable");
                            int pos_vCodigoError = drd.GetOrdinal("vCodigoError");
                            int pos_NroTicket = drd.GetOrdinal("vNroTicket");
                            int pos_vNumero_Credito = drd.GetOrdinal("vNumero_Credito");
                            int pos_vReferencia = drd.GetOrdinal("vReferencia");
                            int pos_nImp_Origen = drd.GetOrdinal("nImp_Origen");
                            int pos_nImpDepositado = drd.GetOrdinal("nImpDepositado");
                            int pos_nImp_Mora = drd.GetOrdinal("nImp_Mora");
                            int pos_iOficina_Pago = drd.GetOrdinal("iOficina_Pago");
                            int pos_dFechaPago = drd.GetOrdinal("dFechaPago");
                            int pos_vObservacion = drd.GetOrdinal("vObservacion");
                            int pos_vNTransaccion = drd.GetOrdinal("vNTransaccion");
                            int pos_dFechaVencimiento = drd.GetOrdinal("dFechaVencimiento");
                            int pos_nSaldoCapital = drd.GetOrdinal("nSaldoCapital");
                            int pos_nOtrosCargos = drd.GetOrdinal("nOtrosCargos");
                            int pos_nOtrosGastos = drd.GetOrdinal("nOtrosGastos");
                            int pos_vSegDesgravamen = drd.GetOrdinal("vSegDesgravamen");
                            int pos_iInt_Moratorio = drd.GetOrdinal("iInt_Moratorio");
                            int pos_iITFF = drd.GetOrdinal("iITFF");
                          

                            while (drd.Read())
                            {
                                EnCorresponsaliaDescargarPagos oObtenerPagos = new EnCorresponsaliaDescargarPagos();
                                oObtenerPagos.NRO_LINEA = drd.IsDBNull(pos_NroLinea) ? 0 : drd.GetInt32(pos_NroLinea);
                                oObtenerPagos.sDOI = drd.IsDBNull(pos_NUMERO_Doi) ? string.Empty : drd.GetString(pos_NUMERO_Doi);
                                oObtenerPagos.vNombreCliente = drd.IsDBNull(pos_SNombre_Cliente) ? string.Empty : drd.GetString(pos_SNombre_Cliente);
                                oObtenerPagos.vDescripcion = drd.IsDBNull(pos_vDescripcion) ? string.Empty : drd.GetString(pos_vDescripcion);
                                oObtenerPagos.sRubro_Cuenta = drd.IsDBNull(pos_vRubroCuenta) ? string.Empty : drd.GetString(pos_vRubroCuenta);
                                oObtenerPagos.vCodigoEstado = drd.IsDBNull(pos_vCodigoEstado) ? string.Empty : drd.GetString(pos_vCodigoEstado);
                                oObtenerPagos.vAsiento_Contable = drd.IsDBNull(pos_vAsiento_Contable) ? string.Empty : drd.GetString(pos_vAsiento_Contable);
                                oObtenerPagos.vCodigoError = drd.IsDBNull(pos_vCodigoError) ? string.Empty : drd.GetString(pos_vCodigoError);
                                oObtenerPagos.vNroTicket = drd.IsDBNull(pos_NroTicket) ? string.Empty : drd.GetString(pos_NroTicket);
                                oObtenerPagos.sNumero_Credito = drd.IsDBNull(pos_vNumero_Credito) ? string.Empty : drd.GetString(pos_vNumero_Credito);
                                oObtenerPagos.vReferencia = drd.IsDBNull(pos_vReferencia) ? string.Empty : drd.GetString(pos_vReferencia);
                                oObtenerPagos.nImp_Origen = drd.IsDBNull(pos_nImp_Origen) ? 0 : drd.GetDecimal(pos_nImp_Origen);        
                                oObtenerPagos.nImpDepositado = drd.IsDBNull(pos_nImpDepositado) ? 0 : drd.GetDecimal(pos_nImpDepositado);     
                                oObtenerPagos.iImp_Mora = drd.IsDBNull(pos_nImp_Mora) ? 0 : drd.GetDecimal(pos_nImp_Mora);  
                                oObtenerPagos.iOficina_Pago = drd.IsDBNull(pos_iOficina_Pago) ? 0 : drd.GetInt32(pos_iOficina_Pago);
                                oObtenerPagos.dFechaPago = drd.IsDBNull(pos_dFechaPago) ? string.Empty : drd.GetString(pos_dFechaPago);
                                oObtenerPagos.vObservacion = drd.IsDBNull(pos_vObservacion) ? string.Empty : drd.GetString(pos_vObservacion);
                                oObtenerPagos.nNTransaccion = drd.IsDBNull(pos_vNTransaccion) ? 0 : drd.GetDecimal(pos_vNTransaccion);   
                                oObtenerPagos.dFechaVencimiento = drd.IsDBNull(pos_dFechaVencimiento) ? string.Empty : drd.GetString(pos_dFechaVencimiento); 
                                oObtenerPagos.nSaldoCapital = drd.IsDBNull(pos_nSaldoCapital) ? 0 : drd.GetDecimal(pos_nSaldoCapital);
                                oObtenerPagos.nOtrosCargos = drd.IsDBNull(pos_nOtrosCargos) ? 0 : drd.GetDecimal(pos_nOtrosCargos);
                                oObtenerPagos.nOtrosGastos = drd.IsDBNull(pos_nOtrosGastos) ? 0 : drd.GetDecimal(pos_nOtrosGastos);
                                oObtenerPagos.vSegDesgravamen = drd.IsDBNull(pos_vSegDesgravamen) ? string.Empty : drd.GetString(pos_vSegDesgravamen);
                                oObtenerPagos.iInt_Moratorio = drd.IsDBNull(pos_iInt_Moratorio) ? 0 : drd.GetDecimal(pos_iInt_Moratorio);   
                                oObtenerPagos.nItf = drd.IsDBNull(pos_iITFF) ? 0 : drd.GetDecimal(pos_iITFF);
                                loObtenerPagos.Add(oObtenerPagos);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return loObtenerPagos;
        }
        #endregion

        #region ListarPago
        public async Task<List<EnCorresponsaliaDescargarPagos>>fad_ListarPago(EnCorresponsaliaParametros Parametros , string fechaInicio  , string fechaFin )
        {
            var lsProducto = new List<EnCorresponsaliaDescargarPagos>();
       
            try
            {
                using (SqlCommand cmd = new SqlCommand("[TOP_OPERA].[sCRE_GeneraReportePago]", conexion))
                {
                     cmd.CommandType = CommandType.StoredProcedure;                    
                     cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                     cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
                     cmd.CommandTimeout = 0;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr != null)
                        {
                            while (rdr.Read())
                            {
                                var data = new EnCorresponsaliaDescargarPagos();
                                data.NRO_LINEA = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);    
                                data.sDOI = rdr.IsDBNull(1) ? "" : rdr.GetString(1);
                                data.vNombreCliente = rdr.IsDBNull(2) ? "" : rdr.GetString(2);
                                data.vMoneda = rdr.IsDBNull(3) ? "" : rdr.GetString(3); 
                                data.vDescripcion= rdr.IsDBNull(4) ? "" : rdr.GetString(4);
                                data.sRubro_Cuenta = rdr.IsDBNull(5) ? "" : rdr.GetString(5);
                                data.vCodigoEstado = rdr.IsDBNull(6) ? "" : rdr.GetString(6);
                                data.vAsiento_Contable = rdr.IsDBNull(7) ? "" : rdr.GetString(7);
                                data.vCodigoEstado = rdr.IsDBNull(8) ? "" : rdr.GetString(8);
                                data.vNroTicket = rdr.IsDBNull(9) ? "" : rdr.GetString(9);
                                data.sNumero_Credito = rdr.IsDBNull(10) ? "" : rdr.GetString(10);
                                data.vReferencia = rdr.IsDBNull(11) ? "" : rdr.GetString(11);
                                data.nImp_Origen = rdr.IsDBNull(12) ? 0 : rdr.GetDecimal(12);  
                                data.nImpDepositado = rdr.IsDBNull(13) ? 0 : rdr.GetDecimal(13); 
                                data.iImp_Mora = rdr.IsDBNull(14) ? 0 : rdr.GetDecimal(14);   
                                data.iOficina_Pago = rdr.IsDBNull(15) ? 0 : rdr.GetInt32(15);
                                data.dFechaPago = rdr.IsDBNull(16) ? "" : rdr.GetString(16);
                                data.vObservacion = rdr.IsDBNull(17) ? "" : rdr.GetString(17);
                                data.nNTransaccion = rdr.IsDBNull(18) ? 0 : rdr.GetDecimal(18);
                                data.dFechaVencimiento = rdr.IsDBNull(19) ? "" : rdr.GetString(19);
                                data.nSaldoCapital = rdr.IsDBNull(20) ? 0 : rdr.GetDecimal(20);
                                data.nOtrosCargos = rdr.IsDBNull(21) ? 0 : rdr.GetDecimal(21);
                                data.nOtrosGastos = rdr.IsDBNull(22) ? 0 : rdr.GetDecimal(22); 
                                data.vSegDesgravamen = rdr.IsDBNull(23) ? "" : rdr.GetString(23);
                                data.iInt_Moratorio = rdr.IsDBNull(24) ? 0 : rdr.GetDecimal(24);
                                data.nItf = rdr.IsDBNull(25) ? 0 : rdr.GetDecimal(25);
                                lsProducto.Add(data);

                            }
                            rdr.Close();
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw new Exception("Error SQL: " + ex.Message);
            }

            return lsProducto.ToList();

        }
        #endregion

        #region ObtenerPagosCreditosVencidos
        public List<EnCorresponsaliaDescargarPagos> adObtenerPagosCV()
        {
            List<EnCorresponsaliaDescargarPagos> loObtenerPagos = new List<EnCorresponsaliaDescargarPagos>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("AFE_MAS.sCRE_DescargarDataCreditosVencidos", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader drd = cmd.ExecuteReader())
                    {
                        if (drd != null)
                        {
                            int pos_FECHA_PROCESO = drd.GetOrdinal("vFechaProceso");
                            int pos_NUMERO_CREDITO = drd.GetOrdinal("vNumeroCredito");
                            int pos_DOI = drd.GetOrdinal("vDoi");
                            int pos_MONEDA = drd.GetOrdinal("vCodigoMoneda");
                            int pos_MONTO_APLICAR = drd.GetOrdinal("nMontoAplicar");
                            int pos_RUBRO_CUENTA = drd.GetOrdinal("vRubroCuenta");
                            int pos_TIPO_MOVIMIENTO = drd.GetOrdinal("vTipoMovimiento");


                            while (drd.Read())
                            {
                                EnCorresponsaliaDescargarPagos oObtenerPagos = new EnCorresponsaliaDescargarPagos();
                                oObtenerPagos.sFecha_Proceso    = drd.IsDBNull(pos_FECHA_PROCESO)   ? string.Empty : drd.GetString(pos_FECHA_PROCESO);
                                oObtenerPagos.sNumero_Credito   = drd.IsDBNull(pos_NUMERO_CREDITO)  ? string.Empty : drd.GetString(pos_NUMERO_CREDITO);
                                oObtenerPagos.sDOI              = drd.IsDBNull(pos_DOI)             ? string.Empty : drd.GetString(pos_DOI);
                                oObtenerPagos.sMoneda           = drd.IsDBNull(pos_MONEDA)          ? string.Empty : drd.GetString(pos_MONEDA);
                                oObtenerPagos.dMonto_Aplicar    = drd.IsDBNull(pos_MONTO_APLICAR)   ? 0            : drd.GetDecimal(pos_MONTO_APLICAR);
                                oObtenerPagos.sRubro_Cuenta     = drd.IsDBNull(pos_RUBRO_CUENTA)    ? string.Empty : drd.GetString(pos_RUBRO_CUENTA);
                                oObtenerPagos.sTipo_Movimiento  = drd.IsDBNull(pos_TIPO_MOVIMIENTO) ? string.Empty : drd.GetString(pos_TIPO_MOVIMIENTO);
                                loObtenerPagos.Add(oObtenerPagos);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return loObtenerPagos;
        }
        #endregion

        #region GeneracionPagos
        public EnCorresponsalia adGeneracionPagos(EnCorresponsaliaExportarType tbExportar_Pagos, EnCorresponsaliaParametros Parametros)
        {
            var oRepuesta = new EnCorresponsalia();

            SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_ImportarData", conexion);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter param = cmd.Parameters.Add("@ptDetalleExportar", SqlDbType.Structured);
                param.Direction = ParameterDirection.Input;
                param.TypeName  = "TOP_OPERA.tCRE_uExportarPagos";
                param.Value     = tbExportar_Pagos.Count == 0 ? null : tbExportar_Pagos;

                SqlParameter par1 = cmd.Parameters.Add("@piIAccion", SqlDbType.Int);
                par1.Direction    = ParameterDirection.Input;
                par1.Value        = Parametros.iIAccion;

                SqlParameter par2 = cmd.Parameters.Add("@piIdUsuario", SqlDbType.Int);
                par2.Direction    = ParameterDirection.Input;
                par2.Value        = Parametros.iIdUsuario;

                SqlParameter par3 = cmd.Parameters.Add("@pvAudNombreUsuarioCreacion", SqlDbType.VarChar, 50);
                par3.Direction    = ParameterDirection.Input;
                par3.Value        = Parametros.sAudNombreUsuarioCreacion;


                SqlParameter par4 = cmd.Parameters.Add("@pvAudIPCreacion", SqlDbType.VarChar, 50);
                par4.Direction    = ParameterDirection.Input;
                par4.Value        = Parametros.sAudIPCreacion;

                SqlParameter par5 = cmd.Parameters.Add("@pvAudMACCreacion", SqlDbType.VarChar, 25);
                par5.Direction    = ParameterDirection.Input;
                par5.Value        = Parametros.sAudMACCreacion;

                SqlParameter par6 = cmd.Parameters.Add("@piCodigoOficinaUsuario", SqlDbType.Int);
                par6.Direction    = ParameterDirection.Input;
                par6.Value        = Parametros.iCodigoOficinaUsuario;



                cmd.CommandTimeout = 0;

                using (SqlDataReader drd = cmd.ExecuteReader())
                {
                    if (drd != null)
                    {
                        int pos_iIndicador       = drd.GetOrdinal("iIndicador");
                        int pos_vMensaje         = drd.GetOrdinal("vMensaje");
                        int pos_iError           = drd.GetOrdinal("iError");

                        while (drd.Read())
                        {
                            oRepuesta.iIndicador = drd.IsDBNull(pos_iIndicador)  ? 0 : drd.GetInt32(pos_iIndicador);
                            oRepuesta.sMensaje   = drd.IsDBNull(pos_vMensaje)    ? string.Empty : drd.GetString(pos_vMensaje);
                            oRepuesta.iError     = drd.IsDBNull(pos_iError)      ? 0 : drd.GetInt32(pos_iError);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_ExcepcionesAD, UtlConstantes.LogNamespace_ExcepcionesRN,
                    this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                    ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }

            return oRepuesta;
        }

        public EnCorresponsalia adGeneracionPagosCA(string json, EnCorresponsaliaParametros Parametros)
        {
            var oRepuesta = new EnCorresponsalia();

            SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_ImportarDeudasCA", conexion);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                // Parámetro de JSON
                SqlParameter parJson = cmd.Parameters.Add("@pvJson", SqlDbType.NVarChar, -1);
                parJson.Direction = ParameterDirection.Input;
                parJson.Value = json;

                // Parámetro de Usuario
                SqlParameter parUsuario = cmd.Parameters.Add("@pvAudUsuario", SqlDbType.VarChar, 20);
                parUsuario.Direction = ParameterDirection.Input;
                parUsuario.Value = Parametros.sAudNombreUsuarioCreacion;

                // Parámetro de IP
                SqlParameter parIP = cmd.Parameters.Add("@pvAudIP", SqlDbType.VarChar, 20);
                parIP.Direction = ParameterDirection.Input;
                parIP.Value = Parametros.sAudIPCreacion;

                // Parámetro de MAC
                SqlParameter parMAC = cmd.Parameters.Add("@pvAudMAC", SqlDbType.VarChar, 20);
                parMAC.Direction = ParameterDirection.Input;
                parMAC.Value = Parametros.sAudMACCreacion;

                // Parámetro de Id de Agencia
                SqlParameter parAgencia = cmd.Parameters.Add("@piIdAgencia", SqlDbType.Int);
                parAgencia.Direction = ParameterDirection.Input;
                parAgencia.Value = Parametros.iCodigoOficinaUsuario;

                cmd.CommandTimeout = 0;

                using (SqlDataReader drd = cmd.ExecuteReader())
                {
                    if (drd != null)
                    {
                        int pos_iIndicador = drd.GetOrdinal("iIndicador");
                        int pos_vMensaje = drd.GetOrdinal("vMensaje");
                        int pos_iError = drd.GetOrdinal("iError");

                        while (drd.Read())
                        {
                            oRepuesta.iIndicador = drd.IsDBNull(pos_iIndicador) ? 0 : drd.GetInt32(pos_iIndicador);
                            oRepuesta.sMensaje = drd.IsDBNull(pos_vMensaje) ? string.Empty : drd.GetString(pos_vMensaje);
                            oRepuesta.iError = drd.IsDBNull(pos_iError) ? 0 : drd.GetInt32(pos_iError);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_ExcepcionesAD, UtlConstantes.LogNamespace_ExcepcionesRN,
                    this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "",
                    ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }

            return oRepuesta;
        }

        #endregion

        #region GeneracionDeudaArchivo
        public List<EnCorresponsaliaArchivo> adGeneracionDeudaArchivo(int iAccion)
        {
            List<EnCorresponsaliaArchivo> loGeneracionDeudaArchivo = new List<EnCorresponsaliaArchivo>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_ObtenerData", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@piIAccion", iAccion);

                    using (SqlDataReader drd = cmd.ExecuteReader())
                    {
                        if (drd != null)
                        {
                            int pos_BCP_DEUDAS       = drd.GetOrdinal("vDetalleBcpDeudas");

                            while (drd.Read())
                            {
                                EnCorresponsaliaArchivo oGeneracionDeudaArchivo = new EnCorresponsaliaArchivo();
                                oGeneracionDeudaArchivo.BcpDeudas = drd.IsDBNull(pos_BCP_DEUDAS) ? string.Empty : drd.GetString(pos_BCP_DEUDAS);
                                loGeneracionDeudaArchivo.Add(oGeneracionDeudaArchivo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return loGeneracionDeudaArchivo;
        }
        #endregion

        #region Reporte - AfectacionesMasivas
        public List<EnAfectacionesMasivasReporteCabecera> fad_ReporteAfectacionesMasivasCabecera(EnAfectacionesMasivasReporteCabecera oParametros)
        {
            var lsReporteAfectacionesCabecera = new List<EnAfectacionesMasivasReporteCabecera>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("AFE_MAS.USP_AfectacionMasiva_Reporte_Cabecera", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pdFechaDesde", oParametros.sFechaInicio);
                    cmd.Parameters.AddWithValue("@pdFechaHasta", oParametros.sFechaFin);
                    cmd.Parameters.AddWithValue("@piNumeroPagina", oParametros.NumeroPagina);
                    cmd.Parameters.AddWithValue("@piNumeroRegistros", oParametros.NumeroRegistros);
                    cmd.CommandTimeout = 0;

                    using (SqlDataReader drd = cmd.ExecuteReader())
                    {
                        if (drd != null)
                        {
                            int pos_iTotalRegistros             = drd.GetOrdinal("iTotalRegistros");
                            int pos_RowNumber                   = drd.GetOrdinal("RowNumber");
                            int pos_vCargoFecha                 = drd.GetOrdinal("vCargoFecha");
                            int pos_vCargoHora                  = drd.GetOrdinal("vCargoHora");
                            int pos_vNombreProceso              = drd.GetOrdinal("vNombreProceso");
                            int pos_vCodigoUsuario              = drd.GetOrdinal("vCodigoUsuario");
                            int pos_vNombreUsuario              = drd.GetOrdinal("vNombreUsuario");
                            int pos_nNumeroTicket               = drd.GetOrdinal("nNumeroTicket");
                            int pos_vCargoEstado                = drd.GetOrdinal("vCargoEstado");
                            int pos_vCargoMensaje               = drd.GetOrdinal("vCargoMensaje");
                            int pos_iNumeroRegistros            = drd.GetOrdinal("iNumeroRegistros");
                            int pos_iNumeroRegistrosCorrectos   = drd.GetOrdinal("iNumeroRegistrosCorrectos");
                            int pos_iNumeroRegistrosFallidos    = drd.GetOrdinal("iNumeroRegistrosFallidos");

                            while (drd.Read())
                            {
                                EnAfectacionesMasivasReporteCabecera oResultado = new EnAfectacionesMasivasReporteCabecera();

                                oResultado.TotalRegistros               = drd.IsDBNull(pos_iTotalRegistros)             ? 0 : drd.GetInt32(pos_iTotalRegistros);
                                oResultado.RowNumber                    = drd.IsDBNull(pos_RowNumber)                   ? 0 : drd.GetInt32(pos_RowNumber);
                                oResultado.sCargoFecha                  = drd.IsDBNull(pos_vCargoFecha)                 ? string.Empty : drd.GetString(pos_vCargoFecha);
                                oResultado.sCargoHora                   = drd.IsDBNull(pos_vCargoHora)                  ? string.Empty : drd.GetString(pos_vCargoHora);
                                oResultado.sNombreProceso               = drd.IsDBNull(pos_vNombreProceso)              ? string.Empty : drd.GetString(pos_vNombreProceso);
                                oResultado.sCodigoUsuario               = drd.IsDBNull(pos_vCodigoUsuario)              ? string.Empty : drd.GetString(pos_vCodigoUsuario);
                                oResultado.sNombreUsuario               = drd.IsDBNull(pos_vNombreUsuario)              ? string.Empty : drd.GetString(pos_vNombreUsuario);
                                oResultado.nNumeroTicket                = drd.IsDBNull(pos_nNumeroTicket)               ? 0 : drd.GetDecimal(pos_nNumeroTicket);
                                oResultado.sCargoEstado                 = drd.IsDBNull(pos_vCargoEstado)                ? string.Empty : drd.GetString(pos_vCargoEstado);
                                oResultado.sCargoMensaje                = drd.IsDBNull(pos_vCargoMensaje)               ? string.Empty : drd.GetString(pos_vCargoMensaje);
                                oResultado.iNumeroRegistros             = drd.IsDBNull(pos_iNumeroRegistros)            ? 0 : drd.GetInt32(pos_iNumeroRegistros);
                                oResultado.iNumeroRegistrosCorrectos    = drd.IsDBNull(pos_iNumeroRegistrosCorrectos)   ? 0 : drd.GetInt32(pos_iNumeroRegistrosCorrectos);
                                oResultado.iNumeroRegistrosFallidos     = drd.IsDBNull(pos_iNumeroRegistrosFallidos)    ? 0 : drd.GetInt32(pos_iNumeroRegistrosFallidos);

                                lsReporteAfectacionesCabecera.Add(oResultado);
                            }
                            drd.Close();
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw new Exception("Error SQL: " + ex.Message);
            }

            return lsReporteAfectacionesCabecera.ToList();
        }

        public List<EnAfectacionesMasivasReporte> adListadoAfectacionesMasivas(EnAfectacionesMasivasReporteCabecera oParametros)
        {
            var lsReporteAfectaciones = new List<EnAfectacionesMasivasReporte>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("AFE_MAS.USP_AfectacionMasiva_Reporte_Detalle", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pdFechaDesde", oParametros.sFechaInicio);
                    cmd.Parameters.AddWithValue("@pdFechaHasta", oParametros.sFechaFin);

                    cmd.CommandTimeout = 0;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr != null)
                        {
                            while (rdr.Read())
                            {
                                EnAfectacionesMasivasReporte data = new EnAfectacionesMasivasReporte();
                                data.iNroLinea = rdr.IsDBNull(0) ? 0 : Convert.ToInt32(rdr.GetValue(0));
                                data.iIdAfectacion = rdr.IsDBNull(1) ? 0 : Convert.ToInt32(rdr.GetValue(1));
                                data.iIdTopaz = rdr.IsDBNull(2) ? 0 : Convert.ToInt32(rdr.GetValue(2));
                                data.nNroTicket = rdr.IsDBNull(3) ? 0 : rdr.GetDecimal(3);
                                data.nCodSucursalCrd = rdr.IsDBNull(4) ? 0 : rdr.GetDecimal(4);
                                data.vSucursalCrd = rdr.IsDBNull(5) ? "" : rdr.GetString(5);
                                data.nNumeroCredito = rdr.IsDBNull(6) ? 0 : rdr.GetDecimal(6);
                                data.vTitularCredito = rdr.IsDBNull(7) ? "" : rdr.GetString(7);
                                data.nNumeroCuenta = rdr.IsDBNull(8) ? 0 : rdr.GetDecimal(8);
                                data.vTitularCuenta = rdr.IsDBNull(9) ? "" : rdr.GetString(9);
                                data.vTipoCuenta = rdr.IsDBNull(10) ? "" : rdr.GetString(10);
                                data.vIndicadorConInd = rdr.IsDBNull(11) ? "" : rdr.GetString(11);
                                data.nIndicadorExtorno = rdr.IsDBNull(12) ? 0 : rdr.GetDecimal(12);
                                data.iDiasAtraso = rdr.IsDBNull(13) ? 0 : Convert.ToInt32(rdr.GetValue(13));
                                data.nNumeroTransaccion = rdr.IsDBNull(14) ? 0 : rdr.GetDecimal(14);
                                data.nMontoAplicar = rdr.IsDBNull(15) ? 0 : rdr.GetDecimal(15);
                                data.vFechaGeneracionAfectacion = rdr.IsDBNull(16) ? "" : Convert.ToString(rdr.GetValue(16));
                                data.vFechaCargoMasiva = rdr.IsDBNull(17) ? "" : Convert.ToString(rdr.GetValue(17));
                                data.vGlosaBloqueo = rdr.IsDBNull(18) ? "" : rdr.GetString(18);
                                data.vMotivoRetencionBloqueo = rdr.IsDBNull(19) ? "" : rdr.GetString(19);
                                data.vTelefono1 = rdr.IsDBNull(20) ? "" : rdr.GetString(20);
                                data.vTelefono2 = rdr.IsDBNull(21) ? "" : rdr.GetString(21);
                                data.vTelefono3 = rdr.IsDBNull(22) ? "" : rdr.GetString(22);
                                data.vTelefono4 = rdr.IsDBNull(23) ? "" : rdr.GetString(23);
                                data.vCorreo = rdr.IsDBNull(24) ? "" : rdr.GetString(24);
                                data.vDireccion = rdr.IsDBNull(25) ? "" : rdr.GetString(25);
                                data.vDepartamento = rdr.IsDBNull(26) ? "" : rdr.GetString(26);
                                data.vProvincia = rdr.IsDBNull(27) ? "" : rdr.GetString(27);
                                data.vDistrito = rdr.IsDBNull(28) ? "" : rdr.GetString(28);
                                data.vEstado = rdr.IsDBNull(29) ? "" : rdr.GetString(29);
                                data.vDescripEstado = rdr.IsDBNull(30) ? "" : rdr.GetString(30);
                                lsReporteAfectaciones.Add(data);

                            }
                            rdr.Close();
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw new Exception("Error SQL: " + ex.Message);
            }

            return lsReporteAfectaciones.ToList();
        }
        //public List<EnAfectacionesMasivasReporte> fad_ReporteAfectacionesMasivasDetalle(EnAfectacionesMasivasReporteCabecera oParametros)
        //{
        //    var lsReporteAfectaciones = new List<EnAfectacionesMasivasReporte>();
        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand("AFE_MAS.sAFE_AmortizacionesMasivasDetalle", conexion))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@pdFechaDesde", oParametros.sFechaInicio);
        //            cmd.Parameters.AddWithValue("@pdFechaHasta", oParametros.sFechaFin);
        //            cmd.CommandTimeout = 0;

        //            using (SqlDataReader drd = cmd.ExecuteReader())
        //            {
        //                if (drd != null)
        //                {
        //                    int pos_iNumeroLinea          = drd.GetOrdinal("iNumeroLinea");
        //                    int pos_iIdTopaz              = drd.GetOrdinal("iIdTopaz");
        //                    int pos_nNumeroTicket         = drd.GetOrdinal("nNumeroTicket");
        //                    int pos_iNumeroSucursal       = drd.GetOrdinal("iNumeroSucursal");
        //                    int pos_vNombreSucursal       = drd.GetOrdinal("vNombreSucursal");
        //                    int pos_iNumeroCuenta         = drd.GetOrdinal("iNumeroCuenta");
        //                    int pos_vTitularCredito       = drd.GetOrdinal("vTitularCredito");
        //                    int pos_vTitularCuenta        = drd.GetOrdinal("vTitularCuenta");
        //                    int pos_vTipoCuenta           = drd.GetOrdinal("vTipoCuenta");
        //                    int pos_vIndicadorcon_ind     = drd.GetOrdinal("vIndicadorcon_ind");
        //                    int pos_iIndicadorextorno     = drd.GetOrdinal("iIndicadorextorno");
        //                    int pos_nAsientocontable      = drd.GetOrdinal("nAsientocontable");
        //                    int pos_nRubrocuenta          = drd.GetOrdinal("nRubrocuenta");
        //                    int pos_nMontoaplicar         = drd.GetOrdinal("nMontoaplicar");
        //                    int pos_vFechaproceso         = drd.GetOrdinal("vFechaproceso");
        //                    int pos_vTel_1                = drd.GetOrdinal("vTel_1");
        //                    int pos_vTel_2                = drd.GetOrdinal("vTel_2");
        //                    int pos_vCelular              = drd.GetOrdinal("vCelular");
        //                    int pos_vFax                  = drd.GetOrdinal("vFax");
        //                    int pos_vCorreo               = drd.GetOrdinal("vCorreo");
        //                    int pos_vDireccion            = drd.GetOrdinal("vDireccion");
        //                    int pos_vNombre_departamento  = drd.GetOrdinal("vNombre_departamento");
        //                    int pos_vNombre_provincia     = drd.GetOrdinal("vNombre_provincia");
        //                    int pos_vNombre_distrito      = drd.GetOrdinal("vNombre_distrito");
        //                    int pos_vEstado               = drd.GetOrdinal("vEstado");
        //                    int pos_vCodigo_error         = drd.GetOrdinal("vCodigo_error");


        //                    while (drd.Read())
        //                    {
        //                        EnAfectacionesMasivasReporte oRow = new EnAfectacionesMasivasReporte();
        //                        oRow.iNumeroLinea	         = drd.IsDBNull(pos_iNumeroLinea)            ? 0 : drd.GetInt32(pos_iNumeroLinea);
        //                        oRow.iIdTopaz	             = drd.IsDBNull(pos_iIdTopaz)                ? 0 : drd.GetInt32(pos_iIdTopaz);
        //                        oRow.nNumeroTicket	         = drd.IsDBNull(pos_nNumeroTicket)           ? 0 : drd.GetDecimal(pos_nNumeroTicket);
        //                        oRow.iNumeroSucursal	     = drd.IsDBNull(pos_iNumeroSucursal)         ? 0 : drd.GetInt32(pos_iNumeroSucursal);
        //                        oRow.sNombreSucursal	     = drd.IsDBNull(pos_vNombreSucursal)         ? string.Empty : drd.GetString(pos_vNombreSucursal);
        //                        oRow.nNumeroCuenta           = drd.IsDBNull(pos_iNumeroCuenta)           ? 0 : drd.GetInt32(pos_iNumeroCuenta);
        //                        oRow.sTitularCredito	     = drd.IsDBNull(pos_vTitularCredito)         ? string.Empty : drd.GetString(pos_vTitularCredito);
        //                        oRow.sTitularCuenta	         = drd.IsDBNull(pos_vTitularCuenta)          ? string.Empty : drd.GetString(pos_vTitularCuenta);
        //                        oRow.sTipoCuenta	         = drd.IsDBNull(pos_vTipoCuenta)             ? string.Empty : drd.GetString(pos_vTipoCuenta);
        //                        oRow.sIndicadorcon_ind	     = drd.IsDBNull(pos_vIndicadorcon_ind)       ? string.Empty : drd.GetString(pos_vIndicadorcon_ind);
        //                        oRow.iIndicadorextorno	     = drd.IsDBNull(pos_iIndicadorextorno)       ? 0 : drd.GetInt32(pos_iIndicadorextorno);
        //                        oRow.nAsientocontable	     = drd.IsDBNull(pos_nAsientocontable)        ? 0 : drd.GetDecimal(pos_nAsientocontable);
        //                        oRow.nRubrocuenta	         = drd.IsDBNull(pos_nRubrocuenta)            ? 0 : drd.GetDecimal(pos_nRubrocuenta);
        //                        oRow.nMontoaplicar	         = drd.IsDBNull(pos_nMontoaplicar)           ? 0 : drd.GetDecimal(pos_nMontoaplicar);
        //                        oRow.sFechaproceso	         = drd.IsDBNull(pos_vFechaproceso)           ? string.Empty : drd.GetString(pos_vFechaproceso);
        //                        oRow.sTel_1	                 = drd.IsDBNull(pos_vTel_1)               	 ? string.Empty : drd.GetString(pos_vTel_1);
        //                        oRow.sTel_2	                 = drd.IsDBNull(pos_vTel_2)               	 ? string.Empty : drd.GetString(pos_vTel_2);
        //                        oRow.sCelular	             = drd.IsDBNull(pos_vCelular)                ? string.Empty : drd.GetString(pos_vCelular);
        //                        oRow.sFax	                 = drd.IsDBNull(pos_vFax)               	 ? string.Empty : drd.GetString(pos_vFax);
        //                        oRow.sCorreo	             = drd.IsDBNull(pos_vCorreo)                 ? string.Empty : drd.GetString(pos_vCorreo);
        //                        oRow.sDireccion	             = drd.IsDBNull(pos_vDireccion)              ? string.Empty : drd.GetString(pos_vDireccion);
        //                        oRow.sNombre_departamento	 = drd.IsDBNull(pos_vNombre_departamento)    ? string.Empty : drd.GetString(pos_vNombre_departamento);
        //                        oRow.sNombre_provincia	     = drd.IsDBNull(pos_vNombre_provincia)       ? string.Empty : drd.GetString(pos_vNombre_provincia);
        //                        oRow.sNombre_distrito	     = drd.IsDBNull(pos_vNombre_distrito)        ? string.Empty : drd.GetString(pos_vNombre_distrito);
        //                        oRow.sEstado	             = drd.IsDBNull(pos_vEstado)                 ? string.Empty : drd.GetString(pos_vEstado);
        //                        oRow.sCodigo_error	         = drd.IsDBNull(pos_vCodigo_error)           ? string.Empty : drd.GetString(pos_vCodigo_error);

        //                        lsReporteAfectaciones.Add(oRow);

        //                    }
        //                    drd.Close();
        //                }
        //            }


        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
        //        throw new Exception("Error SQL: " + ex.Message);
        //    }

        //    return lsReporteAfectaciones.ToList();
        //}

        #endregion

        #region Caja Arequipa
        public EnExportarDeudasCA ad_ExportarDeudasCA()
        {
            EnExportarDeudasCA oResultado = new EnExportarDeudasCA();
            List<string> deudasList = new List<string>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_ExportarDeudasCA", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.CommandTimeout = 0;
                    using (SqlDataReader drd = cmd.ExecuteReader())
                    {
                        if (drd != null)
                        {
                            int pos_sFileName = drd.GetOrdinal("sNombreArchivo");


                            while (drd.Read())
                            {
                                oResultado.sFileName = drd.IsDBNull(pos_sFileName) ? string.Empty : drd.GetString(pos_sFileName);
                            }
                        }

                        drd.NextResult();

                        while (drd.Read())
                        {
                            deudasList.Add(drd["sDeudasCajaArequipa"].ToString());
                        }
                    }
                }

                // Crear el archivo TXT en memoria
                using (MemoryStream memoryStream = new MemoryStream())
                using (StreamWriter writer = new StreamWriter(memoryStream))
                {
                    foreach (var deuda in deudasList)
                    {
                        writer.WriteLine(deuda);
                    }
                    writer.Flush();

                    // Convertir a arreglo de bytes
                    oResultado.bFileBytes = memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
            return oResultado;
        }

        #endregion
    }
}
