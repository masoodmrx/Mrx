namespace Mrx.AspNetCore.Extensions
{
    public static class CommonExtentions
    {
        public static string GetBaseUrl(this HttpContext httpContext) => $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
        public static string GetIp(this HttpContext httpContext) => httpContext.Connection.RemoteIpAddress.ToString();
        public static string GetIp(this IHttpContextAccessor httpContextAccessor) => httpContextAccessor.HttpContext.GetIp();
        public static bool IsAjaxRequest(this HttpRequest request) => request?.Headers?["X-Requested-With"].ToString() == "XMLHttpRequest";
        public static bool IsAjaxRequest(this HttpContext context) => context.Request.IsAjaxRequest();
        public static bool IsAjaxRequest(this IHttpContextAccessor httpContextAccessor) => httpContextAccessor.HttpContext.IsAjaxRequest();
        public static byte[] ToByteArray(this IFormFile file)
        {
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
        public static async Task<byte[]> ToByteArrayAsync(this IFormFile file)
        {
            if (file == null) return null;
            MemoryStream memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}