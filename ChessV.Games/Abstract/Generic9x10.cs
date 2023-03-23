
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

namespace ChessV.Games.Abstract
{
	//**********************************************************************
	//
	//                           Generic9x10
	//
	//    The Generic game classes make it easier to specify games by 
	//    providing functionality common to chess variants.  This class 
	//    extends the Generic__x10 class by adding castling support.

	[Game("Generic 9x10", typeof(Geometry.Rectangular), 9, 10, 
		  Template = true)]
	public class Generic9x10: Generic__x10
	{
		// *** GAME VARIABLES *** //

		[GameVariable] public ChoiceVariable Castling { get; set; }


		// *** CONSTRUCTION *** //

		public Generic9x10
			( Symmetry symmetry ): 
				base
					( /* num files = */ 9, 
					  /* symmetry = */ symmetry )
		{
		}


		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Castling = new ChoiceVariable();
			Castling.AddChoice( "Standard", "King starting on the e file slides two squares either direction, " +
				"subject to the usual restrictions, to castle with the piece in the corner" );
			Castling.AddChoice( "Long", "King starting on the e file slides three squares either direction, " +
				"subject to the usual restrictions, to castle with the piece in the corner" );
			Castling.AddChoice( "Flexible", "King starting on the e file slides two or more squares, subject to the usual " +
				"restrictions, to castle with the piece in the corner" );
			Castling.AddChoice( "Close-Rook", "King starting on the e file slides two squares either direction, " +
				"subject to the usual restrictions, to castle with the piece on the b or h file" );
			Castling.AddChoice( "2R Standard", "King starting on the e file of the second rank slides two squares either direction, " +
				"subject to the usual restrictions, to castle with the piece on the edge" );
			Castling.AddChoice( "2R Long", "King starting on the e file of the second rank slides three squares either direction, " +
				"subject to the usual restrictions, to castle with the piece on the edge" );
			Castling.AddChoice( "2R Flexible", "King starting on the e file of the second rank slides two or more squares, subject to the usual " +
				"restrictions, to castle with the piece on the edge" );
			Castling.AddChoice( "2R Close-Rook", "King starting on the e file of the second rank slides two squares either direction, " +
				"subject to the usual restrictions, to castle with the piece on the b or h file" );
			Castling.AddChoice( "None", "No castling" );
			Castling.Value = "None";
		}
		#endregion

