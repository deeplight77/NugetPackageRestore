using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NugetPackageRestore
{
    public class PackageInfo
    {
        [XmlAttribute("id")]
        public string Name { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlAttribute("targetFramework")]
        public string TargetFramework { get; set; }

        public string FolderName { get { return string.Format("{0}.{1}", this.Name, this.Version); } }
    }
}
