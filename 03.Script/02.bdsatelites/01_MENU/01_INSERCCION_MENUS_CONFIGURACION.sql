Use bdSatelites
Go
--========================== Identificación del Procedimiento =============================
--Descripción		: Creamos los menús para Operaciones topaz	- Caja Arequipa
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

SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN TRY
	BEGIN TRAN

	--Declarar y Asignar Variables de Auditoria
	DECLARE @vNombreUsuario	VARCHAR(20) = 'gpadillab'
	,		@vDireccionIp	VARCHAR(20) = '10.10.113.245'
	,		@vDireccionMac	VARCHAR(20) = 'F2-F2-F2-F2-F2-F2'
	,		@iModulo		INT		    = 26
	,		@iIdSubModulo	INT			= 72
	,		@iIdMenuPadre	INT			= 752


	--	============================================================
	--		1 - INSERCCION DE MENU HIJOS 
	--	============================================================

	--	##	PADRE 01 :: Configuracion
	INSERT SEA_SEGUR.tSEA_Formulario(vRuta,iAccion,bEliminado,vAudNombreUsuarioCreacion,vAudIPCreacion,vAudMACCreacion)
	VALUES	('Corresponsalia/CajaArequipa',1,0,@vNombreUsuario,@vNombreUsuario,@vNombreUsuario)
	DECLARE @iMenuHijoCajaArequipa Int = (Select IDENT_CURRENT('SEA_SEGUR.tSEA_Formulario'))


	--	============================================================			select * from SEA_SEGUR.tSEA_Menu
	--		2 - INSERCCION DE RELACION PADRES / HIJOS
	--	============================================================

		--	##	3.1 :: Configuracion - Generales
		------------------------------------------------------------				
		 SELECT * FROM SEA_SEGUR.tSEA_MenU WHERE iIdMenu = 826 
		 update SEA_SEGUR.tSEA_MenU set vNombre = 'CajaArequipa' WHERE iIdMenu = 826 
		  SELECT * FROM SEA_SEGUR.tSEA_MenU WHERE iIdMenu = 826 
		INSERT	SEA_SEGUR.tSEA_Menu(iIdPadre,iIdSubModulo,iIdFormulario,vNombre,siEstado,bEliminado,vAudNombreUsuarioCreacion,vAudIPCreacion,vAudMACCreacion)
		VALUES	(@iIdMenuPadre,@iIdSubModulo,@iMenuHijoCajaArequipa,'Caja Arequipa',1,0,@vNombreUsuario,@vNombreUsuario,@vNombreUsuario)


	--	============================================================
	--		VERIFICANDO INSERCCIONES
	--	============================================================

		SELECT * FROM SEA_SEGUR.tSEA_Menu		WHERE iIdSubModulo = @iIdSubModulo
		SELECT * FROM SEA_SEGUR.tSEA_Formulario WHERE iIdFormulario IN (@iMenuHijoCajaArequipa)



	COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK TRAN
	--SE CAPTURA EL ERROR
	Declare @vObservacion Varchar(200)
	Set @vObservacion = 							
	'Detalle: ' + Convert(Varchar(200), Error_message()) + ' - ' + 
	'N°: ' + Convert(Varchar(200), Error_number()) + ' - ' + 
	'Línea: ' + Convert(Varchar(200), Error_line())

	--SE PINTA EL ERROR
	Select @vObservacion	
END CATCH
SET XACT_ABORT OFF