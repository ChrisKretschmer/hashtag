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
    public partial class HashTagWindow : Form
    {
        private Dictionary<string, int> currentHashTags;

        public HashTagWindow()
        {
            InitializeComponent();
        }

        public void AddHashTags(Dictionary<string, int> items)
        {
            currentHashTags = items;
            dataGridView1.Rows.Clear();
            foreach (string item in items.Keys)
            {
                var values = new object[] { item, items[item] };
                dataGridView1.Rows.Add(values);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var filter = currentHashTags.Keys.ToArray()[e.RowIndex];
            this.Hide();
            Globals.ThisDocument.openParagraphWindow(filter);
            
        }
    }
}
