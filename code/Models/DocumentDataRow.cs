namespace BlowUp.Models
{
    public class DocumentDataRow
    {
        public DocumentDataRow(string[] values)
        {
            this.Values = values;
        }

        public string[] Values { get; private set; }
    }
}