using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MidWare.Models
{
    public class SubmitReportModel
    {
        public SubmitReportModel()
        {
            ProjectFeedList = new List<ProjectFeedModel>();
        }

        public ProjectFeedModel ProjectFeed { get; set; }
        public List<ProjectFeedModel> ProjectFeedList { get; set; }
        public AddReport AddReport { get; set; }
        public AddInvoice AddInvoice { get; set; }
    }

    public class AddReport
    {
       
        public string ProjectFeedId { get; set; }

        [Required]
        public string Report { get; set; }
    }

    public class AddInvoice
    {
        public string ProjectFeedId { get; set; }

        [Required]
        public string Invoice { get; set; }

        
    }
}
