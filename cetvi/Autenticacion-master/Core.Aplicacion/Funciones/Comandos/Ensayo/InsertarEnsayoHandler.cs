using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class InsertarEnsayoHandler : IRequestHandler<InsertarEnsayoCom, Unit>
    {
        private readonly IEnsayo iEnsayo;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICacheServicio cacheServicio;

        public InsertarEnsayoHandler(IEnsayo iEnsayo, IHttpContextAccessor httpContextAccessor, ICacheServicio cacheServicio)
        {
            this.iEnsayo = iEnsayo ?? throw new ArgumentNullException(nameof(iEnsayo));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.cacheServicio = cacheServicio ?? throw new ArgumentNullException(nameof(cacheServicio));
        }

        public async Task<Unit> Handle(InsertarEnsayoCom request, CancellationToken cancellationToken)
        {
            var idSesion = httpContextAccessor.HttpContext.Request.Headers["IdSesion"].ToString();
            UsuarioModel usuario = await cacheServicio.Obtener<UsuarioModel>(idSesion);

            if (usuario == null) throw new ArgumentException("Sesión no válida");

            int idPruebaExistente = await iEnsayo.ConsultarUltimoIdPrueba();
            double montoTotalParaElPago;

            var pagosExistentes = await iEnsayo.ObtenerPagosPorPrueba(idPruebaExistente);

            if (pagosExistentes != null && pagosExistentes.Any())
            {
                montoTotalParaElPago = pagosExistentes.First().MontoTotal;
            }
            else
            {
                montoTotalParaElPago = (double)request.Ensayo.Ensayos.Sum(e => e.Monto);

                foreach (var item in request.Ensayo.Ensayos)
                {
                    await iEnsayo.InsertarEnsayo(item, request.Ensayo.IdCliente, idPruebaExistente, usuario.Nombre);
                }
            }

            await iEnsayo.RegistrarPago(
                request.Ensayo.IdCliente,
                request.Abono,
                montoTotalParaElPago,
                usuario.Nombre,
                idPruebaExistente
            );

            return Unit.Value;
        }
    }
}