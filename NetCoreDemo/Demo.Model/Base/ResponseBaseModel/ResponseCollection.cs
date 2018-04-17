using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Model.Base
{
    /// <summary>
    /// 响应集合结构
    /// </summary>
    /// <typeparam name="T">服务契约单个实体</typeparam>
    public class ResponseCollection<T>
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<T> Collection { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ResponseCollection()
        {
            Collection = new List<T>();
        }
    }
}
