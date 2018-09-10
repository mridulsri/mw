using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace Domain.NoSql.Data.DomainEntites
{
    [BsonIgnoreExtraElements]
    public class ProjectFeed : TDocument
    {
        public ProjectFeed()
        {
            IsActive = true;
            CreatedDate = DateTime.Now;
            DueDate = DateTime.Now.AddDays(5);
            JobStatus = "open"; // open, assign, complete, close
        }

        public int Type { get; set; }
        public string Title { get; set; }
        public string SkillLevel { get; set; }
        public float Budget { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }
        public List<Bid> Bids { get; set; }

        public string CreatedById { get; set; }
        public string CreatedByEmail { get; set; }

        public string AssignedTo { get; set; }
        public string JobStatus { get; set; }
        // public bool JobStatus { get; set; }


        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }

    }


}
