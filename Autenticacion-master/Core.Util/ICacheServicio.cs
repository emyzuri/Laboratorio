namespace Core.Util
{
    public interface ICacheServicio
    {
        Task<T> Obtener<T>(string llave);
        Task<bool> Agregar<T>(string llave, T valor, TimeSpan tiempoExpiracion);
    }
}
