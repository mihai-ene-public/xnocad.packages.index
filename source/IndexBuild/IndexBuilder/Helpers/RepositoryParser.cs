using IndexBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexBuilder.Helpers;

public class RepositoryParser
{
    public static RepositoryInfo ParseLine(string line)
    {
        //System.Libraries|https://github.com/mihai-ene-public/xnocad.System.Libraries/tree/main/_packages
        //{packageId}|{schema}/{domain}/{user}/{repo}/tree/{branch}/{folderName}

        var lineSplit = line.Split('|');
        var packageId = lineSplit[0];
        var url = lineSplit[1];

        if (string.IsNullOrWhiteSpace(packageId))
            throw new Exception("No package id info exists on this line");
        if (string.IsNullOrWhiteSpace(url))
            throw new Exception("No package url info exists on this line");

        var uri = new Uri(url);

        if (uri.Segments.Length != 6)
            throw new Exception("Packages folder Url does not have the right format.");

        var user = uri.Segments[1].Trim('/');
        var repo = uri.Segments[2].Trim('/');
        var branch = uri.Segments[4].Trim('/');
        var folderName = uri.Segments[5].Trim('/');

        return new RepositoryInfo
        {
            PackageId = packageId,
            UserName = user,
            RepositoryName = repo,
            BranchName = branch,
            FolderName = folderName
        };
    }

    public static IList<RepositoryInfo> ParseLines(string[] lines)
    {
        var result = new List<RepositoryInfo>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var repoInfo = ParseLine(line);

            if (repoInfo != null)
                result.Add(repoInfo);
        }

        return result;
    }
}
