using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SafeNuGet.Unsafe
{
    public class PackageListLoader
    {
        private const String PackageUrl = "https://raw.github.com/eoftedal/SafeNuGet/master/feed/unsafepackages.xml";

        public UnsafePackages GetCachedUnsafePackages(string cachePath, int cacheTimeInMinutes, out bool cacheHit)
        {
            DirectoryInfo dir = new DirectoryInfo(cachePath);
            if (!dir.Exists) dir.Create();
            FileInfo file = new FileInfo(Path.Combine(dir.FullName, "unsafepackages.xml"));
            cacheHit = true;
            if (!file.Exists && file.LastWriteTime < DateTime.Now.AddMinutes(-cacheTimeInMinutes))
            {
                cacheHit = false;
                new WebClient().DownloadFile(PackageUrl, file.FullName);
            }
            using (var s = file.OpenRead())
            {
                return LoadPackages(s);
            }
        }

        public UnsafePackages GetUnsafePackages()
        {
            var request = WebRequest.Create(PackageUrl);
            using (var response = request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    return LoadPackages(stream);
                }
            }
        
        }
        public UnsafePackages LoadPackages(Stream packages)
        {
            var serializer = new XmlSerializer(typeof(UnsafePackages));
            return (UnsafePackages)serializer.Deserialize(packages);
        }
    }
}
