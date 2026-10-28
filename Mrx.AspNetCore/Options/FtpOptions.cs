namespace Mrx.AspNetCore.Options
{
    public class FtpOptions
    {
        public FtpOptions()
        {

        }
        public FtpOptions(string host, int port, string username, string password, string? path = null, FtpEncryptionMode encryptionMode = default, bool acceptValidateCertificate = true, string? domain = null)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
            Path = path;
            EncryptionMode = encryptionMode;
            AcceptValidateCertificate = acceptValidateCertificate;
            Domain = domain;
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Path { get; set; }
        public FtpEncryptionMode EncryptionMode { get; set; }
        public bool AcceptValidateCertificate { get; set; }
        public string? Domain { get; set; }
    }
}