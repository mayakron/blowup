using System.Collections.Generic;

namespace BlowUp.Models
{
    public class SiteHeaderMenuItem
    {
        public string Destination { get; set; }

        public ICollection<SiteHeaderMenuItem> Items { get; set; }

        public string Title { get; set; }
    }
}