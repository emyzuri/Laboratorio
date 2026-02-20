using Dapper;
using System.Data;
using Core.Dominio.Model;
using Core.DataAccess.Configuracion;

public class PermisoServicio : IPermiso
{
    readonly SqlConfiguracion sqlConfiguracion;
    public PermisoServicio(SqlConfiguracion sqlConfig) => sqlConfiguracion = sqlConfig;
    public async Task<List<RolModel>> ListarPermisos()
    {
        using IDbConnection db = sqlConfiguracion.CrearConexion();
        var resultado = await db.QueryAsync<RolModel>(
            "sps_listar_permisos",
            commandType: CommandType.StoredProcedure);

        return resultado.ToList();
    }

    public async Task<bool> QuitarPermiso(int idUsuarioRol)
    {
        using IDbConnection db = sqlConfiguracion.CrearConexion();
        var parametros = new DynamicParameters();
        parametros.Add("@i_ur_id", idUsuarioRol);

        var filasAfectadas = await db.ExecuteAsync(
            "spd_quitar_permiso",
            parametros,
            commandType: CommandType.StoredProcedure);

        return filasAfectadas > 0;
    }
}