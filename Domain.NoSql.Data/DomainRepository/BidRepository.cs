using CustomLogger;
using Domain.NoSql.Data.DomainEntites;
using Domain.NoSql.Data.Provider;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.NoSql.Data.DomainRepository
{
    public interface IBidRepository
    {
        bool AddBid(Bid param);
        List<Bid> GetBids(Object projectId);
    }

    public class BidRepository : IBidRepository
    {
        protected readonly IMongoDatabase Database;
        IMongoCollection<Bid> _collection;

        public BidRepository()
        {
            Database = DatabaseProvider.GetDatabase();
            _collection = CollectionProvider.GetCollection<Bid>(Database);
        }
        public bool AddBid(Bid param)
        {
            try
            {
                _collection.InsertOne(param);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            return false;
        }
        public List<Bid> GetBids(Object projectId)
        {
            return _collection.Find(x => x.ProjectId == projectId).ToList();
        }
    }
}
