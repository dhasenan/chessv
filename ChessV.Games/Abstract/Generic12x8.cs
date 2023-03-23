
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
using ChessV.Evaluations;
using ChessV.Games.Rules;

namespace ChessV.Games.Abstract
{
	//**********************************************************************
	//
	//                           Generic12x8
	//
	//    The Generic game classes make it easier to specify games by 
	//    providing functionality common to chess variants.  This class 
	//    extends the Generic__x8 class by adding support for a 
	//    variety of different castling rules used on 12x8 board

	[Game("Generic 12x8", typeof(Geometry.Rectangular), 12, 8,
		  Template=true)]
	public class Generic12x8: Generic__x8
	{
		// *** GAME VARIABLES *** //

		[GameVariable] public ChoiceVariable Castling { get; set; }


		// *** CONSTRUCTION *** //

		public Generic12x8
			( Symmetry symmetry ):
				base
					( /* num files = */ 12,
					  /* symmetry = */ symmetry )
		{
		}


		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Castling = new ChoiceVariable();
			Castling.AddChoice( "3-3", "King starting on f or g file slides three squares either direction, " +
				"subject to the usual restrictions, to castle with the piece in the corner" );
			Castling.AddChoice( "3-4", "King starting on f or g file slides three squares when castling short " +
				"or four when castling long, subject to the usual restrictions, to castle with the piece in the corner" );
			Castling.AddChoice( "4-4", "King starting on f or g file slides four squares either direction, " +
				"subject to the usual restrictions, to castle with the piece in the corner" );
			Castling.AddChoice( "4-5", "King starting on f or g file slides four squares when castling short " +
				"or five when castling long, subject to the usual restrictions, to castle with the piece in the corner" );
			Castling.AddChoice( "Close-Rook 2-2", "King starting on f or g file slides two squares either direction, " +
				"subject to the usual restrictions, to castle with the piece on the b or k file" );
			Castling.AddChoice( "Close-Rook 2-3", "King starting on f or g file slides two squares when castling short " +
				"or three when castling long, subject to the usual restrictions, to castle with the piece on the b or k file" );
			Castling.AddChoice( "Close-Rook 3-3", "King starting on f or g file slides three squares either direction, " +
				"subject to the usual restrictions, to castle with the piece on the b or k file" );
			Castling.AddChoice( "Close-Rook 3-4", "King starting on f or g file slides three squares when castling short " +
				"or four when castling long, subject to the usual restrictions, to castle with the piece on the b or k file" );
			Castling.AddChoice( "Flexible", "King starting on f or g file slides two or more squares, subject to the usual " +
				"restrictions, to castle with the piece in the corner" );
			Castling.AddChoice( "Close-Rook Flexible", "King starting on f or g file slides two or more squares, subject to the usual " +
				"restrictions, to castle with the piece on the b or k file" );
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
				//	of castilng and we have to accomodate the King on either f1 or g1.  On 12x8 we always use 
				//	shredder notation for castling though.

				//	find the king's start square (must be f1 or g1)
				GenericPiece WhiteKing = new GenericPiece( 0, CastlingType );
				GenericPiece BlackKing = new GenericPiece( 1, CastlingType );
				string whiteKingSquare = null;
				string blackKingSquare = null;
				if( StartingPieces["f1"] == WhiteKing )
					whiteKingSquare = "f1";
				else if( StartingPieces["g1"] == WhiteKing )
					whiteKingSquare = "g1";
				if( StartingPieces["f8"] == BlackKing )
					blackKingSquare = "f8";
				else if( StartingPieces["g8"] == BlackKing )
					blackKingSquare = "g8";
				if( whiteKingSquare == null || blackKingSquare == null )
					throw new Exception( "Can't enable castling rule because King does not start on a supported square" );

