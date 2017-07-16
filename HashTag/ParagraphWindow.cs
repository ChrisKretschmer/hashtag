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

        private BindingSource bindingSource1 = new BindingSource();

        protected DataGridView dataGridView1;

        private string currentFilter = "";


        public ParagraphWindow()
        {
            InitializeComponent();
            currentQuotations = new List<Quotation>();
            filteredQuotations = new List<Quotation>();
            dataGridView1 = new DataGridView();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSize = true;
            splitContainer1.Panel1.Controls.Add(dataGridView1);
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.DataSource = bindingSource1;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;

            // Initialize and add a text box column.
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Start";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.SortMode = DataGridViewColumnSortMode.Automatic;
            column.FillWeight = 20;
            column.Name = "Position";
            dataGridView1.Columns.Add(column);

            // Initialize and add a text box column.
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Content";
            column.Name = "Content";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridView1.Columns.Add(column);

            // Initialize and add a text box column.
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "HashTagList";
            column.SortMode = DataGridViewColumnSortMode.Automatic;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.Name = "HashTags";
            dataGridView1.Columns.Add(column);


            
        }

        

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedItem = (Quotation)dataGridView1.SelectedRows[0]?.DataBoundItem;
            if (selectedItem != null)
            {
                selectedItem.Location.Select();
                this.Hide();
            }
        }

        public void setFilter(string filter, bool refreshBox = true) { 
        
            List<Quotation> quotationsToShow;
            currentFilter = filter;

            if (!string.IsNullOrEmpty(filter))
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
                if (!string.IsNullOrEmpty(filter))
                {
                    textBox1.Text = filter;
                }
                else
                {
                    textBox1.Text = "";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           loadQuotations();
        }

        private void increaseProgress(int max, int i)
        {
            progressBar1.Maximum = max;
            progressBar1.Value = i;
        }

        private void loadQuotations()
        {
            progressBar1.Visible = true;
            progressBar1.Value = 0;
            var quotations = Globals.ThisDocument.GetParagraphs(increaseProgress);
            progressBar1.Visible = false;
            currentQuotations = quotations;
            setFilter(currentFilter);
        }


        private void AddQuotaitons(List<Quotation> items)
        {

            bindingSource1.Clear();
            foreach (Quotation item in items)
            {
                bindingSource1.Add(item);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            setFilter(textBox1.Text, false);
        }
    }
}
