namespace Mrx.AspNetCore.ResultModels
{
    public class FtpUploadResult
    {
        public bool Success { get; set; } = false;
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public static FtpUploadResult Failed()
        {
            return new FtpUploadResult();
        }

        public static FtpUploadResult Failed(string fileName, string filePath)
        {
            return new FtpUploadResult { FileName = fileName, FilePath = filePath };
        }

        public static FtpUploadResult Successed(string fileName, string filePath)
        {
            return new FtpUploadResult { Success = true, FileName = fileName, FilePath = filePath };
        }

        public static implicit operator bool(FtpUploadResult r) => r?.Success ?? false;
        public static implicit operator string?(FtpUploadResult? r) => r?.FileName;
    }
}
