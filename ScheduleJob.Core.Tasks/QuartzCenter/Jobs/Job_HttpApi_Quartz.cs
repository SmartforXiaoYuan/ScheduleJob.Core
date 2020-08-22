using Newtonsoft.Json;
using Quartz;
using ScheduleJob.Core.IServices;
using ScheduleJob.Core.Utility;
using ScheduleJob.Core.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScheduleJob.Core.Tasks.QuartzCenter.Jobs
{
    /// <summary>
    /// 专门用来请求外部的api Job
    /// </summary>
    public class Job_HttpApi_Quartz : JobBase, IJob
    {

        ITasksQzServices _tasksQzServices;
        public Job_HttpApi_Quartz(ITasksQzServices tasksQzServices)
        {
            _tasksQzServices = tasksQzServices;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            DateTime dateTime = DateTime.Now;
            var jobKey = context.JobDetail.Key;
            var jobId = jobKey.Name;
            var model = await _tasksQzServices.QueryById(jobId.ObjToInt());
            var executeLog = await ExecuteJob(context, async () => await Run(context, jobId.ObjToInt()));
        }
        public async Task Run(IJobExecutionContext context, int jobid)
        {
            if (jobid > 0)
            {
                var model = await _tasksQzServices.QueryById(jobid);
                if (model != null)
                {
                    if (model.IsApiUrl == (int)BoolStatus.True)
                    {
                        if (model.MethodType == 1) //model.MethodType?.ToUpper() == "GET"
                        {
                            var rep = await HttpUtil.HttpGetAsync(model.ApiUrl);
                        }
                        else
                        {
                            string json = JsonConvert.SerializeObject(model.RequestValue);
                            var postData = JsonDateTimeFormat(json); //JsonHelper.ToJson(model.RequestValue);
                            var rep = await HttpUtil.HttpPostAsync(model.ApiUrl, postData, null, 30000);
                        }
                    }
                    model.RunTimes += 1;
                    var separator = "<br>";
                    model.Remark =
                        $"【{DateTime.Now}】执行任务【Id：{context.JobDetail.Key.Name}，组别：{context.JobDetail.Key.Group}】【执行成功】{separator}"
                        + string.Join(separator, StringHelper.GetTopDataBySeparator(model.Remark, separator, 9));
                    await _tasksQzServices.Update(model);
                }
            }
        }

        /// <summary>
        /// 处理Json的时间格式为正常格式
        /// </summary>
        public   string JsonDateTimeFormat(string json)
        {
            //json.CheckNotNullOrEmpty("json");
            json = Regex.Replace(json,
                @"\\/Date\((\d+)\)\\/",
                match =>
                {
                    DateTime dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                });
            return json;
        }

    }
}
