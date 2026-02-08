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

        public InsertarEnsayoHandler(IEnsayo iEnsayo)
        {
            this.iEnsayo = iEnsayo ?? throw new ArgumentNullException(nameof(iEnsayo));
        }

        public async Task<Unit> Handle(InsertarEnsayoCom request, CancellationToken cancellationToken)
        {
            string usuarioNombre = "SISTEMA";

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
                    await iEnsayo.InsertarEnsayo(item, request.Ensayo.IdCliente, idPruebaExistente, usuarioNombre);
                }
            }

            await iEnsayo.RegistrarPago(
                request.Ensayo.IdCliente,
                request.Abono,
                montoTotalParaElPago,
                usuarioNombre,
                idPruebaExistente
            );

            return Unit.Value;
        }
    }
}