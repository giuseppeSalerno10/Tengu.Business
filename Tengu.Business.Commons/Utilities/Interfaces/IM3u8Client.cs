
namespace Tengu.Business.Commons
{
    public interface IM3u8Client
    {
        string DownloadPath { get; set; }

        Task Download(string fileName, IEnumerable<string> downloadUrls, CancellationToken cancellationToken = default);
        Task<string[]> GenerateDownloadUrls(string downloadUrl, CancellationToken cancellationToken = default);
    }
}