using Core.Aplicacion.RespuestaUtilitario;
using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model.Ubicacion;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Ubicacion
{
    public class ObtenerParroquiasHandler : IRequestHandler<ObtenerParroquiasCom, Respuesta>
    {
        private readonly IUbicacion _iUbicacion;

        public ObtenerParroquiasHandler(IUbicacion iUbicacion)
        {
            _iUbicacion = iUbicacion ?? throw new ArgumentNullException(nameof(iUbicacion));
        }

        public async Task<Respuesta> Handle(ObtenerParroquiasCom request, CancellationToken cancellationToken)
        {
            if (request.IdCanton <= 0)
                throw new ArgumentException("Id de cantón no válido");

            var lista = (await _iUbicacion.ObtenerParroquias(request.IdCanton)).ToList();

            return new Respuesta
            {
                EsExitoso = true, 
                Datos = lista
            };
        }
    }
}