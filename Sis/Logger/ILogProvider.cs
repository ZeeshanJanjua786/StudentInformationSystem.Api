namespace Sis.Logger
{
    public class ILogProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new ILoggerMiddlewear(categoryName);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
