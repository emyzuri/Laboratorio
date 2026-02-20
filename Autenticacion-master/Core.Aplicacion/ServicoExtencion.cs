using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Core.Util;
using Core.DataAccess.Configuracion;
using Core.DataAccess.Clientes.Servicio;
using Core.DataAccess.Clientes.Interfaz;
using Core.DataAccess.Menu.Servicio;
using Core.DataAccess.Menu.Interfaz;

namespace Core.Aplicacion
{
    public static class ServicoExtencion
    {
        /// <summary>
        /// Agrega servicos para inicializar en la aplicación
        /// </summary>
        /// <param name="servicios">Servicio</param>
        public static void AgregarServicios(this IServiceCollection servicios)
        {
            var conexion = new SqlConfiguracion();
            servicios.AddSingleton(conexion);
            servicios.AddScoped<ICliente, ClienteServicio>();
            servicios.AddScoped<IUsuario, UsuarioServicio>();
            servicios.AddScoped<IMenu, MenuServicio>();
            servicios.AddScoped<IEnsayo, EnsayoServicio>();
            servicios.AddScoped<IRegistrarLog, RegistrarLogServicio>();
            servicios.AddScoped<IPermiso, PermisoServicio>();
            servicios.AddSingleton<ICacheServicio, CacheServicio>();
            servicios.AddTransient<MiddlewareExtension>();
            servicios.AddAutoMapper(Assembly.GetExecutingAssembly());
            servicios.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            servicios.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))  ;
        }
    }
}
