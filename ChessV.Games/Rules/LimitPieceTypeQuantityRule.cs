
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
	public class LimitPieceTypeQuantityRule: Rule
	{
		public LimitPieceTypeQuantityRule()
		{
			pieceTypeLimits = new List<KeyValuePair<PieceType, int>>();
			limits = new Dictionary<int, int>();
		}

		public LimitPieceTypeQuantityRule( PieceType type, int limit )
		{
			pieceTypeLimits = new List<KeyValuePair<PieceType, int>>();
			pieceTypeLimits.Add( new KeyValuePair<PieceType, int>( type, limit ) );
			limits = new Dictionary<int, int>();
		}

		public override void Initialize( Game game )
		{
			base.Initialize( game );
			foreach( var pair in pieceTypeLimits )
				limits.Add( game.GetPieceTypeNumber( pair.Key ), pair.Value );
		}

		public override MoveEventResponse MoveBeingMade( MoveInfo move, int ply )
		{
			int currentType = Board[move.ToSquare].TypeNumber;
			if( move.OriginalType != currentType )
			{
				int limit;
				if( limits.TryGetValue( currentType, out limit ) &&
					Board.GetPieceTypeBitboard( move.Player, currentType ).BitCount > limit )
					//	This would exceed the limit for this piece so the move is illegal
					return MoveEventResponse.IllegalMove;
			}
			return MoveEventResponse.NotHandled;
		}

		public override void GetNotesForPieceType( PieceType type, List<string> notes )
		{
			if( limits.ContainsKey( type.TypeNumber ) )
				notes.Add( "limited quantity: " + limits[type.TypeNumber].ToString() );
		}

		protected Dictionary<int, int> limits;
		protected List<KeyValuePair<PieceType, int>> pieceTypeLimits;
	}
}
