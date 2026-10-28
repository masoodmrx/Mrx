namespace Mrx.Common.Extensions
{
    public static class StringExtentions
    {
        public static string? HtmlToPlainText(this string? html)
        {
            if (html.IsNullOrEmpty()) return null;
            string buf;
            string block = "address|article|aside|blockquote|canvas|dd|div|dl|dt|" +
              "fieldset|figcaption|figure|footer|form|h\\d|header|hr|li|main|nav|" +
              "noscript|ol|output|p|pre|section|table|tfoot|ul|video";

            string patNestedBlock = $"(\\s*?</?({block})[^>]*?>)+\\s*";
            buf = Regex.Replace(html, patNestedBlock, "\n", RegexOptions.IgnoreCase);

            // Replace br tag to newline.
            buf = Regex.Replace(buf, @"<(br)[^>]*>", "\n", RegexOptions.IgnoreCase);

            // (Optional) remove styles and scripts.
            buf = Regex.Replace(buf, @"<(script|style|w:WordDocument)[^>]*?>.*?</\1>", "", RegexOptions.Singleline);

            // Remove all tags.
            buf = Regex.Replace(buf, @"<[^>]*(>|$)", "", RegexOptions.Multiline);

            // Replace HTML entities.
            buf = WebUtility.HtmlDecode(buf);
            return buf;
        }
        public static string? PersianToEnglish(this string? persianString)
        {
            if (persianString.IsNullOrEmpty()) return persianString;
            Dictionary<string, string> LettersDictionary = new Dictionary<string, string>
            {
                //۰۱۲۳۴۵۶۷۸۹
                //Persian
                ["۰"] = "0",
                ["۱"] = "1",
                ["۲"] = "2",
                ["۳"] = "3",
                ["۴"] = "4",
                ["۵"] = "5",
                ["۶"] = "6",
                ["۷"] = "7",
                ["۸"] = "8",
                ["۹"] = "9",
                //٠١٢٣٤٥٦٧٨٩
                //Arabic
                ["٠"] = "0",
                ["١"] = "1",
                ["٢"] = "2",
                ["٣"] = "3",
                ["٤"] = "4",
                ["٥"] = "5",
                ["٦"] = "6",
                ["٧"] = "7",
                ["٨"] = "8",
                ["٩"] = "9",
            };
            return LettersDictionary.Aggregate(persianString, (current, item) =>
                         current.Replace(item.Key, item.Value)).Trim();
        }
        //public static string PersianToEnglish(this string persianString)
        //{
        //    if (persianString.IsNullOrEmpty()) return persianString;
        //    Dictionary<string, string> LettersDictionary = new Dictionary<string, string> { ["۰"] = "0", ["۱"] = "1", ["۲"] = "2", ["۳"] = "3", ["۴"] = "4", ["۵"] = "5", ["۶"] = "6", ["۷"] = "7", ["۸"] = "8", ["۹"] = "9" };
        //    return LettersDictionary.Aggregate(persianString, (current, item) =>
        //                 current.Replace(item.Key, item.Value)).Trim();
        //}
        public static string EnglishToPersian(this string englishString)
        {
            if (englishString.IsNullOrEmpty()) return englishString;
            Dictionary<string, string> LettersDictionary = new Dictionary<string, string> { ["0"] = "۰", ["1"] = "۱", ["2"] = "۲", ["3"] = "۳", ["4"] = "۴", ["5"] = "۵", ["6"] = "۶", ["7"] = "۷", ["8"] = "۸", ["9"] = "۹" };
            return LettersDictionary.Aggregate(englishString, (current, item) =>
                         current.Replace(item.Key, item.Value));
        }
        public static string InsertParams(this string text, params (string Key, string Value)[] parameters)
        {
            if (text.IsNullOrEmpty()) return text;
            foreach (var parameter in parameters)
            {
                text = text.Replace($"{{{parameter.Key}}}", parameter.Value);
                text = text.Replace($"{{{parameter.Key.ToLower()}}}", parameter.Value);
            }
            return text;
        }
        public static string InsertParams(this string text, object obj)
        {
            return text.InsertParams(obj
                .GetType()
                .GetProperties()
                .Where(p => !p.HasAttribute<IgnoreAttribute>())
                .Select(p => (p.Name, p.GetValue(obj)?.ToString()))
                .ToArray());
        }
        public static bool IsNullOrEmpty(this string? value) => string.IsNullOrEmpty(value);
        public static bool IsNotNullOrEmpty(this string? value) => !value.IsNullOrEmpty();
        public static string StringJoin(this IEnumerable<object> values, string separator) => string.Join(separator, values);
        public static string StringJoin(this object[] values, string separator) => string.Join(separator, values);
        public static string ReplaceSpaceWithHyphen(this string value) => value.IsNullOrEmpty() ? value : value.Replace(' ', '-');
        public static string ReplaceNewLineWithBr(this string value) => value.IsNullOrEmpty() ? value : value.Replace("\r\n", "<br>");
        public static string ReplaceBrWithNewLine(this string value) => value.IsNullOrEmpty() ? value : value.Replace("\r\n", "<br>");
        public static string ExtractNumber(this string value) => value.IsNullOrEmpty() ? value : Regex.Match(value, @"\d+").Value;
        public static bool IsNumber(this string value) => value.IsNullOrEmpty() ? false : value.Any(char.IsDigit);
        public static string Truncate(this string value, int length)
            => (value.IsNotNullOrEmpty() && value.Length > length) ? value.Substring(0, length) : value;
    }
}
