namespace BlowUp.Models
{
    public class DocumentText
    {
        public DocumentText(string content)
        {
            this.Content = content;
        }

        public string Content { get; private set; }
    }
}