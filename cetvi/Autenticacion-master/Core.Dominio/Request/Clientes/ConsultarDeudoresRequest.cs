namespace Core.Dominio.Request.Clientes
{
    public class ConsultarDeudoresRequest
    {
        public int? IdCliente { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
