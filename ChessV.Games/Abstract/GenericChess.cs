
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

using System.Collections.Generic;
using ChessV.Games.Rules;
using ChessV.Evaluations;
using System;

namespace ChessV.Games.Abstract
{
	//**********************************************************************
	//
	//                          GenericChess
	//
	//    The Generic game classes make it easier to specify chess-like 
	//    games by providing functionality common to chess variants.  
	//    This is the base class from which other "Generic" classes are 
	//    derived to provide even more common functionality.  
	//
	//    This class provides the requirements for a royal King and 
	//    standard Chess pawns, defines the 50-move rule and draw by 
	//    repitition rule, and sets the standard FEN format.

	[Game("Generic Chess", typeof(Geometry.Rectangular), Template = true)]
	public class GenericChess: Game
	{
		// *** PIECE TYPES *** //

		[Royal] public PieceType King;
		public PieceType Pawn;

		public PieceType Queen;     //   \   This class creates the King and Pawn, but
		public PieceType Rook;      //    \  not these four types.  They are set up only 
		public PieceType Bishop;    //    /  by derived class, but are declared here to 
		public PieceType Knight;    //   /   make things uniform for these common types.


		// *** GAME VARIABLES *** //

		[GameVariable] public ChoiceVariable StalemateResult { get; set; }
		[GameVariable] public ChoiceVariable PromotionRule { get; set; }
		[GameVariable] public string PromotionTypes { get; set; }
		[GameVariable] public bool BareKing { get; set; }
		[GameVariable] public bool EnPassant { get; set; }
		[GameVariable] public PieceType PromotingType { get; set; }
		[GameVariable] public PieceType CastlingType { get; set; }


		// *** PROPERTIES *** //

		public PawnHashEntry PawnStructureInfo { get; set; }


		// *** EVALUATIONS *** //

		public OutpostEvaluation OutpostEval { get; set; }
		public RookTypeEvaluation RookTypeEval { get; set; }


		// *** CONSTRUCTION *** //

		#region Constructor
		public GenericChess
			( int nFiles,               // number of files on the board
			  int nRanks,               // number of ranks on the board
			  Symmetry symmetry ):      // symmetry determining board mirroring/rotation
                base( 2, nFiles, nRanks, symmetry )
        {
		}
		#endregion


		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			FENFormat = "{array} {current player} {castling} {en-passant} {half-move clock} {turn number}";
			FENStart = "#{Array} w #default #default 0 1";
			StalemateResult = new ChoiceVariable( new string[] { "Draw", "Win", "Loss" } );
			StalemateResult.Value = "Draw";
			PromotionRule = new ChoiceVariable( new string[] { "None", "Standard", "Replacement", "Custom" } );
			PromotionRule.Value = "Standard";
			PromotionTypes = "";
			BareKing = false;
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			AddPieceType( King = new King( "King", "K", 0, 0 ) );
			AddPieceType( Pawn = new Pawn( "Pawn", "P", 100, 125 ) );
			CastlingType = King;
		}
		#endregion

		#region AddRules
		public override void AddRules()
		{
			// *** PROMOTION *** //
			if( PromotionRule.Value == "Standard" )
			{
				if( PromotingType == null )
					PromotingType = Pawn;
				List<PieceType> availablePromotionTypes = ParseTypeListFromString( PromotionTypes );
				AddBasicPromotionRule( PromotingType, availablePromotionTypes, loc => loc.Rank == Board.NumRanks - 1 );
			}
			else if( PromotionRule.Value == "Replacement" )
			{
				if( PromotingType == null )
					PromotingType = Pawn;
				AddPromoteByReplacementRule( PromotingType, loc => loc.Rank == Board.NumRanks - 1 ?
					PromotionOption.MustPromote : PromotionOption.CannotPromote );
			}

			// *** EN-PASSANT *** //
			if( EnPassant && Pawn.Enabled )
				AddEnPassantRule( Pawn, new Direction( 1, 0 ) );

			// *** BARE KING *** //
			if( BareKing )
				AddRule( new BareKingRule() );

			//	We added the Bare King rule first before calling the 
			//	base class because BareKing must happen before the 
			//	checkmate rule to work correctly
			base.AddRules();

			// *** STALEMATE RESULT *** //
			if( StalemateResult.Value == "Loss" )
				((CheckmateRule) FindRule( typeof(Rules.CheckmateRule), true )).StalemateResult = MoveEventResponse.GameLost;
			else if( StalemateResult.Value == "Win" )
				((CheckmateRule) FindRule( typeof( Rules.CheckmateRule ), true )).StalemateResult = MoveEventResponse.GameWon;

			// *** FIFTY-MOVE RULE *** //
			AddRule( new Move50Rule( Pawn ) );

			// *** DRAW-BY-REPETITION RULE *** //
			AddRule( new RepetitionDrawRule() );
		}
		#endregion

