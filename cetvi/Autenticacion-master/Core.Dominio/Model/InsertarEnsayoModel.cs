
using Core.Dominio.Request.Ensayos;

namespace Core.Dominio.Model
{
    public class InsertarEnsayoModel
    {
        public int IdCliente { get; set; }
        public string Descripcion { get; set; }
        public List<EnsayoModel> Ensayos { get; set; } = new List<EnsayoModel>();
    }
}
