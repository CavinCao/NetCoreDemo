using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Model.Base.FilterModel
{
    /// <summary>
    /// 基础搜索/分页实体
    /// </summary>
    public class BaseSearchModel : BasePagingModel
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sortby { get; set; } = null;

        /// <summary>
        /// 排序方式
        /// </summary>
        public string Orderby { get; set; } = null;
    }
}
