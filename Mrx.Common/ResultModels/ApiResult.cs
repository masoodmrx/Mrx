namespace Mrx.Common.ResultModels
{
    public class ApiResult : BaseApiResult<object>
    {
        public static ApiResult Failed() => new ApiResult();
        public static ApiResult Failed(string? error) => new ApiResult { Errors = new List<KeyValuePair<string?, string?>> { new KeyValuePair<string?, string?>(null, error) } };
        public static ApiResult<T> Failed<T>(string? error, T? result) where T : class, new() => new ApiResult<T> { Errors = new List<KeyValuePair<string?, string?>> { new KeyValuePair<string?, string?>(null, error) }, Result = result };
        public static ApiResult Failed(List<KeyValuePair<string?, string?>>? errors) => new ApiResult { Errors = errors };
        public static ApiResult<T> Failed<T>(List<KeyValuePair<string?, string?>>? errors, T? result) where T : class, new() => new ApiResult<T> { Errors = errors, Result = result };

        public static ApiResult Successed() => new ApiResult { Success = true };
        //public static ApiResult<T> Successed<T>() where T : class, new() => new ApiResult<T> { Success = true };
        public static ApiResult<T> Successed<T>(T? result) where T : class, new() => Ok(true, result);
        public static ApiResult SuccessedObject(object result) => (ApiResult)Successed(result);

        public static ApiResult<T> Ok<T>(bool success, T? result, List<KeyValuePair<string?, string?>>? errors = null) where T : class, new() => new ApiResult<T> { Success = success, Result = result, Errors = errors };
        public static ApiResult<T> Ok<T>(T? result, List<KeyValuePair<string?, string?>>? errors = null) where T : class, new() => Ok(result != null, result, errors);
        public static ApiResult? OkObject(object result, List<KeyValuePair<string?, string?>>? errors = null) => (ApiResult)Ok(result, errors);
        public static ApiResult Ok(bool success, List<KeyValuePair<string?, string?>>? errors = null) => new ApiResult { Success = success, Errors = errors };
        //public static ApiResult<T> Ok<T>(T? result) where T : class, new() => Ok<T>(result != null, result);
    }
    public class ApiResult<T> : BaseApiResult<T> where T : class, new() { }
}
