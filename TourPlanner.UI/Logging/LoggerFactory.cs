namespace TourPlanner.UI.Logging
{
    public static class LoggerFactory
    {
        public static ILoggerWrapper GetLogger()
        {
            return SerilogWrapper.CreateLogger();
        }
    }
}
