using System.Xml.Linq;

namespace YetAnotherAI.Godot.ResourceChecker.Console;

public class SceneChecker : IResourceChecker
{
    public void Check(string filePath, List<string> allFilesPath, HashSet<string> nonExistsResources)
        => this.Check(File.ReadAllLines(filePath), allFilesPath, nonExistsResources);

    private static string GetResourceAttribute(string line)
    {
        string tinyXml = line.Replace("[", "<").Replace("]", "/>");
        return XElement
            .Parse(tinyXml)
            .Attributes()
            .First(it => it.Name == "path")
            .Value
            .Replace("res://", "");
    }

    public void Check(IEnumerable<string> fileLines, List<string> allFilesPath, HashSet<string> nonExistsResources)
    {
        IEnumerable<string> lines = fileLines.Where(it => it.StartsWith("[ext_resource") && it.EndsWith("]"));
        foreach (string? line in lines)
        {
            string resPath = GetResourceAttribute(line);
            if (!allFilesPath.Contains(resPath))
                nonExistsResources.Add(resPath);
        }
    }

    public void Check(List<string> allFilesPath, HashSet<string> nonExistsResources, string fileContent)
        => throw new NotSupportedException();
}
