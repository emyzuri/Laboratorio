USE ap_aplicacion
GO

IF EXISTS(SELECT 1 FROM sysobjects WHERE NAME ='spu_clientes' AND type= 'P')
BEGIN 
    DROP PROCEDURE dbo.spu_clientes
    PRINT 'SP spu_clientes eliminado correctamente.'
END 
GO

CREATE PROCEDURE spu_clientes
(
    @i_cl_id INT,
    @i_cl_nombre VARCHAR(300),
    @i_cl_apellido VARCHAR(300),
    @i_cl_telefono VARCHAR(20),
    @i_cl_direccion VARCHAR(500),
    @i_cl_ciudad VARCHAR(100),
    @i_cl_titulo VARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ap_cliente
    SET 
        cl_nombre = @i_cl_nombre,
        cl_apellido = @i_cl_apellido,
        cl_telefono = @i_cl_telefono,
        cl_direccion = @i_cl_direccion,
        cl_ciudad = @i_cl_ciudad,
        cl_titulo = @i_cl_titulo,
        -- Usamos fecha_ingreso como fecha de "última actualización" 
        -- ya que no tienes una columna específica de 'modificación'
        cl_fecha_ingreso = GETDATE() 
    WHERE 
        cl_id = @i_cl_id

    -- Retorna cuántas filas se tocaron (1 si fue exitoso)
    SELECT @@ROWCOUNT; 
END
GO