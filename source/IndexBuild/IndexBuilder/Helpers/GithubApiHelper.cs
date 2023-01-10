using IndexBuilder.Models;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net.Http.Json;

namespace IndexBuilder.Helpers;

public class GithubApiHelper
{
    private readonly HttpClient _client;
    public GithubApiHelper()
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
        _client.DefaultRequestHeaders.Add("User-Agent", "xnocad IndexBuilder");
        _client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
    }
    public async Task<PackageInfo?> GetPackage(RepositoryInfo repositoryInfo)
    {
        var folderContentsUrl = $"https://api.github.com/repos/{repositoryInfo.UserName}/{repositoryInfo.RepositoryName}/contents/{repositoryInfo.FolderName}";

        //var response = await _client.GetAsync(folderContentsUrl);

        //response.EnsureSuccessStatusCode();
        // var folderContents = await response.Content.ReadFromJsonAsync<List<FolderResponseInfo>>();

        //var contentString = await response.Content.ReadAsStringAsync();
        //return null;

        var folderContents = await _client.GetFromJsonAsync<List<FolderResponseInfo>>(folderContentsUrl);

        if (folderContents == null)
            return null;

        folderContents = folderContents.Where(f => f.Type == "file"
                                                && f.Size > 0
                                                && Path.GetExtension(f.Name) == ".package")
                        .ToList();

        var package = new PackageInfo();

        foreach (var item in folderContents)
        {
            try
            {
                var uri = new Uri(item.DownloadUrl);
                //https://raw.githubusercontent.com/mihai-ene-public/xnocad.System.Libraries/main/_packages/System.Libraries.0.2.0-preview.package

                //only items with the same package id
                if (!item.Name.StartsWith(repositoryInfo.PackageId, StringComparison.OrdinalIgnoreCase))
                    continue;

                //we're interested in files in the same branch
                var urlBranch = uri.Segments[3].Trim('/');
                if (urlBranch != repositoryInfo.BranchName)
                    continue;

                var downloadPath = await DownloadFile(item);

                var packInfo = ExtractPackageInfo(downloadPath);

                if (packInfo == null)
                    continue;
                //should be our package
                if (!packInfo.Id.Equals(repositoryInfo.PackageId))
                    continue;

                //we keep replacing values on package because we assume they are presented from lowest version to the highest
                //the last version should have the most complete and updated fields
                package.PackageId = packInfo.Id;
                package.Authors = packInfo.Authors;
                package.Description = packInfo.Description;
                package.Icon = packInfo.Icon;
                package.ProjectUrl = packInfo.ProjectUrl;
                package.Tags = packInfo.Tags;

                package.Versions.Add(packInfo.Version);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        if (package.Versions.Count == 0)
            return null;

        return package;
    }

    private async Task<string> DownloadFile(FolderResponseInfo item)
    {
        var downloadPath = Path.Combine(Path.GetTempPath(), item.Name);
        using (var downloadStream = await _client.GetStreamAsync(item.DownloadUrl))
        {
            using (var fs = new FileStream(downloadPath, FileMode.Create))
            {
                await downloadStream.CopyToAsync(fs);
            }
        }

        return downloadPath;
    }

    private PackageFileMetadaInfo? ExtractPackageInfo(string packagePath)
    {
        using (var fs = new FileStream(packagePath, FileMode.Open, FileAccess.Read))
        {
            using (var zip = new ZipArchive(fs, ZipArchiveMode.Read))
            {
                var infoEntry = zip.Entries.FirstOrDefault(e => e.FullName.EndsWith(".info"));
                if (infoEntry != null)
                {
                    using (var entryStream = infoEntry.Open())
                    {
                        var packInfo = XmlHelper.GetObjectfromStream<PackageFileMetadaInfo>(entryStream);

                        return packInfo;
                    }
                }
            }
        }

        return null;
    }
}
