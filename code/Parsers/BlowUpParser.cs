using BlowUp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BlowUp.Parsers
{
    internal class BlowUpParser
    {
        public readonly Document Document;

        public readonly BlowUpParserState ParserState;

        public BlowUpParser()
        {
            this.Document = new Document();

            this.ParserState = new BlowUpParserState();
        }

        public void ParseLine(string inputLine)
        {
            if (this.TryParseDocumentTitle(inputLine, this.Document.Items))
            {
                return;
            }

            if (this.TryParseDocumentObject(inputLine, this.Document.Items))
            {
                return;
            }

            if (this.TryParseDocumentDataTable(inputLine, this.Document.Items))
            {
                return;
            }

            if (this.TryParseDocumentCodeBlock(inputLine, this.Document.Items))
            {
                return;
            }

            if (this.TryParseDocumentQuoteBlock(inputLine, this.Document.Items))
            {
                return;
            }

            if (this.TryParseDocumentList(inputLine, this.Document.Items))
            {
                return;
            }

            if (this.TryParseDocumentImage(inputLine, this.Document.Items))
            {
                return;
            }

            if (this.TryParseDocumentText(inputLine, this.Document.Items))
            {
                return;
            }
        }

        private static DocumentDataColumnHorizontalAlignment TryInferDocumentDataColumnHorizontalAlignment(string contentDataItem)
        {
            if (contentDataItem.StartsWith(" "))
            {
                if (contentDataItem.EndsWith(" "))
                {
                    return DocumentDataColumnHorizontalAlignment.Center;
                }
                else
                {
                    return DocumentDataColumnHorizontalAlignment.Right;
                }
            }
            else
            {
                if (contentDataItem.EndsWith(" "))
                {
                    return DocumentDataColumnHorizontalAlignment.Left;
                }
            }

            return DocumentDataColumnHorizontalAlignment.Unknown;
        }

        private bool TryParseDocumentCodeBlock(string inputLine, List<object> itemsContainer)
        {
            if (inputLine.StartsWith("`"))
            {
                var contentLine = (inputLine.Length > 1) ? inputLine.Substring(2) : string.Empty;

                if (this.ParserState.ActiveDocumentCodeBlock == null)
                {
                    this.ParserState.ActiveDocumentCodeBlock = new DocumentCodeBlock();

                    itemsContainer.Add(this.ParserState.ActiveDocumentCodeBlock);
                }

                this.ParserState.ActiveDocumentCodeBlock.Content.AppendLine(contentLine);

                return true;
            }

            this.ParserState.ActiveDocumentCodeBlock = null;

            return false;
        }

        private bool TryParseDocumentDataTable(string inputLine, List<object> itemsContainer)
        {
            if ((inputLine.StartsWith("| ") && inputLine.EndsWith(" |")) || (inputLine.StartsWith("│ ") && inputLine.EndsWith(" │")) || (inputLine.StartsWith("┌─") && inputLine.EndsWith("─┐")) || (inputLine.StartsWith("├─") && inputLine.EndsWith("─┤")) || (inputLine.StartsWith("└─") && inputLine.EndsWith("─┘")))
            {
                var contentLine = inputLine.Substring(2, inputLine.Length - 4);

                if (Regex.IsMatch(contentLine, "^(-|\\|)+$", RegexOptions.None) || Regex.IsMatch(contentLine, "^(─|┬|┼|┴)+$", RegexOptions.None))
                {
                    return true;
                }

                var contentData = contentLine.Split(new[] { " | ", " │ " }, StringSplitOptions.None);

                var trimmedData = new string[contentData.Length];

                for (int i = 0; i < contentData.Length; i++)
                {
                    trimmedData[i] = contentData[i].Trim();
                }

                if (this.ParserState.ActiveDocumentDataTable == null)
                {
                    this.ParserState.ActiveDocumentDataTable = new DocumentDataTable();

                    itemsContainer.Add(this.ParserState.ActiveDocumentDataTable);

                    for (int i = 0; i < contentData.Length; i++)
                    {
                        var horizontalAlignment = TryInferDocumentDataColumnHorizontalAlignment(contentData[i]);

                        this.ParserState.ActiveDocumentDataTable.Columns.Add(new DocumentDataColumn(trimmedData[i], horizontalAlignment));
                    }
                }
                else
                {
                    for (int i = 0; i < contentData.Length; i++)
                    {
                        if (this.ParserState.ActiveDocumentDataTable.Columns[i].HorizontalAlignment == DocumentDataColumnHorizontalAlignment.Unknown)
                        {
                            this.ParserState.ActiveDocumentDataTable.Columns[i].HorizontalAlignment = TryInferDocumentDataColumnHorizontalAlignment(contentData[i]);
                        }
                    }

                    this.ParserState.ActiveDocumentDataTable.Rows.Add(new DocumentDataRow(trimmedData));
                }

                return true;
            }

            this.ParserState.ActiveDocumentDataTable = null;

            return false;
        }

        private bool TryParseDocumentImage(string inputLine, List<object> itemsContainer)
        {
            var imageMatch = Regex.Match(inputLine, "^\\|\\|([^\\|]+)::([^\\|]+)\\|\\|$", RegexOptions.None);

            if (imageMatch.Success)
            {
                itemsContainer.Add(new DocumentImage(imageMatch.Groups[2].Captures[0].Value, imageMatch.Groups[1].Captures[0].Value));

                return true;
            }

            return false;
        }

        private bool TryParseDocumentList(string inputLine, List<object> itemsContainer)
        {
            var listMatch = Regex.Match(inputLine, "^(    )+((\\*)|([0-9]+\\.)) (.*)$", RegexOptions.None);

            if (listMatch.Success)
            {
                var listLevel = listMatch.Groups[1].Captures.Count - 1;

                var listType = (listMatch.Groups[3].Captures.Count > 0) ? DocumentListType.Unordered : (listMatch.Groups[4].Captures.Count > 0) ? DocumentListType.Ordered : DocumentListType.Unordered;

                var contentLine = listMatch.Groups[5].Captures[0].Value;

                if (this.ParserState.ActiveDocumentListHierarchy == null)
                {
                    this.ParserState.ActiveDocumentListHierarchy = new DocumentList[16];

                    var documentList = new DocumentList(listType);

                    documentList.Items.Add(contentLine);

                    this.ParserState.ActiveDocumentListHierarchy[listLevel] = documentList;

                    itemsContainer.Add(this.ParserState.ActiveDocumentListHierarchy[listLevel]);

                    this.ParserState.ActiveDocumentListHierarchyLevel = listLevel;

                    return true;
                }

                if (listLevel < this.ParserState.ActiveDocumentListHierarchyLevel)
                {
                    for (int i = this.ParserState.ActiveDocumentListHierarchyLevel; i > listLevel; i--)
                    {
                        this.ParserState.ActiveDocumentListHierarchy[i] = null;
                    }

                    this.ParserState.ActiveDocumentListHierarchy[listLevel].Items.Add(contentLine);

                    this.ParserState.ActiveDocumentListHierarchyLevel = listLevel;

                    return true;
                }

                if (listLevel > this.ParserState.ActiveDocumentListHierarchyLevel)
                {
                    var documentList = new DocumentList(listType);

                    documentList.Items.Add(contentLine);

                    this.ParserState.ActiveDocumentListHierarchy[listLevel] = documentList;

                    this.ParserState.ActiveDocumentListHierarchy[this.ParserState.ActiveDocumentListHierarchyLevel].Items.Add(documentList);

                    this.ParserState.ActiveDocumentListHierarchyLevel = listLevel;

                    return true;
                }

                this.ParserState.ActiveDocumentListHierarchy[listLevel].Items.Add(contentLine);

                return true;
            }

            this.ParserState.ActiveDocumentListHierarchy = null;

            this.ParserState.ActiveDocumentListHierarchyLevel = -1;

            return false;
        }

        private bool TryParseDocumentObject(string inputLine, List<object> itemsContainer)
        {
            if (inputLine.StartsWith("$$") && inputLine.EndsWith("$$"))
            {
                var contentLine = (inputLine.Length > 4) ? inputLine.Substring(2, inputLine.Length - 4) : string.Empty;

                if (!string.IsNullOrEmpty(contentLine))
                {
                    Match contentMatch;

                    contentMatch = Regex.Match(contentLine, "^ArticleCards( (.*?))?$", RegexOptions.None);

                    if (contentMatch.Success)
                    {
                        itemsContainer.Add(new DocumentArticleCardsObject(itemsContainer, Utilities.JsonSerializerUtility.DeserializeFromString<DocumentArticleCardsInput>(File.ReadAllText(contentMatch.Groups[2].Value))));

                        return true;
                    }

                    contentMatch = Regex.Match(contentLine, "^DynamicImage( (.*?))?$", RegexOptions.None);

                    if (contentMatch.Success)
                    {
                        itemsContainer.Add(new DocumentDynamicImageObject(itemsContainer, Utilities.JsonSerializerUtility.DeserializeFromString<DocumentDynamicImageInput>(File.ReadAllText(contentMatch.Groups[2].Value))));

                        return true;
                    }

                    contentMatch = Regex.Match(contentLine, "^TableOfContents( (.*?))?$", RegexOptions.None);

                    if (contentMatch.Success)
                    {
                        itemsContainer.Add(new DocumentTableOfContentsObject(itemsContainer));

                        return true;
                    }
                }

                return true;
            }

            return false;
        }

        private bool TryParseDocumentQuoteBlock(string inputLine, List<object> itemsContainer)
        {
            if (inputLine.StartsWith(">"))
            {
                var contentLine = (inputLine.Length > 1) ? inputLine.Substring(2) : string.Empty;

                if (this.ParserState.ActiveDocumentQuoteBlock == null)
                {
                    this.ParserState.ActiveDocumentQuoteBlock = new DocumentQuoteBlock();

                    itemsContainer.Add(this.ParserState.ActiveDocumentQuoteBlock);
                }

                if (this.TryParseDocumentText(contentLine, this.ParserState.ActiveDocumentQuoteBlock.Items))
                {
                    return true;
                }

                return true;
            }

            this.ParserState.ActiveDocumentQuoteBlock = null;

            return false;
        }

        private bool TryParseDocumentText(string inputLine, List<object> itemsContainer)
        {
            if (string.IsNullOrEmpty(inputLine)) itemsContainer.Add(new DocumentWhiteSpace()); else itemsContainer.Add(new DocumentText(inputLine));

            return true;
        }

        private bool TryParseDocumentTitle(string inputLine, List<object> itemsContainer)
        {
            if (inputLine.StartsWith("##### ") && inputLine.EndsWith(" #####"))
            {
                var contentLine = inputLine.Substring(6, inputLine.Length - 12);

                this.Document.Items.Add(new DocumentTitle(1, contentLine));

                this.Document.Title = contentLine;

                return true;
            }

            if (inputLine.StartsWith("#### ") && inputLine.EndsWith(" ####"))
            {
                var contentLine = inputLine.Substring(5, inputLine.Length - 10);

                this.Document.Items.Add(new DocumentTitle(2, contentLine));

                return true;
            }

            if (inputLine.StartsWith("### ") && inputLine.EndsWith(" ###"))
            {
                var contentLine = inputLine.Substring(4, inputLine.Length - 8);

                this.Document.Items.Add(new DocumentTitle(3, contentLine));

                return true;
            }

            if (inputLine.StartsWith("## ") && inputLine.EndsWith(" ##"))
            {
                var contentLine = inputLine.Substring(3, inputLine.Length - 6);

                this.Document.Items.Add(new DocumentTitle(4, contentLine));

                return true;
            }

            if (inputLine.StartsWith("# ") && inputLine.EndsWith(" #"))
            {
                var contentLine = inputLine.Substring(2, inputLine.Length - 4);

                this.Document.Items.Add(new DocumentTitle(5, contentLine));

                return true;
            }

            return false;
        }
    }
}