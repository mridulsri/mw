using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.NoSql.Data.DomainEntites
{
    [BsonIgnoreExtraElements]
    public class Account : TDocument
    {
        public Account()
        {
            IsVerified = true;
            IsPaymentVerified = true;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Password { get; set; }
        public int Type { get; set; }

        
        public bool IsVerified { get; set; }
        public bool IsPaymentVerified { get; set; }

    }
}
