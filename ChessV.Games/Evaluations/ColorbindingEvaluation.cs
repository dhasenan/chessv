
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

namespace ChessV.Evaluations
{
	public class ColorbindingEvaluation: Evaluation
	{
		// *** CONSTRUCTION *** //

		public ColorbindingEvaluation()
		{
		}


		// *** INITIALIZATION *** //

		#region Initialize
		public override void Initialize( Game game )
		{
			base.Initialize( game );

			//	for now, we only support colorbound pieces that 
			//	see exactly half the board (two slices)
			numSlices = 2;
			piecesPerSlice = new int[2];
			totalMaterialPerSlice = new int[2];

			//	go through all piece types and find those that 
			//	have two slices and add them to the list
			PieceType[] pieceTypes;
			int nPieceTypes = game.GetPieceTypes( out pieceTypes );
			colorboundPieceTypes = new List<PieceType>();
			for( int x = 0; x < nPieceTypes; x++ )
				if( pieceTypes[x].NumSlices == 2 )
					colorboundPieceTypes.Add( pieceTypes[x] );
		}
		#endregion


		// *** OVERRIDES *** //

		#region ReleaseMemoryAllocations
		public override void ReleaseMemoryAllocations()
		{
			base.ReleaseMemoryAllocations();
			colorboundPieceTypes = null;
			piecesPerSlice = null;
			totalMaterialPerSlice = null;
		}
		#endregion

		#region AdjustEvaluation
		public override void AdjustEvaluation( ref int midgameEval, ref int endgameEval )
		{
			//	check for player 0
			piecesPerSlice[0] = 0;
			piecesPerSlice[1] = 0;
			totalMaterialPerSlice[0] = 0;
			totalMaterialPerSlice[1] = 0;
			foreach( PieceType pieceType in colorboundPieceTypes )
			{
				BitBoard pieces = board.GetPieceTypeBitboard( 0, pieceType.TypeNumber );
				while( pieces )
				{
					int square = pieces.ExtractLSB();
					piecesPerSlice[board[square].PieceType.SliceLookup[square]]++;
					totalMaterialPerSlice[board[square].PieceType.SliceLookup[square]] += board[square].PieceType.MidgameValue;
				}
			}
			if( piecesPerSlice[0] > 0 && piecesPerSlice[1] > 0 )
			{
				//	bonus for having a piece on each slice (two bishops bonus)
				midgameEval += 50;
				endgameEval += 50;
			}
			else if( (piecesPerSlice[0] == 0 && piecesPerSlice[1] >= 2) ||
				(piecesPerSlice[1] == 0 && piecesPerSlice[0] >= 2) )
			{
				//	significant penalty for having multiple pieces on the 
				//	same slice and no pieces on the other
				midgameEval -= (totalMaterialPerSlice[0] + totalMaterialPerSlice[1]) / 3;
				endgameEval -= (totalMaterialPerSlice[0] + totalMaterialPerSlice[1]) / 2;
			}

			//	check for player 1
			piecesPerSlice[0] = 0;
			piecesPerSlice[1] = 0;
			totalMaterialPerSlice[0] = 0;
			totalMaterialPerSlice[1] = 0;
			foreach( PieceType pieceType in colorboundPieceTypes )
			{
				BitBoard pieces = board.GetPieceTypeBitboard( 1, pieceType.TypeNumber );
				while( pieces )
				{
					int square = pieces.ExtractLSB();
					piecesPerSlice[board[square].PieceType.SliceLookup[square]]++;
					totalMaterialPerSlice[board[square].PieceType.SliceLookup[square]] += board[square].PieceType.MidgameValue;
				}
			}
			if( piecesPerSlice[0] > 0 && piecesPerSlice[1] > 0 )
			{
				//	bonus for having a piece on each slice (two bishops bonus)
				midgameEval -= 50;
				endgameEval -= 50;
			}
			else if( (piecesPerSlice[0] == 0 && piecesPerSlice[1] >= 2) ||
				(piecesPerSlice[1] == 0 && piecesPerSlice[0] >= 2) )
			{
				//	significant penalty for having multiple pieces on the 
				//	same slice and no pieces on the other
				midgameEval += (totalMaterialPerSlice[0] + totalMaterialPerSlice[1]) / 3;
				endgameEval += (totalMaterialPerSlice[0] + totalMaterialPerSlice[1]) / 2;
			}
		}
		#endregion

		#region GetNotesForPieceType
		public override void GetNotesForPieceType( PieceType type, List<string> notes )
		{
			if( colorboundPieceTypes.Contains( type ) )
				notes.Add( "colorbinding evaluation" );
		}
		#endregion


		// *** PROTECTED DATA *** //

		protected List<PieceType> colorboundPieceTypes;
		protected int numSlices;
		protected int[] piecesPerSlice;
		protected int[] totalMaterialPerSlice;
	}
}
