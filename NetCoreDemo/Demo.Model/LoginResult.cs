using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Model
{
    /// <summary>
    /// 用户登录结果
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// 登录结果，-1-非法访问；0-服务异常；1-成功；2-用户名密码错误；3-验证码错误；4-等待扫码二维码；5-二维码失效；6-验证码超时
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 若登录成功后，创建的会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 登录成功后的员工Id
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 是否首次登陆
        /// </summary>
        public bool IsFirstLogin { get; set; }
    }

    public enum LoginResultCode
    {
        /// <summary>
        /// 非法访问
        /// </summary>
        InvalidAccess = -1,

        /// <summary>
        /// 0-服务异常
        /// </summary>
        ServiceError = 0,

        /// <summary>
        /// 1-成功
        /// </summary>
        Succeed = 1,

        /// <summary>
        /// 2-用户名密码错误
        /// </summary>
        InvalidUser = 2,

        /// <summary>
        /// 3-验证码错误
        /// </summary>
        VerifyCodeError = 3,

        /// <summary>
        /// 4-等待扫码二维码
        /// </summary>
        QRCodeStandby = 4,

        /// <summary>
        /// 5-二维码失效
        /// </summary>
        QRCodeExpired = 5,

        /// <summary>
        /// 6-验证码超时
        /// </summary>
        VerifyCodeExcpired = 6,
    }
}
