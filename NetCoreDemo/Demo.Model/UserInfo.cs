using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Model
{
    /// <summary>
    /// 当前用户信息
    /// </summary>
    public class UserInfo
    {
       /// <summary>
       /// 用户实体
       /// </summary>
        public UserInfo()
        {
            Permissions = new List<PermissionDetail>();
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        public long EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public List<PermissionDetail> Permissions { get; private set; }
    }

    /// <summary>
    /// 权限详情数据对象
    /// </summary>
    public class PermissionDetail
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 员工Id
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        public long PermissionId { get; set; }

        /// <summary>
        /// 权限名称（一般指菜单名称或者超链接字符）
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// 权限所属的父权限ID
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 权限对应的功能URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 是否是菜单（若是菜单，一般会外显作为菜单条目）
        /// </summary>
        public bool IsMenu { get; set; }

        /// <summary>
        /// 支持的操作（位运算），1-增；2-删；4-改；8-查
        /// </summary>
        public long Verbs { get; set; }

        /// <summary>
        /// 排序值，越大越靠前
        /// </summary>
        public int Sort { get; set; }
    }
}
