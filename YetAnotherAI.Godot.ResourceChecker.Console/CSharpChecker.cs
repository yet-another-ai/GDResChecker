using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace YetAnotherAI.Godot.ResourceChecker.Console;

public class CSharpChecker : IResourceChecker
{
    public void Check(string filePath, List<string> allFilesPath, HashSet<string> nonExistsResources)
    {
        string fileContent = File.ReadAllText(filePath);
        this.Check(allFilesPath, nonExistsResources, fileContent);
    }

    private static void Travel(SyntaxNode node, List<string> allFilesPath, HashSet<string> nonExistsResources)
    {
        if (node.IsKind(SyntaxKind.StringLiteralExpression))
        {
            string nodeText = node.GetText().ToString();
            if (!nodeText.StartsWith("\"res://"))
            {
                return;
            }

            nodeText = nodeText[7..^1];

            if (!allFilesPath.Contains(nodeText))
            {
                nonExistsResources.Add(nodeText);
            }
            return;
        }

        IEnumerable<SyntaxNode> children = node.ChildNodes();
        foreach (SyntaxNode child in children)
        {
            Travel(child, allFilesPath, nonExistsResources);
        }
    }

    public void Check(IEnumerable<string> fileLines, List<string> allFilesPath, HashSet<string> nonExistsResources)
        => throw new NotSupportedException();

    public void Check(List<string> allFilesPath, HashSet<string> nonExistsResources, string fileContent)
    {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(fileContent);
        SyntaxNode root = tree.GetRoot();
        Travel(root, allFilesPath, nonExistsResources);
    }
}
