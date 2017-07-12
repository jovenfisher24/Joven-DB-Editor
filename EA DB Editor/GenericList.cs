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
	public partial class GenericList : Form
	{
		List<object>	l1, l2;

		public GenericList( string text, List<object> list1, List<object> list2 )
		{
			InitializeComponent();
			this.Text	= text;
			l1			= list1;
			l2			= list2;
		}

		private void GenericList_Load(object sender, EventArgs e)
		{
			this.CenterToParent( );
			int	min	= ( l1.Count < l2.Count ) ? l1.Count : l2.Count;
			for( int i=0; i < min; i++ )
			{
				BetterListViewNS.BetterListView.AddToListView( lveList, l2[i], i, l1[i].ToString( ), l2[i].ToString( ) );
			}
		}
	}
}
