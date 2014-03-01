using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace SunnyKarmaWix.Models
{
    public class Profile : TableEntity
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string About { get; set; }
        public string Motto { get; set; }
        public bool IsShowEmail { get; set; }
        public bool IsVerifiedEmail { get; set; }
        public DateTime Registered { get; set; }
        public DateTime? Birthday { get; set; }
        public long EarnedMoney { get; set; }
        public long SpentMoney { get; set; }
    }
}