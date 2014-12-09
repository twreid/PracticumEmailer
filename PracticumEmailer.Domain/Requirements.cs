using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticumEmailer.Domain
{
    [Flags]
    public enum Requirements : byte
    {
        None = 0x00,
        Fbi = 0x01,
        Fcsr = 0x02,
        Liab = 0x04,
        Tb = 0x08,
        Practicum = 0x10
    }
}
