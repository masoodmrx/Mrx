using Mrx.Common.Extensions;

namespace Mrx.Common.Tests;

public class ModelMappingExtensionsTests
{
    [Fact]
    public void MapToModel_ConvertsScalarEnumDateAndJsonValues_AndIgnoresUnknownKeys()
    {
        var values = new Dictionary<string, string?>
        {
            ["nAmE"] = "Ada",
            ["Age"] = "37",
            ["Enabled"] = "true",
            ["State"] = "Ready",
            ["CreatedOn"] = "2026-10-28",
            ["Identifier"] = "b3f9dc50-0c8d-4f50-a7e0-90481ed00001",
            ["Payload"] = "{\"Count\":4}",
            ["Unknown"] = "ignored"
        };

        var model = values.MapToModel<SampleModel>();

        Assert.Equal("Ada", model.Name);
        Assert.Equal(37, model.Age);
        Assert.True(model.Enabled);
        Assert.Equal(SampleState.Ready, model.State);
        Assert.Equal(new DateOnly(2026, 10, 28), model.CreatedOn);
        Assert.Equal(Guid.Parse("b3f9dc50-0c8d-4f50-a7e0-90481ed00001"), model.Identifier);
        Assert.Equal(4, model.Payload!.Count);
    }

    [Fact]
    public void MapToModel_LastMatchingKeyWins_AndCanPopulateAnExistingInstance()
    {
        var model = new SampleModel { Name = "Existing", Age = 8 };
        var values = new List<KeyValuePair<string, string?>>
        {
            new("Age", "10"),
            new("age", "11"),
            new("Name", "Updated")
        };

        var result = model.MapToModel(values);

        Assert.Same(model, result);
        Assert.Equal("Updated", result.Name);
        Assert.Equal(11, result.Age);
        Assert.False(result.Enabled);
    }

    [Fact]
    public void MapToModel_EmptyNullableAndNullValuesAreHandled_WhileStringsRemainEmpty()
    {
        var model = new SampleModel { Name = "before", OptionalCount = 12 };
        model.MapToModel(new Dictionary<string, string?>
        {
            ["Name"] = string.Empty,
            ["OptionalCount"] = " "
        });

        Assert.Equal(string.Empty, model.Name);
        Assert.Null(model.OptionalCount);
        Assert.Throws<FormatException>(() => new Dictionary<string, string?> { ["Age"] = null }.MapToModel<SampleModel>());
    }

    [Theory]
    [InlineData("not-an-int", "Age")]
    [InlineData("UnknownState", "State")]
    [InlineData("not-a-guid", "Identifier")]
    [InlineData("not-json", "Payload")]
    public void MapToModel_InvalidValuesThrowAnErrorNamingTheProperty(string value, string propertyName)
    {
        var exception = Assert.Throws<FormatException>(() =>
            new Dictionary<string, string?> { [propertyName] = value }.MapToModel<SampleModel>());

        Assert.Contains($"'{propertyName}'", exception.Message);
    }

    [Fact]
    public void MapToModel_RejectsNullArguments()
    {
        Assert.Throws<ArgumentNullException>(() => ModelMappingExtensions.MapToModel<SampleModel>(null!));
        Assert.Throws<ArgumentNullException>(() => ModelMappingExtensions.MapToModel<SampleModel>(null!, Array.Empty<KeyValuePair<string, string?>>()));
        Assert.Throws<ArgumentNullException>(() => new SampleModel().MapToModel(null!));
    }

    public sealed class SampleModel
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public int? OptionalCount { get; set; }
        public bool Enabled { get; set; }
        public SampleState State { get; set; }
        public DateOnly CreatedOn { get; set; }
        public Guid Identifier { get; set; }
        public SamplePayload? Payload { get; set; }
    }

    public enum SampleState { Ready }
    public sealed class SamplePayload { public int Count { get; set; } }
}
