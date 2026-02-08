using Core.Dominio.Model;
using Core.Dominio.Request.Ensayos;
using MediatR;
using System.Linq;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class InsertarEnsayoCom : IRequest<Unit>
    {
        public InsertarEnsayoModel Ensayo { get; set; }
        public double Abono { get; set; }
        public InsertarEnsayoCom() { }
        public InsertarEnsayoCom(InsertarEnsayoRequest request)
        {
            this.Abono = request.Abono;
            this.Ensayo = new InsertarEnsayoModel
            {
                IdCliente = request.IdCliente,
                Descripcion = request.Descripcion,
                Ensayos = request.Ensayos.Select(e => new EnsayoModel
                {
                    IdCatalogo = e.IdCatalogo,
                    Monto = e.Monto
                }).ToList()
            };
        }
    }
}