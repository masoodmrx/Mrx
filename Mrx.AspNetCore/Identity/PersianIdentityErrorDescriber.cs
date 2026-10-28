namespace Mrx.AspNetCore.Identity
{
    /// <summary>
    /// Provides Persian descriptions for ASP.NET Core Identity errors.
    /// Register it with Identity using <c>AddErrorDescriber&lt;PersianIdentityErrorDescriber&gt;()</c>.
    /// </summary>
    public sealed class PersianIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError() => Create(nameof(DefaultError), "یک خطای ناشناخته رخ داده است.");
        public override IdentityError ConcurrencyFailure() => Create(nameof(ConcurrencyFailure), "رکورد جاری پیش‌تر ویرایش شده است و ذخیرهٔ تغییرات شما اطلاعات قبلی را بازنویسی می‌کند.");
        public override IdentityError PasswordMismatch() => Create(nameof(PasswordMismatch), "کلمهٔ عبور نادرست است.");
        public override IdentityError InvalidToken() => Create(nameof(InvalidToken), "توکن نامعتبر است.");
        public override IdentityError LoginAlreadyAssociated() => Create(nameof(LoginAlreadyAssociated), "این ورود پیش‌تر به کاربر دیگری متصل شده است.");
        public override IdentityError InvalidUserName(string? userName) => Create(nameof(InvalidUserName), $"نام کاربری «{userName}» نامعتبر است؛ فقط حروف و اعداد مجاز هستند.");
        public override IdentityError InvalidEmail(string? email) => Create(nameof(InvalidEmail), $"نشانی ایمیل «{email}» نامعتبر است.");
        public override IdentityError DuplicateUserName(string userName) => Create(nameof(DuplicateUserName), $"نام کاربری «{userName}» قبلاً به کاربر دیگری اختصاص یافته است.");
        public override IdentityError DuplicateEmail(string email) => Create(nameof(DuplicateEmail), $"نشانی ایمیل «{email}» قبلاً به کاربر دیگری اختصاص یافته است.");
        public override IdentityError InvalidRoleName(string? role) => Create(nameof(InvalidRoleName), $"نام نقش «{role}» نامعتبر است.");
        public override IdentityError DuplicateRoleName(string role) => Create(nameof(DuplicateRoleName), $"نقش «{role}» قبلاً تعریف شده است.");
        public override IdentityError UserAlreadyHasPassword() => Create(nameof(UserAlreadyHasPassword), "برای این کاربر کلمهٔ عبور تنظیم شده است.");
        public override IdentityError UserLockoutNotEnabled() => Create(nameof(UserLockoutNotEnabled), "قفل شدن این کاربر فعال نیست.");
        public override IdentityError UserAlreadyInRole(string role) => Create(nameof(UserAlreadyInRole), $"نقش «{role}» قبلاً به این کاربر اختصاص یافته است.");
        public override IdentityError UserNotInRole(string role) => Create(nameof(UserNotInRole), $"نقش «{role}» به این کاربر اختصاص نیافته است.");
        public override IdentityError PasswordTooShort(int length) => Create(nameof(PasswordTooShort), $"کلمهٔ عبور باید حداقل {length.ToString(System.Globalization.CultureInfo.InvariantCulture).ToPersianDigits()} نویسه داشته باشد.");
        public override IdentityError PasswordRequiresNonAlphanumeric() => Create(nameof(PasswordRequiresNonAlphanumeric), "کلمهٔ عبور باید دست‌کم یک نویسهٔ غیرحرفی داشته باشد.");
        public override IdentityError PasswordRequiresDigit() => Create(nameof(PasswordRequiresDigit), "کلمهٔ عبور باید دست‌کم یک عدد داشته باشد.");
        public override IdentityError PasswordRequiresLower() => Create(nameof(PasswordRequiresLower), "کلمهٔ عبور باید دست‌کم یک حرف کوچک انگلیسی داشته باشد.");
        public override IdentityError PasswordRequiresUpper() => Create(nameof(PasswordRequiresUpper), "کلمهٔ عبور باید دست‌کم یک حرف بزرگ انگلیسی داشته باشد.");
        public override IdentityError RecoveryCodeRedemptionFailed() => Create(nameof(RecoveryCodeRedemptionFailed), "بازیابی ناموفق بود.");
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => Create(nameof(PasswordRequiresUniqueChars), $"کلمهٔ عبور باید دست‌کم {uniqueChars.ToString(System.Globalization.CultureInfo.InvariantCulture).ToPersianDigits()} نویسهٔ متفاوت داشته باشد.");

        private static IdentityError Create(string code, string description)
            => new() { Code = code, Description = description };
    }
}
