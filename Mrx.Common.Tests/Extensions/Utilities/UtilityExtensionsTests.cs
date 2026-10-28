using Mrx.Common.Extensions;
using Mrx.Common.Helpers;
using Mrx.Common.ResultModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Mrx.Common.Tests;

public class UtilityExtensionsTests
{
    [Fact]
    public void ConversionExtensions_ConvertAndTryConvertValues()
    {
        Assert.Equal(42, ((object)"42").ToInt32());
        Assert.Equal(12.5m, ((object)"12.5").ToDecimal());
        Assert.Null(((object)"not-a-number").ToInt32Nullable());
        Assert.Equal("42", ((object)42).ConvertToString());
        Assert.Null(((object)null!).ConvertToString());
        Assert.Equal('a', ((object)'a').ToChar());
        Assert.True(((object)true).ToBoolean());
        Assert.Equal((byte)2, ((object)2).ToByte());
        Assert.Equal((short)3, ((object)3).ToInt16());
        Assert.Equal(4L, ((object)4).ToInt64());
        Assert.Equal(5f, ((object)5).ToSingle());
        Assert.Equal(6d, ((object)6).ToDouble());
        Assert.Equal(new DateTime(2026, 1, 1), ((object)"2026-01-01").ToDateTime());
        Assert.Null(((object)"invalid").ToDateTimeNullable());
        Assert.Equal(TimeSpan.FromSeconds(1), ((object)"00:00:01").ToTimeSpan());
        Assert.Equal("{\"Value\":7}", new Payload { Value = 7 }.SerializeJson());

        Assert.Equal((char)'b', ((object)"b").ToCharNullable());
        Assert.Equal(true, ((object)"true").ToBooleanNullable());
        Assert.Equal((byte)8, ((object)"8").ToByteNullable());
        Assert.Equal((short)9, ((object)"9").ToInt16Nullable());
        Assert.Equal(10, ((object)"10").ToInt32Nullable());
        Assert.Equal(11L, ((object)"11").ToInt64Nullable());
        Assert.Equal(12f, ((object)"12").ToSingleNullable());
        Assert.Equal(13d, ((object)"13").ToDoubleNullable());
        Assert.Equal(14m, ((object)"14").ToDecimalNullable());
        Assert.Equal(new DateTime(2026, 1, 1), ((object)"2026-01-01").ToDateTimeNullable());
        Assert.Equal(TimeSpan.FromSeconds(2), ((object)"00:00:02").ToTimeSpanNullable());

    }

