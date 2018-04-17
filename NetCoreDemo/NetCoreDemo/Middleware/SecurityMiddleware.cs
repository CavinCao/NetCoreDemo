using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Demo.Common;
using Demo.Model;
using Demo.Model.Base;

namespace NetCoreDemo.Middleware
{
    public class SecurityMiddleware
    {
        /// <summary>
        /// 客户端token的header名称
        /// </summary>
        public const string INVOKER_TOKEN_HEADER = "x-invoker-token";
        private const string USER_TOKEN_COOKIE_NAME = "UserToken";

        private static readonly HashSet<string> excludeUrl = new HashSet<string>();
        private static readonly RedisHelper redis = RedisHelper.GetInstance();

        private static readonly int SESSION_TIMEOUT = AppSetting.GetConfig<int>("Security:SessionTimeout");

        private readonly RequestDelegate _next;

        public SecurityMiddleware(RequestDelegate next)
        {
            this._next = next;
            var urls = AppSetting.GetConfigArray("Security:ExcludeUrl");
            if (urls != null)
            {
                foreach (var item in urls)
                {
                    excludeUrl.Add(item.ToLower());
                }
            }
        }

        public async Task Invoke(HttpContext context)
        {
            string path = context.Request.Path.ToString().ToLower();

            // 判断请求的路径是否是排除权限限制的，如果是，则直接进行下一步
            if (path.StartsWith("/swagger") || excludeUrl.Contains(path))
            {
                await _next(context);
                return;
            }

            string userToken = string.Empty;
            bool hasValue = context.Request.Headers.TryGetValue(INVOKER_TOKEN_HEADER, out StringValues token);
            if (!hasValue || token.Count == 0)
            {
                // 若header没取到token，则尝试从cookie中获取
                userToken = context.Request.Cookies[USER_TOKEN_COOKIE_NAME];
                if (string.IsNullOrWhiteSpace(userToken))
                {
                    // TODO: 尚未登录，未经授权
                    await CreateUnauthorizedResponse(context);

                    return;
                }
            }
            else
            {
                userToken = token[0];
            }

            //根据token从redis中获取用户信息
            var userInfo = await GetUserInfo(userToken);
            if (userInfo == null)
            {
                //尚未登录，未经授权
                await CreateUnauthorizedResponse(context);
                return;
            }
            //if (userInfo.Permissions.Count(m => m.Url.Equals(path, StringComparison.Ordinal)
            //                                 && (m.Verbs & (long)GetVerbs(context.Request.Method)) > 0) > 0)
            //{
            // TODO: 将用户身份信息放入上下文
            context.User = new CustomizedPrincipal(new CustomizedIdentity(userInfo));

            await _next(context);
            //}
            //else
            //{
            //     权限不足的
            //    await CreatePermissionDeniedResponse(context);
            //    return;
            //}
        }

        private static async Task CreateUnauthorizedResponse(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json;charset=utf-8";

            ResponseResult result = new ResponseResult
            {
                Result = false,
                ErrorMessage = "您需要登录后访问此资源，请先进行登录操作。",
                Code = ResponseCode.Unauthorized
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(result), Encoding.UTF8);
        }

        private static async Task CreatePermissionDeniedResponse(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.ContentType = "application/json;charset=utf-8";

            ResponseResult result = new ResponseResult
            {
                Result = false,
                ErrorMessage = "对不起，您没有操作此项目的权限，请核实后再试。",
                Code = ResponseCode.PermissionDenied
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(result), Encoding.UTF8);
        }

        
        private async Task<UserInfo> GetUserInfo(string userToken)
        {
            UserInfo userInfo = await redis.Get<UserInfo>(userToken);
            if (userInfo != null)
                await redis.UpdateTTL(userToken, SESSION_TIMEOUT);

            return userInfo;
        }
    }
}
