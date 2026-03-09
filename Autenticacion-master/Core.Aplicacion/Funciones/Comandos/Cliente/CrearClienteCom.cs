using Core.Aplicacion.RespuestaUtilitario;
using Core.Dominio.Clientes;
using Core.Dominio.Model;
using Core.Dominio.Request.Clientes;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class CrearClienteCom : IRequest<ClienteModel>
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Titulo { get; set; }
        public string Correo { get; set; }
        public CrearClienteCom(CrearClienteRequest cliente)
        {
            this.Cedula = cliente.Cedula;
            this.Nombre = cliente.Nombre;
            this.Apellido = cliente.Apellido;
            this.Telefono = cliente.Telefono;
            this.Direccion = cliente.Direccion;
            this.Ciudad = cliente.Ciudad;
            this.Titulo = cliente.Titulo;
            this.Correo = cliente.Correo;
        }
    }
}