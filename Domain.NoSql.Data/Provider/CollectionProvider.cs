
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.NoSql.Data.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Domain.NoSql.Data.Provider
{
    // public class CollectionProvider //<T> where T : Document
    public class CollectionProvider //<T> where T : TDocument
    {

        static CollectionProvider()
        {
            ConventionPack pack = new ConventionPack
            {
                new IgnoreIfNullConvention(true),
                new LowerCaseElementNameConvention()
            };


            //ConventionRegistry.Register("DataProject Catalog Conventions", pack, type => type == typeof(T));
            ConventionRegistry.Register("DataProject Catalog Conventions", pack, type => true);

        }

        //public static IMongoCollection<T> GetCollection<T>(IMongoDatabase database) where T : Document
        //{
        //    var collectionName = CollectionAttribute.GetCollectionName<T>();
        //    return database.GetCollection<T>(collectionName);
        //}

        public static IMongoCollection<T> GetCollection<T>(IMongoDatabase database) where T : TDocument
        {
            //var collectionName = MongoCollectionNameAttribute.GetCollectionName<T>();
            //return database.GetCollection<T>(collectionName);
            return database.GetCollection<T>(typeof(T).Name.ToLower());

        }

        public static IMongoCollection<T> GetCollection<T>(IMongoDatabase database, string collectionName) where T : TDocument
        {
            return database.GetCollection<T>(collectionName.ToLower());
        }
    }

    /// <summary>
    /// Custom key name (conversion in lowercase) ConventionPack for MongoDB
    /// </summary>
    public class LowerCaseElementNameConvention : IMemberMapConvention
    {
        public void Apply(global::MongoDB.Bson.Serialization.BsonMemberMap memberMap)
        {
            memberMap.SetElementName(memberMap.MemberName.ToLower());
        }

        public string Name
        {
            get { throw new NotImplementedException(); }

        }


    }
}
