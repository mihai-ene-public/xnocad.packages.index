namespace IndexBuilder.Models;

public class PackageInfo
{
    public string PackageId { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public string ProjectUrl { get; set; }
    public string Authors { get; set; }
    public string Tags { get; set; }

    public IList<string> Versions { get; set; } = new List<string>();
}
