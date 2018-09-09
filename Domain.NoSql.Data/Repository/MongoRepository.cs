using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Domain.NoSql.Data.Exceptions;
using Domain.NoSql.Data.Provider;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace Domain.NoSql.Data.Repository
{
    public class MongoRepository<T> : IMongoRepository<T> where T : TDocument
    {
        protected readonly IMongoCollection<T> Collection;
        protected readonly IMongoDatabase Database;

        public MongoRepository() : this(DatabaseProvider.GetDatabase())
        {
            Database = DatabaseProvider.GetDatabase();
        }

        public MongoRepository(string state) : this(DatabaseProvider.GetDatabase(state))
        {
            Database = DatabaseProvider.GetDatabase(state);
        }

        public MongoRepository(IMongoDatabase database)
        {
            Collection = CollectionProvider.GetCollection<T>(database);
            Database = database;
        }

        public T Insert(T doc)
        {
            try
            {
                if (doc == null) throw new ArgumentNullException(nameof(doc));
                Collection.InsertOne(doc);
            }
            catch (MongoWriteConcernException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
            return doc;
        }

        public async Task<T> InsertAsync(T doc)
        {
            try
            {
                if (doc == null)
                    throw new ArgumentNullException(nameof(doc));
                await Collection.InsertOneAsync(doc);
            }
            catch (MongoWriteConcernException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
            return doc;
        }

        public IEnumerable<T> Insert(IEnumerable<T> docs)
        {
            var insertBatch = docs as T[] ?? docs.ToArray();
            Collection.InsertMany(insertBatch);
            return insertBatch;
        }

        public async Task<IEnumerable<T>> InsertAsync(IEnumerable<T> docs)
        {
            var insertBatch = docs as T[] ?? docs.ToArray();
            try
            {
                if (docs == null)
                    throw new ArgumentNullException(nameof(docs));

                await Collection.InsertManyAsync(insertBatch);

            }
            catch (MongoWriteConcernException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
            return insertBatch;
        }

        public void Update(T doc)
        {
            try
            {
                if (doc == null)
                    throw new ArgumentNullException(nameof(doc));

                var filter = Builders<T>.Filter.Eq("_id", doc.Id);
                Collection.ReplaceOne(filter, doc);

            }
            catch (MongoBulkWriteException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public void Update(IEnumerable<T> docs)
        {
            try
            {
                if (docs == null)
                    throw new ArgumentNullException(nameof(docs));
                foreach (var doc in docs)
                {
                    var filter = Builders<T>.Filter.Eq("_id", doc.Id);
                    Collection.ReplaceOne(filter, doc);
                }
            }
            catch (MongoBulkWriteException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateAsync(T doc)
        {
            try
            {
                if (doc == null)
                    throw new ArgumentNullException(nameof(doc));

                var filter = Builders<T>.Filter.Eq("_id", doc.Id);
                var replaceOneResult = await Collection.ReplaceOneAsync(filter, doc);

                return replaceOneResult.IsAcknowledged;

            }
            catch (MongoBulkWriteException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public void Update(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition)
        {
            try
            {

                Collection.UpdateMany(filterDefinition, updateDefinition);
            }
            catch (MongoBulkWriteException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public async void UpdateAsync(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition)
        {
            try
            {
                await Collection.UpdateManyAsync(filterDefinition, updateDefinition);
            }
            catch (MongoBulkWriteException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public void UpdateById(object id, UpdateDefinition<T> updateBuilder)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("_id", id);
                Collection.UpdateOne(filter, updateBuilder);
            }
            catch (MongoWriteConcernException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public T FindAndModifyById(object id, UpdateDefinition<T> updateDefinition)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id));

                var filter = Builders<T>.Filter.Eq("_id", id);
                var result = Collection.FindOneAndUpdate(filter, updateDefinition);
                return result;
            }
            catch (MongoWriteConcernException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public long DeleteById(object id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id));

                var filter = Builders<T>.Filter.Eq("_id", id);
                var result = Collection.DeleteOne(filter);

                return result.DeletedCount;

            }
            catch (MongoWriteException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public long DeleteOne(FilterDefinition<T> filter)
        {
            try
            {
                if (filter == null)
                    throw new ArgumentNullException(nameof(filter));

                var result = Collection.DeleteOne(filter);

                return result.DeletedCount;

            }
            catch (MongoWriteException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public async void DeleteOneAsync(FilterDefinition<T> filter)
        {
            try
            {
                if (filter == null)
                    throw new ArgumentNullException(nameof(filter));

                await Collection.DeleteOneAsync(filter);

            }
            catch (MongoWriteException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public long Delete(FilterDefinition<T> filter)
        {
            try
            {
                if (filter == null)
                    throw new ArgumentNullException(nameof(filter));

                var result = Collection.DeleteMany(filter);

                return result.DeletedCount;

            }
            catch (MongoWriteException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public async void DeleteAsync(FilterDefinition<T> filter)
        {
            try
            {
                if (filter == null)
                    throw new ArgumentNullException(nameof(filter));

                await Collection.DeleteManyAsync(filter);

            }
            catch (MongoWriteException ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public long Count()
        {
            return Collection.AsQueryable().Count();
        }

        public long Count(FilterDefinition<T> filter)
        {
            return Collection.Count(filter);
        }

        public long Count(Expression<Func<T, bool>> expression)
        {
            return Collection.AsQueryable().Where(expression).Count();
        }

        public IQueryable<T> All()
        {
            return Collection.AsQueryable();
        }

        public IQueryable<T> All(int page, int pageSize)
        {
            return Collection.AsQueryable().Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<T> All(Expression<Func<T, bool>> expression)
        {
            return Collection.AsQueryable().Where(expression).AsQueryable();
        }

        public IQueryable<T> All(Expression<Func<T, bool>> expression, int page, int pageSize)
        {
            return Collection.AsQueryable().Where(expression).Skip((page - 1) * pageSize).Take(pageSize).AsQueryable();
        }

        public IEnumerable<T> FindAll()
        {
            return Collection.AsQueryable().ToEnumerable();
        }

        public T FindOneById(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return Collection.FindSync(Builders<T>.Filter.Eq("_id", id)).FirstOrDefault();
        }

        public T FindOne(FilterDefinition<T> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return Collection.FindSync(filter).FirstOrDefault();
        }

        public T FindOne(Expression<Func<T, bool>> expression)
        {
            if (expression == null) throw
                    new ArgumentNullException(nameof(expression));

            return Collection.AsQueryable().Where(expression).FirstOrDefault();
        }

        public IEnumerable<T> Find(FilterDefinition<T> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            return Collection.FindSync(filter).ToEnumerable();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            return Collection.AsQueryable().Where(expression);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression, int page, int pageSize)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            return Collection.AsQueryable().Where(expression).Skip((page - 1) * pageSize).Take(pageSize).ToEnumerable();
        }

        public T FindAndRemove(FilterDefinition<T> filter)
        {
            return Collection.FindOneAndDelete(filter);
        }

        public T FindAndRemove(Expression<Func<T, bool>> expression)
        {
            return Collection.FindOneAndDelete(expression);
        }
        public void RemoveAll()
        {
            Collection.DeleteManyAsync(Builders<T>.Filter.Exists("_id", true));
        }

        public void CreateIndex(IndexKeysDefinition<T> indexKeysDefinition)
        {
            Collection.Indexes.CreateOneAsync(indexKeysDefinition);
        }

        public void CreateIndex(IndexKeysDefinition<T> indexKeysDefinition, CreateIndexOptions options)
        {
            Collection.Indexes.CreateOneAsync(indexKeysDefinition, options);
        }

        public async Task<List<BsonDocument>> GetIndexesAsync()
        {
            var cursor = await Collection.Indexes.ListAsync();

            return cursor.ToList();
        }



    }
}
