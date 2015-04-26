using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
