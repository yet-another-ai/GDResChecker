using System.Text.Json;

using Microsoft.Extensions.FileSystemGlobbing;

namespace GDResChecker.Checker;

public static class Program
{
    private static HashSet<string> CheckResourceExists(string rootPath, List<string> allFilesPath)
    {
        var sceneChecker = new SceneChecker();
        var cSharpChecker = new CSharpChecker();
        var nonExistsResources = new HashSet<string>();

        foreach (string fullPath in allFilesPath.Select(filePath => Path.Combine(rootPath, filePath)))
        {
            if (fullPath.EndsWith(".tscn"))
            {
                sceneChecker.Check(fullPath, allFilesPath, nonExistsResources);
                continue;
            }

            if (!fullPath.EndsWith(".cs"))
                continue;
            cSharpChecker.Check(fullPath, allFilesPath, nonExistsResources);
        }

        return nonExistsResources;
    }

    private static string[] ReadIgnoreFile(string directory)
    {
        try
        {
            return ParseIgnoreFileLines(File.ReadAllLines(Path.Combine(directory, ".resCheckerIgnore")));
        }
        catch (Exception)
        {
            return Array.Empty<string>();
        }
    }

    public static string[] ParseIgnoreFileLines(string[] fileLines) =>
        fileLines
            .Select(it => it.Contains('#') ? it.Split('#')[0] : it) // ignore strings after #
            .Select(it => it.Trim(' ', '\t')) // trim empty characters
            .Where(it => it != "") // ignore empty string
            .ToArray();

    private static int Main(string[] args)
    {
        Console.WriteLine("Start checking resources existence...");

        string directory = args[0];
        var info = new DirectoryInfo(directory);
        int infoFullNameLength = info.FullName.Length;
        string[] ignoredGlob = ReadIgnoreFile(info.FullName);

        Matcher matcher = new();
        matcher.AddInclude("**/*");
        matcher.AddExcludePatterns(ignoredGlob);
        var allFilesPath = matcher.GetResultsInFullPath(info.FullName)
            .Select(it => it[(infoFullNameLength + 1)..].Replace('\\', '/'))
            .ToList();

        HashSet<string> nonExistsResources = CheckResourceExists(info.FullName, allFilesPath);

        if (nonExistsResources.Count == 0)
        {
            Console.WriteLine("All resource exists.");
            return 0;
        }

        foreach (string path in nonExistsResources)
        {
            Console.WriteLine("Resource not exists: " + path);
        }

        return 1;
    }
}
