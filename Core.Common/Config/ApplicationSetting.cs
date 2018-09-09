using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Core.Common.Config
{
    public class ApplicationSetting
    {
        private static readonly IConfiguration Configuration;

        static ApplicationSetting()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            try
            {
                ConfigurationBuilder configBuilder = new ConfigurationBuilder();
                //configBuilder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                //            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                switch (environmentName.ToUpper())
                {
                    case "DEVELOPMENT":
                        {
                            configBuilder
                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true);
                            break;
                        }
                    case "STAGE":
                        {
                            configBuilder
                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile($"appsettings.Stage.json", optional: true, reloadOnChange: true);
                            break;
                        }
                    default:
                        {
                            configBuilder
                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);                            
                            break;
                        }
                }
                Configuration = configBuilder.Build();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string SqlDbString => Configuration["connectionstrings:sqlserver"];
        public static string MongoDbString => Configuration["connectionstrings:mongodb"];

        public static string LogPath => string.IsNullOrEmpty(Configuration["logpath"]) ? Path.Combine(ApplicationPath, "logs") : Configuration["logpath"];
        public static int ServiceUnavailableRetry => string.IsNullOrEmpty(Configuration["serviceunavailableretry"]) ? 5 : Int32.Parse(Configuration["serviceunavailableretry"]);
        public static int SqlMaxRetryCount => string.IsNullOrEmpty(Configuration["sql_max_retry_count"]) ? 5 : Int32.Parse(Configuration["sql_max_retry_count"]);
        public static string PropertyDocumentsPath => string.IsNullOrEmpty(Configuration["property_doc_path"]) ? Path.Combine(ApplicationPath, "docs") : Configuration["property_doc_path"];
        public static string HostUrl => string.IsNullOrEmpty(Configuration["hosturl"]) ? string.Empty : Configuration["hosturl"];


        public static MailSetup MailSetting
        {
            get
            {
                var mailSetup = new MailSetup();
                Configuration.GetSection("mailsetup").Bind(mailSetup);
                return mailSetup;
            }
        }

        public static string ApplicationPath => AppDomain.CurrentDomain.BaseDirectory;

    }


    #region MailSetting
    public class MailSetup
    {
        public string Server { get; set; }
        public bool IsSecure { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
    #endregion
}
