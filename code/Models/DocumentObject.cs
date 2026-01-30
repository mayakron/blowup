using System.Collections.Generic;

namespace BlowUp.Models
{
    public abstract class DocumentObject
    {
        public DocumentObject(List<object> itemsContainer)
        {
            this.ItemsContainer = itemsContainer;
        }

        public List<object> ItemsContainer { get; private set; }
    }
}