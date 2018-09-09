using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidWare.Models
{
    public static class LoggedInUser 
    {
        public static string Id { get; set; }
        public static string Name { get; set; }
        public static int AccountType { get; set; }
        public static string Email { get; set; }
    }

    public  class CurrentUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int AccountType { get; set; }
        public string Email { get; set; }
    }
}
