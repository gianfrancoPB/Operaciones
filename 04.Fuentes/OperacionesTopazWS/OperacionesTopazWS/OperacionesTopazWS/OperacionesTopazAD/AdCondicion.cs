using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SatelitesEN;
using UtilitarioCrk;



namespace OperacionesTopazAD
{
    public class AdCondicion : AdGeneral
    {
        public AdCondicion(SqlConnection con)
        {
            conexion = con;
        }

        #region ListarCondiciones
        public async Task<List<EnCondicion>> fad_ListarCondiciones(EnCondicion oCondicion)
        {
            var lsCondicion = new List<EnCondicion>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_ListarCondiciones", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter par1 = cmd.Parameters.Add("@piNumeroPagina", SqlDbType.Int);
                    par1.Direction = ParameterDirection.Input;
                    par1.Value = oCondicion.NumeroPagina;

                    SqlParameter par2 = cmd.Parameters.Add("@piNumeroRegistros", SqlDbType.Int);
                    par2.Direction = ParameterDirection.Input;
                    par2.Value = oCondicion.NumeroRegistros;
                    cmd.CommandTimeout = 0;


                    using (SqlDataReader drd = cmd.ExecuteReader())
                    {
                        if (drd != null)
                        {

                            int pos_iTotalRegistros = drd.GetOrdinal("iTotalRegistros");
                            int pos_RowNumber = drd.GetOrdinal("iRowNumber");
                            int pos_iIdCondicion = drd.GetOrdinal("iIdCondicion");
                            int pos_iCodigo = drd.GetOrdinal("iCodigo");
                            int pos_vCondicion = drd.GetOrdinal("vCondicion");
                            int pos_vDescripcion = drd.GetOrdinal("vDescripcion");
                            int pos_iEstado = drd.GetOrdinal("iEstado");
                            int pos_vEstado = drd.GetOrdinal("vEstado");
                            int pos_iTipo = drd.GetOrdinal("iTipo");

                            while (drd.Read())
                            {
                                var oReglas = new EnCondicion();

                                oReglas.TotalRegistros = drd.IsDBNull(pos_iTotalRegistros) ? 0 : drd.GetInt32(pos_iTotalRegistros);
                                oReglas.RowNumber = drd.IsDBNull(pos_RowNumber) ? 0 : drd.GetInt32(pos_RowNumber);
                                oReglas.iIdCondicion = drd.IsDBNull(pos_iIdCondicion) ? 0 : drd.GetInt32(pos_iIdCondicion);
                                oReglas.iCodigo = drd.IsDBNull(pos_iCodigo) ? 0 : drd.GetInt32(pos_iCodigo);
                                oReglas.vCondicion = drd.IsDBNull(pos_vCondicion) ? string.Empty : drd.GetString(pos_vCondicion);
                                oReglas.vDescripcion = drd.IsDBNull(pos_vDescripcion) ? string.Empty : drd.GetString(pos_vDescripcion);
                                oReglas.iEstado = drd.IsDBNull(pos_iEstado) ? 0 : drd.GetInt32(pos_iEstado);
                                oReglas.sEstado = drd.IsDBNull(pos_vEstado) ? string.Empty : drd.GetString(pos_vEstado);
                                oReglas.iTipo = drd.IsDBNull(pos_iTipo) ? 0 : drd.GetInt32(pos_iTipo);

                                lsCondicion.Add(oReglas);
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
            return lsCondicion.ToList();
        }
        #endregion

        #region ListarProductoCondicion
        public async Task<List<EnProducto>> fad_ListarProductoCondicion(EnProducto enProducto)
        {
            var lsProducto = new List<EnProducto>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_ListarProductoCondicion", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@piIdCondicion", enProducto.iIdCuadro);
                    cmd.Parameters.AddWithValue("@piOperacion", enProducto.iActivo);
                    cmd.CommandTimeout = 0;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr != null)
                        {
                            while (rdr.Read())
                            {
                                var data = new EnProducto();
                                data.iTotalRegistros = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                                data.iNumeroRegistros = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1);
                                data.iIdProducto = rdr.IsDBNull(2) ? 0 : rdr.GetInt32(2);
                                data.iIdMoneda = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                                data.sDescripcion = rdr.IsDBNull(4) ? "" : rdr.GetString(4);
                                data.sDescripcionTipoMoneda = rdr.IsDBNull(5) ? "" : rdr.GetString(5);
                                data.iVigente = rdr.IsDBNull(6) ? 0 : int.Parse(rdr.GetString(6));
                                data.sEstado = rdr.IsDBNull(7) ? "" : rdr.GetString(7);
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

        #region MantenerProductoCondicion
        public async Task<int> fad_MantenerProductoCondicion(EnCorresponsaliaProductoType tbCarteraProducto)
        {
            int iResult = 0;

            SqlCommand cmd = new SqlCommand("TOP_OPERA.sCBR_MantenimientoProductoCondicion", conexion);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter param = cmd.Parameters.Add("@puCarteraProducto", SqlDbType.Structured);
                param.Direction = ParameterDirection.Input;
                param.TypeName = "CBR_CONFI.uCBR_Producto";
                param.Value = tbCarteraProducto.Count == 0 ? null : tbCarteraProducto;
                cmd.CommandTimeout = 0;

                using (SqlDataReader drd = cmd.ExecuteReader())
                {
                    if (drd != null)
                    {

                        int pos_iReasultado = drd.GetOrdinal("iResultado");

                        while (drd.Read())
                        {
                            iResult = drd.IsDBNull(pos_iReasultado) ? 0 : drd.GetInt32(pos_iReasultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }

            return iResult;
        }
        #endregion

        #region ObtenerCondicionId_CreditoVencido
        public async Task<EnCondicion> fad_ObtenerCondicionId_CV(int iIdCondicion)
        {
            var oReglas = new EnCondicion();

            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_CondicionId_CV", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter par1 = cmd.Parameters.Add("@piIdCondicion", SqlDbType.Int);
                    par1.Direction = ParameterDirection.Input;
                    par1.Value = iIdCondicion;
                    cmd.CommandTimeout = 0;

                    using (SqlDataReader drd = cmd.ExecuteReader())
                    {
                        if (drd != null)
                        {
                            int pos_iIdCondicion = drd.GetOrdinal("iIdCondicion");
                            int pos_iCodigo = drd.GetOrdinal("iCodigo");
                            int pos_vCondicion = drd.GetOrdinal("vCondicion");
                            int pos_vDescripcion = drd.GetOrdinal("vDescripcion");
                            int pos_iEstado = drd.GetOrdinal("iEstado");
                            int pos_vEstado = drd.GetOrdinal("vEstado");
                            int pos_iTipo = drd.GetOrdinal("iTipo");
                            int pos_iDiasAtraso = drd.GetOrdinal("iDiasAtraso");
                            int pos_nMontoMinimo = drd.GetOrdinal("nMontoMinimo");

                            while (drd.Read())
                            {
                                oReglas.iIdCondicion = drd.IsDBNull(pos_iIdCondicion) ? 0 : drd.GetInt32(pos_iIdCondicion);
                                oReglas.iCodigo = drd.IsDBNull(pos_iCodigo) ? 0 : drd.GetInt32(pos_iCodigo);
                                oReglas.vCondicion = drd.IsDBNull(pos_vCondicion) ? string.Empty : drd.GetString(pos_vCondicion);
                                oReglas.vDescripcion = drd.IsDBNull(pos_vDescripcion) ? string.Empty : drd.GetString(pos_vDescripcion);
                                oReglas.iEstado = drd.IsDBNull(pos_iEstado) ? 0 : drd.GetInt32(pos_iEstado);
                                oReglas.sEstado = drd.IsDBNull(pos_vEstado) ? string.Empty : drd.GetString(pos_vEstado);
                                oReglas.iTipo = drd.IsDBNull(pos_iTipo) ? 0 : drd.GetInt32(pos_iTipo);
                                oReglas.iDiasAtraso = drd.IsDBNull(pos_iDiasAtraso) ? 0 : drd.GetInt32(pos_iDiasAtraso);
                                oReglas.nMontoMinimo = drd.IsDBNull(pos_nMontoMinimo) ? Convert.ToDecimal(0.00) : drd.GetDecimal(pos_nMontoMinimo);
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
            return oReglas;
        }
        #endregion

        #region ObtenerCondicionId
        public async Task<EnCondicion> fad_ObtenerCondicionId(int iIdCondicion)
        {
            var oReglas = new EnCondicion();

            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_CondicionId", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter par1 = cmd.Parameters.Add("@piIdCondicion", SqlDbType.Int);
                    par1.Direction = ParameterDirection.Input;
                    par1.Value = iIdCondicion;
                    cmd.CommandTimeout = 0;

                    using (SqlDataReader drd = cmd.ExecuteReader())
                    {
                        if (drd != null)
                        {
                            int pos_iIdCondicion = drd.GetOrdinal("iIdCondicion");
                            int pos_iCodigo = drd.GetOrdinal("iCodigo");
                            int pos_vCondicion = drd.GetOrdinal("vCondicion");
                            int pos_vDescripcion = drd.GetOrdinal("vDescripcion");
                            int pos_iEstado = drd.GetOrdinal("iEstado");
                            int pos_vEstado = drd.GetOrdinal("vEstado");
                            int pos_iTipo = drd.GetOrdinal("iTipo");

                            while (drd.Read())
                            {
                                oReglas.iIdCondicion = drd.IsDBNull(pos_iIdCondicion) ? 0 : drd.GetInt32(pos_iIdCondicion);
                                oReglas.iCodigo = drd.IsDBNull(pos_iCodigo) ? 0 : drd.GetInt32(pos_iCodigo);
                                oReglas.vCondicion = drd.IsDBNull(pos_vCondicion) ? string.Empty : drd.GetString(pos_vCondicion);
                                oReglas.vDescripcion = drd.IsDBNull(pos_vDescripcion) ? string.Empty : drd.GetString(pos_vDescripcion);
                                oReglas.iEstado = drd.IsDBNull(pos_iEstado) ? 0 : drd.GetInt32(pos_iEstado);
                                oReglas.sEstado = drd.IsDBNull(pos_vEstado) ? string.Empty : drd.GetString(pos_vEstado);
                                oReglas.iTipo = drd.IsDBNull(pos_iTipo) ? 0 : drd.GetInt32(pos_iTipo);
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
            return oReglas;
        }
        #endregion

        #region ListarCondicionesCreditoVencidos
        public async Task<List<EnCondicion>> fad_ListarCondicionesCreditoVencidos(EnCondicion oCondicion)
        {
            var lsCondicion = new List<EnCondicion>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_ListarCondicionesCreditosVencidos", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter par1 = cmd.Parameters.Add("@piNumeroPagina", SqlDbType.Int);
                    par1.Direction = ParameterDirection.Input;
                    par1.Value = oCondicion.NumeroPagina;

                    SqlParameter par2 = cmd.Parameters.Add("@piNumeroRegistros", SqlDbType.Int);
                    par2.Direction = ParameterDirection.Input;
                    par2.Value = oCondicion.NumeroRegistros;

                    cmd.CommandTimeout = 0;


                    using (SqlDataReader drd = cmd.ExecuteReader())
                    {
                        if (drd != null)
                        {
                            int pos_iTotalRegistros = drd.GetOrdinal("iTotalRegistros");
                            int pos_RowNumber = drd.GetOrdinal("iRowNumber");
                            int pos_iIdCondicion = drd.GetOrdinal("iIdCondicion");
                            int pos_iCodigo = drd.GetOrdinal("iCodigo");
                            int pos_vCondicion = drd.GetOrdinal("vCondicion");
                            int pos_vDescripcion = drd.GetOrdinal("vDescripcion");
                            int pos_iEstado = drd.GetOrdinal("iEstado");
                            int pos_vEstado = drd.GetOrdinal("vEstado");
                            int pos_iTipo = drd.GetOrdinal("iTipo");


                            while (drd.Read())
                            {
                                var oReglas = new EnCondicion();

                                oReglas.TotalRegistros = drd.IsDBNull(pos_iTotalRegistros) ? 0 : drd.GetInt32(pos_iTotalRegistros);
                                oReglas.RowNumber = drd.IsDBNull(pos_RowNumber) ? 0 : drd.GetInt32(pos_RowNumber);
                                oReglas.iIdCondicion = drd.IsDBNull(pos_iIdCondicion) ? 0 : drd.GetInt32(pos_iIdCondicion);
                                oReglas.iCodigo = drd.IsDBNull(pos_iCodigo) ? 0 : drd.GetInt32(pos_iCodigo);
                                oReglas.vCondicion = drd.IsDBNull(pos_vCondicion) ? string.Empty : drd.GetString(pos_vCondicion);
                                oReglas.vDescripcion = drd.IsDBNull(pos_vDescripcion) ? string.Empty : drd.GetString(pos_vDescripcion);
                                oReglas.iEstado = drd.IsDBNull(pos_iEstado) ? 0 : drd.GetInt32(pos_iEstado);
                                oReglas.sEstado = drd.IsDBNull(pos_vEstado) ? string.Empty : drd.GetString(pos_vEstado);
                                oReglas.iTipo = drd.IsDBNull(pos_iTipo) ? 0 : drd.GetInt32(pos_iTipo);

                                lsCondicion.Add(oReglas);
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
            return lsCondicion.ToList();
        }
        #endregion

        #region ListarProductoCondicionCreditoVencido
        public async Task<List<EnProducto>> fad_ListarProductoCondicionCreditoVencido(EnProducto enProducto)
        {
            var lsProducto = new List<EnProducto>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_ListarProductoCondicionCreditoVencido", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@piIdCondicion", enProducto.iIdCuadro);
                    cmd.Parameters.AddWithValue("@piOperacion", enProducto.iActivo);
                    cmd.CommandTimeout = 0;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr != null)
                        {
                            while (rdr.Read())
                            {
                                var data = new EnProducto();
                                data.iTotalRegistros = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                                data.iNumeroRegistros = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1);
                                data.iIdProducto = rdr.IsDBNull(2) ? 0 : rdr.GetInt32(2);
                                data.iIdMoneda = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                                data.sDescripcion = rdr.IsDBNull(4) ? "" : rdr.GetString(4);
                                data.sDescripcionTipoMoneda = rdr.IsDBNull(5) ? "" : rdr.GetString(5);
                                data.iVigente = rdr.IsDBNull(6) ? 0 : int.Parse(rdr.GetString(6));
                                data.sEstado = rdr.IsDBNull(7) ? "" : rdr.GetString(7);

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

        #region ListarProductoCondicionCreditoVencidoAhorros
        public async Task<List<EnProducto>> fad_ListarProductoCondicionCreditoVencidoAhorros(EnProducto enProducto)
        {
            var lsProducto = new List<EnProducto>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_ListarProductoCondicionCreditoVencidoAhorro", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@piIdCondicion", enProducto.iIdCuadro);
                    cmd.Parameters.AddWithValue("@piOperacion", enProducto.iActivo);
                    cmd.CommandTimeout = 0;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr != null)
                        {
                            while (rdr.Read())
                            {
                                var data = new EnProducto();
                                data.iTotalRegistros = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                                data.iNumeroRegistros = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1);
                                data.iIdProducto = rdr.IsDBNull(2) ? 0 : rdr.GetInt32(2);
                                data.iIdMoneda = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                                data.sDescripcion = rdr.IsDBNull(4) ? "" : rdr.GetString(4);
                                data.sDescripcionTipoMoneda = rdr.IsDBNull(5) ? "" : rdr.GetString(5);
                                data.iVigente = rdr.IsDBNull(6) ? 0 : int.Parse(rdr.GetString(6));
                                data.sEstado = rdr.IsDBNull(7) ? "" : rdr.GetString(7);

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

        #region ListarProductoAhorroDraggTable
        public async Task<List<EnCarteraProductoAhorroOrden>> fad_ListarProductoCondicionProductoAhorrosOrden(EnCarteraProductoAhorroOrden enProducto)
        {
            var lsProducto = new List<EnCarteraProductoAhorroOrden>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("TOP_OPERA.sCRE_ListarProductoCondicionProductoAhorroOrden", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@piIdCondicion", enProducto.iIdCuadro);
                    cmd.Parameters.AddWithValue("@piOperacion", enProducto.iActivo);
                    cmd.CommandTimeout = 0;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr != null)
                        {
                            while (rdr.Read())
                            {
                                var data = new EnCarteraProductoAhorroOrden();
                                data.iNumeroOrden = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                                data.psDescripcionProducto = rdr.IsDBNull(1) ? "" : rdr.GetString(1);
                                data.iCodigoProducto = rdr.IsDBNull(2) ? "" : rdr.GetString(2);
                              

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

        #region MantenerProductoCondicionCreditoVencido
        public async Task<int> fad_MantenerProductoCondicionCreditoVencido(EnCorresponsaliaProductoType tbCarteraProducto, EnCorresponsaliaProductoType tbCarteraProductoAhorro, int DiasAtraso , decimal MontoMinimo)
        {
            int iResult = 0;

            SqlCommand cmd = new SqlCommand("TOP_OPERA.sCBR_MantenimientoProductoCondicionCreditoVencido", conexion);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter param = cmd.Parameters.Add("@puCarteraProducto", SqlDbType.Structured);
                param.Direction = ParameterDirection.Input;
                param.TypeName = "CBR_CONFI.uCBR_Producto";
                param.Value = tbCarteraProducto.Count == 0 ? null : tbCarteraProducto;
                cmd.CommandTimeout = 0;

                SqlParameter param1 = cmd.Parameters.Add("@puProductoNuevoAhorro", SqlDbType.Structured);
                param1.Direction = ParameterDirection.Input;
                param1.TypeName = "CBR_CONFI.uCBR_ProductoAhorro";
                param1.Value = tbCarteraProductoAhorro.Count == 0 ? null : tbCarteraProductoAhorro;
                cmd.CommandTimeout = 0;

                SqlParameter par2 = cmd.Parameters.Add("@piDiasAtraso", SqlDbType.Int);
                par2.Direction = ParameterDirection.Input;
                par2.Value = DiasAtraso;

                SqlParameter par3 = cmd.Parameters.Add("@MontoMinimo", SqlDbType.Decimal);
                par3.Direction = ParameterDirection.Input;
                par3.Value = MontoMinimo;

                using (SqlDataReader drd = cmd.ExecuteReader())
                {
                    if (drd != null)
                    {

                        int pos_iReasultado = drd.GetOrdinal("iResultado");

                        while (drd.Read())
                        {
                            iResult = drd.IsDBNull(pos_iReasultado) ? 0 : drd.GetInt32(pos_iReasultado);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }

            return iResult;
        }
        #endregion

        #region GuardarOrdenProductoAhorro
        public async Task<int> fad_GuardarOrdenProductoAhorro(EnProductoAhorroOrdenType tbCarteraProductoAhorro)
        {
            int iResult = 0;

            SqlCommand cmd = new SqlCommand("TOP_OPERA.sCBR_GuardarOrdenProductosAhorros", conexion);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {

                SqlParameter param1 = cmd.Parameters.Add("@puCarteraProductoAhorro", SqlDbType.Structured);
                param1.Direction = ParameterDirection.Input;
                param1.TypeName = "CBR_CONFI.uCBR_ProductoAhorroOrdenado";
                param1.Value = tbCarteraProductoAhorro.Count == 0 ? null : tbCarteraProductoAhorro;
                cmd.CommandTimeout = 0;

                using (SqlDataReader drd = cmd.ExecuteReader())
                {
                    if (drd != null)
                    {

                        int pos_iReasultado = drd.GetOrdinal("iResultado");

                        while (drd.Read())
                        {
                            iResult = drd.IsDBNull(pos_iReasultado) ? 0 : drd.GetInt32(pos_iReasultado);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazAD, UtlConstantes.LogNamespace_OperacionesTopazAD, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }

            return iResult;
        }
        #endregion

    }

}
