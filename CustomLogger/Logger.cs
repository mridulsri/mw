using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace CustomLogger
{
    public static class Logger
    {
        #region DataMembers

        private static readonly ILogger Log;

        #endregion

        #region Class Initializer

        static Logger()
        {
           // var logFilePath = Path.Combine(!string.IsNullOrEmpty(ConfigurationManager.AppSettings["app.log.path"]) ? ConfigurationManager.AppSettings["hunters.log.path"] : AppDomain.CurrentDomain.BaseDirectory, "logs/app", "log-{Date}.log");
            string logFilePath = Path.Combine(Core.Common.Config.ApplicationSetting.LogPath, "app", "log-{Date}.log");
            Log = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .WriteTo.RollingFile(logFilePath, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
                .CreateLogger();
        }

        #endregion

        #region ILogger Members

        public static void EnterMethod(string methodName)
        {
            // if(Log.IsInfoEnabled)
            Log.Information(string.Format(CultureInfo.InvariantCulture, "Entering Method {0}", methodName));

        }

        public static void LeaveMethod(string methodName)
        {
            Log.Information(string.Format(CultureInfo.InvariantCulture, "Leaving Method {0}", methodName));
        }

        //Log Exception
        public static void LogException(Exception ex)
        {
            LogExceptionImpl(ex, null, GetMethodName(ex));
        }
        public static void LogException(Exception ex, string message)
        {
            LogExceptionImpl(ex, message, GetMethodName(ex));
        }
        public static void LogException(Exception ex, string messageFormat, params object[] args)
        {
            string message = FormatMessage(messageFormat, args);
            LogExceptionImpl(ex, message, GetMethodName(ex));
        }

        //Log Message
        public static void LogInfo(string message)
        {
            Log.Information(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }
        public static void LogInfo(string messageFormat, params object[] args)
        {
            string message = FormatMessage(messageFormat, args);
            LogInfo(message);
        }
        //Log Error
        public static void LogError(string message)
        {
            Log.Error(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }
        public static void LogError(string messageFormat, params object[] args)
        {
            var message = FormatMessage(messageFormat, args);
            LogError(message);
        }

        //Log Worming
        public static void LogWarning(string message)
        {
            Log.Warning(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }

        public static void LogWarning(string messageFormat, params object[] args)
        {
            var message = FormatMessage(messageFormat, args);
            LogWarning(message);
        }
        //Log Debug
        public static void LogDebug(string message)
        {
            // if (Log.IsDebugEnabled)
            Log.Debug(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }
        public static void LogDebug(string messageFormat, params object[] args)
        {
            var message = FormatMessage(messageFormat, args);
            LogDebug(message);
        }

        //Log Fatal
        public static void LogFatal(string message)
        {
            // if (Log.IsDebugEnabled)
            Log.Debug(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }
        public static void LogFatal(string messageFormat, params object[] args)
        {
            var message = FormatMessage(messageFormat, args);
            LogFatal(message);
        }


        #endregion

        #region private methods
        private static void LogExceptionImpl(Exception ex, string message, string methodName)
        {
            LogErrorImpl(FormatException(ex, message));
        }
        private static void LogErrorImpl(string message)
        {
            Log.Error(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }

        private static string GetMethodName(Exception ex)
        {
            var stackTrace = new StackTrace(ex, true);
            var stackFrame = stackTrace.GetFrames()[2];
            return stackFrame.GetMethod().Name;
        }

        private static string FormatMessage(string messageFormat, params object[] args)
        {
            try
            {
                return string.Format(messageFormat, args);
            }
            catch (Exception)
            {
                return messageFormat;
            }
        }
        private static string FormatException(Exception ex, string message)
        {
            try
            {
                return string.Format("{4}:\r\n{0}\r\n{1}: {2}\r\n{3}", message, ex.GetType(), ex.Message, ex.StackTrace, System.Environment.MachineName);
            }
            catch (Exception)
            {
                return System.Environment.MachineName + ":\r\n(Unknown exception) " + message;
            }
        }
        #endregion
    }
}
