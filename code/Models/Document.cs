using System.Collections.Generic;

namespace BlowUp.Models
{
    public class Document
    {
        public Document()
        {
            this.Items = new List<object>();
        }

        public List<object> Items { get; private set; }

        public string Title { get; set; }
    }
}