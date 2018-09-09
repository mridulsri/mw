using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Domain.NoSql.Data.Repository
{
    public interface IMongoRepository<T>
    {
        T Insert(T doc);
        Task<T> InsertAsync(T doc);
        IEnumerable<T> Insert(IEnumerable<T> docs);
        Task<IEnumerable<T>> InsertAsync(IEnumerable<T> docs);
        void Update(T doc);
        void Update(IEnumerable<T> docs);
        Task<bool> UpdateAsync(T doc);
        void Update(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition);
        void UpdateAsync(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition);
        void UpdateById(object id, UpdateDefinition<T> updateBuilder);
        T FindAndModifyById(object id, UpdateDefinition<T> updateDefinition);
        long DeleteById(object id);
        long DeleteOne(FilterDefinition<T> filter);
        void DeleteOneAsync(FilterDefinition<T> filter);
        long Delete(FilterDefinition<T> filter);
        void DeleteAsync(FilterDefinition<T> filter);
        long Count();
        long Count(FilterDefinition<T> filter);
        long Count(Expression<Func<T, bool>> expression);
        IQueryable<T> All();
        IQueryable<T> All(int page, int pageSize);
        IQueryable<T> All(Expression<Func<T, bool>> expression);
        IQueryable<T> All(Expression<Func<T, bool>> expression, int page, int pageSize);
        IEnumerable<T> FindAll();
        T FindOneById(object id);
        T FindOne(FilterDefinition<T> filter);
        T FindOne(Expression<Func<T, bool>> expression);
        IEnumerable<T> Find(FilterDefinition<T> filter);
        IQueryable<T> Find(Expression<Func<T, bool>> expression);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression, int page, int pageSize);
        T FindAndRemove(FilterDefinition<T> filter);
        void RemoveAll();
        void CreateIndex(IndexKeysDefinition<T> indexKeysDefinition);
        void CreateIndex(IndexKeysDefinition<T> indexKeysDefinition, CreateIndexOptions options);
        Task<List<BsonDocument>> GetIndexesAsync();
    }
}
