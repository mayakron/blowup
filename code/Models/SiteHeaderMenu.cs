using System.Collections.Generic;

namespace BlowUp.Models
{
    public class SiteHeaderMenu
    {
        public ICollection<SiteHeaderMenuSection> Sections { get; set; }
    }
}