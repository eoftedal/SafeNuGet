using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SafeNuGet.NuGet
{
    public class NuGetPackageLoader
    {
        public NuGetPackages LoadPackages(String path)
        {
            using (var stream = File.OpenRead(path))
            {
                return LoadPackages(stream);
            }
        }
        
        public NuGetPackages LoadPackages(Stream packageConfig)
        {
            var serializer = new XmlSerializer(typeof(NuGetPackages));
            return (NuGetPackages)serializer.Deserialize(packageConfig);
        }
    }
}
