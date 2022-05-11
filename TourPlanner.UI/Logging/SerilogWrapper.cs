using Serilog;

namespace TourPlanner.UI.Logging
{
    /// <summary>
    ///  Setup Serilog Global Logger
    /// </summary>
    class SerilogWrapper : ILoggerWrapper
    {

        public static SerilogWrapper CreateLogger() 
        {
            Log.Logger = new LoggerConfiguration()
                            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                            .MinimumLevel.Verbose()
                            .CreateLogger();
            
            return new SerilogWrapper();
        }

        private SerilogWrapper(){}

        // Fatal — used for reporting about errors that are forcing shutdown of the application.
        public void Fatal(string message)
        {
            Log.Logger.Fatal(message);
        }

        // Error — used for logging serious problems occurred during execution of the program.
        public void Error(string message)
        {
            Log.Logger.Error(message);
        }

        // Warning  — used for reporting non-critical unusual behavior.
        public void Warning(string message)
        {
            Log.Logger.Warning(message);
        }

        // Information — used for informative messages highlighting the progress of the application for sysadmin and end user.
        public void Information(string message)
        {
            Log.Logger.Information(message);
        }

        // Debug — used for debugging messages with extended information about application processing.
        public void Debug(string message)
        {
            Log.Logger.Debug(message);
        }

        // Verbose — the noisiest level, used for tracing the code.
        public void Verbose(string message)
        {
            Log.Logger.Verbose(message);
        }
    }
}
