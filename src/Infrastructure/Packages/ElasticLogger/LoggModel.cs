using Microsoft.Extensions.FileProviders;

namespace ElasticLogger
{
    internal class LoggModel
    {
        public string RequestId { get; set; }
        public static string InstanceId { get; set; }
        public static string ApplicationName { get; set; }
        public static string EnvironmentName { get; set; }
        public static string ApplicationVersion { get; set; }
        public DateTime DateTime { get; set; }
        public LogType LogType { get; set; }
        public LogFunctionType LogFunction { get; set; }
        public static string ContentRootPath { get; internal set; }
        public string Address { get; set; }
        public string Argument { get; set; }
    }
    internal enum LogFunctionType
    {
        CallingMethod,
        TimeInMethod,
        ResultMethod,
        CustomInMethod
    }
    public enum LogType
    {
        Information,
        Data,
        Exception,
        NotImplemented,
        Trace,
        Curl,
    }


}
