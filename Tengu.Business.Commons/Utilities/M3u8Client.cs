using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Commons
{
    public class M3u8Client : IM3u8Client
    {
        public string DownloadPath { get; set; } = string.Empty;

        public async Task<string[]> GenerateDownloadUrls(string downloadUrl, CancellationToken cancellationToken = default)
        {
            List<string> downloadUrls = new List<string>();

            var m3u8InfoResponse = downloadUrl.GetStringAsync().Result;

            var streamUrl = m3u8InfoResponse
                .Split("#")[3]
                .Split("\r\n")[1]
                .Replace("./", "");

            var m3u8FinalUrl = downloadUrl.Replace("playlist.m3u8", streamUrl);

            var m3u8FinalUrlResponse = await m3u8FinalUrl.GetStringAsync();

            var tsList = m3u8FinalUrlResponse
                .Trim()
                .Split("#")
                .Where(ts => ts.Contains("EXTINF"));

            foreach (var ts in tsList)
            {
                downloadUrl = string.Empty;

                var rawUrl = m3u8FinalUrl.Split("/");
                rawUrl[^1] = ts.Split("\n")[1];

                foreach (var urlPart in rawUrl)
                {
                    downloadUrl += urlPart + "/";
                }

                downloadUrls.Add(downloadUrl.Remove(downloadUrl.Length - 1));
            }

            return downloadUrls.ToArray();
        }

        public async Task Download(string fileName, IEnumerable<string> downloadUrls, CancellationToken cancellationToken = default)
        {
            var file = File.Create($"{DownloadPath}\\{fileName}");

            var downloadTasks = new List<Task<byte[]>>();

            foreach (var url in downloadUrls)
            {
                downloadTasks.Add(url.GetBytesAsync());
            }

            foreach (var downloadTask in downloadTasks)
            {
                var bytes = await downloadTask;
                file.Write(bytes, 0, bytes.Length);
            }
        }

    }
}
