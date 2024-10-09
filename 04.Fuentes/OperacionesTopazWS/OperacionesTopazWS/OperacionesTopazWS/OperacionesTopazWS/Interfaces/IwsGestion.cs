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
    public interface IwsGestion
    {
        [OperationContract]
        EnCorresponsalia wsGeneracionDeudaCV(EnCorresponsaliaParametros Parametros);

        [OperationContract]
        EnCorresponsalia wsGeneracionDeuda(EnCorresponsaliaParametros Parametros);

        [OperationContract]
        EnCorresponsalia wsGeneracionPagos(string vRuta, string vNombre, EnCorresponsaliaParametros Parametros);

        [OperationContract]
        EnCorresponsalia wsGeneracionPagosCA(string vRuta, string vNombre, EnCorresponsaliaParametros Parametros);

        [OperationContract]
        byte[] wsObtenerArchivo(int piFlag);

        [OperationContract]
        byte[] wsObtenerPagos();
        [OperationContract]
        byte[] wsObtenerPagosCA(int iIdPagosCA);

        [OperationContract]
        byte[] wsObtenerPagosCV();

        [OperationContract]
        Task<List<EnCorresponsaliaDescargarPagos>> WsListarPago(EnCorresponsaliaParametros Parametros, string fechaInicio, string fechaFin);
        #region Reporte - AfectacionesMasivas
        [OperationContract]
        List<EnAfectacionesMasivasReporteCabecera> WsReporteAfectacionesMasivasCabecera(EnAfectacionesMasivasReporteCabecera oParametros);
        #endregion


        [OperationContract]
        byte[] wsExportarPagos();

        [OperationContract]
        byte[] wsExportarAfectacionesMasivas(EnAfectacionesMasivasReporteCabecera oParametros);
        [OperationContract]
        EnExportarDeudasCA wsExportarDeudasCA();

    }
}