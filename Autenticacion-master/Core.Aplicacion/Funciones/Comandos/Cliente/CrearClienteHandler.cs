using Core.Aplicacion.RespuestaUtilitario;
using Core.Util;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class CrearClienteHandler : IRequestHandler<CrearClienteCom, Respuesta<int>>
    {
        //private readonly ITipoIdentificacion tipoIdentificacion;

        private readonly ICacheServicio cacheServicio;

        public CrearClienteHandler(ICacheServicio cacheServicio)
        {
            this.cacheServicio = cacheServicio;
        }
        public async Task<Respuesta<int>> Handle(CrearClienteCom request, CancellationToken cancellationToken)
        {
            await cacheServicio.Agregar("llave", "valor", new TimeSpan(0, 2, 0));
            string valor = await cacheServicio.Obtener<string>("llave");
            //var cons = await tipoIdentificacion.ObtenerIdentificaciones();
            return new Respuesta<int>(8);
        }
    }
}
