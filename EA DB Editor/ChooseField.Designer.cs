namespace EA_DB_Editor
{
	partial class ChooseField
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseField));
			this.cbField = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// cbField
			// 
			this.cbField.FormattingEnabled = true;
			this.cbField.Location = new System.Drawing.Point(12, 12);
			this.cbField.Name = "cbField";
			this.cbField.Size = new System.Drawing.Size(378, 21);
			this.cbField.TabIndex = 0;
			// 
			// ChooseField
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(402, 45);
			this.Controls.Add(this.cbField);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChooseField";
			this.Text = "Pick a field to use as the key for this export...";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChooseField_FormClosing);
			this.Load += new System.EventHandler(this.ChooseField_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox cbField;
	}
}