using Demo.Model.Base;
using Demo.Model.Base.ExceptionModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Demo.Model.Attributes
{
    /// <summary>
    /// 验证工具
    /// </summary>
    public class ValidateUtil
    {
        /// <summary>
        /// 反射标识
        /// </summary>
        private const BindingFlags MEMBER_INFO_BINDINGFLAGS = BindingFlags.Public | BindingFlags.Instance;

        /// <summary>
        /// 缓存反射成员
        /// </summary>
        private static readonly ConcurrentDictionary<Type, ValidationStorage> Storage = new ConcurrentDictionary<Type, ValidationStorage>();

        /// <summary>
        /// 检查对象是否有效，返回第一个验证失败的对象。
        /// </summary>
        /// <param name="obj">需要验证的类</param>
        /// <param name="result">[输出参数]，响应结果集</param>
        /// <returns></returns>
        public static bool IsValid(object obj, out ResponseResult result)
        {
            if (obj == null)
            {
                result = new ResponseResult { Result = false, Code = ResponseCode.ParametersRequired, ErrorMessage = "传入实体为null。" };
                return false;
            }

            var value = GetOrInsertMemberInfoValidationFromStorage(obj);

            foreach (var aMember in value.MemberInfos)
            {
                var memberVal = GetValueFromMemberInfo(aMember.MemberInfo, obj);
                foreach (var aAttribute in aMember.Attributes)
                {
                    if (!aAttribute.IsValid(memberVal))
                    {
                        result = aAttribute.BuildFailureResult();
                        return false;
                    }
                }
            }

            result = BuildSuccessResult();
            return true;
        }

        /// <summary>
        /// 检查对象是否有效，返回验证结果。
        /// </summary>
        /// <param name="obj">需要验证的类</param>
        /// <returns></returns>
        public static bool IsValid(object obj)
        {
            if (obj == null)
            {
                throw new ExceptionModel(ResponseCode.ParametersRequired, "传入实体为null。");
            }

            var value = GetOrInsertMemberInfoValidationFromStorage(obj);

            foreach (var aMember in value.MemberInfos)
            {
                var memberVal = GetValueFromMemberInfo(aMember.MemberInfo, obj);
                foreach (var aAttribute in aMember.Attributes)
                {
                    if (!aAttribute.IsValid(memberVal))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 从存储对象中获取成员验证对象集合
        /// </summary>
        /// <param name="obj">类型验证对象</param>
        /// <returns></returns>
        private static ValidationStorage GetOrInsertMemberInfoValidationFromStorage(object obj)
        {
            ValidationStorage retval;
            var type = obj.GetType();

            if (Storage.ContainsKey(type))
            {
                retval = Storage[type];
            }
            else
            {
                var objType = obj.GetType();
                retval = new ValidationStorage
                {
                    MemberInfos = new List<ValidationMemberInfo>()
                };

                // 取出所有字段和属性
                var memberInfos = objType.GetFields(MEMBER_INFO_BINDINGFLAGS).Select(m => (MemberInfo)m).ToList();
                memberInfos.AddRange(objType.GetProperties(MEMBER_INFO_BINDINGFLAGS));

                foreach (var aMemberInfo in memberInfos)
                {
                    // 包含验证特性基类，存入缓存中
                    var attrColls = aMemberInfo.GetCustomAttributes<Validation>().ToList();
                    if (attrColls.Any())
                    {
                        retval.MemberInfos.Add(new ValidationMemberInfo
                        {
                            Attributes = attrColls,
                            MemberInfo = aMemberInfo
                        });
                    }
                }
                Storage.AddOrUpdate(type, retval, (t, v) => retval);
            }
            return retval;
        }

        /// <summary>
        /// 从成员对象中获取值
        /// </summary>
        /// <param name="aMemberInfo">成员信息</param>
        /// <param name="obj">成员所属对象</param>
        /// <returns></returns>
        private static object GetValueFromMemberInfo(MemberInfo aMemberInfo, object obj)
        {
            if (aMemberInfo == null)
            {
                throw new ArgumentNullException(nameof(aMemberInfo), "成员对象不能为Null值。");
            }
            if ((aMemberInfo.MemberType & MemberTypes.Field) == 0 && (aMemberInfo.MemberType & MemberTypes.Property) == 0)
            {
                throw new InvalidOperationException("只能从字段或者属性中获取值。");
            }
            var memberInfo = aMemberInfo as PropertyInfo;
            if (memberInfo != null)
            {
                return memberInfo.GetValue(obj, null);
            }
            var info = aMemberInfo as FieldInfo;
            return info?.GetValue(obj);
        }

        /// <summary>
        /// 构造成功结果
        /// </summary>
        /// <returns></returns>
        private static ResponseResult BuildSuccessResult()
        {
            return null;
        }

        /// <summary>
        /// 验证存储
        /// </summary>
        private class ValidationStorage
        {
            /// <summary>
            /// 成员信息集合
            /// </summary>
            public List<ValidationMemberInfo> MemberInfos { get; set; }
        }

        /// <summary>
        /// 成员信息集合
        /// </summary>
        private class ValidationMemberInfo
        {
            /// <summary>
            /// 成员信息
            /// </summary>
            public MemberInfo MemberInfo { get; set; }

            /// <summary>
            /// 成员验证特性集合
            /// </summary>
            public List<Validation> Attributes { get; set; }
        }
    }
}