		#region AddRules
		public override void AddRules()
		{
			base.AddRules();

			// *** CASTLING *** //

			#region Castling
			//	Only handle here if this is a castling type we defined
			if( Castling.Choices.IndexOf( Castling.Value ) < Castling.Choices.IndexOf( "None" ) )
			{
				#region First rank castling rules
				if( Castling.Value[0] != '2' )
				{
					//	The Kings must be centered on e1 and e10
					GenericPiece WhiteKing = new GenericPiece( 0, CastlingType );
					GenericPiece BlackKing = new GenericPiece( 1, CastlingType );
					if( StartingPieces["e1"] != WhiteKing || StartingPieces["e10"] != BlackKing )
						throw new Exception( "Can't enable castling rule because King does not start on a supported square" );

					//	NOTE: we use Shredder-FEN notation exclusively (IAia) because with the king 
					//	centered, king-side and queen-side no longer have any meaning.

					//	STANDARD CASTLING - King slides two squares and corner piece jumps over to adjacent square
					if( Castling.Value == "Standard" )
					{
						AddCastlingRule();
						CastlingMove( 0, "e1", "c1", "a1", "d1", 'A' );
						CastlingMove( 0, "e1", "g1", "i1", "f1", 'I' );
						CastlingMove( 1, "e10", "c10", "a10", "d10", 'a' );
						CastlingMove( 1, "e10", "g10", "i10", "f10", 'i' );
					}
					//	LONG CASTLING - King slides three squares and corner piece jumps over to adjacent square
					else if( Castling.Value == "Long" )
					{
						AddCastlingRule();
						CastlingMove( 0, "e1", "b1", "a1", "c1", 'A' );
						CastlingMove( 0, "e1", "h1", "i1", "g1", 'I' );
						CastlingMove( 1, "e10", "b10", "a10", "c10", 'a' );
						CastlingMove( 1, "e10", "h10", "i10", "g10", 'i' );
					}
					//	FLEXIBLE CASTLING - King slides two or more squares (but must stop short of the 
					//	corner) and the corner piece jumps over to adjacent square
					else if( Castling.Value == "Flexible" )
					{
						AddFlexibleCastlingRule();
						FlexibleCastlingMove( 0, "e1", "c1", "a1", 'A' );
						FlexibleCastlingMove( 0, "e1", "g1", "i1", 'I' );
						FlexibleCastlingMove( 1, "e10", "c10", "a10", 'a' );
						FlexibleCastlingMove( 1, "e10", "g10", "i10", 'i' );
					}
					//	CLOSE-ROOK CASTLING - Castling pieces are on b1/b10 and h1/h10 rather than in the 
					//	corners.  King slides two squares and castling piece jumps over to adjacent square.
					else if( Castling.Value == "Close-Rook" )
					{
						AddCastlingRule();
						CastlingMove( 0, "e1", "c1", "b1", "d1", 'B' );
						CastlingMove( 0, "e1", "g1", "h1", "f1", 'H' );
						CastlingMove( 1, "e10", "c10", "b10", "d10", 'b' );
						CastlingMove( 1, "e10", "g10", "h10", "f10", 'h' );
					}
				}
				#endregion

				#region Second rank castling rules
				else
				{
					//	The Kings must be centered on e2 and e9
					GenericPiece WhiteKing = new GenericPiece( 0, CastlingType );
					GenericPiece BlackKing = new GenericPiece( 1, CastlingType );
					if( StartingPieces["e2"] != WhiteKing || StartingPieces["e9"] != BlackKing )
						throw new Exception( "Can't enable castling rule because King does not start on a supported square" );

					//	2R STANDARD CASTLING - King slides two squares and corner piece jumps over to adjacent square
					if( Castling.Value == "2R Standard" )
					{
						AddCastlingRule();
						CastlingMove( 0, "e2", "c2", "a2", "d2", 'A' );
						CastlingMove( 0, "e2", "g2", "i2", "f2", 'I' );
						CastlingMove( 1, "e9", "c9", "a9", "d9", 'a' );
						CastlingMove( 1, "e9", "g9", "i9", "f9", 'i' );
					}
					//	2R LONG CASTLING - King slides three squares and corner piece jumps over to adjacent square
					else if( Castling.Value == "2R Long" )
					{
						AddCastlingRule();
						CastlingMove( 0, "e2", "b2", "a2", "c2", 'A' );
						CastlingMove( 0, "e2", "h2", "i2", "g2", 'I' );
						CastlingMove( 1, "e9", "b9", "a9", "c9", 'a' );
						CastlingMove( 1, "e9", "h9", "i9", "g9", 'i' );
					}
					//	2R FLEXIBLE CASTLING - King slides two or more squares (but must stop short of the 
					//	corner) and the corner piece jumps over to adjacent square
					else if( Castling.Value == "2R Flexible" )
					{
						AddFlexibleCastlingRule();
						FlexibleCastlingMove( 0, "e2", "c2", "a2", 'A' );
						FlexibleCastlingMove( 0, "e2", "g2", "i2", 'I' );
						FlexibleCastlingMove( 1, "e9", "c9", "a9", 'a' );
						FlexibleCastlingMove( 1, "e9", "g9", "i9", 'i' );
					}
					//	2R CLOSE-ROOK CASTLING - Castling pieces are on b2/b9 and h2/h9 rather than in the 
					//	corners.  King slides two squares and castling piece jumps over to adjacent square.
					else if( Castling.Value == "2R Close-Rook" )
					{
						AddCastlingRule();
						CastlingMove( 0, "e2", "c2", "b2", "d2", 'B' );
						CastlingMove( 0, "e2", "g2", "h2", "f2", 'H' );
						CastlingMove( 1, "e9", "c9", "b9", "d9", 'b' );
						CastlingMove( 1, "e9", "g9", "h9", "f9", 'h' );
					}
				}
				#endregion
			}
			#endregion
		}
		#endregion


		// *** OPERATIONS *** //

		public void AddChessPieceTypes()
		{
			AddPieceType( Queen = new Queen( "Queen", "Q", 1000, 1100 ) );
			AddPieceType( Rook = new Rook( "Rook", "R", 550, 600 ) );
			AddPieceType( Bishop = new Bishop( "Bishop", "B", 350, 400 ) );
			AddPieceType( Knight = new Knight( "Knight", "N", 275, 275 ) );
		}
	}
}
