using System.Collections.Generic;

namespace BlowUp.Models
{
    public class DocumentDataTable
    {
        public DocumentDataTable()
        {
            this.Columns = new List<DocumentDataColumn>();

            this.Rows = new List<DocumentDataRow>();
        }

        public List<DocumentDataColumn> Columns { get; private set; }

        public List<DocumentDataRow> Rows { get; private set; }
    }
}