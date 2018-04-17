using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Model.Attributes
{
    /// <summary>
    /// 排序方式
    /// </summary>
    public enum Orderby
    {
        /// <summary>
        /// 正序
        /// </summary>
        [JsonProperty("asc")]
        Asc,

        /// <summary>
        /// 倒序
        /// </summary>
        [JsonProperty("desc")]
        Desc
    }
}
