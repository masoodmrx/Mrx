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
        /// <summary>
        /// Converts Persian and Arabic-Indic digits to ASCII digits without changing other characters.
        /// </summary>
        public static string? ToEnglishDigits(this string? value)
        {
            if (value is null)
            {
                return null;
            }

            var characters = value.ToCharArray();
            for (var index = 0; index < characters.Length; index++)
            {
                if (characters[index] is >= '\u06F0' and <= '\u06F9')
                {
                    characters[index] = (char)('0' + characters[index] - '\u06F0');
                }
                else if (characters[index] is >= '\u0660' and <= '\u0669')
                {
                    characters[index] = (char)('0' + characters[index] - '\u0660');
                }
            }

            return new string(characters);
        }

        /// <summary>
        /// Converts ASCII digits to Persian digits without changing other characters.
        /// </summary>
        public static string? ToPersianDigits(this string? value)
        {
            if (value is null)
            {
                return null;
            }

            var characters = value.ToCharArray();
            for (var index = 0; index < characters.Length; index++)
            {
                if (characters[index] is >= '0' and <= '9')
                {
                    characters[index] = (char)('\u06F0' + characters[index] - '0');
                }
            }

            return new string(characters);
        }

        /// <summary>
        /// Backwards-compatible digit conversion. Historically this method also trimmed whitespace.
        /// </summary>
        public static string? PersianToEnglish(this string? persianString)
            => persianString.ToEnglishDigits()?.Trim();

        public static string? EnglishToPersian(this string? englishString)
            => englishString.ToPersianDigits();

        public static string? InsertParams(this string? text, params (string Key, string? Value)[]? parameters)
        {
            if (string.IsNullOrEmpty(text) || parameters is null || !text.Contains('{')) return text;

            var replacements = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
            foreach (var parameter in parameters)
            {
                if (string.IsNullOrEmpty(parameter.Key))
                {
                    continue;
                }

                // Keep the first value for duplicate keys, matching the old sequential replacement behavior.
                replacements.TryAdd(parameter.Key, parameter.Value);
            }

            if (replacements.Count == 0)
            {
                return text;
            }

            var result = new StringBuilder(text.Length);
            var position = 0;
            while (position < text.Length)
            {
                var openingBrace = text.IndexOf('{', position);
                if (openingBrace < 0)
                {
                    result.Append(text.AsSpan(position));
                    break;
                }

                var closingBrace = text.IndexOf('}', openingBrace + 1);
                if (closingBrace < 0)
                {
                    result.Append(text.AsSpan(position));
                    break;
                }

                var key = text.Substring(openingBrace + 1, closingBrace - openingBrace - 1);
                if (key.Length > 0 && replacements.TryGetValue(key, out var value))
                {
                    result.Append(text.AsSpan(position, openingBrace - position));
                    result.Append(value);
                    position = closingBrace + 1;
                }
                else
                {
                    // Keep unknown or empty placeholders unchanged and continue scanning after the opening brace.
                    result.Append(text.AsSpan(position, openingBrace - position + 1));
                    position = openingBrace + 1;
                }
            }

            return result.ToString();
        }

        public static string? InsertParams(this string? text, object? obj)
        {
            if (obj is null)
            {
                return text;
            }

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
        public static string ReplaceBrWithNewLine(this string value)
            => value.IsNullOrEmpty() ? value : Regex.Replace(value, @"<br\s*/?>", Environment.NewLine, RegexOptions.IgnoreCase);
        public static string ExtractNumber(this string value) => value.IsNullOrEmpty() ? value : Regex.Match(value, @"\d+").Value;
        public static bool IsNumber(this string value) => value.IsNullOrEmpty() ? false : value.Any(char.IsDigit);
        public static string Truncate(this string value, int length)
            => (value.IsNotNullOrEmpty() && value.Length > length) ? value.Substring(0, length) : value;
    }
}
