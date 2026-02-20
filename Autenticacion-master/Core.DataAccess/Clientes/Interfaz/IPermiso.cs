public interface IPermiso
{
    Task<List<RolModel>> ListarPermisos();
    Task<bool> QuitarPermiso(int idUsuarioRol);
}
