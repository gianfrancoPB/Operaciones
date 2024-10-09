Use bdSatelites
Go
--========================== Identificaci�n del Procedimiento =============================
--Descripci�n		: Creacion de tabla de pagos para Caja Arequipa Detalle.
--.........................................................................................
--Nombre			: CREATE_TABLE_[TOP_OPERA].[tCRE_PagosCA_Detalle]
--Sistema			: Operaciones
--M�dulo 			: Operaciones Topaz
--Autor				: Gianfranco Josep Padilla Bonilla
--Fecha de Creaci�n	: 20240930
--===============================Procedimiento de Ejecuci�n================================    
--Secuencia			: desde BD
--Ejecuci�n			: Manual 
--Input				: Ninguno
--Output			: Ninguno
--Req. Adicionales	: Ninguno
--=============================== Control de Cambios=======================================  
--Fecha | Autor | Sustento / Detalle  
--=========================================================================================

CREATE TABLE [TOP_OPERA].[tCRE_PagosCA_Detalle](
	[iIdPagosCA]				[int] NOT NULL,			-- Clave for�nea a la tabla de pagos
    [CONTRATO]					[varchar](20) NULL,		-- Max 20 caracteres (NUMERO DE DNI, RUC)
    [NRODOCUMENTO]				[varchar](20) NULL,		-- Max 20 caracteres (NRO DE CREDITO + NRO DE CUOTA)
    [FECHA_DE_PAGO]				[varchar](8) NULL,		-- Formato YYYYMMDD (FECHA DE PAGO)
    [IMPORTE]					[numeric](10, 2) NULL,	-- Dos �ltimos son decimales (IMPORTE DE PAGO)
    [HORA]						[varchar](6) NULL,		-- Formato HHMMSS (HORA)
    [COD_PAGO]					[varchar](10) NULL,		-- 
    [COD_CANAL]					[varchar](1) NULL,		-- C�digo de pago (CANAL)
    [NOMBRE_DEL_CLIENTE]		[varchar](50) NULL,		-- C�digo de canal (NOMBRE CLIENTE)
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'iIdPagosCA', @value=N'Id seguimiento' , @level0type=N'SCHEMA',@level0name=N'TOP_OPERA', @level1type=N'TABLE',@level1name=N'tCRE_PagosCA_Detalle', @level2type=N'COLUMN',@level2name=N'iIdPagosCA'
GO

EXEC sys.sp_addextendedproperty @name=N'CONTRATO', @value=N'NUMERO DE DNI, RUC' , @level0type=N'SCHEMA', @level0name=N'TOP_OPERA', @level1type=N'TABLE', @level1name=N'tCRE_PagosCA_Detalle', @level2type=N'COLUMN', @level2name=N'CONTRATO'
GO

EXEC sys.sp_addextendedproperty @name=N'NRODOCUMENTO', @value=N'NRO DE CREDITO + NRO DE CUOTA' , @level0type=N'SCHEMA', @level0name=N'TOP_OPERA', @level1type=N'TABLE', @level1name=N'tCRE_PagosCA_Detalle', @level2type=N'COLUMN', @level2name=N'NRODOCUMENTO'
GO

EXEC sys.sp_addextendedproperty @name=N'FECHA_DE_PAGO', @value=N'FECHA DE PAGO' , @level0type=N'SCHEMA', @level0name=N'TOP_OPERA', @level1type=N'TABLE', @level1name=N'tCRE_PagosCA_Detalle', @level2type=N'COLUMN', @level2name=N'FECHA_DE_PAGO'
GO

EXEC sys.sp_addextendedproperty @name=N'IMPORTE', @value=N'IMPORTE DE PAGO' , @level0type=N'SCHEMA', @level0name=N'TOP_OPERA', @level1type=N'TABLE', @level1name=N'tCRE_PagosCA_Detalle', @level2type=N'COLUMN', @level2name=N'IMPORTE'
GO

EXEC sys.sp_addextendedproperty @name=N'HORA', @value=N'HORA' , @level0type=N'SCHEMA', @level0name=N'TOP_OPERA', @level1type=N'TABLE', @level1name=N'tCRE_PagosCA_Detalle', @level2type=N'COLUMN', @level2name=N'HORA'
GO

EXEC sys.sp_addextendedproperty @name=N'COD_PAGO', @value=N'CANAL' , @level0type=N'SCHEMA', @level0name=N'TOP_OPERA', @level1type=N'TABLE', @level1name=N'tCRE_PagosCA_Detalle', @level2type=N'COLUMN', @level2name=N'COD_PAGO'
GO

EXEC sys.sp_addextendedproperty @name=N'COD_CANAL', @value=N'NOMBRE CLIENTE' , @level0type=N'SCHEMA', @level0name=N'TOP_OPERA', @level1type=N'TABLE', @level1name=N'tCRE_PagosCA_Detalle', @level2type=N'COLUMN', @level2name=N'COD_CANAL'
GO

EXEC sys.sp_addextendedproperty @name=N'NOMBRE_DEL_CLIENTE', @value=N'Max 50 caracteres' , @level0type=N'SCHEMA', @level0name=N'TOP_OPERA', @level1type=N'TABLE', @level1name=N'tCRE_PagosCA_Detalle', @level2type=N'COLUMN', @level2name=N'NOMBRE_DEL_CLIENTE'
GO

CREATE INDEX IX_tCRE_PagosCA_Detalle_iIdPagosCA
ON TOP_OPERA.tCRE_PagosCA_Detalle (iIdPagosCA);
