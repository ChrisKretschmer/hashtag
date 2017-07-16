using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace HashTag
{
    public class Quotation
    {
        public static readonly Regex regex = new Regex(@"(?<=#)\w+", RegexOptions.Compiled);

        public Quotation(string content, Range location)
        {
            Content = content;
            Location = location;
            HashTags = new List<string>();
            extractHashTags();
        }

        public string Content { get; set; }

        public Range Location { get; set; }

        public List<string> HashTags { get; set; }

        private void extractHashTags()
        {
            var matches = regex.Matches(Content);

            foreach (Match match in matches)
            {
                HashTags.Add(match.Value);
            }
        }

        public string[] getTableRepresentation()
        {
            var result = new string[3];
            result[0] = Location.Start.ToString();
            result[1] = Content;
            result[2] = String.Join(", ", HashTags.ToArray());

            return result;
        }
    }
}
