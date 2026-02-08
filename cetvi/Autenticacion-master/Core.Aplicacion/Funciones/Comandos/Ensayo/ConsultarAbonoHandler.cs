using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Request.Ensayos;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class ConsultarAbonoHandler : IRequestHandler<ConsultarAbonoCom, IEnumerable<ConsultarAbonoRequest>>
    {
        private readonly IEnsayo _iEnsayo;

        public ConsultarAbonoHandler(IEnsayo iEnsayo)
        {
            _iEnsayo = iEnsayo;
        }

        public async Task<IEnumerable<ConsultarAbonoRequest>> Handle(ConsultarAbonoCom request, CancellationToken cancellationToken)
        {
            return await _iEnsayo.ObtenerAbonosPorCliente(request.IdCliente);
        }
    }
}