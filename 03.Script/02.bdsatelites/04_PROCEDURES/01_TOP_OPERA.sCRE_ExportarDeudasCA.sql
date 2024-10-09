USE bdSatelites
GO
--========================== Identificación del Procedimiento =============================
--Descripción        : Procedimiento para Exportar las deudas de Caja Arequipa
--.........................................................................................

--Nombre             : Configuracion_Menu_Operaciones_Topaz
--Sistema            : Operaciones									  sCRE_ExportarDeudasCA
--Módulo            : Operaciones Topaz
--Autor              : Gianfranco Josep Padilla Bonilla
--Fecha de Creación  : 20240930
--=============================== Procedimiento de Ejecución ==============================
--Secuencia         : desde BD
--Ejecución         : Manual 
--Input             : Ninguno
--Output            : Ninguno
--Req. Adicionales  : Ninguno
--=============================== Control de Cambios ======================================  
--Fecha | Autor | Sustento / Detalle  
--=========================================================================================

CREATE PROCEDURE TOP_OPERA.sCRE_ExportarDeudasCA  
AS
BEGIN
    DECLARE	@vFechaProceso  VARCHAR(20) 
	SELECT	@vFechaProceso = FORMAT(FECHAPROCESO, 'ddMMyyyy')
    FROM	snTPZ_PARAMETROS 

    DECLARE @sNombreArchivo NVARCHAR(100) = 'CREDINKA' + @vFechaProceso + '.txt';
																				
 
    SELECT	@sNombreArchivo AS sNombreArchivo;

	DECLARE	@tDeudasCA			TABLE
	(		CONTRATO			VARCHAR(100)
	,		FECHA_SISTEMA		VARCHAR(100)
	,		NUMERO_DOCUMENTO	VARCHAR(100)
	,		MONTO_COBRO			NUMERIC(10,2)
	,		MONTO_ITF			NUMERIC(10,2)
	,		MONTO_TOTAL			NUMERIC(10,2)
	,		FECHA_FACTURACION	VARCHAR(100)
	,		FECHA_VENCIMIENTO	VARCHAR(100)
	,		NOMBRE_CLIENTE		VARCHAR(100)
	,		TIPO_INFORMACION	VARCHAR(100)
	)	

	INSERT	@tDeudasCA
	
	SELECT	
			CONTRATO
	,		FECHA_SISTEMA
	,		NUMERO_DOCUMENTO
	,		montoCuota
    ,		CAST(montoCuota / 1000 AS INT) * 0.05 
	,		montoCuota + CAST(montoCuota / 1000 AS INT) * 0.05 
	,		FECHA_FACTURACION
    ,       FECHA_VENCIMIENTO
    ,       NOMBRE_CLIENTE
    ,       TIPO_INFORMACION
	FROM	[snTPZ_VW_ITF_OT_AREQUIPA_DEUDAS]
    WHERE	codigo_moneda = 1


    SELECT 
        'M0140010000' + 
        FORMAT(FECHAPROCESO, 'ddMMyyyy') + -- Formato día, mes, año
        ' CREDINKA S.A.' AS sDeudasCajaArequipa
    FROM 
        snTPZ_PARAMETROS

    UNION ALL

    SELECT 	  
        CONCAT(
            CONTRATO,
            FECHA_SISTEMA,
            NUMERO_DOCUMENTO,
            RIGHT('000000000' + REPLACE(CAST(MONTO_TOTAL AS VARCHAR(9)), '.', ''), 9),
            FECHA_FACTURACION,
            FECHA_VENCIMIENTO,
            NOMBRE_CLIENTE,
            TIPO_INFORMACION
        )
    FROM 
			@tDeudasCA

END
