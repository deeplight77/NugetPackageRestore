using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NugetPackageRestore
{
    [XmlRoot("packages")]
    public class NuGetPackageConfig
    {
        public NuGetPackageConfig()
        {
            Packages = new List<PackageInfo>();
        }

        [XmlElement("package")]
        public List<PackageInfo> Packages { get; set; }
    }
}
