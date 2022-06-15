using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.API.DTO.Base
{
    public class TenguResult<TData> : TenguResult
    {
        public TData Data { get; set; } = default!;
    }

    public class TenguResult
    {
        public bool Success { get; set; }
        public TenguException? Exception { get; set; }
    }
}
