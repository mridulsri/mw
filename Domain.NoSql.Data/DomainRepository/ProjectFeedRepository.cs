using CustomLogger;
using Domain.NoSql.Data.DomainEntites;
using Domain.NoSql.Data.Provider;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.NoSql.Data.DomainRepository
{
    public interface IProjectFeedRepository
    {
        bool AddProject(ProjectFeed param);
        bool UpdateProject(Bid param, string id);
        List<ProjectFeed> GetProjectFeeds();
        List<ProjectFeed> GetProjectFeedCreatedBy(string id);
        ProjectFeed GetProjectById(string id);
        List<ProjectFeed> GetAwardedProjectByAccountId(string id);
        bool UpdateProjecActiveStatus(string projectId, bool status);
        bool AssignProject(string projId, string accId);
        List<ProjectFeed> GetProjectFeedByType(int typeId);
    }

    public class ProjectFeedRepository : IProjectFeedRepository
    {
        protected readonly IMongoDatabase Database;
        IMongoCollection<ProjectFeed> _collection;

        public ProjectFeedRepository()
        {
            Database = DatabaseProvider.GetDatabase();
            _collection = CollectionProvider.GetCollection<ProjectFeed>(Database);
        }

        public bool AddProject(ProjectFeed param)
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

        public bool UpdateProject(Bid param, string projectId)
        {
            try
            {
                param.Id =ObjectId.GenerateNewId();
                var filter = Builders<ProjectFeed>.Filter.Eq("_id", new ObjectId(projectId));
                var update = Builders<ProjectFeed>.Update.Push("bids", param);
                var result = _collection.UpdateOneAsync(filter, update).Result;
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            return false;
        }
        public bool UpdateProjecActiveStatus(string projectId, bool status)
        {
            try
            {
                var filter = Builders<ProjectFeed>.Filter.Eq("_id", new ObjectId(projectId));
                var update = Builders<ProjectFeed>.Update.Set(x => x.IsActive, status);
                var result = _collection.UpdateOneAsync(filter, update).Result;
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            return false;
        }



        public List<ProjectFeed> GetProjectFeeds()
        {
            try
            {
               return _collection.Find(t => t.IsActive == true).ToList<ProjectFeed>()
                    .OrderByDescending(o=>o.CreatedDate).ToList<ProjectFeed>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            return null;
        }

        public ProjectFeed GetProjectById(string id)
        {
            try
            {
                return _collection.Find(x => x.Id == ObjectId.Parse(id)).FirstOrDefault<ProjectFeed>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            return null;

        }
        public List<ProjectFeed> GetAwardedProjectByAccountId(string id)
        {
            try
            {
                return _collection.Find(x => x.AssignedTo == id).ToList<ProjectFeed>().OrderByDescending(o => o.CreatedDate).ToList<ProjectFeed>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            return null;

        }

        public List<ProjectFeed> GetProjectFeedCreatedBy(string id)
        {
            try
            {
                return _collection.Find(t => t.CreatedById == id).ToList<ProjectFeed>().OrderByDescending(o => o.CreatedDate).ToList<ProjectFeed>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            return null;
        }

        public List<ProjectFeed> GetProjectFeedByType(int typeId)
        {
            try
            {
                return _collection.Find(t => t.Type == typeId).ToList<ProjectFeed>().OrderByDescending(o => o.CreatedDate).ToList<ProjectFeed>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            return null;
        }

        public bool AssignProject(string projId, string accId)
        {
            try
            {
                var filter = Builders<ProjectFeed>.Filter.Eq("_id", ObjectId.Parse(projId));
                var update = Builders<ProjectFeed>.Update.Set("assignedto", accId);
                var result = _collection.UpdateOneAsync(filter, update).Result;
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            return false;
        }

    }
}
