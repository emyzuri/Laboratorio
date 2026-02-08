namespace Core.Dominio.Comunes
{
    /// <summary>
    /// Clase entidad auditoria base
    /// </summary>
    public abstract class EntidadAuditoriaBase
    {
        /// <summary>
        /// Identificador de auditoria
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Usuario que creación
        /// </summary>
        public string UsuarioCreador { get; set; }

        /// <summary>
        /// Fecha creación
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        // <summary>
        /// Usuario que creación
        /// </summary>
        public string UsuarioModificacion { get; set; }

        /// <summary>
        /// Fecha modificación
        /// </summary>
        public DateTime? FechaModificacion { get; set; }
    }
}
