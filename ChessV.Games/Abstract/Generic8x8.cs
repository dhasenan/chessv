
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
	//                           Generic8x8
	//
	//    The Generic game classes make it easier to specify games by 
	//    providing functionality common to chess variants.  This class 
	//    extends the Generic__x8 class by adding castling support.

	[Game( "Generic 8x8",
		  typeof( Geometry.Rectangular ), 8, 8,
		  GameDescription1 = "Abstract class for chess-like games",
		  GameDescription2 = "on a board with 8 files and 8 ranks",
		  Template = true )]
	public class Generic8x8: Generic__x8
	{
		// *** GAME VARIABLES *** //

		[GameVariable] public ChoiceVariable Castling { get; set; }


		// *** CONSTRUCTION *** //

		public Generic8x8
			( Symmetry symmetry ) :
				base
					( /* num files = */ 8,
					  /* symmetry = */ symmetry )
		{
		}


		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Castling = new ChoiceVariable();
			Castling.AddChoice( "Standard", "King starting on d or e file slides two squares either direction, " +
				"subject to the usual restrictions, to castle with the piece in the corner" );
			Castling.AddChoice( "Long", "King starting on d or e file slides two squares when castling short " +
				"or three when castling long, subject to the usual restrictions, to castle with the piece in the corner" );
			Castling.AddChoice( "Flexible", "King starting on d or e file slides two or more squares, subject to the usual " +
				"restrictions, to castle with the piece in the corner" );
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
				//	We will accomodate the king starting on either d1 or e1.
				//	The FEN privilege notation KQkq will be used if the kings 
				//	start on e1/e8 and the d1 piece has a 'Q' notation.
				//	Otherwise, we will use Shredder-FEN notation (HAha)

				//	find the king's start square (must be d1 or e1)
				GenericPiece WhiteKing = new GenericPiece( 0, CastlingType );
				GenericPiece BlackKing = new GenericPiece( 1, CastlingType );
				string kingSquare;
				if( StartingPieces["d1"] == WhiteKing )
					kingSquare = "d1";
				else if( StartingPieces["e1"] == WhiteKing )
					kingSquare = "e1";
				else
					throw new Exception( "Can't enable castling rule because King does not start on a supported square" );

				//	Use Shredder-FEN notation?
				bool shredderNotation = true;
				if( kingSquare == "e1" && StartingPieces["e8"] != null && StartingPieces["e8"] == BlackKing &&
					StartingPieces["d1"] != null && StartingPieces["d1"].PieceType.Notation[0] == "Q" )
					shredderNotation = false;

				//	STANDARD CASTLING - King slides two squares and corner piece jumps over to adjacent square
				if( Castling.Value == "Standard" )
				{
					AddCastlingRule();
					if( kingSquare == "e1" )
					{
						CastlingMove( 0, "e1", "g1", "h1", "f1", shredderNotation ? 'H' : 'K' );
						CastlingMove( 0, "e1", "c1", "a1", "d1", shredderNotation ? 'A' : 'Q' );
						CastlingMove( 1, "e8", "g8", "h8", "f8", shredderNotation ? 'h' : 'k' );
						CastlingMove( 1, "e8", "c8", "a8", "d8", shredderNotation ? 'a' : 'q' );
					}
					else
					{
						CastlingMove( 0, "d1", "f1", "h1", "e1", 'H' );
						CastlingMove( 0, "d1", "b1", "a1", "c1", 'A' );
						CastlingMove( 1, "d8", "f8", "h8", "e8", 'h' );
						CastlingMove( 1, "d8", "b8", "a8", "c8", 'a' );
					}
				}
				//	LONG CASTLING - King slides two squares to closer corner or three squares to 
				//	farther corner and the corner piece jumps over to adjacent square
				else if( Castling.Value == "Long" )
				{
					AddCastlingRule();
					if( kingSquare == "e1" )
					{
						CastlingMove( 0, "e1", "g1", "h1", "f1", shredderNotation ? 'H' : 'K' );
						CastlingMove( 0, "e1", "b1", "a1", "c1", shredderNotation ? 'A' : 'Q' );
						CastlingMove( 1, "e8", "g8", "h8", "f8", shredderNotation ? 'h' : 'k' );
						CastlingMove( 1, "e8", "b8", "a8", "c8", shredderNotation ? 'a' : 'q' );
					}
					else
					{
						CastlingMove( 0, "d1", "g1", "h1", "f1", 'H' );
						CastlingMove( 0, "d1", "b1", "a1", "c1", 'A' );
						CastlingMove( 1, "d8", "g8", "h8", "f8", 'h' );
						CastlingMove( 1, "d8", "b8", "a8", "c8", 'a' );
					}
				}
				//	FLEXIBLE CASTLING - King slides two or more squares (but must stop short of the 
				//	corner) and the corner piece jumps over to adjacent square
				else if( Castling.Value == "Flexible" )
				{
					AddFlexibleCastlingRule();
					if( kingSquare == "e1" )
					{
						FlexibleCastlingMove( 0, "e1", "g1", "h1", shredderNotation ? 'H' : 'K' );
						FlexibleCastlingMove( 0, "e1", "c1", "a1", shredderNotation ? 'A' : 'Q' );
						FlexibleCastlingMove( 1, "e8", "g8", "h8", shredderNotation ? 'h' : 'k' );
						FlexibleCastlingMove( 1, "e8", "c8", "a8", shredderNotation ? 'a' : 'q' );
					}
					else
					{
						FlexibleCastlingMove( 0, "d1", "f1", "h1", 'H' );
						FlexibleCastlingMove( 0, "d1", "b1", "a1", 'A' );
						FlexibleCastlingMove( 1, "d8", "f8", "h8", 'h' );
						FlexibleCastlingMove( 1, "d8", "b8", "a8", 'a' );
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
			CheckmateRule rule = (CheckmateRule) FindRule( typeof( CheckmateRule ) );
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
			AddPieceType( Rook = new Rook( "Rook", "R", 500, 550 ) );
			AddPieceType( Bishop = new Bishop( "Bishop", "B", 325, 350 ) );
			AddPieceType( Knight = new Knight( "Knight", "N", 325, 325 ) );
			AddPieceType( Queen = new Queen( "Queen", "Q", 950, 1000 ) );
		}
	}
}
