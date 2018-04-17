using Demo.Model.Base;
using Demo.Model.Base.ResponseBaseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NetCoreDemo.Controllers
{
    [Route("api/[controller]")]
    public class FileController : BaseController
    {
        //基本路径
        private readonly string BASEFILE = "upload_file";


        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="fileOwner"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("download")]
        public async Task<IActionResult> Get(string fileOwner, string fileName)
        {
            try
            {
                var addrUrl = Path.Combine(Directory.GetCurrentDirectory(), BASEFILE, $@"{fileOwner}", $@"{fileName}");
                FileStream fs = new FileStream(addrUrl, FileMode.Open);
                return File(fs, "application/vnd.android.package-archive", fileName);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="fileOwner"></param>
        /// <param name="files"></param>
        /// <param name="mD5Codes"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        //[DisableRequestSizeLimit] //禁用http限制大小
        [RequestSizeLimit(100 * 1024 * 1024)] //限制http大小
        public async Task<IActionResult> Post([FromHeader]string fileOwner, List<IFormFile> files)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileOwner))
                    return AssertNotFound(new ResponseFileResult { Result = false, Code = ResponseCode.InvalidParameters, ErrorMessage = "文件归属不能为空" });
                if (files == null || !files.Any())
                    return AssertNotFound(new ResponseFileResult { Result = false, Code = ResponseCode.InvalidParameters, ErrorMessage = "附件不能为空" });


                string filePath = Path.Combine(Directory.GetCurrentDirectory(), BASEFILE, $@"{fileOwner}");
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                var result = new ResponseFileResult();
                var fileList = new List<FileResultModel>();

                foreach (var file in files)
                {
                    var fileModel = new FileResultModel();
                    var fileName = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');
                    var newName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                    var filefullPath = Path.Combine(filePath, $@"{newName}");

                    using (FileStream fs = new FileStream(filefullPath, FileMode.Create))//System.IO.File.Create(filefullPath)
                    {
                        await file.CopyToAsync(fs);
                        fs.Flush();
                    }

                    fileList.Add(new FileResultModel { Name = fileName, Size = file.Length, Url = $@"/file/download?fileOwner={fileOwner}&fileName={newName}"});
                }
                result.FileResultList = fileList;
                return AssertNotFound(result);
            }
            catch (Exception ex)
            {
                return AssertNotFound(new ResponseFileResult { Result = false, Code = ResponseCode.UnknownException, ErrorMessage = ex.Message });
            }
        }
    }
}
