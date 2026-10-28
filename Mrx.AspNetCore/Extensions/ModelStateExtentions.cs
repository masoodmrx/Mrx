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
                .SelectMany(modelError => modelError.Value!.Errors.Select(error =>
                    new KeyValuePair<string, string>(modelError.Key, error.ErrorMessage)))
                .ToList();
        }
        public static List<KeyValuePair<string, string>> GetErrors(this IActionContextAccessor actionContextAccessor)
        {
            return actionContextAccessor?.ActionContext?.ModelState?.GetErrors() ?? [];
        }
        public static string? GetError(this ModelStateDictionary modelState)
        {
            return modelState.Values
                .SelectMany(state => state.Errors)
                .Select(error => error.ErrorMessage)
                .FirstOrDefault(error => !string.IsNullOrWhiteSpace(error));
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
        public static void AddErrors(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors)
        {
            ArgumentNullException.ThrowIfNull(modelState);
            ArgumentNullException.ThrowIfNull(errors);

            foreach (var error in errors)
            {
                modelState.AddModelError(error.Code, error.Description);
            }
        }
        public static void AddErrors(this IActionContextAccessor actionContextAccessor, IEnumerable<IdentityError> errors)
        {
            actionContextAccessor?.ActionContext?.ModelState?.AddErrors(errors);
        }
    }
}
