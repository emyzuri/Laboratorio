namespace Core.Dominio.Request.Ensayos
{
    public class InsertarEnsayoRequest
    {
        public int IdCliente { get; set; }
        public string Descripcion { get; set; }
        public decimal Abono { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int IdParroquia { get; set; }
        public List<EnsayoRequest> Ensayos { get; set; } = new List<EnsayoRequest>();
    }
}
