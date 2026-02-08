
namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    [Serializable]
    internal class ExcepcionOperativa : Exception
    {
        private int conflict;
        private string v;

        public ExcepcionOperativa()
        {
        }

        public ExcepcionOperativa(string? message) : base(message)
        {
        }

        public ExcepcionOperativa(int conflict, string v)
        {
            this.conflict = conflict;
            this.v = v;
        }

        public ExcepcionOperativa(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}