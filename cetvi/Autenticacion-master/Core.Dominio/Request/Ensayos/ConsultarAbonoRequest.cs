
namespace Core.Dominio.Request.Ensayos
{
    public class ConsultarAbonoRequest
    {
        public int IdPago { get; set; }
        public double AbonoRealizado { get; set; }
        public double Saldo { get; set; }
        public string Usuario { get; set; }
        public string Fecha { get; set; }
    }
}
