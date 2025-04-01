using static System.Formats.Asn1.AsnWriter;
using System.Diagnostics;
using AspectInjector.Broker;
using Newtonsoft.Json;
using Serilog.Core;
using Serilog;
using Microsoft.Extensions.DependencyInjection;

using System.ComponentModel;
using System.Xml.Linq;
using System.Numerics;

namespace ElasticLogger
{
    [Aspect(AspectInjector.Broker.Scope.Global)]
    [Injection(typeof(LogCall))]
    public class LogCall : Attribute
    {

        internal static AsyncLocal<LoggModel> LoggLocalModel { get; set; }

        [Advice(Kind.Before)] // you can have also After (async-aware), and Around(Wrap/Instead) kinds
        public void LogEnter([Argument(Source.Type)] Type type, [Argument(Source.Name)] string name, [Argument(Source.Arguments)] object[] objcts)
        {


            if (!name.Contains(".ctor"))
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                WriteLog(type.FullName + "." + name, LogFunctionType.CallingMethod, LogType.Trace, JsonConvert.SerializeObject(objcts, settings));
            }
        }
        [Advice(Kind.Around, Targets = Target.AnyAccess)] // you can have also After (async-aware), and Around(Wrap/Instead) kinds
        public object Trace(
       [Argument(Source.Type)] Type type,
       [Argument(Source.Name)] string name,
       [Argument(Source.Target)] Func<object[], object> methodDelegate,
       [Argument(Source.Arguments)] object[] args)
        {


            var sw = Stopwatch.StartNew();
            var result = methodDelegate(args);
            sw.Stop();
            if (!type.FullName.Contains(".ctor"))
            {
                WriteLog(type.Name + "." + name, LogFunctionType.TimeInMethod, LogType.Trace, $"Time Execute {sw.ElapsedMilliseconds} ms");
            }
            return result;

        }
        [Advice(Kind.After)] // you can have also After (async-aware), and Around(Wrap/Instead) kinds
        public void LogOut([Argument(Source.Type)] Type type, [Argument(Source.Name)] string name, [Argument(Source.ReturnValue)] object objcts)
        {

            if (!name.Contains(".ctor"))
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                //Log.Information($"Out '{type.FullName}.{name}'( " + JsonConvert.SerializeObject(objcts) + " )");   //you can debug it	
                WriteLog(type.FullName + "." + name, LogFunctionType.ResultMethod, LogType.Trace, JsonConvert.SerializeObject(objcts, settings));
            }
        }
        public static void LogException(Exception ex)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var exJson = JsonConvert.SerializeObject(ex, settings);
            Log.ForContext(nameof(LoggModel.RequestId), LoggLocalModel.Value.RequestId)
                      .ForContext(nameof(LoggModel.ApplicationName), LoggModel.ApplicationName)
                      .ForContext(nameof(LoggModel.ApplicationVersion), LoggModel.ApplicationVersion)
                      .ForContext(nameof(LoggModel.InstanceId), LoggModel.InstanceId)
                      .ForContext(nameof(LoggModel.ContentRootPath), LoggModel.ContentRootPath)
                      .ForContext(nameof(LoggModel.EnvironmentName), LoggModel.EnvironmentName)
                      .ForContext(nameof(LoggModel.DateTime), DateTime.Now)
                      .Error(exJson);
        }
        private void WriteLog(string address, LogFunctionType logFunctionType, LogType logType, string argument)
        {

            if (LoggLocalModel is null) { return; }
            LoggLocalModel.Value.Address = address;
            LoggLocalModel.Value.LogFunction = logFunctionType;
            LoggLocalModel.Value.LogType = logType;
            LoggLocalModel.Value.DateTime = DateTime.Now;
            LoggLocalModel.Value.Argument = argument;
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            Log
                .ForContext(nameof(LoggModel.RequestId), LoggLocalModel.Value.RequestId)
                .ForContext(nameof(LoggModel.ApplicationName), LoggModel.ApplicationName)
                .ForContext(nameof(LoggModel.ApplicationVersion), LoggModel.ApplicationVersion)
                .ForContext(nameof(LoggModel.InstanceId), LoggModel.InstanceId)
                .ForContext(nameof(LoggModel.ContentRootPath), LoggModel.ContentRootPath)
                .ForContext(nameof(LoggModel.EnvironmentName), LoggModel.EnvironmentName)
                      //      .ForContext(nameof(LoggModel.DateTime), LoggLocalModel.Value.DateTime)
                      //      .ForContext(nameof(LoggModel.LogFunction), LoggLocalModel.Value.LogFunction)
                      //      .ForContext(nameof(LoggModel.LogType), LoggLocalModel.Value.LogType)
                      //      .ForContext(nameof(LoggModel.Address), LoggLocalModel.Value.Address)
                      //      .ForContext(nameof(LoggModel.Argument), LoggLocalModel.Value.Argument)
                      .Information(JsonConvert.SerializeObject(LoggLocalModel.Value, settings));
        }
        public static void SetManualLog(string address, LogType logType, string argument)
        {
            var callLog = new LogCall();
            callLog.WriteLog(address, LogFunctionType.CustomInMethod, logType, argument);
        }

    }


}
