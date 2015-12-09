using System;

namespace PracticumEmailer.Domain
{
    public enum Requirements : byte
    {
        None = 0,
        Fbi = 0x1,
        Fcsr = 0x2,
        Liab = 0x4,
        Tb = 0x8,
        Practicum = 0x10,
    }
}