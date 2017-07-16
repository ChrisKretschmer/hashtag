using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HashTag
{
    public partial class ParagraphWindow : Form
    {
        private List<Quotation> currentQuotations;
        private List<Quotation> filteredQuotations;

        public ParagraphWindow()
        {
            InitializeComponent();
            currentQuotations = new List<Quotation>();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(currentQuotations[e.RowIndex].Content);
            filteredQuotations[e.RowIndex].Location.Select();
            this.Hide();
        }

        public void setFilter(string filter, bool refreshBox = true)
        {
            List<Quotation> quotationsToShow;
            if (filter != null)
            {
                quotationsToShow = currentQuotations.FindAll(item => item.HashTags.Contains(filter));
            }
            else
            {
                quotationsToShow = currentQuotations;
            }
            AddQuotaitons(quotationsToShow);
            filteredQuotations = quotationsToShow;
            if (refreshBox)
            {
                textBox1.Text = filter;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            progressBar1.Value = 0;
            var quotations = Globals.ThisDocument.GetParagraphs(increaseProgress);
            progressBar1.Visible = false;
            currentQuotations = quotations;
            AddQuotaitons(quotations);
        }

        private void increaseProgress(int max, int i)
        {
            progressBar1.Maximum = max;
            progressBar1.Value = i;
        }


        private void AddQuotaitons(List<Quotation> items)
        {
            
            dataGridView1.Rows.Clear();
            foreach (Quotation item in items)
            {
                dataGridView1.Rows.Add(item.getTableRepresentation());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            setFilter(textBox1.Text, false);
        }
    }
}
