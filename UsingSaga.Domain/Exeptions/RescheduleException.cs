using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingSaga.Domain.Exeptions
{
    public class RescheduleException : Exception
    {

        public RescheduleException(string msg) : base(msg) { }

    }
}
