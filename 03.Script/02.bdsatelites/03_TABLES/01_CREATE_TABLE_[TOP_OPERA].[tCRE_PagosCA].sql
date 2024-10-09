Use bdSatelites
Go
--========================== Identificación del Procedimiento =============================
--Descripción		: Creacion de tabla de pagos para Caja Arequipa -
--.........................................................................................
--Nombre			: CREATE_TABLE_[TOP_OPERA].[tCRE_PagosCA]
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

CREATE TABLE [TOP_OPERA].[tCRE_PagosCA](
	[iIdPagosCA]				[int]	IDENTITY(1,1) NOT NULL,
	[iCantidadRegistros]		[int]	NULL,
	[vAudNombreUsuarioCreacion] [varchar](30) NULL,
	[vNombres]					[varchar](100) NULL,
	[dtFechaCrecion]			[datetime] NULL,
	[vAudIPCreacion]			[varchar](20) NULL,
	[vAudMACCreacion]			[varchar](20) NULL,
	[siCodigoOficinaUsuario]	[smallint] NULL
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'iIdPagosCA', @value=N'Id seguimiento' , @level0type=N'SCHEMA',@level0name=N'TOP_OPERA', @level1type=N'TABLE',@level1name=N'tCRE_PagosCA', @level2type=N'COLUMN',@level2name=N'iIdPagosCA'
GO

EXEC sys.sp_addextendedproperty @name=N'iCantidadRegistros', @value=N'Cantidad Registros' , @level0type=N'SCHEMA',@level0name=N'TOP_OPERA', @level1type=N'TABLE',@level1name=N'tCRE_PagosCA', @level2type=N'COLUMN',@level2name=N'iCantidadRegistros'
GO

EXEC sys.sp_addextendedproperty @name=N'vAudNombreUsuarioCreacion', @value=N'Usuario creacion' , @level0type=N'SCHEMA',@level0name=N'TOP_OPERA', @level1type=N'TABLE',@level1name=N'tCRE_PagosCA', @level2type=N'COLUMN',@level2name=N'vAudNombreUsuarioCreacion'
GO

EXEC sys.sp_addextendedproperty @name=N'vNombres', @value=N'Nombres' , @level0type=N'SCHEMA',@level0name=N'TOP_OPERA', @level1type=N'TABLE',@level1name=N'tCRE_PagosCA', @level2type=N'COLUMN',@level2name=N'vNombres'
GO

EXEC sys.sp_addextendedproperty @name=N'dtFechaCrecion', @value=N'Fecha creacion' , @level0type=N'SCHEMA',@level0name=N'TOP_OPERA', @level1type=N'TABLE',@level1name=N'tCRE_PagosCA', @level2type=N'COLUMN',@level2name=N'dtFechaCrecion'
GO

EXEC sys.sp_addextendedproperty @name=N'vAudIPCreacion', @value=N'IP creacion' , @level0type=N'SCHEMA',@level0name=N'TOP_OPERA', @level1type=N'TABLE',@level1name=N'tCRE_PagosCA', @level2type=N'COLUMN',@level2name=N'vAudIPCreacion'
GO

EXEC sys.sp_addextendedproperty @name=N'vAudMACCreacion', @value=N'MAC creacion' , @level0type=N'SCHEMA',@level0name=N'TOP_OPERA', @level1type=N'TABLE',@level1name=N'tCRE_PagosCA', @level2type=N'COLUMN',@level2name=N'vAudMACCreacion'
GO

EXEC sys.sp_addextendedproperty @name=N'siCodigoOficinaUsuario', @value=N'Codigo oficina usuario' , @level0type=N'SCHEMA',@level0name=N'TOP_OPERA', @level1type=N'TABLE',@level1name=N'tCRE_PagosCA', @level2type=N'COLUMN',@level2name=N'siCodigoOficinaUsuario'
GO									