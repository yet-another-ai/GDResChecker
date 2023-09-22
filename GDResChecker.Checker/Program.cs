namespace GDResChecker.Checker;

internal class Program
{
    private readonly static HashSet<string> nonExistsResources = new();
    private readonly static List<string> allFilesPath = new();
    private readonly static CSharpChecker cSharpChecker = new();
    private readonly static SceneChecker sceneChecker = new();

    private static void GetAllFilePath(DirectoryInfo info, int rootPathLength)
    {
        DirectoryInfo[] directories = info.GetDirectories();
        foreach (DirectoryInfo dir in directories)
        {
            GetAllFilePath(dir, rootPathLength);
        }

        FileInfo[] files = info.GetFiles();
        foreach (FileInfo file in files)
        {
            allFilesPath.Add(file.FullName[rootPathLength..]);
        }
    }

    private static void CheckResourceExists(string rootPath)
    {
        allFilesPath.ForEach(it =>
        {
            string fullPath = Path.Combine(rootPath, it);
            if (fullPath.EndsWith(".tscn"))
            {
                sceneChecker.Check(fullPath, allFilesPath, nonExistsResources);
                return;
            }
            if (fullPath.EndsWith(".cs"))
            {
                cSharpChecker.Check(fullPath, allFilesPath, nonExistsResources);
                return;
            }
        });
    }

    private static int Main(string[] args)
    {
        Console.WriteLine("Start checking resources existence...");

        string directory = args[0];
        var info = new DirectoryInfo(directory);
        GetAllFilePath(info, info.FullName.Length + 1);
        CheckResourceExists(info.FullName);

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
