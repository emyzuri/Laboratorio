USE ap_aplicacion
GO

 --1. Limpieza previa: Borrar el SP si ya existe para evitar conflictos
IF EXISTS(SELECT 1 FROM sysobjects WHERE NAME ='sps_usuarios' AND type= 'P')
BEGIN 
    DROP PROCEDURE dbo.sps_usuarios
    PRINT 'SP sps_usuarios eliminado correctamente.'
END 
GO

-- 2. Creación del Procedimiento
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE sps_usuarios
AS
BEGIN
    SELECT
                    us_id AS 'IdUsuario', 
                    us_nombre AS 'Nombre', 
                    us_apellido AS 'Apellido', 
                    us_login AS 'Usuario', 
                    us_password AS 'Password', 
                    us_telefono AS 'Telefono', 
                    us_cedula AS 'Cedula', 
                    us_fecha_nacimiento AS 'FechaNacimiento', 
                    us_fecha_registro AS 'FechaRegistro', 
                    us_fecha_actualizacion AS 'FechaActualizacion',
                    us_numero_login AS 'NumeroLogin', 
                    us_activo AS 'Activo'
                FROM ap_usuario
                WHERE us_activo = 1
END
GO