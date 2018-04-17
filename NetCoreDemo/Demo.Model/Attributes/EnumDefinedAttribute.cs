using System;
using System.Collections.Generic;
using System.Text;
using Demo.Model.Base;

namespace Demo.Model.Attributes
{
    /// <summary>
    /// 枚举定义特性
    /// </summary>
    public class EnumDefined : Validation
    {
        /// <summary>
        /// 枚举类型
        /// </summary>
        public Type DefinedEnum { get; set; }

        /// <summary>
        /// 是否允许特殊值，等于MagicNumber时通过验证。常用于全选。
        /// </summary>
        public bool AllowMagicNumber { get; set; } = false;

        /// <summary>
        /// 特殊值
        /// </summary>
        public long MagicNumber { get; set; } = -1;

        /// <summary>
        /// 是否为位运算（true:位运算的校验方式；false:正常的校验方式(默认)）
        /// </summary>
        public bool IsShiftOperate { get; set; } = false;

        /// <summary>
        /// 特性验证
        /// </summary>
        /// <param name="definedEnum">枚举值</param>
        /// <param name="errorMessage">错误信息</param>
        public EnumDefined(Type definedEnum, string errorMessage) : base(ResponseCode.InvalidParameters, errorMessage)
        {
            DefinedEnum = definedEnum;
        }

        /// <summary>
        /// 校验参数是否合法
        /// </summary>
        /// <param name="value">需要校验的对象</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            EnsureEnumTypeValidate();

            if (value == null)
            {
                return true;
            }
            if (IsShiftOperate)
            {
                var obj = IsShiftOperateValid(value);
                return obj;
            }

            if (Enum.IsDefined(DefinedEnum, value))
            {
                return true;
            }
            if (!AllowMagicNumber) return false;
            long objVal;
            try
            {
                objVal = Convert.ToInt64(value);
            }
            catch (Exception)
            {
                return false;
            }

            return objVal == MagicNumber;
        }

        /// <summary>
        /// 位运算的校验方式
        /// </summary>
        /// <param name="value">需要校验的对象</param>
        /// <returns></returns>
        private bool IsShiftOperateValid(object value)
        {
            var enumUnderlyingType = DefinedEnum.GetEnumUnderlyingType();
            var enumVal = Convert.ToInt64(value);
            if (enumVal < 0) return false;

            var enumValue = enumVal;
            var pointerCount = 0;
            long suitabilityType = 0;

            do
            {
                long pointerValue = 1 << pointerCount;
                var obj = enumVal % 2 == 1 && Enum.IsDefined(DefinedEnum, Convert.ChangeType(pointerValue, enumUnderlyingType)) ? Enum.ToObject(DefinedEnum, pointerValue) : null;

                if (obj != null)
                {
                    suitabilityType += Convert.ToInt64(obj);
                }

                enumVal >>= 1;
                pointerCount++;
            } while (enumVal > 0);

            return enumValue == suitabilityType;
        }


        private void EnsureEnumTypeValidate()
        {
            if (!DefinedEnum.IsEnum)
            {
                throw new InvalidOperationException("枚举定义");
            }
        }
    }
}
