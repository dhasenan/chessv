
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

using ChessV.Games.Rules;

namespace ChessV.Games
{
	[Game("Symmetric Chess", typeof(Geometry.Rectangular), 9, 8,
		  InventedBy = "Carlos Cetina",
		  Invented = "2014",
		  Tags = "Chess Variant",
		  GameDescription1 = "Symmetric layout with the King flanked by two Queens",
		  GameDescription2 = "Bishops range on opposite colors thanks to the Bishop Conversion Rule")]
	public class SymmetricChess: Abstract.Generic9x8
	{
		// *** GAME VARIABLES *** //

		[GameVariable] public bool UseBishopConversionRule { get; set; }


		// *** CONSTRUCTION *** //

		public SymmetricChess(): 
			base
				( /* symmetry = */ new MirrorSymmetry() )
		{
		}


		// *** INITIALIZATION *** //

		#region SetGameVariables
		public override void SetGameVariables()
		{
			base.SetGameVariables();
			FENFormat = "{array} {current player} {castling} {en-passant} {bishop-conversion} {half-move clock} {turn number}";
			FENStart = "#{Array} w #default #default #default 0 1";
			Array = "rnbqkqbnr/ppppppppp/9/9/9/9/PPPPPPPPP/RNBQKQBNR";
			PawnDoubleMove = true;
			EnPassant = true;
			Castling.Value = "Long";
			PromotionRule.Value = "Standard";
			PromotionTypes = "QRNB";
			UseBishopConversionRule = true;
		}
		#endregion

		#region AddPieceTypes
		public override void AddPieceTypes()
		{
			base.AddPieceTypes();
			AddChessPieceTypes();
		}
		#endregion

		#region AddRules
		public override void AddRules()
		{
			base.AddRules();
			if( UseBishopConversionRule )
				AddRule( new BishopConversionRule( new string[] { "c1", "g1", "c8", "g8" } ) );
		}
		#endregion
	}
}