		#region ReorderRules
		public override void ReorderRules()
		{
			//	pass to base class first
			base.ReorderRules();
			//	if we have a RepetitionDrawRule, it needs to be at the 
			//	end of the list so it will get the messages last
			Rule repetitionDrawRule = null;
			foreach( Rule rule in rules )
			{
				if( rule is RepetitionDrawRule )
				{
					repetitionDrawRule = rule;
					rules.Remove( rule );
					break;
				}
			}
			if( repetitionDrawRule != null )
				rules.Add( repetitionDrawRule );
		}
		#endregion

		#region AddEvaluations
		public override void AddEvaluations()
		{
			base.AddEvaluations();

			//  Add pawn structure evaluation
			if( Pawn.Enabled )
				AddEvaluation( new PawnStructureEvaluation() );

			//	Add development evaluation
			AddEvaluation( new DevelopmentEvaluation() );

			//	Add evalation for low or insufficient material
			AddEvaluation( new LowMaterialEvaluation() );

			//	Check for colorbound pieces
			bool colorboundPieces = false;
			for( int x = 0; x < nPieceTypes; x++ )
				if( pieceTypes[x].NumSlices == 2 )
					colorboundPieces = true;
			if( colorboundPieces )
				AddEvaluation( new ColorbindingEvaluation() );
		}
		#endregion


		// *** OVERRIDES *** //

		#region CanPruneMove
		public override bool CanPruneMove( MoveInfo move )
		{
			//	do not prune passed pawn pushes
			return PromotionRule.Value == "None" || !PawnStructureInfo.PassedPawns[move.FromSquare];
		}
		#endregion

		#region RemoveRule
		public override void RemoveRule( Type ruleType, bool inheritedTypes = false )
		{
			base.RemoveRule( ruleType, inheritedTypes );
			if( ruleType == typeof(BasicPromotionRule) || 
				ruleType.IsSubclassOf( typeof(BasicPromotionRule) ) )
			{
				PromotingType = null;
			}
		}
		#endregion


		// *** HELPER FUNCTIONS *** //

		#region EnPassant
		public void AddEnPassantRule( PieceType pawnType, int nDirection )
		{ AddRule( new EnPassantRule( pawnType, nDirection ) ); }

		public void AddEnPassantRule( PieceType pawnType, Direction direction )
		{ AddEnPassantRule( pawnType, GetDirectionNumber( direction ) ); }
		#endregion

		#region Castling
		public void AddCastlingRule()
		{
			castlingRule = new CastlingRule();
			AddRule( castlingRule );
		}

		public void AddFlexibleCastlingRule()
		{
			castlingRule = new FlexibleCastlingRule();
			AddRule( castlingRule );
		}

    /** privChar: A character representing one privilege to castle, e.g. K for white's kingside castle */
    public void castlingMove( int player, int kingFrom, int kingTo, int otherFrom, int otherTo, char privChar )
		{
			castlingRule.AddCastlingMove( player, kingFrom, kingTo, otherFrom, otherTo, privChar );
		}

    public void CastlingMove( int player, string kingFrom, string kingTo, string otherFrom, string otherTo, char privChar )
		{
			castlingMove( player, NotationToSquare( kingFrom ), NotationToSquare( kingTo ),
				NotationToSquare( otherFrom ), NotationToSquare( otherTo ), privChar );
		}

    public void FlexibleCastlingMove( int player, string kingFrom, string kingTo, string otherFrom, char privChar, bool allowMoveOntoCastlingPiece = false )
		{
			castlingMove( player, NotationToSquare( kingFrom ), NotationToSquare( kingTo ),
				NotationToSquare( otherFrom ), allowMoveOntoCastlingPiece ? 1 : 0, privChar );
		}
		#endregion

		#region Promotion
		public void AddBasicPromotionRule( PieceType promotingType, List<PieceType> availablePromotionTypes, ConditionalLocationDelegate destinationConditionDelegate )
		{
			if( PromotingType == null )
				PromotingType = promotingType;
			AddRule( new BasicPromotionRule( promotingType, availablePromotionTypes, destinationConditionDelegate ) );
		}

		public void AddBasicPromotionRule( PieceType promotingType, List<PieceType> availablePromotionTypes, ConditionalLocationDelegate destinationConditionDelegate, ConditionalLocationDelegate originConditionDelegate )
		{
			if( PromotingType == null )
				PromotingType = promotingType;
			AddRule( new BasicPromotionRule( promotingType, availablePromotionTypes, destinationConditionDelegate, originConditionDelegate ) );
		}

		public void AddPromoteByReplacementRule( PieceType promotingType, OptionalPromotionLocationDelegate conditionDelegate )
		{
			if( PromotingType == null )
				PromotingType = promotingType;
			AddRule( new PromoteByReplacementRule( promotingType, conditionDelegate ) );
		}
		#endregion

		#region cleanup
		protected override void cleanup()
		{
			base.cleanup();
			PawnStructureInfo = new PawnHashEntry();
		}
		#endregion


		// *** PROTECTED DATA MEMBERS *** //

		protected CastlingRule castlingRule;
	}
}
