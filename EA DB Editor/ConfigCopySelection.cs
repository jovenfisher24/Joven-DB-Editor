using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using BetterListViewNS;
using MC02Handler;
using ListViewEx;

namespace EA_DB_Editor
{
	public partial class ConfigCopySelection : Form
	{
		private ToolTip			tooltip				= new ToolTip( );
		public List<XMLConfig>	lMappedTablesSrc	= null;
		public List<XMLConfig>	lMappedTablesDst	= null;
		public List<XMLConfig>	lMappedFieldsSrc	= null;
		public List<XMLConfig>	lMappedFieldsDst	= null;
		public List<XMLConfig>	lMappedTablesRes	= new List<XMLConfig>( );
		public List<XMLConfig>	lMappedFieldsRes	= new List<XMLConfig>( );
		public bool				bCanceled			= false;


		public ConfigCopySelection()
		{
			InitializeComponent();

			lvMappings.ShowItemToolTips	= true;
			lvMappings.SubItemClicked	+= new SubItemEventHandler( GridItemClick );
			lvMappings.ListClicked		+= new SubItemEventHandler( GridClicked );

			lvMappings.Columns.Add( "#" );
			lvMappings.Columns.Add( "Source" );
			lvMappings.Columns.Add( "Destination" );
			lvMappings.AutoResizeColumns( ColumnHeaderAutoResizeStyle.HeaderSize );
		}
		public void GridClicked( object obj, SubItemEventArgs e )
		{
			if( e.Button == MouseButtons.Right )
			{
				Point				point	= lvMappings.PointToClient( new Point( MousePosition.X, MousePosition.Y ) );
				ListViewHitTestInfo	info	= lvMappings.HitTest( point.X, point.Y );
				if( info.SubItem.Text != "" )
					tooltip.Show( ((XMLConfig) info.SubItem.Tag).ToString( ), lvMappings, point.X, point.Y, 5000 );
			}
		}
		public void GridItemClick( object obj, SubItemEventArgs args )
		{
			ListViewItem.ListViewSubItem	lviSubClicked	= args.Item.SubItems[ args.SubItem ];
			ListViewItem.ListViewSubItem	lviSubOther		= args.Item.SubItems[ args.SubItem == 2 ? 1 : 2 ];

			if( lviSubClicked.Text == "" )
				return;

			lviSubClicked.BackColor	= Color.Yellow;
			lviSubOther.BackColor	= Color.White;
		}

		private void ConfigCopySelection_Load(object sender, EventArgs e)
		{
			this.CenterToParent( );
			lMappedTablesRes.Clear( );
			lMappedFieldsRes.Clear( );
			lMappedFieldsDst	= new List<XMLConfig>( lMappedFieldsDst );
			lMappedFieldsSrc	= new List<XMLConfig>( lMappedFieldsSrc );
			lMappedTablesDst	= new List<XMLConfig>( lMappedTablesDst );
			lMappedTablesSrc	= new List<XMLConfig>( lMappedTablesSrc );

			lMappedFieldsDst.Sort( (a,b) => a.Abbreviation.CompareTo( b.Abbreviation ) );
			lMappedTablesDst.Sort( (a,b) => a.Abbreviation.CompareTo( b.Abbreviation ) );

			// cleanup duplciates
			for( int i=0; i < lMappedFieldsDst.Count; i++ )
			{	while( ( i +1 ) < lMappedFieldsDst.Count && lMappedFieldsDst[ i ].Abbreviation == lMappedFieldsDst[ i +1 ].Abbreviation )
					lMappedFieldsDst.RemoveAt( i +1 );
			}
			for( int i=0; i < lMappedTablesDst.Count; i++ )
			{	while( ( i +1 ) < lMappedTablesDst.Count && lMappedTablesDst[ i ].Abbreviation == lMappedTablesDst[ i +1 ].Abbreviation )
					lMappedTablesRes.RemoveAt( i +1 );
			}

			for( int i=0; i < lMappedTablesDst.Count; i++ )
			{
				XMLConfig						src		= lMappedTablesSrc.Find( (a) => a.Abbreviation == lMappedTablesDst[ i ].Abbreviation );
				ListViewItem					lvitems = new ListViewItem( ( i+1 ).ToString( ) );
				ListViewItem.ListViewSubItem	sub1	= new ListViewItem.ListViewSubItem( lvitems, ( src != null ? "T: " + ( src.Name == "" ? src.Abbreviation : src.Name ) : "" ) );
				ListViewItem.ListViewSubItem	sub2	= new ListViewItem.ListViewSubItem( lvitems, "T: " + ( lMappedTablesDst[ i ].Name == "" ? lMappedTablesDst[ i ].Abbreviation : lMappedTablesDst[ i ].Name ) );

				sub1.Tag			= src;
				sub2.Tag			= lMappedTablesDst[ i ];
				if( src != null && src.Name != "" && lMappedTablesDst[ i ].Name == "" )
					sub1.BackColor		= Color.Yellow;
				else
					sub2.BackColor		= Color.Yellow;
				lvitems.ToolTipText	= "";
				lvitems.SubItems.Add( sub1 );
				lvitems.SubItems.Add( sub2 );
				lvitems.UseItemStyleForSubItems	= false;

				lvMappings.Items.Add( lvitems );
			}

			for( int i=0; i < lMappedFieldsDst.Count; i++ )
			{
				XMLConfig						src		= lMappedFieldsSrc.Find( (a) => a.Abbreviation == lMappedFieldsDst[ i ].Abbreviation );
				ListViewItem					lvitems = new ListViewItem( ( i+1 ).ToString( ) );
				ListViewItem.ListViewSubItem	sub1	= new ListViewItem.ListViewSubItem( lvitems, ( src != null ? "F: " + ( src.Name == "" ? src.Abbreviation : src.Name ) : "" ) );
				ListViewItem.ListViewSubItem	sub2	= new ListViewItem.ListViewSubItem( lvitems, "F: " + ( lMappedFieldsDst[ i ].Name == "" ? lMappedFieldsDst[ i ].Abbreviation : lMappedFieldsDst[ i ].Name ) );

				sub1.Tag			= src;
				sub2.Tag			= lMappedFieldsDst[ i ];
				if( src != null && src.Name != "" && lMappedFieldsDst[ i ].Name == "" )
					sub1.BackColor		= Color.Yellow;
				else
					sub2.BackColor		= Color.Yellow;
				lvitems.ToolTipText	= "";
				lvitems.SubItems.Add( sub1 );
				lvitems.SubItems.Add( sub2 );
				lvitems.UseItemStyleForSubItems	= false;

				lvMappings.Items.Add( lvitems );
			}

			lvMappings.AutoResizeColumns( ColumnHeaderAutoResizeStyle.ColumnContent );
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			foreach( ListViewItem lvi in lvMappings.Items )
			{
				ListViewItem.ListViewSubItem	sub	= null;
				sub	= ( lvi.SubItems[ 1 ].BackColor == Color.Yellow ) ? sub = lvi.SubItems[ 1 ] : sub = lvi.SubItems[ 2 ];
				if( sub.Text.StartsWith( "T" ) )
					lMappedTablesRes.Add( (XMLConfig) sub.Tag );
				else
					lMappedFieldsRes.Add( (XMLConfig) sub.Tag );
			}

			bCanceled	= false;
			this.Close( );
		}
		private void bCancel_Click(object sender, EventArgs e)
		{
			bCanceled	= true;
			this.Close( );
		}
	}
}
