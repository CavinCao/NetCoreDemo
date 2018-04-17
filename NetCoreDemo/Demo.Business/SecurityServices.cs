using Demo.Common;
using Demo.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Business
{
    public class SecurityServices
    {
        private static readonly int SESSION_TIMEOUT = AppSetting.GetConfig<int>("Security:SessionTimeout");
        private static readonly int QRCODE_TIMEOUT = AppSetting.GetConfig<int>("Security:QRCodeTimeout");
        private static readonly int VERIFY_CODE_TIMEOUT = AppSetting.GetConfig<int>("Security:VerfiyCodeTimeout");
        private static readonly RedisHelper redis = RedisHelper.GetInstance();


       
        public static async Task<LoginResult> UserLogin(LoginDto login, string verifyCodeToken)
        {
            if (login.LoginStyle == 1)
            {
                // TODO: 判断验证码是否正确
                if (string.IsNullOrWhiteSpace(verifyCodeToken))
                {
                    return new LoginResult { Code = (int)LoginResultCode.VerifyCodeError };
                }

                string verifyCode = await redis.Get<string>(verifyCodeToken);

                // 验证即失效
                await redis.RemoveKey(verifyCodeToken);

                if (string.IsNullOrWhiteSpace(verifyCode))
                {
                    return new LoginResult { Code = (int)LoginResultCode.VerifyCodeExcpired };
                }

                if (!verifyCode.Equals(login.VerifyCode, StringComparison.OrdinalIgnoreCase))
                {
                    return new LoginResult { Code = (int)LoginResultCode.VerifyCodeError };
                }
            }

            //从数据库里获取用户信息，校验用户密码是否正确，获取动作省略
            UserInfo resultData = new UserInfo();
            
            if (resultData != null)
            {
                // 验证成功，获取用户信息，存入缓存
                UserInfo userInfo = new UserInfo
                {
                    UserId = resultData.UserId,
                    EmployeeName = resultData.EmployeeName,
                    EmployeeId = resultData.EmployeeId
                };

                // TODO: 获取用户权限数据,获取动作省略
                List<PermissionDetail> permssions = new List<PermissionDetail>();
                if (permssions != null)
                {
                    userInfo.Permissions.AddRange(permssions);
                }

                string token = CreateSessionId();
                await redis.Set(token, userInfo, SESSION_TIMEOUT);
                if (login.LoginStyle == 2)
                {
                    // 若为二维码登录，则将会话ID存入二维码登录token
                    await redis.Set($"{login.QRToken}_1", token);
                }

                return new LoginResult { Code = (int)LoginResultCode.Succeed, SessionId = token, EmployeeId = resultData.EmployeeId, EmployeeName = resultData.EmployeeName};
            }
            else
            {
                // TODO: 记录错误日志

                return new LoginResult { Code = (int)LoginResultCode.ServiceError };
            }

            return new LoginResult { Code = (int)LoginResultCode.InvalidUser };
        }

        public static async Task Logout(string token)
        {
            await redis.RemoveKey(token);
        }


        public static async Task<string> GetVerifyCodeToken(string verifyCode)
        {
            string token = CreateSessionId();
            await redis.Set(token, verifyCode, VERIFY_CODE_TIMEOUT);

            return token;
        }

        public static async Task<string> GetQRToken()
        {
            string token = CreateSessionId();
            await redis.Set(token, 1, QRCODE_TIMEOUT);

            return token;
        }

        public static async Task<LoginResult> QRResult(string token)
        {
            long temp = await redis.IncAsync(token);
            if (temp <= 1)
            {
                return new LoginResult { Code = (int)LoginResultCode.QRCodeExpired };
            }

            string sessionId = await redis.Get<string>($"{token}_1");
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return new LoginResult { Code = (int)LoginResultCode.QRCodeStandby };
            }

            return new LoginResult { Code = (int)LoginResultCode.Succeed, SessionId = sessionId };
        }

        private static string CreateSessionId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
