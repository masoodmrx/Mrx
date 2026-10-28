namespace Mrx.Common.Extensions
{
    public static class DictionaryExtentions
    {
        //public static Dictionary<string, string> AddAndMerge(this Dictionary<string, string> dictionary, string key, string value)
        //{
        //    var dic = new Dictionary<string, string>();
        //    dic.Add(key, value);
        //    return DictionaryHelper.MergeString(dictionary, dic);
        //}
        public static Dictionary<string, object> ToDictionary(this Dictionary<string, string> dictionary)
        {
            return dictionary.ToDictionary(p => p.Key, p => p.Value as object);
        }
        public static Dictionary<string, string> ToDictionary(this object obj)
        {
            return obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(obj, null));
        }
    }
}
