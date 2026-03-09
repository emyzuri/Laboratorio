using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dominio.Model.Ubicacion
{
    public class CantonModel
    {
        public int Id { get; set; }
        public int IdProvincia { get; set; }
        public string Nombre { get; set; }
    }
}
