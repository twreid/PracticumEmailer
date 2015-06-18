using System;

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
        Practicum = 0x16
    }
}