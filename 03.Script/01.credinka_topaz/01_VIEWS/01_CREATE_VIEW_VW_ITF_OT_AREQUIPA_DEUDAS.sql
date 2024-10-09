USE CREDINKA_TOPAZ

-- ============================== Identificación del Script ====================================
--
-- Descripción		: Script para obtener las deudas de Caja Arequipa
-- .............	.................................................................................
-- Nombre			: Creacion de vista VW_ITF_OT_AREQUIPA_DEUDAS
-- Satélite			: Interfaces - Topaz
-- Autor			: Nilton Rosas Gutierrez
-- Fecha Creación	: 20240930
-- ================================ Procedimiento de Ejecución =================================
-- Secuencia		: Ejecución manual
-- Input			: No Aplica
-- Output			: No Aplica
-- ============================================================================================= 
GO

ALTER VIEW VW_ITF_OT_AREQUIPA_DEUDAS
AS
select  --TOP 10000
			--SAL.JTS_OID																	AS JTS_OID	
			RIGHT('                    ' + CONVERT(VARCHAR,DOC.NRODOCUMENTO	),20 )			AS CONTRATO	
			--RIGHT(REPLICATE('0', 17)+CAST(SAL.CUENTA AS VARCHAR), 17)+RIGHT(REPLICATE('0', 3) + CAST(p.C2300 AS VARCHAR), 3) AS CONTRATO
			--,convert(varchar,YEAR( PAR.FECHAPROCESO)) + REPLICATE('0', 1)+CAST(MONTH( PAR.FECHAPROCESO) AS VARCHAR(2))									AS FECHA_SISTEMA
			,CONCAT(YEAR( P.C2302),RIGHT(REPLICATE('0', 2)+CAST(MONTH(P.C2302) AS VARCHAR), 2))									AS FECHA_SISTEMA
			--,RIGHT('00000000000000000000' + CONVERT(VARCHAR,DOC.NRODOCUMENTO	),20 )			AS NUMERO_DOCUMENTO	
			,RIGHT(REPLICATE('0', 17)+CAST(SAL.CUENTA AS VARCHAR), 17)+RIGHT(REPLICATE('0', 3) + CAST(p.C2300 AS VARCHAR), 3) AS NUMERO_DOCUMENTO	
			,CONVERT(VARCHAR, P.C2302, 112)												AS FECHA_VENCIMIENTO_CTA
			,RIGHT('000000000' + CONVERT(VARCHAR,
						 CONVERT(INT, ( Isnull(p.C2309,0) +																		--CAPITAL COUOTA
									   Isnull(p.C2310,0) +																		--INTERES CUOTA
									   Isnull(p.C2311,0) + 																	    --MORATORIO CUOTA
									   Isnull(p.icmora_dev_cuo,0)  +															--COMPENSATORIO CUOTA
									   Isnull((Select Sum(Isnull(SALDO_GASTO,0)) From GASTOS_POR_CUOTA  g with(nolock) 
										Where g.SALDOS_JTS_OID=sal.JTS_OID and g.NUMERO_CUOTA=p.C2300 and g.TZ_LOCK=0),0)	-- OTROS CARGOS CUOTA (INCLUYE DESGRAVAMEN)						   
									   )*100)),9 )		AS MONTO_COBRAR
			,( Isnull(p.C2309,0) +																		--CAPITAL COUOTA
									   Isnull(p.C2310,0) +																		--INTERES CUOTA
									   Isnull(p.C2311,0) + 																	    --MORATORIO CUOTA
									   Isnull(p.icmora_dev_cuo,0)  +															--COMPENSATORIO CUOTA
									   Isnull((Select Sum(Isnull(SALDO_GASTO,0)) From GASTOS_POR_CUOTA  g with(nolock) 
										Where g.SALDOS_JTS_OID=sal.JTS_OID and g.NUMERO_CUOTA=p.C2300 and g.TZ_LOCK=0),0)	-- OTROS CARGOS CUOTA (INCLUYE DESGRAVAMEN)						   
									   ) As MONTOCUOTA
			--,CONVERT(VARCHAR, PAR.FECHAPROCESO	, 101)									AS FECHA_FACTURACION
			,CONVERT(VARCHAR(8),PAR.FECHAPROCESO,112) AS FECHA_FACTURACION
			,CONVERT(VARCHAR(8),P.C2302,112) AS FECHA_VENCIMIENTO
			, LEFT(CONVERT(VARCHAR,
			REPLACE(REPLACE( /*vocales ÃÕ*/
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE( /*vocales ÄËÏÖÜ*/
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE( /*vocales ÂÊÎÔÛ*/
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE( /*vocales ÀÈÌÒÙ*/
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE( /*vocales ÁÉÍÓÚ*/
			REPLACE(REPLACE(REPLACE(REPLACE( /*vocales ñÑçÇ  incluido espacio en blanco*/
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE( /*vocales äëïöü*/
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE( /*vocales âêîôû*/
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE( /*vocales àèìòù*/
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE( /*vocales áéíóú*/ REPLACE(REPLACE(UPPER(CLI.C1000), '|', ''), '''', '')
					,'á','A'),'é','E'),'í','I'),'ó','O'),'ú','U')		
					,'à','A'),'è','E'),'ì','I'),'ò','O'),'ù','U')
					,'â','A'),'ê','E'),'î','I'),'ô','O'),'û','U')
					,'ä','A'),'ë','E'),'ï','I'),'ö','O'),'ü','U')
					,'ñ','N'),'Ñ','N'),'ç','C'),'Ç','C')
					,'Á','A'),'É','E'),'Í','I'),'Ó','O'),'Ú','U') 
					,'À','A'),'È','E'),'Ì','I'),'Ò','O'),'Ù','U') 
					,'Â','A'),'Ê','E'),'Î','I'),'Ô','O'),'Û','U')
					,'Ä','A'),'Ë','E'),'Ï','I'),'Ö','O'),'Ü','U') 
					,'Ã','A'),'Õ','O')
			) + '                                             ',50 ) AS NOMBRE_CLIENTE	
			,'A'										AS TIPO_INFORMACION
			,CONVERT(VARCHAR,SAL.PRODUCTO)				AS CODIGO_PRODUCTO			--  DATOS ADICIONES PARA FILTRADO
			,SAL.MONEDA								AS CODIGO_MONEDA			--	
		FROM	 SALDOS		SAL	with(NOLOCK)
		inner join BS_PLANPAGOS (NOLOCK) p on SAL.JTS_OID = p.SALDO_JTS_OID and p.TZ_LOCK = 0 	and sal.C1604 <>0 ---- 20240305 ANPS
		INNER JOIN CL_RELPERDOC	DOC	with	(NOLOCK)	    ON		SAL.C1803	= DOC.IDPERSONA		AND		DOC.TZ_LOCK		= 0	
		INNER JOIN	CLI_CLIENTES  CLI	with	(NOLOCK)	ON		SAL.C1803		= CLI.C0902			AND		CLI.TZ_LOCK	= 0
		,PARAMETROS PAR with (NOLOCK)  
		where 	SAL.C1785 = 5 
				and p.C2300 > sal.C1645 
				And p.C2302 <= dateadd(DAY,15,PAR.FECHAPROCESO)	 --SOLO PENDIENTES
				and C1728 not in ('C','E')   --NO CASTIGADOS (C); NO JUDICIAL (E)
				and sal.PRODUCTO not in (77000001,90000001,92000001,94000001,96000001,85000000002)
GO
