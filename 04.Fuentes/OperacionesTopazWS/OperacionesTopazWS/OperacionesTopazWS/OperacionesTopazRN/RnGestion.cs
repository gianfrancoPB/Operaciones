using System.IO;
//using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SatelitesEN;
using System.Reflection;
using UtilitarioCrk;
using OperacionesTopazAD;
using OfficeOpenXml;
using System.Drawing;
using System.Configuration;
using SeguridadCrkWCF;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using FontUnderlineType = NPOI.SS.UserModel.FontUnderlineType;
using NPOI.SS.Util;
using System.Security.Permissions;
using System.Net;
using Newtonsoft.Json;



namespace OperacionesTopazRN
{
    public class RnGestion : RnGeneral
    {
        static HSSFWorkbook hssfworkbook;
        static HSSFWorkbook workbook;

        //Modificado 1 Septiembre 2022
        public const string sMascaraFechaArchivo = "yyyy_MM_dd_HH_mm_ss";
        //Modificado 1 Septiembre 2022

        #region GeneracionDeudaCreditosVencidos
        public EnCorresponsalia rnGeneracionDeudaCV(EnCorresponsaliaParametros Parametros)
        {
            EnCorresponsalia loGeneracionDeuda = new EnCorresponsalia();
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdGestion oadGestion = new AdGestion(con);
                    loGeneracionDeuda = oadGestion.adGeneracionDeudaCV(Parametros);

                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }
            return loGeneracionDeuda;
        }
        #endregion

