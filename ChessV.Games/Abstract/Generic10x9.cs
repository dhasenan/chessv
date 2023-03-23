
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
using ChessV.Games.Rules;
using ChessV.Evaluations;

namespace ChessV.Games.Abstract
{
	//**********************************************************************
	//
	//                           Generic10x9
	//
	//    The Generic game classes make it easier to specify games by 
	//    providing functionality common to chess variants.  This class 
	//    extends the Generic__x9 class by adding castling support.

	[Game("Generic 10x9", typeof(Geometry.Rectangular), 10, 9, 
		  Template = true)]
	public class Generic10x9: Generic__x9
	{
		// *** GAME VARIABLES *** //

		[GameVariable] public ChoiceVariable Castling { get; set; }


		// *** CONSTRUCTION *** //

		public Generic10x9
			( Symmetry symmetry ): 
				base
					( /* num files = */ 10, 
					  /* symmetry = */ symmetry )
		{
		}


		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Castling = new ChoiceVariable();
			Castling.AddChoice( "Standard", "King starting on e or f file slides three squares either direction, " +
				"subject to the usual restrictions, to castle with the piece in the corner" );
			Castling.AddChoice( "Long", "King starting on e or f file slides three squares when castling short " +
				"or four when castling long, subject to the usual restrictions, to castle with the piece in the corner" );
			Castling.AddChoice( "Flexible", "King starting on e or f file slides two or more squares, subject to the usual " +
				"restrictions, to castle with the piece in the corner" );
			Castling.AddChoice( "Close-Rook", "King starting on e or f file slides two squares either direction, " +
				"subject to the usual restrictions, to castle with the piece on the b or i file" );
			Castling.AddChoice( "Close-Rook Flexible", "King starting on e or f file slides two or more squares, " +
				"subject to the usual restrictions, to castle with the piece on the b or i file" );
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
				//	Adding castling rule is somewhat complicated because there are a number of different forms 
				//	of castilng and we have to accomodate the King on either e1 or f1 as well as the fact that 
				//	the FEN notation, (typically KQkq), for King-side or Queen-side might be inappropriate,  
				//	in which case we use Shredder-FEN notation where the priv char is the file of the rook

				//	find the king's start square (must be e1 or f1)
				GenericPiece WhiteKing = new GenericPiece( 0, CastlingType );
				GenericPiece BlackKing = new GenericPiece( 1, CastlingType );
				bool supported = false;
				string kingSquare = null;
				if( StartingPieces["e1"] == WhiteKing )
				{
					kingSquare = "e1";
					if( StartingPieces["e9"] == BlackKing )
						supported = true;
				}
				else if( StartingPieces["f1"] == WhiteKing )
				{
					kingSquare = "f1";
					if( StartingPieces["f9"] == BlackKing )
						supported = true;
				}
				if( !supported )
					throw new Exception( "Can't enable castling rule because King does not start on a supported square" );

