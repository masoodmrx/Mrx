using Mrx.Common.Helpers;

namespace Mrx.Common.Tests;

public class DictionaryHelperTests
{
    [Fact]
    public void Merge_ReturnsEmptyDictionaryWhenThereAreNoSources()
    {
        Assert.Empty(DictionaryHelper.Merge());
        Assert.Empty(DictionaryHelper.Merge((Dictionary<string, string>[])null!));
        Assert.Empty(DictionaryHelper.Merge(null!, null!));
    }

    [Fact]
    public void Merge_UsesTheFirstValueForDuplicateKeysAcrossSources()
    {
        var first = new Dictionary<string, string> { ["shared"] = "first", ["first-only"] = "one" };
        var second = new Dictionary<string, string> { ["shared"] = "second", ["second-only"] = "two" };
        var third = new Dictionary<string, string> { ["shared"] = "third" };

        var merged = DictionaryHelper.Merge(first, null!, second, third);

        Assert.Equal("first", merged["shared"]);
        Assert.Equal("one", merged["first-only"]);
        Assert.Equal("two", merged["second-only"]);
        Assert.Equal("first", first["shared"]);
        Assert.Equal("second", second["shared"]);
        Assert.NotSame(first, merged);
    }
}
