using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Model
{
    public class LoginDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 图形验证码
        /// </summary>
        public string VerifyCode { get; set; }

        /// <summary>
        /// 登录方式，1-PC登录（未加密通讯）；2-APP登录（密码已加密）
        /// </summary>
        public int LoginStyle { get; set; }

        /// <summary>
        /// 二维码登录token
        /// </summary>
        public string QRToken { get; set; }
    }
}
