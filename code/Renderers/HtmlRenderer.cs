using BlowUp.Models;
using BlowUp.Utilities;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BlowUp.Renderers
{
    internal class HtmlRenderer
    {
        private readonly Document document;

        private readonly string runnersDirectoryPath;

        private readonly string siteFooterFilePath;

        private readonly string siteHeaderFilePath;

        private readonly string siteStyleFilePath;

        private readonly string sourceDirectoryPath;

        private readonly Stream stream;

        public HtmlRenderer(Document document, Stream stream, string sourceDirectoryPath, string runnersDirectoryPath, string siteStyleFilePath, string siteHeaderFilePath, string siteFooterFilePath)
        {
            this.document = document;
            this.stream = stream;
            this.sourceDirectoryPath = sourceDirectoryPath;
            this.runnersDirectoryPath = runnersDirectoryPath;
            this.siteStyleFilePath = siteStyleFilePath;
            this.siteHeaderFilePath = siteHeaderFilePath;
            this.siteFooterFilePath = siteFooterFilePath;
        }

        public static string GetFileExtension()
        {
            return ".htm";
        }

        public void RenderDocument()
        {
            using (var streamWriter = new StreamWriter(this.stream, Encoding.UTF8))
            {
                streamWriter.Write("<!DOCTYPE html>");
                streamWriter.Write("<html lang=\"en\">");
                streamWriter.Write("<head>");
                streamWriter.Write("<meta charset=\"utf-8\">");
                streamWriter.Write("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");

                if (!string.IsNullOrEmpty(this.siteStyleFilePath))
                {
                    var style = File.ReadAllText(this.siteStyleFilePath);

                    streamWriter.Write($"<style>{style}</style>");
                }

                streamWriter.Write("<title>" + HtmlEncode(this.document.Title) + "</title>");
                streamWriter.Write("</head>");
                streamWriter.Write("<body>");

                if (!string.IsNullOrEmpty(this.siteHeaderFilePath))
                {
                    var siteHeader = JsonSerializerUtility.DeserializeFromString<SiteHeader>(File.ReadAllText(this.siteHeaderFilePath));

                    streamWriter.Write("<nav>");

                    streamWriter.Write("<div id=\"navbar\">");

                    if (siteHeader.Menu != null)
                    {
                        streamWriter.Write("<div id=\"navico\"><a href=\"javascript:void(0);\" onclick=\"toggleNavmnu();\"><svg id=\"navic1\" xmlns=\"http://www.w3.org/2000/svg\" width=\"32\" height=\"32\" viewBox=\"0 0 24 24\"><path fill=\"#ffffff\" d=\"M24 11v3H0v-3h24Zm-6 7v3H0v-3h18Zm6-14v3H0V4h24Z\"></path></svg><svg id=\"navic2\" xmlns=\"http://www.w3.org/2000/svg\" width=\"32\" height=\"32\" viewBox=\"0 0 24 24\"><path fill=\"#ffffff\" d=\"m20.792969 18.144531-6.148438-6.171875 6.164063-6.09375c.253906-.253906.253906-.667968 0-.917968l-1.753906-1.765626c-.121094-.121093-.285157-.1875-.460938-.1875-.171875 0-.335938.070313-.457031.1875L12 9.273438 5.855469 3.203125c-.121094-.125-.285157-.1875-.460938-.1875-.171875 0-.335937.070313-.457031.1875l-1.75 1.761719c-.253906.253906-.253906.664062 0 .917968l6.164062 6.09375-6.144531 6.167969c-.121093.121094-.191406.285157-.191406.460938 0 .171875.0625.335937.191406.457031l1.753907 1.765625c.125.125.289062.191406.457031.191406.164062 0 .332031-.0625.460937-.191406L12 14.675781l6.125 6.144531c.128906.128907.292969.195313.460938.195313.164062 0 .332031-.0625.460937-.195313l1.75-1.761718c.125-.121094.195313-.285156.195313-.457032-.007813-.171874-.078126-.335937-.199219-.457031Zm0 0\"></path></svg></a></div>");
                    }

                    if (siteHeader.Logo != null)
                    {
                        if (!string.IsNullOrEmpty(siteHeader.Logo.Title))
                        {
                            streamWriter.Write($"<div id=\"navlgo\"><span id=\"spnlgo\">{HtmlEncode(siteHeader.Logo.Title)}</span></div>");
                        }
                    }

                    streamWriter.Write("</div>");

                    if (siteHeader.Menu != null)
                    {
                        streamWriter.Write("<div id=\"navmnu\">");

                        foreach (var section in siteHeader.Menu.Sections)
                        {
                            if (!string.IsNullOrEmpty(section.Title))
                            {
                                streamWriter.Write($"<span class=\"navmsn\">{HtmlEncode(section.Title)}</span>");
                            }

                            streamWriter.Write("<ul class=\"navmi1\">");

                            foreach (var item in section.Items)
                            {
                                streamWriter.Write("<li>");

                                streamWriter.Write($"<a class=\"navmi1\" href=\"{GetAnchorDestination(item.Destination)}\">{HtmlEncode(item.Title)}</a>");

                                if (item.Items != null)
                                {
                                    streamWriter.Write("<ul class=\"navmi2\">");

                                    foreach (var itemL2 in item.Items)
                                    {
                                        streamWriter.Write("<li>");

                                        streamWriter.Write($"<a class=\"navmi2\" href=\"{GetAnchorDestination(itemL2.Destination)}\">{HtmlEncode(itemL2.Title)}</a>");

                                        if (itemL2.Items != null)
                                        {
                                            streamWriter.Write("<ul class=\"navmi3\">");

                                            foreach (var itemL3 in itemL2.Items)
                                            {
                                                streamWriter.Write("<li>");

                                                streamWriter.Write($"<a class=\"navmi3\" href=\"{GetAnchorDestination(itemL3.Destination)}\">{HtmlEncode(itemL3.Title)}</a>");

                                                if (itemL3.Items != null)
                                                {
                                                    streamWriter.Write("<ul class=\"navmi4\">");

                                                    foreach (var itemL4 in itemL3.Items)
                                                    {
                                                        streamWriter.Write("<li>");

                                                        streamWriter.Write($"<a class=\"navmi4\" href=\"{GetAnchorDestination(itemL4.Destination)}\">{HtmlEncode(itemL4.Title)}</a>");

                                                        if (itemL4.Items != null)
                                                        {
                                                            streamWriter.Write("<ul class=\"navmi5\">");

                                                            foreach (var itemL5 in itemL4.Items)
                                                            {
                                                                streamWriter.Write("<li>");

                                                                streamWriter.Write($"<a class=\"navmi5\" href=\"{GetAnchorDestination(itemL5.Destination)}\">{HtmlEncode(itemL5.Title)}</a>");

                                                                streamWriter.Write("</li>");
                                                            }

                                                            streamWriter.Write("</ul>");
                                                        }

                                                        streamWriter.Write("</li>");
                                                    }

                                                    streamWriter.Write("</ul>");
                                                }

                                                streamWriter.Write("</li>");
                                            }

                                            streamWriter.Write("</ul>");
                                        }

                                        streamWriter.Write("</li>");
                                    }

                                    streamWriter.Write("</ul>");
                                }

                                streamWriter.Write("</li>");
                            }

                            streamWriter.Write("</ul>");
                        }

                        streamWriter.Write("</div>");
                    }

                    streamWriter.Write("<script>var navmnuEl = document.getElementById('navmnu'); var navic1El = document.getElementById('navic1'); var navic2El = document.getElementById('navic2'); function toggleNavmnu() { if (navmnuEl.style.display != 'block') { navmnuEl.style.display = 'block'; navic1El.style.display = 'none'; navic2El.style.display = 'block'; } else { navmnuEl.style.display = 'none'; navic1El.style.display = 'block'; navic2El.style.display = 'none'; } }</script>");

                    streamWriter.Write("</nav>");
                }

                streamWriter.Write("<article>");

                foreach (var item in this.document.Items)
                {
                    switch (item)
                    {
                        case DocumentTitle typedItem: this.RenderDocumentTitle(typedItem, streamWriter); break;
                        case DocumentArticleCardsObject typedItem: RenderDocumentArticleCardsObject(typedItem, streamWriter); break;
                        case DocumentDynamicImageObject typedItem: RenderDocumentDynamicImageObject(typedItem, streamWriter); break;
                        case DocumentTableOfContentsObject typedItem: RenderDocumentTableOfContentsObject(typedItem, streamWriter); break;
                        case DocumentDataTable typedItem: this.RenderDocumentDataTable(typedItem, streamWriter); break;
                        case DocumentCodeBlock typedItem: this.RenderDocumentCodeBlock(typedItem, streamWriter); break;
                        case DocumentQuoteBlock typedItem: this.RenderDocumentQuoteBlock(typedItem, streamWriter); break;
                        case DocumentList typedItem: this.RenderDocumentList(typedItem, streamWriter); break;
                        case DocumentImage typedItem: this.RenderDocumentImage(typedItem, streamWriter); break;
                        case DocumentText typedItem: this.RenderDocumentText(typedItem, streamWriter); break;
                        case DocumentWhiteSpace typedItem: this.RenderDocumentWhiteSpace(typedItem, streamWriter); break;
                    }
                }

                streamWriter.Write("</article>");

                if (!string.IsNullOrEmpty(this.siteFooterFilePath))
                {
                    streamWriter.Write("<footer>");

                    streamWriter.Write(File.ReadAllText(this.siteFooterFilePath));

                    streamWriter.Write("</footer>");
                }

                streamWriter.Write("</body>");
                streamWriter.Write("</html>");
            }
        }

        private static string GetAnchorDestination(string value)
        {
            return Regex.IsMatch(value, "^[A-Z0-9]+$", RegexOptions.IgnoreCase) ? $"{value}.htm" : value.Replace("\\", "/");
        }

        private static string HtmlEncode(string text)
        {
            return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        private void RenderContent(string content, StreamWriter streamWriter)
        {
            var encodedContent = HtmlEncode(content);

            encodedContent = Parsers.BlowUpParserUtilities.ContentCodeRegex.Replace(encodedContent, match => $"``{Convert.ToBase64String(Encoding.UTF8.GetBytes(match.Groups[1].Captures[0].Value))}``");

            encodedContent = Parsers.BlowUpParserUtilities.ContentFontStyleBoldRegex.Replace(encodedContent, "<b>$1</b>");
            encodedContent = Parsers.BlowUpParserUtilities.ContentFontStyleItalicRegex.Replace(encodedContent, "<i>$1</i>");
            encodedContent = Parsers.BlowUpParserUtilities.ContentFontStyleStrikethroughRegex.Replace(encodedContent, "<strike>$1</strike>");

            encodedContent = Parsers.BlowUpParserUtilities.ContentAnchorRegex2.Replace(encodedContent, match => $"<a href=\"{GetAnchorDestination(match.Groups[2].Value)}\">{HtmlEncode(match.Groups[1].Value)}</a>");
            encodedContent = Parsers.BlowUpParserUtilities.ContentAnchorRegex1.Replace(encodedContent, match => $"<a href=\"{GetAnchorDestination(match.Groups[1].Value)}\">{match.Groups[1].Value}</a>");

            encodedContent = Parsers.BlowUpParserUtilities.ContentCodeRegex.Replace(encodedContent, match => $"<span class=\"code\">{Encoding.UTF8.GetString(Convert.FromBase64String(match.Groups[1].Value))}</span>");

            streamWriter.Write(encodedContent);
        }

        private void RenderDocumentArticleCardsObject(DocumentArticleCardsObject obj, StreamWriter streamWriter)
        {
            streamWriter.Write("<div class=\"crdblk\">");

            foreach (var item in obj.Input.Items)
            {
                streamWriter.Write("<div class=\"crditm\">");
                streamWriter.Write($"<img class=\"crdimg\" src=\"{item.Image}\">");
                streamWriter.Write($"<span class=\"crdtx1\"><a href=\"{item.Destination}\">{HtmlEncode(item.Title)}</a></span>");
                streamWriter.Write($"<span class=\"crdtx2\">{HtmlEncode(item.Description)}</span>");
                streamWriter.Write($"<span class=\"crdtx3\">{HtmlEncode(item.AuthorDateContext)}</span>");
                streamWriter.Write("</div>");
            }

            streamWriter.Write("</div>");
        }

        private void RenderDocumentCodeBlock(DocumentCodeBlock codeBlock, StreamWriter streamWriter)
        {
            streamWriter.Write("<pre>");
            streamWriter.Write("<code>");

            streamWriter.Write(HtmlEncode(codeBlock.Content.ToString()));

            streamWriter.Write("</code>");
            streamWriter.Write("</pre>");
        }

        private void RenderDocumentDataTable(DocumentDataTable dataTable, StreamWriter streamWriter)
        {
            streamWriter.Write("<table>");

            streamWriter.Write("<thead>");
            streamWriter.Write("<tr>");

            foreach (var column in dataTable.Columns)
            {
                var horizontalAlignmentTag = (column.HorizontalAlignment == DocumentDataColumnHorizontalAlignment.Left) ? "left" : (column.HorizontalAlignment == DocumentDataColumnHorizontalAlignment.Center) ? "center" : (column.HorizontalAlignment == DocumentDataColumnHorizontalAlignment.Right) ? "right" : "left";

                streamWriter.Write($"<th align=\"{horizontalAlignmentTag}\" scope=\"col\">");

                RenderContent(column.Name, streamWriter);

                streamWriter.Write("</th>");
            }

            streamWriter.Write("</tr>");
            streamWriter.Write("</thead>");

            streamWriter.Write("<tbody>");

            foreach (var row in dataTable.Rows)
            {
                streamWriter.Write("<tr>");

                for (int i = 0; i < row.Values.Length; i++)
                {
                    var horizontalAlignmentLabel = (dataTable.Columns[i].HorizontalAlignment == DocumentDataColumnHorizontalAlignment.Left) ? "left" : (dataTable.Columns[i].HorizontalAlignment == DocumentDataColumnHorizontalAlignment.Center) ? "center" : (dataTable.Columns[i].HorizontalAlignment == DocumentDataColumnHorizontalAlignment.Right) ? "right" : "left";

                    streamWriter.Write($"<td align=\"{horizontalAlignmentLabel}\">");

                    RenderContent(row.Values[i], streamWriter);

                    streamWriter.Write("</td>");
                }

                streamWriter.Write("</tr>");
            }

            streamWriter.Write("</tbody>");

            streamWriter.Write("</table>");
        }

        private void RenderDocumentDynamicImageObject(DocumentDynamicImageObject obj, StreamWriter streamWriter)
        {
            var process = Process.Start
            (
                new ProcessStartInfo
                {
                    Arguments = obj.Input.Arguments,
                    FileName = Path.Combine(this.runnersDirectoryPath, obj.Input.Runner),
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = sourceDirectoryPath
                }
            );

            process.WaitForExit();

            if (obj.Input.ExpectedExitCode.HasValue)
            {
                if (process.ExitCode != obj.Input.ExpectedExitCode.Value)
                {
                    throw new Exception($"The runner \"{obj.Input.Runner}\" terminated with exit code {process.ExitCode} instead of {obj.Input.ExpectedExitCode.Value} when invoked with arguments [[{obj.Input.Arguments}]].");
                }
            }

            var absoluteExpectedFileName = Path.Combine(this.sourceDirectoryPath, obj.Input.ExpectedFileName);

            if (!File.Exists(absoluteExpectedFileName))
            {
                throw new Exception($"The runner \"{obj.Input.Runner}\" did not produce the expected file name {obj.Input.ExpectedFileName} when invoked with arguments [[{obj.Input.Arguments}]].");
            }

            var imageFromFile = Image.FromFile(absoluteExpectedFileName);

            streamWriter.Write($"<img src=\"{obj.Input.ExpectedFileName}\" width=\"{imageFromFile.Width}\" height=\"{imageFromFile.Height}\"{(!string.IsNullOrEmpty(obj.Input.Description) ? $" alt=\"{HtmlEncode(obj.Input.Description)}\"" : string.Empty)}><br/>");
        }

        private void RenderDocumentImage(DocumentImage image, StreamWriter streamWriter)
        {
            if (File.Exists(image.Source))
            {
                var imageFromFile = Image.FromFile(image.Source);

                streamWriter.Write($"<img src=\"{image.Source}\" width=\"{imageFromFile.Width}\" height=\"{imageFromFile.Height}\"{(!string.IsNullOrEmpty(image.Description) ? $" alt=\"{HtmlEncode(image.Description)}\"" : string.Empty)}><br/>");
            }
            else
            {
                streamWriter.Write($"<img src=\"{image.Source}\"{(!string.IsNullOrEmpty(image.Description) ? $" alt=\"{HtmlEncode(image.Description)}\"" : string.Empty)}><br/>");
            }
        }

        private void RenderDocumentList(DocumentList list, StreamWriter streamWriter)
        {
            var listTypeLabel = (list.ListType == DocumentListType.Ordered) ? "ol" : (list.ListType == DocumentListType.Unordered) ? "ul" : "ul";

            streamWriter.Write($"<{listTypeLabel}>");

            foreach (var item in list.Items)
            {
                switch (item)
                {
                    case DocumentList typedItem: RenderDocumentList(typedItem, streamWriter); break;
                    case string typedItem: streamWriter.Write("<li>"); RenderContent(typedItem, streamWriter); streamWriter.Write("</li>"); break;
                }
            }

            streamWriter.Write($"</{listTypeLabel}>");
        }

        private void RenderDocumentQuoteBlock(DocumentQuoteBlock quoteBlock, StreamWriter streamWriter)
        {
            streamWriter.Write("<blockquote>");

            foreach (var item in quoteBlock.Items)
            {
                switch (item)
                {
                    case DocumentText typedItem: RenderDocumentText(typedItem, streamWriter); break;
                    case DocumentWhiteSpace typedItem: RenderDocumentWhiteSpace(typedItem, streamWriter); break;
                }
            }

            streamWriter.Write("</blockquote>");
        }

        private void RenderDocumentTableOfContentsObject(DocumentTableOfContentsObject obj, StreamWriter streamWriter)
        {
            foreach (var item in obj.ItemsContainer)
            {
                switch (item)
                {
                    case DocumentTitle typedItem: if (typedItem.Level > 1) streamWriter.Write($"<a class=\"tc{typedItem.Level - 1}\" href=\"#{typedItem.Id}\">{HtmlEncode(typedItem.Content)}</a><br/>"); break;
                }
            }
        }

        private void RenderDocumentText(DocumentText text, StreamWriter streamWriter)
        {
            RenderContent(text.Content, streamWriter);

            streamWriter.Write("<br/>");
        }

        private void RenderDocumentTitle(DocumentTitle title, StreamWriter streamWriter)
        {
            if (title.Level > 1)
            {
                streamWriter.Write($"<a name=\"{title.Id}\"></a>");
            }

            streamWriter.Write($"<h{title.Level}>{HtmlEncode(title.Content)}</h{title.Level}>");
        }

        private void RenderDocumentWhiteSpace(DocumentWhiteSpace whiteSpace, StreamWriter streamWriter)
        {
            streamWriter.Write("<br/>");
        }
    }
}