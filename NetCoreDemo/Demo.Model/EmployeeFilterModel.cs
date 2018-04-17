using Demo.Model.Base.FilterModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Model
{
    /// <summary>
    /// 员工筛选实体
    /// </summary>
    public class EmployeeFilterModel : BaseSearchModel
    {
        /// <summary>
        /// 自增长ID
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// 员工Id集合
        /// </summary>
        public List<long> Ids { get; set; } = new List<long>();


        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Name { get; set; } = string.Empty;

    }
}
