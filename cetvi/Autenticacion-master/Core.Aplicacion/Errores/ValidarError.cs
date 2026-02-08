using FluentValidation.Results;

namespace Core.Aplicacion.Errores
{
    public class ValidarError : Exception
    {
        /// <summary>
        /// Lista de errores
        /// </summary>
        public List<string> Errores { get; }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ValidarError() : base("Erro de validación de modelo requerido.")
        {
            Errores = [];
        }

        /// <summary>
        /// Agrega lista de errores aretornar
        /// </summary>
        /// <param name="erroresValidacion">Lista de errores</param>
        public ValidarError(IEnumerable<ValidationFailure> erroresValidacion) : this()
        {
            foreach (var item in erroresValidacion)
            {
                Errores.Add(item.ErrorMessage);
            }
        }
    }
}
