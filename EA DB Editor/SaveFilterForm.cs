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
    public partial class SaveFilterForm : Form
    {
        public List<FieldFilter> listFilters = null;
        public List<FieldFilter> adjustFilters = null;
        public MaddenTable table = null;
        public string text = "";
        public SaveAction fmAction = SaveAction.Save;
        public List<SavedCriteria> lSavedCriteria = null;
        public SavedCriteria savedCriteria = null;

        public enum SaveAction
        {
            Save,
            Load
        }

        public SaveFilterForm(List<FieldFilter> lsF, List<FieldFilter> adF, MaddenTable mt, SaveAction action, string title)
        {
            InitializeComponent();

            listFilters = lsF;
            adjustFilters = adF;
            table = mt;
            text = title;
            fmAction = action;

            try
            {
                lSavedCriteria = XmlSerialization.ReadFromXmlFile<List<SavedCriteria>>("SavedFilter.txt");
                foreach (SavedCriteria sc in lSavedCriteria)
                {
                    cboSavedName.Items.Add(sc.Name);
                }
                cboSavedName.SelectedIndex = 0;
            }
            catch
            {
                MessageBox.Show("Could not read SavedFilter.txt");
            }
        }

        public SaveFilterForm(SaveAction action, string title)
        {
            InitializeComponent();
            
            text = title;
            fmAction = action;

            try
            {
                lSavedCriteria = XmlSerialization.ReadFromXmlFile<List<SavedCriteria>>("SavedFilter.txt");
                foreach (SavedCriteria sc in lSavedCriteria)
                {
                    cboSavedName.Items.Add(sc.Name);
                }
                cboSavedName.SelectedIndex = 0;
            }
            catch (System.IO.FileNotFoundException e)
            {
                MessageBox.Show("Did not find file 'SavedFilter.txt'. Saving a Filter will automatically create a new file in the same directory as the editor.");
            }

            catch
            {
                MessageBox.Show("Could not read 'SavedFilter.txt'");
                this.Close();
            }
        }

        private void SaveFilterForm_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            this.Text = text;
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            switch (fmAction)
            {
                case SaveAction.Save:

                    savedCriteria = new SavedCriteria(cboSavedName.Text, txtDescription.Text, table.Abbreviation, listFilters, adjustFilters);

                    if (lSavedCriteria == null)
                    {
                        lSavedCriteria = new List<SavedCriteria>() { savedCriteria };
                    }

                    else if (cboSavedName.Items.Contains(cboSavedName.Text))
                    {
                        DialogResult dialogResult = MessageBox.Show("This Criteria already exists! Do you want to overwrite?", "Save Confirm", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.Yes)
                        {
                            lSavedCriteria.RemoveAt(cboSavedName.Items.IndexOf(cboSavedName.Text));
                            lSavedCriteria.Add(savedCriteria);
                        }
                    }
                    else
                    {
                        lSavedCriteria.Add(savedCriteria);
                    }

                    XmlSerialization.WriteToXmlFile<List<SavedCriteria>>("SavedFilter.txt", lSavedCriteria, false);
                    break;
                case SaveAction.Load:
                    savedCriteria = lSavedCriteria[cboSavedName.SelectedIndex];
                    break;
            }
            this.Close();
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class SavedCriteria
    {
        public string Name = "Untitled";
        public string Description = "";
        public string Table = "";
        public List<FieldFilter> listFilters = null;
        public List<FieldFilter> adjustFilters = null;
        

        public SavedCriteria ()
        {
        }

        public SavedCriteria (string sname, string desc, string mt, List<FieldFilter> lsf, List<FieldFilter> adf)
        {
            Name = sname;
            Description = desc;
            Table = mt;
            listFilters = lsf;
            adjustFilters = adf;
        }
    }
}
