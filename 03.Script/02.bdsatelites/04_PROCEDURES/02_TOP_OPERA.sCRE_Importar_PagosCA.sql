Use bdSatelites
Go
--========================== Identificación del Procedimiento =============================
--Descripción		: Procedimiento para Exportar las deudas de Caja Arequipa
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


CREATE	PROCEDURE TOP_OPERA.sCRE_ImportarDeudasCA  
		@pvJson				NVARCHAR(MAX)
	,	@pvAudUsuario		VARCHAR(20)
	,	@pvAudIP			VARCHAR(20)
	,	@pvAudMAC			VARCHAR(20)
	,	@piIdAgencia		INT
AS

	DECLARE	@iIndicador		INT				= 1
	,		@vMensaje		VARCHAR(200)	= ''
	,		@iError			INT				= 0

BEGIN TRY


	DECLARE	@iRegistros		INT				= 0
	,		@iIdPagosCA		INT				= 0

	DECLARE @vNombres		VARCHAR(100) 
	SELECT	@vNombres = CONCAT(vNombres,' ',vApellidos)	  
	FROM	[SEA_SEGUR].[tRHH_Persona]	PER WITH(NOLOCK)
	JOIN	[SEA_SEGUR].[tSEA_Usuario]	USU WITH(NOLOCK) ON PER.iIdPersona = USU.iIdPersona
	WHERE	USU.vCodigoUsuario = @pvAudUsuario


    DECLARE	@tJson TABLE(sJson VARCHAR(500))
	INSERT	@tJson 
	SELECT	VALUE 
	FROM	OPENJSON(@pvJson)

	--	##	Numero de registros de JSON
    SELECT	@iRegistros = COUNT(1) FROM @tJson

	--	##	Registrando tabla Principal
	INSERT	TOP_OPERA.tCRE_PagosCA
	SELECT 
			@iRegistros               AS iCantidadRegistros
	,		@pvAudUsuario             AS vAudNombreUsuarioCreacion
	,		@vNombres                 AS vNombres
	,		GETDATE()                 AS dtFechaCreacion
	,		@pvAudIP                  AS vAudIPCreacion
	,		@pvAudMAC                 AS vAudMACCreacion
	,		@piIdAgencia              AS siCodigoOficinaUsuario

	SELECT	@iIdPagosCA = SCOPE_IDENTITY()

	--	##	Registrando tabla Detalle
	INSERT	TOP_OPERA.tCRE_PagosCA_Detalle
	SELECT	@iIdPagosCA			
	,		SUBSTRING(sJson, 1 , 20)															AS 'CONTRATO'			--	##  ( 20 CARACTERES )		
	,		SUBSTRING(sJson, 21, 20)															AS 'NRODOCUMENTO'		--  ##  ( 20 CARACTERES )	
	,		SUBSTRING(sJson, 41, 8 )															AS 'FECHA DE PAGO'		--  ##  ( 08 CARACTERES )	
	,		CAST(CONCAT(SUBSTRING(sJson, 49, 7),'.',SUBSTRING(sJson, 56, 2))AS DECIMAL(10,2))	AS 'IMPORTE '			--  ##	( 10 CARACTERES )	- LOS DOS ULTIMO SON DECIMALES		 (NUMERIC 8,2) 
    ,		SUBSTRING(sJson, 58, 6)																AS 'HORA'				--	##	( 06 CARACTERES )	
    ,		SUBSTRING(sJson, 64, 10)															AS 'COD PAGO'			--	##	( 10 CARACTERES )	
    ,		SUBSTRING(sJson, 74, 1)																AS 'COD CANAL'			--	##	( 01 CARACTERES )	
    ,		SUBSTRING(sJson, 75, 50)															AS 'NOMBRE DEL CLIENTE'	--	##	( 50 CARACTERES )	
	FROM	@tJson

	SELECT	@vMensaje		= CONCAT('se realizo la carga de <strong>',	@iRegistros ,'</strong> registros. <strong>[ Id Pago : ' , @iIdPagosCA, ' ]</strong>') 
	SELECT	@iError			= 0
	--	##	AllRigth
	SELECT	@iIdPagosCA		AS iIndicador		
	,		@vMensaje		AS vMensaje		
	,		@iError			AS iError			

END TRY
BEGIN CATCH
    -- Manejo de errores
	SELECT	@iError =  1
    SELECT	@vMensaje = CONCAT('se produjo el siguiente error: ' ,ERROR_MESSAGE())
	--	##	Error
	SELECT	@iIndicador		AS iIndicador		
	,		@vMensaje		AS vMensaje		
	,		@iError			AS iError			
				
END CATCH				