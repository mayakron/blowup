using System.Collections.Generic;

namespace BlowUp.Models
{
    public class SiteHeaderMenuSection
    {
        public ICollection<SiteHeaderMenuItem> Items { get; set; }

        public string Title { get; set; }
    }
}