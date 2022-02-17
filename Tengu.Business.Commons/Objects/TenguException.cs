using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Commons.Objects
{
    public class TenguException : Exception
    {
        public TenguException(string message) : base(message) { }
    }
}
