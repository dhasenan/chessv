
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
	[Game("ArchCourier Chess", typeof(Geometry.Rectangular), 12, 8,
		  Invented = "2006",
		  InventedBy = "Eric V. Greenwood",
		  Tags = "Chess Variant")]
	public class ArchCourierChess: Abstract.Generic12x8
	{
		// *** PIECE TYPES *** //

		public PieceType Guard;
		public PieceType Centaur;
		public PieceType Squirrel;
		public PieceType DragonKing;
		public PieceType DragonHorse;


		// *** CONSTRUCTION *** //

		public ArchCourierChess():
			base
				( /* symmetry = */ new MirrorSymmetry() )
		{
		}

		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			Array = "rhabdqksbchr/ppppppgppppp/6p5/12/12/6P5/PPPPPPGPPPPP/RHABDQKSBCHR";
			PromotionRule.Value = "Replacement";
			PawnDoubleMove = true;
			EnPassant = true;
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			base.AddPieceTypes();
			AddChessPieceTypes();
			Knight.Name = "Horse";
			Knight.SetNotation( "H" );

			AddPieceType( Guard = new General( "Guard", "G", 325, 325 ) );
			AddPieceType( Centaur = new Centaur( "Duke", "D", 625, 625 ) );
			AddPieceType( Squirrel = new Squirrel( "Squirrel", "S", 575, 575 ) );
			AddPieceType( DragonKing = new DragonKing( "Crowned Rook", "C", 700, 700 ) );
			AddPieceType( DragonHorse = new DragonHorse( "ArchCourier", "A", 500, 550 ) );
		}
		#endregion

		#region AddEvaluations
		public override void AddEvaluations()
		{
			base.AddEvaluations();

			if( DragonKing != null && DragonKing.Enabled )
				RookTypeEval.AddRookOn7thBonus( DragonKing, King, 2, 8 );

			if( Squirrel != null && Squirrel.Enabled )
				OutpostEval.AddOutpostBonus( Squirrel, 10, 2, 5, 5 );
		}
		#endregion
	}
}
