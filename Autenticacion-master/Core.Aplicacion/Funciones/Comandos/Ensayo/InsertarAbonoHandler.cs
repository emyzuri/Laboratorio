using Core.DataAccess.Clientes.Interfaz; 
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class InsertarAbonoHandler : IRequestHandler<InsertarAbonoCom, bool>
    {
        private readonly IEnsayo _ensayoRepo;

        public InsertarAbonoHandler(IEnsayo ensayoRepo)
        {
            _ensayoRepo = ensayoRepo;
        }

        public async Task<bool> Handle(InsertarAbonoCom request, CancellationToken cancellationToken)
        {
            return await _ensayoRepo.RegistrarNuevoAbono(request.IdEnsayo, request.Monto, request.Usuario);
        }
    }
}