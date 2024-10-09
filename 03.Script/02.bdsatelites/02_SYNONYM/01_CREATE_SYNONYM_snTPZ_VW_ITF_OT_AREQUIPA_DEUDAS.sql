USE bdSatelites
go
-- ============================== Identificación del Script ====================================
--
-- Descripción		: Script que crea todos los sinonimos para AREQUIPA_DEUDAS
-- .............	.................................................................................
-- Nombre			: Creacion_Sinonimos_ VW_ITF_OT_AREQUIPA_DEUDAS
-- Satélite			: Interfaces - Topaz
-- Autor			: Gianfranco Josep Padilla Bonilla
-- Fecha Creación	: 20240930
-- ================================ Procedimiento de Ejecución =================================
-- Secuencia		: Ejecución manual
-- Input			: No Aplica
-- Output			: No Aplica
-- ============================================================================================= 



--##	Operaciones Topaz
--##		-> Correponsalia 
	
CREATE SYNONYM [snTPZ_VW_ITF_OT_AREQUIPA_DEUDAS]			For	[TOPAZDESBD\TOPAZMICRO].[CREDINKA_TOPAZ].[dbo].[VW_ITF_OT_AREQUIPA_DEUDAS]
