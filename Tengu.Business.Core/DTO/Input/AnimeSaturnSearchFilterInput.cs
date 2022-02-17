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
        public IEnumerable<string> Years { get; set; } = Array.Empty<string>();
        public IEnumerable<string> Statuses { get; set; } = Array.Empty<string>();
        public string Language { get; set; } = string.Empty;
    }
}
