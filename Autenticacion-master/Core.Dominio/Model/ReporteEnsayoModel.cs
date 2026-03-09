using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dominio.Model
{
    public class ReporteEnsayoModel
    {
        public int IdEnsayo { get; set; }
        public string Cliente { get; set; }
        public DateTime FechaRegistro { get; set; }
        public decimal TotalAPagar { get; set; }
        public decimal TotalAbonado { get; set; }
        public decimal SaldoPendiente { get; set; }
    }
}
