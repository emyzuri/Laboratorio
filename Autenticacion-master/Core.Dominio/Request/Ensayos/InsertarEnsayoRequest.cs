namespace Core.Dominio.Request.Ensayos
{
    public class InsertarEnsayoRequest
    {
        public int IdCliente { get; set; }
        public string Descripcion { get; set; }
        public double Abono { get; set; }
        public List<EnsayoRequest> Ensayos { get; set; } = new List<EnsayoRequest>();


    }
}
