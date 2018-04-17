using System;
using Dapper.Contrib.Extensions;
using Demo.Model.Attributes;

namespace Demo.Model
{
    /// <summary>
    /// 表说明：员工表
    /// </summary>
    [Table("employee")]
    public class EmployeeModel
    {
        /// <summary>
        /// 自增长ID
        /// </summary>
        [Key]
        public long Id { get; set; }


        /// <summary>
        /// 员工姓名
        /// </summary>
        [MaxLength(50, "员工姓名长度不能超过50。")]
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(20, "手机号长度不能超过20。")]
        public string Mobile { get; set; } = string.Empty;


        /// <summary>
        /// 性别
        /// </summary>
        [Range(0, 1, "性别值超出范围。")]
        public int Sex { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        [MaxLength(100, "头像长度不能超过100。")]
        public string HeadUrl { get; set; } = string.Empty;


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatorTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Required("创建人不可为空。")]
        [MaxLength(50, "创建人长度不能超过50。")]
        public string CreatorName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Required("更新人不可为空。")]
        [MaxLength(50, "更新人长度不能超过50。")]
        public string UpdateName { get; set; }

    }
}
