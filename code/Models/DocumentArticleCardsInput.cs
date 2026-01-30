using System.Collections.Generic;

namespace BlowUp.Models
{
    public class DocumentArticleCardsInput
    {
        public ICollection<Item> Items { get; set; }

        public class Item
        {
            public string AuthorDateContext { get; set; }

            public string Description { get; set; }

            public string Destination { get; set; }

            public string Image { get; set; }

            public string Title { get; set; }
        }
    }
}