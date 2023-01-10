using IndexBuilder.Helpers;
using IndexBuilder.Models;
using System.Text.Json;

namespace IndexBuilder;
internal class Program
{
    static async Task<int> Main(string[] args)
    {

        try
        {
            var repositoriesPath = args[0];//../repositories.txt
            var indexOutputPath = args[1];//../index.json

            Console.WriteLine($"repoPath: {repositoriesPath}");
            Console.WriteLine($"indexPath: {indexOutputPath}");

            var repoLines = File.ReadAllLines(repositoriesPath);

            var repoList = RepositoryParser.ParseLines(repoLines);

            var repoValidator = new RepoInfoValidator();
            var githubHelper = new GithubApiHelper();

            var packages = new List<PackageInfo>();

            foreach (var repoInfo in repoList)
            {
                try
                {
                    repoValidator.Validate(repoInfo);

                    Console.WriteLine($"Fetching package: {repoInfo.PackageId}");

                    var package = await githubHelper.GetPackage(repoInfo);

                    if (package != null)
                    {
                        packages.Add(package);
                        Console.WriteLine($"Fetched package: {repoInfo.PackageId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            var serOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            using var createStream = File.Create(indexOutputPath);
            await JsonSerializer.SerializeAsync(createStream, packages, serOptions);
            await createStream.DisposeAsync();

            Console.WriteLine("Index build finished");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 1;
        }

        return 0;

    }
}