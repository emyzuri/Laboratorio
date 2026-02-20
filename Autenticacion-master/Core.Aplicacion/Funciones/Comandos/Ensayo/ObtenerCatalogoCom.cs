using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class ObtenerCatalogoCom : IRequest<IEnumerable<CatalogoEnsayoModel>>
    {
    }
}
