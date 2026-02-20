namespace Core.Infraestructura.Extension
{
    public class DbLoggerOptions
    {
        public string ConnectionString { get; set; }
        public string[] LogFiles { get; set; }
        public string LogTable { get; set; }
        public DbLoggerOptions()
        {

        }
    }
}