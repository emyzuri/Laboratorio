using Core.Dominio.Comunes;

namespace Core.Dominio.Model
{
    public class ClienteModel : EntidadAuditoriaBase
    {
        public int IdCliente { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Titulo { get; set; }
        public string Estado { get; set; }
        public string Correo { get; set; }
        public Guid IdSesion { get; set; }

        public ClienteModel()
        {
            IdSesion = Guid.NewGuid();
        }
    }
}
