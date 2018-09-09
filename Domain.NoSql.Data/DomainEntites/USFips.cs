using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.NoSql.Data.DomainEntites
{
    [BsonIgnoreExtraElements]
    public class USFips : TDocument
    {
        [BsonElement("state")]
        public string State { get; set; }
        [BsonElement("county")]
        public string County { get; set; }
        [BsonElement("fipsstate")]
        public int FipsState { get; set; }
        [BsonElement("fipscounty")]
        public int FipsCounty { get; set; }
        [BsonElement("statecode")]
        public string StateCode { get; set; }
    }
}
