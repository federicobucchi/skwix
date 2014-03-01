using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace SunnyKarmaWix.Models
{
    public class User : TableEntity
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public DateTime Created { get; set; }
        public string AuthPairs { get; set; }
        public bool IsAdmin { get; set; }
        public string BPAccountURI { get; set; }
        public string Status { get; set; }
    }
}