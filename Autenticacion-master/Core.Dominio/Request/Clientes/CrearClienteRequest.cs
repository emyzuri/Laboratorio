
namespace Core.Dominio.Request.Clientes
{
    public class CrearClienteRequest
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Titulo { get; set; }
        public string Correo { get; set; }
    }
}
