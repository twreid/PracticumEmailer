using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace PracticumEmailer.Util
{
    public static class TemplateManifestUtils
    {
        public static string GetFileHash(FileInfo file)
        {
            using(var sha = new SHA256Managed())
            using(var sourceStream = file.Open(FileMode.Open))
            using (var bufferedStream = new BufferedStream(sourceStream))
            {
                return Convert.ToBase64String(sha.ComputeHash(bufferedStream));
            }
        } 
    }
}
