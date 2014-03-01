using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace SunnyKarmaWix.Models
{
    public class WixOwner : TableEntity
    {
        public string WixInstanceId { get; set; }
        public string Username { get; set; }
    }
}