				//	FLEXIBLE CASTLING - King slides two or more squares towards the corner 
				//	and the corner piece leaps to the square immediately to the other side
				if( Castling.Value == "Flexible" )
				{
					AddFlexibleCastlingRule();
					if( whiteKingSquare == "g1" )
					{
						FlexibleCastlingMove( 0, "g1", "i1", "l1", 'L' );
						FlexibleCastlingMove( 0, "g1", "e1", "a1", 'A' );
					}
					else
					{
						FlexibleCastlingMove( 0, "f1", "d1", "a1", 'A' );
						FlexibleCastlingMove( 0, "f1", "h1", "l1", 'L' );
					}
					if( blackKingSquare == "g8" )
					{
						FlexibleCastlingMove( 1, "g8", "i8", "l8", 'l' );
						FlexibleCastlingMove( 1, "g8", "e8", "a8", 'a' );
					}
					else
					{
						FlexibleCastlingMove( 1, "f8", "d8", "a8", 'a' );
						FlexibleCastlingMove( 1, "f8", "h8", "l8", 'l' );
					}
				}
				//	CLOSE-ROOK FLEXIBLE CASTLING - King slides two or more squares towards the 
				//	b or k file and that piece leaps to the square immediately to the other side
				else if( Castling.Value == "Close-Rook Flexible" )
				{
					AddFlexibleCastlingRule();
					if( whiteKingSquare == "g1" )
					{
						FlexibleCastlingMove( 0, "g1", "i1", "k1", 'K' );
						FlexibleCastlingMove( 0, "g1", "e1", "b1", 'B' );
					}
					else
					{
						FlexibleCastlingMove( 0, "f1", "d1", "b1", 'B' );
						FlexibleCastlingMove( 0, "f1", "h1", "k1", 'K' );
					}
					if( blackKingSquare == "g8" )
					{
						FlexibleCastlingMove( 1, "g8", "i8", "k8", 'k' );
						FlexibleCastlingMove( 1, "g8", "e8", "b8", 'b' );
					}
					else
					{
						FlexibleCastlingMove( 1, "f8", "d8", "b8", 'b' );
						FlexibleCastlingMove( 1, "f8", "h8", "k8", 'k' );
					}
				}
				else
				{
					//	handle implementation of all other castling options by algorithm
					bool closeRook = Castling.Value.IndexOf( "Close-Rook" ) >= 0;
					int shortDistance = Convert.ToInt32( Castling.Value.Substring( Castling.Value.Length - 3, 1 ) );
					int longDistance = Convert.ToInt32( Castling.Value.Substring( Castling.Value.Length - 1, 1 ) );
					AddCastlingRule();
					if( !closeRook )
					{
						if( whiteKingSquare == "g1" )
						{
							CastlingMove( 0, "g1", (char) ('g' + shortDistance) + "1", "l1", (char) ('g' + shortDistance - 1) + "1", 'L' );
							CastlingMove( 0, "g1", (char) ('g' - longDistance) + "1", "a1", (char) ('g' - longDistance + 1) + "1", 'A' );
						}
						else
						{
							CastlingMove( 0, "f1", (char) ('f' + longDistance) + "1", "l1", (char) ('f' + longDistance - 1) + "1", 'L' );
							CastlingMove( 0, "f1", (char) ('f' - shortDistance) + "1", "a1", (char) ('f' - shortDistance + 1) + "1", 'A' );
						}
						if( blackKingSquare == "g8" )
						{
							CastlingMove( 1, "g8", (char) ('g' + shortDistance) + "8", "l8", (char) ('g' + shortDistance - 1) + "8", 'l' );
							CastlingMove( 1, "g8", (char) ('g' - longDistance) + "8", "a8", (char) ('g' - longDistance + 1) + "8", 'a' );
						}
						else
						{
							CastlingMove( 1, "f8", (char) ('f' + longDistance) + "8", "l8", (char) ('f' + longDistance - 1) + "8", 'l' );
							CastlingMove( 1, "f8", (char) ('f' - shortDistance) + "8", "a8", (char) ('f' - shortDistance + 1) + "8", 'a' );
						}
					}
					else
					{
						{
							if( whiteKingSquare == "g1" )
							{
								CastlingMove( 0, "g1", (char) ('g' + shortDistance) + "1", "k1", (char) ('g' + shortDistance - 1) + "1", 'K' );
								CastlingMove( 0, "g1", (char) ('g' - longDistance) + "1", "b1", (char) ('g' - longDistance + 1) + "1", 'B' );
							}
							else
							{
								CastlingMove( 0, "f1", (char) ('f' + longDistance) + "1", "k1", (char) ('f' + longDistance - 1) + "1", 'K' );
								CastlingMove( 0, "f1", (char) ('f' - shortDistance) + "1", "b1", (char) ('f' - shortDistance + 1) + "1", 'B' );
							}
							if( blackKingSquare == "g8" )
							{
								CastlingMove( 1, "g8", (char) ('g' + shortDistance) + "8", "k8", (char) ('g' + shortDistance - 1) + "8", 'k' );
								CastlingMove( 1, "g8", (char) ('g' - longDistance) + "8", "b8", (char) ('g' - longDistance + 1) + "8", 'b' );
							}
							else
							{
								CastlingMove( 1, "f8", (char) ('f' + longDistance) + "8", "k8", (char) ('f' + longDistance - 1) + "8", 'k' );
								CastlingMove( 1, "f8", (char) ('f' - shortDistance) + "8", "b8", (char) ('f' - shortDistance + 1) + "8", 'b' );
							}
						}
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
					OutpostEval.AddOutpostBonus( Knight, 10, 4, 5, 5 );
				if( Bishop != null && Bishop.Enabled )
					OutpostEval.AddOutpostBonus( Bishop, 8, 2, 5, 5 );
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
			AddPieceType( Queen = new Queen( "Queen", "Q", 1000, 1050 ) );
			AddPieceType( Rook = new Rook( "Rook", "R", 550, 600 ) );
			AddPieceType( Bishop = new Bishop( "Bishop", "B", 350, 350 ) );
			AddPieceType( Knight = new Knight( "Knight", "N", 285, 285 ) );
		}
	}
}
