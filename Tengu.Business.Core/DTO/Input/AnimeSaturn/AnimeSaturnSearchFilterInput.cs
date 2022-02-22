using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public class AnimeSaturnSearchFilterInput
    {
        public IEnumerable<string> Genres { get; set; } = Array.Empty<string>();
        public string Year { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
