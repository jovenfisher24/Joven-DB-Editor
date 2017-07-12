namespace EA_DB_Editor
{
	partial class FilterForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterForm));
			this.cbTable = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.cbField = new System.Windows.Forms.ComboBox();
			this.cbOp = new System.Windows.Forms.ComboBox();
			this.tbValue = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.lvFitlers = new ListViewEx.ListViewEx();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cbMass = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// cbTable
			// 
			this.cbTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbTable.FormattingEnabled = true;
			this.cbTable.Location = new System.Drawing.Point(12, 12);
			this.cbTable.Name = "cbTable";
			this.cbTable.Size = new System.Drawing.Size(326, 21);
			this.cbTable.TabIndex = 0;
			this.cbTable.SelectedIndexChanged += new System.EventHandler(this.cbTable_SelectedIndexChanged);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point(175, 266);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(163, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Remove";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
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
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Location = new System.Drawing.Point(12, 266);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(157, 23);
			this.button2.TabIndex = 6;
			this.button2.Text = "Add";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// lvFitlers
			// 
			this.lvFitlers.AllowColumnReorder = true;
			this.lvFitlers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvFitlers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.lvFitlers.DoubleClickActivation = true;
			this.lvFitlers.FullRowSelect = true;
			this.lvFitlers.GridLines = true;
			this.lvFitlers.HideSelection = false;
			this.lvFitlers.Location = new System.Drawing.Point(12, 39);
			this.lvFitlers.Name = "lvFitlers";
			this.lvFitlers.Size = new System.Drawing.Size(326, 221);
			this.lvFitlers.TabIndex = 1;
			this.lvFitlers.UseCompatibleStateImageBehavior = false;
			this.lvFitlers.View = System.Windows.Forms.View.Details;
			this.lvFitlers.SubItemClicked += new ListViewEx.SubItemEventHandler(this.lvFitlers_SubItemClicked);
			this.lvFitlers.SubItemEndEditing += new ListViewEx.SubItemEndEditingEventHandler(this.lvFitlers_SubItemEndEditing);
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
			// FilterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(350, 301);
			this.Controls.Add(this.cbMass);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.tbValue);
			this.Controls.Add(this.cbOp);
			this.Controls.Add(this.cbField);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.lvFitlers);
			this.Controls.Add(this.cbTable);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FilterForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FilterForm_FormClosing);
			this.Load += new System.EventHandler(this.FilterForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cbTable;
		private ListViewEx.ListViewEx lvFitlers;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox cbField;
		private System.Windows.Forms.ComboBox cbOp;
		private System.Windows.Forms.TextBox tbValue;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ComboBox cbMass;
	}
}