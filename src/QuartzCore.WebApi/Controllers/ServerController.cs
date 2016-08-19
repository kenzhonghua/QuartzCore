using Microsoft.AspNetCore.Mvc;
using Quartz.Impl;
using QuartzCore.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartzCore.WebApi.Controllers
{
    [Route("api/server")]
    public class ServerController : Controller
    {
        [HttpGet]
        [Route("list")]
        public List<string> AllServers()
        {
            var servers = Config.ConfigInfo.ServerIp;
            return servers;
        }

        [HttpGet]
        [Route("{serverName}/details")]
        public async Task<ServerDetailsModel> ServerDetails(string serverName)
        {
            var schedulers = await SchedulerRepository.Instance.LookupAll().ConfigureAwait(false);
            return new ServerDetailsModel(schedulers);
        }
    }
}
