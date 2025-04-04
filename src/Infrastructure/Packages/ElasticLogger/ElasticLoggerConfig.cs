using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticLogger
{
    public static class ElasticLoggerConfig
    {
        public static WebApplicationBuilder CreateLoggerBuilder(this WebApplicationBuilder builder)
        {
            var url = builder.Configuration["Elasticsearch:Url"];
            if (url == null)
            {
                throw new Exception("Elasticsearch:Url is null");
            }
            var IndexFormat = builder.Configuration["Elasticsearch:IndexFormat"];
            if (IndexFormat == null)
            {
                throw new Exception("Elasticsearch:IndexFormat is null");
            }
            var Username = builder.Configuration["Elasticsearch:Username"];
            if (Username == null)
            {
                throw new Exception("Elasticsearch:Username is null");
            }
            var password = builder.Configuration["Elasticsearch:Password"];
            if (password == null)
            {
                throw new Exception("Elasticsearch:passwqord  is null");
            }

            LoggModel.ApplicationName = builder.Environment.ApplicationName;
            LoggModel.EnvironmentName = builder.Environment.EnvironmentName;
            LoggModel.ContentRootPath = builder.Environment.ContentRootPath;
            LoggModel.ApplicationVersion = builder.Configuration["Version"];
            LoggModel.InstanceId = builder.Configuration["InstanceId"];

            var logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(url))
        {
            IndexFormat = "IDP_logs-{0:yyyy.MM.DD}",
            AutoRegisterTemplate = true,
            ModifyConnectionSettings = x => x.BasicAuthentication(Username, password)
        })
        .CreateLogger();
            builder.Logging.AddSerilog(logger);

            Log.Logger = logger;
            return builder;
        }

    }
}
