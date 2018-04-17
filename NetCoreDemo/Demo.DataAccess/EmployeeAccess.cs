using Demo.Model;
using Demo.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Linq;
using Demo.Common;
using Demo.Model.Attributes;

namespace Demo.DataAccess
{
    /// <summary>
    /// 员工数据访问层
    /// </summary>
    public class EmployeeAccess
    {        
        /// <summary>
        /// 分页获取员工
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<ResponseCollection<EmployeeModel>> GetPaging(EmployeeFilterModel entity)
        {
            IEnumerable<EmployeeModel> employeeList = new List<EmployeeModel>();

            var sqlWhere = new StringBuilder();
            var sqlCount = new StringBuilder();
            var sqlQuery = new StringBuilder();

            sqlQuery.Append($@"SELECT {ParametersHelper.GetSelectParameters<EmployeeModel>()}
                               FROM  { DatabaseManager.Demo_Tables.Employee} ");

            sqlCount.Append($@"SELECT COUNT(1) FROM { DatabaseManager.Demo_Tables.Employee} ");

            sqlWhere.Append(" WHERE 1=1 ");

            /*具体的WHERE条件进行填充*/
            if (entity.Id > 0)
                sqlWhere.Append(" AND Id=@Id ");

            if (!string.IsNullOrWhiteSpace(entity.Name))
                sqlWhere.Append(" AND Name=@Name ");

            if (entity.Ids.Any())
                sqlWhere.Append(" AND Id in @Ids ");


            sqlCount.Append(sqlWhere);
            sqlCount.Append(DatabaseManager.GetSqlComments("Cavin", "获取员工"));

            if (!string.IsNullOrWhiteSpace(entity.Sortby))
                sqlWhere.Append(OrderConditionHelper.TransformOrderCondition<EmployeeModel>(entity.Sortby, entity.Orderby));
            if (entity.Limit > 0)
                sqlWhere.Append($" LIMIT {entity.Offset},{entity.Limit} ");

            sqlQuery.Append(sqlWhere);
            sqlQuery.Append(DatabaseManager.GetSqlComments("Cavin", "获取员工数量"));


            long totalCount = 0;

            try
            {
                using (var conn = DatabaseManager.GetConnection(DatabaseManager.DBName))
                {
                    await conn.OpenAsync();

                    if (entity.Limit >= 0)
                        totalCount = await conn.ExecuteScalarAsync<long>(sqlCount.ToString(), entity);

                    if (entity.Limit == 0)
                        return new ResponseCollection<EmployeeModel>() { TotalCount = totalCount };

                    employeeList = await conn.QueryAsync<EmployeeModel>(sqlQuery.ToString(), entity);

                    if (employeeList == null || !employeeList.Any())
                        return new ResponseCollection<EmployeeModel>() { TotalCount = totalCount };

                    if (entity.Limit == -1)
                        totalCount = employeeList.Count();

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return new ResponseCollection<EmployeeModel>() { Collection = employeeList, TotalCount = totalCount };

        }

        /// <summary>
        /// 根据Id获取员工
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<EmployeeModel> GetById(long id)
        {
            var result = new ResponseResult();
            try
            {
                using (var conn = DatabaseManager.GetConnection(DatabaseManager.DBName))
                {
                    await conn.OpenAsync();
                    return await conn.GetAsync<EmployeeModel>(id);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 新增员工
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<ResponseResult> Post(EmployeeModel entity)
        {
            var result = new ResponseResult();
            try
            {
                entity.CreatorTime = entity.UpdateTime = DateTime.Now;
                using (var conn = DatabaseManager.GetConnection(DatabaseManager.DBName))
                {
                    await conn.OpenAsync();
                    await conn.InsertAsync(entity);
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult { Result = false, Code = ResponseCode.UnknownException, ErrorMessage = ex.Message };
            }
            return result;
        }

        /// <summary>
        /// 修改员工
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<ResponseResult> Put(EmployeeModel entity)
        {
            var result = new ResponseResult();
            try
            {
              
                using (var conn = DatabaseManager.GetConnection(DatabaseManager.DBName))
                {
                    await conn.OpenAsync();
                    await conn.UpdateAsync(entity);
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult { Result = false, Code = ResponseCode.UnknownException, ErrorMessage = ex.Message };
            }
            return result;
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static async Task<ResponseResult> Delete(long id)
        {
            var result = new ResponseResult();
            try
            {
                using (var conn = DatabaseManager.GetConnection(DatabaseManager.DBName))
                {
                    await conn.OpenAsync();
                    await conn.DeleteAsync(new EmployeeModel { Id = id });
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult { Result = false, Code = ResponseCode.UnknownException, ErrorMessage = ex.Message };
            }
            return result;
        }        

    }
}
