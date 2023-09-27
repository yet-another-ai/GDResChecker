using Shouldly;

using Xunit;

using YetAnotherAI.Godot.ResourceChecker.Console;

namespace YetAnotherAI.Godot.ResourceChecker.Test;

public class CSharpCheckerTest
{
    [Fact]
    public void TestCheckSuccess()
    {
        var allFilePath = new List<string>{
            "path/to/exists",
        };
        var nonExistsPath = new HashSet<string>();

        var checker = new CSharpChecker();
        checker.Check(allFilePath, nonExistsPath, $"string path=\"res://path/to/exists\";");

        nonExistsPath.ShouldBeEmpty();
    }

    [Fact]
    public void TestCheckFailure()
    {
        var allFilePath = new List<string>{
            "path/to/exists",
        };
        var nonExistsPath = new HashSet<string>();

        var checker = new CSharpChecker();
        checker.Check(allFilePath, nonExistsPath, $"string path=\"res://path/to/not/exists\";");

        nonExistsPath.Count.ShouldBe(1);
        nonExistsPath.ShouldContain("path/to/not/exists");
    }

    [Fact]
    public void TestCheckMultipleNonExistsButTheSame()
    {
        var allFilePath = new List<string>{
            "path/to/exists",
        };
        var nonExistsPath = new HashSet<string>();

        var checker = new CSharpChecker();
        checker.Check(allFilePath, nonExistsPath, $"string path=\"res://path/to/not/exists\";string path2=\"res://path/to/not/exists\";");

        nonExistsPath.Count.ShouldBe(1);
        nonExistsPath.ShouldContain("path/to/not/exists");
    }
}
