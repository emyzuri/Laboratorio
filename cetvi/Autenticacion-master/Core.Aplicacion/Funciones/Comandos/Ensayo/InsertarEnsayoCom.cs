
using Core.Dominio.Model;
using Core.Dominio.Request.Ensayos;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class InsertarEnsayoCom : IRequest<Unit>
    {
        public InsertarEnsayoModel Ensayo { get; set; }
        public InsertarEnsayoCom(InsertarEnsayoRequest ensayo)
        {

            Ensayo = new InsertarEnsayoModel
            {
                IdCliente = ensayo.IdCliente,
                Descripcion = ensayo.Descripcion,
                Ensayos = ensayo.Ensayos.Select(e => new EnsayoModel
                {
                    IdCatalogo = e.IdCatalogo,
                    Monto = e.Monto
                }).ToList()
            };
        }
    }
}
