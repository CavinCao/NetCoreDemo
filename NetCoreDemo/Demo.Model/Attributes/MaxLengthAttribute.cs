using System;
using System.Collections.Generic;
using System.Text;
using Demo.Model.Base;

namespace Demo.Model.Attributes
{
    /// <summary>
    /// 数组和字符串最大长度验证 特性
    /// </summary>
    public class MaxLength : Validation
    {
        /// <summary>
        /// 最小长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 数组和字符串最大长度验证特性 构造函数
        /// </summary>
        /// <param name="length">最大长度</param>
        /// <param name="errorMessage">错误提示</param>
        public MaxLength(int length, string errorMessage = null) : base(ResponseCode.InvalidParameters, errorMessage)
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

            return length <= Length;
        }

        /// <summary>
        /// 构造失败结果
        /// </summary>
        /// <returns>Common Result</returns>
        public override ResponseResult BuildFailureResult()
        {
            return new ResponseResult
            {
                Result=false,
                Code = ErrorCode,
                ErrorMessage = ErrorMessage ?? $"字符串长度不能大于{Length}。"
            };
        }

        /// <summary>
        /// 确保Length长度是否是合法的值。
        /// </summary>
        /// <exception cref="InvalidOperationException">Length is zero or less than negative one.</exception>
        private void EnsureLegalLengths()
        {
            if (Length == 0 || Length < -1)
            {
                throw new InvalidOperationException("在 MaxLengthAttribute 中，限定长度不能小于等于零。");
            }
        }
    }
}
