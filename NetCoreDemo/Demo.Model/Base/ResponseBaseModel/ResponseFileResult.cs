using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Model.Base.ResponseBaseModel
{
    /// <summary>
    /// 上传附件反馈实体
    /// </summary>
    public class ResponseFileResult : ResponseResult
    {
        public List<FileResultModel> FileResultList { get; set; }
    }

    public class FileResultModel
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 下载url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// MD5编码
        /// </summary>
        public string MD5Code { get; set; }
    }
}
