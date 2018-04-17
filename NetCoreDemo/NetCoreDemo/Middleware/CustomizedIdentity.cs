using Demo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCoreDemo.Middleware
{
    /// <summary>
    /// 自定义用户身份标识
    /// </summary>
    public class CustomizedIdentity : ClaimsIdentity
    {
        private UserInfo _user = null;

        public CustomizedIdentity(UserInfo user)
        {
            _user = user;
        }

        public UserInfo UserInfo => _user;
    }
}
