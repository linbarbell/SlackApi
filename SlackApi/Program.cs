using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SlackApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var exePath = Process.GetCurrentProcess().MainModule.FileName;
            var directoryPath = Path.GetDirectoryName(exePath);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(directoryPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var webHost = new WebHostBuilder()
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            webHost.Run();
        }
    }
}
