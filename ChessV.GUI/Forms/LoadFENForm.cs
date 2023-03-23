
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2019 BY GREG STRONG

This file is part of ChessV.  ChessV is free software; you can redistribute
it and/or modify it under the terms of the GNU General Public License as 
published by the Free Software Foundation, either version 3 of the License, 
or (at your option) any later version.

ChessV is distributed in the hope that it will be useful, but WITHOUT ANY 
WARRANTY; without even the implied warranty of MERCHANTABILITY or 
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for 
more details; the file 'COPYING' contains the License text, but if for
some reason you need a copy, please visit <http://www.gnu.org/licenses/>.

****************************************************************************/

using System;
using System.Windows.Forms;

namespace ChessV.GUI
{
	public partial class LoadFENForm: Form
	{
		public Game Game { get; private set; }

		public LoadFENForm( Game game )
		{
			Game = game;

			InitializeComponent();
		}

		private void LoadFENForm_Load( object sender, EventArgs e )
		{
			lblGameName.Text = Game.Name;
			lblFENFormat.Text = Game.FENFormat;
			lblGameStartFEN.Text = Game.FENStart;
			currentFEN = Game.FEN.ToString();
			txtCurrentFEN.Text = currentFEN;

			PieceType[] pieceTypes;
			int nPieceTypes = Game.GetPieceTypes( out pieceTypes );
			for( int x = 0; x < nPieceTypes; x++ )
			{
				PieceType type = pieceTypes[x];
				//	determine if notation requires _ prefix
				bool requiresPrefix = false;
				if( type.Notation[0].Length == 2 &&
					type.Notation[0][0] >= 'A' && type.Notation[0][0] <= 'Z' )
				{
					//	see if the first character of this two-character notation
					//	conflicts with a single-character notation
					for( int y = 0; y < nPieceTypes; y++ )
						if( pieceTypes[y].Notation[0] == type.Notation[0][0].ToString() )
							requiresPrefix = true;
				}
				ListViewItem lvi = new ListViewItem( (requiresPrefix ? "_" : "") + type.Notation[0] );
				lvi.SubItems.Add( type.Name );
				lvi.SubItems.Add( type.InternalName );
				listPieceTypes.Items.Add( lvi );
			}
		}

		private void btnOK_Click( object sender, EventArgs e )
		{
			if( txtCurrentFEN.Text != currentFEN )
			{
				Game.ClearGameState();
				Game.LoadFEN( txtCurrentFEN.Text );
				Game.FENStart = txtCurrentFEN.Text;
			}
			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click( object sender, EventArgs e )
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private string currentFEN;
	}
}
