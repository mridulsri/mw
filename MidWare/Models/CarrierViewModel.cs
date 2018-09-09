using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidWare.Models
{
    public class CarrierViewModel
    {
        public ProjectFeedModel Project { get; set; }
        
        public List<BidDetailViewModel> BidDetails { get; set; }
    }


    public class BidDetailViewModel
    {
        public AccountModel Account { get; set; }
        public BidViewModel Bid { get; set; }
    }
}
