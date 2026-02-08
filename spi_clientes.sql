USE [ap_aplicacion]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spi_clientes]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[spi_clientes]
GO

CREATE PROCEDURE [dbo].[spi_clientes]
    @i_cl_nombre VARCHAR(300),
    @i_cl_apellido VARCHAR(300),
    @i_cl_telefono VARCHAR(20),
    @i_cl_direccion VARCHAR(500),
    @i_cl_ciudad VARCHAR(100),
    @i_cl_titulo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.ap_cliente (
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
    VALUES (
        @i_cl_nombre, 
        @i_cl_apellido, 
        @i_cl_telefono, 
        @i_cl_direccion, 
        @i_cl_ciudad, 
        @i_cl_titulo, 
        1, 
        GETDATE(), 
        GETDATE()
    );

    SELECT SCOPE_IDENTITY() AS IdGenerado;
END
GO