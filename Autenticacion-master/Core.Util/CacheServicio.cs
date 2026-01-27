
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Core.Util
{
    public class CacheServicio : ICacheServicio
    {
        private readonly IConnectionMultiplexer redis;
        public CacheServicio(IConnectionMultiplexer redis)
        {
            this.redis = redis;
        }

        public async Task<bool> Agregar<T>(string llave, T valor, TimeSpan tiempoExpiracion)
        {
            var cache = redis.GetDatabase();
            return await cache.StringSetAsync(llave, JsonConvert.SerializeObject(valor), tiempoExpiracion);
        }

        public async Task<T> Obtener<T>(string llave)
        {
            var cache = redis.GetDatabase();
            RedisValue valor = await cache.StringGetAsync(llave);
            if (valor.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(valor);
            }

            return default;
        }
    }
}
