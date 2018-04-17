using System;
namespace Demo.Model.Base
{

    public class ResponseEntity
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// 返回编码
        /// </summary>
        public ResponseCode Code { get; set; } = ResponseCode.Success;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 返回结果（有的话）
        /// </summary>
        public string Result { get; set; }
    }
    /// <summary>
    /// 服务返回响应实体
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 操作结果：成功为True 否则为False
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; } = true;

        /// <summary>
        /// 错误消息
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 响应码
        /// </summary>
        /// <value>The code.</value>
        public ResponseCode Code { get; set; } = ResponseCode.Success;

        /// <summary>
        /// 新增返回自增长Id
        /// </summary>
        public long AutoId { get; set; }
    }
    /// <summary>
    /// 响应码集合
    /// </summary>
    public enum ResponseCode
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 未知错误类型
        /// </summary>
        UnknownException = 10000,

        /// <summary>
        /// 未传入主键或主键非法
        /// </summary>
        NeedsKeyParameter = 10001,

        /// <summary>
        /// 相关数据已经存在
        /// </summary>
        IsExistsValue = 10005,

        /// <summary>
        /// 相关数据不存在
        /// </summary>
        NotExistsValue = 10006,

        /// <summary>
        /// 暂不支持此操作
        /// </summary>
        NotSupported = 10007,

        /// <summary>
        /// 存在未传入的必传参数
        /// </summary>
        ParametersRequired = 10010,

        /// <summary>
        /// 传入参数存在问题
        /// </summary>
        InvalidParameters = 10011,

        /// <summary>
        /// 传入参数超出范围
        /// </summary>
        OutOfRangeParameters = 10012,

        /// <summary>
        /// 取消失败
        /// </summary>
        CancelNotValue = 10003,

        /// <summary>
        /// 未授权
        /// </summary>
        Unauthorized=40001,

        /// <summary>
        /// 没有指定权限
        /// </summary>
        PermissionDenied=40002
    }
}
