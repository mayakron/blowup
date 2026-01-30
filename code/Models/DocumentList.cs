using System.Collections.Generic;

namespace BlowUp.Models
{
    public class DocumentList
    {
        public DocumentList(DocumentListType listType)
        {
            this.Items = new List<object>();

            this.ListType = listType;
        }

        public List<object> Items { get; private set; }

        public DocumentListType ListType { get; private set; }
    }
}