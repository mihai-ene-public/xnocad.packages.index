using IndexBuilder.Helpers;

namespace IndexBuilder.Tests;

public class RepositoryParserTests
{
    [Theory]
    [InlineData("System.Libraries|https://github.com/mihai-ene-public/xnocad.System.Libraries/tree/main/_packages",
        "System.Libraries",
        "mihai-ene-public",
        "xnocad.System.Libraries",
        "main",
        "_packages"
        )]
    public void ParseLine(
        string line,
        string expectedPackageId,
        string expectedUserName,
        string expectedRepoName,
        string expectedBranchName,
        string expectedFolderName
        )
    {
        var info = RepositoryParser.ParseLine(line);

        Assert.NotNull(info);
        Assert.Equal(expectedPackageId, info.PackageId);
        Assert.Equal(expectedUserName, info.UserName);
        Assert.Equal(expectedRepoName, info.RepositoryName);
        Assert.Equal(expectedBranchName, info.BranchName);
        Assert.Equal(expectedFolderName, info.FolderName);
    }
}