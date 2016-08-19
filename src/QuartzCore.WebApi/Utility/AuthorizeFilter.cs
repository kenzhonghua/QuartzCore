using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzCore.WebApi.Utility
{
    public class AuthorizeFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!string.IsNullOrEmpty(Config.ConfigInfo.Key))
            {
                var key = context.HttpContext.Request.Query["key"];
                if (Config.ConfigInfo.Key != key)
                {
                    context.Result = new JsonResult(new { code = 201, reason = "Key不正确，请确认" });
                    return;
                }
            }
        }
    }
}
