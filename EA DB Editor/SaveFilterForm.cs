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
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            switch (fmAction)
            {
                case SaveAction.Save:
                    SavedCriteria sc = new SavedCriteria("test", "This is a test", table.Abbreviation, listFilters, adjustFilters);
                    XmlSerialization.WriteToXmlFile<SavedCriteria>("SavedFilter.txt", sc, false);
                    break;
                case SaveAction.Load:
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
        public string table = "";
        public List<FieldFilter> listFilters = null;
        public List<FieldFilter> adjustFilters = null;
        

        public SavedCriteria ()
        {
        }

        public SavedCriteria (string sname, string desc, string mt, List<FieldFilter> lsf, List<FieldFilter> adf)
        {
            Name = sname;
            Description = desc;
            table = mt;
            listFilters = lsf;
            adjustFilters = adf;
        }
    }
}
