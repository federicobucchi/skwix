using Microsoft.WindowsAzure.Storage.Table;

namespace SunnyKarmaWix.Models
{
    public class Cause : TableEntity
    {
        public string CauseID { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
        public string Image180 { get; set; }
        public string Image370 { get; set; }
        public long Cash { get; set; }
        public bool? IsAvailable { get; set; }
    }
}