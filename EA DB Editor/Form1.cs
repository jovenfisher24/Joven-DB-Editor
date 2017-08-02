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
	public partial class Form1 : Form
	{
		public List<Field>			lMappedFields	= new List<Field>( );
		public List<MaddenTable>	lMappedTables	= new List<MaddenTable>( );
		public List<View>			lMappedViews	= new List<View>( );
		public MaddenDatabase		maddenDB		= null;
		public bool					bConfigRead		= false;
		public View					currentView		= null;
		static public bool			mc02Recalc		= false;


		public Form1()
		{
			InitializeComponent();

			View.viewChanged	= viewChange;
			View.getMappedField	= getMappedField;
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			this.CenterToScreen( );
			filterToolStripMenuItem.Enabled				= false;
			massToolStripMenuItem.Enabled				= false;
			selectedToolStripMenuItem.Enabled			= false;
			allVisibleToolStripMenuItem.Enabled			= false;
			asNewItemsToolStripMenuItem.Enabled			= false;
			overwriteExistingToolStripMenuItem.Enabled	= false;
		}

		public void ReadXMLConfig( string configfile )
		{
			Field				field			= null;
			MaddenTable			table			= null;
			View				view			= null;
			string				Path			= "\\";


			lMappedFields.Clear( );
			lMappedTables.Clear( );
			lMappedViews.Clear( );

			XmlTextReader	reader	= new XmlTextReader( configfile );
			while( reader.Read( ) )
			{
				switch( reader.NodeType )
				{
					case XmlNodeType.Element:
						#region map open elements
						if( Path == "\\xml\\" && reader.Name == "Field" )
							field	= new Field( );

						if( Path == "\\xml\\" && reader.Name == "Table" )
							table	= new MaddenTable( );

						if( Path == "\\xml\\" && reader.Name == "View" )
							view	= new View( );

						if( reader.Name == "Formulas" )
						{	field.Formulas		= Field.Formula.ReadFormulas( reader, Path + "Formulas\\" );
							break;
						}
						#endregion

						Path	+= reader.Name + "\\";
						break;

					case XmlNodeType.Text:

						#region map main entries	
						if( Path.EndsWith( "Main\\Size\\Width\\" ) )
							this.Size	= new Size( Convert.ToInt32( reader.Value ), this.Size.Height );

						if( Path.EndsWith( "Main\\Size\\Height\\" ) )
							this.Size	= new Size( this.Size.Width, Convert.ToInt32( reader.Value ) );
						#endregion

						#region map field entries
						if( Path.EndsWith( "Field\\Abbreviation\\" ) )
							field.Abbreviation	= reader.Value;

						if( Path.EndsWith( "Field\\Name\\" ) )
							field.Name			= reader.Value;

						if( Path.EndsWith( "Field\\ControlType\\" ) )
						{	field.ControlType	= reader.Value;
							switch( reader.Value )
							{
								case "TextBox" :			field.EditControl	= new TextBox( );	break;
								case "ComboBox":			field.EditControl	= new ComboBox( );	break;
								case "CheckBox":			field.EditControl	= new CheckBox( );	break;
								case "Calculated":			field.EditControl	= new Label( );		break;
								case "AdjustedComboBox":	field.EditControl	= new ComboBox( );	break;
								case "MappedComboBox":		field.EditControl	= new ComboBox( );	break;
								case "TimeOfDayInMinutes":	field.EditControl	= new TextBox( );	break;
							}
							if( field.EditControl != null )
								field.EditControl.Visible	= false;
						}

						if( Path.EndsWith( "Field\\ControlItemCount\\" ) )
							field.ControlItems	= Convert.ToInt32( reader.Value );

						if( Path.EndsWith( "Field\\ControlItem\\" ) )
						{
							switch( field.ControlType )
							{
								case "ComboBox":
								case "AdjustedComboBox":
								case "MappedComboBox":
									((ComboBox)field.EditControl).Items.Add( reader.Value );
									break;
							}
						}

						if( Path.EndsWith( "Field\\ControlLocked\\" ) )
							field.ControlLocked	= Convert.ToBoolean( reader.Value );

						if( Path.EndsWith( "Field\\ControlLink\\Table\\" ) )
							field.ControlLink	= reader.Value;

						if( Path.EndsWith( "Field\\ControlLink\\IndexField\\" ) )
							field.ControlIF		= reader.Value;

						if( Path.EndsWith( "Field\\ControlLink\\ReferenceField\\" ) )
							field.ControlRF		= reader.Value;

						if( Path.EndsWith( "Field\\ControlLink\\ReferenceField2\\" ) )
							field.ControlRF2	= reader.Value;

						if( Path.EndsWith( "Field\\ControlLink\\Min\\" ) )
							field.Min			= Convert.ToDouble( reader.Value );

						if( Path.EndsWith( "Field\\ControlLink\\Max\\" ) )
							field.Max			= Convert.ToDouble( reader.Value );

						if( Path.EndsWith( "Field\\Offset\\" ) )
							field.Offset		= Convert.ToInt32( reader.Value );

						if( Path.EndsWith( "Field\\Description\\" ) )
							field.Description	= reader.Value;

						if( Path.EndsWith( "Field\\Type\\" ) )
						{
							switch( reader.Value )
							{
								case "uint":		field.type	= (ulong) Field.FieldType.tdbUInt;		break;
								case "sint":		field.type	= (ulong) Field.FieldType.tdbSInt;		break;
								case "string":		field.type	= (ulong) Field.FieldType.tdbString;	break;
								case "float":		field.type	= (ulong) Field.FieldType.tdbFloat;		break;
								case "binary":		field.type	= (ulong) Field.FieldType.tdbBinary;	break;
							}
						}
						#endregion

						#region map table entries	
						if( Path.EndsWith( "Table\\Abbreviation\\" ) )
							table.Abbreviation	= reader.Value;

						if( Path.EndsWith( "Table\\Name\\" ) )
							table.Name			= reader.Value;
						#endregion

						#region map view entries
						if( Path.EndsWith( "View\\Name\\" ) )
							view.Name			= reader.Value;

						if( Path.EndsWith( "View\\Type\\" ) )
							view.Type			= reader.Value;

						if( Path.EndsWith( "View\\Source\\Type\\" ) )
							view.SourceType		= reader.Value;

						if( Path.EndsWith( "View\\Source\\Name\\" ) )
							view.SourceName		= reader.Value;

						if( Path.EndsWith( "View\\Position\\X\\" ) )
							view.Position_x			= Convert.ToInt32( reader.Value );

						if( Path.EndsWith( "View\\Position\\Y\\" ) )
							view.Position_y			= Convert.ToInt32( reader.Value );

						if( Path.EndsWith( "View\\Position\\Z\\" ) )
							view.Position_z			= Convert.ToInt32( reader.Value );

						if( Path.EndsWith( "View\\Size\\Width\\" ) )
							view.Size_width			= Convert.ToInt32( reader.Value );

						if( Path.EndsWith( "View\\Size\\Height\\" ) )
							view.Size_height		= Convert.ToInt32( reader.Value );

						if( Path.EndsWith( "View\\ChildCount\\" ) )
							view.ChildCount			= Convert.ToInt32( reader.Value );

						if( Path.EndsWith( "View\\FieldCount\\" ) )
							view.ChildFieldCount	= Convert.ToInt32( reader.Value );

						if( Path.EndsWith( "View\\Child\\" ) )
							view.ChildViews.Add( reader.Value );

						if( Path.EndsWith( "View\\Field\\" ) )
							view.ChildFields.Add( reader.Value );
						#endregion
						break;

					case XmlNodeType.EndElement:
						#region map close elements
						if( Path == "\\xml\\Field\\" && reader.Name == "Field" )
							lMappedFields.Add( field );

						if( Path == "\\xml\\Table\\" && reader.Name == "Table" )
							lMappedTables.Add( table );

						if( Path == "\\xml\\View\\" && reader.Name == "View" )
							lMappedViews.Add( view );
						#endregion

						try
						{	Path	= Path.Remove( Path.LastIndexOf( reader.Name + "\\" ) );
						}	catch( Exception e )
						{
							MessageBox.Show( "XML closing element not found: " + reader.Name + ", " + reader.LineNumber, "Error in XML config" );
							throw( e );
						}
						break;
				}
			}
			reader.Close( );
			return;
		}
		public void PostProcessMaps( )
		{	int	i, j;

			#region field post processing
			#region cross lnk combo boxes where appropriate
			for( i=0; i < lMappedFields.Count; i++ )
			{
				if( lMappedFields[ i ].ControlLink != "" && lMappedFields[ i ].ControlType == "ComboBox" )
				{
					((ComboBox) lMappedFields[ i ].EditControl).Items.Clear( );
					lMappedFields[ i ].KeyToIndexMappings.Clear( );

					#region get the real table
					MaddenTable	mt	= MaddenTable.FindTable( lMappedTables, lMappedFields[ i ].ControlLink );
					if( mt == null )
					{
						MessageBox.Show( "Table " + lMappedFields[ i ].ControlLink + "not found in Field->Control for field " +  lMappedFields[ i ].Abbreviation, "Error in xml config" );
						continue;
					}
					mt				= maddenDB[ mt.Abbreviation ];
					#endregion

					for( j = 0; j < mt.lRecords.Count; j++ )
					{
						#region look up index & reference fields
						Field	fi	= Field.FindField( lMappedFields, lMappedFields[ i ].ControlIF );
						Field	fr	= Field.FindField( lMappedFields, lMappedFields[ i ].ControlRF );
						Field	fr2	= null;

						if( lMappedFields[ i ].ControlRF2 != "" )
							fr2	= Field.FindField( lMappedFields, lMappedFields[ i ].ControlRF2 );

						if( fi == null )
						{
							MessageBox.Show( "Field " + lMappedFields[ i ].Abbreviation + ": Control Link index field not found in table: " + lMappedFields[ i ].ControlIF, "Error in xml config" );
							continue;
						}
						if( fr == null )
						{
							MessageBox.Show( "Field " + lMappedFields[ i ].Abbreviation + ": Control Link reference field not found in table: " + lMappedFields[ i ].ControlRF, "Error in xml config" );
							continue;
						}
						#endregion
						#region add the mapping to the combobox for the field
						string	value	= "(" + mt.lRecords[ j ][ fi.Abbreviation ] + ") " + mt.lRecords[ j ][ fr.Abbreviation ];
						if( fr2 != null )
							value		+= " " + mt.lRecords[ j ][ fr2.Abbreviation ];

						RefObj	rf	= new RefObj( mt.lRecords[ j ][ fi.Abbreviation ], value );

						((ComboBox) lMappedFields[ i ].EditControl).Items.Add( rf );
						lMappedFields[ i ].KeyToIndexMappings.Add( rf.key, ((ComboBox) lMappedFields[ i ].EditControl).Items.Count -1 );
						#endregion
					}
				}
			}
			#endregion
			#region update column fields with real field data
			foreach( View view in lMappedViews )
			{	if( view.Type != "Grid" )	continue;

				MaddenTable	mt	= MaddenTable.FindTable( lMappedTables, view.SourceName );
				mt				= maddenDB[ mt.Abbreviation ];

				foreach( ColumnHeader ch in ((ListView) view.DisplayControl).Columns )
				{	if( ch.Tag == null )	continue;

					Field	colfield	= (Field) ch.Tag;
					Field	f			= Field.GetField( mt.lFields, colfield.Abbreviation );

					if( f != null )
					{	colfield.bits	= f.bits;
						colfield.name	= f.name;
						colfield.offset	= f.offset;
						colfield.type	= f.type;
					}
				}
			}
			#endregion
			#endregion
		}
		private void UpdateTableBoundViews( )
		{
			foreach( View v in lMappedViews )
			{
				if( v.SourceType == "Table" )
				{	MaddenTable	mt	= MaddenTable.FindTable( lMappedTables, v.SourceName );
					if( v.Type == "Grid" )
						v.UpdateGridData( maddenDB[ mt.Abbreviation ] );
				}
			}
		}
		public void viewChange( View view )
		{
			if( view.lChildren.Count == 0 )
				currentView	= view;
			else
			{	TabControl	tab		= (TabControl) view.DisplayControl;
				int			sel		= tab.SelectedIndex;
				tab.SelectedIndex	= -1;
				tab.SelectedIndex	= sel > -1 ? sel : 0;
			}
		}
		public Field getMappedField( string name )
		{	return Field.FindField( lMappedFields, name );
		}

		public bool ChooseConfig( )
		{
			OpenFileDialog	openFileDialog	= new OpenFileDialog( );

			openFileDialog.AddExtension	= true;
			openFileDialog.DefaultExt	= ".xml";
			openFileDialog.Filter		= "*.xml|*.xml|all|*.*";
			openFileDialog.Multiselect	= false;
			openFileDialog.Title		= "Select an XML config file to use...";

			if( System.Windows.Forms.DialogResult.OK == openFileDialog.ShowDialog( ) )
			{
				// clean up old controls first
				List<Control>	remove	= new List<Control>( );
				foreach( Control c in this.Controls )
				{
					if( c.GetType( ).ToString( ).EndsWith( "TextBox" ) ||
						c.GetType( ).ToString( ).EndsWith( "ComboBox" ) ||
						c.GetType( ).ToString( ).EndsWith( "ListView" ) ||
						c.GetType( ).ToString( ).EndsWith( "TabControl" ) )
						remove.Add( c );
				}
				foreach( Control c in remove )
				{	c.Controls.Clear( );
					this.Controls.Remove( c );
				}


				ReadXMLConfig( openFileDialog.FileName );

				if( ! View.ProcessAllViewSettings( lMappedViews, lMappedFields ) )
					this.Close( );
				if( ! View.SetViewChildren( lMappedViews, this ) )
					this.Close( );

				bConfigRead									= true;
				return true;
			}
			return false;
		}
		public void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if( ! bConfigRead )
			{	MessageBox.Show( "Please choose a config file before opening an EA file. You can use the option to generate one from a file if you need to.", "Alert!" );
				if( ! ChooseConfig( ) )
					return;
			}

			OpenFileDialog	openFileDialog	= new OpenFileDialog( );

			openFileDialog.AddExtension	= true;
			openFileDialog.DefaultExt	= ".MC02";
			openFileDialog.Filter		= "*.MC02|*.MC02|*.DB|*.DB|all|*.*";
			openFileDialog.Multiselect	= false;
			openFileDialog.Title		= "Select MC02 file to open...";

			if( System.Windows.Forms.DialogResult.OK == openFileDialog.ShowDialog( ) )
			{
				Cursor.Current	= Cursors.WaitCursor;
				maddenDB	= new MaddenDatabase( openFileDialog.FileName );

				// walked each table and field and add in the mapped elements
				foreach( MaddenTable mt in maddenDB.lTables )
				{
					MaddenTable	mtmapped	= MaddenTable.FindTable( lMappedTables, mt.Table.TableName );
					mt.Abbreviation			= mt.Table.TableName;
					if( mtmapped != null )
						mt.Name				= mtmapped.Name;

					foreach( Field f in mt.lFields )
					{
						Field	fmapped	= Field.FindField( lMappedFields, f.name );
						f.Abbreviation	= f.name;
						if( fmapped != null )
							f.Name		= fmapped.Name;
					}
				}

				this.Text		= maddenDB.realfileName.Substring( maddenDB.realfileName.LastIndexOf( '\\' ) +1 );
				Cursor.Current	= Cursors.Default;

				filterToolStripMenuItem.Enabled				= true;
				massToolStripMenuItem.Enabled				= true;
				selectedToolStripMenuItem.Enabled			= true;
				allVisibleToolStripMenuItem.Enabled			= true;
				asNewItemsToolStripMenuItem.Enabled			= true;
				overwriteExistingToolStripMenuItem.Enabled	= true;

				PostProcessMaps( );
				UpdateTableBoundViews( );
			}
		}
		public void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor.Current	= Cursors.WaitCursor;
			maddenDB.Save( );
			Cursor.Current	= Cursors.Default;
			MessageBox.Show( "Done" );
		}
		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			#region open file
			SaveFileDialog	saveFileDialog	= new SaveFileDialog( );

			saveFileDialog.Filter		= "*.MC02|*.MC02|all|*.*";
			saveFileDialog.DefaultExt	= ".MC02";
			saveFileDialog.Title		= "Save as...";

			if( System.Windows.Forms.DialogResult.OK != saveFileDialog.ShowDialog( ) )
				return;
			#endregion

			Cursor.Current	= Cursors.WaitCursor;
			maddenDB.SaveAs( saveFileDialog.FileName );
			Cursor.Current	= Cursors.Default;

			this.Text	= maddenDB.realfileName.Substring( maddenDB.realfileName.LastIndexOf( '\\' ) +1 );
			MessageBox.Show( "Done" );
		}
		public void createConfigToolStripMenuItem_Click(object sender, EventArgs e)
		{	OpenFileDialog	openFileDialog	= new OpenFileDialog( );

			openFileDialog.AddExtension	= true;
			openFileDialog.DefaultExt	= ".MC02";
			openFileDialog.Filter		= "*.MC02|*.MC02|*.DB|*.DB|all|*.*";
			openFileDialog.Multiselect	= false;
			openFileDialog.Title		= "Select MC02 file to open...";

			if( System.Windows.Forms.DialogResult.OK == openFileDialog.ShowDialog( ) )
			{
				Cursor.Current	= Cursors.WaitCursor;
				MaddenDatabase	maddenDB2	= new MaddenDatabase( openFileDialog.FileName );
				Cursor.Current	= Cursors.Default;

				SaveFileDialog	saveFileDialog	= new SaveFileDialog( );

				saveFileDialog.AddExtension	= true;
				saveFileDialog.DefaultExt	= ".xml";
				saveFileDialog.Filter		= "*.xml|*.xml|all|*.*";
				saveFileDialog.Title		= "Save xml config as...";

				if( System.Windows.Forms.DialogResult.OK == saveFileDialog.ShowDialog( ) )
				{
					Cursor.Current	= Cursors.WaitCursor;

					FileStream		fs	= File.Open( saveFileDialog.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite );
					StreamWriter	sw	= new StreamWriter( fs );

					sw.WriteLine( "<xml>" );
					sw.WriteLine( "" );
					sw.WriteLine( "" );

					#region create the main tab view
					sw.WriteLine( "" );
					sw.WriteLine( "<View>" );
					sw.WriteLine( "\t<Name>MainTab</Name>" );
					sw.WriteLine( "\t<Type>Tab</Type>" );
					sw.WriteLine( "\t<Position>" );
					sw.WriteLine( "\t\t<X>10</X>" );
					sw.WriteLine( "\t\t<Y>30</Y>" );
					sw.WriteLine( "\t\t<Z>0</Z>" );
					sw.WriteLine( "\t</Position>" );
					sw.WriteLine( "\t<Size>" );
					sw.WriteLine( "\t\t<Width>800</Width>" );
					sw.WriteLine( "\t\t<Height>340</Height>" );
					sw.WriteLine( "\t</Size>" );
					#region add tables
					foreach( MaddenTable mt in maddenDB2.lTables )
						sw.WriteLine( "\t<Child>" + mt.Table.TableName + "</Child>" );
					#endregion
					sw.WriteLine( "</View>" );
					sw.WriteLine( "" );
					#endregion

					foreach( MaddenTable mt in maddenDB2.lTables )
					{
						#region create a view
						sw.WriteLine( "" );
						sw.WriteLine( "<View>" );
						sw.WriteLine( "\t<Name>" + mt.Table.TableName + "</Name>" );
						sw.WriteLine( "\t<Type>Grid</Type>" );
						sw.WriteLine( "\t<Position>" );
						sw.WriteLine( "\t\t<X>0</X>" );
						sw.WriteLine( "\t\t<Y>0</Y>" );
						sw.WriteLine( "\t\t<Z>0</Z>" );
						sw.WriteLine( "\t</Position>" );
						sw.WriteLine( "\t<Size>" );
						sw.WriteLine( "\t\t<Width>200</Width>" );
						sw.WriteLine( "\t\t<Height>100</Height>" );
						sw.WriteLine( "\t</Size>" );
						sw.WriteLine( "\t<Source>" );
						sw.WriteLine( "\t\t<Type>Table</Type>" );
						sw.WriteLine( "\t\t<Name>" + mt.Table.TableName + "</Name>" );
						sw.WriteLine( "\t</Source>" );
						#region add fields
						foreach( Field f in mt.lFields )
							sw.WriteLine( "\t<Field>" + f.name + "</Field>" );
						#endregion
						sw.WriteLine( "</View>" );
						sw.WriteLine( "" );
						#endregion
						#region create the table
						sw.WriteLine( "" );
						sw.WriteLine( "<Table>" );
						sw.WriteLine( "\t<Abbreviation>" + mt.Table.TableName + "</Abbreviation>" );
						sw.WriteLine( "\t<Name></Name>" );
						sw.WriteLine( "</Table>" );
						sw.WriteLine( "" );
						#endregion
						#region create the fields
						foreach( Field f in mt.lFields )
						{	string	type	= "";
							switch( f.type )
							{
								case (ulong)Field.FieldType.tdbString:	type	= "string";	break;
								case (ulong)Field.FieldType.tdbSInt:	type	= "sint";	break;
								case (ulong)Field.FieldType.tdbUInt:	type	= "uint";	break;
								case (ulong)Field.FieldType.tdbBinary:	type	= "binary";	break;
								case (ulong)Field.FieldType.tdbFloat:	type	= "float";	break;
							}
							sw.WriteLine( "<Field>" );
							sw.WriteLine( "\t<Abbreviation>" + f.name + "</Abbreviation>" );
							sw.WriteLine( "\t<Name></Name>" );
							sw.WriteLine( "\t<ControlType>TextBox</ControlType>" );
							sw.WriteLine( "\t<Type>" + type + "</Type>" );
							sw.WriteLine( "</Field>" );
						}
						#endregion
					}

					sw.WriteLine( "" );
					sw.WriteLine( "" );
					sw.WriteLine( "</xml>" );
					sw.Flush( );
					sw.Close( );
					fs.Close( );

					Cursor.Current	= Cursors.Default;

				}

			}
		}
		private void loadConfigToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ChooseConfig( );
		}
		private void filterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterForm	ff	= new FilterForm( lMappedFields, lMappedTables, new List<View>() { currentView }, FilterForm.CBToUse.filter, "Filters" );
			ff.ShowDialog( );

			MaddenTable	mt	= MaddenTable.FindTable( lMappedTables, ff.view.SourceName );
			ff.view.UpdateGridData( maddenDB[ mt.Abbreviation ], ff.lFilters );
		}
		private void massToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FilterForm	ff	= new FilterForm( lMappedFields, lMappedTables, new List<View>() { currentView }, FilterForm.CBToUse.mass, "Mass Operations" );
			ff.ShowDialog( );

			foreach( ListViewItem lvi in ((ListView) ff.view.DisplayControl).Items )
			{
				foreach( FieldFilter mass in ff.lFilters )
					mass.Process( lMappedFields, ((MaddenRecord) lvi.Tag) );
			}

			MaddenTable	mt	= MaddenTable.FindTable( lMappedTables, ff.view.SourceName );
			ff.view.RefreshGridData( maddenDB[ mt.Abbreviation ] );
		}
		private void headerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<object>	lo1	= new List<object>( );
			List<object>	lo2	= new List<object>( );

			lo1.Add( (object) "Header" );
			lo1.Add( (object) "Version" );
			lo1.Add( (object) "Uknown 1" );
			lo1.Add( (object) "DB Size" );
			lo1.Add( (object) "Zero" );
			lo1.Add( (object) "Table Count" );
			lo1.Add( (object) "Uknown 2" );

			lo2.Add( (object) maddenDB.dbFileInfo.header.ToString( ) );
			lo2.Add( (object) maddenDB.dbFileInfo.version.ToString( ) );
			lo2.Add( (object) maddenDB.dbFileInfo.unknown_1.ToString( ) );
			lo2.Add( (object) maddenDB.dbFileInfo.DBsize.ToString( ) );
			lo2.Add( (object) maddenDB.dbFileInfo.zero.ToString( ) );
			lo2.Add( (object) maddenDB.dbFileInfo.tableCount.ToString( ) );
			lo2.Add( (object) maddenDB.dbFileInfo.unknown_2.ToString( ) );

			GenericList	gl	= new GenericList( "", lo1, lo2 );
			gl.Show( );
		}
		private void selectedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if( currentView == null )
				return;

			if( currentView.Type.ToLower( ) == "grid" || currentView.Type.ToLower( ) == "list item" )
			{	// make sure there is a selection
				if( ((ListView)currentView.DisplayControl).SelectedItems.Count <= 0 )
					return;

				// now let's get the table
				MaddenTable		mt	= MaddenTable.FindTable( maddenDB.lTables, currentView.SourceName );

				// request the field to use as a key
				ChooseField		cf	= new ChooseField( );
				cf.table			= mt;
				cf.ShowDialog( );

				if( cf.choosen == null )
				{
					MessageBox.Show( "No field choosen - canceling export" );
					return;
				}

				// now get the record selected
				MaddenRecord	mr	= (MaddenRecord) ((ListView)currentView.DisplayControl).SelectedItems[0].Tag;

				#region now open the file dialog
				SaveFileDialog	saveFileDialog1	= new SaveFileDialog( );

				saveFileDialog1.Filter			= "*.csv|*.csv|all|*.*";
				saveFileDialog1.DefaultExt		= ".csv";
				saveFileDialog1.AddExtension	= true;
				saveFileDialog1.FileName		= "";

				if( System.Windows.Forms.DialogResult.OK != saveFileDialog1.ShowDialog( ) )
					return;
				#endregion

				FileStream			fs		= new FileStream( saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.Write );
				StreamWriter		sw		= new StreamWriter( fs );


				Cursor.Current	= Cursors.WaitCursor;
				#region write the table name, fields, and record
				sw.WriteLine( mt.Abbreviation + "," + cf.choosen.name );

				foreach( Field f in mt.lFields )
					sw.Write( f.name + "," );
				sw.WriteLine( "" );

				foreach( Field f in mt.lFields )
					sw.Write( mr[ f.name ] + "," );
				sw.WriteLine( "" );

				sw.Flush( );
				sw.Close( );
				fs.Close( );
				#endregion
				Cursor.Current	= Cursors.Default;
			}
		}
		private void allVisibleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if( currentView == null )
				return;

			// now let's export the selection. get the table first
			MaddenTable		mt	= MaddenTable.FindTable( maddenDB.lTables, currentView.SourceName );

			// request the field to use as a key
			ChooseField		cf	= new ChooseField( );
			cf.table			= mt;
			cf.ShowDialog( );

			if( cf.choosen == null )
			{
				MessageBox.Show( "No field choosen - canceling export" );
				return;
			}

			#region now open the file dialog
			SaveFileDialog	saveFileDialog1	= new SaveFileDialog( );

			saveFileDialog1.Filter			= "*.csv|*.csv|all|*.*";
			saveFileDialog1.DefaultExt		= ".csv";
			saveFileDialog1.AddExtension	= true;
			saveFileDialog1.FileName		= "";

			if( System.Windows.Forms.DialogResult.OK != saveFileDialog1.ShowDialog( ) )
				return;
			#endregion

			FileStream			fs		= new FileStream( saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.Write );
			StreamWriter		sw		= new StreamWriter( fs );

			Cursor.Current	= Cursors.WaitCursor;
			#region write the table name, fields, and records

			sw.WriteLine( mt.Abbreviation + "," + cf.choosen.name );

			foreach( Field f in mt.lFields )
				sw.Write( f.name + "," );
			sw.WriteLine( "" );

			foreach( ListViewItem lvi in ((ListView)currentView.DisplayControl).Items )
			{
				MaddenRecord	mr	= (MaddenRecord) lvi.Tag;

				foreach( Field f in mt.lFields )
					sw.Write( mr[ f.name ] + "," );
				sw.WriteLine( "" );
			}

			sw.Flush( );
			sw.Close( );
			fs.Close( );
			#endregion
			Cursor.Current	= Cursors.Default;
		}
		private void asNewItemsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			#region open file
			OpenFileDialog	openFileDialog1	= new OpenFileDialog( );

			openFileDialog1.Filter		= "*.csv|*.csv|all|*.*";
			openFileDialog1.DefaultExt	= ".csv";
			openFileDialog1.Multiselect	= false;

			if( System.Windows.Forms.DialogResult.OK != openFileDialog1.ShowDialog( ) )
				return;
			#endregion

			Cursor.Current				= Cursors.WaitCursor;

			// should only ever have 1 file
			for( int iFile = 0; iFile < openFileDialog1.FileNames.Length; iFile++ )
			{
				FileStream			fs		= new FileStream( openFileDialog1.FileNames[iFile], FileMode.Open, FileAccess.Read );
				StreamReader		sr		= new StreamReader( fs );

				#region read table & headers then check their validity
				string[]	header	= sr.ReadLine( ).Split( new char[]{ ',' } );
				if( header.Length < 2 )
				{	MessageBox.Show( "Corrupted header - should contain table name and key field" );
					sr.Close( );
					fs.Close( );
					return;
				}
				string		table	= header[0];
				string		key		= header[1];
				string[]	sfields	= sr.ReadLine( ).Split( new char[]{ ',' }, StringSplitOptions.RemoveEmptyEntries );

				// get our table
				MaddenTable	mt		= MaddenTable.FindMaddenTable( maddenDB.lTables, table );
				if( mt == null )
				{	MessageBox.Show( "The table this data is for cannot be found in this database" );
					sr.Close( );
					fs.Close( );
					return;
				}

				// make sure our table matches our view
				if( currentView.SourceName != mt.Name && currentView.SourceName != mt.Abbreviation )
				{	MessageBox.Show( "The selected file contains data for a different table. This table: " + currentView.SourceName + " The file's: " + mt.Abbreviation );
					sr.Close( );
					fs.Close( );
					return;
				}

				// make sure key exists
				Field	keyf	= Field.FindField( mt.lFields, key );
				if( keyf == null )
				{	MessageBox.Show( "The key provided (" + key + ") does not exists in the " + mt.ToString( ) + " table" );
					sr.Close( );
					fs.Close( );
					return;
				}

				// check the import file only uses headers in our table
				for( int i=0; i < sfields.Length; i++ )
				{	
					if( mt.lFields.Find( (a) => a.name == sfields[ i ] ) == null )
					{	MessageBox.Show( "Table " + mt + " does not have a field named " + sfields[ i ] + " as listed in " + openFileDialog1.FileNames[iFile] );
						sr.Close( );
						fs.Close( );
						return;
					}
				}
				#endregion

				while( ! sr.EndOfStream )
				{
					#region read our data
					string[]	data	= sr.ReadLine( ).Split( new char[] { ',' } );

					if( data.Length < sfields.Length )
					{	MessageBox.Show( openFileDialog1.FileNames[iFile] + " does not have as many data entries as field entries listed" );
						sr.Close( );
						fs.Close( );
						return;
					}
					#endregion
					#region create a new record to hold the data
					MaddenRecord	mr	= new MaddenRecord( mt, mt.lFields );
					for( int i=0; i < sfields.Length; i++ )
					{	mr[ sfields[ i ] ]	= data[ i ];
					}
					#endregion
					#region now see if this record already exists
					MaddenRecord	exists	= mt.lRecords.Find( (a) => a[ key ] == mr[ key ] );
					if( exists != null )
					{
						if( keyf.type != (ulong) Field.FieldType.tdbUInt && keyf.type != (ulong) Field.FieldType.tdbSInt )
						{	MessageBox.Show( "A record exists with the same data in field " + key + " which is not an integer type, therefore aborting the import" );
							sr.Close( );
							fs.Close( );
							return;
						}

						// now find the first number that isn't taken
						List<int>	ints	= new List<int>( );
						foreach( MaddenRecord r in mt.lRecords )
							ints.Add( Convert.ToInt32( r[ key ] ) );
						ints.Sort( );

						int	index	= 0;
						for( index =0; index < mt.Table.maxrecords; index++ )
						{	if( index != ints[ index ] )
								break;
						}

						if( index >= mt.Table.maxrecords )
						{	MessageBox.Show( "The table " + mt.ToString( ) + " is full @ " + mt.Table.maxrecords.ToString( ) + " and therefore we cannot import a new entry; aborting" );
							sr.Close( );
							fs.Close( );
							return;
						}

						// set the value to an unused value for the key field
						mr[ key ]	= index.ToString( );
					}
					#endregion
					#region fell through, so we're adding the record
					if( ! mt.InsertRecord( mr ) )
					{	MessageBox.Show( "Table is full; cannot create a new record", "Error" );
						Cursor.Current	= Cursors.Default;
						return;
					}
					#endregion
				}

				PostProcessMaps( );
				currentView.RefreshGridData( mt );

				sr.Close( );
				fs.Close( );

			}

			Cursor.Current	= Cursors.Default;
		}
		private void overwriteExistingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			#region open file
			OpenFileDialog	openFileDialog1	= new OpenFileDialog( );

			openFileDialog1.Filter		= "*.csv|*.csv|all|*.*";
			openFileDialog1.DefaultExt	= ".csv";
			openFileDialog1.Multiselect	= false;

			if( System.Windows.Forms.DialogResult.OK != openFileDialog1.ShowDialog( ) )
				return;
			#endregion

			Cursor.Current				= Cursors.WaitCursor;

			// should only ever have 1 file
			for( int iFile = 0; iFile < openFileDialog1.FileNames.Length; iFile++ )
			{
				FileStream			fs		= new FileStream( openFileDialog1.FileNames[iFile], FileMode.Open, FileAccess.Read );
				StreamReader		sr		= new StreamReader( fs );

				#region read table & headers then check their validity
				string[]	header	= sr.ReadLine( ).Split( new char[]{ ',' } );
				if( header.Length < 2 )
				{	MessageBox.Show( "Corrupted header - should contain table name and key field" );
					sr.Close( );
					fs.Close( );
					return;
				}
				string		table	= header[0];
				string		key		= header[1];
				string[]	sfields	= sr.ReadLine( ).Split( new char[]{ ',' }, StringSplitOptions.RemoveEmptyEntries );

				// get our table
				MaddenTable	mt		= MaddenTable.FindMaddenTable( maddenDB.lTables, table );
				if( mt == null )
				{	MessageBox.Show( "The table this data is for cannot be found in this database" );
					sr.Close( );
					fs.Close( );
					return;
				}

				// make sure our table matches our view
				if( currentView.SourceName != mt.Name && currentView.SourceName != mt.Abbreviation )
				{	MessageBox.Show( "The selected file contains data for a different table. This table: " + currentView.SourceName + " The file's: " + mt.Abbreviation );
					sr.Close( );
					fs.Close( );
					return;
				}

				// make sure key exists
				Field	keyf	= Field.FindField( mt.lFields, key );
				if( keyf == null )
				{	MessageBox.Show( "The key provided (" + key + ") does not exists in the " + mt.ToString( ) + " table" );
					sr.Close( );
					fs.Close( );
					return;
				}

				// check the import file only uses headers in our table
				for( int i=0; i < sfields.Length; i++ )
				{	
					if( mt.lFields.Find( (a) => a.name == sfields[ i ] ) == null )
					{	MessageBox.Show( "Table " + mt + " does not have a field named " + sfields[ i ] + " as listed in " + openFileDialog1.FileNames[iFile] );
						sr.Close( );
						fs.Close( );
						return;
					}
				}
				#endregion

				int	count	= 1;
				while( ! sr.EndOfStream )
				{
					#region read our data
					string[]	data	= sr.ReadLine( ).Split( new char[] { ',' } );

					if( data.Length < sfields.Length )
					{	MessageBox.Show( openFileDialog1.FileNames[iFile] + " does not have as many data entries as field entries listed" );
						sr.Close( );
						fs.Close( );
						return;
					}
					#endregion
					#region create a new record to hold the data
					MaddenRecord	mr	= new MaddenRecord( mt, mt.lFields );
					for( int i=0; i < sfields.Length; i++ )
					{	mr[ sfields[ i ] ]	= data[ i ];
					}
					#endregion
					#region now copy over the existing record ( only the fields provided )
					MaddenRecord	exists	= mt.lRecords.Find( (a) => a[ key ] == mr[ key ] );
					if( exists == null )
					{
						MessageBox.Show( "Record # " + count + " was not found; skipping" );
						continue;
					}

					for( int i=0; i < sfields.Length; i++ )
					{	exists[ sfields[ i ] ]	= mr[ sfields[ i ] ];
					}

					count++;
					#endregion
				}

				PostProcessMaps( );
				currentView.RefreshGridData( mt );

				sr.Close( );
				fs.Close( );

			}

			Cursor.Current	= Cursors.Default;
		}
		private void overwriteSelectedminusKeyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if( currentView == null )
				return;

			if( currentView.Type.ToLower( ) != "grid" && currentView.Type.ToLower( ) != "list item" )
				return;

			if( ((ListView)currentView.DisplayControl).SelectedItems.Count <= 0 )
				return;

			#region open file
			OpenFileDialog	openFileDialog1	= new OpenFileDialog( );

			openFileDialog1.Filter		= "*.csv|*.csv|all|*.*";
			openFileDialog1.DefaultExt	= ".csv";
			openFileDialog1.Multiselect	= false;

			if( System.Windows.Forms.DialogResult.OK != openFileDialog1.ShowDialog( ) )
				return;
			#endregion

			Cursor.Current				= Cursors.WaitCursor;

			// should only ever have 1 file
			for( int iFile = 0; iFile < openFileDialog1.FileNames.Length; iFile++ )
			{
				FileStream			fs		= new FileStream( openFileDialog1.FileNames[iFile], FileMode.Open, FileAccess.Read );
				StreamReader		sr		= new StreamReader( fs );

				#region read table & headers then check their validity
				string[]	header	= sr.ReadLine( ).Split( new char[]{ ',' } );
				if( header.Length < 2 )
				{	MessageBox.Show( "Corrupted header - should contain table name and key field" );
					sr.Close( );
					fs.Close( );
					return;
				}
				string		table	= header[0];
				string		key		= header[1];
				string[]	sfields	= sr.ReadLine( ).Split( new char[]{ ',' }, StringSplitOptions.RemoveEmptyEntries );

				// get our table
				MaddenTable	mt		= MaddenTable.FindMaddenTable( maddenDB.lTables, table );
				if( mt == null )
				{	MessageBox.Show( "The table this data is for cannot be found in this database" );
					sr.Close( );
					fs.Close( );
					return;
				}

				// make sure our table matches our view
				if( currentView.SourceName != mt.Name && currentView.SourceName != mt.Abbreviation )
				{	MessageBox.Show( "The selected file contains data for a different table. This table: " + currentView.SourceName + " The file's: " + mt.Abbreviation );
					sr.Close( );
					fs.Close( );
					return;
				}

				// make sure key exists
				Field	keyf	= Field.FindField( mt.lFields, key );
				if( keyf == null )
				{	MessageBox.Show( "The key provided (" + key + ") does not exists in the " + mt.ToString( ) + " table" );
					sr.Close( );
					fs.Close( );
					return;
				}

				// check the import file only uses headers in our table
				for( int i=0; i < sfields.Length; i++ )
				{	
					if( mt.lFields.Find( (a) => a.name == sfields[ i ] ) == null )
					{	MessageBox.Show( "Table " + mt + " does not have a field named " + sfields[ i ] + " as listed in " + openFileDialog1.FileNames[iFile] );
						sr.Close( );
						fs.Close( );
						return;
					}
				}
				#endregion
				#region read our data
				string[]	data	= sr.ReadLine( ).Split( new char[] { ',' } );

				if( data.Length < sfields.Length )
				{	MessageBox.Show( openFileDialog1.FileNames[iFile] + " does not have as many data entries as field entries listed" );
					sr.Close( );
					fs.Close( );
					return;
				}
				#endregion
				#region create a new record to hold the data
				MaddenRecord	mr	= new MaddenRecord( mt, mt.lFields );
				for( int i=0; i < sfields.Length; i++ )
				{	mr[ sfields[ i ] ]	= data[ i ];
				}
				#endregion
				#region now copy over the existing record ( only the fields provided, minus the key )
				MaddenRecord	selected	= (MaddenRecord) ((ListView)currentView.DisplayControl).SelectedItems[0].Tag;

				for( int i=0; i < sfields.Length; i++ )
				{	if( sfields[ i ] != key )
						selected[ sfields[ i ] ]	= mr[ sfields[ i ] ];
				}
				#endregion

				PostProcessMaps( );
				currentView.RefreshGridData( mt );

				sr.Close( );
				fs.Close( );
			}

			Cursor.Current	= Cursors.Default;
		}
		private void recalcMapsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor.Current	= Cursors.WaitCursor;
			PostProcessMaps( );
			MaddenTable	mt	= MaddenTable.FindTable( lMappedTables, currentView.SourceName );
			currentView.RefreshGridData( maddenDB[ mt.Abbreviation ] );
			Cursor.Current	= Cursors.Default;
		}
		private void skipChecksumRecalcExNCAAOnOffToolStripMenuItem_Click(object sender, EventArgs e)
		{
			mc02Recalc	= ! mc02Recalc;
			if( mc02Recalc )
				skipChecksumRecalcExNCAAOnOffToolStripMenuItem.Text	= "MC02 Recalc: ON";
			else
				skipChecksumRecalcExNCAAOnOffToolStripMenuItem.Text	= "MC02 Recalc: OFF";
		}
		private void copyConfigToolStripMenuItem_Click(object sender, EventArgs e)
		{
			#region open src file
			OpenFileDialog	openFileDialog1	= new OpenFileDialog( );

			openFileDialog1.Filter		= "*.xml|*.xml|all|*.*";
			openFileDialog1.DefaultExt	= ".xml";
			openFileDialog1.Title		= "Select source config...";
			openFileDialog1.Multiselect	= false;

			if( System.Windows.Forms.DialogResult.OK != openFileDialog1.ShowDialog( ) )
				return;
			#endregion
			#region open dst file
			OpenFileDialog	openFileDialog2	= new OpenFileDialog( );

			openFileDialog2.Filter		= "*.xml|*.xml|all|*.*";
			openFileDialog2.DefaultExt	= ".xml";
			openFileDialog2.Title		= "Select destination config...";
			openFileDialog2.Multiselect	= false;

			if( System.Windows.Forms.DialogResult.OK != openFileDialog2.ShowDialog( ) )
				return;
			#endregion

			List<XMLConfig>	srcViews	= new List<XMLConfig>( );
			List<XMLConfig>	srcTables	= new List<XMLConfig>( );
			List<XMLConfig>	srcFields	= new List<XMLConfig>( );
			List<XMLConfig>	dstViews	= new List<XMLConfig>( );
			List<XMLConfig>	dstTables	= new List<XMLConfig>( );
			List<XMLConfig>	dstFields	= new List<XMLConfig>( );

			Cursor.Current				= Cursors.WaitCursor;

			XMLConfig.ReadXMLConfig( openFileDialog1.FileName, srcViews, srcTables, srcFields );
			XMLConfig.ReadXMLConfig( openFileDialog2.FileName, dstViews, dstTables, dstFields );

			ConfigCopySelection	ccs	= new ConfigCopySelection( );
			ccs.lMappedFieldsDst	= dstFields;
			ccs.lMappedFieldsSrc	= srcFields;
			ccs.lMappedTablesDst	= dstTables;
			ccs.lMappedTablesSrc	= srcTables;

			ccs.ShowDialog( );
			if( ccs.bCanceled )
				return;

			//XMLConfig.CopyMappedValues( srcTables, dstTables );
			//XMLConfig.CopyMappedValues( srcFields, dstFields );

			//dstViews.Sort( (a,b) => a.Name.CompareTo( b.Name ) );
			ccs.lMappedTablesRes.Sort( (a,b) => a.Abbreviation.CompareTo( b.Abbreviation ) );
			ccs.lMappedFieldsRes.Sort( (a,b) => a.Abbreviation.CompareTo( b.Abbreviation ) );
			XMLConfig.UseFriendlyNames( dstViews, ccs.lMappedTablesRes, ccs.lMappedFieldsRes );

			FileStream		fs			= new FileStream( openFileDialog2.FileName, FileMode.Truncate, FileAccess.Write );
			StreamWriter	sw			= new StreamWriter( fs );

			sw.Write( XMLConfig.WriteXMLConfig( dstViews, ccs.lMappedTablesRes, ccs.lMappedFieldsRes ) );
			sw.Flush( );
			sw.Close( );
			fs.Close( );

			Cursor.Current				= Cursors.Default;
		}
		private void refreshViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor.Current	= Cursors.WaitCursor;
			MaddenTable	mt	= MaddenTable.FindTable( lMappedTables, currentView.SourceName );
			currentView.RefreshGridData( maddenDB[ mt.Abbreviation ] );
			Cursor.Current	= Cursors.Default;
		}

        private void filterAdjustToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FilterAdjustForm fa = new FilterAdjustForm(lMappedFields, lMappedTables, new List<View>() { currentView }, "Filter Adjustment");
            fa.ShowDialog();

            MaddenTable mt = MaddenTable.FindTable(lMappedTables, fa.view.SourceName);
            fa.view.UpdateGridData(maddenDB[mt.Abbreviation], fa.lFilters);

            foreach (ListViewItem lvi in ((ListView)fa.view.DisplayControl).Items)
            {
                foreach (FieldFilter mass in fa.aFilters)
                    mass.Process(lMappedFields, ((MaddenRecord)lvi.Tag));
            }

            fa.view.RefreshGridData(maddenDB[mt.Abbreviation]);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFilterForm sff = new SaveFilterForm(SaveFilterForm.SaveAction.Load, "Load Saved Criteria");
            sff.ShowDialog();

            FilterAdjustForm fa = new FilterAdjustForm(lMappedFields, maddenDB.lTables, lMappedViews, sff.savedCriteria.Name, sff.savedCriteria.Table, sff.savedCriteria.listFilters);
            fa.ShowDialog();

            MaddenTable mt = fa.table;
            fa.view.UpdateGridData(maddenDB[mt.Abbreviation], fa.lFilters);

            foreach (ListViewItem lvi in ((ListView)fa.view.DisplayControl).Items)
            {
                foreach (FieldFilter mass in fa.aFilters)
                    mass.Process(lMappedFields, ((MaddenRecord)lvi.Tag));
            }

            fa.view.RefreshGridData(maddenDB[mt.Abbreviation]);
        }
    }

    public class BitStream
	{	public	byte[]		buffer;
		public	BitArray	bitarray;
		public	long		bitsused;


		public void AllocBits( long bitcount )
		{	buffer		= new byte [ (bitcount /8) ];
			bitsused	= bitcount;
			bitarray	= new BitArray( buffer );
		}
		public BitStream( )
		{	buffer		= null;
			bitsused	= 0;
			bitarray	= null;
		}
		public BitStream( long bitcount )
		{	AllocBits( bitcount );
		}
		public void Copy( BitStream bits )
		{	buffer		= new byte[ bits.buffer.Length ];
			bits.buffer.CopyTo( buffer, 0 );
			bitarray	= new BitArray( buffer );
		}

		public ulong ReadBits( long offset, long bitstoread )
		{	ulong	ret		= 0;
			ulong	mask	= 1;

			for( int i=(int)bitstoread; i>0; i-- )
			{	if( bitarray.Get( i +(int)offset -1 ) == true )
				{	ret	= ret | mask;
				}
				mask	= mask << 1;
			}
			return ret;
		}
		public void WriteBits( ulong value, long offset, long bitstowrite )
		{	ulong	mask	= 1;

			for( int i=(int)bitstowrite; i>0; i-- )
			{
				if( (mask & value) == mask )
					bitarray.Set( i +(int)offset -1, true );
				else
					bitarray.Set( i +(int)offset -1, false );
				mask	= mask << 1;
			}
		}
		private byte ReverseByte( byte b )
		{	byte	r		= 0;
			for( int i=0; i<8; i++ )
			{	r	= (byte)(r << 1);
				if( (b & 0x01) == 1 )
				{	r	|= 0x01;
				}
				b	= (byte)(b >> 1);
			}
			return r;
		}
		private byte[] StringToBuffer( string s, int maxchars )
		{	byte[]	buffer	= new byte[ maxchars ];
			char[]	str		= s.ToCharArray( );

			for( int i=0; i<maxchars && i<s.Length; i++ )
			{	buffer[i]	= (byte) str[i];
			}
			return buffer;
		}
		public void StoreBytes( string bytes, long offset, int bytestowrite )
		{	byte[]	buffer	= StringToBuffer( bytes, bytestowrite );
			for( int i=0; i<bytestowrite; i++ )
				WriteBits( ( (ulong)(buffer[i]) & 0x000000FF ), offset +(i *8), 8 );
		}
		public void ActivateBits( )
		{	// reverse each byte for this
			byte[]	nbuf	= new byte[ buffer.Length ];
			for( int i=0; i<buffer.Length; i++ )
			{	nbuf[i]	= ReverseByte( buffer[i] );
			}
			bitarray	= new BitArray( nbuf );
		}
		public void DeactivateBits( )
		{	byte[]	nbuf	= new byte[ buffer.Length ];
			for( int i=0; i<buffer.Length; i++ )
			{	nbuf[i]	= ( (byte)ReadBits( i *8, 8 ) );
			}
			buffer	= nbuf;
		}

		public string PrintBits( )
		{	string	ret		="";
			int		count1	= 0;

			foreach( Object obj in bitarray )
			{
				if( (count1 % 80) == 0 )
					ret	+= "\n" + (count1 / 80) + "\t";
				if( (count1 % 8) == 0 )
					ret += " ";
				if( obj.ToString( ) == "True" )
					ret += "1";
				else
					ret	+= "0";
				count1++;
			}

			ret	+= "\n\n\n";
			count1	= 0;

			foreach( Object obj in bitarray )
			{
				if( (count1 % 80) == 0 )
					ret	+= "\n" + (count1 / 80) + "\t";
				if( obj.ToString( ) == "True" )
					ret += "1";
				else
					ret	+= "0";
				count1++;
			}

			return ret;
		}
	}
	public class Field
	{
		#region members
		public string	name;
		public ulong	type;
		public ulong	offset;
		public ulong	bits;

		// for experimental XML config mapping
		public string	Abbreviation	= "";
		public string	Name			= "";
		public string	Description		= "";
		public string	ControlType		= "";
		public Control	EditControl		= new TextBox( );
		public int		ControlItems	= 0;
		public string	ControlLink		= "";
		public string	ControlIF		= "";
		public string	ControlRF		= "";
		public string	ControlRF2		= "";
		public bool		ControlLocked	= false;
		public int		Offset			= 0;
		#region new for calculated type, 10/23/13
		public class Variable
		{	public string	vField		= "";
			public double	Multiplier	= 0.0;

			public double Calc( List<Field> lMappedFields, MaddenRecord record )
			{	double	r	= 0.0;
				Field	f	= FindField( lMappedFields, vField );

				if( f != null )
				{	double	v	= Convert.ToDouble( record[ f.Abbreviation ] );
					r			= v * Multiplier;
				}

				return r;
			}
			public static List<Variable> ReadVariables( XmlTextReader reader, string Path )
			{
				List<Variable>	vars	= new List<Variable>( );
				Variable		var		= null;

				while( reader.Read( ) )
				{
					switch( reader.NodeType )
					{
						case XmlNodeType.Element:
							if( reader.Name == "Variable" )
								var	= new Variable( );

							Path	+= reader.Name + "\\";
							break;

						case XmlNodeType.Text:
							if( Path.EndsWith( "Variable\\Field\\" ) )
								var.vField		= reader.Value;

							if( Path.EndsWith( "Variable\\Multiplier\\" ) )
								var.Multiplier	= Convert.ToDouble( reader.Value );
							break;

						case XmlNodeType.EndElement:
							if( reader.Name == "Variable" )
								vars.Add( var );

							try
							{	Path	= Path.Remove( Path.LastIndexOf( reader.Name + "\\" ) );
							}	catch( Exception e )
							{
								MessageBox.Show( "XML closing element not found: " + reader.Name + ", " + reader.LineNumber, "Error in XML config" );
								throw( e );
							}
							if( reader.Name == "Variables" )	// should only ever return from here
								return vars;
						
							break;
					}

				}
				// bad XML!
				throw( new Exception( "Closing Variables Tag Missing! Exiting!" ) );
			}
		}
		public class Formula
		{
			public string			IndexValue	= "";
			public List<Variable>	Variables	= new List<Variable>( );
			public double			Adjustment	= 0.0;

			public double Calc( List<Field> lMappedFields, MaddenRecord record )
			{	double	r	= 0.0;

				foreach( Variable v in Variables )
					r	+= v.Calc( lMappedFields, record );

				return r + Adjustment;
			}
			public static List<Formula> ReadFormulas( XmlTextReader reader, string Path )
			{
				List<Formula>	forms	= new List<Formula>( );
				Formula			form	= null;

				while( reader.Read( ) )
				{
					switch( reader.NodeType )
					{
						case XmlNodeType.Element:
							if( reader.Name == "Formula" )
								form	= new Formula( );

							if( reader.Name == "Variables" )
							{	form.Variables	= Variable.ReadVariables( reader, Path + "Variables\\" );
								break;
							}

							Path	+= reader.Name + "\\";
							break;

						case XmlNodeType.Text:
							if( Path.EndsWith( "Formula\\IndexValue\\" ) )
								form.IndexValue	= reader.Value;

							if( Path.EndsWith( "Formula\\Adjustment\\" ) )
								form.Adjustment	= Convert.ToDouble( reader.Value );
							break;

						case XmlNodeType.EndElement:
							if( reader.Name == "Formula" )
								forms.Add( form );

							try
							{	Path	= Path.Remove( Path.LastIndexOf( reader.Name + "\\" ) );
							}	catch( Exception e )
							{
								MessageBox.Show( "XML closing element not found: " + reader.Name + ", " + reader.LineNumber, "Error in XML config" );
								throw( e );
							}
							if( reader.Name == "Formulas" )	// should only ever return from here
								return forms;
						
							break;
					}

				}
				// bad XML!
				throw( new Exception( "Closing Formulas Tag Missing! Exiting!" ) );
			}
		}
		public double			Min			= 0.0;
		public double			Max			= 0.0;
		public	List<Formula>	Formulas	= new List<Formula>( );

		// only if this is a Calculated type should this function be called
		public double RunFormula( List<Field> lMappedFields, MaddenRecord record )
		{	double	r		= 0.0;
			Field	f		= FindField( lMappedFields, ControlIF );

			if( f == null )
				return r;

			Formula	form	= Formulas.Find( (a) => a.IndexValue == record[ f.Abbreviation ] );
			if( form == null )
					form	= Formulas.Find( (a) => a.IndexValue == "*" );
			if( form != null )
				r	= form.Calc( lMappedFields, record );
			r	= Math.Min( r, Max );
			r	= Math.Max( r, Min );

			return Math.Round( r, 0 );
		}
		#endregion
		// new attempt to speed up searches with ref objs
		public Dictionary<string,int>		KeyToIndexMappings	= new Dictionary<string,int>( );
		#endregion

		public static int	fieldsize	= 16;

		public enum FieldType
		{
			tdbString = 0,
			tdbBinary = 1,
			tdbSInt = 2,
			tdbUInt = 3,
			tdbFloat = 4
		}

		public Field( )
		{	name		= "";
			type		= 0;
			offset		= 0;
			bits		= 0;
		}
		public Field( string Name, FieldType Type, ulong Offset, ulong Bits )
		{	name		= Name;
			type		= (ulong) Type;
			offset		= Offset;
			bits		= Bits;
		}
		public override string ToString()
		{
			if( Name != null && Name != "" )					return Name;
			if( Abbreviation != null && Abbreviation != "" )	return Abbreviation;
			if( name != null )
				return name;

			return base.ToString();
		}
		public int CompareDataAsType( string data1, string data2 )
		{	int	returnVal	= 0;

			//if( this.ControlType == "ComboBox" && data1.IndexOf( '(' ) >-1 && data1.IndexOf( ')' ) >-1 )
			//{	data1	= data1.Substring( data1.IndexOf( '(' ) +1 );
			//    data1	= data1.Substring( 0, data1.IndexOf( ')' ) );
			//    data2	= data2.Substring( data2.IndexOf( '(' ) +1 );
			//    data2	= data2.Substring( 0, data2.IndexOf( ')' ) );
			//}

			switch( type )
			{
				case (ulong) Field.FieldType.tdbString:
				case (ulong) Field.FieldType.tdbBinary:
					returnVal	= String.Compare( data1, data2 );
					break;

				case (ulong) Field.FieldType.tdbSInt:
				case (ulong) Field.FieldType.tdbUInt:
					returnVal	= Convert.ToInt32( data1 ).CompareTo( Convert.ToInt32( data2 ) );
					break;

				case (ulong) Field.FieldType.tdbFloat:
					returnVal	= Convert.ToDouble( data1 ).CompareTo( Convert.ToDouble( data2 ) );
					break;
			}
			return returnVal;
		}

		public void WriteField( byte[] buf, int _offset )
		{	char[]	arr	= name.ToCharArray( );

			DBFileInfo.WriteDW2Buf( buf, _offset +0, (uint) type );
			DBFileInfo.WriteDW2Buf( buf, _offset +4, (uint) offset );

			buf[ _offset + 8 ]	= (byte) arr[3];
			buf[ _offset + 9 ]	= (byte) arr[2];
			buf[ _offset +10 ]	= (byte) arr[1];
			buf[ _offset +11 ]	= (byte) arr[0];

			DBFileInfo.WriteDW2Buf( buf, _offset +12, (uint) bits );
		}

		public static Field ReadEntry( FileStream fs, int offset )
		{	byte[]	array	= new byte[ fieldsize ];
			Field	field	= new Field( );

			fs.Position		= offset;
			fs.Read( array, 0, fieldsize );
			field.type		= (((ulong)array[3]) ) | (((ulong)array[2]) << 8 ) | (((ulong)array[1]) << 16 ) | (((ulong)array[0]) << 24 );
			field.offset	= (((ulong)array[7]) ) | (((ulong)array[6]) << 8 ) | (((ulong)array[5]) << 16 ) | (((ulong)array[4]) << 24 );
			field.bits		= (((ulong)array[15]) ) | (((ulong)array[14]) << 8 ) | (((ulong)array[13]) << 16 ) | (((ulong)array[12]) << 24 );
			field.name		= Convert.ToChar( array[11] ).ToString( ) + Convert.ToChar( array[10] ).ToString( ) + Convert.ToChar( array[9] ).ToString( ) + Convert.ToChar( array[8] ).ToString( );
			return field;
		}
		public static List<Field> ReadFields( DBTable table, FileStream fs )
		{	List<Field>	lFields	= new List<Field>( );

			for( int i=0; i< table.numfields; i++ )
			{	Field	f	= ReadEntry( fs, (int) table.fieldStart + ( i * fieldsize ) );
				lFields.Add( f );
			}

			return lFields;
		}
		public static Field GetField( List<Field> lFields, string name )
		{	foreach( Field f in lFields )
			{	if( f.name == name )
					return f;
			}
			return null;
		}
		public static byte[] ReadBytes( Field f, BitStream bits )
		{	byte[]	buf	= new byte[ f.bits/8 ];

			for( int i=0; i < (int)(f.bits/8); i++ )
			{	buf[i]	= bits.buffer[ (int)(f.offset / 8) +i ];
			}
			return buf;
		}
		public static string ReadString( Field f, BitStream bits )
		{	string	buf	= "";

			for( int i=0; i < (int)(f.bits/8); i++ )
			{	byte	b	= bits.buffer[ (int)(f.offset / 8) +i ];
				if( b != 0 )
					buf	+= Convert.ToChar( b ).ToString( );
			}
			return buf;
		}
		public static byte[] ReadBytes( List<Field> lFields, string name, BitStream bits )
		{	Field	f	= Field.GetField( lFields, name );
			return ReadBytes( f, bits );
		}
		public static string ReadString( List<Field> lFields, string name, BitStream bits )
		{	Field	f	= Field.GetField( lFields, name );
			return ReadString( f, bits );
		}

		public static Field GetFieldByName( List<Field> lFields, string name )
		{	foreach( Field f in lFields )
			{	if( f.Name == name )
					return f;
			}
			return null;
		}
		public static Field GetFieldByAbbreviation( List<Field> lFields, string name )
		{	foreach( Field f in lFields )
			{	if( f.Abbreviation == name || f.name == name )
					return f;
			}
			return null;
		}
		public static Field FindField( List<Field> lFields, string Name )
		{
			Field	f	= GetFieldByName( lFields, Name );
			if( f != null )
				return f;
			return GetFieldByAbbreviation( lFields, Name );
		}

	}
	public class DBData
	{	public	Field		field		= null;
		private	string		_str		= "";
		private ulong		_ulong		= 0;
		private byte[]		array		= null;

		
		public DBData( ){}
		public DBData( Field f )
		{	SetField( f );
		}
		public DBData( Field f, BitStream bits )
		{	SetField( f );
			ReadData( bits );
		}
		public DBData( string fieldname, int type, string data )
		{	field		= new Field( );
			field.name	= fieldname;
			field.type	= (ulong) type;
			if( type == (int) Field.FieldType.tdbString )
				_str	= data;
			else
				_ulong	= Convert.ToUInt32( data );
		}
		public void SetField( Field f ){	field	= f; }
		public string GetFieldName( ){		return field.name; }
		public void ReadData( BitStream bits )
		{
			switch( field.type )
			{
				case (ulong) Field.FieldType.tdbString:
					_str	= Field.ReadString( field, bits );
					break;
				case (ulong) Field.FieldType.tdbBinary:
					array	= Field.ReadBytes( field, bits );
					break;
				case (ulong) Field.FieldType.tdbSInt:
				case (ulong) Field.FieldType.tdbUInt:
				case (ulong) Field.FieldType.tdbFloat:
					_ulong	= bits.ReadBits( (long)field.offset, (long)field.bits );
					break;
			}
		}
		public void WriteData( BitStream bits )
		{
			switch( field.type )
			{
				case (ulong) Field.FieldType.tdbString:
					bits.StoreBytes( _str, (long) field.offset, (int) field.bits /8 );
					break;

				case (ulong) Field.FieldType.tdbBinary:	// need to do this one day I suppose
					break;
				case (ulong) Field.FieldType.tdbSInt:
				case (ulong) Field.FieldType.tdbUInt:
				case (ulong) Field.FieldType.tdbFloat:
					bits.WriteBits( _ulong, (long) field.offset, (int) field.bits );
					break;
			}
		}
		public string Data
		{	get
			{
				switch( field.type )
				{
					case (ulong) Field.FieldType.tdbString:
						return _str;

					case (ulong) Field.FieldType.tdbBinary:	// major assumption made that all binary fields are %8 =0
						string	s	= "";
						foreach( byte b in array )
							s	+= b.ToString( "X2" );
						return s;

					case (ulong) Field.FieldType.tdbSInt:
						return ((int) _ulong).ToString( );

					case (ulong) Field.FieldType.tdbUInt:
						return ((uint) _ulong).ToString( );

					case (ulong) Field.FieldType.tdbFloat:
						return ((float) _ulong).ToString( "F" );
				}
				return "";
			}
			set
			{
				switch( field.type )
				{
					case (ulong) Field.FieldType.tdbString:
						_str	= value;
						break;

					case (ulong) Field.FieldType.tdbBinary:
						char[]	temp	= ((string)value).ToCharArray( );
						if( array == null )
							array		= new byte[ temp.Length /2 ];
						for( int i=0; i < (temp.Length /2); i++ )
						{	array[ i ]	= Convert.ToByte( temp[ (i *2) +0 ].ToString( ) + temp[ (i *2) +1 ].ToString( ), 16 );
						}
						break;

					case (ulong) Field.FieldType.tdbSInt:
						_ulong	= Convert.ToUInt32( value );
						break;

					case (ulong) Field.FieldType.tdbUInt:
						_ulong	= Convert.ToUInt32( value );
						break;

					case (ulong) Field.FieldType.tdbFloat:
						_ulong	= Convert.ToUInt32( value );
						break;
				}

			}
		}
		public override string ToString()
		{
			return field.name + ":" + Data;
		}
	}
	public class DBTable
	{
		public string	TableName;							// 1st 4 bytes
		public UInt32	offsetFromIndex;					// from end of table index

		public int		headersize		= 8;

		public UInt32	priorcrc;							// checksum?
		public UInt32	unknown_2		= 0x00000006;		// type?
		public UInt32	len_bytes;							// size of each record in bytes
		public UInt32	len_bits;							// size of each record in bits
		public UInt32	zero			= 0;
		public UInt16	maxrecords;
		public UInt16	currecords;
		public UInt32	unknown_3		= 0x0000ffff;
		public byte		numfields;
		public byte		indexcount		= 0;
		public UInt16	zero2			= 0;
		public UInt32	zero3			= 0;
		public UInt32	headercrc;							// or crc poly?

		public int		infosize		= 40;

		public long		fieldStart		= 0;
		public long		dataStart		= 0;

		public UInt32	calcPcrc		= 0;
		public UInt32	calcHcrc		= 0;


		public DBTable( )
		{	TableName		= "";
			offsetFromIndex	= 0;
			priorcrc		= 0;
			unknown_2		= 6;
			len_bytes		= 0;
			len_bits		= 0;
			zero			= 0;
			maxrecords		= 0;
			currecords		= 0;
			unknown_3		= 0x0000ffff;
			numfields		= 0;
			indexcount		= 0;
			zero2			= 0;
			zero3			= 0;
			headercrc		= 0;
		}
		public DBTable( FileStream fs )
		{	TableName		= "";
			offsetFromIndex	= 0;
			priorcrc		= 0;
			unknown_2		= 0;
			len_bytes		= 0;
			len_bits		= 0;
			zero			= 0;
			maxrecords		= 0;
			currecords		= 0;
			unknown_3		= 0;
			numfields		= 0;
			indexcount		= 0;
			zero2			= 0;
			zero3			= 0;
			headercrc		= 0;

			ReadTableDefinition( fs );
		}
		public DBTable( DBTable org )
		{	TableName		= org.TableName;
			offsetFromIndex	= org.offsetFromIndex;
			priorcrc		= org.priorcrc;
			unknown_2		= org.unknown_2;
			len_bytes		= org.len_bytes;
			len_bits		= org.len_bits;
			zero			= org.zero;
			maxrecords		= org.maxrecords;
			currecords		= org.currecords;
			unknown_3		= org.unknown_3;
			numfields		= org.numfields;
			indexcount		= org.indexcount;
			zero2			= org.zero2;
			zero3			= org.zero3;
			headercrc		= org.headercrc;
		}
		public void ReadTableDefinition( FileStream fs )
		{	byte[]	array	= new byte[ headersize ];

			fs.Read( array, 0, headersize );
			TableName		= Convert.ToChar( array[3] ).ToString( ) + Convert.ToChar( array[2] ).ToString( ) + Convert.ToChar( array[1] ).ToString( ) + Convert.ToChar( array[0] ).ToString( );
			offsetFromIndex	= (((UInt32)array[7]) ) | (((UInt32)array[6]) << 8 ) | (((UInt32)array[5]) << 16 ) | (((UInt32)array[4]) << 24 );
		}
		public void ReadTableHeader( FileStream fs, long datastart )
		{	byte[]	array	= new byte[ infosize ];

			fs.Position	= datastart + offsetFromIndex;
			fs.Read( array, 0, infosize );

			priorcrc	= ( ((UInt32)array[ 3]) ) | ( ((UInt32)array[ 2]) << 8 ) | ( ((UInt32)array[ 1]) << 16 ) | ( ((UInt32)array[ 0]) << 24 );
			unknown_2	= ( ((UInt32)array[ 7]) ) | ( ((UInt32)array[ 6]) << 8 ) | ( ((UInt32)array[ 5]) << 16 ) | ( ((UInt32)array[ 4]) << 24 );
			len_bytes	= ( ((UInt32)array[11]) ) | ( ((UInt32)array[10]) << 8 ) | ( ((UInt32)array[ 9]) << 16 ) | ( ((UInt32)array[ 8]) << 24 );
			len_bits	= ( ((UInt32)array[15]) ) | ( ((UInt32)array[14]) << 8 ) | ( ((UInt32)array[13]) << 16 ) | ( ((UInt32)array[12]) << 24 );
			zero		= ( ((UInt32)array[19]) ) | ( ((UInt32)array[18]) << 8 ) | ( ((UInt32)array[17]) << 16 ) | ( ((UInt32)array[16]) << 24 );
			maxrecords	= (UInt16) (( ((UInt16)array[21]) ) | ( ((UInt16)array[20]) << 8 ));
			currecords	= (UInt16) (( ((UInt16)array[23]) ) | ( ((UInt16)array[22]) << 8 ));
			unknown_3	= ( ((UInt32)array[27]) ) | ( ((UInt32)array[26]) << 8 ) | ( ((UInt32)array[25]) << 16 ) | ( ((UInt32)array[24]) << 24 );
			numfields	= array[28];
			indexcount	= array[29];
			zero2		= (UInt16) (( ((UInt16)array[31]) ) | ( ((UInt16)array[30]) << 8 ));
			zero3		= ( ((UInt32)array[35]) ) | ( ((UInt32)array[34]) << 8 ) | ( ((UInt32)array[33]) << 16 ) | ( ((UInt32)array[32]) << 24 );
			headercrc	= ( ((UInt32)array[39]) ) | ( ((UInt32)array[38]) << 8 ) | ( ((UInt32)array[37]) << 16 ) | ( ((UInt32)array[36]) << 24 );

			fieldStart	= datastart + offsetFromIndex + infosize;
			dataStart	= fieldStart + ( numfields * 16 );

			DB_CRC	db	= new DB_CRC( );
			calcHcrc	= ~db.crc32_be( 0, array, (uint) infosize -8, 4 );
		}
		public void WriteHeader( byte[] buf )
		{
			DBFileInfo.WriteDW2Buf( buf, (int)( fieldStart - infosize +0x00 ), priorcrc );
			DBFileInfo.WriteDW2Buf( buf, (int)( fieldStart - infosize +0x04 ), unknown_2 );
			DBFileInfo.WriteDW2Buf( buf, (int)( fieldStart - infosize +0x08 ), len_bytes );
			DBFileInfo.WriteDW2Buf( buf, (int)( fieldStart - infosize +0x0C ), len_bits );
			DBFileInfo.WriteDW2Buf( buf, (int)( fieldStart - infosize +0x10 ), zero );
			DBFileInfo.WriteW2Buf(  buf, (int)( fieldStart - infosize +0x14 ), maxrecords );
			DBFileInfo.WriteW2Buf(  buf, (int)( fieldStart - infosize +0x16 ), currecords );
			DBFileInfo.WriteDW2Buf( buf, (int)( fieldStart - infosize +0x18 ), unknown_3 );
			buf[fieldStart - infosize +0x1C]	= numfields;
			buf[fieldStart - infosize +0x1D]	= indexcount;
			DBFileInfo.WriteW2Buf(  buf, (int)( fieldStart - infosize +0x1E ), zero2 );
			DBFileInfo.WriteDW2Buf( buf, (int)( fieldStart - infosize +0x20 ), zero3 );
			DBFileInfo.WriteDW2Buf( buf, (int)( fieldStart - infosize +0x24 ), headercrc );
		}
	}
	public class DBFileInfo
	{
		#region internals & statics
		public UInt32		tableIndexOffset	= 0x24;		// from DB start ( really 20, but we're treating it differently )
		public byte[]		theFile				= null;
		public long			absPosition			= 0;
		public long			startData			= 0;

		public UInt16		header;
		public UInt16		version;
		public UInt32		unknown_1;
		public UInt32		DBsize;
		public UInt32		zero;
		public UInt32		tableCount;
		public UInt32		unknown_2;						// checksum on the header

		public int			headersize		= 24;			// really 20...

		public UInt32		calcdHeaderCRC		= 0;
		public UInt32		calcdEOFCRC			= 0;

		public List<DBTable>	lTables;

		#endregion
		public DBFileInfo( )
		{
			header		= 0;
			version		= 0;
			unknown_1	= 0;
			DBsize		= 0;
			zero		= 0;
			tableCount	= 0;
			unknown_2	= 0;
			lTables		= new List<DBTable>( );
		}
		public DBFileInfo( FileStream fs )
		{
			header		= 0;
			version		= 0;
			unknown_1	= 0;
			DBsize		= 0;
			zero		= 0;
			tableCount	= 0;
			unknown_2	= 0;
			lTables		= new List<DBTable>( );

			ReadDBHeader( fs );
		}
		public void ReadDBHeader( FileStream fs )
		{	byte[]	array	= new byte[ headersize ];

			// first, load the whole file into memory
			fs.Position	= 0;
			theFile		= new byte[ fs.Length ];
			fs.Read( theFile, 0, (int) fs.Length );

			// now reset the file. already wrote the code to read all data in via the file, so keeping that, but will change writes to in-memory
			absPosition	= 0;
			fs.Position	= 0;
			fs.Read( array, 0, 4 );

			fs.Position	= absPosition;
			fs.Read( array, 0, headersize );

			header	= (UInt16) (( ((UInt16)array[1]) ) | ( ((UInt16)array[0]) << 8 ));
			version	= (UInt16) (( ((UInt16)array[3]) ) | ( ((UInt16)array[2]) << 8 ));

			unknown_1	= ( ((UInt32)array[ 7]) ) | ( ((UInt32)array[ 6]) << 8 ) | ( ((UInt32)array[ 5]) << 16 ) | ( ((UInt32)array[ 4]) << 24 );
			DBsize		= ( ((UInt32)array[11]) ) | ( ((UInt32)array[10]) << 8 ) | ( ((UInt32)array[ 9]) << 16 ) | ( ((UInt32)array[ 8]) << 24 );
			zero		= ( ((UInt32)array[15]) ) | ( ((UInt32)array[14]) << 8 ) | ( ((UInt32)array[13]) << 16 ) | ( ((UInt32)array[12]) << 24 );
			tableCount	= ( ((UInt32)array[19]) ) | ( ((UInt32)array[18]) << 8 ) | ( ((UInt32)array[17]) << 16 ) | ( ((UInt32)array[16]) << 24 );
			unknown_2	= ( ((UInt32)array[23]) ) | ( ((UInt32)array[22]) << 8 ) | ( ((UInt32)array[21]) << 16 ) | ( ((UInt32)array[20]) << 24 );

			if( header == 0x4442 )	// 'DB'
			{
				for( int i=0; i< tableCount; i++ )		// read table index
					lTables.Add( new DBTable( fs ) );

				startData	= fs.Position;				// record start of actual data

				foreach( DBTable te in lTables )		// read table info
					te.ReadTableHeader( fs, startData );
			}

		}
		public void CalcChecksums( FileStream fs )
		{	DB_CRC	 db			= new DB_CRC( );
			UInt32	priorcrc	= 0;

			// first, load the whole file into memory
			fs.Position	= 0;
			theFile		= new byte[ fs.Length ];
			fs.Read( theFile, 0, (int) fs.Length );
			fs.Position	= 0;

			// DB header
			calcdHeaderCRC		= ~db.crc32_be( 0, theFile, 20, 0 );

			// Table index
			priorcrc	= ~db.crc32_be( 0, theFile, tableCount *8, 24 );

			// each table, minus 1
			for( long i=0; i < tableCount -1; i++ )
			{	long	start	= startData + lTables[ (int) i ].offsetFromIndex + lTables[ (int) i ].infosize;
				long	end		= startData + lTables[ (int) i +1 ].offsetFromIndex;

				lTables[ (int) i ].calcPcrc	= priorcrc;
				priorcrc		= ~db.crc32_be( 0, theFile, (uint)( end - start ), (uint) start );
			}
			// the last table
			long laststart	= startData + lTables[ (int)( tableCount -1 ) ].offsetFromIndex + lTables[ (int)( tableCount -1 ) ].infosize;
			long lastend	= DBsize -4;

			lTables[ (int)( tableCount -1 ) ].calcPcrc	= priorcrc;
			calcdEOFCRC		= ~db.crc32_be( 0, theFile, (uint)( lastend - laststart ), (uint) laststart );


			// put the data back into the buffer
			WriteDW2Buf( theFile, 20, calcdHeaderCRC );
			foreach( DBTable dt in lTables )
			{
				WriteDW2Buf( theFile, (int)(startData + dt.offsetFromIndex), dt.calcPcrc );
				WriteDW2Buf( theFile, (int)(startData + dt.offsetFromIndex + dt.infosize -4), dt.calcHcrc );
			}
			WriteDW2Buf( theFile, (int)(DBsize -4), calcdEOFCRC );

		}
		public void CalcChecksums( )
		{	DB_CRC	 db			= new DB_CRC( );
			UInt32	priorcrc	= 0;
			long	start		= 0;
			long	end			= 0;


			// DB header
			calcdHeaderCRC		= ~db.crc32_be( 0, theFile, 20, 0 );

			// Table index
			priorcrc	= ~db.crc32_be( 0, theFile, tableCount *8, 24 );

			// each table, minus 1
			for( long i=0; i < tableCount -1; i++ )
			{
				// table header crc
				start							= startData + lTables[ (int) i ].offsetFromIndex +4;
				lTables[ (int) i ].calcHcrc		= ~db.crc32_be( 0, theFile, (uint)( lTables[ (int) i ].infosize -8 ), (uint) start );

				// table data crc
				start							= startData + lTables[ (int) i ].offsetFromIndex + lTables[ (int) i ].infosize;
				end								= startData + lTables[ (int) i +1 ].offsetFromIndex;

				lTables[ (int) i ].calcPcrc		= priorcrc;
				priorcrc						= ~db.crc32_be( 0, theFile, (uint)( end - start ), (uint) start );
			}

			// the last table
			// table header crc
			start											= startData + lTables[ (int)( tableCount -1 ) ].offsetFromIndex +4;
			lTables[ (int)( tableCount -1 ) ].calcHcrc		= ~db.crc32_be( 0, theFile, (uint)( lTables[ (int)( tableCount -1 ) ].infosize -8 ), (uint) start );

			// table data crc
			start	= startData + lTables[ (int)( tableCount -1 ) ].offsetFromIndex + lTables[ (int)( tableCount -1 ) ].infosize;
			end		= DBsize -4;

			lTables[ (int)( tableCount -1 ) ].calcPcrc	= priorcrc;
			calcdEOFCRC		= ~db.crc32_be( 0, theFile, (uint)( end - start ), (uint) start );


			// put the data back into the buffer
			WriteDW2Buf( theFile, 20, calcdHeaderCRC );
			foreach( DBTable dt in lTables )
			{
				WriteDW2Buf( theFile, (int)(startData + dt.offsetFromIndex), dt.calcPcrc );
				WriteDW2Buf( theFile, (int)(startData + dt.offsetFromIndex + dt.infosize -4), dt.calcHcrc );
			}
			WriteDW2Buf( theFile, (int)(DBsize -4), calcdEOFCRC );

		}
		public void Save( FileStream fs )
		{	fs.Position	= 0;
			CalcChecksums( );
			fs.Write( theFile, 0, (int) DBsize );
		}

		/// <summary>
		/// used in the franchise -> roster conversion
		/// assumes theFile is already allocated
		/// </summary>
		public void DBHeaderToBuffer( )
		{
			WriteW2Buf(  theFile,  0, header );
			WriteW2Buf(  theFile,  2, version );
			WriteDW2Buf( theFile,  4, unknown_1 );
			WriteDW2Buf( theFile,  8, DBsize );
			WriteDW2Buf( theFile, 12, zero );
			WriteDW2Buf( theFile, 16, tableCount );
			WriteDW2Buf( theFile, 20, unknown_2 );
		}

		public static void WriteDW2Buf( byte[] buf, int offset, UInt32 dword )
		{
			buf[offset +0]	= (byte)( (dword >> 24) & 0x000000FF );
			buf[offset +1]	= (byte)( (dword >> 16) & 0x000000FF );
			buf[offset +2]	= (byte)( (dword >>  8) & 0x000000FF );
			buf[offset +3]	= (byte)( (dword      ) & 0x000000FF );
		}
		public static void WriteW2Buf( byte[] buf, int offset, UInt16 word )
		{
			buf[offset +0]	= (byte)( (word >>  8) & 0x00FF );
			buf[offset +1]	= (byte)( (word      ) & 0x00FF );
		}
		public static void WriteBytesFromBuf( byte[] des, byte[] src, long offset, int bytes )
		{
			for( int i=0; i < bytes; i++ )
				des[ i ]	= src[ i +offset ];
		}
		public static void WriteBytesToBuf( byte[] des, byte[] src, long offset, int bytes )
		{
			for( int i=0; i < bytes; i++ )
				des[ i +offset ]	= src[ i ];
		}
	}
	public class DB_CRC
	{
		private	UInt32		CRCPOLY_BE		= 0x04c11db7;
		private	UInt32[]	crc32table_be	= new UInt32 [256];

		public DB_CRC( )
		{	crc32init_be( );
		}
		public DB_CRC( UInt32 poly )
		{	CRCPOLY_BE	= poly;
			crc32init_be( );
		}
		private void crc32init_be( )
		{
			UInt32	i, j;
			UInt32	crc		= 0x80000000;

			crc32table_be[0] = 0;

			for( i = 1 ; i < 1<< 4; i <<= 1)
			{
				crc	= (crc << 1) ^ ( ((crc & 0x80000000) != 0) ? CRCPOLY_BE : 0);
				for( j = 0; j < i; j++ )
					crc32table_be[ i+j ]	= crc ^ crc32table_be[ j ];
			}
		}
		public UInt32 crc32_be( UInt32 crc, byte[] p, UInt32 len, UInt32 start=0 )
		{	UInt32	x	= start;

			crc ^= 0xFFFFFFFF;
			while( len-- >0 )
			{
				crc	^= (uint) p[ x++ ] << 24;
				crc	= (crc << 4) ^ crc32table_be[crc >> 28];
				crc	= (crc << 4) ^ crc32table_be[crc >> 28];
			}
			return crc ^ 0xFFFFFFFF;
		}
	}
	public class MC02Descriptor
	{
		public byte[]	data;	

		public MC02Descriptor( )
		{	data	= new byte[ 40 ];

			SetDword( 16, 0x524c5f50 );
			SetDword( 20, 0x61746368 );
			SetDword( 24, 0x322d3731 );
			SetDword( 28, 0x35303032 );
			SetDword( 32, 0x00000000 );
			SetDword( 36, 0x00000000 );
		
			Offset	= 0x000b15dc;
			Year	= 2012;
			Month	= 1;
			Day		= 1;
			Hour	= 12;
			Minute	= 0;
			Second	= 0;
		}

		private UInt16 GetWord( int index )
		{
			return	(UInt16) ( (( Convert.ToUInt16( data[index] ) & 0x00ff ) << 8 ) | ( Convert.ToUInt16( data[index+1] ) & 0x00ff ));
		}
		private UInt32 GetDword( int index )
		{
			return ( Convert.ToUInt32( GetWord(index) & 0x0000ffff ) << 16 ) | ( Convert.ToUInt32( GetWord(index+2) ) & 0x0000ffff );
		}
		private void SetWord( int index, UInt16 word )
		{
			data[index +0]	= (byte)( (word >>  8) & 0x00FF );
			data[index +1]	= (byte)( (word      ) & 0x00FF );
		}
		private void SetDword( int index, UInt32 dword )
		{
			SetWord( index +0, (UInt16)( (dword >> 16) & 0x0000FFFF ) );
			SetWord( index +2, (UInt16)( (dword      ) & 0x0000FFFF ) );
		}

		public UInt32 Offset
		{
			get
			{	return GetDword( 0 );
			}
			set
			{	SetDword( 0, value );
			}
		}
		public UInt16 Year
		{
			get
			{	return GetWord( 4 );
			}
			set
			{	SetWord( 4, value );
			}
		}
		public UInt16 Month
		{
			get
			{	return GetWord( 6 );
			}
			set
			{	SetWord( 6, value );
			}
		}
		public UInt16 Day
		{
			get
			{	return GetWord( 8 );
			}
			set
			{	SetWord( 8, value );
			}
		}
		public UInt16 Hour
		{
			get
			{	return GetWord( 10 );
			}
			set
			{	SetWord( 10, value );
			}
		}
		public UInt16 Minute
		{
			get
			{	return GetWord( 12 );
			}
			set
			{	SetWord( 12, value );
			}
		}
		public UInt16 Second
		{
			get
			{	return GetWord( 14 );
			}
			set
			{	SetWord( 14, value );
			}
		}
	}
	public class MaddenRecord
	{
		public MaddenTable		Table			= null;
		public List<DBData>		lEntries		= null;

		public MaddenRecord( )
		{	lEntries	= new List<DBData>( );
		}
		public MaddenRecord( MaddenTable table, List<Field> lFields )
		{	BitStream	bits	= new BitStream( );

			Table	= table;
			bits.AllocBits( Table.Table.len_bytes *8 );
			bits.ActivateBits( );
			
			lEntries	= new List<DBData>( );

			foreach( Field f in lFields )
			{	DBData	db	= new DBData( f );
				lEntries.Add( db );
			}
		}
		public MaddenRecord( int entryNum, MaddenTable table, List<Field> lFields, FileStream fs )
		{	BitStream	bits	= new BitStream( );

			Table	= table;
			bits.AllocBits( Table.Table.len_bytes *8 );
			fs.Position	= Table.Table.dataStart + ( entryNum * Table.Table.len_bytes );
			fs.Read( bits.buffer, 0, (int) Table.Table.len_bytes );
			bits.ActivateBits( );
			
			lEntries	= new List<DBData>( );

			foreach( Field f in lFields )
			{	DBData	db	= new DBData( f, bits );
				lEntries.Add( db );
			}
		}
		public MaddenRecord( MaddenRecord org )
		{	Table		= org.Table;
			lEntries	= new List<DBData>( );
			foreach( DBData db in org.lEntries )
			{	DBData	n	= new DBData( db.field );
				//n.Data		= db.Data;
				lEntries.Add( n );
			}
		}
		public MaddenRecord( MaddenRecord org, List<Field> lNewFields )
		{	Table		= org.Table;
			lEntries	= new List<DBData>( );
			foreach( Field f in lNewFields )
			{	DBData	n	= new DBData( f );
				DBData	r	= org.GetEntry( f.name );
				if( r != null )
					n.Data	= r.Data;
				lEntries.Add( n );
			}
		}
		public void CopyData( MaddenRecord org )
		{	if( org.lEntries.Count != lEntries.Count )
				return;

			for( int i=0; i < lEntries.Count; i++ )
				lEntries[i].Data	= org.lEntries[i].Data;
		}
		public void WriteRecord( int entryNum, byte[] buffer )
		{	BitStream	bits	= new BitStream( );

			bits.AllocBits( Table.Table.len_bytes *8 );

			long	position	= Table.Table.dataStart + ( entryNum * Table.Table.len_bytes );
			DBFileInfo.WriteBytesFromBuf( bits.buffer, buffer, position, (int) Table.Table.len_bytes );

			bits.ActivateBits( );
			
			foreach( DBData db in lEntries )
				db.WriteData( bits );

			bits.DeactivateBits( );

			position	= Table.Table.dataStart + ( entryNum * Table.Table.len_bytes );
			DBFileInfo.WriteBytesToBuf( buffer, bits.buffer, position, (int) Table.Table.len_bytes );
		}
		public DBData GetEntry( string fieldName )
		{	foreach( DBData data in lEntries )
			{	if( data.GetFieldName( ) == fieldName )
					return data;
			}
			return null;
		}
		public string this[ string fieldName ]
		{
			get
			{	DBData	data	= GetEntry( fieldName );
				if( data == null )
					return "0";
				return data.Data;
			}
			set
			{	DBData	data	= GetEntry( fieldName );
				if( data != null )
					data.Data	= value;
			}
		}
	}
	public class MaddenTable
	{
		public DBTable				Table			= null;
		public List<Field>			lFields			= null;
		public List<MaddenRecord>	lRecords		= null;

		// for experimental XML mapping
		public string				Abbreviation	= "";
		public string				Name			= "";


		public MaddenTable( DBTable table, FileStream fs )
		{
			Table			= table;
			Abbreviation	= table.TableName;
			lFields			= Field.ReadFields( Table, fs );
			lRecords		= new List<MaddenRecord>( );

			for( int i=0; i < Table.currecords; i++ )
			{	MaddenRecord	mr	= new MaddenRecord( i, this, lFields, fs );
				lRecords.Add( mr );
			}
		}
		public MaddenTable( MaddenTable table )
		{
			Table		= new DBTable( table.Table );
			lFields		= table.lFields;
			lRecords	= new List<MaddenRecord>( );

			foreach( MaddenRecord mr in table.lRecords )
			{	MaddenRecord	mr2	= new MaddenRecord( mr );
				mr2.Table			= this;
				mr2.CopyData( mr );
				lRecords.Add( mr2 );
			}
		}
		public MaddenTable( MaddenTable table, List<Field> lNewFields )
		{
			Table		= new DBTable( table.Table );
			lFields		= lNewFields;
			lRecords	= new List<MaddenRecord>( );

			foreach( MaddenRecord mr in table.lRecords )
			{	MaddenRecord	mr2	= new MaddenRecord( mr, lNewFields );
				mr2.Table			= this;
				lRecords.Add( mr2 );
			}
		}
		public MaddenTable( )
		{
		}

		public override string ToString( )
		{	return Table.TableName;
		}
		public static int CompareName( MaddenTable x, MaddenTable y )
        {
			return x.ToString( ).CompareTo( y.ToString( ) );
        }

		/// <summary>
		/// adds a record - to be used by wrapper class so that it can then set values appropriately
		/// </summary>
		public MaddenRecord AddNewRecord( )
		{	MaddenRecord	mr	= null;

			if( Table.currecords < Table.maxrecords )
			{	mr	= new MaddenRecord( this, lFields );

				if( mr != null )
				{	lRecords.Add( mr );
					Table.currecords++;
				}
			}
			return mr;
		}
		public bool InsertRecord( MaddenRecord mr )
		{
			if( Table.currecords < Table.maxrecords )
			{
				lRecords.Add( mr );
				Table.currecords++;
				return true;
			}
			return false;
		}

		/// <summary>
		/// write a back out to the file
		/// </summary>
		public void WriteTable( byte[] buffer )
		{
			Table.WriteHeader( buffer );

			int	x = 0;
			foreach( Field field in lFields )
			{	field.WriteField( buffer, (int)( Table.fieldStart + (x * 16) ) );
				x++;
			}

			x = 0;
			foreach( MaddenRecord mr in lRecords )
				mr.WriteRecord( x++, buffer );
		}
		/// <summary>
		/// find a MaddenTable from a list of MaddenTables ( typicalling in the MaddenDatabase )
		/// </summary>
		public static MaddenTable FindMaddenTable( List<MaddenTable> lMaddenTables, string name )
		{	foreach( MaddenTable mt in lMaddenTables )
			{	if( mt.Table.TableName == name )
					return mt;
			}
			return null;
		}

		public static MaddenTable GetTableByName( List<MaddenTable> lMaddenTables, string name )
		{	foreach( MaddenTable t in lMaddenTables )
			{	if( t.Name == name )
					return t;
			}
			return null;
		}
		public static MaddenTable GetTableByAbbreviation( List<MaddenTable> lMaddenTables, string name )
		{	foreach( MaddenTable t in lMaddenTables )
			{	if( t.Abbreviation == name )
					return t;
			}
			return null;
		}
		public static MaddenTable FindTable( List<MaddenTable> lMaddenTables, string Name )
		{
			MaddenTable	t	= GetTableByName( lMaddenTables, Name );
			if( t != null )
				return t;
			return GetTableByAbbreviation( lMaddenTables, Name );
		}
	}
	public class MaddenDatabase
	{	public	DBFileInfo			dbFileInfo		= null;
		public	List<MaddenTable>	lTables			= new List<MaddenTable>( );
		public	string				fileName		= "";
		public	string				realfileName	= "";
		public	MaddenFileType		type			= MaddenFileType.FileType_None;
		

		private MaddenDatabase( )
		{	dbFileInfo	= new DBFileInfo( );
		}
		public MaddenDatabase( string file )
		{
			FileStream	fs		= new FileStream( file, FileMode.Open, FileAccess.Read );
			realfileName		= file;

			// check file type
			type	= MaddenDatabase.CheckFileType( fs );

			#region unknown file type
			if( type == MaddenFileType.FileType_None )
			{	MessageBox.Show( "Error - this is not a DB or MC02 file!" );
				fs.Close( );
				return;
			}
			#endregion
			#region CON file type
			if( type == MaddenFileType.FileType_CON )
			{	MessageBox.Show( "You must first extract the MC02 / DB file from the roster!" );
				fs.Close( );
				return;
			}
			#endregion
			#region MC02 file type
			if( type == MaddenFileType.FileType_MC02 )
			{
				MC02Handler.Package	package	= null;
				byte[]	mc02				= new byte[ fs.Length ];

				try
				{	fs.Read( mc02, 0, (int) fs.Length );
					fs.Position	= 0;
					package	= new MC02Handler.Package( mc02 );
				} catch( Exception exception )
				{	MessageBox.Show( "Error opening MC02 package: " +exception.ToString( ) );
					fs.Close( );
					return;
				}

				// extract the DB file now & set the filename to work on
				fileName	= realfileName + ".DB";
				package.Extract( Package.DataType.SaveData, fileName );
				package.Dispose( );

				fs.Close( );
				fs			= new FileStream( fileName, FileMode.Open, FileAccess.Read );

			} else	// must be a DB file
			#endregion
			#region DB file type
				fileName	= file;
			#endregion

			dbFileInfo	= new DBFileInfo( fs );
			foreach( DBTable dbt in dbFileInfo.lTables )
				lTables.Add( new MaddenTable( dbt, fs ) );

			fs.Close( );
		}
		public MaddenTable GetTable( string tableName )
		{	foreach( MaddenTable table in lTables )
			{	if( table.Table.TableName == tableName )
					return table;
			}
			return null;
		}
		public MaddenTable this[ string tableName ]
		{
			get
			{	return GetTable( tableName );
			}
		}
		public void Save( )
		{
			FileStream	fs	= new FileStream( fileName, FileMode.Truncate, FileAccess.ReadWrite );

			foreach( MaddenTable mt in lTables )
				mt.WriteTable( dbFileInfo.theFile );

			dbFileInfo.Save( fs );
			fs.Close( );

			#region if this was an MC02 file, repackage
			if( type == MaddenDatabase.MaddenFileType.FileType_MC02 )
			{	fs	= new FileStream( realfileName, FileMode.Open, FileAccess.ReadWrite );

				// read the data descriptor @ 0x14 and save it
				byte[]	descriptor	= new byte[ 12 ];
				fs.Position			= 0x10;
				fs.Read( descriptor, 0, 12 );
				fs.Position			= 0;

				MC02Handler.Package	package	= null;
				byte[]	mc02				= new byte[ fs.Length ];

				try
				{	fs.Read( mc02, 0, (int) fs.Length );
					package	= new MC02Handler.Package( mc02 );

				} catch( Exception exception )
				{
					MessageBox.Show( "Error opening MC02 package to insert DB: " +exception.ToString( ) );
					Cursor.Current	= Cursors.Default;
					return;
				}

				package.Overwrite( Package.DataType.SaveData, dbFileInfo.theFile );
				mc02	= package.Save( true );

				// keep descriptor, ex. NCAA 14
				if( ! Form1.mc02Recalc )
				{
					mc02[ 0x10 ]	= descriptor[ 0 ];
					mc02[ 0x11 ]	= descriptor[ 1 ];
					mc02[ 0x12 ]	= descriptor[ 2 ];
					mc02[ 0x13 ]	= descriptor[ 3 ];
					mc02[ 0x14 ]	= descriptor[ 4 ];
					mc02[ 0x15 ]	= descriptor[ 5 ];
					mc02[ 0x16 ]	= descriptor[ 6 ];
					mc02[ 0x17 ]	= descriptor[ 7 ];
					mc02[ 0x18 ]	= descriptor[ 8 ];
					mc02[ 0x19 ]	= descriptor[ 9 ];
					mc02[ 0x1A ]	= descriptor[ 10 ];
					mc02[ 0x1B ]	= descriptor[ 11 ];
				}

				fs.Position	= 0;
				fs.Write( mc02, 0, mc02.Length );
				fs.Close( );

				package.Dispose( );
			}
			#endregion
		}
		public void SaveAs( string newfile )
		{
			File.Copy( fileName, newfile + ".DB" );
			File.Copy( realfileName, newfile );

			fileName		= newfile + ".DB";
			realfileName	= newfile;

			Save( );
		}

		public enum MaddenFileType
		{	FileType_None,
			FileType_DB,
			FileType_MC02,
			FileType_CON
		}
		public static MaddenFileType CheckFileType( FileStream fs )
		{	byte[]	b	= new byte[4];

			fs.Position	= 0;
			fs.Read( b, 0, 4 );
			fs.Position	= 0;

			if( b[0] == 'D' && b[1] == 'B' )
				return MaddenFileType.FileType_DB;
			if( b[0] == 'M' && b[1] == 'C' && b[2] == '0' && b[3] == '2' )
				return MaddenFileType.FileType_MC02;
			if( b[0] == 'C' && b[1] == 'O' && b[2] == 'N' )
				return MaddenFileType.FileType_CON;
			return MaddenFileType.FileType_None;
		}
		/// <summary>
		/// create a new, blank, Madden 12 roster
		/// all of this is subject to change for future versions of Madden!
		/// </summary>
		public static MaddenDatabase CreateMaddeDB_Roster( )
		{	MaddenDatabase	md	= new MaddenDatabase( );

			// reserve our file space
			md.dbFileInfo.theFile		= new byte[ 0x000b15b4 ];

			// file in the typical DB header values for a roster ( as of Madden 12 )
			md.dbFileInfo.header		= 0x4442;
			md.dbFileInfo.version		= 0x0008;
			md.dbFileInfo.unknown_1		= 0x01000000;
			md.dbFileInfo.DBsize		= 0x000b15b4;
			md.dbFileInfo.zero			= 0x00000000;
			md.dbFileInfo.tableCount	= 0x00000004;
			md.dbFileInfo.unknown_2		= 0xe0dc10f5;
			md.dbFileInfo.DBHeaderToBuffer( );

			md.dbFileInfo.startData		= 56;

			// build our default table header
			DBFileInfo.WriteDW2Buf( md.dbFileInfo.theFile, 24, 0x54484344 );
			DBFileInfo.WriteDW2Buf( md.dbFileInfo.theFile, 28, 0x00000000 );
			DBFileInfo.WriteDW2Buf( md.dbFileInfo.theFile, 32, 0x594a4e49 );
			DBFileInfo.WriteDW2Buf( md.dbFileInfo.theFile, 36, 0x00005b68 );
			DBFileInfo.WriteDW2Buf( md.dbFileInfo.theFile, 40, 0x59414c50 );
			DBFileInfo.WriteDW2Buf( md.dbFileInfo.theFile, 44, 0x00006610 );
			DBFileInfo.WriteDW2Buf( md.dbFileInfo.theFile, 48, 0x4d414554 );
			DBFileInfo.WriteDW2Buf( md.dbFileInfo.theFile, 52, 0x000af730 );
			DBFileInfo.WriteDW2Buf( md.dbFileInfo.theFile, 56, 0x9dcf334f );

			return md;
		}
	}

	public class View
	{
		public delegate void ViewChanged( View v );
		public delegate Field GetMappedField( string name );
		#region members
		public string					Name			= "";
		public string					Type			= "";
		public Control					DisplayControl	= null;
		public int						Position_x		= 0;
		public int						Position_y		= 0;
		public int						Position_z		= 0;
		public int						Size_width		= 0;
		public int						Size_height		= 0;
		public int						ChildCount		= 0;
		public List<string>				ChildViews		= new List<string>( );
		public List<View>				lChildren		= new List<View>( );
		public string					SourceType		= "";
		public string					SourceName		= "";
		public int						ChildFieldCount	= 0;
		public List<string>				ChildFields		= new List<string>( );
		public List<Field>				lChildFields	= new List<Field>( );
		public List<FieldFilter>		lastFilters		= null;
		public ToolTip					toolTip			= new ToolTip( );
		static public ViewChanged		viewChanged		= null;
		static public GetMappedField	getMappedField	= null;
		#endregion

		public View( )
		{
		}
		public override string ToString()
		{
			return Name;
		}
		public bool ProcessSettings( List<Field> lMappedFields )
		{
			switch( Type )
			{
				#region grid list view
				case "Grid":
					// setup list view
					ListViewEx.ListViewEx	lv	= new ListViewEx.ListViewEx( );
					lv.FullRowSelect			= true;
					lv.DoubleClickActivation	= true;
					lv.Height					= Size_height;
					lv.Width					= Size_width;
					lv.Location					= new Point( Position_x, Position_y );
					lv.Anchor					= AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
					lv.ListClicked				+= new SubItemEventHandler( GridClick );
					lv.SubItemClicked			+= new SubItemEventHandler( GridItemClick );
					lv.SubItemEndEditing		+= new SubItemEndEditingEventHandler( GridItemEdited );
					lv.ColumnClick				+= new ColumnClickEventHandler( GridColumnClicked );
//					lv.SelectedIndexChanged		+= new EventHandler(lv_SelectedIndexChanged);

					// add the columns
					lv.Columns.Add( "" );
					for( int i=0; i < ChildFields.Count; i++ )
					{
						Field	f	= Field.FindField( lMappedFields, ChildFields[i] );
						if( f == null )
						{	MessageBox.Show( "Field " + ChildFields[i] + " in view " + Name + " not in main field list; aborting" );
							return false;
						}
						lChildFields.Add( f );

						string	field	= ( f.Name != "" ) ? f.Name : f.Abbreviation;

						ColumnHeader	ch	= new ColumnHeader( );
						ch.Text				= field;
						ch.Tag				= f;

						lv.Columns.Add( ch );
					}
					lv.AutoResizeColumns( ColumnHeaderAutoResizeStyle.HeaderSize );

					// save the control
					DisplayControl				= lv;
					DisplayControl.Tag			= this;
					break;
				#endregion

				#region list item list view
				case "List Item":
					// setup list view ( this is a name / value type view )
					ListViewEx.ListViewEx	lv2	= new ListViewEx.ListViewEx( );
					lv2.FullRowSelect			= true;
					lv2.DoubleClickActivation	= true;
					lv2.Height					= Size_height;
					lv2.Width					= Size_width;
					lv2.Location				= new Point( Position_x, Position_y );

					// add two columns
					lv2.Columns.Add( "Field" );
					lv2.Columns.Add( "Value" );
					lv2.AutoResizeColumns( ColumnHeaderAutoResizeStyle.HeaderSize );

					// save the control
					DisplayControl				= lv2;
					DisplayControl.Tag			= this;
					break;
				#endregion

				#region tab view
				case "Tab":	// to do
					TabControl	tab				= new TabControl( );
					tab.Height					= Size_height;
					tab.Width					= Size_width;
					tab.Location				= new Point( Position_x, Position_y );
					tab.Anchor					= AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
					tab.Selecting				+= new TabControlCancelEventHandler( TabSelecting );

					DisplayControl				= tab;
					DisplayControl.Tag			= this;
					break;
				#endregion
			}
			return true;
		}

		public void UpdateGridData( MaddenTable maddenTable, List<FieldFilter> lFilters=null )
		{	int	i, j, x;

			Cursor.Current	= Cursors.WaitCursor;
			((ListView) DisplayControl).BeginUpdate( );

			lastFilters	= lFilters;
			#region columns specified
			if( ChildFields.Count > 0 )
			{
				((ListView) DisplayControl).Items.Clear( );

				for( i=0; i < maddenTable.lRecords.Count; i++ )
				{
					if( ! FieldFilter.ProcessFilters( lFilters, lChildFields, maddenTable.lRecords[ i ] ) )
						continue;

					ListViewItem lvitems = new ListViewItem( ( i+1 ).ToString( ) );

					for( j=0; j < ChildFields.Count; j++ )
					{
						Field							f	= Field.FindField( lChildFields, ChildFields[ j ] );
						ListViewItem.ListViewSubItem	sub	= null;

						switch( f.ControlType )
						{
							case "TextBox":
								sub	= new ListViewItem.ListViewSubItem( lvitems, maddenTable.lRecords[ i ][ f.Abbreviation ] );
								break;

							case "ComboBox":

								if( f.ControlLink != "" )
								{
									#region select the item via lookup
									#region find the ojbect in the list
									if( f.KeyToIndexMappings.ContainsKey( maddenTable.lRecords[ i ][ f.Abbreviation ] ) )
									{	((ComboBox) f.EditControl).SelectedIndex	= f.KeyToIndexMappings[ maddenTable.lRecords[ i ][ f.Abbreviation ] ];
										sub	= new ListViewItem.ListViewSubItem( lvitems, ((ComboBox) f.EditControl).SelectedItem.ToString( ) );
									}

									//for( x=0; x < ((ComboBox) f.EditControl).Items.Count; x++ )
									//{
									//    RefObj	ro	= (RefObj) ((ComboBox) f.EditControl).Items[ x ];

									//    if( ro.key == maddenTable.lRecords[ i ][ f.Abbreviation ] )
									//    {	((ComboBox) f.EditControl).SelectedIndex	= x;
									//        break;
									//    }
									//}
									#endregion
									//if( ((ComboBox) f.EditControl).SelectedIndex > -1 )
									//    sub	= new ListViewItem.ListViewSubItem( lvitems, ((ComboBox) f.EditControl).SelectedItem.ToString( ) );
									else
									{
										RefObj	ro	= new RefObj( maddenTable.lRecords[ i ][ f.Abbreviation ], maddenTable.lRecords[ i ][ f.Abbreviation ] );
										((ComboBox) f.EditControl).Items.Add( ro );
										f.KeyToIndexMappings.Add( maddenTable.lRecords[ i ][ f.Abbreviation ], ((ComboBox) f.EditControl).Items.Count -1 );
										((ComboBox) f.EditControl).SelectedIndex	= ((ComboBox) f.EditControl).Items.Count -1;
										sub	= new ListViewItem.ListViewSubItem( lvitems, ro.value );
									}
									#endregion

								}	else
								{
									#region select by index
									if( ((ComboBox) f.EditControl).Items.Count < Convert.ToInt32( maddenTable.lRecords[ i ][ f.Abbreviation ] ) )
										sub	= new ListViewItem.ListViewSubItem( lvitems, maddenTable.lRecords[ i ][ f.Abbreviation ] );
									else
									{
										((ComboBox) f.EditControl).SelectedIndex	= Convert.ToInt32( maddenTable.lRecords[ i ][ f.Abbreviation ] );
										sub	= new ListViewItem.ListViewSubItem( lvitems, ((ComboBox) f.EditControl).SelectedItem.ToString( ) );
									}
									#endregion
								}
								break;

							case "Calculated":
								sub	= new ListViewItem.ListViewSubItem( lvitems, f.RunFormula( lChildFields, maddenTable.lRecords[ i ] ).ToString( ) );
								break;

							case "AdjustedComboBox":
								((ComboBox) f.EditControl).SelectedIndex	= Convert.ToInt32( maddenTable.lRecords[ i ][ f.Abbreviation ] ) + f.Offset;
								sub	= new ListViewItem.ListViewSubItem( lvitems, ((ComboBox) f.EditControl).SelectedItem.ToString( ) );
								break;

							case "MappedComboBox":
								Field	f2	= ( getMappedField != null ) ? getMappedField( f.ControlIF ) : null;
								if( f2 == null )
								{	MessageBox.Show( "Could not find mapped field therefore cannot edit value!" );
									break;
								}
								((ComboBox) f.EditControl).SelectedIndex	= Convert.ToInt32( maddenTable.lRecords[ i ][ f2.Abbreviation ] );
								sub	= new ListViewItem.ListViewSubItem( lvitems, ((ComboBox) f.EditControl).SelectedItem.ToString( ) );
								break;

							case "TimeOfDayInMinutes":
								TimeSpan	span	= TimeSpan.FromMinutes( Convert.ToInt32( maddenTable.lRecords[ i ][ f.Abbreviation ] ) );
								DateTime	time	= new DateTime( 2012, 1, 1 );
								time				= time + span;
								sub					= new ListViewItem.ListViewSubItem( lvitems, time.ToString( "t" ) );
								break;

							default:
								sub	= new ListViewItem.ListViewSubItem( lvitems, maddenTable.lRecords[ i ][ f.Abbreviation ] );
								break;
						}
						sub.Tag								= f;
						lvitems.SubItems.Add( sub );
					}

					lvitems.UseItemStyleForSubItems	= true;
					lvitems.Tag						= maddenTable.lRecords[ i ];

					((ListView) DisplayControl).Items.Add( lvitems );

				}
			}
			#endregion
			#region columns not specified
			else
			{
				((ListView) DisplayControl).Clear( );

				for( i=0; i < maddenTable.lFields.Count; i++ )
				{
					ColumnHeader	ch	= new ColumnHeader( );
					ch.Text				= maddenTable.lFields[ i ].name;
					ch.Tag				= maddenTable.lFields[ i ];

					((ListView) DisplayControl).Columns.Add( ch );
				}

				for( i=0; i < maddenTable.lRecords.Count; i++ )
				{
					if( ! FieldFilter.ProcessFilters( lFilters, maddenTable.lFields, maddenTable.lRecords[ i ] ) )
						continue;

					ListViewItem lvitems = new ListViewItem( ( i+1 ).ToString( ) );

					for( j=0; j < maddenTable.lFields.Count; j++ )
					{
						ListViewItem.ListViewSubItem	sub	= new ListViewItem.ListViewSubItem( lvitems, maddenTable.lRecords[ i ][ maddenTable.lFields[ j ].name ] );
						sub.Tag								= maddenTable.lFields[ j ];
						lvitems.SubItems.Add( sub );
					}

					lvitems.UseItemStyleForSubItems	= true;
					lvitems.Tag						= maddenTable.lRecords[ i ];

					((ListView) DisplayControl).Items.Add( lvitems );

				}
			}
			#endregion

			((ListView) DisplayControl).AutoResizeColumns( ColumnHeaderAutoResizeStyle.HeaderSize );
			((ListView) DisplayControl).EndUpdate( );
			Cursor.Current	= Cursors.Default;
		}
		public void RefreshGridData( MaddenTable maddenTable )
		{
			UpdateGridData( maddenTable, lastFilters );
		}
		public void GridClick( object obj, SubItemEventArgs args )
		{
			Field	f	= (Field) args.Item.SubItems[ args.SubItem ].Tag;
			if( f != null && f.EditControl != null && ! f.ControlLocked )
			{
				if( (args.Button & MouseButtons.Right) != 0 )
				{
					Point	point	= DisplayControl.PointToClient( Cursor.Position );
					if( f.Description != "" )
						toolTip.Show( f.Description, DisplayControl, point.X, point.Y, 5000 );
				}
			}
		}
		public void GridItemClick( object obj, SubItemEventArgs args )
		{
			Field	f	= (Field) args.Item.SubItems[ args.SubItem ].Tag;
			if( f != null && f.EditControl != null && ! f.ControlLocked && f.ControlType != "Calculated" )
			{
				f.EditControl.Parent	= DisplayControl;
				((ListViewEx.ListViewEx) DisplayControl).StartEditing( f.EditControl, args.Item, args.SubItem );
			}
		}
		public void GridItemEdited( object obj, SubItemEventArgs args )
		{
			Field	f	= Field.FindField( lChildFields, ((ListView) DisplayControl).Columns[ args.SubItem ].Text );
			if( f != null && f.EditControl != null )
			{
				MaddenRecord	mr	= (MaddenRecord) args.Item.Tag;
				if( mr != null )
				{
					switch( f.ControlType )
					{
						case "TextBox":
							mr[ f.Abbreviation ]	= f.EditControl.Text;
							break;

						case "ComboBox":
							if( f.ControlLink != "" )
							{	// set via lookup
								if( ((ComboBox) f.EditControl).SelectedIndex >-1 )
								{
									RefObj	ro				= (RefObj) ((ComboBox) f.EditControl).SelectedItem;
									mr[ f.Abbreviation ]	= ro.key;
								}

							}	else
							{	// set by using the index
								if( ((ComboBox) f.EditControl).SelectedIndex >-1 )
									mr[ f.Abbreviation ]	= ((ComboBox) f.EditControl).SelectedIndex.ToString( );
							}

							break;

						case "AdjustedComboBox":
							if( ((ComboBox) f.EditControl).SelectedIndex >-1 )
								mr[ f.Abbreviation ]	= ( ((ComboBox) f.EditControl).SelectedIndex  - f.Offset ).ToString( );
							break;

						case "MappedComboBox":
								Field	f2	= ( getMappedField != null ) ? getMappedField( f.ControlIF ) : null;
								if( f2 == null )
								{	MessageBox.Show( "Could not find mapped field therefore cannot save value!" );
									break;
								}
							if( ((ComboBox) f.EditControl).SelectedIndex >-1 )
								mr[ f2.Abbreviation ]	= ( ((ComboBox) f.EditControl).SelectedIndex  - f.Offset ).ToString( );
							break;

						case "TimeOfDayInMinutes":
							string[]	stime			= f.EditControl.Text.Split( new char[]{ ' ' }, StringSplitOptions.RemoveEmptyEntries );
							if( stime.Length != 2 )
							{	MessageBox.Show( "Time format incorrect! Try hh:mm AM/PM" );
								break;
							}

							TimeSpan	span;
							if( TimeSpan.TryParse( stime[0], out span ) )
							{	if( stime[1].ToUpper( ) == "PM" )
									span	= span.Add( new TimeSpan( 12, 0, 0 ) );
								mr[ f.Abbreviation ]	= span.TotalMinutes.ToString( );
							}
							else
								MessageBox.Show( "Time format incorrect! Try hh:mm AM/PM" );

							break;

						default:
							mr[ f.Abbreviation ]	= f.EditControl.Text;
							break;
					}

					// perform recalcs
					for( int i=0; i < lChildFields.Count; i++ )
					{	if( lChildFields[ i ].ControlType == "Calculated" )
						{	ColumnHeader	ch	= ((ListView) DisplayControl).Columns[ i +1 ];

							if( ch != null )
							{	args.Item.SubItems[ ch.Index ].Text	= lChildFields[ i ].RunFormula( lChildFields, mr ).ToString( );
							}
						}
					}

				}
			}
		}
		public void GridColumnClicked( object sender, ColumnClickEventArgs e )
		{
			Field	cf	= (Field) ((ListView) DisplayControl).Columns[ e.Column ].Tag;
	
			((ListView) DisplayControl).ListViewItemSorter		= new ListViewItemComparer( e.Column, cf );
			((ListView) DisplayControl).Sort( );

			ListViewItemComparer.sortDir	= -ListViewItemComparer.sortDir;
		}
		public void TabSelecting(object sender, TabControlCancelEventArgs e)
		{
 			if( viewChanged != null && e.TabPage != null )
				viewChanged( (View) e.TabPage.Tag );
		}

		static public View FindView( List<View> lMappedViews, string Name )
		{	foreach( View v in lMappedViews )
			{	if( v.Name == Name )
					return v;
			}
			return null;
		}
		static public bool ProcessAllViewSettings( List<View> lMappedViews, List<Field> lMappedFields )
		{	foreach( View v in lMappedViews )
			{	if( ! v.ProcessSettings( lMappedFields ) )
					return false;
			}
			return true;
		}
		static public bool SetViewChildren( List<View> lMappedViews, Form MainForm )
		{
			// set all to be children of the main form first by default
			foreach( View v in lMappedViews )
				MainForm.Controls.Add( v.DisplayControl );

			foreach( View v in lMappedViews )
			{
				foreach( string s in v.ChildViews )
				{
					View	temp	= View.FindView( lMappedViews, s );
					if( temp != null )
					{	// first remove from the main form
						MainForm.Controls.Remove( temp.DisplayControl );

						// add to child list and control's children
						v.lChildren.Add( temp );

						if( v.Type != "Tab" )
						{
							v.DisplayControl.Controls.Add( temp.DisplayControl );
						}
						else
						{
							TabPage	page	= new TabPage( temp.Name );
							page.Controls.Add( temp.DisplayControl );
							page.Tag		= temp;
							((TabControl) v.DisplayControl).Controls.Add( page );
						}
					}	else
					{
						MessageBox.Show( "Child view " + s + " not found for view " + v.Name, "Error in config" );
						return false;
					}
				}
			}

			TabControl	tab	= null;
			foreach( Control c in MainForm.Controls )
			{	if( c.GetType( ).Name == "TabControl" )
				{	tab	= (TabControl) c;
					break;
				}
			}
			tab.SelectedIndex	= -1;
			tab.SelectedIndex	= 0;

			return true;
		}
	}
	class ListViewItemComparer : IComparer
	{
		private int			col;
		public static int	sortDir		= 1;
		private	Field		field		= null;

		public ListViewItemComparer( int column, Field f )
		{
			col		= column;
			field	= f;
		}
		public int Compare( object x, object y ) 
		{
			if( col == 0 )
				return sortDir * Convert.ToInt32( ((ListViewItem)x).SubItems[col].Text ).CompareTo( Convert.ToInt32( ((ListViewItem)y).SubItems[col].Text ) );

			//if( field.ControlType == "AdjustedComboBox" || field.ControlType == "TimeOfDayInMinutes" )
			//    return sortDir * field.CompareDataAsType( ((MaddenRecord) ((ListViewItem)x).Tag)[ field.name ], ((MaddenRecord) ((ListViewItem)y).Tag)[ field.name ] );

			//if( field.ControlLink == "" )
			//    return sortDir * field.CompareDataAsType( ((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text );

			return sortDir * field.CompareDataAsType( ((MaddenRecord) ((ListViewItem)x).Tag)[ field.name ], ((MaddenRecord) ((ListViewItem)y).Tag)[ field.name ] );
		}
	}
	public class RefObj
	{
		public string	key		= "";
		public string	value	= "";

		public RefObj( )
		{
		}
		public RefObj( string k, string v )
		{	key		= k;
			value	= v;
		}
		public override string ToString()
		{
			return value;
		}
	}
	public class FieldFilter
	{
		public enum Operation
		{
			None,
			Equal,
			NotEqual,
			GreaterThan,
			LessThan,
			Contains,
			DoesNotContain,
			EndsWith,
			StartsWith,
			Set,
			Adjust
		}

		public string		field	= "";
		public string		value	= "";
		public Operation	op		= Operation.None;

		public FieldFilter( )
		{
		}
		public FieldFilter( string f, string operation, string v )
		{	Create( f, operation, v );
		}
        public string OperationToText ()
        {
            string s = this.op.ToString().ToLower();
            switch ( s )
            {
                case "equal": return "=";
                case "notequal": return "!=";
                case "greaterthan": return ">";
                case "lessthan": return "<";
                case "doesnotcontain": return "!contains";
                case "set": return "<-";
                case "adjust": return "+/-";
                default: return s;
            }
        }
		public void Create( string f, string operation, string v )
		{
			field	= f;
			value	= v;

			switch( operation.ToLower( ) )
			{
				case "=":			op	= Operation.Equal;
					break;
				case "!=":			op	= Operation.NotEqual;
					break;
				case ">":			op	= Operation.GreaterThan;
					break;
				case "<":			op	= Operation.LessThan;
					break;
				case "contains":	op	= Operation.Contains;
					break;
				case "!contains":	op	= Operation.DoesNotContain;
					break;
				case "endswith":	op	= Operation.EndsWith;
					break;
				case "startswith":	op	= Operation.StartsWith;
					break;
				// mass operations
				case "<-":			op	= Operation.Set;
					break;
				case "+/-":			op	= Operation.Adjust;
					break;
			}
		}
		public bool Process( List<Field> lMF, MaddenRecord mr )
		{
			Field	f		= Field.FindField( lMF, field );
			string	code	= f.Abbreviation != "" ? f.Abbreviation : f.name;

			switch( op )
			{
				case Operation.Contains:
				case Operation.DoesNotContain:
				case Operation.EndsWith:
				case Operation.StartsWith:

					#region is a combox item
					if( f.ControlType.EndsWith( "ComboBox" ) )
					{	string	testData	= "";

						// need to find item based on type of combobox
						#region regular combobox with an item list
						if( f.ControlType == "ComboBox" && f.ControlLink == "" )
						{	testData	= ((ComboBox) f.EditControl ).Items[ Convert.ToInt32( mr[ code ] ) ].ToString( );
						}
						#endregion
						#region linked combobox
						if( f.ControlType == "ComboBox" && f.ControlLink != "" )
						{
							RefObj	linkedObj	= null;
							#region find the ojbect in the list
							for( int x=0; x < ((ComboBox) f.EditControl).Items.Count; x++ )
							{
								RefObj	ro	= (RefObj) ((ComboBox) f.EditControl).Items[ x ];
								if( ro.key == mr[ f.Abbreviation ] )
								{	linkedObj	= ro;
									break;
								}
							}
							#endregion
							testData	= ( linkedObj != null ) ? linkedObj.value : "";
						}
						#endregion
						#region adjusted combobox with an item list
						if( f.ControlType == "AdjustedComboBox" )
						{	testData	= ((ComboBox) f.EditControl ).Items[ Convert.ToInt32( mr[ code ] ) + f.Offset ].ToString( );
						}
						#endregion
						#region mapped combobox with an item list
						if( f.ControlType == "MappedComboBox" )
							testData	= ((ComboBox) f.EditControl ).Items[ Convert.ToInt32( mr[ f.ControlIF ] ) ].ToString( );
						#endregion

						switch( op )
						{
							case Operation.Contains:
								return ( testData.IndexOf( value ) == -1 ) ? false : true;

							case Operation.DoesNotContain:
								return ( testData.IndexOf( value ) > -1 ) ? false : true;

							case Operation.EndsWith:
								return testData.EndsWith( value );

							case Operation.StartsWith:
								return testData.StartsWith( value );
						}
					}
					#endregion

					#region not a combobox variant
					switch( op )
					{
						case Operation.Contains:
							return ( mr[ code ].IndexOf( value ) == -1 ) ? false : true;

						case Operation.DoesNotContain:
							return ( mr[ code ].IndexOf( value ) > -1 ) ? false : true;

						case Operation.EndsWith:
							return mr[ code ].EndsWith( value );

						case Operation.StartsWith:
							return mr[ code ].StartsWith( value );
					}
					#endregion
					return false;	// should never get here


				case Operation.Equal:
					return ( f.CompareDataAsType( mr[ code ], value ) == 0 ) ? true : false;

				case Operation.NotEqual:
					return ( f.CompareDataAsType( mr[ code ], value ) != 0 ) ? true : false;

				case Operation.GreaterThan:
					return ( f.CompareDataAsType( mr[ code ], value ) == 1 ) ? true : false;

				case Operation.LessThan:
					return ( f.CompareDataAsType( mr[ code ], value ) == -1 ) ? true : false;

				// mass operations
				case Operation.Set:
					mr[ code ]	= value.ToString( );
					return true;

				case Operation.Adjust:
					mr[ code ]	= ( Convert.ToInt32( mr[ code ] ) + Convert.ToInt32( value.ToString( ) ) ).ToString( );
					return true;
			}
			return false;
		}

		static public bool ProcessFilters( List<FieldFilter> lFF, List<Field> lMF, MaddenRecord mr )
		{	bool	ret	= true;

			if( lFF != null && lMF != null && mr != null )
			{
				foreach( FieldFilter ff in lFF )
					ret	&= ff.Process( lMF, mr );
			}

			return ret;
		}
	}

	// xml config info used in copying known fields / tables
	public class XMLConfig
	{
		#region fields
		public string		StartLabel					= "";
		public string		Name						= "";
		public string		Abbreviation				= "";
		public string		Type						= "";
		public string		Description					= "";
		public string		ControlType					= "";
		public bool			ControlLink					= false;
		public string		LinkTable					= "";
		public string		IndexField					= "";
		public string		ReferenceField				= "";
		public string		ReferenceField2				= "";
		public List<string>	ControlItems				= new List<string>( );
		public string		PosX						= "";
		public string		PosY						= "";
		public string		PosZ						= "";
		public string		SizeW						= "";
		public string		SizeH						= "";
		public string		SrcType						= "";
		public string		SrcName						= "";
		public List<string>	Children					= new List<string>( );
		public List<string>	ChildFields					= new List<string>( );
		public string		Min							= "";
		public string		Max							= "";
		public List<Field.Formula>	Formulas			= new List<Field.Formula>( );
		public string		Offset						= "";
		#endregion

		public XMLConfig( ){}
		public override string ToString()
		{	string	data	= "";

			#region view
			if( StartLabel == "View" )
			{
				data	+= "<View>\r\n";
				data	+= "\t<Name>" + Name + "</Name>\r\n";
				data	+= "\t<Type>" + Type + "</Type>\r\n";
				data	+= "\t<Position>\r\n";
				data	+= "\t\t<X>" + PosX + "</X>\r\n";
				data	+= "\t\t<Y>" + PosY + "</Y>\r\n";
				data	+= "\t\t<Z>" + PosZ + "</Z>\r\n";
				data	+= "\t</Position>\r\n";
				data	+= "\t<Size>\r\n";
				data	+= "\t\t<Width>"  + SizeW + "</Width>\r\n";
				data	+= "\t\t<Height>" + SizeH + "</Height>\r\n";
				data	+= "\t</Size>\r\n";

				if( SrcName != "" && SrcType != "" )
				{
					data	+= "\t<Source>\r\n";
					data	+= "\t\t<Type>" + SrcType + "</Type>\r\n";
					data	+= "\t\t<Name>" + SrcName + "</Name>\r\n";
					data	+= "\t</Source>\r\n";
				}

				if( Children.Count >0 )
				{
					foreach( string s in Children )
						data	+= "\t<Child>" + s + "</Child>\r\n";
				}

				if( ChildFields.Count >0 )
				{
					foreach( string s in ChildFields )
						data	+= "\t<Field>" + s + "</Field>\r\n";
				}

				data	+= "</View>\r\n\r\n\r\n";
			}
			#endregion
			#region table
			if( StartLabel == "Table" )
			{
				data	+= "<Table>\r\n";
				data	+= "\t<Abbreviation>" + Abbreviation + "</Abbreviation>\r\n";
				data	+= "\t<Name>" + Name + "</Name>\r\n";
				data	+= "</Table>\r\n\r\n";
			}
			#endregion
			#region field
			if( StartLabel == "Field" )
			{
				data	+= "<Field>\r\n";
				data	+= "\t<Abbreviation>" + Abbreviation + "</Abbreviation>\r\n";
				data	+= "\t<Name>" + Name + "</Name>\r\n";
				data	+= "\t<ControlType>" + ControlType + "</ControlType>\r\n";

				if( ControlItems.Count >0 )
				{
					foreach( string s in ControlItems )
						data	+= "\t<ControlItem>" + s + "</ControlItem>\r\n";
				}

				if( ControlLink )
				{
					data	+= "\t<ControlLink>\r\n";
					data	+= "\t\t<Table>" + LinkTable + "</Table>\r\n";
					data	+= "\t\t<IndexField>" + IndexField + "</IndexField>\r\n";

					if( ReferenceField != "" )
					data	+= "\t\t<ReferenceField>" + ReferenceField + "</ReferenceField>\r\n";
					if( ReferenceField2 != "" )
						data	+= "\t\t<ReferenceField2>" + ReferenceField2 + "</ReferenceField2>\r\n";

					if( Formulas.Count >0 )
					{
						data	+= "\t\t<Formulas>\r\n";
						foreach( Field.Formula form in Formulas )
						{
							data	+= "\t\t\t<Formula>\r\n";
							data	+= "\t\t\t\t<IndexValue>" + form.IndexValue + "</IndexValue>\r\n";
							if( form.Variables.Count >0 )
							{
								data	+= "\t\t\t\t<Variables>\r\n";
								foreach( Field.Variable var in form.Variables )
								{
									data	+= "\t\t\t\t\t<Variable>\r\n";
									data	+= "\t\t\t\t\t\t<Field>" + var.vField + "</Field>\r\n";
									data	+= "\t\t\t\t\t\t<Multiplier>" + var.Multiplier.ToString( ) + "</Multiplier>\r\n";
									data	+= "\t\t\t\t\t</Variable>\r\n";
								}
								data	+= "\t\t\t\t</Variables>\r\n";
							}
							data	+= "\t\t\t\t<Adjustment>" + form.Adjustment.ToString( ) + "</Adjustment>\r\n";
							data	+= "\t\t\t</Formula>\r\n";
						}
						data	+= "\t\t</Formulas>\r\n";
					}

					if( Min != "" )
						data	+= "\t\t<Min>" + Min + "</Min>\r\n";
					if( Max != "" )
						data	+= "\t\t<Max>" + Max + "</Max>\r\n";
					data	+= "\t</ControlLink>\r\n";
				}

				if( Offset != "" )
				data	+= "\t<Offset>" + Offset + "</Offset>\r\n";
				data	+= "\t<Description>" + Description + "</Description>\r\n";
				data	+= "\t<Type>" + Type + "</Type>\r\n";
				data	+= "</Field>\r\n";
			}
			#endregion

			return data;
		}
		public void Copy( XMLConfig org )
		{
			StartLabel					= org.StartLabel;
			Name						= org.Name;
			Abbreviation				= org.Abbreviation;
			Type						= org.Type;
			Description					= org.Description;
			ControlType					= org.ControlType;
			ControlLink					= org.ControlLink;
			LinkTable					= org.LinkTable;
			IndexField					= org.IndexField;
			ReferenceField				= org.ReferenceField;
			ReferenceField2				= org.ReferenceField2;
			ControlItems				= new List<string>( org.ControlItems );
			PosX						= org.PosX;
			PosY						= org.PosY;
			PosZ						= org.PosZ;
			SizeW						= org.SizeW;
			SizeH						= org.SizeH;
			SrcType						= org.SrcType;
			SrcName						= org.SrcName;
			Children					= new List<string>( org.Children );
			ChildFields					= new List<string>( org.ChildFields );
			Min							= org.Min;
			Max							= org.Max;
			Formulas					= new List<Field.Formula>( org.Formulas );
		}

		static public void ReadXMLConfig( string configfile, List<XMLConfig> views, List<XMLConfig> tables, List<XMLConfig> fields )
		{
			XMLConfig			xml				= null;
			string				Path			= "\\";


			XmlTextReader	reader	= new XmlTextReader( configfile );
			while( reader.Read( ) )
			{
				switch( reader.NodeType )
				{
					case XmlNodeType.Element:
						#region map open elements
						if( Path == "\\xml\\" && reader.Name == "Field" )
						{	xml				= new XMLConfig( );
							xml.StartLabel	= "Field";
						}

						if( Path == "\\xml\\" && reader.Name == "Table" )
						{	xml				= new XMLConfig( );
							xml.StartLabel	= "Table";
						}

						if( Path == "\\xml\\" && (reader.Name == "View" || reader.Name == "Main") )
						{	xml				= new XMLConfig( );
							xml.StartLabel	= "View";
						}

						if( reader.Name == "Formulas" )
						{	xml.Formulas	= Field.Formula.ReadFormulas( reader, Path + "Formulas\\" );
							break;
						}
						#endregion

						Path	+= reader.Name + "\\";
						break;

					case XmlNodeType.Text:

						#region map main entries	
						if( Path.EndsWith( "Main\\Size\\Width\\" ) )
							xml.SizeW	= reader.Value;

						if( Path.EndsWith( "Main\\Size\\Height\\" ) )
							xml.SizeH	= reader.Value;
						#endregion

						#region map field entries
						if( Path.EndsWith( "Field\\Abbreviation\\" ) )
							xml.Abbreviation	= reader.Value;

						if( Path.EndsWith( "Field\\Name\\" ) )
							xml.Name			= reader.Value;

						if( Path.EndsWith( "Field\\ControlType\\" ) )
							xml.ControlType		= reader.Value;

						if( Path.EndsWith( "Field\\ControlItem\\" ) )
							xml.ControlItems.Add( reader.Value );

						if( Path.EndsWith( "Field\\ControlLink\\Table\\" ) )
						{	xml.ControlLink		= true;
							xml.LinkTable		= reader.Value;
						}

						if( Path.EndsWith( "Field\\ControlLink\\IndexField\\" ) )
						{	xml.ControlLink		= true;
							xml.IndexField		= reader.Value;
						}

						if( Path.EndsWith( "Field\\ControlLink\\ReferenceField\\" ) )
						{	xml.ControlLink		= true;
							xml.ReferenceField	= reader.Value;
						}

						if( Path.EndsWith( "Field\\ControlLink\\ReferenceField2\\" ) )
						{	xml.ControlLink		= true;
							xml.ReferenceField2	= reader.Value;
						}

						if( Path.EndsWith( "Field\\ControlLink\\Formulas\\" ) )
						{	xml.ControlLink		= true;
							xml.Formulas		= Field.Formula.ReadFormulas( reader, Path );
						}

						if( Path.EndsWith( "Field\\ControlLink\\Min\\" ) )
						{	xml.ControlLink		= true;
							xml.Min				= reader.Value;
						}

						if( Path.EndsWith( "Field\\ControlLink\\Max\\" ) )
						{	xml.ControlLink		= true;
							xml.Max				= reader.Value;
						}

						if( Path.EndsWith( "Field\\Offset\\" ) )
							xml.Offset			= reader.Value;

						if( Path.EndsWith( "Field\\Description\\" ) )
							xml.Description		= reader.Value;

						if( Path.EndsWith( "Field\\Type\\" ) )
							xml.Type			= reader.Value;
						#endregion

						#region map table entries	
						if( Path.EndsWith( "Table\\Abbreviation\\" ) )
							xml.Abbreviation	= reader.Value;

						if( Path.EndsWith( "Table\\Name\\" ) )
							xml.Name			= reader.Value;
						#endregion

						#region map view entries
						if( Path.EndsWith( "View\\Name\\" ) )
							xml.Name			= reader.Value;

						if( Path.EndsWith( "View\\Type\\" ) )
							xml.Type			= reader.Value;

						if( Path.EndsWith( "View\\Source\\Type\\" ) )
							xml.SrcType			= reader.Value;

						if( Path.EndsWith( "View\\Source\\Name\\" ) )
							xml.SrcName			= reader.Value;

						if( Path.EndsWith( "View\\Position\\X\\" ) )
							xml.PosX			= reader.Value;

						if( Path.EndsWith( "View\\Position\\Y\\" ) )
							xml.PosY			= reader.Value;

						if( Path.EndsWith( "View\\Position\\Z\\" ) )
							xml.PosZ			= reader.Value;

						if( Path.EndsWith( "View\\Size\\Width\\" ) )
							xml.SizeW			= reader.Value;

						if( Path.EndsWith( "View\\Size\\Height\\" ) )
							xml.SizeH			= reader.Value;

						if( Path.EndsWith( "View\\Child\\" ) )
							xml.Children.Add( reader.Value );

						if( Path.EndsWith( "View\\Field\\" ) )
							xml.ChildFields.Add( reader.Value );
						#endregion
						break;

					case XmlNodeType.EndElement:
						#region map close elements
						if( Path == "\\xml\\Field\\" && reader.Name == "Field" )
							fields.Add( xml );

						if( Path == "\\xml\\Table\\" && reader.Name == "Table" )
							tables.Add( xml );

						if( Path == "\\xml\\View\\" && reader.Name == "View" )
							views.Add( xml );
						#endregion

						try
						{	Path	= Path.Remove( Path.LastIndexOf( reader.Name + "\\" ) );
						}	catch( Exception e )
						{
							MessageBox.Show( "XML closing element not found: " + reader.Name + ", " + reader.LineNumber, "Error in XML config" );
							throw( e );
						}
						break;
				}
			}
			reader.Close( );
			return;
		}
		static public void CopyMappedValues( List<XMLConfig> from, List<XMLConfig> to )
		{
			foreach( XMLConfig xml in to )
			{
				XMLConfig	f	= from.Find( (x) => x.Abbreviation == xml.Abbreviation );
				if( f != null )
				{
					if( f.Name != "" )
						xml.Copy( f );
				}

			}
		}
		static public void UseFriendlyNames( List<XMLConfig> views, List<XMLConfig> tables, List<XMLConfig> fields )
		{
			foreach( XMLConfig v in views )
			{
				// make sure source table is using the friendly name
				XMLConfig	t	= tables.Find( (a) => a.Abbreviation == v.SrcName );
				if( t != null && t.Name != "" && t.Name != t.Abbreviation )
					v.SrcName	= t.Name;

				// now do the same for the fields
				for( int i=0; i < v.ChildFields.Count; i++ )
				{
					XMLConfig	f	= fields.Find( (a) => a.Abbreviation == v.ChildFields[ i ] );
					if( f != null && f.Name != "" && f.Name != f.Abbreviation )
						v.ChildFields[ i ]	= f.Name;
				}
			}

		}
		static public string WriteXMLConfig( List<XMLConfig> views, List<XMLConfig> tables, List<XMLConfig> fields )
		{	string	data	= "<xml>\r\n\r\n\r\n";

			foreach( XMLConfig x in views )
				data	+= x.ToString( );
			foreach( XMLConfig x in tables )
				data	+= x.ToString( );
			foreach( XMLConfig x in fields )
				data	+= x.ToString( );

			return data + "\r\n\r\n</xml>\r\n";
		}
	}
}
