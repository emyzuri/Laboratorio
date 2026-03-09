using Core.Dominio.Model.Ubicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Clientes.Interfaz
{
    public interface IUbicacion
    {
        /// <summary>
        /// Obtiene el listado completo de provincias de ap_provincia
        /// </summary>
        Task<IEnumerable<ProvinciaModel>> ObtenerProvincias();

        /// <summary>
        /// Obtiene los cantones filtrados por el ID de provincia
        /// </summary>
        Task<IEnumerable<CantonModel>> ObtenerCantones(int idProvincia);

        /// <summary>
        /// Obtiene las parroquias/ciudades filtradas por el ID de cantón
        /// </summary>
        Task<IEnumerable<ParroquiaModel>> ObtenerParroquias(int idCanton);
    }
}
