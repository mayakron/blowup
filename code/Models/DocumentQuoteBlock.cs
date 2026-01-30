using System.Collections.Generic;

namespace BlowUp.Models
{
    public class DocumentQuoteBlock
    {
        public DocumentQuoteBlock()
        {
            this.Items = new List<object>();
        }

        public List<object> Items { get; private set; }
    }
}