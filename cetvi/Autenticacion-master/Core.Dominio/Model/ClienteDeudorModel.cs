
namespace Core.Dominio.Model
{
    public class ClienteDeudorModel
    {
        public string NombreCompleto { get; set; }
        public string Ensayo { get; set; }
        public double TotalAbonado { get; set; }
        public double TotalAPagar { get; set; }
        public double SaldoPendiente { get; set; }
        public DateTime FechaUltimoMovimiento { get; set; }
    }
}
