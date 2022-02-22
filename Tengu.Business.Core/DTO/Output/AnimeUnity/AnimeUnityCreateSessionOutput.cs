using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Core
{
    public class AnimeUnityCreateSessionOutput
    {
        public string XSRFToken { get; internal set; } = string.Empty;
        public string CSRFToken { get; internal set; } = string.Empty;
        public string AnimeUnitySession { get; internal set; } = string.Empty;
        public string XSRFCookieToken { get; internal set; } = string.Empty;
    }
}
