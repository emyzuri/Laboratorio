using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model.Ubicacion;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Ubicacion
{
    public class ObtenerCantonesHandler : IRequestHandler<ObtenerCantonesCom, List<CantonModel>>
    {
        private readonly IUbicacion iUbicacion;

        public ObtenerCantonesHandler(IUbicacion iUbicacion)
        {
            this.iUbicacion = iUbicacion ?? throw new ArgumentNullException(nameof(iUbicacion));
        }

        public async Task<List<CantonModel>> Handle(ObtenerCantonesCom request, CancellationToken cancellationToken)
        {
            if (request.IdProvincia <= 0)
                throw new ArgumentException("Id de provincia no válido");

            return (await iUbicacion.ObtenerCantones(request.IdProvincia)).ToList();
        }
    }
}
