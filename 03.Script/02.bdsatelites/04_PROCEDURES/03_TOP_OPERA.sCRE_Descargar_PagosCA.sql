Use bdSatelites
Go
--========================== Identificación del Procedimiento =============================
--Descripción		: Procedimiento para Descargar las deudas de Caja Arequipa
--.........................................................................................
--Nombre			: Configuracion_Menu_Operaciones_Topaz
--Sistema			: Operaciones
--Módulo 			: Operaciones Topaz
--Autor				: Gianfranco Josep Padilla Bonilla
--Fecha de Creación	: 20240930
--===============================Procedimiento de Ejecución================================    
--Secuencia			: desde BD
--Ejecución			: Manual 
--Input				: Ninguno
--Output			: Ninguno
--Req. Adicionales	: Ninguno
--=============================== Control de Cambios=======================================  
--Fecha | Autor | Sustento / Detalle  
--=========================================================================================


CREATE	PROCEDURE TOP_OPERA.sCRE_DescargarDeudasCA  
		@iIdPagosCA			INT
AS


BEGIN 

	DECLARE	@vFechaProceso	VARCHAR(20)
	SELECT	@vFechaProceso = FORMAT(FECHAPROCESO, 'yyyy/MM/dd')
	FROM	snTPZ_PARAMETROS	WITH (NOLOCK)

	SELECT	FORMAT( CAST(FECHA_DE_PAGO AS DATE), 'yyyy/MM/dd') 			AS vFechaProceso 
	,		CONCAT(CAST(LEFT(NRODOCUMENTO, LEN(NRODOCUMENTO) - 3)	AS NUMERIC(20)),'')	AS vNumeroCredito
	,		CASE 
			WHEN LEN(CAST(CONTRATO AS NUMERIC(15))) <= 8 THEN CONCAT(RIGHT(CONTRATO, 8),'')
			WHEN LEN(CAST(CONTRATO AS NUMERIC(15))) >  8 THEN CONCAT(RIGHT(CONTRATO, 11),'')
			END AS vDoi
	--,		CONTRATO										AS vDoi
	,		'1'												AS vCodigoMoneda
	,		IMPORTE											AS nMontoAplicar
	,		'19180711070101 '								AS vRubroCuenta
	,		'RC'											AS vTipoMovimiento -- select * 
	FROM	TOP_OPERA.tCRE_PagosCA_Detalle
	WHERE	iIdPagosCA = @iIdPagosCA

END 

