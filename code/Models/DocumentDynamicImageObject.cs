using System.Collections.Generic;

namespace BlowUp.Models
{
    public class DocumentDynamicImageObject : DocumentObject
    {
        public DocumentDynamicImageObject(List<object> itemsContainer, DocumentDynamicImageInput input) : base(itemsContainer)
        {
            this.Input = input;
        }

        public DocumentDynamicImageInput Input { get; private set; }
    }
}