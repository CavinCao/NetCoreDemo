using System;
using System.Collections.Generic;
using System.Text;
using Demo.Model.Base;

namespace Demo.Model.Attributes
{
    /// <summary>
    /// 参数验证 特性基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public abstract class Validation : Attribute
    {
        /// <summary>
        /// 增删改操作通用返回结果
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 错误响应码
        /// </summary>
        public ResponseCode ErrorCode { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="errorCode">响应码</param>
        /// <param name="errorMessage">错误信息</param>
        protected Validation(ResponseCode errorCode, string errorMessage)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// 校验参数是否合法
        /// </summary>
        /// <param name="value">需要校验的对象</param>
        /// <returns></returns>
        public abstract bool IsValid(object value);

        /// <summary>
        /// 构造失败结果
        /// </summary>
        /// <returns>Common Result</returns>
        public virtual ResponseResult BuildFailureResult()
        {
            return new ResponseResult
            {
                Result = false,
                Code = ErrorCode,
                ErrorMessage = ErrorMessage
            };
        }
    }
}
