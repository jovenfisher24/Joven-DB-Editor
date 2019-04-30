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
        public MaddenTable table = null;
        public List<FieldFilter> lFilters = new List<FieldFilter>();
        public List<FieldFilter> aFilters = new List<FieldFilter>();
        public string text = "";
        public bool isViewInConfig = true;


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

        public FilterAdjustForm(List<Field> lMF, List<MaddenTable> lMT, List<View> lMV, string title, string MTabbr, List<FieldFilter> lFltr, List<FieldFilter> aFltr)
        {
            InitializeComponent();

            lMappedFields = lMF;
            lMappedTables = lMT;
            lMappedViews = lMV;
            text = title;

            // TODO: add try..catch
            
            
            table = MaddenTable.GetTableByAbbreviation(lMappedTables, MTabbr);
            string tName = table.Name == "" ? table.Abbreviation : table.Name;

            foreach (View v in lMappedViews)
            {
                if (v.Type == "Grid")
                    cbTable.Items.Add(v.Name);
            }

            Console.Write(FindViewbyFieldFilter(aFltr, tName));

            if (cbTable.Items.Contains(tName))
            {
                cbTable.SelectedIndex = cbTable.Items.IndexOf(tName);
            }
            else if (lFltr.Count == 0 )
            {
                cbTable.SelectedIndex = cbTable.Items.IndexOf(FindViewbyFieldFilter(aFltr, tName));
            }
            else if (aFltr.Count == 0 || isFieldFilterinView(aFltr, FindViewbyFieldFilter(lFltr, tName)))
            {
                cbTable.SelectedIndex = cbTable.Items.IndexOf(FindViewbyFieldFilter(lFltr, tName));
            }
            
            else
            {
                //if (table.Name == table.Abbreviation)
                //{
                //TODO: Create new view
                isViewInConfig = false;
                View tbview = new View();
                tbview.Name = table.Abbreviation;
                tbview.Type = "Grid";
                tbview.SourceName = table.Abbreviation;
                tbview.SourceType = "Table";
                List<string> cf = new List<string>();
                foreach (Field f in table.lFields)
                {
                    cf.Add(f.name);
                }
                tbview.ChildFields = cf;
                lMappedViews.Insert(0, tbview);
                cbTable.Items.Add(tbview.Name);
                cbTable.SelectedIndex = cbTable.Items.IndexOf(tbview.Name);
                //}
                MessageBox.Show("The Table for this filter is not mapped in this config. You will not be able to view the changes.");
            }

            foreach (FieldFilter ff in lFltr)
            {
                
                Field mf = Field.GetFieldByAbbreviation(lMappedFields, ff.field);
                string fName = mf.Name == "" ? mf.Abbreviation : mf.Name;
                BetterListViewNS.BetterListView.AddToListView(lvFilters, null, lvFilters.Items.Count, fName, ff.OperationToText(), ff.value.ToString());            
            }

            foreach (FieldFilter ff in aFltr)
            {
                
                Field mf = Field.GetFieldByAbbreviation(lMappedFields, ff.field);
                string fName = mf.Name == "" ? mf.Abbreviation : mf.Name;
                BetterListViewNS.BetterListView.AddToListView(lvAdjust, null, lvAdjust.Items.Count, fName, ff.OperationToText(), ff.value.ToString(), ff.min.ToString(), ff.max.ToString());
            }
        }

        private void FilterAdjustForm_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            this.Text = text;
        }

        private string FindViewbyFieldFilter (List<FieldFilter> ffilter, string tn)
        {
            ///for each view with tablename as source, for each
            foreach (View v in lMappedViews)
            {
                if (v.SourceName == tn)
                {
                    foreach (FieldFilter ff in ffilter)
                    {
                        Field mf = Field.GetFieldByAbbreviation(lMappedFields, ff.field);
                        if (v.lChildFields.Contains(mf))
                        {
                            return v.Name;
                        }
                    }
                }
            }
            return tn;
        }
        private bool isFieldFilterinView(List<FieldFilter> ffilter, string vm)
        {
            View v = View.FindView(lMappedViews, vm);

            if (v != null)
            {
                foreach (FieldFilter ff in ffilter)
                {
                    Field mf = Field.GetFieldByAbbreviation(lMappedFields, ff.field);
                    if (v.lChildFields.Contains(mf))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void runFilters()
        {
            //Clear old items from filter list
            lFilters.Clear();
            foreach (ListViewItem lvi in lvFilters.Items)
            {
                Field f = Field.FindField(lMappedFields, lvi.SubItems[0].Text);
                lFilters.Add(new FieldFilter(f.Abbreviation, lvi.SubItems[1].Text, lvi.SubItems[2].Text));
            }

            //Clear old items from adjust list
            aFilters.Clear();
            foreach (ListViewItem lvi in lvAdjust.Items)
            {
                Field f = Field.FindField(lMappedFields, lvi.SubItems[0].Text);
                aFilters.Add(new FieldFilter(f.Abbreviation, lvi.SubItems[1].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text, lvi.SubItems[4].Text));
            }
        }

        private void cbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            view = View.FindView(lMappedViews, cbTable.SelectedItem.ToString());

            lvFilters.Items.Clear();
            lvAdjust.Items.Clear();
            cbField.Items.Clear();

            table = MaddenTable.FindTable(lMappedTables, view.SourceName);
            if (view.lChildFields.Count > 0)
            {
                foreach (Field f in view.lChildFields)
                    cbField.Items.Add((f.Name != "") ? f.Name : f.Abbreviation);
            }
            else
            {
                if (table == null)
                    return;

                foreach (Field f in table.lFields)
                    cbField.Items.Add((f.Name != "") ? f.Name : f.Abbreviation);
            }
            
        }
        private void RemoveFilter_Click(object sender, EventArgs e)
        {
            if (lvFilters.Items.Count > 0)
                lvFilters.Items.RemoveAt(lvFilters.Items.Count - 1);
        }
        private void AddFilter_Click(object sender, EventArgs e)
        {
            string cb = cbOp.Items[0].ToString();
            BetterListViewNS.BetterListView.AddToListView(lvFilters, null, lvFilters.Items.Count, cbField.Items[0].ToString(), cb, "");
        }
        private void lvFilters_SubItemClicked(object sender, ListViewEx.SubItemEventArgs e)
        {
            switch (e.SubItem)
            {
                case 0:
                    lvFilters.StartEditing(cbField, e.Item, e.SubItem);
                    break;
                case 1:
                    lvFilters.StartEditing(cbOp, e.Item, e.SubItem);
                    break;
                case 2:
                    Field f = Field.FindField(lMappedFields, e.Item.SubItems[0].Text);

                    if (f.ControlType != "ComboBox")
                        lvFilters.StartEditing(tbValue, e.Item, e.SubItem);
                    else
                    {
                        if (e.Item.SubItems[1].Text.Length > 2)
                            lvFilters.StartEditing(tbValue, e.Item, e.SubItem);
                        else
                        {
                            f.EditControl.Parent = lvFilters;
                            lvFilters.StartEditing(f.EditControl, e.Item, e.SubItem);
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

                    if (f.ControlType != "ComboBox")
                        lvAdjust.StartEditing(tbValue, e.Item, e.SubItem);
                    else
                    {
                        if (e.Item.SubItems[1].Text.Length > 2)
                            lvAdjust.StartEditing(tbValue, e.Item, e.SubItem);
                        else
                        {
                            f.EditControl.Parent = lvAdjust;
                            lvAdjust.StartEditing(f.EditControl, e.Item, e.SubItem);
                        }
                    }
                    break;
               case 3:
                    lvAdjust.StartEditing(tbValue, e.Item, e.SubItem);
                    break;
               case 4:
                    lvAdjust.StartEditing(tbValue, e.Item, e.SubItem);
                    break;
            }
        }
        private void lvFilters_SubItemEndEditing(object sender, ListViewEx.SubItemEndEditingEventArgs e)
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
            BetterListViewNS.BetterListView.AddToListView(lvAdjust, null, lvAdjust.Items.Count, cbField.Items[0].ToString(), cb, "", "", "");
        }

        private void RemoveAdjust_Click(object sender, EventArgs e)
        {
            if (lvAdjust.Items.Count > 0)
                lvAdjust.Items.RemoveAt(lvAdjust.Items.Count - 1);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            runFilters();
            SaveFilterForm sf = new SaveFilterForm(lFilters, aFilters, table, SaveFilterForm.SaveAction.Save, "");
            sf.ShowDialog();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            runFilters();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
