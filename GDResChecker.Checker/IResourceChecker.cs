namespace GDResChecker.Checker;

public interface IResourceChecker
{
    void Check(string filePath, List<string> allFilesPath, HashSet<string> nonExistsResources);
    void Check(IEnumerable<string> fileLines, List<string> allFilesPath, HashSet<string> nonExistsResources);
    void Check(List<string> allFilesPath, HashSet<string> nonExistsResources, string fileContent);
}
