using System.Collections.Generic;

namespace BlowUp.Models
{
    public class DocumentArticleCardsObject : DocumentObject
    {
        public DocumentArticleCardsObject(List<object> itemsContainer, DocumentArticleCardsInput input) : base(itemsContainer)
        {
            this.Input = input;
        }

        public DocumentArticleCardsInput Input { get; private set; }
    }
}