using Newtonsoft.Json;
using StackExchange.Redis;

namespace Core.Util
{
    /// <summary>
    /// Servicio de cache para almacenar y recuperar datos de manera eficiente, evitando consultas repetitivas a la base de datos o a servicios externos. Este servicio proporciona métodos para obtener y agregar elementos al cache utilizando una llave única, así como un tiempo de expiración para controlar la validez de los datos almacenados. Al implementar esta interfaz, se puede mejorar el rendimiento de la aplicación al reducir la latencia en la recuperación de datos y disminuir la carga en los recursos del sistema.
    /// </summary>
    public class CacheServicio(IConnectionMultiplexer redis) : ICacheServicio
    {
        /// <summary>
        /// Conexión al servidor Redis, utilizada para interactuar con el cache y realizar operaciones de almacenamiento y recuperación de datos. Esta conexión se inyecta a través del constructor para facilitar la separación de responsabilidades y mejorar la testabilidad de la clase.
        /// </summary>
        private readonly IConnectionMultiplexer redis = redis;

        /// <summary>
        /// Agrega un elemento al cache con una llave única y un tiempo de expiración. Este método es fundamental para almacenar datos de manera eficiente, permitiendo que la aplicación recupere información rápidamente sin necesidad de realizar consultas repetitivas a la base de datos o a servicios externos. El tiempo de expiración asegura que los datos almacenados en el cache sean válidos solo por un período determinado, lo que ayuda a mantener la frescura de la información y evita el uso de datos obsoletos.
        /// </summary>
        /// <typeparam name="T">Objeto a almacenar</typeparam>
        /// <param name="llave">Llave para almacenar</param>
        /// <param name="valor">Valor a almacenar</param>
        /// <param name="tiempoExpiracion">Tiempo de almacenamiento</param>
        /// <returns>True: si se almacenó, False: si no se almaceno</returns>
        public async Task<bool> Agregar<T>(string llave, T valor, TimeSpan tiempoExpiracion)
        {
            IDatabase cache = redis.GetDatabase();
            return await cache.StringSetAsync(llave, JsonConvert.SerializeObject(valor), tiempoExpiracion);
        }

        /// <summary>
        /// Obtiene un elemento del cache utilizando una llave única. Si el elemento no existe o ha expirado, se devuelve null. Este método es esencial para recuperar datos de manera eficiente, evitando la necesidad de realizar consultas repetitivas a la base de datos o a servicios externos, lo que puede mejorar significativamente el rendimiento de la aplicación.
        /// </summary>
        /// <typeparam name="T">Objeto respuesta</typeparam>
        /// <param name="llave">Llave de consulta</param>
        /// <returns>Objeto transaccional</returns>
        public async Task<T> Obtener<T>(string llave)
        {
            IDatabase cache = redis.GetDatabase();
            RedisValue valor = await cache.StringGetAsync(llave);
            if (valor.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(valor);
            }

            return default;
        }

        /// <summary>
        /// Metodo para verificar si una llave existe en el cache. Este método es útil para determinar si un elemento específico está almacenado en el cache antes de intentar recuperarlo, lo que puede ayudar a evitar operaciones innecesarias y mejorar el rendimiento de la aplicación al reducir la latencia en la recuperación de datos.
        /// </summary>
        /// <param name="llave">Llave a validar</param>
        /// <returns>True: existe, False: no existe</returns>
        public async Task<bool> Existe(string llave)
        {
            IDatabase cache = redis.GetDatabase();
            return await cache.KeyExistsAsync(llave);
           
        }
    }
}
