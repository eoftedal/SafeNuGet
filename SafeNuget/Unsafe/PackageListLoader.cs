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
        public UnsafePackages GetUnsafePackages()
        {
            var request = WebRequest.Create("https://raw.github.com/eoftedal/SafeNuGet/master/feed/unsafepackages.xml");
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
