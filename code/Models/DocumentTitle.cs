using System.Text.RegularExpressions;

namespace BlowUp.Models
{
    public class DocumentTitle
    {
        public DocumentTitle(int level, string content, string id = null)
        {
            this.Level = level;

            this.Content = content;

            this.Id = id ?? Regex.Replace(Regex.Replace(Regex.Replace(content, "[\"\\(\\)\\[\\]\\{\\}]", string.Empty), "( +)|[/.,:;]", "-"), "-+", "-");
        }

        public string Content { get; private set; }

        public string Id { get; private set; }

        public int Level { get; private set; }
    }
}