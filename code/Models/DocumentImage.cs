namespace BlowUp.Models
{
    public class DocumentImage
    {
        public DocumentImage(string source, string context, string description = null)
        {
            this.Source = source;

            this.Description = description;
        }

        public string Description { get; private set; }

        public string Source { get; private set; }
    }
}