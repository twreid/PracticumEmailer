using System;
using System.IO;
using System.Security.Cryptography;

namespace PracticumEmailer.Util
{
    public static class TemplateManifestUtils
    {
        public static string GetFileHash(FileInfo file)
        {
            FileStream sourceStream = file.Open(FileMode.Open);
            using (var sha = new SHA256Managed())
            using (var bufferedStream = new BufferedStream(sourceStream))
            {
                return Convert.ToBase64String(sha.ComputeHash(bufferedStream));
            }
        }
    }
}