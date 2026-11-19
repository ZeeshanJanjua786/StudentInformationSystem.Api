namespace Sis.Logger
{
    public class ILoggerMiddlewear : ILogger
    {

        private string _categoryName;
        public ILoggerMiddlewear(string categoryName)
        {
            _categoryName = categoryName;
        }
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if(logLevel > LogLevel.Information)
            {
                return true;
            }
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);


          }
    }
}
