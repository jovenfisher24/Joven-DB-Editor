namespace EA_DB_Editor
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.createConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.massToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.headerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.iOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allVisibleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.asNewItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.overwriteExistingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.overwriteSelectedminusKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.recalcMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.skipChecksumRecalcExNCAAOnOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.refreshViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.filterToolStripMenuItem,
            this.massToolStripMenuItem,
            this.headerToolStripMenuItem,
            this.iOToolStripMenuItem,
            this.recalcMapsToolStripMenuItem,
            this.refreshViewToolStripMenuItem,
            this.skipChecksumRecalcExNCAAOnOffToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(823, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadConfigToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.createConfigToolStripMenuItem,
            this.copyConfigToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// loadConfigToolStripMenuItem
			// 
			this.loadConfigToolStripMenuItem.Name = "loadConfigToolStripMenuItem";
			this.loadConfigToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.loadConfigToolStripMenuItem.Text = "&Load Config";
			this.loadConfigToolStripMenuItem.Click += new System.EventHandler(this.loadConfigToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.saveAsToolStripMenuItem.Text = "Save &As";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// createConfigToolStripMenuItem
			// 
			this.createConfigToolStripMenuItem.Name = "createConfigToolStripMenuItem";
			this.createConfigToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.createConfigToolStripMenuItem.Text = "&Create Config";
			this.createConfigToolStripMenuItem.Click += new System.EventHandler(this.createConfigToolStripMenuItem_Click);
			// 
			// copyConfigToolStripMenuItem
			// 
			this.copyConfigToolStripMenuItem.Name = "copyConfigToolStripMenuItem";
			this.copyConfigToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.copyConfigToolStripMenuItem.Text = "Copy Config";
			this.copyConfigToolStripMenuItem.Click += new System.EventHandler(this.copyConfigToolStripMenuItem_Click);
			// 
			// filterToolStripMenuItem
			// 
			this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
			this.filterToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
			this.filterToolStripMenuItem.Text = "Fi&lter";
			this.filterToolStripMenuItem.Click += new System.EventHandler(this.filterToolStripMenuItem_Click);
			// 
			// massToolStripMenuItem
			// 
			this.massToolStripMenuItem.Name = "massToolStripMenuItem";
			this.massToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
			this.massToolStripMenuItem.Text = "&Mass";
			this.massToolStripMenuItem.Click += new System.EventHandler(this.massToolStripMenuItem_Click);
			// 
			// headerToolStripMenuItem
			// 
			this.headerToolStripMenuItem.Name = "headerToolStripMenuItem";
			this.headerToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.headerToolStripMenuItem.Text = "&Header";
			this.headerToolStripMenuItem.Click += new System.EventHandler(this.headerToolStripMenuItem_Click);
			// 
			// iOToolStripMenuItem
			// 
			this.iOToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.importToolStripMenuItem});
			this.iOToolStripMenuItem.Name = "iOToolStripMenuItem";
			this.iOToolStripMenuItem.Size = new System.Drawing.Size(36, 20);
			this.iOToolStripMenuItem.Text = "I/O";
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedToolStripMenuItem,
            this.allVisibleToolStripMenuItem});
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
			this.exportToolStripMenuItem.Text = "E&xport";
			// 
			// selectedToolStripMenuItem
			// 
			this.selectedToolStripMenuItem.Name = "selectedToolStripMenuItem";
			this.selectedToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
			this.selectedToolStripMenuItem.Text = "Single";
			this.selectedToolStripMenuItem.Click += new System.EventHandler(this.selectedToolStripMenuItem_Click);
			// 
			// allVisibleToolStripMenuItem
			// 
			this.allVisibleToolStripMenuItem.Name = "allVisibleToolStripMenuItem";
			this.allVisibleToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
			this.allVisibleToolStripMenuItem.Text = "All Visible";
			this.allVisibleToolStripMenuItem.Click += new System.EventHandler(this.allVisibleToolStripMenuItem_Click);
			// 
			// importToolStripMenuItem
			// 
			this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asNewItemsToolStripMenuItem,
            this.overwriteExistingToolStripMenuItem,
            this.overwriteSelectedminusKeyToolStripMenuItem});
			this.importToolStripMenuItem.Name = "importToolStripMenuItem";
			this.importToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
			this.importToolStripMenuItem.Text = "Import";
			// 
			// asNewItemsToolStripMenuItem
			// 
			this.asNewItemsToolStripMenuItem.Name = "asNewItemsToolStripMenuItem";
			this.asNewItemsToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
			this.asNewItemsToolStripMenuItem.Text = "As New Items";
			this.asNewItemsToolStripMenuItem.Click += new System.EventHandler(this.asNewItemsToolStripMenuItem_Click);
			// 
			// overwriteExistingToolStripMenuItem
			// 
			this.overwriteExistingToolStripMenuItem.Name = "overwriteExistingToolStripMenuItem";
			this.overwriteExistingToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
			this.overwriteExistingToolStripMenuItem.Text = "Overwrite Existing";
			this.overwriteExistingToolStripMenuItem.Click += new System.EventHandler(this.overwriteExistingToolStripMenuItem_Click);
			// 
			// overwriteSelectedminusKeyToolStripMenuItem
			// 
			this.overwriteSelectedminusKeyToolStripMenuItem.Name = "overwriteSelectedminusKeyToolStripMenuItem";
			this.overwriteSelectedminusKeyToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
			this.overwriteSelectedminusKeyToolStripMenuItem.Text = "Overwrite Selected (minus key)";
			this.overwriteSelectedminusKeyToolStripMenuItem.Click += new System.EventHandler(this.overwriteSelectedminusKeyToolStripMenuItem_Click);
			// 
			// recalcMapsToolStripMenuItem
			// 
			this.recalcMapsToolStripMenuItem.Name = "recalcMapsToolStripMenuItem";
			this.recalcMapsToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
			this.recalcMapsToolStripMenuItem.Text = "Recalc Maps";
			this.recalcMapsToolStripMenuItem.Click += new System.EventHandler(this.recalcMapsToolStripMenuItem_Click);
			// 
			// skipChecksumRecalcExNCAAOnOffToolStripMenuItem
			// 
			this.skipChecksumRecalcExNCAAOnOffToolStripMenuItem.Name = "skipChecksumRecalcExNCAAOnOffToolStripMenuItem";
			this.skipChecksumRecalcExNCAAOnOffToolStripMenuItem.Size = new System.Drawing.Size(114, 20);
			this.skipChecksumRecalcExNCAAOnOffToolStripMenuItem.Text = "MC02 Recalc: OFF";
			this.skipChecksumRecalcExNCAAOnOffToolStripMenuItem.Click += new System.EventHandler(this.skipChecksumRecalcExNCAAOnOffToolStripMenuItem_Click);
			// 
			// refreshViewToolStripMenuItem
			// 
			this.refreshViewToolStripMenuItem.Name = "refreshViewToolStripMenuItem";
			this.refreshViewToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
			this.refreshViewToolStripMenuItem.Text = "Re&fresh View";
			this.refreshViewToolStripMenuItem.Click += new System.EventHandler(this.refreshViewToolStripMenuItem_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(823, 380);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Generic EA DB Reader";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem createConfigToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadConfigToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem massToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem headerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem iOToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem allVisibleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem asNewItemsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem overwriteExistingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem overwriteSelectedminusKeyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recalcMapsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem skipChecksumRecalcExNCAAOnOffToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyConfigToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem refreshViewToolStripMenuItem;
	}
}

