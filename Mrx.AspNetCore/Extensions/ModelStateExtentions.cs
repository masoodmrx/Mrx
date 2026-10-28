namespace Mrx.AspNetCore.Extensions
{
    public static class ModelStateExtentions
    {
        //public static async Task<byte[]> GetErrors(this ModelStateDictionary modelState)
        //{
        //    return modelState.Keys.Select(p =>p. p.Errors.FirstOrDefault().ErrorMessage);
        //}
        public static List<KeyValuePair<string, string>> GetErrors(this ModelStateDictionary modelState)
        {
            return modelState
                .Where(modelError => modelError.Value?.Errors.Count > 0)
                .Select(modelError => new KeyValuePair<string, string>(modelError.Key, modelError.Value?.Errors.FirstOrDefault()?.ErrorMessage))
                .ToList();
        }
        public static List<KeyValuePair<string, string>> GetErrors(this IActionContextAccessor actionContextAccessor)
        {
            return actionContextAccessor?.ActionContext?.ModelState?.GetErrors();
        }
        public static string? GetError(this ModelStateDictionary modelState)
        {
            return modelState.Values.Select(p => p.Errors.FirstOrDefault()?.ErrorMessage).FirstOrDefault();
        }
        public static string? GetError(this IActionContextAccessor actionContextAccessor)
        {
            return actionContextAccessor?.ActionContext?.ModelState?.GetError();
        }
        public static void AddError(this IActionContextAccessor actionContextAccessor, string key, string errorMessage)
        {
            actionContextAccessor?.ActionContext?.ModelState?.AddModelError(key, errorMessage);
        }
        public static void AddError(this IActionContextAccessor actionContextAccessor, string errorMessage)
        {
            actionContextAccessor?.AddError("", errorMessage);
        }
        public static void AddError(this ModelStateDictionary modelState, string key, string errorMessage)
        {
            modelState?.AddModelError(key, errorMessage);
        }
        public static void AddError(this ModelStateDictionary modelState, string errorMessage)
        {
            modelState?.AddModelError("", errorMessage);
        }
        public static void AddErrors(this IActionContextAccessor actionContextAccessor, IEnumerable<IdentityError> errors)
        {
            foreach (IdentityError error in errors)
            {
                actionContextAccessor.AddError(error.Code, error.Description);
            }
        }
    }
}
