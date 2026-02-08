using Core.Dominio.Request.Ensayos;
using MediatR;
using System.Collections.Generic;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class ConsultarAbonoCom : IRequest<IEnumerable<ConsultarAbonoRequest>>
    {
        public int IdCliente { get; set; }
        public ConsultarAbonoCom(int idCliente) => IdCliente = idCliente;
    }
}