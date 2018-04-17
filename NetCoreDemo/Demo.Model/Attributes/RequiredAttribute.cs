using System;
using System.Collections.Generic;
using System.Text;
using Demo.Model.Base;

namespace Demo.Model.Attributes
{
    /// <summary>
    /// 必填参数验证 特性
    /// </summary>
    public class Required : Validation
    {
        /// <summary>
        /// 允许字符串为空（包含空格）
        /// </summary>
        public bool AllowEmptyString { get; set; } = false;

        #region  + Constructor

        /// <summary>
        /// 必填参数验证特性 构造函数
        /// </summary>
        /// <param name="errorMessage">错误提示</param>
        public Required(string errorMessage = null) : base(ResponseCode.ParametersRequired, errorMessage)
        {
        }

        #endregion

        /// <summary>
        /// 校验是否有效
        /// </summary>
        /// <param name="value">Object 对象</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            // only check string length if empty strings are not allowed
            var stringValue = value as string;
            if (stringValue != null && !AllowEmptyString)
            {
                return stringValue.Trim().Length != 0;
            }

            return true;
        }

        /// <summary>
        /// 构造失败结果
        /// </summary>
        /// <returns>Common Result</returns>
        public override ResponseResult BuildFailureResult()
        {
            return new ResponseResult
            {
                Result = false,
                Code = ErrorCode,
                ErrorMessage = ErrorMessage ?? "存在必填的字段未填。"
            };
        }
    }
}
