namespace Mrx.AspNetCore.Extensions
{
    public static class IdentityExtentions
    {
        public static void AddModelErrors(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors)
        {
            modelState.AddErrors(errors);
        }
        public static string? GetUserId(this ClaimsPrincipal? claims) => claims?.FindFirstValue(ClaimTypes.NameIdentifier);
        public static string? GetUserId(this IHttpContextAccessor? httpContextAccessor) => httpContextAccessor?.HttpContext?.User?.GetUserId();
        public static DateTime? GetUpdateDate(this ClaimsPrincipal? claims) => claims?.FindFirstValue("UpdateDate").ToDateTimeNullable();
    }
}
