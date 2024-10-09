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
    public class wsCondicion : IwsCondicion
    {
        #region WsListarCondiciones
        public async Task<List<EnCondicion>> WsListarCondiciones(EnCondicion oCondicion)
        {

            try
            {
                RnCondicion ornReglasParametro = new RnCondicion();
                return await ornReglasParametro.frn_ListarCondiciones(oCondicion);
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region WsListarProductoCondicion
        public async Task<List<EnProducto>> WsListarProductoCondicion(EnProducto enProducto)
        {
            try
            {
                RnCondicion ornReglasParametro = new RnCondicion();
                return await ornReglasParametro.frn_ListarProductoCondicion(enProducto);
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region WsMantenerProductoCondicion
        public async Task<int> WsMantenerProductoCondicion(List<Int64> ploProductos, string oBase, int piCodigoCartera)
        {
            try
            {
                var lsProdutos = new List<EnCarteraProducto>();

                foreach (var item in ploProductos)
                {

                    lsProdutos.Add(new EnCarteraProducto
                    {
                        sCodigoProducto     = item.ToString(),
                        iCodigoCartera = piCodigoCartera,
                        sNombreCartera = "",
                        vAudNombreUsuarioCreacion = oBase.Split('|')[0].ToString(),
                        dtAudFechaCreacion = DateTime.Now,
                        vAudIPCreacion = oBase.Split('|')[1].ToString(),
                        vAudMACCreacion = oBase.Split('|')[2].ToString(),
                        vAudNombreUsuarioModificacion = oBase.Split('|')[0].ToString(),
                        dtAudFechaModificacion = DateTime.Now,
                        vAudIPModificacion = oBase.Split('|')[1].ToString(),
                        vAudMACModificacion = oBase.Split('|')[2].ToString(),
                    });


                }
                RnCondicion ornReglasParametro = new RnCondicion();
                return await ornReglasParametro.frn_MantenerProductoCondicion(lsProdutos);
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region WsObtenerCondicionId_CV
        public async Task<EnCondicion> WsObtenerCondicionId_CV(int iIdCondicion)
        {
            try
            {
                RnCondicion ornReglasParametro = new RnCondicion();
                return await ornReglasParametro.frn_ObtenerCondicionId_CV(iIdCondicion);
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region WsObtenerCondicionId
        public async Task<EnCondicion> WsObtenerCondicionId(int iIdCondicion)
        {
            try
            {
                RnCondicion ornReglasParametro = new RnCondicion();
                return await ornReglasParametro.frn_ObtenerCondicionId(iIdCondicion);
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region WsListarCondicionesCreditosVencidos
        public async Task<List<EnCondicion>> WsListarCondicionesCreditosVencidos(EnCondicion oCondicion)
        {

            try
            {
                RnCondicion ornReglasParametro = new RnCondicion();
                return await ornReglasParametro.frn_ListarCondicionesCreditosVencidos(oCondicion);
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region WsListarProductoCondicionCreditoVencidos
        public async Task<List<EnProducto>> WsListarProductoCondicionCreditoVencidos(EnProducto enProducto)
        {
            try
            {
                RnCondicion ornReglasParametro = new RnCondicion();
                return await ornReglasParametro.frn_ListarProductoCondicionCreditoVencido(enProducto);
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region WsListarProductoCondicionCreditoVencidosAhorro
        public async Task<List<EnProducto>> WsListarProductoCondicionCreditoVencidosAhorro(EnProducto enProducto )
        {
            try
            {
                RnCondicion ornReglasParametro = new RnCondicion();
                return await ornReglasParametro.frn_ListarProductoCondicionCreditoVencidoAhorros(enProducto  );
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region WsListarProductoCondicionProductoAhorrosOrden
        public async Task<List<EnCarteraProductoAhorroOrden>> WsListarProductoCondicionProductoAhorrosOrden(EnCarteraProductoAhorroOrden enProducto)
        {
            try
            {
                RnCondicion ornReglasParametro = new RnCondicion();
                return await ornReglasParametro.frn_ListarProductoCondicionProductoAhorrosOrden(enProducto);
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region WsMantenerProductoCondicionCreditoVencido
        public async Task<int> WsMantenerProductoCondicionCreditoVencido(List<Int64> ploProductos, List<Int64> ploProductosAhorro, string oBase, int piCodigoCartera , int DiasAtraso , decimal MontoMinimo)
        {
            try
            {
                var lsProdutos = new List<EnCarteraProducto>();
                var lsProdutosAhorro = new List<EnCarteraProducto>();

                foreach (var item in ploProductos)
                {
                    lsProdutos.Add(new EnCarteraProducto
                    {
                        sCodigoProducto = item.ToString(),
                        iCodigoCartera = piCodigoCartera,
                        sNombreCartera = "",
                        vAudNombreUsuarioCreacion = oBase.Split('|')[0].ToString(),
                        dtAudFechaCreacion = DateTime.Now,
                        vAudIPCreacion = oBase.Split('|')[1].ToString(),
                        vAudMACCreacion = oBase.Split('|')[2].ToString(),
                        vAudNombreUsuarioModificacion = oBase.Split('|')[0].ToString(),
                        dtAudFechaModificacion = DateTime.Now,
                        vAudIPModificacion = oBase.Split('|')[1].ToString(),
                        vAudMACModificacion = oBase.Split('|')[2].ToString(),
                    });


                }
                foreach (var item in ploProductosAhorro)
                {

                    lsProdutosAhorro.Add(new EnCarteraProducto
                    {
                        sCodigoProducto = item.ToString(),
                        iCodigoCartera = piCodigoCartera,
                        sNombreCartera = "",
                        vAudNombreUsuarioCreacion = oBase.Split('|')[0].ToString(),
                        dtAudFechaCreacion = DateTime.Now,
                        vAudIPCreacion = oBase.Split('|')[1].ToString(),
                        vAudMACCreacion = oBase.Split('|')[2].ToString(),
                        vAudNombreUsuarioModificacion = oBase.Split('|')[0].ToString(),
                        dtAudFechaModificacion = DateTime.Now,
                        vAudIPModificacion = oBase.Split('|')[1].ToString(),
                        vAudMACModificacion = oBase.Split('|')[2].ToString(),
                    });
                }
                RnCondicion ornReglasParametro = new RnCondicion();
                return await ornReglasParametro.frn_MantenerProductoCondicionCreditoVencido(lsProdutos, lsProdutosAhorro, DiasAtraso, MontoMinimo);
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region WsGuardarOrdenProductoAhorro
        public async Task<int> WsGuardarOrdenProductoAhorro(List<EnCarteraProductoAhorroOrden> loProductoAhorro, string oBase, int piCodigoCartera )
        {
            try
            {
            
                var lsProdutosAhorro = new EnProductoAhorroOrdenType(); 
                foreach (var item in loProductoAhorro)

                {
                    var productoAhorro = new EnCarteraProductoAhorroOrden
                    {
                        iNumeroOrden = Convert.ToInt32(item.iNumeroOrden),
                        psDescripcionProducto = item.ToString(),
                        iCodigoProducto = item.ToString(),
                    };
                }
                RnCondicion ornReglasParametro = new RnCondicion();
                return await ornReglasParametro.frn_GuardarOrdenProductoAhorro(loProductoAhorro);
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


 