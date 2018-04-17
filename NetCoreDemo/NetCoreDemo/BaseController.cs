using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Demo.Common;
using Demo.Model;
using NetCoreDemo.Middleware;

namespace NetCoreDemo
{
    public class BaseController : Controller
    {

        protected static readonly string SERVICE_URL = AppSetting.GetConfig("Services:InternalServiceUrl");

        protected UserInfo CurrentUserInfo
        {
            get
            {
                if (HttpContext.User.Identity is CustomizedIdentity identity)
                {
                    return identity.UserInfo;
                }

                return null;
            }
        }
        protected IActionResult AssertNotFound<T>(T obj) where T : new()
        {
            if (obj == null)
            {
                return NotFound();
            }

            return Json(obj);
        }
    }
}
