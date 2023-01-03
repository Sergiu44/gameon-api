using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Exceptions
{
    public class NotFound: Exception
    {
        public NotFound(string message) : base(message) { }
    }
}
