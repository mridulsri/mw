using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidWare.Models
{
    public class AccountTypeModel
    {
        public int Id { set; get; }
        public string AccountType { set; get; }
    }

    public class ProjectTypeModel
    {
        public int Id { set; get; }
        public string ProjectType { set; get; }
    }
}
