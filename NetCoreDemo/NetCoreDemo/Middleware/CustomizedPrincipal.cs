using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace NetCoreDemo.Middleware
{
    /// <summary>
    /// 自定义角色
    /// </summary>
    public class CustomizedPrincipal : ClaimsPrincipal
    {
        public CustomizedPrincipal(IIdentity identity) : base(identity)
        {
        }
    }
}
