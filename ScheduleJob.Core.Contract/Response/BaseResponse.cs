using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleJob.Core.Contract.Response
{
    /// <summary>
    /// 通用的Response
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 请求状态，catch异常时Status=false，由框架控制
        /// </summary>
        public bool Status { get; private set; } = true;
        /// <summary>
        /// 业务错误代码，Status=true时前端会判断Code值(200=成功)
        /// </summary>
        public int Code { set; get; } = 200;
        /// <summary>
        /// 程序/业务错误信息，发生错误的说明
        /// </summary>
        public string Msg { set; get; } = "";
        /// <summary>
        /// 接口的一些耗时，ip等信息
        /// </summary>
        public string ServerInfo { set; get; } = "";

        /// <summary>
        /// 此方法由框架调用，开发过程中不要调用此方法
        /// </summary>
        /// <param name="st"></param>
        public void SetResponseStatus(bool st)
        {
            Status = st;
        }
    }
    public class BaseResponse<T> : BaseResponse
    {
        /// <summary>
        /// 通用数据类型
        /// </summary>
        public T Data { set; get; }
    }
}
