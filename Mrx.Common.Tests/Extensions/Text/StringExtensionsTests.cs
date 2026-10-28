using Mrx.Common.Extensions;

namespace Mrx.Common.Tests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("۰۱۲۳۴۵۶۷۸۹", "0123456789")]
    [InlineData("٠١٢٣٤٥٦٧٨٩", "0123456789")]
    [InlineData("شماره ۱۲٣", "شماره 123")]
    public void ToEnglishDigits_ConvertsPersianAndArabicDigitsWithoutTrimming(string input, string expected)
        => Assert.Equal(expected, input.ToEnglishDigits());

    [Fact]
    public void DigitConversions_KeepNullAndConvertAsciiDigitsToPersian()
    {
        Assert.Null(StringExtentions.ToEnglishDigits(null));
        Assert.Null(StringExtentions.ToPersianDigits(null));
        Assert.Equal("۱۲۳ ABC", "123 ABC".ToPersianDigits());
    }

    [Fact]
    public void LegacyPersianToEnglish_TrimsWhitespace()
    {
        Assert.Equal("12", "  ۱۲  ".PersianToEnglish());
        Assert.Equal("۱۲", "12".EnglishToPersian());
    }

    [Fact]
    public void InsertParams_IsCaseInsensitiveAndNullSafe()
    {
        Assert.Equal("Hello World", "Hello {NAME}".InsertParams(("name", "World")));
        Assert.Equal("Hello ", "Hello {Name}".InsertParams(("Name", null)));
        Assert.Equal("{Name}", "{Name}".InsertParams(("", "ignored")));
        (string Key, string? Value)[]? noParameters = null;
        Assert.Null(StringExtentions.InsertParams(null, noParameters));
        Assert.Equal("{Name}", "{Name}".InsertParams((object?)null));
    }

    [Fact]
    public void InsertParams_ReplacesRepeatedTokensInOnePassAndKeepsUnknownOrMalformedTokens()
    {
        Assert.Equal("Ada/Ada/{Missing}/{broken", "{Name}/{name}/{Missing}/{broken".InsertParams(("name", "Ada")));
        Assert.Equal("first", "{Key}".InsertParams(("Key", "first"), ("key", "second")));
        Assert.Equal("{Value}", "{Key}".InsertParams(("Key", "{Value}"), ("Value", "expanded")));
        Assert.Equal("unchanged", "unchanged".InsertParams(("Key", "value")));
        Assert.Equal("{}", "{}".InsertParams(("", "value")));
    }

    [Fact]
    public void InsertParams_Object_UsesPublicPropertiesAndSkipsIgnoreAttribute()
    {
        var result = "{Name}:{Count}:{Secret}".InsertParams(new TemplateValues());
        Assert.Equal("Ada:3:{Secret}", result);
    }

    [Fact]
    public void HtmlToPlainText_RemovesTagsAndDecodesEntities()
    {
        Assert.Equal("\none\ntwo & three\n", "<p>one</p><p>two &amp; three</p>".HtmlToPlainText());
        Assert.Null(((string?)null).HtmlToPlainText());
    }

    [Fact]
    public void BasicStringHelpers_HandleExpectedValues()
    {
        Assert.True("".IsNullOrEmpty());
        Assert.True("x".IsNotNullOrEmpty());
        Assert.Equal("a-b", "a b".ReplaceSpaceWithHyphen());
        Assert.Equal("one<br>two", "one\r\ntwo".ReplaceNewLineWithBr());
        Assert.Equal($"one{Environment.NewLine}two{Environment.NewLine}three", "one<BR>two<br />three".ReplaceBrWithNewLine());
        Assert.Equal("one, two", new object[] { "one", "two" }.StringJoin(", "));
        Assert.Equal("one, two", ((IEnumerable<object>)new object[] { "one", "two" }).StringJoin(", "));
        Assert.Equal("abc", "abcdef".Truncate(3));
        Assert.Equal("abcdef", "abcdef".Truncate(10));
        Assert.Equal("123", "abc123def".ExtractNumber());
        Assert.True("abc2".IsNumber());
        Assert.False("abc".IsNumber());
        Assert.Equal("", "abc".ExtractNumber());
        Assert.Equal("", "".ReplaceNewLineWithBr());
    }

    private sealed class TemplateValues
    {
        public string Name => "Ada";
        public int Count => 3;
        [Mrx.Common.Attributes.Ignore]
        public string Secret => "hidden";
    }
}





