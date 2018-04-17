using Demo.Business;
using Demo.Common;
using Demo.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreDemo.Controllers
{
    /// <summary>
    /// 员工服务
    /// </summary>
    [Route("api/[controller]")]
    public class EmployeeController : BaseController
    {
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpGet("Export")]
        public async Task<IActionResult> GetExcel(EmployeeFilterModel entity)
        {
            var result = await EmployeeServices.GetPaging(entity);
            var columns = new Dictionary<string, string>() {
                { "Id","员工Id"},
                { "Name","员工姓名"},
                { "Mobile","手机号"},
                { "CreatorTime","创建时间"},
                { "CreatorName","创建人"},
            };
            var fs = ExcelHelper.GetByteToExportExcel(result.Collection.ToList(), columns, new List<string>());
            return File(fs, "application/vnd.android.package-archive", $"[{DateTime.Now.ToString("yyyy-MM-dd")}]员工信息.xlsx");
        }
        /// <summary>
        /// 分页获取员工信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPaging(EmployeeFilterModel entity)
        {
            return AssertNotFound(await EmployeeServices.GetPaging(entity));
        }
        /// <summary>
        /// 获取员工信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return AssertNotFound(await EmployeeServices.Get(id));
        }

        /// <summary>
        /// 新增员工
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EmployeeModel entity)
        {
            return AssertNotFound(await EmployeeServices.Post(entity));
        }

        /// <summary>
        /// 修改员工
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody]EmployeeModel entity)
        {
            entity.Id = id;
            return AssertNotFound(await EmployeeServices.Put(entity));
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            return AssertNotFound(await EmployeeServices.Delete(id));
        }
    }

}
