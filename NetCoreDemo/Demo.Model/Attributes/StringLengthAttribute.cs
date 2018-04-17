using System;
using System.Collections.Generic;
using System.Text;
using Demo.Model.Base;

namespace Demo.Model.Attributes
{
    /// <summary>
    /// 数组和字符串长度范围验证 特性
    /// </summary>
    public class StringLength : Validation
    {
        /// <summary>
        /// 最小长度
        /// </summary>
        public int MinimumLength { get; set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaximumLength { get; set; }

        #region  + Constructor

        /// <summary>
        /// 数组和字符串最大长度验证特性 构造函数
        /// </summary>
        /// <param name="maxinumLength">最小长度</param>
        /// <param name="minimumLength">最大长度</param>
        /// <param name="errorMessage">错误提示</param>
        public StringLength(int minimumLength, int maxinumLength, string errorMessage) : base(ResponseCode.InvalidParameters, errorMessage)
        {
            MinimumLength = minimumLength;
            MaximumLength = maxinumLength;
        }

        #endregion

        /// <summary>
        /// 校验是否有效
        /// </summary>
        /// <param name="value">Object 对象</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            EnsureLegalLengths();

            int length = ((string)value)?.Length ?? 0;
            return value == null || (length >= MinimumLength && length <= MaximumLength);
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
                ErrorMessage = ErrorMessage ?? $"字符串长度需要在{MinimumLength}和{MaximumLength}之间。"
            };
        }

        private void EnsureLegalLengths()
        {
            if (MaximumLength < 0)
            {
                throw new InvalidOperationException("在 StringLengthAttribute 中，最大值不能小于等于零。");
            }

            if (MaximumLength < MinimumLength)
            {
                throw new InvalidOperationException("在 StringLengthAttribute 中，最大值不能小于最小价。");
            }
        }
    }
}
