

using Mrx.AspNetCore.Services;
using Mrx.AspNetCore.Options;

//"Ftp": {
//    "Host": "ftp://ftp.topbime.com/",
//    "Port": "21",
//    "Username": "dl.topbime",
//    "Password": "el8h8C?0",
//    "Domain": "https://dl.topbime.com"
//  },

var ftp = new FtpService();
ftp.Init(new FtpOptions("ftp://ftp.persiantheme.net/", 21, "dl.persiantheme", "HBC##%Khgfj6@$", "/dl.persiantheme.net", FluentFTP.FtpEncryptionMode.None));
//ftp.Init(new FtpOptions("ftp://ftp.persiantheme.net/", 21, "dl.topbime", "el8h8C?0"));
//ftp.Init(new FtpOptions("ftp://ftp.topbime.com/", 21, "dl.topbime", "el8h8C?0", FluentFTP.FtpEncryptionMode.None));
//await ftp.asyncFtp.SetWorkingDirectory("/dl.persiantheme.net");
await ftp.ConnectAsync();
//await ftp.asyncFtp.SetWorkingDirectory();
Console.WriteLine(string.Join(" , ", (await ftp.asyncFtp.GetListing()).Select(p => p.FullName)));
//Console.WriteLine(await ftp.asyncFtp.GetWorkingDirectory());

