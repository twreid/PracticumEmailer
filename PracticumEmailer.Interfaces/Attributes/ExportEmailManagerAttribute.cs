using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticumEmailer.Interfaces.Attributes
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportEmailManagerAttribute : ExportAttribute
    {
        public ExportEmailManagerAttribute() : base(typeof (IEmailManager))
        {
        }

        public EmailHandler Handler { get; set; }
    }
}
