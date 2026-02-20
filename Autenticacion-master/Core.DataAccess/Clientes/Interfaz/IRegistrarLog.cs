namespace Core.DataAccess.Clientes.Interfaz
{
    public interface IRegistrarLog
    {
        Task RegistrarLog(string controlador, string response, string usuario);
    }
}
