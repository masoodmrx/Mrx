namespace Mrx.AspNetCore.Services
{
    public class EmailService
    {
        private readonly EmailOptions options;

        public EmailService(IOptions<EmailOptions> options)
        {
            this.options = options.Value;
            Init(this.options.Host, this.options.Port, this.options.Email, this.options.UserName, this.options.Password, this.options.EnableSsl);
        }

        public void Init(string host, int port, string email, string userName, string password, bool enableSsl)
        {
            Host = host;
            Port = port;
            Email = email;
            UserName = userName;
            Password = password;
            EnableSsl = enableSsl;
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }

        public void Send(string subject, string body, params string[] emails)
        {
            if (emails.IsNull())
            {
                return;
            }

            SmtpClient smtpClient = new SmtpClient
            {
                Credentials = new NetworkCredential
                {
                    UserName = UserName,
                    Password = Password
                },
                Host = Host,
                Port = Port,
                EnableSsl = EnableSsl
            };

            MailMessage emailMessage = new MailMessage();
            foreach (string email in emails)
            {
                emailMessage.To.Add(new MailAddress(email));
            }

            emailMessage.From = new MailAddress(Email);
            emailMessage.Subject = subject;
            emailMessage.IsBodyHtml = true;
            emailMessage.Body = body;
            smtpClient.Send(emailMessage);
        }
    }
}
