using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace BetterListViewNS
{
	public class BetterListView : ListView
	{
        // WM_VSCROLL message constants
        private const int WM_VSCROLL = 0x0115;

        protected override void WndProc(ref Message m)
        {
            // Trap the WM_VSCROLL message to generate the Scroll event
            if (m.Msg == WM_VSCROLL)
            {
				this.Refresh( );
			}
            base.WndProc(ref m);
        }
		public static void AddToListView( ListView lv, object obj, int itemnum, params string[] strings )
		{
			ListViewItem lvitems = new ListViewItem( strings[0] );

			for( int i=1; i < strings.Length; i++ )
			{
				ListViewItem.ListViewSubItem sub = new ListViewItem.ListViewSubItem( lvitems, strings[i] );

				lvitems.SubItems.Add( sub );
			}
			lv.Items.Add( lvitems );
			lv.Items[ itemnum ].UseItemStyleForSubItems	= true;
			lv.Items[ itemnum ].Tag	= obj;
		}
	}
}
