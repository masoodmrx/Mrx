namespace Mrx.AspNetCore.FormBuilder
{
    public static class FileType
    {
        public const string Audio = "audio/*";
        public const string Video = "video/*";
        public const string Image = "image/*";
    }
    public static class FileExtension
    {
        public const string Gif = ".gif";
        public const string Svg = ".svg";
        public const string Png = ".png";
        public const string Apk = ".apk";
    }
    /// <summary>
    /// https://www.iana.org/assignments/media-types/media-types.xhtml
    /// </summary>
    public static class MediaType
    {
        public static class App
        {
            public const string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel";
            public const string Pdf = "application/pdf";
        }
        public static class Image
        {
            public const string Png = "image/png";
            public const string Jpeg = "image/jpeg";
            public const string Bmp = "image/bmp";
            public const string Tiff = "image/tiff";
            public const string Svg = "image/svg+xml";
        }
        public static class Video
        {
        }
    }
}
