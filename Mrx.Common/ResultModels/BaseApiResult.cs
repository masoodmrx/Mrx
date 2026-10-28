using System.Text.Json.Serialization;

namespace Mrx.Common.ResultModels
{
    public abstract class BaseApiResult<TResult> where TResult : class, new()
    {
        public bool Success { get; set; } = false;
        public TResult? Result { get; set; }
        public List<KeyValuePair<string?, string?>>? Errors { get; set; }
        [JsonIgnore]
        public string? Error
        {
            get => Errors?.FirstOrDefault().Value;
            set
            {
                if (value.IsNotNullOrEmpty())
                {
                    Errors = Errors ?? new List<KeyValuePair<string?, string?>>();
                    Errors.Add(new KeyValuePair<string?, string?>(null, value));
                }
            }
        }
        public static implicit operator bool(BaseApiResult<TResult> r) => r.Success;
        public static implicit operator TResult?(BaseApiResult<TResult> r) => r.Result;
    }
}
