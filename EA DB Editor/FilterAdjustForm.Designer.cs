namespace EA_DB_Editor
{
    partial class FilterAdjustForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterAdjustForm));
            this.cbTable = new System.Windows.Forms.ComboBox();
            this.RemoveFilter = new System.Windows.Forms.Button();
            this.cbField = new System.Windows.Forms.ComboBox();
            this.cbOp = new System.Windows.Forms.ComboBox();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.AddFilter = new System.Windows.Forms.Button();
            this.lvFilters = new ListViewEx.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbMass = new System.Windows.Forms.ComboBox();
            this.AddAdjust = new System.Windows.Forms.Button();
            this.RemoveAdjust = new System.Windows.Forms.Button();
            this.lvAdjust = new ListViewEx.ListViewEx();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbTable
            // 
            this.cbTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTable.FormattingEnabled = true;
            this.cbTable.Location = new System.Drawing.Point(55, 12);
            this.cbTable.Name = "cbTable";
            this.cbTable.Size = new System.Drawing.Size(414, 21);
            this.cbTable.TabIndex = 0;
            this.cbTable.SelectedIndexChanged += new System.EventHandler(this.cbTable_SelectedIndexChanged);
            // 
            // RemoveFilter
            // 
            this.RemoveFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveFilter.Location = new System.Drawing.Point(306, 170);
            this.RemoveFilter.Name = "RemoveFilter";
            this.RemoveFilter.Size = new System.Drawing.Size(163, 23);
            this.RemoveFilter.TabIndex = 2;
            this.RemoveFilter.Text = "Remove Filter";
            this.RemoveFilter.UseVisualStyleBackColor = true;
            this.RemoveFilter.Click += new System.EventHandler(this.RemoveFilter_Click);
            // 
            // cbField
            // 
            this.cbField.FormattingEnabled = true;
            this.cbField.Location = new System.Drawing.Point(-8, 103);
            this.cbField.Name = "cbField";
            this.cbField.Size = new System.Drawing.Size(121, 21);
            this.cbField.TabIndex = 3;
            this.cbField.Visible = false;
            // 
            // cbOp
            // 
            this.cbOp.FormattingEnabled = true;
            this.cbOp.Items.AddRange(new object[] {
            "=",
            ">",
            "<",
            "!=",
            "contains",
            "!contains",
            "endswith",
            "startswith"});
            this.cbOp.Location = new System.Drawing.Point(-8, 130);
            this.cbOp.Name = "cbOp";
            this.cbOp.Size = new System.Drawing.Size(121, 21);
            this.cbOp.TabIndex = 4;
            this.cbOp.Visible = false;
            // 
            // tbValue
            // 
            this.tbValue.Location = new System.Drawing.Point(2, 157);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(100, 20);
            this.tbValue.TabIndex = 5;
            this.tbValue.Visible = false;
            // 
            // AddFilter
            // 
            this.AddFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddFilter.Location = new System.Drawing.Point(12, 170);
            this.AddFilter.Name = "AddFilter";
            this.AddFilter.Size = new System.Drawing.Size(157, 23);
            this.AddFilter.TabIndex = 6;
            this.AddFilter.Text = "Add Filter";
            this.AddFilter.UseVisualStyleBackColor = true;
            this.AddFilter.Click += new System.EventHandler(this.AddFilter_Click);
            // 
            // lvFilters
            // 
            this.lvFilters.AllowColumnReorder = true;
            this.lvFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFilters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvFilters.DoubleClickActivation = true;
            this.lvFilters.FullRowSelect = true;
            this.lvFilters.GridLines = true;
            this.lvFilters.HideSelection = false;
            this.lvFilters.Location = new System.Drawing.Point(12, 39);
            this.lvFilters.Name = "lvFilters";
            this.lvFilters.Size = new System.Drawing.Size(457, 125);
            this.lvFilters.TabIndex = 1;
            this.lvFilters.UseCompatibleStateImageBehavior = false;
            this.lvFilters.View = System.Windows.Forms.View.Details;
            this.lvFilters.SubItemClicked += new ListViewEx.SubItemEventHandler(this.lvFilters_SubItemClicked);
            this.lvFilters.SubItemEndEditing += new ListViewEx.SubItemEndEditingEventHandler(this.lvFilters_SubItemEndEditing);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Field";
            this.columnHeader1.Width = 145;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Op";
            this.columnHeader2.Width = 74;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Value";
            this.columnHeader3.Width = 101;
            // 
            // cbMass
            // 
            this.cbMass.FormattingEnabled = true;
            this.cbMass.Items.AddRange(new object[] {
            "<-",
            "+/-"});
            this.cbMass.Location = new System.Drawing.Point(232, 130);
            this.cbMass.Name = "cbMass";
            this.cbMass.Size = new System.Drawing.Size(121, 21);
            this.cbMass.TabIndex = 7;
            this.cbMass.Visible = false;
            // 
            // AddAdjust
            // 
            this.AddAdjust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddAdjust.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddAdjust.Location = new System.Drawing.Point(12, 348);
            this.AddAdjust.Name = "AddAdjust";
            this.AddAdjust.Size = new System.Drawing.Size(157, 23);
            this.AddAdjust.TabIndex = 10;
            this.AddAdjust.Text = "Add Adjustment";
            this.AddAdjust.UseVisualStyleBackColor = true;
            this.AddAdjust.Click += new System.EventHandler(this.AddAdjust_Click);
            // 
            // RemoveAdjust
            // 
            this.RemoveAdjust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveAdjust.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveAdjust.Location = new System.Drawing.Point(306, 348);
            this.RemoveAdjust.Name = "RemoveAdjust";
            this.RemoveAdjust.Size = new System.Drawing.Size(163, 23);
            this.RemoveAdjust.TabIndex = 9;
            this.RemoveAdjust.Text = "Remove Adjustment";
            this.RemoveAdjust.UseVisualStyleBackColor = true;
            this.RemoveAdjust.Click += new System.EventHandler(this.RemoveAdjust_Click);
            // 
            // lvAdjust
            // 
            this.lvAdjust.AllowColumnReorder = true;
            this.lvAdjust.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvAdjust.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lvAdjust.DoubleClickActivation = true;
            this.lvAdjust.FullRowSelect = true;
            this.lvAdjust.GridLines = true;
            this.lvAdjust.HideSelection = false;
            this.lvAdjust.Location = new System.Drawing.Point(12, 203);
            this.lvAdjust.Name = "lvAdjust";
            this.lvAdjust.Size = new System.Drawing.Size(457, 139);
            this.lvAdjust.TabIndex = 8;
            this.lvAdjust.UseCompatibleStateImageBehavior = false;
            this.lvAdjust.View = System.Windows.Forms.View.Details;
            this.lvAdjust.SubItemClicked += new ListViewEx.SubItemEventHandler(this.lvAdjust_SubItemClicked);
            this.lvAdjust.SubItemEndEditing += new ListViewEx.SubItemEndEditingEventHandler(this.lvAdjust_SubItemEndEditing);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Field";
            this.columnHeader4.Width = 145;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Op";
            this.columnHeader5.Width = 74;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Value";
            this.columnHeader6.Width = 101;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Table:";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(131, 389);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(79, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRun.Location = new System.Drawing.Point(232, 389);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(79, 23);
            this.btnRun.TabIndex = 18;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // FilterAdjustForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 424);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AddAdjust);
            this.Controls.Add(this.RemoveAdjust);
            this.Controls.Add(this.lvAdjust);
            this.Controls.Add(this.cbMass);
            this.Controls.Add(this.AddFilter);
            this.Controls.Add(this.tbValue);
            this.Controls.Add(this.cbOp);
            this.Controls.Add(this.cbField);
            this.Controls.Add(this.RemoveFilter);
            this.Controls.Add(this.lvFilters);
            this.Controls.Add(this.cbTable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FilterAdjustForm";
            this.Load += new System.EventHandler(this.FilterAdjustForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbTable;
        private ListViewEx.ListViewEx lvFilters;
        private System.Windows.Forms.Button RemoveFilter;
        private System.Windows.Forms.ComboBox cbField;
        private System.Windows.Forms.ComboBox cbOp;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.Button AddFilter;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ComboBox cbMass;
        private System.Windows.Forms.Button AddAdjust;
        private System.Windows.Forms.Button RemoveAdjust;
        private ListViewEx.ListViewEx lvAdjust;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRun;
    }
}