using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Demo.Model.Base.FilterModel
{
    /// <summary>
    /// 基本分页实体
    /// </summary>
    public class BasePagingModel
    {
        /// <summary>
        /// 最大限制单页数据量
        /// </summary>
        protected int MAX_LIMIT_COUNT { get; set; } = 100000;

        private int _pageSize = 10;

        private int _pageIndex = 1;

        /// <summary>
        /// 页码
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "页码数值应大于等于0！")]
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value < 1 ? 1 : value; }
        }
        /// <summary>
        /// 单页数量
        /// </summary>
        [Range(-1, int.MaxValue, ErrorMessage = "单页数值应大于等于0！")]
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }


        /// <summary>
        /// 单页数据量
        /// </summary>
        /// <remarks>
        /// 若 Limit 字段传值为 0 时，则只查询数量并赋值 TotalCount，不再查询结果集；
        /// 若 limit 字段传值为 -1 时，只查询结果集，不在单独查询数量 ToatalCount 直接取结果集总数量;
        /// </remarks>
        public int Limit
        {
            get { return PageSize < -1 || PageSize > MAX_LIMIT_COUNT ? MAX_LIMIT_COUNT : PageSize; }
        }

        /// <summary>
        /// 偏移量
        /// </summary>
        public int Offset
        {
            get { return PageIndex <= 0 ? 0 : (PageIndex - 1) * PageSize; }
        }
    }
}
