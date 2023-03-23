
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

namespace ChessV.Games
{
	//**********************************************************************
	//
	//                        CylindricalChess
	//
	//    This class implements the game of Cylindrical Chess.  It is like 
	//    orthodox Chess except that the board is treated as a cylindar,  
	//    with the left and right sides connected.  A piece may move off 
	//    one side and wrap arround.

	[Game("Cylindrical Chess", typeof(Geometry.Rectangular), 8, 8,
		  XBoardName = "cylinder",
		  InventedBy = "Unknown",
		  Invented = "circa 10th century",
		  Tags = "Chess Variant,Historic,Popular",
		  GameDescription1 = "Standard chess but with the modification that the edges",
		  GameDescription2 = "of the board are considered to be connected")]
	public class CylindricalChess: Chess
	{
		// *** CONSTRUCTION *** //

		public CylindricalChess()
		{
		}


		// *** INITIALIZATION *** //

		#region CreateBoard
		//	We override the CreateBoard function so the game uses a 
		//	cylindrical board.
		public override Board CreateBoard( int nPlayers, int nFiles, int nRanks, Symmetry symmetry )
		{ return new Boards.CylindricalBoard( nFiles, nRanks ); }
		#endregion

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();

			//	We need to turn on move deduplication.  Otherwise, we will 
			//	generate duplicates.  For example, a rook on an open rank can 
			//	reach squares on that rank by two different paths
			DeduplicateMoves = true;
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			base.AddPieceTypes();

			Queen.MidgameValue = Queen.EndgameValue = 1100;
			Rook.MidgameValue = Rook.EndgameValue = 500;
			Bishop.MidgameValue = Bishop.EndgameValue = 400;
			Knight.MidgameValue = Knight.EndgameValue = 325;
		}
		#endregion
	}
}
