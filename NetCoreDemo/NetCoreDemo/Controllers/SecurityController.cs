using Demo.Business;
using Demo.Common;
using Demo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.DrawingCore.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreDemo.Controllers
{
    /// <summary>
    /// 访问控制服务
    /// </summary>
    [Route("[controller]")]
    public class SecurityController : BaseController
    {
        private const string INVOKER_TOKEN_HEADER = "x-invoker-token";
        private const string QRTOKEN_COOKIE_NAME = "qrToken";
        private const string USER_TOKEN_COOKIE_NAME = "UserToken";
        private const string VERFIY_CODE_TOKEN_COOKIE_NAME = "VerifyCodeToken";

        private static readonly string domain = AppSetting.GetConfig("Domain").TrimEnd('/');

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<LoginResult> Login([FromBody]LoginDto login)
        {
            string verifyCodeToken = Request.Cookies[VERFIY_CODE_TOKEN_COOKIE_NAME];

            //从数据库中获取相应数据
            LoginResult result = await SecurityServices.UserLogin(login, verifyCodeToken);

            if (result != null && result.Code == (int)LoginResultCode.Succeed)
            {
                // 创建cookie
                Response.Cookies.Append(USER_TOKEN_COOKIE_NAME, result.SessionId);
                Response.Headers.Add(INVOKER_TOKEN_HEADER, result.SessionId);
            }

            return result;
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            bool hasValue = Request.Headers.TryGetValue(INVOKER_TOKEN_HEADER, out StringValues token);
            if (!hasValue || token.Count == 0)
            {
                return new EmptyResult();
            }

            await SecurityServices.Logout(token[0]);

            return new EmptyResult();
        }

        /// <summary>
        /// 获取图形验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet("VerifyCode")]
        public async Task GetVerifyCode()
        {
            Response.ContentType = "image/jpeg";
            using (var stream = VerifyCodeHelper.Create(out string code))
            {
                var buffer = stream.ToArray();

                // 将验证码的token放入cookie
                Response.Cookies.Append(VERFIY_CODE_TOKEN_COOKIE_NAME, await SecurityServices.GetVerifyCodeToken(code));

                await Response.Body.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// 获取登录二维码
        /// </summary>
        /// <returns></returns>
        [HttpGet("qrcode")]
        public async Task GetQRCode()
        {
            Response.ContentType = "image/jpeg";

            string qrToken = await SecurityServices.GetQRToken();

            var bitmap = QRCodeHelper.GetQRCode($"{domain}/Security/Login?token={qrToken}", 4);

            // 将二维码回调标识输出给cookie
            // 创建cookie
            Response.Cookies.Append(QRTOKEN_COOKIE_NAME, qrToken);

            bitmap.Save(Response.Body, ImageFormat.Jpeg);
        }

        /// <summary>
        /// 二维码登录结果回调
        /// </summary>
        /// <returns></returns>
        [HttpGet("qrresult")]
        public async Task<LoginResult> GetQRResult()
        {
            string qrToken = Request.Cookies[QRTOKEN_COOKIE_NAME];
            if (string.IsNullOrWhiteSpace(qrToken))
            {
                return new LoginResult { Code = (int)LoginResultCode.InvalidAccess };
            }

            return await SecurityServices.QRResult(qrToken);
        }
    }
}
