using System;
using System.Collections.Generic;

namespace PracticumEmailer.Domain
{
    public class TemplateManifest
    {
        public TemplateManifest()
        {
            Files = new List<Tuple<string, string>>();
        }

        public Version Version { get; set; }

        public List<Tuple<string, string>> Files { get; set; }
    }
}