namespace BlowUp.Models
{
    public class DocumentDataColumn
    {
        public DocumentDataColumn(string name, DocumentDataColumnHorizontalAlignment horizontalAlignment = DocumentDataColumnHorizontalAlignment.Unknown)
        {
            this.Name = name;

            this.HorizontalAlignment = horizontalAlignment;
        }

        public DocumentDataColumnHorizontalAlignment HorizontalAlignment { get; set; }

        public string Name { get; private set; }
    }
}