    [Fact]
    public void ConversionExtensions_RespectCurrentCultureAndHandleNumericDateBoundaries()
    {
        var previousCulture = CultureInfo.CurrentCulture;
        var previousUiCulture = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Assert.Equal(12.5m, ((object)"12.5").ToDecimal());
            Assert.Equal(new DateTime(2025, 1, 2), ((object)"01/02/2025").ToDateTime());

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("de-DE");
            Assert.Equal(12.5m, ((object)"12,5").ToDecimal());
            Assert.Equal(new DateTime(2025, 12, 31), ((object)"31.12.2025").ToDateTime());
            Assert.Equal(new DateTime(2025, 2, 1), ((object)"01/02/2025").ToDateTime());

            Assert.Equal(int.MaxValue, ((object)int.MaxValue.ToString(CultureInfo.InvariantCulture)).ToInt32Nullable());
            Assert.Null(((object)"2147483648").ToInt32Nullable());
            Assert.Null(((object)"not-a-number").ToDecimalNullable());
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
            CultureInfo.CurrentUICulture = previousUiCulture;
        }
    }

    [Fact]
    public void JsonExtensions_RoundTripAndReturnDefaultForInvalidJson()
    {
        var json = new Payload { Value = 7 }.ToJson();
        Assert.Equal("7", System.Text.Json.JsonDocument.Parse(json).RootElement.GetProperty("Value").ToString());
        Assert.Equal(7, "{\"Value\":7}".DeserializeJson<Payload>()!.Value);
        Assert.Null("{".DeserializeJson<Payload>());
    }

    [Fact]
    public void EnumExtensions_EnumerateAndParseValues()
    {
        Assert.Equal(new[] { SampleEnum.First, SampleEnum.Second }, EnumExtentions.GetValues<SampleEnum>());
        Assert.Equal(SampleEnum.Second, "Second".ToEnum<SampleEnum>());
        Assert.Null("invalid".ToEnum<SampleEnum>());
        Assert.Equal(new object[] { SampleEnum.First, SampleEnum.Second }, EnumExtentions.GetEnumValues(typeof(SampleEnum)));
        Assert.Equal(new[] { SampleEnum.First, SampleEnum.Second }, EnumHelper.GetValues<SampleEnum>());
        Assert.Equal(new[] { SampleEnum.First, SampleEnum.Second }, typeof(SampleEnum).GetEnumValues().Cast<SampleEnum>());
    }

    [Fact]
    public void AttributeExtensions_ReadDisplayDescriptionAndData()
    {
        var member = typeof(SampleEnum).GetMember(nameof(SampleEnum.First)).Single();
        Assert.True(member.HasAttribute<DescriptionAttribute>());
        Assert.Equal("First label", SampleEnum.First.GetDisplayName());
        Assert.Equal("first-description", SampleEnum.First.GetDescription());
        Assert.Equal("first-data", SampleEnum.First.GetDataValue());
        Assert.Equal("First label", member.GetDisplayName());
        Assert.True(SampleEnum.First.TryGetAttribute<DescriptionAttribute>(out _));
        Assert.Equal("Second label", SampleEnum.Second.GetDisplayName());
        Assert.Equal("Second label", SampleEnum.Second.GetEnumDisplayName());
        Assert.Null(SampleEnum.Second.GetDescription());
        Assert.Equal("first-data", SampleEnum.First.GetDataValue<string>());
        Assert.False(SampleEnum.Second.TryGetAttribute<DescriptionAttribute>(out _));
        Assert.Null(((SampleEnum)999).GetAttribute<DescriptionAttribute>());
        Assert.Null(((SampleEnum)999).GetDisplayName());
        Assert.False(typeof(Outer).HasAttribute<DescriptionAttribute>());
        Assert.Empty(typeof(Outer).GetAttributes<DescriptionAttribute>());
        Assert.Null(typeof(Outer).GetAttribute<DescriptionAttribute>());
        Assert.Null(typeof(Outer).GetDisplayName());
        Assert.Equal("Annotated model", typeof(AnnotatedModel).GetDisplayName());
        Assert.Equal("Annotated model", typeof(AnnotatedModel).GetAttribute<DisplayAttribute>()!.Name);
        Assert.Single(member.GetAttributes<DisplayAttribute>());
        Assert.Null(typeof(Outer).GetProperty(nameof(Outer.Inner))!.GetAttribute<DescriptionAttribute>());
    }

    [Fact]
    public void ReflectionExtensions_InspectTypesAndNestedProperties()
    {
        Assert.Equal("Grace", new Outer { Inner = new Inner { Name = "Grace" } }.GetPropertyValue("Inner.Name"));
        Assert.Null(new Outer().GetPropertyValue("Inner.Name"));
        Assert.True(typeof(SampleEnum?).IsEnum());
        Assert.True(typeof(int?).IsNullableType<int>());
        Assert.Equal(typeof(int), typeof(int?).GetTypeNullableType());
        Assert.True(typeof(decimal?).IsNumber());
        Assert.False(typeof(string).IsNumber());
        Assert.Equal("System.Collections.Generic.List<System.Int32>", typeof(List<int>).GetFullName());
        Assert.Equal("System.String", typeof(string).GetFullName());
        Assert.True(typeof(int?).IsNullableType(typeof(int)));
        Assert.False(typeof(int).IsNullableType());
        Assert.True(typeof(int?).AnyNullableTypes(typeof(string), typeof(int)));
        Assert.True(typeof(int?).AllNullableTypes(typeof(int), typeof(int)));
        Assert.Equal("Grace", typeof(Outer).GetPropertyValue(new Outer { Inner = new Inner { Name = "Grace" } }, "Inner" ) is Inner inner ? inner.Name : null);
    }

    [Fact]
    public void ObjectAndDictionaryExtensions_ConvertExpectedValues()
    {
        object value = "value";
        Assert.Equal("value", value.CastTo<string>());
        Assert.Equal("value", value.CastToReflected(typeof(string)));
        Assert.Equal(new Dictionary<string, string> { ["A"] = "x" }, new { A = "x" }.ToDictionary());
        Assert.Equal(new Dictionary<string, object> { ["A"] = "x" }, new Dictionary<string, string> { ["A"] = "x" }.ToDictionary());
        Assert.Equal(new Dictionary<string, string> { ["a"] = "first", ["b"] = "second" }, DictionaryHelper.Merge(
            new() { ["a"] = "first" }, new() { ["a"] = "ignored", ["b"] = "second" }));
    }

    [Fact]
    public void CommonExtensions_FormatPercentAndSizes()
    {
        Assert.Equal(12.5m, 50m.Percent(25m));
        Assert.Equal(25d, 100d.Percent(25d));
        Assert.Equal(25L, 100L.Percent(25L));
        Assert.Equal("1.00 KB", 1024L.SizeSuffix());
        Assert.Contains("KB", 1024L.SizeSuffix());
        Assert.Contains("کیلوبایت", 1024L.SizeSuffixPersian());
        Assert.Contains("-", (-1024L).SizeSuffix());
    }

    [Fact]
    public void HelperValidatesIranianNationalCodeAndPhone()
    {
        Assert.Equal("Hello Ada", "Hello {Name}".InsertsValues(new[] { new KeyValuePair<string, string>("Name", "Ada") }));
        Assert.True("۱۲۳۴۵۶۷۸۱۴".IsValidNationalCode());
        Assert.False("1111111111".IsValidNationalCode());
        Assert.True("09123456789".IsValidPhoneNumber());
        Assert.True("۰۹۱۲۳۴۵۶۷۸۹".IsValidPhoneNumber());
        Assert.False("091234".IsValidPhoneNumber());
        Assert.True("سلام دنیا".IsValidPersian());
        Assert.False("hello".IsValidPersian());
        Assert.False("".IsValidPersian());
        Assert.False("123456789".IsValidNationalCode());
        Assert.False("abcdefghij".IsValidNationalCode());
        Assert.False("0000000000".IsValidNationalCode());
        Assert.False("1234567815".IsValidNationalCode());
        Assert.False(((string?)null)!.IsValidNationalCode(out var emptyCodeError));
        Assert.Equal("کد ملی را بدرستی وارد نمایید.", emptyCodeError);
        Assert.False("05912345678".IsValidPhoneNumber());
        Assert.False("۱۲۳".IsValidPhoneNumber());
        Assert.False("123".IsValidPersian());

        Assert.True("1234567814".IsValidNationalCode(out var validError));
        Assert.Null(validError);
        Assert.False("123".IsValidNationalCode(out var lengthError));
        Assert.Equal("طول کد ملی باید ده کاراکتر باشد.", lengthError);
    }

    [Fact]
    public void DateTimeExtensions_CheckRangesAndRoundUp()
    {
        var date = new DateTime(2026, 1, 2, 10, 15, 0);
        Assert.Equal(new DateTime(2026, 1, 2, 11, 0, 0), date.GetDateTimeHourRoundUp());
        Assert.Equal(new DateTime(2026, 1, 2, 0, 0, 0), date.Date.GetDateTimeHourRoundUp());
        Assert.True(date.IsBetween(date.AddMinutes(-1), date.AddMinutes(1)));
        Assert.False(((DateTime?)null).IsBetween(date, date.AddDays(1)));
        Assert.Equal(TimeSpan.FromMinutes(3), new[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2) }.Sum(_ => TimeSpan.FromMinutes(1.5)));
        Assert.Equal("fallback", ((DateTime?)null).ToShortDateString("fallback"));
        Assert.Equal(date.ToShortDateString(), ((DateTime?)date).ToShortDateString());
        Assert.Equal(date.ToLongDateString(), ((DateTime?)date).ToLongDateString());
        Assert.Equal(date.ToShortTimeString(), ((DateTime?)date).ToShortTimeString());
        Assert.Equal(date.ToLongTimeString(), ((DateTime?)date).ToLongTimeString());
        Assert.True(new TimeSpan(10, 0, 0).IsBetween(new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0)));
        Assert.True(date.IsBetween(new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0)));
        Assert.False(((TimeSpan?)null).IsBetween(TimeSpan.Zero, TimeSpan.FromHours(1)));
        Assert.Equal("fallback", ((DateTime?)null).ToLongTimeString("fallback"));
        Assert.True(((DateTime?)date).IsBetween(new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0)));
        Assert.False(((DateTime?)null).IsBetween(TimeSpan.Zero, TimeSpan.FromHours(1)));
        Assert.Equal("اخیراً", DateTime.Now.GetTimeAgo());
        Assert.Equal("1 دقیقه پیش", DateTime.Now.AddMinutes(-1.5).GetTimeAgo());
        Assert.Equal("20 دقیقه پیش", DateTime.Now.AddMinutes(-20).GetTimeAgo());
        Assert.Equal("1 ساعت پیش", DateTime.Now.AddMinutes(-60).GetTimeAgo());
        Assert.Equal("10 ساعت پیش", DateTime.Now.AddHours(-10).GetTimeAgo());
        Assert.Equal("دیروز", DateTime.Now.AddHours(-36).GetTimeAgo());
        Assert.Equal("5 روز پیش", DateTime.Now.AddDays(-5).GetTimeAgo());
        Assert.Equal("3 ماه پیش", DateTime.Now.AddDays(-100).GetTimeAgo());
        Assert.Equal("1 سال پیش", DateTime.Now.AddDays(-400).GetTimeAgo());
        Assert.Equal("2 سال", DateTime.Now.AddDays(-800).GetTimeAgo());
        Assert.Equal(TimeSpan.Zero, Array.Empty<int>().Sum(_ => TimeSpan.FromSeconds(1)));
        Assert.False(date.AddDays(-1).IsBetween(date, date.AddDays(1)));
        Assert.True(((DateTime?)date).IsBetween(date, date));
        Assert.True(((TimeSpan?)TimeSpan.FromMinutes(30)).IsBetween(TimeSpan.Zero, TimeSpan.FromHours(1)));
        Assert.False(TimeSpan.FromHours(2).IsBetween(TimeSpan.Zero, TimeSpan.FromHours(1)));
        Assert.False(date.IsBetween(TimeSpan.Zero, TimeSpan.FromHours(1)));
        Assert.Equal(new DateTime(2026, 1, 2, 11, 0, 0), new DateTime(2026, 1, 2, 10, 0, 1).GetDateTimeHourRoundUp());
    }

    [Fact]
    public void AttributeAndTableHelpers_RespectMetadataAndIgnoredProperties()
    {
        var property = typeof(AnnotatedModel).GetProperty(nameof(AnnotatedModel.Visible))!;
        Assert.Equal("Visible label", property.GetDisplayName());
        Assert.Equal("icon-home", property.GetAttribute<Mrx.Common.Attributes.IconAttribute>()!.Icon);

        var output = TablePrinter.GetDataInTableFormat(new List<AnnotatedModel>
        {
            new() { Visible = "shown", Hidden = "secret" }
        });
        Assert.Contains("Visible", output);
        Assert.DoesNotContain("Hidden", output);
        Assert.DoesNotContain("secret", output);
        Assert.Equal(string.Empty, TablePrinter.GetDataInTableFormat(new List<string[]>()));
        Assert.Equal(string.Empty, TablePrinter.GetDataInTableFormat((List<string[]>?)null!));
        Assert.Equal(string.Empty, TablePrinter.GetDataInTableFormat(new List<string[]> { null! }));
        Assert.Equal(string.Empty, TablePrinter.GetDataInTableFormat<PrintModel>(null!));
        Assert.Equal(string.Empty, TablePrinter.GetDataInTableFormat(new List<PrintModel>()));
        Assert.Contains("---", TablePrinter.GetDataInTableFormat(new List<PrintModel> { null! }));
        Assert.Contains("---", TablePrinter.GetDataInTableFormat(new List<string[]> { new[] { "A" }, new string[] { null! } }));
        Assert.Contains("two", TablePrinter.GetDataInTableFormat(new List<string[]> { new[] { "A", "B" }, new[] { "one", "two" }, new[] { "three", "four" } }));
        Assert.NotEmpty(TablePrinter.GetDataInTableFormat(new List<string[]> { new[] { "A" }, new[] { "one", "two", "three" } }));
        Assert.NotEmpty(TablePrinter.GetDataInTableFormat(new List<string[]> { new[] { "A", "B", "C" }, new[] { "one" } }));
        Assert.NotEmpty(TablePrinter.GetDataInTableFormat(new List<string[]> { new[] { "A", "B", "C" }, new[] { "one", "two", "three", "four" } }));
        Assert.NotEmpty(TablePrinter.GetDataInTableFormat(new List<string[]> { new[] { "A", "B", "C", "D" }, new[] { "one", "two", "three" } }));
        Assert.NotEmpty(TablePrinter.GetDataInTableFormat(new List<string[]> { new[] { "A" }, new[] { "one", "two", "three", "four" } }));
        Assert.NotEmpty(TablePrinter.GetDataInTableFormat(new List<string[]> { new[] { "A", "B", "C", "D" }, new[] { "one" } }));

        var previousOutput = Console.Out;
        using var outputWriter = new StringWriter();
        try
        {
            Console.SetOut(outputWriter);
            TablePrinter.Print(new PrintModel { Value = "single" });
            TablePrinter.Print(new List<PrintModel> { new() { Value = "first" }, new() { Value = "second" } });
        }
        finally
        {
            Console.SetOut(previousOutput);
        }
        Assert.Contains("single", outputWriter.ToString());
        Assert.Contains("second", outputWriter.ToString());
    }

    [Fact]
    public void ApiResultFactoriesSetSuccessResultAndErrors()
    {
        var failed = ApiResult.Failed("bad request");
        Assert.False(failed.Success);
        Assert.Equal("bad request", failed.Error);
        var succeeded = ApiResult.Successed(new Payload { Value = 9 });
        Assert.True(succeeded.Success);
        Assert.Equal(9, succeeded.Result!.Value);
        Assert.True((bool)ApiResult.Successed());

        var okWithErrors = ApiResult.Ok(new Payload { Value = 2 }, new() { new("field", "warning") });
        Assert.True(okWithErrors.Success);
        Assert.Equal("warning", okWithErrors.Error);
        Assert.False(ApiResult.Ok<Payload>(false, null).Success);
        Assert.Equal(4, ApiResult.Failed<Payload>("failure", new Payload { Value = 4 }).Result!.Value);
        Assert.Equal("failure", ApiResult.Failed<Payload>("failure", null).Error);
        Assert.Null(ApiResult.Failed().Errors);
        Assert.Empty(ApiResult.Failed(new List<KeyValuePair<string?, string?>>()).Errors!);
        Assert.Equal("explicit", ApiResult.Failed(new List<KeyValuePair<string?, string?>> { new("field", "explicit") }).Error);
        Assert.Equal("field error", ApiResult.Failed<Payload>(new List<KeyValuePair<string?, string?>> { new("field", "field error") }, null).Error);
        Assert.True(ApiResult.Ok(true).Success);
        Assert.Equal(1, ApiResult.SuccessedObject(new Payload { Value = 1 }).Result is Payload payload ? payload.Value : 0);
        Assert.Equal(2, (ApiResult.OkObject(new Payload { Value = 2 })!.Result is Payload okPayload) ? okPayload.Value : 0);
        Assert.True(ApiResult.Ok(true, new List<KeyValuePair<string?, string?>>()).Success);
        Assert.Null(ApiResult.OkObject(new Payload { Value = 3 })!.Errors);
        Assert.False(ApiResult.OkObject(null!)!.Success);
        Assert.Null(ApiResult.OkObject(null!)!.Result);

        var noMessageFailure = ApiResult.Failed((string?)null);
        Assert.False(noMessageFailure.Success);
        Assert.Null(noMessageFailure.Error);
        Assert.Single(noMessageFailure.Errors!);

        var errorState = ApiResult.Failed("first");
        errorState.Error = string.Empty;
        errorState.Error = null;
        errorState.Error = "second";
        Assert.Equal(2, errorState.Errors!.Count);
        Assert.Equal("first", errorState.Error);

        BaseApiResult<Payload> baseResult = ApiResult.Successed(new Payload { Value = 6 });
        bool success = baseResult;
        Payload? result = baseResult;
        Assert.True(success);
        Assert.Equal(6, result!.Value);
        baseResult.Error = "another error";
        Assert.Equal("another error", baseResult.Error);
    }

    private sealed class Payload { public int Value { get; set; } }
    private sealed class PrintModel { public string Value { get; set; } = string.Empty; }
    private sealed class Outer { public Inner? Inner { get; set; } }
    private sealed class Inner { public string? Name { get; set; } }

    [Display(Name = "Annotated model")]
    private sealed class AnnotatedModel
    {
        [Display(Name = "Visible label")]
        [Mrx.Common.Attributes.Icon("icon-home")]
        public string? Visible { get; set; }

        [Mrx.Common.Attributes.Ignore]
        public string? Hidden { get; set; }
    }

    private enum SampleEnum
    {
        [Display(Name = "First label")]
        [Description("first-description")]
        [Mrx.Common.Attributes.Data("first-data")]
        First,
        [Display(Name = "Second label")]
        Second
    }
}


