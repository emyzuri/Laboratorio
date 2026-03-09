using Core.Dominio.Comunes;

namespace Core.Dominio.Model
{
    public class CatalogoEnsayoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? IdPadre { get; set; }
        public List<CatalogoEnsayoModel> Hijos { get; set; } = new List<CatalogoEnsayoModel>();
    }
}
