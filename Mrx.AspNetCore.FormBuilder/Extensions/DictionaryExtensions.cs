namespace Mrx.AspNetCore.FormBuilder
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, string> AddClasses(this Dictionary<string, string> dictionary, List<string> classes)
        {
            dictionary.Add("class", Bootstrap.Class(classes.ToArray()));
            return dictionary;
        }
    }
}
