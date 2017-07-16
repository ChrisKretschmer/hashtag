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

        public string Content { get; private set; }

        public Range Location { get; set; }

        public int Start
        {
            get { return Location.Start; }
        }

        public string HashTagList
        {
            get
            {
                var strings = HashTags.ToArray();
                Array.Sort(strings, StringComparer.InvariantCulture);
                return String.Join(", ", strings);
            }
        }

        public List<string> HashTags { get; set; }

        private void extractHashTags()
        {
            var matches = regex.Matches(Content);

            foreach (Match match in matches)
            {
                HashTags.Add(match.Value);
            }
        }
        
    }
}
