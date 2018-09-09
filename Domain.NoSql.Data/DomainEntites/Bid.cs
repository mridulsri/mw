using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.NoSql.Data.DomainEntites
{
    [BsonIgnoreExtraElements]
    public class Bid : TDocument
    {
        public object ProjectId { get; set; }
        public object BidBy { get; set; }
        public decimal BidValue { get; set; }
        public int Duration { get; set; }
        public string Comment { get; set; }
        public DateTime BidDate { get; set; }
    }
}
