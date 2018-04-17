using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Demo.Model.Base;

namespace Demo.Model.Attributes
{
    /// <summary>
    /// 数值范围 特性
    /// </summary>
    public class Range : Validation
    {
        /// <summary>
        /// 最小值
        /// </summary>
        public object Minimum { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public object Maximum { get; set; }

        /// <summary>
        /// 操作数类型
        /// </summary>
        public Type OperandType { get; set; }

        /// <summary>
        /// 转换器
        /// </summary>
        private Func<object, object> Conversion { get; set; }

        /// <summary>
        /// 必填参数验证特性 构造函数
        /// </summary>
        /// <param name="minimum">最小值</param>
        /// <param name="maximum">最大值</param>
        /// <param name="errorMessage">错误提示</param>
        public Range(int minimum, int maximum, string errorMessage = null)
            : base(ResponseCode.OutOfRangeParameters, errorMessage)
        {
            OperandType = typeof(int);
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// 必填参数验证特性 构造函数
        /// </summary>
        /// <param name="minimum">最小值</param>
        /// <param name="maximum">最大值</param>
        /// <param name="errorMessage">错误提示</param>
        public Range(long minimum, long maximum, string errorMessage = null)
            : base(ResponseCode.OutOfRangeParameters, errorMessage)
        {
            OperandType = typeof(long);
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// 必填参数验证特性 构造函数
        /// </summary>
        /// <param name="minimum">最小值</param>
        /// <param name="maximum">最大值</param>
        /// <param name="errorMessage">错误提示</param>
        public Range(double minimum, double maximum, string errorMessage = null)
            : base(ResponseCode.OutOfRangeParameters, errorMessage)
        {
            OperandType = typeof(double);
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// 校验是否有效
        /// </summary>
        /// <param name="value">Object 对象</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            SetupConversion();

            if (value == null)
            {
                return true;
            }
            string s = value as string;
            if (s != null && String.IsNullOrEmpty(s))
            {
                return true;
            }

            object convertedValue;

            try
            {
                convertedValue = Conversion(value);
            }
            catch (FormatException)
            {
                return false;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }

            IComparable min = (IComparable)Minimum;
            IComparable max = (IComparable)Maximum;
            return min.CompareTo(convertedValue) <= 0 && max.CompareTo(convertedValue) >= 0;
        }

        private void Initialize(IComparable minimum, IComparable maximum, Func<object, object> conversion)
        {
            if (minimum.CompareTo(maximum) > 0)
            {
                throw new InvalidOperationException("RangeAttribute初始化错误，最小值必须小于等于最大值。");
            }

            Minimum = minimum;
            Maximum = maximum;
            Conversion = conversion;
        }

        private void SetupConversion()
        {
            if (Conversion == null)
            {
                object minimum = Minimum;
                object maximum = Maximum;

                if (minimum == null || maximum == null)
                {
                    throw new InvalidOperationException();
                }

                var operandType = minimum.GetType();

                if (operandType == typeof(int))
                {
                    Initialize((int)minimum, (int)maximum, v => Convert.ToInt32(v));
                }
                else if (operandType == typeof(double))
                {
                    Initialize((double)minimum, (double)maximum, v => Convert.ToDouble(v));
                }
                else
                {
                    Type type = OperandType;
                    if (type == null)
                    {
                        throw new InvalidOperationException();
                    }
                    Type comparableType = typeof(IComparable);
                    if (!comparableType.IsAssignableFrom(type))
                    {
                        throw new InvalidOperationException();
                    }

                    TypeConverter converter = TypeDescriptor.GetConverter(type);
                    IComparable min = (IComparable)converter.ConvertTo(Minimum, OperandType);
                    IComparable max = (IComparable)converter.ConvertTo(Maximum, OperandType);

                    Func<object, object> conversion = value => value != null && value.GetType() == type ? value : converter.ConvertFrom(value);

                    Initialize(min, max, conversion);
                }
            }
        }
    }
}
