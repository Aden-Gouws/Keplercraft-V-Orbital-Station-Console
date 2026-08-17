using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keplercraft_V_Orbital_Station_Console
{
    public class ValueBelowZero : Exception
    {
        public ValueBelowZero(string message)
            : base(message)
        {
        }
    }
}
