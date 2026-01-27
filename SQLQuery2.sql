USE ap_aplicacion
GO

 --1. Limpieza previa: Borrar el SP si ya existe para evitar conflictos
IF EXISTS(SELECT 1 FROM sysobjects WHERE NAME ='sps_login_usuario' AND type= 'P')
BEGIN 
    DROP PROCEDURE dbo.sps_login_usuario
    PRINT 'SP sps_login_usuario eliminado correctamente.'
END 
GO

-- 2. Creación del Procedimiento
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE sps_login_usuario
(
    @i_us_login VARCHAR(20)='bory', 
    @i_us_password VARCHAR(50) ='123'
)
AS
BEGIN
    SET NOCOUNT ON; -- Buena práctica para evitar mensajes de "n filas afectadas" extra
    
    DECLARE @w_numero_login INT;

    -- PASO 1: VERIFICAR SI EXISTE EL USUARIO
    IF EXISTS (SELECT 1,1 FROM ap_usuario WHERE us_login = @i_us_login)
    BEGIN 
        -- PASO 2: VERIFICAR SI ESTÁ ACTIVO (NO BLOQUEADO)
		IF EXISTS( SELECT 1, 1 FROM ap_usuario WHERE us_login = @i_us_login AND us_activo = 1 )
        BEGIN 
            -- PASO 3: VERIFICAR CONTRASEÑA
            IF EXISTS (SELECT us_id  FROM ap_usuario WHERE us_login = @i_us_login AND us_password = @i_us_password)
            BEGIN 
                -- A) LOGIN EXITOSO
				 -- Reiniciar contador de intentos a 0
                UPDATE ap_usuario SET us_numero_login = 0, us_fecha_ultimo_ingreso = GETDATE()
                WHERE us_login = @i_us_login
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
                WHERE us_login = @i_us_login AND us_password = @i_us_password

               
            END
            ELSE
            BEGIN 
                -- B) CONTRASEÑA INCORRECTA
                -- Aumentamos intentos
                UPDATE ap_usuario SET us_numero_login = us_numero_login + 1
                WHERE us_login = @i_us_login

                -- Verificamos si llegamos al límite (3)
                SELECT @w_numero_login = us_numero_login 
                FROM ap_usuario
                WHERE us_login = @i_us_login

                IF(@w_numero_login >= 3) 
                BEGIN
                    -- Bloquear usuario
                    UPDATE ap_usuario SET us_activo = 0
                    WHERE us_login = @i_us_login
                    
                    PRINT 'RETURN 1002' -- CÓDIGO: Usuario bloqueado tras 3 intentos
                END
                ELSE
                BEGIN
                    PRINT 'RETURN 1001' -- CÓDIGO: Contraseña incorrecta
                END
            END 
        END
        ELSE
        BEGIN
             PRINT 'RETURN 1002' -- CÓDIGO: Usuario Bloqueado / Inactivo (Existe pero activo=0)
        END
    END
    ELSE 
    BEGIN
        PRINT 'RETURN 1004' -- CÓDIGO: No existe usuario 
    END
END
GO