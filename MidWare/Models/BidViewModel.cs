using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Domain.NoSql.Data.DomainEntites;
using Domain.NoSql.Data.DomainRepository;

namespace MidWare.Models
{
    public class BidViewModel
    {
        private readonly IAccountRepository _repo;

        public BidViewModel()
        {
            _repo = new AccountRepository();
        }

        public BidViewModel(Bid bid)
        {
            _repo = new AccountRepository();

            ProjectId = Convert.ToString(bid.ProjectId);
            BidBy = Convert.ToString(bid.BidBy);
            BidByName = _repo.GetUserById(Convert.ToString(bid.BidBy)).Name;
            BidValue =  bid.BidValue;
            Duration = bid.Duration;
            Comment = bid.Comment;
            BidDate = bid.BidDate;
        }

        [Display(Name = "ProjectId")]
        public string ProjectId { get; set; }
        
        [Display(Name = "BidBy")]
        public string BidBy { get; set; }

        [Display(Name = "BidByName")]
        public string BidByName { get; set; }

        [Required]
        [Display(Name = "Bid Value")]
        public decimal BidValue { get; set; }

        [Required]
        [Display(Name = "Duration")]
        public int Duration { get; set; }

        [Required]
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Display(Name = "BidDate")]
        public DateTime BidDate { get; set; }
    }
}
