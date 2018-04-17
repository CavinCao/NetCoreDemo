using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Common
{
    /// <summary>
    /// 字段自动生成帮助类
    /// </summary>
    public class ParametersHelper
    {
        /// <summary>
        /// 获取查询的字段列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetSelectParameters<T>(string tag = "")
        {
            var result = new StringBuilder();
            var propertyInfo = typeof(T).GetProperties();

            foreach (var info in propertyInfo)
            {
                result.Append($@"{tag}`{info.Name}`,");
            }
            return result.ToString().TrimEnd(',');
        }


        /// <summary>
        /// 获取要更新的字符串列表
        /// </summary>
        /// <typeparam name="T">DataTransferObject</typeparam>
        /// <param name="t">实体</param>
        /// <returns></returns>
        public static string GetUpdateParameters<T>(T t)
        {
            var result = new StringBuilder();
            var propertyInfo = t.GetType().GetProperties();

            foreach (
                var info in
                    propertyInfo.Where(
                        info => info.GetValue(t, null) != null))
            {
                result.AppendFormat(@"{0}=@{0},", info.Name);
            }
            return result.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 获取要更新的字符串列表
        /// </summary>
        /// <typeparam name="T">DataTransferObject</typeparam>
        /// <param name="t">实体</param>
        /// <param name="outOfColumns">[可选参数]，忽略属性列表</param>
        /// <returns></returns>
        public static string GetUpdateParameters<T>(T t, List<string> outOfColumns)
        {
            var result = new StringBuilder();
            var propertyInfo = t.GetType().GetProperties();

            foreach (
                var info in
                    propertyInfo.Where(
                        info => info.GetValue(t, null) != null && !outOfColumns.Contains(info.Name)))
            {
                result.AppendFormat(@"{0}=@{0},", info.Name);
            }
            return result.ToString().TrimEnd(',');
        }
    }
}
