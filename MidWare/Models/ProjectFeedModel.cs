using Domain.NoSql.Data.DomainEntites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MidWare.Models
{
    public class CreateProjectFeedModel
    {
        public CreateProjectFeedModel()
        {
            ProjectTypes = new List<ProjectTypeModel>();
        }
        [Required]
        [Display(Name = "Type")]
        public int Type { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "SkillLevel")]
        public string SkillLevel { get; set; }

        [Required]
        [Display(Name = "Budget")]
        public float Budget { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Details")]
        public string Details { get; set; }

        [Required]
        [Display(Name = "JobStatus")]
        public string JobStatus { get; set; }

        [Required]
        [Display(Name = "AssignedTo")]
        public string AssignedTo { get; set; }

        
        public IEnumerable<ProjectTypeModel> ProjectTypes { set; get; }
    }

    public class ProjectFeedModel
    {
        public ProjectFeedModel()
        {
            Account = new AccountModel()
            {
                Rating = 3.5,
                IsVerified = true,
                IsPaymentVerified = true,
            };
            Bids = new List<BidViewModel>();
        }

        public ProjectFeedModel(ProjectFeed projectFeed)
        {
            Type = projectFeed.Type;
            Title = projectFeed.Title;
            SkillLevel = projectFeed.SkillLevel;
            Budget = projectFeed.Budget;
            Name = projectFeed.Name;
            Address = projectFeed.Address;
            Details = projectFeed.Details;
            Type = projectFeed.Type;
            Id = projectFeed.Id.ToString();
            CreatedById = projectFeed.CreatedById;
            AssignTo = Convert.ToString(projectFeed.AssignedTo);

            CreatedDate = projectFeed.CreatedDate;
            DueDate = DueDate;



            Account = new AccountModel()
            {
                Rating = 3.5,
                IsVerified = true,
                IsPaymentVerified = true
            };
            Bids = new List<BidViewModel>();
            if (projectFeed.Bids != null)
            {
                foreach (var bid in projectFeed.Bids)
                {
                    Bids.Add(new BidViewModel(bid));
                }
            }
        }

        public int Type { get; set; }
        public string Title { get; set; }
        public string SkillLevel { get; set; }
        public float Budget { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }

        public string Id { get; set; }
        public string CreatedById { get; set; }
        public string AssignTo { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }

        public AccountModel Account { get; set; }

        public List<BidViewModel> Bids { get; set; }
        public int BidCount
        {
            get
            {
                if (Bids != null)
                    return Bids.Count();
                else
                    return 0;

            }
        }
    }


    public class ProjectFeedDetailModel
    {
        public AccountModel Account { get; set; }
        public ProjectFeedModel ProjectFeed { get; set; }
        public List<FeedbackModel> Feedbacks { get; set; }
    }

    public class FeedbackModel
    {
        public Double Rating { get; set; }
        public bool IsVerified { get; set; }
        public string[] Comments { get; set; }
    }
}
