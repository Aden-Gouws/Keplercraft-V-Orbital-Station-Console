using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keplercraft_V_Orbital_Station_Console
{
    internal class SystemNotOperational : Exception
    {
        public SystemNotOperational(string message) : base(message) { }
    }
}
