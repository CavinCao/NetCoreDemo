using System;
using System.Collections.Generic;
using System.Text;
using Demo.Model.Base;

namespace Demo.Model.Attributes
{
    /// <summary>
    /// 数组和字符串最小长度验证 特性
    /// </summary>
    public class MinLength : Validation
    {
        /// <summary>
        /// 最小长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 数组和字符串最小长度验证特性 构造函数
        /// </summary>
        /// <param name="length">最小长度</param>
        /// <param name="errorMessage">错误提示</param>
        /// <exception cref="ArgumentException">最小长度传入负数。</exception>
        public MinLength(int length, string errorMessage = null) : base(ResponseCode.InvalidParameters, errorMessage)
        {
            Length = length;
        }

        /// <summary>
        /// 校验是否有效
        /// </summary>
        /// <param name="value">Object 对象</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            EnsureLegalLengths();

            if (value == null)
            {
                return true;
            }
            var str = value as string;
            var length = str?.Length ?? ((Array)value).Length;

            return length >= Length;
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
                ErrorMessage = ErrorMessage ?? $"字符串长度不能小于{Length}。"
            };
        }

        /// <summary>
        /// 确保Length长度是否是合法的值。
        /// </summary>
        /// <exception cref="InvalidOperationException">Length is zero or less than negative one.</exception>
        private void EnsureLegalLengths()
        {
            if (Length < 0)
            {
                throw new InvalidOperationException("在 MinLengthAttribute 中，限定长度不能小于零。");
            }
        }
    }
}
