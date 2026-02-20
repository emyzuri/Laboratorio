
namespace Core.Dominio.Model
{
    public class EnsayoDetalladoModel
    {
        public int IdEnsayo { get; set; }
        public string Cedula { get; set; }
        public string NombreCompleto { get; set; }
        public int IdCatalogo { get; set; }
        public string NombreCatalogo { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
