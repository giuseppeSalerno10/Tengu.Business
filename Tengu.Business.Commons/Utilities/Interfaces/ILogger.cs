
namespace Tengu.Business.Commons
{
    public interface ILogger
    {
        void WriteError(string message, Exception? exception = null, bool writeToFile = true);
        void WriteInfo(string message, object? obj = null, bool writeToFile = true);
    }
}