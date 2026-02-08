
namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    [Serializable]
    internal class ManejoExcepciones : Exception
    {
        private object conflict;
        private string v;

        public ManejoExcepciones()
        {
        }

        public ManejoExcepciones(string? message) : base(message)
        {
        }

        public ManejoExcepciones(object conflict, string v)
        {
            this.conflict = conflict;
            this.v = v;
        }

        public ManejoExcepciones(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}