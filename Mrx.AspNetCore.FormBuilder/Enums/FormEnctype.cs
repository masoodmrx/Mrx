namespace Mrx.AspNetCore.FormBuilder
{
    public enum FormEnctype
    {
        Auto,
        [Data("application/x-www-form-urlencoded")]
        application_x_www_form_urlencoded,
        [Data("multipart/form-data")]
        multipart_form_data,
        [Data("text/plain")]
        text_plain
    }
}
