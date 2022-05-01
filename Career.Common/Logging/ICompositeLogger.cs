namespace Career.Common.Logging
{
    public interface ICompositeLogger
    {
        void Info(string message, object[] args);

        void Error(string message, Exception ex);

        void Warning(string message, Exception ex);

        void Warning(string message, object[] args);
    }
}