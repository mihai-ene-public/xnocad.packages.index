using System.Text.Json;
using System.Text.Json.Serialization;

namespace IndexBuilder.Helpers;

public class FolderResponseInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("download_url")]
    public string DownloadUrl { get; set; }
}
