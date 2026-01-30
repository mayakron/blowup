using BlowUp.Models;

namespace BlowUp.Parsers
{
    internal class BlowUpParserState
    {
        public DocumentCodeBlock ActiveDocumentCodeBlock;

        public DocumentDataTable ActiveDocumentDataTable;

        public DocumentList[] ActiveDocumentListHierarchy;

        public int ActiveDocumentListHierarchyLevel;

        public DocumentQuoteBlock ActiveDocumentQuoteBlock;
    }
}