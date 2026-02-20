
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Roles
{
    public class ListarPermisosHandler : IRequestHandler<ListarPermisosCom, List<RolModel>>
    {
        readonly IPermiso _permisoServicio;
        public ListarPermisosHandler(IPermiso permisoServicio) => _permisoServicio = permisoServicio;

        public async Task<List<RolModel>> Handle(ListarPermisosCom request, CancellationToken ct)
            => await _permisoServicio.ListarPermisos();
    }

    public class QuitarPermisoHandler : IRequestHandler<QuitarPermisoCom, bool>
    {
        readonly IPermiso _permisoServicio;
        public QuitarPermisoHandler(IPermiso permisoServicio) => _permisoServicio = permisoServicio;

        public async Task<bool> Handle(QuitarPermisoCom request, CancellationToken ct)
            => await _permisoServicio.QuitarPermiso(request.IdUsuarioRol);
    }
}
