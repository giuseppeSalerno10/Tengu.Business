namespace Tengu.Business.Core.DTO.Output.AnimeUnity
{
    public class AnimeUnityCreateSessionOutput
    {
        public string XSRFToken { get; internal set; } = string.Empty;
        public string CSRFToken { get; internal set; } = string.Empty;
        public string AnimeUnitySession { get; internal set; } = string.Empty;
        public string XSRFCookieToken { get; internal set; } = string.Empty;
    }
}
