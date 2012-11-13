using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace PracticumEmailer.Data
{
    public class DataFactory
    {
        [Import]
        public IDataAccess DataAccess { get; set; }
    }
}
