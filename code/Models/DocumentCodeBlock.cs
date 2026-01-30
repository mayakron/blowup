using System.Text;

namespace BlowUp.Models
{
    public class DocumentCodeBlock
    {
        public DocumentCodeBlock()
        {
            this.Content = new StringBuilder();
        }

        public StringBuilder Content { get; private set; }
    }
}