using FluentValidation;
using MediatR;

namespace Core.Aplicacion.Validaciones
{
    /// <summary>
    /// Constructor de la clase
    /// </summary>
    /// <param name="validador">Objeto a validar</param>
    public class ValidadorModelo<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validador) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Validador
        /// </summary>
        private readonly IEnumerable<IValidator<TRequest>> validador = validador;

        /// <summary>
        /// Validador de propiedades
        /// </summary>
        /// <param name="request">Objeto requerido a validar</param>
        /// <param name="next">Sigue el flujo</param>
        /// <param name="cancellationToken">Control de errores</param>
        /// <returns>Objeto respuesta</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validador.Any())
            {

                ValidationContext<TRequest> context = new FluentValidation.ValidationContext<TRequest>(request);
                var resultadoValidacion = await Task.WhenAll(validador.Select(x => x.ValidateAsync(context, cancellationToken)));
                var errores = resultadoValidacion.SelectMany(r => r.Errors).Where(f => f != null).ToList();
                if (errores.Count != 0)
                {
                    throw new ValidationException(errores);
                }
            }

            return await next();
        }
    }
}
