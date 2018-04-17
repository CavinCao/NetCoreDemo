using Demo.DataAccess;
using Demo.Model;
using Demo.Model.Attributes;
using Demo.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Business
{
    /// <summary>
    ///员工Services层
    /// </summary>
    public class EmployeeServices
    {

        /// <summary>
        /// 分页查询员工
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<ResponseCollection<EmployeeModel>> GetPaging(EmployeeFilterModel entity)
        {
            return await EmployeeAccess.GetPaging(entity);
        }
        /// <summary>
        /// 查询员工
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<EmployeeModel> Get(long id)
        {
            return await EmployeeAccess.GetById(id);
        }
        /// <summary>
        /// 新增员工
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<ResponseResult> Post(EmployeeModel entity)
        {
            ResponseResult result;

            if (!ValidateUtil.IsValid(entity, out result))
            {
                return result;
            }

            return await EmployeeAccess.Post(entity);
        }

        /// <summary>
        /// 修改员工
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<ResponseResult> Put(EmployeeModel entity)
        {
            ResponseResult result;

            if (!ValidateUtil.IsValid(entity, out result))
            {
                return result;
            }

            return await EmployeeAccess.Put(entity);
        }
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<ResponseResult> Delete(long id)
        {
            if (id <= 0)
                return new ResponseResult { Result = false, ErrorMessage = "未传入主键或主键非法", Code = ResponseCode.NeedsKeyParameter };
            return await EmployeeAccess.Delete(id);
        }
    }
}
