using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticLogger
{
    public class LoggMiddelware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (LogCall.LoggLocalModel is null)
            {
                LogCall.LoggLocalModel = new AsyncLocal<LoggModel>();
            }
            if (LogCall.LoggLocalModel.Value is null)
            {
                LogCall.LoggLocalModel.Value = new LoggModel();
            }
            LogCall.LoggLocalModel.Value.RequestId = context.TraceIdentifier;
            await next(context);
        }
    }
}
