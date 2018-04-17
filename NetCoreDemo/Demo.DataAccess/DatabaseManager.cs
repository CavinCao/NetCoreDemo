using Demo.Common;
using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Demo.DataAccess
{
    /// <summary>
    /// 数据库管理对象
    /// </summary>
    public class DatabaseManager
    {
        /// <summary>
        /// OA主库名称
        /// </summary>
        public const string DBName = "mytest";


        /// <summary>
        /// OA主库连接字符串
        /// </summary>
        private static readonly string TEST_CONNECTION_STRING = AppSetting.GetConfig("ConnectionStrings:Test");


        /// <summary>
        /// OA主库表名称
        /// </summary>
        public struct Demo_Tables
        {
            /// <summary>
            /// 员工表
            /// </summary>
            public static readonly string Employee = $"`{DBName}`.`employee`";
           
        }

        

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <returns></returns>
        public static DbConnection GetConnection(string dbName)
        {
            if (string.IsNullOrWhiteSpace(dbName))
                throw new ArgumentNullException(nameof(dbName));

            return new MySqlConnection(TEST_CONNECTION_STRING);

        }

        /// <summary>
        /// 创建sql注释
        /// </summary>
        /// <param name="author">sql作者</param>
        /// <param name="sqlDesc">sql用途描述</param>
        /// <returns></returns>
        public static string GetSqlComments(string author, string sqlDesc)
        {
            return $"/*Author:{author}/For:{sqlDesc}*/";
        }
    }
}
