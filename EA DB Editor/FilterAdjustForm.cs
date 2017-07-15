using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EA_DB_Editor
{
    public partial class FilterAdjustForm : Form
    {
        public List<Field> lMappedFields = new List<Field>();
        public List<MaddenTable> lMappedTables = new List<MaddenTable>();
        public List<View> lMappedViews = new List<View>();
        public View view = null;
        public List<FieldFilter> lFilters = new List<FieldFilter>();
        public string text = "";


        public FilterAdjustForm(List<Field> lMF, List<MaddenTable> lMT, List<View> lMV, string title)
        {
            InitializeComponent();

            lMappedFields = lMF;
            lMappedTables = lMT;
            lMappedViews = lMV;

            
            text = title;

            foreach (View v in lMappedViews)
            {
                if (v.Type == "Grid")
                    cbTable.Items.Add(v.Name);
            }

            cbTable.SelectedIndex = 0;
        }
        private void FilterAdjustForm_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            this.Text = text;
        }
        private void FilterAdjustForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (ListViewItem lvi in lvFitlers.Items)
            {
                lFilters.Add(new FieldFilter(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text));
            }
        }

        private void cbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            view = View.FindView(lMappedViews, cbTable.SelectedItem.ToString());

            lvFitlers.Items.Clear();
            lvAdjust.Items.Clear();
            cbField.Items.Clear();

            if (view.lChildFields.Count > 0)
            {
                foreach (Field f in view.lChildFields)
                    cbField.Items.Add((f.Name != "") ? f.Name : f.Abbreviation);

            }
            else
            {
                MaddenTable table = MaddenTable.FindTable(lMappedTables, view.SourceName);
                if (table == null)
                    return;

                foreach (Field f in table.lFields)
                    cbField.Items.Add((f.Name != "") ? f.Name : f.Abbreviation);
            }
        }
        private void RemoveFilter_Click(object sender, EventArgs e)
        {
            if (lvFitlers.Items.Count > 0)
                lvFitlers.Items.RemoveAt(lvFitlers.Items.Count - 1);
        }
        private void AddFilter_Click(object sender, EventArgs e)
        {
            string cb = cbOp.Items[0].ToString();
            BetterListViewNS.BetterListView.AddToListView(lvFitlers, null, lvFitlers.Items.Count, cbField.Items[0].ToString(), cb, "");
        }
        private void lvFitlers_SubItemClicked(object sender, ListViewEx.SubItemEventArgs e)
        {
            switch (e.SubItem)
            {
                case 0:
                    lvFitlers.StartEditing(cbField, e.Item, e.SubItem);
                    break;
                case 1:
                    lvFitlers.StartEditing(cbOp, e.Item, e.SubItem);
                    break;
                case 2:
                    Field f = Field.FindField(lMappedFields, e.Item.SubItems[0].Text);

                    if (f.ControlType != "ComboBox")
                        lvFitlers.StartEditing(tbValue, e.Item, e.SubItem);
                    else
                    {
                        if (e.Item.SubItems[1].Text.Length > 2)
                            lvFitlers.StartEditing(tbValue, e.Item, e.SubItem);
                        else
                        {
                            f.EditControl.Parent = lvFitlers;
                            lvFitlers.StartEditing(f.EditControl, e.Item, e.SubItem);
                        }
                    }
                    break;
            }
        }
        private void lvAdjust_SubItemClicked(object sender, ListViewEx.SubItemEventArgs e)
        {
            switch (e.SubItem)
            {
                case 0:
                    lvAdjust.StartEditing(cbField, e.Item, e.SubItem);
                    break;
                case 1:
                    lvAdjust.StartEditing(cbMass, e.Item, e.SubItem);
                    break;
                case 2:
                    Field f = Field.FindField(lMappedFields, e.Item.SubItems[0].Text);
                    lvAdjust.StartEditing(tbValue, e.Item, e.SubItem);
                    break;
            }
        }
        private void lvFitlers_SubItemEndEditing(object sender, ListViewEx.SubItemEndEditingEventArgs e)
        {
            if (e.SubItem == 2)
            {
                Field f = Field.FindField(lMappedFields, e.Item.SubItems[0].Text);

                if (f.ControlType == "ComboBox" && e.Item.SubItems[1].Text.Length <= 2)
                {
                    if (f.ControlLink == "")
                        e.DisplayText = ((ComboBox)f.EditControl).SelectedIndex.ToString();
                    else
                        e.DisplayText = ((RefObj)((ComboBox)f.EditControl).SelectedItem).key;
                }
            }
        }

        private void lvAdjust_SubItemEndEditing(object sender, ListViewEx.SubItemEndEditingEventArgs e)
        {
            if (e.SubItem == 2)
            {
                Field f = Field.FindField(lMappedFields, e.Item.SubItems[0].Text);

                if (f.ControlType == "ComboBox" && e.Item.SubItems[1].Text.Length <= 2)
                {
                    if (f.ControlLink == "")
                        e.DisplayText = ((ComboBox)f.EditControl).SelectedIndex.ToString();
                    else
                        e.DisplayText = ((RefObj)((ComboBox)f.EditControl).SelectedItem).key;
                }
            }
        }


        private void AddAdjust_Click(object sender, EventArgs e)
        {
            string cb =  cbMass.Items[0].ToString();
            BetterListViewNS.BetterListView.AddToListView(lvAdjust, null, lvAdjust.Items.Count, cbField.Items[0].ToString(), cb, "");
        }

        private void RemoveAdjust_Click(object sender, EventArgs e)
        {
            if (lvAdjust.Items.Count > 0)
                lvAdjust.Items.RemoveAt(lvAdjust.Items.Count - 1);
        }

    }
}
