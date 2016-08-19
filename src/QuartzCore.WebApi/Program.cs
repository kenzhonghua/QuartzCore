using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace QuartzCore.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls(string.Format("http://{0}:{1}", string.IsNullOrEmpty(Config.ConfigInfo.HostName) ? "*" : Config.ConfigInfo.HostName, Config.ConfigInfo.Port == 0 ? 20009 : Config.ConfigInfo.Port))
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
