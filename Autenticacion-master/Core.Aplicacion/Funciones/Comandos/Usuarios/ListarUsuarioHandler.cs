using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    internal class ListarTodosUsuariosHandler : IRequestHandler<ListarTodosUsuariosCom, List<UsuarioModel>>
    {
        readonly IUsuario iUsuario;

        public ListarTodosUsuariosHandler(IUsuario iUsuario)
        {
            this.iUsuario = iUsuario ?? throw new ArgumentException(nameof(iUsuario));
        }

        public async Task<List<UsuarioModel>> Handle(ListarTodosUsuariosCom request, CancellationToken cancellationToken)
        {
            return await iUsuario.ObtenerUsuarios();
        }
    }
}