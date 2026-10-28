namespace Mrx.Common.Helpers
{
    public static class Helper
    {
        public static string InsertsValues(this string text, KeyValuePair<string, string>[] items)
        {
            foreach (KeyValuePair<string, string> item in items)
            {
                text = text.Replace("{" + item.Key + "}", item.Value);
            }

            return text;
        }
        /// <summary>
        /// تعیین معتبر بودن کد ملی
        /// </summary>
        /// <param name="nationalCode">کد ملی وارد شده</param>
        /// <returns>
        /// در صورتی که کد ملی صحیح باشد خروجی <c>true</c> و در صورتی که کد ملی اشتباه باشد خروجی <c>false</c> خواهد بود
        /// </returns>
        /// <exception cref="Exception"></exception>
        public static bool IsValidNationalCode(this string nationalCode, out string error)
        {
            //در صورتی که کد ملی وارد شده تهی باشد

            if (string.IsNullOrEmpty(nationalCode))
            {
                error = "کد ملی را بدرستی وارد نمایید.";
                return false;
            }

            nationalCode = nationalCode.PersianToEnglish();

            //در صورتی که کد ملی وارد شده طولش کمتر از 10 رقم باشد
            if (nationalCode.Length != 10)
            {
                error = "طول کد ملی باید ده کاراکتر باشد.";
                return false;
            }

            //در صورتی که کد ملی ده رقم عددی نباشد
            Regex regex = new Regex(@"\d{10}");
            if (!regex.IsMatch(nationalCode))
            {
                error = "کد ملی تشکیل شده از ده رقم عددی می‌باشد؛ لطفا کد ملی را بدرستی وارد نمایید.";
                return false;
            }

            //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
            string[] allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
            if (allDigitEqual.Contains(nationalCode))
            {
                error = "کد ملی را بدرستی وارد نمایید.";
                return false;
            }

            //عملیات شرح داده شده در بالا
            char[] chArray = nationalCode.ToCharArray();
            int num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
            int num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
            int num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
            int num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
            int num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
            int num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
            int num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
            int num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
            int num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
            int a = Convert.ToInt32(chArray[9].ToString());

            int b = num0 + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9;
            int c = b % 11;
            bool result = c < 2 && a == c || c >= 2 && 11 - c == a;
            error = result ? null : "کد ملی را بدرستی وارد نمایید.";
            return result;
        }
        /// <summary>
        /// تعیین معتبر بودن کد ملی
        /// </summary>
        /// <param name="nationalCode">کد ملی وارد شده</param>
        /// <returns>
        /// در صورتی که کد ملی صحیح باشد خروجی <c>true</c> و در صورتی که کد ملی اشتباه باشد خروجی <c>false</c> خواهد بود
        /// </returns>
        /// <exception cref="Exception"></exception>
        public static bool IsValidNationalCode(this string nationalCode)
        {
            return nationalCode.IsValidNationalCode(out string error);
        }
        public static bool IsValidPhoneNumber(this string phoneNumber)
        {
            string pattern = "^[0۰][9۹][0-9۰-۹]{9}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
        public static bool IsValidPersian(this string persian)
        {
            string pattern = @"^[\u0600-\u06FF\s]+$";
            return Regex.IsMatch(persian, pattern);
        }
    }
}
