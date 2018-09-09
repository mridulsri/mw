using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.NoSql.Data.DomainEntites;
using MidWare.Extention;

namespace MidWare.Models
{
    public class AccountModel
    {
        public AccountModel()
        {
            Rating = Helper.Ratings();
            IsVerified = true;
            IsPaymentVerified = true;
            NumberOfProjectDone = Helper.RandomNumber();
            AccountFeedBack = new List<AccountFeedBack>();
        }

        public AccountModel(Account account)
        {
            Name = account.Name;
            Email = account.Email;
            ContactNumber = account.ContactNumber;
            Type = account.Type;
            Rating = Helper.Ratings();
            IsVerified = true;
            IsPaymentVerified = true;
            NumberOfProjectDone = Helper.RandomNumber();
            Badge = Helper.Badge();
            AccountFeedBack = new List<AccountFeedBack>();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Password { get; set; }
        public int Type { get; set; }

        public double Rating { get; set; }
        public bool IsVerified { get; set; }
        public bool IsPaymentVerified { get; set; }
        public int NumberOfProjectDone { get; set; }
        public DateTime MemberSince { get; set; }
        public string Badge { get; set; }
        public List<AccountFeedBack> AccountFeedBack { get; set; }
    }

    public class AccountFeedBack
    {
        public AccountFeedBack()
        {
            CreatedAt = DateTime.Now.AddDays(Convert.ToDouble(Helper.RandomNumber()));
            Ratings = Helper.Ratings();
        }
        public string Name { get; set; }
        public string Comment { get; set; }
        public double Ratings { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
