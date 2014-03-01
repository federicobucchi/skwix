using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace SunnyKarmaWix.Models
{
    public class NativeBalanceChange : TableEntity
    {
        public string BalanceID { get; set; }
        public long Change { get; set; }
        public DateTime Created { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string DescriptionValue { get; set; }
    }
}