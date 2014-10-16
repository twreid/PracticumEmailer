﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticumEmailer.Interfaces
{
    [Flags]
    public enum Requirements
    {
        None,
        Fbi,
        Fcsr,
        Liab,
        Tb,
    }

    public interface IStudentManager
    {
        Requirements DetermineRequirements(Domain.Student student);

        bool IsCleared(Domain.Student student, Requirements requirements);

    }
}