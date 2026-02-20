

namespace Core.Dominio.Model
{
    public class InsertarEnsayoModel
    {
        public int IdCliente { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaEntrega { get; set; }
        public decimal Abono { get; set; }
        public string Usuario { get; set; } 
        public List<EnsayoModel> Ensayos { get; set; } = new List<EnsayoModel>();
    }
}