        #region GeneracionDeuda
        public EnCorresponsalia rnGeneracionDeuda(EnCorresponsaliaParametros Parametros)
        {
            EnCorresponsalia loGeneracionDeuda = new EnCorresponsalia();
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdGestion oadGestion = new AdGestion(con);
                    loGeneracionDeuda = oadGestion.adGeneracionDeuda(Parametros);
                    if (loGeneracionDeuda.iAccion == 2)
                    {
                        rnGeneracionDeudaArchivos(3, "Trucando");                               // TRUNCA INFOMRACION SOLES Y DOLARES
                    }

                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }
            return loGeneracionDeuda;
        }
        #endregion

        #region GeneracionPagos
        public EnCorresponsalia rnGeneracionPagosCA( string vRuta, string vNombre, EnCorresponsaliaParametros Parametros)
        {

            var sRutaCompleta = vRuta;
            var sRutaArchivo = vRuta + vNombre;


            string[] lines = null;
            string json = null; // Cambiar a string para almacenar el JSON
            try
            {
                if (Directory.Exists(sRutaCompleta))
                {

                    using (var sr = new StreamReader(sRutaArchivo))
                    {
                        var sContenido = sr.ReadToEnd();
                    }
                }
          
                lines = System.IO.File.ReadAllLines(sRutaArchivo);
                json = JsonConvert.SerializeObject(lines);

            }
            catch (Exception ex)
            {

                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }

   

            try
            {
                using (SqlConnection con = new SqlConnection(sConexion))
                {
                    con.Open();
                    AdGestion oadGestion = new AdGestion(con);
                    return    oadGestion.adGeneracionPagosCA(json, Parametros);
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        public EnCorresponsalia rnGeneracionPagos(string vRuta, string vNombre, EnCorresponsaliaParametros Parametros)
        {

            var sRutaCompleta = vRuta;
            var sRutaArchivo = vRuta + vNombre;


            string[] lines = null;

            try
            {
                if (Directory.Exists(sRutaCompleta))
                {

                    using (var sr = new StreamReader(sRutaArchivo))
                    {
                        var sContenido = sr.ReadToEnd();
                    }
                }

                lines = System.IO.File.ReadAllLines(sRutaArchivo);

            }
            catch (Exception ex)
            {

                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }

            var tbExportar_Pagos = new EnCorresponsaliaExportarType();

            try
            {
                using (SqlConnection con = new SqlConnection(sConexion))
                {
                    foreach (string line in lines)
                    {
                        EnCorresponsaliaExportar oLineas = new EnCorresponsaliaExportar();
                        oLineas.sBCP_PAGOS = line;

                        tbExportar_Pagos.Add(oLineas);
                    }
                    con.Open();
                    AdGestion oadGestion = new AdGestion(con);
                    return oadGestion.adGeneracionPagos(tbExportar_Pagos, Parametros);
                }
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region GeneracionDeudaArchivos
        public void rnGeneracionDeudaArchivos(int iAccion, string path)
        {

            List<EnCorresponsaliaArchivo> loGeneracionDeudaArchivo = new List<EnCorresponsaliaArchivo>();
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdGestion oadGestion = new AdGestion(con);
                    loGeneracionDeudaArchivo = oadGestion.adGeneracionDeudaArchivo(iAccion);
                    if (loGeneracionDeudaArchivo.Count() == 0) // Cuando no tenga registros o no exista datos para exportar.
                    {
                        string CabeceraVacia = "";
                        if (iAccion == 1)
                        {
                            CabeceraVacia = "CC19102623027C2020010500000000000000000000000066250T2VP02001135SIN MOVIMIENTOCP0000                                                                                                                                                                       " + Environment.NewLine;                            // CABECERA
                        }
                        else
                        {
                            CabeceraVacia = "CC19102623027C2020010500000000000000000000000066251T2VP02001135SIN MOVIMIENTOCP0000                                                                                                                                                                       " + Environment.NewLine;                            // CABECERA
                        }

                        File.AppendAllText(path, CabeceraVacia);
                    }
                    else
                    {
                        if (iAccion == 1)
                        {
                            int comienzo = 1;

                            foreach (var item in loGeneracionDeudaArchivo)
                            {
                                if (comienzo == 1)
                                {
                                    string Cabecera = item.BcpDeudas + Environment.NewLine;                            // CABECERA
                                    File.AppendAllText(path, Cabecera);
                                }
                                else                                                                                   //DETALLE
                                {
                                    string Detalle = item.BcpDeudas + Environment.NewLine;
                                    File.AppendAllText(path, Detalle);
                                }

                                comienzo++;
                            }
                        }
                        if (iAccion == 2)
                        {
                            int comienzo = 1;

                            foreach (var item in loGeneracionDeudaArchivo)
                            {
                                if (comienzo == 1)
                                {
                                    string Cabecera = item.BcpDeudas + Environment.NewLine;                            // CABECERA
                                    File.AppendAllText(path, Cabecera);
                                }
                                else                                                                                   //DETALLE
                                {
                                    string Detalle = item.BcpDeudas + Environment.NewLine;
                                    File.AppendAllText(path, Detalle);
                                }

                                comienzo++;
                            }
                        }
                    }

                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }
        }
        #endregion

        #region ObtenerPagos
        public byte[] rnObtenerPagos()
        {
            byte[] bPagos;
            List<EnCorresponsaliaDescargarPagos> loObtenerPagos = new List<EnCorresponsaliaDescargarPagos>();
            ExcelPackage xlsApp = null;
            String Archivo = string.Empty;
            string sRutaAlmacenamiento = "";
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    //int piTipo = 1;
                    con.Open();
                    AdGestion oadReporte = new AdGestion(con);

                    //sRutaPlantilla = "/inetpub/wwwroot/Satelites.TopazOperaciones.WS/App_Data/AMORTIZACIONES_MASIVAS.xlsx";
                    sRutaAlmacenamiento = "AMORTIZACIONES_MASIVAS_BCP_" + DateTime.Now.ToString(sMascaraFechaArchivo);
                    loObtenerPagos = oadReporte.adObtenerPagos();

                    //Creacion del metodo HSSFworkbook , y declarando el shet1 con el nombre de la pagina en el excel
                    hssfworkbook = new HSSFWorkbook();
                    HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Amortizaciones");


                    IRow rowFilaCab = sheet1.CreateRow(0);
                    rowFilaCab.CreateCell(0);
                    ICellStyle styleLeft1 = hssfworkbook.CreateCellStyle();

                    styleLeft1.Alignment = HorizontalAlignment.Right;
                    //styleLeft1.VerticalAlignment = VerticalAlignment.Top;
                    sheet1.AutoSizeColumn(0);


                    IFont boldFont = hssfworkbook.CreateFont();
                    boldFont.FontHeightInPoints = 11;
                    boldFont.IsBold = true;


                    ICellStyle boldStyle = hssfworkbook.CreateCellStyle();

                    // Creacion del style borde
                    ICellStyle blackBorder = hssfworkbook.CreateCellStyle();
                    boldStyle.BorderBottom = BorderStyle.Thin;

                    //Creacion del estilo y estilo negrita para las cabeceras 
                    boldStyle.SetFont(boldFont);
                    boldFont.FontName = ("Calibri");
                    sheet1.AutoSizeColumn(0);
                    IRow row = sheet1.CreateRow(0);
                    ICell cell = row.CreateCell(0);
                    cell.CellStyle = (boldStyle);

                    cell.SetCellValue("FECHA_PROCESO");
                    // Uso del estilo del border bootom para abajo  
                    blackBorder.BorderBottom = BorderStyle.Thin;
                    sheet1.AutoSizeColumn(0);

                    //Creacion de la cabecera Numero_Credito
                    ICell cell1 = row.CreateCell(1);
                    cell1.CellStyle = (boldStyle);
                    cell1.SetCellValue("NUMERO_CREDITO");
                    blackBorder.BorderBottom = BorderStyle.Thin;
                    sheet1.SetColumnWidth(1, 1000);
                    sheet1.AutoSizeColumn(1);

                    //Creacion de la cabecera Doi

                    ICell cell2 = row.CreateCell(2);
                    cell2.SetCellValue("DOI");
                    cell2.CellStyle = (boldStyle);
                    sheet1.SetColumnWidth(2, 3500);
                    sheet1.AutoSizeColumn(2);

                    //Creacion de la cabecera Moneda
                    ICell cell3 = row.CreateCell(3);
                    cell3.CellStyle = (boldStyle);
                    cell3.SetCellValue("MONEDA");
                    sheet1.AutoSizeColumn(3);

                    //Creacion de la cabecera Monto_Aplicar
                    ICell cell4 = row.CreateCell(4);
                    cell4.CellStyle = (boldStyle);
                    cell4.SetCellValue("MONTO_APLICAR");
                    sheet1.AutoSizeColumn(4);

                    //Creacion de la cabecera Rubro_cuenta
                    ICell cell5 = row.CreateCell(5);
                    cell5.SetCellValue("RUBRO_CUENTA");
                    cell5.CellStyle = (boldStyle);
                    sheet1.SetColumnWidth(5, 4500);

                    //Creacion de la cabecera Tipo_Movimiento
                    ICell cell6 = row.CreateCell(6);
                    cell6.SetCellValue("TIPO_MOVIMIENTO");
                    cell6.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(6);


                    //Variable que inicia desde la celda 1 
                    var iFila = 1;



                    //  FORMATO COLUMNAS  
                    IDataFormat dataFormatCustom = hssfworkbook.CreateDataFormat();
                    //  Formato o style Fecha
                    ICellStyle styleFecha = hssfworkbook.CreateCellStyle();
                    //  Para el campo numerico en enteros
                    ICellStyle styleNumeric = hssfworkbook.CreateCellStyle();
                    // Para el campo numerico en decimales
                    ICellStyle styleNumericDecimal = hssfworkbook.CreateCellStyle();
                    //  Estilo rigcht (Izquierdo)
                    ICellStyle styleRight = hssfworkbook.CreateCellStyle();
                    ICellStyle styleRightDoi = hssfworkbook.CreateCellStyle();
                    //   Estilo Tipo texto
                    ICellStyle styletext = hssfworkbook.CreateCellStyle();
                    ICellStyle styleRightRubrocuenta = hssfworkbook.CreateCellStyle();


                    //Creacion de los formatos 
                    IDataFormat format = hssfworkbook.CreateDataFormat();
                    IDataFormat format2 = hssfworkbook.CreateDataFormat();


                    //For que jala el metodo que trae informacion desde la base de datos 
                    foreach (var campo in loObtenerPagos)
                    {

                        //Creacion de las filas 
                        IRow rowFila = sheet1.CreateRow(iFila);
                        //Creacion del estilo bold o negrita
                        IFont boldFont1 = hssfworkbook.CreateFont();
                        //Estilo de las CellFila para el llamado de los estilos
                        ICell cellFilla = rowFila.CreateCell(0);


                        //Creacion de la columna Fecha Proceso que va debajo de su cabecera 
                        rowFila = sheet1.CreateRow(iFila);
                        boldFont1 = hssfworkbook.CreateFont();
                        cell = sheet1.CreateRow(iFila).CreateCell(0);
                        cell.CellStyle = styleFecha;
                        cell.CellStyle = styleRight;
                        cell.CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
                        cell.SetCellValue(DateTime.Parse(campo.sFecha_Proceso));
                        cell.CellStyle = styleRight;


                        //Creacion de la columna Numero_Credito , con su respectivo estilo a la derecha
                        //Tipo de data , convertido a string para que coja la cantidad necesaria de los digitos 
                        rowFila.CreateCell(1).SetCellValue(Convert.ToString(campo.sNumero_Credito));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(1).CellStyle = styleLeft1;
                        cell.CellStyle = styleRight;


                        //Creacion de la columna Doi  con sus respectivos estilos y tipo de dato
                        ICell cell33 = rowFila.CreateCell(2);
                        cell33.CellStyle = styletext;
                        cell33.CellStyle = styleRightDoi;
                        styleRightDoi.Alignment = HorizontalAlignment.Right;
                        styleRightDoi.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(2).CellStyle = styletext;
                        rowFila.GetCell(2).CellStyle = styleRightDoi;
                        cell33.CellStyle.DataFormat = dataFormatCustom.GetFormat("text");
                        cell33.SetCellValue(campo.sDOI);


                        //Creacion de la columna moneda , con su data convertido a float
                        rowFila.CreateCell(3).SetCellValue(float.Parse(campo.sMoneda));


                        //Creacion de la columna Decimal
                        ICell cell21 = rowFila.CreateCell(4);
                        cell21.CellStyle = styleNumericDecimal;
                        //Tipo de dato numerico
                        // Alineamiento a la derecha en la parte inferior
                        styleNumericDecimal.Alignment = HorizontalAlignment.Right;
                        styleNumericDecimal.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(4).CellStyle = styleLeft1;
                        cell21.CellStyle = styleNumericDecimal;
                        //Formato del campo monto_aplicar en numeric con dos decimales a la derecha
                        cell21.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                        cell21.SetCellValue((double)campo.dMonto_Aplicar);


                        //Creacion de la columna Rubro cuenta con sus respectivos estilos y f<ormato de data
                        ICell cell20 = rowFila.CreateCell(5);
                        cell20.CellStyle = styleNumeric;
                        cell20.CellStyle = styleRightRubrocuenta;
                        //Estilo a la derecha en la parte inferior
                        styleRightRubrocuenta.Alignment = HorizontalAlignment.Right;
                        styleRightRubrocuenta.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(5).CellStyle = styleLeft1;
                        cell20.CellStyle = styleRightRubrocuenta;
                        // Stilo del formato en enteros Int
                        cell20.CellStyle.DataFormat = dataFormatCustom.GetFormat("0");
                        cell20.SetCellValue(Convert.ToInt64(campo.sRubro_Cuenta));



                        //Creacion de la columna TipoMovimiento
                        rowFila.CreateCell(6).SetCellValue(Convert.ToString(campo.sTipo_Movimiento));



                        //Variable de inicio que se incrementa de acuerdo a la cantidad de data que recorra el for each 
                        iFila++;
                    }


                    //  VARIABLES PARA RUTA DEL ARCHIVO TEMPORAL 
                    sRutaAlmacenamiento = "AMORTIZACIONES_MASIVAS/" + DateTime.Now.ToString(sMascaraFechaArchivo);



                    //Ruta de descarga del archivo xls , en el servidor 74 
                    Archivo = @"C:\/\Users\Public\Downloads";
                    //Archivo = System.IO.Path.GetTempPath() + sRutaAlmacenamiento;

                    //Tipo de extension en .xls
                    Archivo = Archivo + ".xls";

                    // En el file 2 se guarda la ruta del archivo + su extension
                    FileInfo newFile = new FileInfo(Archivo);
                    FileStream file2 = new FileStream(Archivo, FileMode.Create);

                    //  ESCRIBE EL ARCHIVO EXCELL 
                    hssfworkbook.Write(file2);
                    file2.Close();

                    bPagos = System.IO.File.ReadAllBytes(Archivo);
                    System.IO.File.Delete(Archivo);

                }
                catch (Exception ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    bPagos = null;
                    throw ex;
                }
                finally
                {
                    if (xlsApp != null)
                        xlsApp.Dispose();

                    xlsApp = null;
                }
            }

            return bPagos;
        }
        public byte[] rnObtenerPagosCA(int iIdPagosCA)
        {
            byte[] bPagos;
            List<EnCorresponsaliaDescargarPagos> loObtenerPagos = new List<EnCorresponsaliaDescargarPagos>();
            ExcelPackage xlsApp = null;
            String Archivo = string.Empty;
            string sRutaAlmacenamiento = "";
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    //int piTipo = 1;
                    con.Open();
                    AdGestion oadReporte = new AdGestion(con);

                    //sRutaPlantilla = "/inetpub/wwwroot/Satelites.TopazOperaciones.WS/App_Data/AMORTIZACIONES_MASIVAS.xlsx";
                    sRutaAlmacenamiento = "AMORTIZACIONES_MASIVAS_BCP_" + DateTime.Now.ToString(sMascaraFechaArchivo);
                    loObtenerPagos = oadReporte.adObtenerPagosCA(iIdPagosCA);

                    //Creacion del metodo HSSFworkbook , y declarando el shet1 con el nombre de la pagina en el excel
                    hssfworkbook = new HSSFWorkbook();
                    HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Amortizaciones");


                    IRow rowFilaCab = sheet1.CreateRow(0);
                    rowFilaCab.CreateCell(0);
                    ICellStyle styleLeft1 = hssfworkbook.CreateCellStyle();

                    styleLeft1.Alignment = HorizontalAlignment.Right;
                    //styleLeft1.VerticalAlignment = VerticalAlignment.Top;
                    sheet1.AutoSizeColumn(0);


                    IFont boldFont = hssfworkbook.CreateFont();
                    boldFont.FontHeightInPoints = 11;
                    boldFont.IsBold = true;


                    ICellStyle boldStyle = hssfworkbook.CreateCellStyle();

                    // Creacion del style borde
                    ICellStyle blackBorder = hssfworkbook.CreateCellStyle();
                    boldStyle.BorderBottom = BorderStyle.Thin;

                    //Creacion del estilo y estilo negrita para las cabeceras 
                    boldStyle.SetFont(boldFont);
                    boldFont.FontName = ("Calibri");
                    sheet1.AutoSizeColumn(0);
                    IRow row = sheet1.CreateRow(0);
                    ICell cell = row.CreateCell(0);
                    cell.CellStyle = (boldStyle);

                    cell.SetCellValue("FECHA_PROCESO");
                    // Uso del estilo del border bootom para abajo  
                    blackBorder.BorderBottom = BorderStyle.Thin;
                    sheet1.AutoSizeColumn(0);

                    //Creacion de la cabecera Numero_Credito
                    ICell cell1 = row.CreateCell(1);
                    cell1.CellStyle = (boldStyle);
                    cell1.SetCellValue("NUMERO_CREDITO");
                    blackBorder.BorderBottom = BorderStyle.Thin;
                    sheet1.SetColumnWidth(1, 1000);
                    sheet1.AutoSizeColumn(1);

                    //Creacion de la cabecera Doi

                    ICell cell2 = row.CreateCell(2);
                    cell2.SetCellValue("DOI");
                    cell2.CellStyle = (boldStyle);
                    sheet1.SetColumnWidth(2, 3500);
                    sheet1.AutoSizeColumn(2);

                    //Creacion de la cabecera Moneda
                    ICell cell3 = row.CreateCell(3);
                    cell3.CellStyle = (boldStyle);
                    cell3.SetCellValue("MONEDA");
                    sheet1.AutoSizeColumn(3);

                    //Creacion de la cabecera Monto_Aplicar
                    ICell cell4 = row.CreateCell(4);
                    cell4.CellStyle = (boldStyle);
                    cell4.SetCellValue("MONTO_APLICAR");
                    sheet1.AutoSizeColumn(4);

                    //Creacion de la cabecera Rubro_cuenta
                    ICell cell5 = row.CreateCell(5);
                    cell5.SetCellValue("RUBRO_CUENTA");
                    cell5.CellStyle = (boldStyle);
                    sheet1.SetColumnWidth(5, 4500);

                    //Creacion de la cabecera Tipo_Movimiento
                    ICell cell6 = row.CreateCell(6);
                    cell6.SetCellValue("TIPO_MOVIMIENTO");
                    cell6.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(6);


                    //Variable que inicia desde la celda 1 
                    var iFila = 1;



                    //  FORMATO COLUMNAS  
                    IDataFormat dataFormatCustom = hssfworkbook.CreateDataFormat();
                    //  Formato o style Fecha
                    ICellStyle styleFecha = hssfworkbook.CreateCellStyle();
                    //  Para el campo numerico en enteros
                    ICellStyle styleNumeric = hssfworkbook.CreateCellStyle();
                    // Para el campo numerico en decimales
                    ICellStyle styleNumericDecimal = hssfworkbook.CreateCellStyle();
                    //  Estilo rigcht (Izquierdo)
                    ICellStyle styleRight = hssfworkbook.CreateCellStyle();
                    ICellStyle styleRightDoi = hssfworkbook.CreateCellStyle();
                    //   Estilo Tipo texto
                    ICellStyle styletext = hssfworkbook.CreateCellStyle();
                    ICellStyle styleRightRubrocuenta = hssfworkbook.CreateCellStyle();


                    //Creacion de los formatos 
                    IDataFormat format = hssfworkbook.CreateDataFormat();
                    IDataFormat format2 = hssfworkbook.CreateDataFormat();


                    //For que jala el metodo que trae informacion desde la base de datos 
                    foreach (var campo in loObtenerPagos)
                    {

                        //Creacion de las filas 
                        IRow rowFila = sheet1.CreateRow(iFila);
                        //Creacion del estilo bold o negrita
                        IFont boldFont1 = hssfworkbook.CreateFont();
                        //Estilo de las CellFila para el llamado de los estilos
                        ICell cellFilla = rowFila.CreateCell(0);


                        //Creacion de la columna Fecha Proceso que va debajo de su cabecera 
                        rowFila = sheet1.CreateRow(iFila);
                        boldFont1 = hssfworkbook.CreateFont();
                        cell = sheet1.CreateRow(iFila).CreateCell(0);
                        cell.CellStyle = styleFecha;
                        cell.CellStyle = styleRight;
                        cell.CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
                        cell.SetCellValue(DateTime.Parse(campo.sFecha_Proceso));
                        cell.CellStyle = styleRight;


                        //Creacion de la columna Numero_Credito , con su respectivo estilo a la derecha
                        //Tipo de data , convertido a string para que coja la cantidad necesaria de los digitos 
                        rowFila.CreateCell(1).SetCellValue(Convert.ToString(campo.sNumero_Credito));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(1).CellStyle = styleLeft1;
                        cell.CellStyle = styleRight;


                        //Creacion de la columna Doi  con sus respectivos estilos y tipo de dato
                        ICell cell33 = rowFila.CreateCell(2);
                        cell33.CellStyle = styletext;
                        cell33.CellStyle = styleRightDoi;
                        styleRightDoi.Alignment = HorizontalAlignment.Right;
                        styleRightDoi.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(2).CellStyle = styletext;
                        rowFila.GetCell(2).CellStyle = styleRightDoi;
                        cell33.CellStyle.DataFormat = dataFormatCustom.GetFormat("text");
                        cell33.SetCellValue(campo.sDOI);


                        //Creacion de la columna moneda , con su data convertido a float
                        rowFila.CreateCell(3).SetCellValue(float.Parse(campo.sMoneda));


                        //Creacion de la columna Decimal
                        ICell cell21 = rowFila.CreateCell(4);
                        cell21.CellStyle = styleNumericDecimal;
                        //Tipo de dato numerico
                        // Alineamiento a la derecha en la parte inferior
                        styleNumericDecimal.Alignment = HorizontalAlignment.Right;
                        styleNumericDecimal.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(4).CellStyle = styleLeft1;
                        cell21.CellStyle = styleNumericDecimal;
                        //Formato del campo monto_aplicar en numeric con dos decimales a la derecha
                        cell21.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                        cell21.SetCellValue((double)campo.dMonto_Aplicar);


                        //Creacion de la columna Rubro cuenta con sus respectivos estilos y f<ormato de data
                        ICell cell20 = rowFila.CreateCell(5);
                        cell20.CellStyle = styleNumeric;
                        cell20.CellStyle = styleRightRubrocuenta;
                        //Estilo a la derecha en la parte inferior
                        styleRightRubrocuenta.Alignment = HorizontalAlignment.Right;
                        styleRightRubrocuenta.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(5).CellStyle = styleLeft1;
                        cell20.CellStyle = styleRightRubrocuenta;
                        // Stilo del formato en enteros Int
                        cell20.CellStyle.DataFormat = dataFormatCustom.GetFormat("0");
                        cell20.SetCellValue(Convert.ToInt64(campo.sRubro_Cuenta));



                        //Creacion de la columna TipoMovimiento
                        rowFila.CreateCell(6).SetCellValue(Convert.ToString(campo.sTipo_Movimiento));



                        //Variable de inicio que se incrementa de acuerdo a la cantidad de data que recorra el for each 
                        iFila++;
                    }

                    sheet1.SetColumnWidth(2, 6000); // Ajustar la columna DOI manualmente si lo deseas

                    //  VARIABLES PARA RUTA DEL ARCHIVO TEMPORAL 
                    sRutaAlmacenamiento = "AMORTIZACIONES_MASIVAS/" + DateTime.Now.ToString(sMascaraFechaArchivo);



                    //Ruta de descarga del archivo xls , en el servidor 74 
                    Archivo = @"C:\/\Users\Public\Downloads";
                    //Archivo = System.IO.Path.GetTempPath() + sRutaAlmacenamiento;

                    //Tipo de extension en .xls
                    Archivo = Archivo + ".xls";

                    // En el file 2 se guarda la ruta del archivo + su extension
                    FileInfo newFile = new FileInfo(Archivo);
                    FileStream file2 = new FileStream(Archivo, FileMode.Create);

                    //  ESCRIBE EL ARCHIVO EXCELL 
                    hssfworkbook.Write(file2);
                    file2.Close();

                    bPagos = System.IO.File.ReadAllBytes(Archivo);
                    System.IO.File.Delete(Archivo);

                }
                catch (Exception ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    bPagos = null;
                    throw ex;
                }
                finally
                {
                    if (xlsApp != null)
                        xlsApp.Dispose();

                    xlsApp = null;
                }
            }

            return bPagos;
        }
        #endregion

        #region ListarPago
        public async Task<List<EnCorresponsaliaDescargarPagos>>frn_ListarPago(EnCorresponsaliaParametros Parametros, string fechaInicio, string fechaFin)
        {
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdGestion oadReglasParametro = new AdGestion(con);
                    return await oadReglasParametro.fad_ListarPago(Parametros,  fechaInicio,  fechaFin);
                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }
        }
        #endregion

        #region ReporteAfectacionesMasivasCabecera
        public List<EnAfectacionesMasivasReporteCabecera> frn_ReporteAfectacionesMasivasCabecera(EnAfectacionesMasivasReporteCabecera oParametros)
        {
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdGestion oadGestion = new AdGestion(con);
                    return oadGestion.fad_ReporteAfectacionesMasivasCabecera(oParametros);
                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }
        }
        #endregion

        #region ObtenerPagosCreditosVencidos
        public byte[] rnObtenerPagosCV()
        {
            byte[] bPagos;
            List<EnCorresponsaliaDescargarPagos> loObtenerPagos = new List<EnCorresponsaliaDescargarPagos>();
            ExcelPackage xlsApp = null;
            String Archivo = string.Empty;
            string sRutaAlmacenamiento = "";
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdGestion oadReporte = new AdGestion(con);

                    //sRutaPlantilla = "/inetpub/wwwroot/Satelites.TopazOperaciones.WS/App_Data/AMORTIZACIONES_MASIVAS.xlsx";
                    sRutaAlmacenamiento = "AMORTIZACIONES_MASIVAS_BCP_" + DateTime.Now.ToString(sMascaraFechaArchivo);
                    loObtenerPagos = oadReporte.adObtenerPagosCV();
                      if (loObtenerPagos == null || loObtenerPagos.Count() == 0)
                    {
                        return null;
                    }

                    //Creacion del metodo HSSFworkbook , y declarando el shet1 con el nombre de la pagina en el excel
                    hssfworkbook = new HSSFWorkbook();
                    HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Amortizaciones");


                    IRow rowFilaCab = sheet1.CreateRow(0);
                    rowFilaCab.CreateCell(0);
                    ICellStyle styleLeft1 = hssfworkbook.CreateCellStyle();

                    styleLeft1.Alignment = HorizontalAlignment.Right;
                    //styleLeft1.VerticalAlignment = VerticalAlignment.Top;
                    sheet1.AutoSizeColumn(0);


                    IFont boldFont = hssfworkbook.CreateFont();
                    boldFont.FontHeightInPoints = 11;
                    boldFont.IsBold = true;

                    ICellStyle boldStyle = hssfworkbook.CreateCellStyle();

                    // Creacion del style borde
                    ICellStyle blackBorder = hssfworkbook.CreateCellStyle();
                    boldStyle.BorderBottom = BorderStyle.Thin;

                    //Creacion del estilo y estilo negrita para las cabeceras 
                    boldStyle.SetFont(boldFont);
                    boldFont.FontName = ("Calibri");
                    sheet1.AutoSizeColumn(0);
                    IRow row = sheet1.CreateRow(0);
                    ICell cell = row.CreateCell(0);
                    cell.CellStyle = (boldStyle);

                    cell.SetCellValue("FECHA_PROCESO");
                    // Uso del estilo del border bootom para abajo  
                    blackBorder.BorderBottom = BorderStyle.Thin;
                    sheet1.AutoSizeColumn(0);

                    //Creacion de la cabecera Numero_Credito
                    ICell cell1 = row.CreateCell(1);
                    cell1.CellStyle = (boldStyle);
                    cell1.SetCellValue("NUMERO_CREDITO");
                    blackBorder.BorderBottom = BorderStyle.Thin;
                    sheet1.AutoSizeColumn(1);

                    //Creacion de la cabecera Doi
                    //sheet1.SetColumnWidth(3, 5000);
                    ICell cell2 = row.CreateCell(2);
                    cell2.SetCellValue("DOI");
                    cell2.CellStyle = (boldStyle);
                    //sheet1.SetColumnWidth(2, 3500);
                    sheet1.SetColumnWidth(2, 6000); // Establece el ancho de la columna a 3000
                  //  sheet1.AutoSizeColumn(2);

                    //Creacion de la cabecera Moneda
                    ICell cell3 = row.CreateCell(3);
                    cell3.CellStyle = (boldStyle);
                    cell3.SetCellValue("MONEDA");
                    sheet1.AutoSizeColumn(3);

                    //Creacion de la cabecera Monto_Aplicar
                    ICell cell4 = row.CreateCell(4);
                    cell4.CellStyle = (boldStyle);
                    cell4.SetCellValue("MONTO_APLICAR");
                    sheet1.AutoSizeColumn(4);

                    //Creacion de la cabecera Rubro_cuenta
                    ICell cell5 = row.CreateCell(5);
                    cell5.SetCellValue("RUBRO_CUENTA");
                    cell5.CellStyle = (boldStyle);
                    sheet1.SetColumnWidth(5, 4500);

                    //Creacion de la cabecera Tipo_Movimiento
                    ICell cell6 = row.CreateCell(6);
                    cell6.SetCellValue("TIPO_MOVIMIENTO");
                    cell6.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(6);


                    //Variable que inicia desde la celda 1 
                    var iFila = 1;



                    //  FORMATO COLUMNAS  
                    IDataFormat dataFormatCustom = hssfworkbook.CreateDataFormat();
                    //  Formato o style Fecha
                    ICellStyle styleFecha = hssfworkbook.CreateCellStyle();
                    //  Para el campo numerico en enteros
                    ICellStyle styleNumeric = hssfworkbook.CreateCellStyle();
                    // Para el campo numerico en decimales
                    ICellStyle styleNumericDecimal = hssfworkbook.CreateCellStyle();
                    //  Estilo rigcht (Izquierdo)
                    ICellStyle styleRight = hssfworkbook.CreateCellStyle();
                    ICellStyle styleRightDoi = hssfworkbook.CreateCellStyle();
                    //   Estilo Tipo texto
                    ICellStyle styletext = hssfworkbook.CreateCellStyle();
                    ICellStyle styleRightRubrocuenta = hssfworkbook.CreateCellStyle();


                    //Creacion de los formatos 
                    IDataFormat format = hssfworkbook.CreateDataFormat();
                    IDataFormat format2 = hssfworkbook.CreateDataFormat();


                    //For que jala el metodo que trae informacion desde la base de datos 
                    foreach (var campo in loObtenerPagos)
                    {

                        //Creacion de las filas 
                        IRow rowFila = sheet1.CreateRow(iFila);
                        //Creacion del estilo bold o negrita
                        IFont boldFont1 = hssfworkbook.CreateFont();
                        //Estilo de las CellFila para el llamado de los estilos
                        ICell cellFilla = rowFila.CreateCell(0);


                        //Creacion de la columna Fecha Proceso que va debajo de su cabecera 
                        //Creacion del formato fecha 
                        //( cell.CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
                        //( cell.SetCellValue(DateTime.Parse(campo.sFecha_Proceso));     )
                        rowFila = sheet1.CreateRow(iFila);
                        boldFont1 = hssfworkbook.CreateFont();
                        cell = sheet1.CreateRow(iFila).CreateCell(0);
                        cell.CellStyle = styleFecha;
                        cell.CellStyle = styleRight;
                        cell.CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
                        cell.SetCellValue(DateTime.Parse(campo.sFecha_Proceso));
                        cell.CellStyle = styleRight;


                        //Creacion de la columna Numero_Credito , con su respectivo estilo a la derecha
                        //Tipo de data , convertido a string para que coja la cantidad necesaria de los digitos 
                        rowFila.CreateCell(1).SetCellValue(Convert.ToString(campo.sNumero_Credito));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(1).CellStyle = styleLeft1;
                        cell.CellStyle = styleRight;


                        //Creacion de la columna Doi  con sus respectivos estilos y tipo de dato
                        ICell cell33 = rowFila.CreateCell(2);
                        cell33.CellStyle = styletext;
                        cell33.CellStyle = styleRightDoi;
                        styleRightDoi.Alignment = HorizontalAlignment.Right;
                        styleRightDoi.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(2).CellStyle = styletext;
                        rowFila.GetCell(2).CellStyle = styleRightDoi;
                        cell33.CellStyle.DataFormat = dataFormatCustom.GetFormat("text");
                        cell33.SetCellValue(campo.sDOI);


                        //Creacion de la columna moneda , con su data convertido a float
                        rowFila.CreateCell(3).SetCellValue(float.Parse(campo.sMoneda));


                        //Creacion de la columna Decimal
                        ICell cell21 = rowFila.CreateCell(4);
                        cell21.CellStyle = styleNumericDecimal;
                        //Tipo de dato numerico
                        // Alineamiento a la derecha en la parte inferior
                        styleNumericDecimal.Alignment = HorizontalAlignment.Right;
                        styleNumericDecimal.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(4).CellStyle = styleLeft1;
                        cell21.CellStyle = styleNumericDecimal;
                        //Formato del campo monto_aplicar en numeric con dos decimales a la derecha
                        cell21.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                        cell21.SetCellValue((double)campo.dMonto_Aplicar);


                        //Creacion de la columna Rubro cuenta con sus respectivos estilos y f<ormato de data
                        ICell cell20 = rowFila.CreateCell(5);
                        cell20.CellStyle = styleNumeric;
                        cell20.CellStyle = styleRightRubrocuenta;
                        //Estilo a la derecha en la parte inferior
                        styleRightRubrocuenta.Alignment = HorizontalAlignment.Right;
                        styleRightRubrocuenta.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(5).CellStyle = styleLeft1;
                        cell20.CellStyle = styleRightRubrocuenta;
                        // Stilo del formato en enteros Int
                        cell20.CellStyle.DataFormat = dataFormatCustom.GetFormat("0");
                        cell20.SetCellValue(Convert.ToInt64(campo.sRubro_Cuenta));



                        //Creacion de la columna TipoMovimiento
                        rowFila.CreateCell(6).SetCellValue(Convert.ToString(campo.sTipo_Movimiento));


                        //Variable de inicio que se incrementa de acuerdo a la cantidad de data que recorra el for each 
                        iFila++;
                    }


                    sheet1.AutoSizeColumn(2);

                    //  VARIABLES PARA RUTA DEL ARCHIVO TEMPORAL 
                    sRutaAlmacenamiento = "AMORTIZACIONES_MASIVAS/" + DateTime.Now.ToString(sMascaraFechaArchivo);


                    //Ruta de descarga del archivo xls , en el servidor 74 
                    Archivo = @"C:\/\Users\Public\Downloads";
                    //Archivo = System.IO.Path.GetTempPath() + sRutaAlmacenamiento; Ruta para provar localmente

                    //Tipo de extension en .xls
                    Archivo = Archivo + ".xls";

                    // En el file 2 se guarda la ruta del archivo + su extension
                    FileInfo newFile = new FileInfo(Archivo);
                    FileStream file2 = new FileStream(Archivo, FileMode.Create);

                    //  ESCRIBE EL ARCHIVO EXCELL 
                    hssfworkbook.Write(file2);
                    file2.Close();

                    bPagos = System.IO.File.ReadAllBytes(Archivo);
                    System.IO.File.Delete(Archivo);

                }
                catch (Exception ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    bPagos = null;
                    throw ex;
                }
                finally
                {
                    if (xlsApp != null)
                        xlsApp.Dispose();

                    xlsApp = null;
                }
            }

            return bPagos;
        }
        #endregion

        #region GenerarExcelPagos
        public ExcelPackage GenerarExcelPagos(List<EnCorresponsaliaDescargarPagos> pListado, ExcelPackage pxlsApp)
        {

            ExcelWorksheet xlsSheet = pxlsApp.Workbook.Worksheets[1];
            try
            {
                int comienzo = 2;

                foreach (EnCorresponsaliaDescargarPagos oItem in pListado)
                {

                    xlsSheet.Cells[comienzo, 1].Value = oItem.sFecha_Proceso;
                    xlsSheet.Cells[comienzo, 1].Style.Numberformat.Format = "dd/MM/yyyy";
                    xlsSheet.Cells[comienzo, 2].Value = Convert.ToInt64(oItem.sNumero_Credito);
                    xlsSheet.Cells[comienzo, 2].Style.Numberformat.Format = "0";
                    xlsSheet.Cells[comienzo, 3].Value = Convert.ToString(oItem.sDOI);
                    xlsSheet.Cells[comienzo, 4].Value = Convert.ToInt64(oItem.sMoneda);
                    xlsSheet.Cells[comienzo, 4].Style.Numberformat.Format = "0";
                    xlsSheet.Cells[comienzo, 5].Value = oItem.dMonto_Aplicar;
                    xlsSheet.Cells[comienzo, 5].Style.Numberformat.Format = "0.00";
                    xlsSheet.Cells[comienzo, 6].Value = Convert.ToInt64(oItem.sRubro_Cuenta);
                    xlsSheet.Cells[comienzo, 6].Style.Numberformat.Format = "0";
                    xlsSheet.Cells[comienzo, 7].Value = oItem.sTipo_Movimiento;

                    comienzo++;
                }
                return pxlsApp;
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region GenerarExcelPagosCreditoVencidos
        public ExcelPackage GenerarExcelPagosCV(List<EnCorresponsaliaDescargarPagos> pListado, ExcelPackage pxlsApp)
        {
            ExcelWorksheet xlsSheet = pxlsApp.Workbook.Worksheets[1];
            try
            {
                int comienzo = 2;

                foreach (EnCorresponsaliaDescargarPagos oItem in pListado)
                {
                    xlsSheet.Cells[comienzo, 1].Value = oItem.sFecha_Proceso;
                    xlsSheet.Cells[comienzo, 1].Style.Numberformat.Format = "dd/MM/yyyy";
                    xlsSheet.Cells[comienzo, 2].Value = Convert.ToInt64(oItem.sNumero_Credito);
                    xlsSheet.Cells[comienzo, 2].Style.Numberformat.Format = "0";
                    xlsSheet.Cells[comienzo, 3].Value = Convert.ToString(oItem.sDOI);
                    xlsSheet.Cells[comienzo, 4].Value = Convert.ToInt64(oItem.sMoneda);
                    xlsSheet.Cells[comienzo, 4].Style.Numberformat.Format = "0";
                    xlsSheet.Cells[comienzo, 5].Value = oItem.dMonto_Aplicar;
                    xlsSheet.Cells[comienzo, 5].Style.Numberformat.Format = "0.00";
                    xlsSheet.Cells[comienzo, 6].Value = Convert.ToInt64(oItem.sRubro_Cuenta);
                    xlsSheet.Cells[comienzo, 6].Style.Numberformat.Format = "0";
                    xlsSheet.Cells[comienzo, 7].Value = oItem.sTipo_Movimiento;

                    comienzo++;
                }
                return pxlsApp;
            }
            catch (Exception ex)
            {
                UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                throw ex;
            }
        }
        #endregion

        #region ObtenerArchivo
        public byte[] rnObtenerArchivo(int piTipo)
        {
            byte[] bOficina;

            List<EnCorresponsaliaArchivo> loGeneracionDeudaArchivo = null;//new List<EnCorresponsaliaArchivo>();
            String Archivo = string.Empty;
            string sRutaAlmacenamiento = "";
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdGestion oadReporte = new AdGestion(con);

                    switch (piTipo)
                    {
                        case 1:
                            sRutaAlmacenamiento = "CREP1";  // *1   :: Archivo soles = CREP1
                            break;
                        case 2:
                            sRutaAlmacenamiento = "CREP2";  // * 2   :: Archivo dolares = CREP2
                            break;

                    }

                    Archivo = System.IO.Path.GetTempPath() + sRutaAlmacenamiento;
                    Archivo = Archivo + ".txt";

                    rnGeneracionDeudaArchivos(piTipo, Archivo);


                    bOficina = System.IO.File.ReadAllBytes(Archivo);
                    System.IO.File.Delete(Archivo);

                }
                catch (Exception ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    bOficina = null;
                    throw ex;
                }

            }
            return bOficina;
        }
        #endregion

        #region ExportarPagos
        public byte[] rnExportarPagos()
        {
            byte[] bPagos;
            List<EnCorresponsaliaDescargarPagos> loObtenerPagos = new List<EnCorresponsaliaDescargarPagos>();
            ExcelPackage xlsApp = null;
            String Archivo = string.Empty;
            string sRutaAlmacenamiento = "";
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdGestion oadReporte = new AdGestion(con);

                    //sRutaPlantilla = "/inetpub/wwwroot/Satelites.TopazOperaciones.WS/App_Data/AMORTIZACIONES_MASIVAS.xlsx";
                    sRutaAlmacenamiento = "REPORTES_PAGO" + DateTime.Now.ToString(sMascaraFechaArchivo);
                    loObtenerPagos = oadReporte.adListadoPagos();

                    //Creacion del metodo HSSFworkbook , y declarando el shet1 con el nombre de la pagina en el excel
                    hssfworkbook = new HSSFWorkbook();
                    HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Reporte_Pago");

                    IRow rowFilaCab = sheet1.CreateRow(0);
                    rowFilaCab.CreateCell(0);
                    ICellStyle styleLeft1 = hssfworkbook.CreateCellStyle();

                    styleLeft1.Alignment = HorizontalAlignment.Right;
                    //styleLeft1.VerticalAlignment = VerticalAlignment.Top;
                    sheet1.AutoSizeColumn(0);

                    ICellStyle styleLeft2 = hssfworkbook.CreateCellStyle();

                    styleLeft2.Alignment = HorizontalAlignment.Center;


                    IFont boldFont = hssfworkbook.CreateFont();
                    boldFont.FontHeightInPoints = 11;
                    boldFont.IsBold = true;
 

                    ICellStyle boldStyle = hssfworkbook.CreateCellStyle();

                    // Creacion del style borde
                    ICellStyle blackBorder = hssfworkbook.CreateCellStyle();
                    boldStyle.BorderBottom = BorderStyle.Thin;



                    // ############     Color fondo Inicio

                    HSSFWorkbook workbook = new HSSFWorkbook();
                    HSSFPalette palette = workbook.GetCustomPalette();
                    palette.SetColorAtIndex(HSSFColor.Orange.Index, (byte)246, (byte)142, (byte)108);
                    //HSSFColor myColor = palette.AddColor((byte)253, (byte)0, (byte)0);

                 
                    ICellStyle style001 = workbook.CreateCellStyle();
                    style001.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Orange.Index;
                    style001.FillPattern = FillPattern.SolidForeground;
                 

                    // ############     Color fondo Fin

 

                    //Creacion del estilo y estilo negrita para las cabeceras 
                    boldStyle.SetFont(boldFont);
                    boldFont.FontName = ("Calibri");
                    sheet1.AutoSizeColumn(0);
                    IRow row = sheet1.CreateRow(0);
                    ICell cell = row.CreateCell(0);
                    cell.CellStyle = (boldStyle); 
                    cell.SetCellValue("  N#");
                    // Uso del estilo del border bootom para abajo  
                    blackBorder.BorderBottom = BorderStyle.Thin;                                  
                    //sheet1.AutoSizeColumn(0);
                    sheet1.SetColumnWidth(0, 1280);
                    //cell.CellStyle = style001;


                    //Creacion de la cabecera Numero_Credito
                    ICell cell1 = row.CreateCell(1);
                    cell1.CellStyle = (boldStyle);
                    cell1.SetCellValue("            Doi"); 
                    blackBorder.BorderBottom = BorderStyle.Thin;
                  
                    //sheet1.AutoSizeColumn(1);
                    sheet1.SetColumnWidth(1, 3500);


                    ICell cell2 = row.CreateCell(2);
                    cell2.SetCellValue("                                      NombreCliente");
                    cell2.CellStyle = (boldStyle);
                    sheet1.SetColumnWidth(2, 12500);


                    ICell cell3 = row.CreateCell(3);
                    cell3.CellStyle = (boldStyle);
                    cell3.SetCellValue(" Procesado");
                    sheet1.AutoSizeColumn(3);


                    //Creacion de la cabecera Monto_Aplicar
                    ICell cell4 = row.CreateCell(4);
                    cell4.CellStyle = (boldStyle);
                    cell4.SetCellValue("    Rubro Cuenta");
                    sheet1.AutoSizeColumn(4);


                    //Creacion de la cabecera Rubro_cuenta
                    ICell cell5 = row.CreateCell(5);
                    cell5.SetCellValue("    Codigo Estado");
                    cell5.CellStyle = (boldStyle);
                    sheet1.SetColumnWidth(5, 4500);


                    //Creacion de la cabecera Tipo_Movimiento
                    ICell cell6 = row.CreateCell(6);
                    cell6.SetCellValue("Asiento_Contable");
                    cell6.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(6);


                    ICell cell7 = row.CreateCell(7);
                    cell7.SetCellValue("Codigo error");
                    cell7.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(7);


                    ICell cell8 = row.CreateCell(8);
                    cell8.SetCellValue("N# Ticket");
                    cell8.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(8);


                    ICell cell9 = row.CreateCell(9);
                    cell9.SetCellValue("Numero_Credito");
                    cell9.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(9);


                    ICell cell10 = row.CreateCell(10);
                    cell10.SetCellValue("  Referencia");
                    cell10.CellStyle = (boldStyle);
                    //sheet1.AutoSizeColumn(10);
                    sheet1.SetColumnWidth(10, 4500);
                    //Variable que inicia desde la celda 1 


                    ICell cell11 = row.CreateCell(11);
                    cell11.SetCellValue("Importe Origen");
                    cell11.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(11);


                    ICell cell12= row.CreateCell(12);
                    cell12.SetCellValue("Importe Depositado");
                    cell12.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(12);


                    ICell cell13 = row.CreateCell(13);
                    cell13.SetCellValue("Importe Mora");
                    cell13.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(13);


                    ICell cell14 = row.CreateCell(14);
                    cell14.SetCellValue("Oficina Pago");
                    cell14.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(14);


                    ICell cell15 = row.CreateCell(15);
                    cell15.SetCellValue("Fecha Pago");
                    cell15.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(15);

                    ICell cell16 = row.CreateCell(16);
                    cell16.SetCellValue("Observacion");
                    cell16.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(16);

                    ICell cell17 = row.CreateCell(17);
                    cell17.SetCellValue("N# Transaccion");
                    cell17.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(17);

                    ICell cell18 = row.CreateCell(18);
                    cell18.SetCellValue("Fecha Vencimiento");
                    cell18.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(18);

                    ICell cell19 = row.CreateCell(19);
                    cell19.SetCellValue("Saldo Capital");
                    cell19.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(19);


                    ICell cell200 = row.CreateCell(20);
                    cell200.SetCellValue("Otros Cargos");
                    cell200.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(20);


                    ICell cell210 = row.CreateCell(21);
                    cell210.SetCellValue("Otros Gastos");
                    cell210.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(21);

                    ICell cell22 = row.CreateCell(22);
                    cell22.SetCellValue("Seguro Desgravamen");
                    cell22.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(22);


                    ICell cell23 = row.CreateCell(23);
                    cell23.SetCellValue("Importe Mora");
                    cell23.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(23);


                    ICell cell24 = row.CreateCell(24);
                    cell24.SetCellValue("  ITF");
                    cell24.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(24);
 

                    var iFila = 1;



                    //  FORMATO COLUMNAS  
                    IDataFormat dataFormatCustom = hssfworkbook.CreateDataFormat();
                    //  Formato o style Fecha
                    ICellStyle styleFecha = hssfworkbook.CreateCellStyle();
                    //  Para el campo numerico en enteros
                    ICellStyle styleNumeric = hssfworkbook.CreateCellStyle();
                    // Para el campo numerico en decimales
                    ICellStyle styleNumericDecimal = hssfworkbook.CreateCellStyle();
                    //  Estilo rigcht (Izquierdo)
                    ICellStyle styleRight = hssfworkbook.CreateCellStyle();
                    ICellStyle styleRightDoi = hssfworkbook.CreateCellStyle();
                    //   Estilo Tipo texto
                    ICellStyle styletext = hssfworkbook.CreateCellStyle();
                    ICellStyle styleRightRubrocuenta = hssfworkbook.CreateCellStyle();

                     
                    //Creacion de los formatos 
                    IDataFormat format = hssfworkbook.CreateDataFormat();
                    IDataFormat format2 = hssfworkbook.CreateDataFormat();


                    //For que jala el metodo que trae informacion desde la base de datos 
                    foreach (var campo in loObtenerPagos)
                    {

                        //Creacion de las filas en el archivoExcel
                        IRow rowFila = sheet1.CreateRow(iFila);
                        //Creacion del estilo bold o negrita
                        IFont boldFont1 = hssfworkbook.CreateFont();
                        //Estilo de las CellFila para el llamado de los estilos
                        ICell cellFilla = rowFila.CreateCell(0);

 
                        rowFila = sheet1.CreateRow(iFila);
                        boldFont1 = hssfworkbook.CreateFont();
                       


                        rowFila.CreateCell(0).SetCellValue(Convert.ToString(campo.NRO_LINEA));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(0).CellStyle = styleLeft2;
                        //cell.CellStyle = styleRight;




                        //Creacion de la columna Numero_Credito , con su respectivo estilo a la derecha
                        //Tipo de data , convertido a string para que coja la cantidad necesaria de los digitos 
                        rowFila.CreateCell(1).SetCellValue(Convert.ToString(campo.sDOI));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;                        
                        rowFila.GetCell(1).CellStyle = styleLeft2;
                        //cell.CellStyle = styleRight;





                        //Creacion de la columna Doi  con sus respectivos estilos y tipo de dato
                        ICell cell33 = rowFila.CreateCell(2);
                        cell33.CellStyle = styletext;
                        cell33.CellStyle = styleRightDoi;
                        styleRightDoi.Alignment = HorizontalAlignment.Right;
                        styleRightDoi.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(2).CellStyle = styletext;
                        rowFila.GetCell(2).CellStyle = styleRightDoi;
                        rowFila.GetCell(2).CellStyle = styleLeft2;
                        cell33.CellStyle.DataFormat = dataFormatCustom.GetFormat("text");
                        cell33.SetCellValue(campo.vNombreCliente);



                        //Creacion de la columna moneda , con su data convertido a float
                        //rowFila.CreateCell(3).SetCellValue(int.Parse(campo.vMoneda));
                        rowFila.CreateCell(3).SetCellValue(Convert.ToString(campo.vDescripcion));
                        rowFila.GetCell(3).CellStyle = styleLeft2;

                        //cell3.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");

                        //Creacion de la columna Decimal
                        ICell cell21 = rowFila.CreateCell(4);
                        //cell21.CellStyle = styleNumericDecimal;
                        //Tipo de dato numerico
                        // Alineamiento a la derecha en la parte inferior
                        //styleNumericDecimal.Alignment = HorizontalAlignment.Right;
                        styleNumericDecimal.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(4).CellStyle = styleLeft2;
                        //cell21.CellStyle = styleNumericDecimal;
                        //Formato del campo monto_aplicar en numeric con dos decimales a la derecha
                        //cell21.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                        cell21.SetCellValue(campo.sRubro_Cuenta);//cell21.SetCellValue((double)campo.dMonto_Aplicar);


                        //Creacion de la columna Rubro cuenta con sus respectivos estilos y f<ormato de data
                        ICell cell20 = rowFila.CreateCell(5);
                        cell20.CellStyle = styleNumeric;
                        //cell20.CellStyle = styleRightRubrocuenta;
                        //Estilo a la derecha en la parte inferior
                        //styleRightRubrocuenta.Alignment = HorizontalAlignment.Right;
                        styleRightRubrocuenta.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(5).CellStyle = styleLeft2;
                        //cell20.CellStyle = styleRightRubrocuenta;
                        // Stilo del formato en enteros Int
                        // cell20.CellStyle.DataFormat = dataFormatCustom.GetFormat("0");
                        //cell20.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                        cell20.SetCellValue(campo.vCodigoEstado);  //cell20.SetCellValue(Convert.ToInt64(campo.Monto));



                        //Creacion de la columna TipoMovimiento
                        rowFila.CreateCell(6).SetCellValue(Convert.ToString(campo.vAsiento_Contable));
                        rowFila.GetCell(6).CellStyle = styleLeft2;

                        rowFila.CreateCell(7).SetCellValue(Convert.ToString(campo.vCodigoError));
                        rowFila.GetCell(7).CellStyle = styleLeft2;



                        rowFila.CreateCell(8).SetCellValue(Convert.ToString(campo.vNroTicket));
                        rowFila.GetCell(8).CellStyle = styleLeft2;

                        rowFila.CreateCell(9).SetCellValue(Convert.ToString(campo.sNumero_Credito));
                        rowFila.GetCell(9).CellStyle = styleLeft2;



                        rowFila.CreateCell(10).SetCellValue(Convert.ToString(campo.vReferencia));
                        rowFila.GetCell(10).CellStyle = styleLeft2;

                        rowFila.CreateCell(11).SetCellValue(Convert.ToString(campo.nImp_Origen));
                        rowFila.GetCell(11).CellStyle = styleLeft2;

                        rowFila.CreateCell(12).SetCellValue(Convert.ToString(campo.nImpDepositado));
                        rowFila.GetCell(12).CellStyle = styleLeft2;

                        rowFila.CreateCell(13).SetCellValue(Convert.ToString(campo.iImp_Mora));
                        rowFila.GetCell(13).CellStyle = styleLeft2;

                        rowFila.CreateCell(14).SetCellValue(Convert.ToString(campo.iOficina_Pago));
                        rowFila.GetCell(14).CellStyle = styleLeft2;

                        rowFila.CreateCell(15).SetCellValue(Convert.ToString(campo.dFechaPago));
                        rowFila.GetCell(15).CellStyle = styleLeft2;

                        rowFila.CreateCell(16).SetCellValue(Convert.ToString(campo.vObservacion));
                        rowFila.GetCell(16).CellStyle = styleLeft2;


                        rowFila.CreateCell(17).SetCellValue(Convert.ToInt32(campo.nNTransaccion));
                        rowFila.GetCell(17).CellStyle = styleLeft2;

                        rowFila.CreateCell(18).SetCellValue(Convert.ToString(campo.dFechaVencimiento));
                        rowFila.GetCell(18).CellStyle = styleLeft2;


                        rowFila.CreateCell(19).SetCellValue(Convert.ToString(campo.nSaldoCapital));
                        rowFila.GetCell(19).CellStyle = styleLeft2;



                        rowFila.CreateCell(20).SetCellValue(Convert.ToString(campo.nOtrosCargos));
                        rowFila.GetCell(20).CellStyle = styleLeft2;

                        rowFila.CreateCell(21).SetCellValue(Convert.ToString(campo.nOtrosGastos));
                        rowFila.GetCell(21).CellStyle = styleLeft2;


                        rowFila.CreateCell(22).SetCellValue(Convert.ToString(campo.vSegDesgravamen));
                        rowFila.GetCell(22).CellStyle = styleLeft2;


                        rowFila.CreateCell(23).SetCellValue(Convert.ToString(campo.iInt_Moratorio));
                        rowFila.GetCell(23).CellStyle = styleLeft2;

                        rowFila.CreateCell(24).SetCellValue(Convert.ToString(campo.nItf));
                        rowFila.GetCell(24).CellStyle = styleLeft2;


                        //Variable de inicio que se incrementa de acuerdo a la cantidad de data que recorra el for each 
                        iFila++;
                    }
               

                    //  VARIABLES PARA RUTA DEL ARCHIVO TEMPORAL 
                    sRutaAlmacenamiento = "AMORTIZACIONES_MASIVAS/" + DateTime.Now.ToString(sMascaraFechaArchivo);



                    //Ruta de descarga del archivo xls , en el servidor 74 
                    Archivo = @"C:\/\Users\Public\Downloads";
                    //Archivo = System.IO.Path.GetTempPath() + sRutaAlmacenamiento;

                    //Tipo de extension en .xls
                    Archivo = Archivo + ".xls";

                    // En el file 2 se guarda la ruta del archivo + su extension
                    FileInfo newFile = new FileInfo(Archivo);
                    FileStream file2 = new FileStream(Archivo, FileMode.Create);

                    //  ESCRIBE EL ARCHIVO EXCELL 
                    hssfworkbook.Write(file2);
                    file2.Close();

                    bPagos = System.IO.File.ReadAllBytes(Archivo);
                    System.IO.File.Delete(Archivo);

                }
                catch (Exception ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    bPagos = null;
                    throw ex;
                }
                finally
                {
                    if (xlsApp != null)
                        xlsApp.Dispose();

                    xlsApp = null;
                }
            }

            return bPagos;
        }
        #endregion

        #region ExportarAfectacionesMasivas
        public byte[] rnExportarAfectacionesMasivas(EnAfectacionesMasivasReporteCabecera oParametros)
        {
            byte[] bAfectacionesMasivas;
            List<EnAfectacionesMasivasReporte> lAfectacionesMasivasReporte = new List<EnAfectacionesMasivasReporte>();
            ExcelPackage xlsApp = null;
            String Archivo = string.Empty;
            string sRutaAlmacenamiento = "";
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();                    

                    //sRutaPlantilla = "/inetpub/wwwroot/Satelites.TopazOperaciones.WS/App_Data/AMORTIZACIONES_MASIVAS.xlsx";
                    sRutaAlmacenamiento = "REPORTES_AFECTACIONES_MASIVAS" + DateTime.Now.ToString(sMascaraFechaArchivo);
                    lAfectacionesMasivasReporte = new AdGestion(con).adListadoAfectacionesMasivas(oParametros);



                    // ############     Color fondo Inicio

                    HSSFWorkbook workbook = new HSSFWorkbook();
                    HSSFPalette palette = workbook.GetCustomPalette();
                    palette.SetColorAtIndex(HSSFColor.Orange.Index, (byte)246, (byte)142, (byte)108);

                    //Creacion del metodo HSSFworkbook , y declarando el shet1 con el nombre de la pagina en el excel
                    hssfworkbook = new HSSFWorkbook();
                    HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Reporte_AfetacionesMasivas");
                    HSSFPalette paletteCab = hssfworkbook.GetCustomPalette();
                    paletteCab.SetColorAtIndex(HSSFColor.Orange.Index, (byte)246, (byte)142, (byte)108);


                    IRow rowFilaCab = sheet1.CreateRow(0);
                    rowFilaCab.CreateCell(0);
                    ICellStyle styleLeft1 = hssfworkbook.CreateCellStyle();

                    styleLeft1.Alignment = HorizontalAlignment.Center;
                    sheet1.AutoSizeColumn(0);

                    ICellStyle styleLeft2 = hssfworkbook.CreateCellStyle();
                    styleLeft2.Alignment = HorizontalAlignment.Center;
                    sheet1.AutoSizeColumn(0);

                    IFont boldFont = hssfworkbook.CreateFont();
                    boldFont.FontHeightInPoints = 11;
                    boldFont.IsBold = true;
                    boldFont.FontName = ("Calibri");


                    ICellStyle boldStyle = hssfworkbook.CreateCellStyle();

                    // Creacion del style borde
                    ICellStyle blackBorder = hssfworkbook.CreateCellStyle();
                    boldStyle.BorderBottom = BorderStyle.Thin;
                    boldStyle.SetFont(boldFont);
                    boldStyle.Alignment = HorizontalAlignment.Center;

               


                    ICellStyle style001 = workbook.CreateCellStyle();
                    style001.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Orange.Index;
                    style001.FillPattern = FillPattern.SolidForeground;

                    // ############     Color fondo Fin

                    //Creacion del estilo y estilo negrita para las cabeceras 
                    boldStyle.SetFont(boldFont);
                    boldFont.FontName = ("Calibri");
                    sheet1.AutoSizeColumn(0);


                    IRow row = sheet1.CreateRow(0);
                    ICell cell = row.CreateCell(0);
                    cell.CellStyle = (boldStyle);
                    cell.SetCellValue("Item");
                    sheet1.SetColumnWidth(0, 1500);
                    sheet1.AutoSizeColumn(0);



                    ICell cell1 = row.CreateCell(1);
                    cell1.CellStyle = (boldStyle);
                    cell1.SetCellValue("Codigo Afectacion");
                    sheet1.SetColumnWidth(1, 3500);
                    sheet1.AutoSizeColumn(1);

                    ICell cell2 = row.CreateCell(2);
                    cell2.SetCellValue("Codigo Ticket Topaz");
                    cell2.CellStyle = (boldStyle);
                    sheet1.SetColumnWidth(2, 3500);
                    sheet1.AutoSizeColumn(2);

                    ICell cell3 = row.CreateCell(3);
                    cell3.CellStyle = (boldStyle);
                    cell3.SetCellValue(" Agencia");
                    sheet1.SetColumnWidth(3, 6000);
                    sheet1.AutoSizeColumn(3);


                    ICell cell4 = row.CreateCell(4);
                    cell4.CellStyle = (boldStyle);
                    cell4.SetCellValue("    Nro. Credito");
                    sheet1.AutoSizeColumn(4);

                    
                    ICell cell5 = row.CreateCell(5);
                    cell5.SetCellValue("    Titular Credito");
                    cell5.CellStyle = (boldStyle);
                    sheet1.SetColumnWidth(5, 4500);

                    
                    ICell cell6 = row.CreateCell(6);
                    cell6.SetCellValue("Nro. Cuenta");
                    cell6.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(6);


                    ICell cell7 = row.CreateCell(7);
                    cell7.SetCellValue("Titular Cuenta");
                    cell7.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(7);


                    ICell cell8 = row.CreateCell(8);
                    cell8.SetCellValue("Tipo Cuenta");
                    cell8.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(8);


                    ICell cell9 = row.CreateCell(9);
                    cell9.SetCellValue("Indicador Con. Ind.");
                    cell9.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(9);


                    ICell cell10 = row.CreateCell(10);
                    cell10.SetCellValue("  Indicador Extorno");
                    cell10.CellStyle = (boldStyle);                    
                    sheet1.SetColumnWidth(10, 4500);
                    


                    ICell cell11 = row.CreateCell(11);
                    cell11.SetCellValue("Dias Atraso");
                    cell11.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(11);


                    ICell cell12 = row.CreateCell(12);
                    cell12.SetCellValue("Numero Transaccion");
                    cell12.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(12);


                    ICell cell13 = row.CreateCell(13);
                    cell13.SetCellValue("Monto Total");
                    cell13.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(13);


                    ICell cell14 = row.CreateCell(14);
                    cell14.SetCellValue("Fecha Proceso Afectacion");
                    cell14.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(14);


                    ICell cell15 = row.CreateCell(15);
                    cell15.SetCellValue("Fecha Cargo Masiva");
                    cell15.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(15);

                    ICell cell16 = row.CreateCell(16);
                    cell16.SetCellValue("Glosa Bloqueo");
                    cell16.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(16);

                    ICell cell17 = row.CreateCell(17);
                    cell17.SetCellValue("Motivo Retencion Bloqueo");
                    cell17.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(17);

                    ICell cell18 = row.CreateCell(18);
                    cell18.SetCellValue("Telefono 1");
                    cell18.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(18);

                    ICell cell19 = row.CreateCell(19);
                    cell19.SetCellValue("Telefono 2");
                    cell19.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(19);


                    ICell cell20 = row.CreateCell(20);
                    cell20.SetCellValue("Telefono 3");
                    cell20.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(20);


                    ICell cell21 = row.CreateCell(21);
                    cell21.SetCellValue("Telefono 4");
                    cell21.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(21);

                    ICell cell22 = row.CreateCell(22);
                    cell22.SetCellValue("Correo");
                    cell22.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(22);


                    ICell cell23 = row.CreateCell(23);
                    cell23.SetCellValue("Direccion");
                    cell23.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(23);


                    ICell cell24 = row.CreateCell(24);
                    cell24.SetCellValue("  Departamento");
                    cell24.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(24);


                    ICell cell25 = row.CreateCell(25);
                    cell25.SetCellValue("  Provincia");
                    cell25.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(25);


                    ICell cell26 = row.CreateCell(26);
                    cell26.SetCellValue("  Distrito");
                    cell26.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(26);                    


                    ICell cell27 = row.CreateCell(27);
                    cell27.SetCellValue("  Cod. Estado");
                    cell27.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(27);


                    ICell cell28 = row.CreateCell(28);
                    cell28.SetCellValue("  Dscrp. Estado");
                    cell28.CellStyle = (boldStyle);
                    sheet1.AutoSizeColumn(28);

                    var iFila = 1;



                    //  FORMATO COLUMNAS  
                    IDataFormat dataFormatCustom = hssfworkbook.CreateDataFormat();
                    //  Formato o style Fecha
                    ICellStyle styleFecha = hssfworkbook.CreateCellStyle();
                    //  Para el campo numerico en enteros
                    ICellStyle styleNumeric = hssfworkbook.CreateCellStyle();
                    // Para el campo numerico en decimales
                    ICellStyle styleNumericDecimal = hssfworkbook.CreateCellStyle();
                    //  Estilo rigcht (Izquierdo)
                    ICellStyle styleRight = hssfworkbook.CreateCellStyle();
                    ICellStyle styleRightDoi = hssfworkbook.CreateCellStyle();
                    //   Estilo Tipo texto
                    ICellStyle styletext = hssfworkbook.CreateCellStyle();
                    ICellStyle styleRightRubrocuenta = hssfworkbook.CreateCellStyle();


                    //Creacion de los formatos 
                    IDataFormat format = hssfworkbook.CreateDataFormat();
                    IDataFormat format2 = hssfworkbook.CreateDataFormat();


                    //For que jala el metodo que trae informacion desde la base de datos 
                    foreach (var campo in lAfectacionesMasivasReporte)
                    {

                        //Creacion de las filas en el archivoExcel
                        IRow rowFila = sheet1.CreateRow(iFila);
                        //Creacion del estilo bold o negrita
                        IFont boldFont1 = hssfworkbook.CreateFont();
                        //Estilo de las CellFila para el llamado de los estilos
                        ICell cellFilla = rowFila.CreateCell(0);


                        rowFila = sheet1.CreateRow(iFila);
                        boldFont1 = hssfworkbook.CreateFont();

                        sheet1.SetColumnWidth(0, 5000);
                        rowFila.CreateCell(0).SetCellValue(Convert.ToString(campo.iNroLinea));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(0).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(1, 5000);
                        rowFila.CreateCell(1).SetCellValue(Convert.ToString(campo.iIdAfectacion));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(1).CellStyle = styleLeft2;


                        sheet1.SetColumnWidth(2, 5000);
                        rowFila.CreateCell(2).SetCellValue(Convert.ToString(campo.nNroTicket));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(2).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(3, 5000);
                        sheet1.AutoSizeColumn(3);
                        rowFila.CreateCell(3).SetCellValue(Convert.ToString(campo.vSucursalCrd));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(3).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(4, 15000);
                        rowFila.CreateCell(4).SetCellValue(Convert.ToString(campo.nNumeroCredito));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(4).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(5, 15000);
                        rowFila.CreateCell(5).SetCellValue(Convert.ToString(campo.vTitularCredito));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(5).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(6, 10000);
                        rowFila.CreateCell(6).SetCellValue(Convert.ToString(campo.nNumeroCuenta));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(6).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(7, 10000);
                        rowFila.CreateCell(7).SetCellValue(Convert.ToString(campo.vTitularCuenta));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(7).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(8, 6000);
                        rowFila.CreateCell(8).SetCellValue(Convert.ToString(campo.vTipoCuenta));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(8).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(9, 6000);
                        rowFila.CreateCell(9).SetCellValue(Convert.ToString(campo.vIndicadorConInd));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(9).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(10, 6000);
                        rowFila.CreateCell(10).SetCellValue(Convert.ToString(campo.nIndicadorExtorno));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(10).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(11, 6000);
                        rowFila.CreateCell(11).SetCellValue(Convert.ToString(campo.iDiasAtraso));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(11).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(12, 6000);
                        rowFila.CreateCell(12).SetCellValue(Convert.ToString(campo.nNumeroTransaccion));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(12).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(13, 6000);
                        rowFila.CreateCell(13).SetCellValue(Convert.ToString(campo.nMontoAplicar));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(13).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(14, 6000);
                        rowFila.CreateCell(14).SetCellValue(Convert.ToString(campo.vFechaGeneracionAfectacion));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(14).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(15, 6000);
                        rowFila.CreateCell(15).SetCellValue(Convert.ToString(campo.vFechaCargoMasiva));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(15).CellStyle = styleLeft2;

                        rowFila.CreateCell(16).SetCellValue(Convert.ToString(campo.vGlosaBloqueo));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(16).CellStyle = styleLeft2;

                        rowFila.CreateCell(17).SetCellValue(Convert.ToString(campo.vMotivoRetencionBloqueo));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(17).CellStyle = styleLeft2;

                        rowFila.CreateCell(18).SetCellValue(Convert.ToString(campo.vTelefono1));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(18).CellStyle = styleLeft2;


                        rowFila.CreateCell(19).SetCellValue(Convert.ToString(campo.vTelefono2));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(19).CellStyle = styleLeft2;


                        rowFila.CreateCell(20).SetCellValue(Convert.ToString(campo.vTelefono3));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(20).CellStyle = styleLeft2;

                        rowFila.CreateCell(21).SetCellValue(Convert.ToString(campo.vTelefono4));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(21).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(22, 15000);
                        rowFila.CreateCell(22).SetCellValue(Convert.ToString(campo.vCorreo));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(22).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(23, 10000);
                        rowFila.CreateCell(23).SetCellValue(Convert.ToString(campo.vDireccion));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(23).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(24, 6000);
                        rowFila.CreateCell(24).SetCellValue(Convert.ToString(campo.vDepartamento));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(24).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(25, 6000);
                        rowFila.CreateCell(25).SetCellValue(Convert.ToString(campo.vProvincia));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(25).CellStyle = styleLeft2;

                        sheet1.SetColumnWidth(26, 6000);
                        rowFila.CreateCell(26).SetCellValue(Convert.ToString(campo.vDistrito));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(26).CellStyle = styleLeft2;


                        sheet1.SetColumnWidth(27, 6000);
                        rowFila.CreateCell(27).SetCellValue(Convert.ToString(campo.vEstado));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(27).CellStyle = styleLeft2;


                        sheet1.SetColumnWidth(28, 15000);
                        rowFila.CreateCell(28).SetCellValue(Convert.ToString(campo.vDescripEstado));
                        styleRight.Alignment = HorizontalAlignment.Right;
                        styleRight.VerticalAlignment = VerticalAlignment.Bottom;
                        rowFila.GetCell(28).CellStyle = styleLeft2;
                        
                        //Variable de inicio que se incrementa de acuerdo a la cantidad de data que recorra el for each 
                        iFila++;
                    }
                    //  VARIABLES PARA RUTA DEL ARCHIVO TEMPORAL 
                    sRutaAlmacenamiento = "AMORTIZACIONES_MASIVAS/" + DateTime.Now.ToString(sMascaraFechaArchivo);

                    //Ruta de descarga del archivo xls , en el servidor 74 
                    Archivo = @"C:\/\Users\Public\Downloads";
                    //Archivo = System.IO.Path.GetTempPath() + sRutaAlmacenamiento;

                    //Tipo de extension en .xls
                    Archivo = Archivo + ".xls";

                    // En el file 2 se guarda la ruta del archivo + su extension
                    FileInfo newFile = new FileInfo(Archivo);
                    FileStream file2 = new FileStream(Archivo, FileMode.Create);

                    //  ESCRIBE EL ARCHIVO EXCELL 
                    hssfworkbook.Write(file2);
                    file2.Close();

                    bAfectacionesMasivas = File.ReadAllBytes(Archivo);
                    File.Delete(Archivo);

                }
                catch (Exception ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    bAfectacionesMasivas = null;
                    throw ex;
                }
                finally
                {
                    if (xlsApp != null)
                        xlsApp.Dispose();

                    xlsApp = null;
                }
            }

            return bAfectacionesMasivas;
        }
        #endregion

        #region Caja Arequipa

        public EnExportarDeudasCA rn_ExportarDeudasCA()
        {
            EnExportarDeudasCA oResultado = new EnExportarDeudasCA();
            using (SqlConnection con = new SqlConnection(sConexion))
            {
                try
                {
                    con.Open();
                    AdGestion oadGestion = new AdGestion(con);
                    oResultado = oadGestion.ad_ExportarDeudasCA();

                }
                catch (SqlException ex)
                {
                    UtlLog.toWrite(UtlConstantes.Satelite_OperacionesTopazRN, UtlConstantes.LogNamespace_OperacionesTopazRN, this.GetType().Name.ToString(), MethodBase.GetCurrentMethod().Name, UtlConstantes.LogTipoError, "", ex.StackTrace.ToString(), ex.Message.ToString());
                    throw ex;
                }
            }
            return oResultado;
        }

        #endregion
    }
}
