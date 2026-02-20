using Core.Dominio.Comunes;

namespace Core.Dominio.Model
{
    public class CatalogoEnsayoModel : EntidadAuditoriaBase
    {
        public int IdCatalogo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
