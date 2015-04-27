using System;
using System.ComponentModel.Composition;

namespace PracticumEmailer.Interfaces.Attributes
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportEmailManagerAttribute : ExportAttribute
    {
        public ExportEmailManagerAttribute()
            : base(typeof (IEmailManager))
        {
        }

        public EmailHandler Handler { get; set; }
    }
}