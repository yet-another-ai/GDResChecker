using Shouldly;

using Xunit;

using YetAnotherAI.Godot.ResourceChecker.Console;

namespace YetAnotherAI.Godot.ResourceChecker.Test;

public class SceneCheckerTest
{
    [Fact]
    public void TestCheckSuccess()
    {
        var allFilePath = new List<string>{
            "path/to/exists",
        };
        var nonExistsPath = new HashSet<string>();

        var checker = new SceneChecker();
        checker.Check(new List<string>{
            "[ext_resource path=\"res://path/to/exists\"]",
            "[sub_resource type=\"CameraAttributesPractical\"]"
        }, allFilePath, nonExistsPath);

        nonExistsPath.ShouldBeEmpty();
    }

    [Fact]
    public void TestCheckFailure()
    {
        var allFilePath = new List<string>{
            "path/to/exists",
        };
        var nonExistsPath = new HashSet<string>();

        var checker = new SceneChecker();
        checker.Check(new List<string>{
            "[ext_resource path=\"res://path/to/not/exists\"]",
            "[sub_resource type=\"CameraAttributesPractical\"]"
        }, allFilePath, nonExistsPath);

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

        var checker = new SceneChecker();
        checker.Check(new List<string>{
            "[ext_resource path=\"res://path/to/not/exists\"]",
            "[ext_resource path=\"res://path/to/not/exists\"]",
            "[sub_resource type=\"CameraAttributesPractical\"]"
        }, allFilePath, nonExistsPath);

        nonExistsPath.Count.ShouldBe(1);
        nonExistsPath.ShouldContain("path/to/not/exists");
    }
}
