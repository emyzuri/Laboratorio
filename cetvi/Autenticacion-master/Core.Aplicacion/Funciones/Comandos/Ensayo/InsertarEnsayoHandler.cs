
using Core.Aplicacion.Funciones.Comandos.Usuarios;
using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class InsertarEnsayoHandler : IRequestHandler<InsertarEnsayoCom, Unit>
    {
        private readonly IEnsayo iEnsayo;
        private readonly IHttpContextAccessor httpContextAccessor;

        public InsertarEnsayoHandler(IEnsayo iEnsayo, IHttpContextAccessor httpContextAccessor)
        {
            this.iEnsayo = iEnsayo ?? throw new ArgumentException(nameof(iEnsayo));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentException(nameof(httpContextAccessor));
        }
        public async Task<Unit> Handle(InsertarEnsayoCom request, CancellationToken cancellationToken)
        {
            //var idSesion = httpContextAccessor.HttpContext.Request.Headers["IdSesion"].ToString();
            int idPrueba = await iEnsayo.ConsultarUltimoIdPrueba();
            foreach (var item in request.Ensayo.Ensayos)
            {
                await iEnsayo.InsertarEnsayo(item, request.Ensayo.IdCliente, idPrueba + 1, "idSesion");
            }
            return Unit.Value;
        }




    }
}