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
	public partial class ChooseField : Form
	{
		public MaddenTable	table			= null;
		public Field		choosen			= null;


		public ChooseField()
		{
			InitializeComponent();
		}

		private void ChooseField_Load(object sender, EventArgs e)
		{
			if( table != null )
			{
				List<Field>	lFields	= new List<Field>( table.lFields );
				lFields.Sort( (a,b) => a.ToString( ).CompareTo( b.ToString( ) ) );

				foreach( Field f in lFields )
				{
					cbField.Items.Add( f );
				}
			}
			this.CenterToParent( );
		}
		private void ChooseField_FormClosing(object sender, FormClosingEventArgs e)
		{
			if( table != null && cbField.SelectedIndex > -1 )
			{
				choosen	= (Field) cbField.SelectedItem;
			}
		}
	}
}
