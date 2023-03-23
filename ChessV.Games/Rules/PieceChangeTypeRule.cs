
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

namespace ChessV.Games.Rules
{
	public class PieceChangeTypeRule: Rule
	{
		public PieceChangeTypeRule()
		{
			changes = new List<PieceTypeChangeParameters>();
		}

		public void TypeChangeOnCapture( PieceType type, PieceType newType, PieceType capturedType = null )
		{
			changes.Add( new PieceTypeChangeParameters( type, newType, false, true, capturedType ) );
		}

		public void TypeChangeOnNonCapture( PieceType type, PieceType newType )
		{
			changes.Add( new PieceTypeChangeParameters( type, newType, true, false ) );
		}

		public void TypeChangeOnEveryMove( PieceType type, PieceType newType )
		{
			changes.Add( new PieceTypeChangeParameters( type, newType, true, true ) );
		}

		public override MoveEventResponse MoveBeingGenerated( MoveList moves, int from, int to, MoveType type )
		{
			Piece movingPiece = Board[from];
			foreach( var change in changes )
				if( change.PieceType == movingPiece.PieceType )
				{
					bool capturing = (type & MoveType.CaptureProperty) != 0;
					if( (capturing && change.ChangeOnCapture) || (!capturing && change.ChangeOnMove) )
					{
						Piece capturedPiece = Board[to];
						if( capturedPiece != null )
						{
							//	does capture have to be of a specific type?
							if( change.RequiredCaptureType != null &&
								//	and is this the wrong type?
								capturedPiece.PieceType != change.RequiredCaptureType )
								//	this change rule is not applicable
								continue;
							moves.BeginMoveAdd( type, from, to );
							moves.AddPickup( from );
							moves.AddPickup( to );
							moves.AddDrop( movingPiece, to, change.NewType );
							moves.EndMoveAdd( change.NewType.MidgameValue - change.PieceType.MidgameValue
								+ change.NewType.GetMidgamePST( from ) - change.PieceType.GetMidgamePST( to )
								+ capturedPiece.PieceType.MidgameValue + 1000 );
						}
						else
						{
							moves.BeginMoveAdd( type, from, to );
							moves.AddPickup( from );
							moves.AddDrop( movingPiece, to, change.NewType );
							moves.EndMoveAdd( change.NewType.MidgameValue - change.PieceType.MidgameValue
								+ change.NewType.GetMidgamePST( from ) - change.PieceType.GetMidgamePST( to ) );
						}
						return MoveEventResponse.Handled;
					}
					continue;
				}
			return MoveEventResponse.NotHandled;
		}

		public override void GetNotesForPieceType( PieceType type, List<string> notes )
		{
			foreach( var change in changes )
				if( change.PieceType == type )
				{
					if( change.ChangeOnMove && change.ChangeOnCapture )
						notes.Add( "type-changes on move to " + change.NewType.Name );
					else if( change.ChangeOnMove )
						notes.Add( "type-changes on non-capture move to " + change.NewType.Name );
					else if( change.ChangeOnCapture )
					{
						if( change.RequiredCaptureType == null )
							notes.Add( "type-changes on capture to " + change.NewType.Name );
						else
							notes.Add( "type-changes on capture of " + change.RequiredCaptureType.Name + " to " + change.NewType.Name );
					}
				}
		}

		protected List<PieceTypeChangeParameters> changes;
	}

	#region struct PieceTypeChangeParameters
	public struct PieceTypeChangeParameters
	{
		public PieceType PieceType;
		public PieceType NewType;
		public bool ChangeOnMove;
		public bool ChangeOnCapture;
		public PieceType RequiredCaptureType;

		public PieceTypeChangeParameters
			( PieceType type, 
			  PieceType newType, 
			  bool changeOnMove, 
			  bool changeOnCapture, 
			  PieceType requiredCaptureType = null )
		{
			PieceType = type;
			NewType = newType;
			ChangeOnMove = changeOnMove;
			ChangeOnCapture = changeOnCapture;
			RequiredCaptureType = requiredCaptureType;
		}
	}
	#endregion
}
