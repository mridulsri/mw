using Domain.NoSql.Data.DomainEntites;
using Domain.NoSql.Data.Exceptions;
using Domain.NoSql.Data.Provider;
using Domain.NoSql.Data.Repository;
using Domain.NoSql.Data.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.NoSql.Data.DomainRepository
{
    public interface IPropertyDetailsRepository
    {
        PropertyDetail FindByIgdId(string county,string igdId);
        PropertyDetail FindByParcelId(string state, string county, string parcelId);
    }
    
    public class PropertyDetailsRepository : MongoRepository<PropertyDetail>, IPropertyDetailsRepository
    {
        protected readonly IMongoDatabase _database;
        private const string _current = "_current";
        private const string _backup = "_backup";

        public PropertyDetailsRepository()
        {
            _database = DatabaseProvider.GetDatabase();
        }

        public PropertyDetailsRepository(string state)
        {
            _database = DatabaseProvider.GetDatabase(state.ToLower());
        }
        
        public PropertyDetail FindByIgdId(string county, string igdId)
        {
            try
            {
                if (string.IsNullOrEmpty(county) ||string.IsNullOrEmpty(igdId))
                    throw new ArgumentNullException(igdId);

                var collectionName = county.ToLower() + _current;
                IMongoCollection<PropertyDetail> collection = CollectionProvider.GetCollection<PropertyDetail>(_database, collectionName);

                var filter = Builders<PropertyDetail>.Filter.Eq("igdid", igdId.ToUpper());

                string query = filter.ToString();
                var json = filter.RenderToBsonDocument().ToJson();

                var doc = collection.Find<PropertyDetail>(filter).FirstOrDefault();

                return doc ?? null;

            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public PropertyDetail FindByParcelId(string state,string county, string parcelId)
        {
            try
            {
                if (string.IsNullOrEmpty(state) || string.IsNullOrEmpty(county) || string.IsNullOrEmpty(parcelId))
                    throw new ArgumentNullException(parcelId);

                var collectionName = county.ToLower() + _current;
                IMongoCollection<PropertyDetail> collection = CollectionProvider.GetCollection<PropertyDetail>(Database, _current);

                var filter = Builders<PropertyDetail>.Filter.Eq("parcelid", parcelId);

                var doc = collection.Find(filter).FirstOrDefault();

                return doc ?? null;

            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }
    }
}
