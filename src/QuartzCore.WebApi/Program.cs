using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

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
