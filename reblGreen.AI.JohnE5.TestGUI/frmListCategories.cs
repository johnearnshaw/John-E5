using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using reblGreen.AI.JohnE5;
using reblGreen.AI.JohnE5.Interfaces;

namespace TestJohnE5
{
    public partial class frmListCategories : Form
    {
        public frmListCategories()
        {
            InitializeComponent();
        }

        private void ListCategories_Load(object sender, EventArgs e)
        {
            List<IRankedCategory> categories = Program.Classifier.Model.GetAllCategories();

            foreach (var cat in categories)
            {
                var catText = cat.Name;

                textBox1.Text += string.Format("{0} (wordcount: {1})\r\n", cat.Name, (int)cat.Score);
            }
        }
    }
}
