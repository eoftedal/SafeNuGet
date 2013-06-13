using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SafeNuGet.NuGet
{
    [XmlRoot(Namespace="", ElementName="packages")]
    public class NuGetPackages : List<NuGetPackage>
    {
    }

    [XmlType(TypeName="package")]
    public class NuGetPackage
    {
        [XmlAttribute(AttributeName="id")]
        public String Id { get; set; }
        [XmlAttribute(AttributeName="version")]
        public String Version { get; set; }
    }
}
