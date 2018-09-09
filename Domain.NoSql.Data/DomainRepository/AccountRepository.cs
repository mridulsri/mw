using CustomLogger;
using Domain.NoSql.Data.DomainEntites;
using Domain.NoSql.Data.Provider;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.NoSql.Data.DomainRepository
{
    public interface IAccountRepository
    {
        bool AddAccount(Account param);
        Account GetUser(string email, string password);
        Account GetUserById(string id);
        Account GetUserByEmail(string email);
    }

    public class AccountRepository : IAccountRepository
    {
        protected readonly IMongoDatabase Database;
        IMongoCollection<Account> _collection;

        public AccountRepository()
        {
            Database = DatabaseProvider.GetDatabase();
            _collection = CollectionProvider.GetCollection<Account>(Database);
        }

        public bool AddAccount(Account param)
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

        public Account GetUserByEmail(string email)
        {
            try
            {
                return _collection.Find(x => x.Email == email.ToLower()).FirstOrDefault<Account>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            return null;
        }

        public Account GetUser(string email, string password)
        {
            try
            {
                return _collection.Find(x => x.Email == email.ToLower() && x.Password == password.ToLower()).FirstOrDefault<Account>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            return null;
        }

        public Account GetUserById(string id)
        {
            try
            {
                return _collection.Find(x => x.Id == ObjectId.Parse(id)).FirstOrDefault<Account>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            return null;
        }
    }
}
