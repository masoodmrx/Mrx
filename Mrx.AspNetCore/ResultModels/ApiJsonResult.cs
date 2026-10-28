namespace Mrx.AspNetCore.ResultModels
{
    public class ApiJsonResult : BaseApiResult<object>
    {
        public static JsonResult Failed() => Json(ApiResult.Failed());
        public static JsonResult Failed(string error) => Json(ApiResult.Failed(error));
        public static JsonResult Failed<T>(string error, T result) where T : class, new() => Json(ApiResult.Failed(error, result));
        public static JsonResult Failed(List<KeyValuePair<string, string>> errors) => Json(ApiResult.Failed(errors));
        public static JsonResult Failed<T>(List<KeyValuePair<string, string>> errors, T result) where T : class, new() => Json(ApiResult.Failed(errors, result));
        public static JsonResult Successed() => Json(ApiResult.Successed());
        public static JsonResult Successed<T>(T result) where T : class, new() => Json(ApiResult.Successed(result));
        public static JsonResult SuccessedObject(object result) => Successed(result);
        public static JsonResult Ok<T>(bool success, T result = null, List<KeyValuePair<string, string>> errors = null) where T : class, new() => Json(ApiResult.Ok(success, result, errors));
        public static JsonResult Ok<T>(T result = null, List<KeyValuePair<string, string>> errors = null) where T : class, new() => Json(ApiResult.Ok(result, errors));
        public static JsonResult OkObject(object result = null, List<KeyValuePair<string, string>> errors = null) => Ok(result, errors);
        public static JsonResult Ok(bool success, List<KeyValuePair<string, string>> errors = null) => Json(ApiResult.Ok(success, errors));
        //public static JsonResult Ok<T>(T result = null) where T : class, new() => Json(ApiResult.Ok<T>(result));

        public static JsonResult Json(object value) => new JsonResult(value);
    }
}
