using System.Xml.Serialization;

namespace IndexBuilder.Models;

//same as class "PackageMetadata" from xnocad
[XmlRoot("PackageMetadata")]
public class PackageFileMetadaInfo
{
    public string Id { get; set; }

    public string Version { get; set; }

    public string Authors { get; set; }

    public string License { get; set; }
    public string LicenseUrl { get; set; }
    public string Icon { get; set; }
    public string ProjectUrl { get; set; }
    public string Description { get; set; }
    public string Copyright { get; set; }
    public string Tags { get; set; }
    //public PackageMetadataRepository Repository { get; set; }
}