				//	STANDARD CASTLING - King slides three squares and corner piece jumps over to adjacent square
				if( Castling.Value == "Standard" )
				{
					AddCastlingRule();
					if( kingSquare == "f1" )
					{
						CastlingMove( 0, "f1", "i1", "j1", "h1", 'J' );
						CastlingMove( 0, "f1", "c1", "a1", "d1", 'A' );
						CastlingMove( 1, "f9", "i9", "j9", "h9", 'j' );
						CastlingMove( 1, "f9", "c9", "a9", "d9", 'a' );
					}
					else
					{
						CastlingMove( 0, "e1", "b1", "a1", "c1", 'A' );
						CastlingMove( 0, "e1", "h1", "j1", "g1", 'J' );
						CastlingMove( 1, "e9", "b9", "a9", "c9", 'a' );
						CastlingMove( 1, "e9", "h9", "j9", "g9", 'j' );
					}
				}
				//	LONG CASTLING - King slides three squares to closer corner or four squares to 
				//	farther corner and the corner piece jumps over to adjacent square
				else if( Castling.Value == "Long" )
				{
					AddCastlingRule();
					if( kingSquare == "f1" )
					{
						CastlingMove( 0, "f1", "i1", "j1", "h1", 'J' );
						CastlingMove( 0, "f1", "b1", "a1", "c1", 'A' );
						CastlingMove( 1, "f9", "i9", "j9", "h9", 'j' );
						CastlingMove( 1, "f9", "b9", "a9", "c9", 'a' );
					}
					else
					{
						CastlingMove( 0, "e1", "b1", "a1", "c1", 'A' );
						CastlingMove( 0, "e1", "i1", "j1", "h1", 'J' );
						CastlingMove( 1, "e9", "b9", "a9", "c9", 'a' );
						CastlingMove( 1, "e9", "i9", "j9", "h9", 'j' );
					}
				}
				//	FLEXIBLE CASTLING - King slides two or more squares (but must stop short of the 
				//	corner) and the corner piece jumps over to adjacent square
				else if( Castling.Value == "Flexible" )
				{
					AddFlexibleCastlingRule();
					if( kingSquare == "f1" )
					{
						FlexibleCastlingMove( 0, "f1", "h1", "j1", 'J' );
						FlexibleCastlingMove( 0, "f1", "d1", "a1", 'A' );
						FlexibleCastlingMove( 1, "f9", "h9", "j9", 'j' );
						FlexibleCastlingMove( 1, "f9", "d9", "a9", 'a' );
					}
					else
					{
						FlexibleCastlingMove( 0, "e1", "c1", "a1", 'A' );
						FlexibleCastlingMove( 0, "e1", "g1", "j1", 'J' );
						FlexibleCastlingMove( 1, "e9", "c9", "a9", 'a' );
						FlexibleCastlingMove( 1, "e9", "g9", "j9", 'j' );
					}
				}
				//	CLOSE ROOK CASTLING - Castling pieces are on b1/b9 and i1/i9 rather than in the 
				//	corners.  King slides two squares and castling piece jumps over to adjacent square.
				else if( Castling.Value == "Close-Rook" )
				{
					AddCastlingRule();
					if( kingSquare == "f1" )
					{
						CastlingMove( 0, "f1", "h1", "i1", "g1", 'I' );
						CastlingMove( 0, "f1", "d1", "b1", "e1", 'B' );
						CastlingMove( 1, "f9", "h9", "i9", "g9", 'i' );
						CastlingMove( 1, "f9", "d9", "b9", "e9", 'b' );
					}
					else
					{
						CastlingMove( 0, "e1", "c1", "b1", "d1", 'B' );
						CastlingMove( 0, "e1", "g1", "i1", "f1", 'I' );
						CastlingMove( 1, "e9", "c9", "b9", "d9", 'b' );
						CastlingMove( 1, "e9", "g9", "i9", "f9", 'i' );
					}
				}
				//	CLOSE-ROOK FLEXIBLE - King slides two or more squares to castle with 
				//	the piece on the b or i file (which jumps to the other side)
				else if( Castling.Value == "Close-Rook Flexible" )
				{
					AddFlexibleCastlingRule();
					if( kingSquare == "f1" )
					{
						FlexibleCastlingMove( 0, "f1", "h1", "i1", 'I' );
						FlexibleCastlingMove( 0, "f1", "d1", "b1", 'B' );
						FlexibleCastlingMove( 1, "f9", "h9", "i9", 'i' );
						FlexibleCastlingMove( 1, "f9", "d9", "b9", 'b' );
					}
					else
					{
						FlexibleCastlingMove( 0, "e1", "c1", "b1", 'B' );
						FlexibleCastlingMove( 0, "e1", "g1", "i1", 'I' );
						FlexibleCastlingMove( 1, "e9", "c9", "b9", 'b' );
						FlexibleCastlingMove( 1, "e9", "g9", "i9", 'i' );
					}
				}
			}
			#endregion
		}
		#endregion

		#region AddEvaluations
		public override void AddEvaluations()
		{
			base.AddEvaluations();

			//	Outpost Evaluations
			if( (Knight != null && Knight.Enabled) ||
				(Bishop != null && Bishop.Enabled) )
			{
				OutpostEval = new OutpostEvaluation();
				if( Knight != null && Knight.Enabled )
					OutpostEval.AddOutpostBonus( Knight );
				if( Bishop != null && Bishop.Enabled )
					OutpostEval.AddOutpostBonus( Bishop, 10, 2, 5, 5 );
				AddEvaluation( OutpostEval );
			}

			//	Rook-type Evaluations (rook-mover on open file 
			//	and rook-mover on 7th ranks with enemy king on 8th)

			//	Do we have a royal king?
			CheckmateRule rule = (CheckmateRule) FindRule( typeof(CheckmateRule) );
			bool royalKing = rule != null && King != null && King.Enabled && rule.RoyalPieceType == King;

			if( (Rook != null && Rook.Enabled) ||
				(Queen != null && Queen.Enabled && royalKing) )
			{
				RookTypeEval = new RookTypeEvaluation();
				if( Rook != null && Rook.Enabled )
				{
					RookTypeEval.AddOpenFileBonus( Rook );
					if( royalKing )
						RookTypeEval.AddRookOn7thBonus( Rook, King );
				}
				if( Queen != null && Queen.Enabled && royalKing )
					RookTypeEval.AddRookOn7thBonus( Queen, King, 2, 8 );
				AddEvaluation( RookTypeEval );
			}
		}
		#endregion


		// *** OPERATIONS *** //

		public void AddChessPieceTypes()
		{
			AddPieceType( Queen = new Queen( "Queen", "Q", 950, 950 ) );
			AddPieceType( Rook = new Rook( "Rook", "R", 475, 500 ) );
			AddPieceType( Bishop = new Bishop( "Bishop", "B", 350, 350 ) );
			AddPieceType( Knight = new Knight( "Knight", "N", 285, 285 ) );
		}
	}
}
