using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.NoSql.Data.Provider
{
    internal class QueryProvider
    {

        public static string RenderQuery<T>(FilterDefinition<T> filterDefinition) where T : TDocument
        {
            var query = string.Empty;
            try
            {
                IBsonSerializerRegistry registry = BsonSerializer.SerializerRegistry;
                IBsonSerializer<T> serializer = BsonSerializer.SerializerRegistry.GetSerializer<T>();
                FilterDefinitionBuilder<T> __subject = Builders<T>.Filter;

                 query = filterDefinition.Render(serializer, registry).ToString();
                
            }
            catch (Exception)
            {
             
            }
            return query;
        }

        public static string RenderQuery<T>(ProjectionDefinition<T> filterDefinition) where T : TDocument
        {
            var query = string.Empty;
            try
            {
                IBsonSerializerRegistry registry = BsonSerializer.SerializerRegistry;
                IBsonSerializer<T> serializer = BsonSerializer.SerializerRegistry.GetSerializer<T>();
                FilterDefinitionBuilder<T> __subject = Builders<T>.Filter;

                query = filterDefinition.Render(serializer, registry).ToString();
            }
            catch (Exception)
            {
            }
            return query;
        }

        public static string RenderQuery<T>(UpdateDefinition<T> filterDefinition) where T : TDocument
        {
            var query = string.Empty;
            try
            {
                IBsonSerializerRegistry registry = BsonSerializer.SerializerRegistry;
                IBsonSerializer<T> serializer = BsonSerializer.SerializerRegistry.GetSerializer<T>();
                FilterDefinitionBuilder<T> __subject = Builders<T>.Filter;

                query = filterDefinition.Render(serializer, registry).ToString();

            }
            catch (Exception)
            {

            }
            return query;
        }

    }
}
