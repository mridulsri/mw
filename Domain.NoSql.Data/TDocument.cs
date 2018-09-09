using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.NoSql.Data
{
    public abstract class TDocument
    {
        //[BsonIgnore]
        //public abstract object Id { get; set; }
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
