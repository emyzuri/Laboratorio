using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class ValidarClienteCom : IRequest<ClienteModel>
    {
        /// <summary>
        /// Clave del usuario
        /// </summary>
        public string Clave { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        public string Usuario { get; set; }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="clave">Clave del usuario</param>
        /// <param name="usuario">Usuario</param>
        public ValidarClienteCom(string clave, string usuario)
        {
            this.Clave = clave;
            this.Usuario = usuario;
        }
    }
}
