using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using reblGreen.AI.JohnE5;
using reblGreen.AI.JohnE5.Classes;
using reblGreen.AI.JohnE5.Interfaces;

namespace TestJohnE5
{
    public partial class frmTest : Form
    {
        private List<string> currentTokens;
        private List<IToken> keywords = new List<IToken>();
        List<IRankedCategory> categories = new List<IRankedCategory>();

        Dictionary<CheckBox, ComboBox> goodCheckboxes = new Dictionary<CheckBox, ComboBox>();

        public frmTest()
        {
            InitializeComponent();

            btnAddCategory.Enabled = false;
            btnClassify.Enabled = false;
            btnClear.Enabled = false;
            btnSave.Enabled = false;

            textBox1.KeyDown += textBox1_KeyDown;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.A))
            {
                if (sender != null)
                    ((TextBox)sender).SelectAll();
                e.Handled = true;
            }
        }


        private void btnAnalyse_Click(object sender, EventArgs e)
        {
            goodCheckboxes.Clear();

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                currentTokens = textBox1.Text.Split(' ').ToList();
                categories = Program.Classifier.GetWeightedCategories(currentTokens.ToArray());

                btnAddCategory.Enabled = true;
                btnSave.Enabled = true;

                UpdateCategoriesPanel();

                Console.WriteLine("analysed.");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            btnAddCategory.Enabled = false;
            btnSave.Enabled = false;

            foreach (KeyValuePair<CheckBox, ComboBox> kv in goodCheckboxes)
            {
                if (kv.Key.Checked)
                {
                    ICategory cat = Program.Classifier.Model.AddCategory(kv.Key.Name);
                    if (kv.Value.SelectedIndex > 0)
                    {
                        cat.SetParent((ICategory)kv.Value.SelectedItem);
                    }

                    Program.Classifier.TrainModel(cat, currentTokens.ToArray());
                }
            }

            categories.Clear();
            panelScoreChanges.Controls.Clear();

            List<IRankedCategory> suggestion = Program.Classifier.GetTrainingSuggestions();
            if (suggestion.Count > 1)
                label1.Text = "NEED INPUT!!! Train me more " + suggestion[0].Name.ToUpper() + " or teach me more topics!";
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string catName = null;
            if (InputBox.Show("Add Category", "Add a new category for this block of text", ref catName) == System.Windows.Forms.DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(catName))
                {
                    var existing = categories.FirstOrDefault(c => c.Name == catName);

                    if (existing == null)
                    {
                        var cat = Program.Classifier.Model.AddCategory(catName);
                        existing = RankedCategory.FromCategory(cat);
                        categories.Add(existing);
                    }

                    UpdateCategoriesPanel(true);
                }
            }
        }

        private void UpdateCategoriesPanel(bool newCat = false)
        {
            if (!newCat)
                panelScoreChanges.Controls.Clear();

            foreach (var cat in categories)
            {
                if (goodCheckboxes.Count(x => x.Key.Name == cat.Name) == 0)
                {
                    Panel panel = new Panel();
                    Label label = new Label();
                    Label score = new Label();
                    ComboBox combo = new ComboBox();
                    CheckBox good = new CheckBox();

                    panel.BackColor = SystemColors.ControlLight;
                    panel.Width = 200;
                    panel.Height = 90;

                    label.Text = cat.Name;
                    label.Top = 5;
                    label.Left = 5;
                    label.Width = 135;
                    label.Height = 30;
                    label.AutoSize = false;
                    panel.Controls.Add(label);

                    score.Text = (cat.Score * 100).ToString("0.00") + "%";
                    score.Top = 5;
                    score.Left = 140;
                    score.Width = 50;
                    score.Height = 30;
                    score.TextAlign = ContentAlignment.TopRight;
                    score.AutoSize = false;
                    panel.Controls.Add(score);


                    good.Text = "train";
                    good.Name = cat.Name;
                    good.Top = 65;
                    good.Left = 5;
                    good.Width = 190;
                    good.Checked = newCat;
                    good.Click += GoodCheckClick;
                    goodCheckboxes.Add(good, combo);
                    panel.Controls.Add(good);

                    combo.Items.Add("(parent)");

                    var cats = Program.Classifier.Model.GetAllCategories();

                    if (cats != null && cats.Count > 0)
                    {
                        foreach (var c in cats)
                        {
                            if (c.Name != cat.Name && !combo.Items.Contains(cat))
                                combo.Items.Add(c);
                        }
                    }

                    if (cat.ParentCategory > -1)
                    {
                        ICategory parent = Program.Classifier.Model.GetCategory(cat.ParentCategory);
                        if (parent != null)
                        {
                            if (!combo.Items.Contains(parent))
                            {
                                combo.Items.Add(parent);
                            }
                        }

                        combo.SelectedItem = parent;
                        combo.Enabled = false;
                    }
                    else
                    {
                        combo.SelectedIndex = 0;
                    }

                    combo.DropDownStyle = ComboBoxStyle.DropDownList;
                    combo.Top = 35;
                    combo.Left = 5;
                    combo.Width = 190;
                    panel.Controls.Add(combo);

                    panelScoreChanges.Controls.Add(panel);

                    if (newCat)
                    {
                        panel.BackColor = SystemColors.ControlLightLight;
                        panelScoreChanges.Controls.SetChildIndex(panel, 0);
                    }
                }
            }
        }

        private void GoodCheckClick(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;

            foreach (KeyValuePair<CheckBox, ComboBox> kv in goodCheckboxes)
            {
                if (kv.Key != chk)
                {
                    kv.Key.Checked = false;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                categories.Clear();
                panelScoreChanges.Controls.Clear();

                btnAddCategory.Enabled = false;
                btnClassify.Enabled = true;
                btnClear.Enabled = true;
                btnSave.Enabled = false;
            }
            else
            {
                btnAddCategory.Enabled = false;
                btnClassify.Enabled = false;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            categories.Clear();
            panelScoreChanges.Controls.Clear();

            textBox1.Text = null;
            btnAddCategory.Enabled = false;
            btnClassify.Enabled = false;
            btnClear.Enabled = false;
            btnSave.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form frm = new frmListCategories();
            frm.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you shure you want to delete my brain? This will wipe all current training data.", "Wipe my brain",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                Program.Classifier.Model.Categories.Clear();
                Program.Classifier.Model.Tokens.Clear();
                Program.Classifier.Model.Save();
            }
        }
    }
}
