using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Core.Common.Config;

namespace Domain.NoSql.Data.Provider
{
    public class DatabaseProvider
    {
        public static IMongoDatabase GetDatabase(string connectionStringName = "app")
        {
            var connectionStringSettings = ApplicationSetting.MongoDbString;
            if (connectionStringSettings == null)
            {
                // throw new NullReferenceException("A connectionString does not existe.");
                connectionStringSettings = "mongodb://midware:midware321123@ds149732.mlab.com:49732/midware";
            }
               
            var url = new MongoUrl(connectionStringSettings);
            var client = new MongoClient(url);
            return client.GetDatabase(url.DatabaseName);
        }
    }
}
