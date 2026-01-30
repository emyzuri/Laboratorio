USE ap_aplicacion
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ap_cliente]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ap_cliente](
	[cl_id] [int] IDENTITY(1,1) NOT NULL,
	[cl_nombre] [varchar](300) NOT NULL,
	[cl_apellido] [varchar](300) NOT NULL,
	[cl_telefono] [varchar](20) NULL,
	[cl_direccion] [varchar](500) NULL,
	[cl_ciudad] [varchar](100) NULL,
	[cl_titulo] [varchar](100) NULL,      -- Título (ej: Ing, Lic, Sr.)
	[cl_estado] [bit] NOT NULL DEFAULT 1, -- 1: Activo, 0: Inactivo
	[cl_fecha_registro] [datetime] NOT NULL DEFAULT GETDATE(),
	[cl_fecha_ingreso] [datetime] NULL,   -- Fecha específica que pediste
 CONSTRAINT [PK_ap_cliente] PRIMARY KEY CLUSTERED 
(
	[cl_id] ASC
)
) ON [PRIMARY]
END
GO
USE ap_aplicacion
GO

-- Limpieza previa
IF EXISTS(SELECT 1 FROM sysobjects WHERE NAME ='spi_cliente' AND type= 'P')
BEGIN 
    DROP PROCEDURE dbo.spi_cliente
    PRINT 'SP spi_cliente eliminado correctamente.'
END 
GO

CREATE PROCEDURE spi_cliente
(
    @i_cl_nombre VARCHAR(300),
    @i_cl_apellido VARCHAR(300),
    @i_cl_telefono VARCHAR(20),
    @i_cl_direccion VARCHAR(500),
    @i_cl_ciudad VARCHAR(100),
    @i_cl_titulo VARCHAR(100),
    @i_cl_fecha_ingreso DATETIME
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ap_cliente
    (
        cl_nombre,
        cl_apellido,
        cl_telefono,
        cl_direccion,
        cl_ciudad,
        cl_titulo,
        cl_estado,
        cl_fecha_registro,
        cl_fecha_ingreso
    )
    VALUES
    (
        @i_cl_nombre,
        @i_cl_apellido,
        @i_cl_telefono,
        @i_cl_direccion,
        @i_cl_ciudad,
        @i_cl_titulo,
        1,          -- Se crea activo por defecto
        GETDATE(),  -- Fecha de registro automática (hoy)
        @i_cl_fecha_ingreso
    )
    
    SELECT SCOPE_IDENTITY() AS NewId;
END
GO

USE ap_aplicacion
GO

-- 1. Borramos el anterior para actualizarlo
IF EXISTS(SELECT 1 FROM sysobjects WHERE NAME ='sps_clientes' AND type= 'P')
BEGIN 
    DROP PROCEDURE dbo.sps_clientes
    PRINT 'SP sps_clientes eliminado correctamente.'
END 
GO

-- 2. Creamos la versión CON PARÁMETROS DE BÚSQUEDA
CREATE PROCEDURE sps_clientes
(
    @i_cl_id INT = NULL,           -- Parámetro opcional para buscar por ID
    @i_cl_nombre VARCHAR(300) = NULL -- Parámetro opcional para buscar por Nombre
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        cl_id AS 'IdCliente',
        cl_nombre AS 'Nombre',
        cl_apellido AS 'Apellido',
        cl_telefono AS 'Telefono',
        cl_direccion AS 'Direccion',
        cl_ciudad AS 'Ciudad',
        cl_titulo AS 'Titulo',
        cl_estado AS 'Estado',
        cl_fecha_registro AS 'FechaRegistro',
        cl_fecha_ingreso AS 'FechaIngreso'
    FROM ap_cliente
    WHERE cl_estado = 1
      -- Truco para filtros opcionales:
      AND (@i_cl_id IS NULL OR cl_id = @i_cl_id) 
      AND (@i_cl_nombre IS NULL OR cl_nombre LIKE '%' + @i_cl_nombre + '%')
END
GO
USE ap_aplicacion
GO

INSERT INTO ap_cliente 
(
    cl_nombre, 
    cl_apellido, 
    cl_telefono, 
    cl_direccion, 
    cl_ciudad, 
    cl_titulo, 
    cl_estado, 
    cl_fecha_registro, 
    cl_fecha_ingreso
)
VALUES 
(
    'emy',           
    'zurita',       
    '0987449708',    
    'darquea',       
    'riobamba',     
    'emy',          
    1,             
    GETDATE(),      
    GETDATE()  
)
GO

SELECT * FROM ap_cliente;