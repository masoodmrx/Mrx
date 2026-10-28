namespace Mrx.AspNetCore.Services
{
    public class FtpService
    {
        public bool AutoConnect { get; set; } = true;
        public FtpClient ftp;
        public AsyncFtpClient asyncFtp;
        private FtpOptions options;

        public FtpOptions Account
        {
            get => options;
            set
            {
                options = value;
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var config = new FtpConfig();
                //if (options.Protocol == FtpProtocol.Ftps)
                //{
                //    config.EncryptionMode = FtpEncryptionMode.Explicit;
                //    //config.ValidateAnyCertificate = true;
                //    //config.ValidateCertificateRevocation = true;
                //}
                //config.DataConnectionType = FtpDataConnectionType.PASV;
                //config.;
                config.EncryptionMode = options.EncryptionMode;
                ftp = new FtpClient(options.Host, options.Username, options.Password, options.Port, config);
                asyncFtp = new AsyncFtpClient(options.Host, options.Username, options.Password, options.Port, config);
                if (options.AcceptValidateCertificate)
                {
                    ftp.ValidateCertificate += (s, e) => e.Accept = true;
                    asyncFtp.ValidateCertificate += (s, e) => e.Accept = true;
                }
            }
        }
        public FtpService() { }
        public FtpService(IOptions<FtpOptions> options)
        {
            this.options = options.Value;
            Init(this.options);
        }
        public void Init(FtpOptions ftpAccount)
        {
            Account = ftpAccount;
        }

        public async Task ConnectAsync()
        {
            await asyncFtp.Connect();
            await SetWorkingDirectoryAsync(Account.Path);
        }
        public async Task SetWorkingDirectoryAsync(string? path)
        {
            if (path != null) await asyncFtp.SetWorkingDirectory(path);
        }
        public async Task DisconnectAsync()
        {
            await asyncFtp.Disconnect();
        }
        public async Task<FtpUploadResult> UploadAsync(byte[] bytes, string directory, string fileName)
        {
            string path = $"{directory}/{fileName}";
            if (bytes.IsNull())
            {
                return FtpUploadResult.Failed(fileName, Account.Domain + path);
            }

            if (AutoConnect)
            {
                await ConnectAsync();
            }

            if (!asyncFtp.IsConnected)
            {
                return FtpUploadResult.Failed();
            }

            if (!await asyncFtp.DirectoryExists(directory))
            {
                await asyncFtp.CreateDirectory(directory, true);
            }

            FtpStatus result = await asyncFtp.UploadBytes(bytes, path);
            if (AutoConnect)
            {
                await DisconnectAsync();
            }

            if (!result.IsSuccess())
            {
                return FtpUploadResult.Failed();
            }

            return FtpUploadResult.Successed(fileName, Account.Domain + path);
        }
        public async Task<FtpUploadResult> UploadAsync(IFormFile file, string directory, string fileName, string fileNameForDelete = null)
        {
            if (file == null)
            {
                return FtpUploadResult.Failed(fileNameForDelete, null);
            }

            FtpUploadResult result = await UploadAsync(await file.ToByteArrayAsync(), directory, fileName);
            if (fileNameForDelete.IsNotNullOrEmpty() && result.Success)
            {
                await DeleteAsync(directory, fileNameForDelete);
            }

            return result;
        }
        public async Task<FtpUploadResult> UploadAsync(IFormFile file, string directory, string fileNameForDelete = null)
        {
            return await UploadAsync(file, directory, file == null ? null : (Guid.NewGuid() + Path.GetExtension(file.FileName).ToLower()), fileNameForDelete);
        }
        public async Task<bool> DeleteAsync(string directory, string fileName)
        {
            if (fileName.IsNullOrEmpty())
            {
                return false;
            }

            if (AutoConnect)
            {
                await ConnectAsync();
            }

            if (!asyncFtp.IsConnected)
            {
                return false;
            }

            string path = $"{directory}/{fileName}";
            if (await asyncFtp.FileExists(path))
            {
                await asyncFtp.DeleteFile(path);
            }

            if (AutoConnect)
            {
                await DisconnectAsync();
            }

            return true;
        }
        public async Task<bool> DeleteAsync(string directory, params string[] fileNames)
        {
            bool result = true;
            foreach (string fileName in fileNames)
            {
                if (!await DeleteAsync(directory, fileName))
                {
                    result = false;
                }
            }
            return result;
        }
        public async Task<bool> DeleteAsync(string directory, List<string> fileNames)
        {
            return await DeleteAsync(directory, fileNames.ToArray());
        }

        public void Connect()
        {
            ftp.Connect();
            SetWorkingDirectory(Account.Path);
        }
        public void SetWorkingDirectory(string? path)
        {
            if (path != null) ftp.SetWorkingDirectory(path);
        }
        public void Disconnect()
        {
            ftp.Disconnect();
        }
        public FtpUploadResult Upload(byte[] bytes, string directory, string fileName)
        {
            string path = $"{directory}/{fileName}";
            if (bytes.IsNull())
            {
                return FtpUploadResult.Failed(fileName, Account.Domain + path);
            }

            if (AutoConnect)
            {
                Connect();
            }

            if (!ftp.IsConnected)
            {
                return FtpUploadResult.Failed();
            }

            if (!ftp.DirectoryExists(directory))
            {
                ftp.CreateDirectory(directory, true);
            }

            FtpStatus result = ftp.UploadBytes(bytes, path);
            if (AutoConnect)
            {
                Disconnect();
            }

            if (!result.IsSuccess())
            {
                return FtpUploadResult.Failed();
            }

            return FtpUploadResult.Successed(fileName, Account.Domain + path);
        }
        public FtpUploadResult Upload(IFormFile file, string directory, string fileName, string fileNameForDelete = null)
        {
            if (file == null)
            {
                return FtpUploadResult.Failed(fileNameForDelete, null);
            }

            FtpUploadResult result = Upload(file.ToByteArray(), directory, fileName);
            if (!string.IsNullOrEmpty(fileNameForDelete) && result.Success)
            {
                Delete(directory, fileNameForDelete);
            }

            return result;
        }
        public FtpUploadResult Upload(IFormFile file, string directory, string fileNameForDelete = null)
        {
            return Upload(file, directory, file == null ? null : (Guid.NewGuid() + Path.GetExtension(file.FileName).ToLower()), fileNameForDelete);
        }
        public bool Delete(string directory, string fileName)
        {
            if (fileName.IsNullOrEmpty())
            {
                return false;
            }

            if (AutoConnect)
            {
                Connect();
            }

            if (!ftp.IsConnected)
            {
                return false;
            }

            string path = $"{directory}/{fileName}";
            if (ftp.FileExists(path))
            {
                ftp.DeleteFile(path);
            }

            if (AutoConnect)
            {
                Disconnect();
            }

            return true;
        }
        public bool Delete(string directory, params string[] fileNames)
        {
            bool result = true;
            foreach (string fileName in fileNames)
            {
                if (!Delete(directory, fileName))
                {
                    result = false;
                }
            }
            return result;
        }
        public bool Delete(string directory, List<string> fileNames)
        {
            return Delete(directory, fileNames.ToArray());
        }
    }
}
