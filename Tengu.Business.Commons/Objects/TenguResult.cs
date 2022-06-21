using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.Commons.Objects
{
    public class TenguResult<TData> : TenguResult
    {
        public TData Data { get; set; } = default!;
    }

    public class TenguResult
    {
        public Hosts Host { get; set; } = Hosts.None;
        public bool Success { get; set; }
        public TenguException? Exception { get; set; }
    }
}
