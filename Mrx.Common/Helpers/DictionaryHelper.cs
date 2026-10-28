namespace Mrx.Common.Helpers
{
    public static class DictionaryHelper
    {
        //public static Dictionary<string, string> MergeString(params Dictionary<string, string>[] dictionaries)
        //{
        //    var dic = new Dictionary<string, string>();
        //    if (dictionaries.IsNull()) return dic;
        //    foreach (var dictionary in dictionaries)
        //    {
        //        if (dictionary.IsNull()) continue;
        //        foreach (var (key, val) in dictionary)
        //        {
        //            if (dic.ContainsKey(key))
        //            {
        //                dic[key] += " " + val;
        //            }
        //            else
        //            {
        //                dic.Add(key, val);
        //            }
        //        }
        //    }
        //    return dic;
        //    //if (dictionaries.Where(p => p.IsNotNull()).IsNull()) return new Dictionary<string, string>();
        //    //Dictionary<string, string> dictionary = dictionaries.SelectMany(p => p).ToDictionary(p => p.Key, p => p.Value);
        //    //var d = dictionary.GroupBy(p => p.Key).ToDictionary(p => p.Key, p => p.Where(p => !p.Value.IsNullOrEmpty()).Distinct().Select(q => q.Value.Trim()).StringJoin(" ")).ToDictionary(p => p.Key, p => p.Value);
        //    //return dictionary.GroupBy(p => p.Key).ToDictionary(p => p.Key, p => p.Where(p => !p.Value.IsNullOrEmpty()).Distinct().Select(q => q.Value.Trim()).StringJoin(" ")).ToDictionary(p => p.Key, p => p.Value);
        //    //return dictionaries.SelectMany(dict => dict)
        //    //               .ToLookup(pair => pair.Key, pair => pair.Value)
        //    //               .ToDictionary(group => group.Key, group => group.Where(p => !p.Value.IsNullOrEmpty()).Distinct().Select(q => q.Value.Trim()).StringJoin(" "));
        //}
        public static Dictionary<string, string> Merge(params Dictionary<string, string>[] dictionaries)
        {
            if (dictionaries == null || dictionaries.Length == 0)
                return new Dictionary<string, string>();

            return dictionaries
                .Where(p => p.IsNotNullOrEmpty())
                .SelectMany(dict => dict)
                .ToLookup(pair => pair.Key, pair => pair.Value)
                .ToDictionary(group => group.Key, group => group.First());
        }
    }
}
