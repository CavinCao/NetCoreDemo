using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Model.Base.ExceptionModel
{
    /// <summary>
    /// 异常实体
    /// </summary>
    public class ExceptionModel : Exception
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public ResponseCode ErrorCode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 错误消息构造
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <param name="errorMessage">错误消息</param>
        public ExceptionModel(ResponseCode errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            throw new Exception(errorMessage, this);
        }

        /// <summary>
        /// 错误消息构造
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        public ExceptionModel(string errorMessage)
        {
            ErrorCode = ResponseCode.UnknownException;
            ErrorMessage = errorMessage;
            throw new Exception(errorMessage, this);
        }
    }
}
