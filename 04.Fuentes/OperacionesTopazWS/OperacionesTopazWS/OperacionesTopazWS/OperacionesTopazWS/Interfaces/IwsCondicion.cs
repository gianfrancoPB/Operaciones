using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using SatelitesEN;

namespace OperacionesTopazWS.Interfaces
{
    [ServiceContract]
    public interface IwsCondicion
    {
        [OperationContract]
        Task<List<EnCondicion>> WsListarCondiciones(EnCondicion oCondicion);

        [OperationContract]
        Task<EnCondicion> WsObtenerCondicionId(int iIdCondicion);

        [OperationContract]
        Task<List<EnProducto>> WsListarProductoCondicion(EnProducto enProducto);

        [OperationContract]
        Task<int> WsMantenerProductoCondicion(List<Int64> ploProductos, string oBase, int piCodigoCartera);

        [OperationContract]
        Task<List<EnCondicion>> WsListarCondicionesCreditosVencidos(EnCondicion oCondicion);       
        
        [OperationContract]
        Task<List<EnProducto>> WsListarProductoCondicionCreditoVencidos(EnProducto enProducto);

        [OperationContract]
        Task<List<EnProducto>> WsListarProductoCondicionCreditoVencidosAhorro(EnProducto enProducto  );

        [OperationContract]
        Task<List<EnCarteraProductoAhorroOrden>> WsListarProductoCondicionProductoAhorrosOrden(EnCarteraProductoAhorroOrden enProducto);
       
        [OperationContract]
        Task<int> WsMantenerProductoCondicionCreditoVencido(List<Int64> ploProductos, List<Int64> ploProductosAhorro, string oBase, int piCodigoCartera , int DiasAtraso , decimal MontoMinimo);
       
        [OperationContract]
        Task<int> WsGuardarOrdenProductoAhorro(List<EnCarteraProductoAhorroOrden> loProductoAhorro, string oBase, int piCodigoCartera);

        [OperationContract]
        Task<EnCondicion> WsObtenerCondicionId_CV(int iIdCondicion);

        
    }
}


