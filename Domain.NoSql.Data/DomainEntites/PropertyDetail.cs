using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.NoSql.Data.DomainEntites
{
    [BsonIgnoreExtraElements]
    public class PropertyDetail:TDocument
    {
        [BsonElement("igdid")]
        public string IgdId { get; set; }
        [BsonElement("parcelid")]
        public string ParcelId { get; set; }
        [BsonElement("data")]
        public List<Data> Data { get; set; }
    }
}
