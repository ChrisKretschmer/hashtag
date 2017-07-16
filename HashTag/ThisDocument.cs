using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Tools.Word;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;

namespace HashTag
{
    public partial class ThisDocument
    {
        private ParagraphWindow paragraphWindow;
        private HashTagWindow hashTagWindow;

        private void ThisDocument_Startup(object sender, System.EventArgs e)
        {
            this.Application.DocumentBeforeSave +=
                new Word.ApplicationEvents4_DocumentBeforeSaveEventHandler(Application_DocumentBeforeSave);
        }

        private void ThisDocument_Shutdown(object sender, System.EventArgs e)
        {
            paragraphWindow?.Close();
            hashTagWindow?.Close();

            paragraphWindow?.Dispose();
            hashTagWindow?.Dispose();

        }

        void Application_DocumentBeforeSave(Word.Document Doc, ref bool SaveAsUI, ref bool Cancel)
        {

        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new Ribbon1();
        }

        public void openHashTagWindow()
        {
            var results = GetHashTags();
            if (hashTagWindow == null)
            {
                hashTagWindow = new HashTagWindow();

            }

            hashTagWindow.AddHashTags(results);

            hashTagWindow.ShowDialog();

        }

        public void openParagraphWindow()
        {
            openParagraphWindow(null);
        }

        public void openParagraphWindow(string filter)
        {

            if (paragraphWindow == null)
            {
                paragraphWindow = new ParagraphWindow();

            }
            paragraphWindow.setFilter(filter);
            paragraphWindow.ShowDialog();
        }

        public List<Quotation> GetParagraphs(Action<int, int> increaseProgress)
        {
            var foundHashTags = new List<Quotation>();
            var doc = ThisApplication.ActiveDocument;
            var paragraphs = doc.Paragraphs;

            for (int i = 0; i < paragraphs.Count; i++)
            {
                increaseProgress(paragraphs.Count, i);
                var currentParagraph = paragraphs[i + 1];
                string temp = currentParagraph.Range.Text.Trim();
                if (temp != string.Empty)
                {
                    var hashTag = new Quotation(temp, currentParagraph.Range);
                    foundHashTags.Add(hashTag);
                }

            }

            return foundHashTags;

        }

        public Dictionary<string, int> GetHashTags()
        {
            var result = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
            var content = ThisApplication.ActiveDocument.Content.Text;
            var wordPattern = new Regex(@"(?<=#)\w+");

            foreach (Match match in wordPattern.Matches(content))
            {
                int currentCount = 0;
                result.TryGetValue(match.Value, out currentCount);

                currentCount++;
                result[match.Value] = currentCount;
            }

            return result;

        }

        #region Vom VSTO-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisDocument_Startup);
            this.Shutdown += new System.EventHandler(ThisDocument_Shutdown);
        }

        #endregion
    }
}
