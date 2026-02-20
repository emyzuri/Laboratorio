
namespace Core.Dominio.Model
{
    public class ClienteDeudorModel
    {

        public int IdCliente { get; set; }
        public string Cedula { get; set; }
        public string NombreCompleto { get; set; }
        //public string NombreCatalogo { get; set; }
        public int IdEnsayo { get; set; }
        public decimal TotalAbonado { get; set; }
        public decimal TotalAPagar { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string NombreCliente { get; set; }
        public IEnumerable<EnsayoDetalladoModel> Ensayos { get; set; }

    }
}
