using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OperacionesTopazAD;
using SatelitesEN;
using UtilitarioCrk;



namespace OperacionesTopazRN
{
    public class RnCondicion : RnGeneral
    {
        public async Task<List<EnCondicion>> frn_ListarCondiciones(EnCondicion oAutonomia)
        {
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdCondicion oadReglasParametro = new AdCondicion(con);
                    return await oadReglasParametro.fad_ListarCondiciones(oAutonomia);
                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }

        }

        public async Task<List<EnProducto>> frn_ListarProductoCondicion(EnProducto enProducto)
        {
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdCondicion oadReglasParametro = new AdCondicion(con);
                    return await oadReglasParametro.fad_ListarProductoCondicion(enProducto);
                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }
        }

 
        #region MantenerProductoCondicion
        public async Task<int> frn_MantenerProductoCondicion(List<EnCarteraProducto> ploProductos)
        {
            var tbCarteraProducto = new EnCorresponsaliaProductoType();
            try
            {
                using (SqlConnection con = new SqlConnection(sConexion))
                {
                    foreach (EnCarteraProducto oCarteraProducto in ploProductos)
                    {
                        tbCarteraProducto.Add(oCarteraProducto);
                    }

                    con.Open();
                    AdCondicion oAdCartera = new AdCondicion(con);
                    return await oAdCartera.fad_MantenerProductoCondicion(tbCarteraProducto);
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region ObtenerCondicionId_CV
        public async Task<EnCondicion> frn_ObtenerCondicionId_CV(int iIdCondicion)
        {
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdCondicion oadReglasParametro = new AdCondicion(con);
                    return await oadReglasParametro.fad_ObtenerCondicionId_CV(iIdCondicion);
                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }
        }
        #endregion

        #region ObtenerCondicionId
        public async Task<EnCondicion> frn_ObtenerCondicionId(int iIdCondicion)
        {
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdCondicion oadReglasParametro = new AdCondicion(con);
                    return await oadReglasParametro.fad_ObtenerCondicionId(iIdCondicion);
                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }
        }
        #endregion

        #region ListarCondicionesCreditosVencidos
        public async Task<List<EnCondicion>> frn_ListarCondicionesCreditosVencidos(EnCondicion oAutonomia)
        {
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdCondicion oadReglasParametro = new AdCondicion(con);
                    return await oadReglasParametro.fad_ListarCondicionesCreditoVencidos(oAutonomia);
                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }

        }
        #endregion

        #region ListarProductoCondicionCreditoVencido
        public async Task<List<EnProducto>> frn_ListarProductoCondicionCreditoVencido(EnProducto enProducto)
        {
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdCondicion oadReglasParametro = new AdCondicion(con);
                    return await oadReglasParametro.fad_ListarProductoCondicionCreditoVencido(enProducto);
                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }
        }
        #endregion

        #region ListarProductoCondicionCreditoVencidoAhorros
        public async Task<List<EnProducto>> frn_ListarProductoCondicionCreditoVencidoAhorros(EnProducto enProducto)
        {
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdCondicion oadReglasParametro = new AdCondicion(con);
                    return await oadReglasParametro.fad_ListarProductoCondicionCreditoVencidoAhorros(enProducto);
                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }
        }
        #endregion

        #region ListarProductoCondicionProductoAhorrosOrden
        public async Task<List<EnCarteraProductoAhorroOrden>> frn_ListarProductoCondicionProductoAhorrosOrden(EnCarteraProductoAhorroOrden enProducto)
        {
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdCondicion oadReglasParametro = new AdCondicion(con);
                    return await oadReglasParametro.fad_ListarProductoCondicionProductoAhorrosOrden(enProducto);
                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }
        }
        #endregion

        #region ListarProductoCondicionProductoAhorrosOrden
        public async Task<int> frn_MantenerProductoCondicionCreditoVencido(List<EnCarteraProducto> ploProductos , List<EnCarteraProducto> ploProductosAhorro, int DiasAtraso , decimal MontoMinimo)
        {
            var tbCarteraProducto = new EnCorresponsaliaProductoType();
            var tbCarteraProductoAhorro = new EnCorresponsaliaProductoType();
            try
            {
                using (SqlConnection con = new SqlConnection(sConexion))
                {
                    foreach (EnCarteraProducto oCarteraProducto in ploProductos)
                    {
                        tbCarteraProducto.Add(oCarteraProducto);
                    
                    }

                  
                        foreach (EnCarteraProducto oCarteraProductoAhorro in ploProductosAhorro)
                        {
                        tbCarteraProductoAhorro.Add(oCarteraProductoAhorro);

                        }


                        con.Open();
                    AdCondicion oAdCartera = new AdCondicion(con);
                    return await oAdCartera.fad_MantenerProductoCondicionCreditoVencido(tbCarteraProducto, tbCarteraProductoAhorro, DiasAtraso, MontoMinimo);
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region GuardarOrdenProductoAhorro
        public async Task<int> frn_GuardarOrdenProductoAhorro(List<EnCarteraProductoAhorroOrden> loProductoAhorro)
        {

            var tbCarteraProductoAhorroOrden = new EnProductoAhorroOrdenType();
            try
            {
                using (SqlConnection con = new SqlConnection(sConexion))
                {
                     
                    foreach (EnCarteraProductoAhorroOrden oCarteraProductoAhorroOrden in loProductoAhorro)
                    {
                        tbCarteraProductoAhorroOrden.Add(oCarteraProductoAhorroOrden);

                    }


                    con.Open();
                    AdCondicion oAdCartera = new AdCondicion(con);
                    return await oAdCartera.fad_GuardarOrdenProductoAhorro(tbCarteraProductoAhorroOrden);
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

    }
}



