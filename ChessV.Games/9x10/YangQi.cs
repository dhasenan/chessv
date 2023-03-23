
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
	[Game("Yáng Qí", typeof(Geometry.Rectangular), 9, 10,
		  InventedBy = "Fergus Duniho",
		  Invented = "2001",
		  Tags = "Chess Variant")]
	[Appearance(ColorScheme = "Surrealistic Summer", PieceSet = "Abstract", Player2Color = "255,0,0")]
	public class YangQi: Abstract.Generic9x10
	{
		// *** PIECE TYPES *** //

		public PieceType Cannon;
		public PieceType Vao;


		// *** CONSTRUCTION *** //

		public YangQi(): 
			base
				( /* symmetry = */ new MirrorSymmetry() )
		{
		}


		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Array = "rnbakabnr/1c5c1/p1p1p1p1p/1p1p1p1p1/9/9/1P1P1P1P1/P1P1P1P1P/1C5C1/RNBAKABNR";
			PawnMultipleMove.Value = "Grand";
			PromotionRule.Value = "Replacement";
			EnPassant = true;
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			base.AddPieceTypes();
			King.PSTMidgameForwardness = 0;
			AddPieceType( Rook = new Rook( "Rook", "R", 550, 600 ) );
			AddPieceType( Bishop = new Bishop( "Bishop", "B", 350, 400 ) );
			AddPieceType( Knight = new Knight( "Knight", "N", 275, 275 ) );
			AddPieceType( Cannon = new Cannon( "Cannon", "C", 400, 275 ) );
			AddPieceType( Vao = new Vao( "Arrow", "A", 300, 175 ) );
		}
		#endregion

		#region AddRules
		public override void AddRules()
		{
			base.AddRules();
			AddRule( new Rules.YangQi.YangQiKingSwapRule( King, Pawn ) );
		}
		#endregion

		#region AddEvaluations
		public override void AddEvaluations()
		{
			base.AddEvaluations();

			//	Replace the development evaluation function with an updated one that 
			//	understands that there is no castling and the rooks are already connected
			Evaluations.Grand.GrandChessDevelopmentEvaluation newDevelopentEval = new Evaluations.Grand.GrandChessDevelopmentEvaluation();
			ReplaceEvaluation( FindEvaluation( typeof( Evaluations.DevelopmentEvaluation ) ), newDevelopentEval );
		}
		#endregion

	}
}
