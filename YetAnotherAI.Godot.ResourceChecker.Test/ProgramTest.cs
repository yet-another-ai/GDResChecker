using Shouldly;

using Xunit;

using YetAnotherAI.Godot.ResourceChecker.Console;

namespace YetAnotherAI.Godot.ResourceChecker.Test;

public class ProgramTest
{
    [Fact]
    public void TestParseIgnoreFileLines()
    {
        string[] expected = {
            "/path/first",
            "/path/second"
        };

        string[] forParse =
        {
            "/path/first",
            "/path/second     # second",
            "# /path/third"
        };

        string[] actual = Program.ParseIgnoreFileLines(forParse);

        actual.ShouldBe(expected);
    }
}
