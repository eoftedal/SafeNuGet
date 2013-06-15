using SafeNuGet.NuGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SafeNuGet.Unsafe
{
    [XmlRoot(Namespace = "", ElementName = "unsafepackages")]
    public class UnsafePackages : List<UnsafePackage>
    {
    }

    [XmlType(TypeName = "package")]
    public class UnsafePackage
    {
        [XmlAttribute(AttributeName = "id")]
        public String Id { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public String Version { get; set; }
        [XmlAttribute(AttributeName = "before")]
        public String Before { get; set; }
        [XmlAttribute(AttributeName = "beforeOrAt")]
        public String BeforeOrAt { get; set; }
        [XmlAttribute(AttributeName = "after")]
        public String After { get; set; }
        [XmlAttribute(AttributeName = "afterOrAt")]
        public String AfterOrAt { get; set; }
        [XmlAttribute(AttributeName = "infoUri")]
        public String InfoUri { get; set; }


        public Boolean Is(NuGetPackage nugetPackage) 
        {
            if (nugetPackage.Id != Id)
                return false;
            var nugetVersion = new PackageVersion(nugetPackage.Version);
            if (!String.IsNullOrWhiteSpace(Version) && new PackageVersion(Version) == nugetVersion)
            {
                return true;
            }
            bool? before = null;
            bool? after = null;
            if (!String.IsNullOrWhiteSpace(Before))
                before = new PackageVersion(Before) > nugetVersion;
            if (!String.IsNullOrWhiteSpace(BeforeOrAt))
                before = new PackageVersion(BeforeOrAt) >= nugetVersion;
            if (!String.IsNullOrWhiteSpace(After))
                after = new PackageVersion(After) < nugetVersion;
            if (!String.IsNullOrWhiteSpace(AfterOrAt))
                after = new PackageVersion(AfterOrAt) <= nugetVersion;
            return before.HasValue && before.Value && after.HasValue && after.Value
                || !before.HasValue && after.HasValue && after.Value
                || before.HasValue && before.Value && !after.HasValue;
            
        }

    }
}
