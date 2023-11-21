
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

using ChessV.Evaluations;

namespace ChessV.Games
{
  //********************************************************************
  //
  //                       CapablancaChess
  //
  //    This class implements Jose Raul Capablanca's 10x8 variant 
  //    that adds the missing compounds to Chess.  It is used as a 
  //    the base class for a number of similar variants that differ 
  //    mostly in piece arrangement (array) and castling rule.

  [Game("Capablanca Chess", typeof(Geometry.Rectangular), 10, 8,
      XBoardName = "capablanca",
      Invented = "1940",
      InventedBy = "Jose Raul Capablanca",
      Tags = "Chess Variant,Capablanca Variant,Historic,Popular")]
  public class CapablancaChess : Abstract.Generic10x8
  {
    // *** PIECE TYPES *** //

    public PieceType Archbishop;
    public PieceType Chancellor;


    // *** CONSTRUCTION *** //

    public CapablancaChess() :
      base
        ( /* symmetry = */ new MirrorSymmetry())
    {
    }


    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      Array = "rnabqkbcnr/pppppppppp/10/10/10/10/PPPPPPPPPP/RNABQKBCNR";
      PawnDoubleMove = true;
      EnPassant = true;
      Castling.Value = "Standard";
      PromotionRule.Value = "Standard";
      PromotionTypes = "QCARBN";
    }
    #endregion

    #region AddPieceTypes
    public override void AddPieceTypes()
    {
      base.AddPieceTypes();
      AddChessPieceTypes();
      AddPieceType(Archbishop = new Archbishop("Archbishop", "A", 825, 850));
      AddPieceType(Chancellor = new Chancellor("Chancellor", "C", 875, 875));
    }
    #endregion

    #region AddEvaluations
    public override void AddEvaluations()
    {
      base.AddEvaluations();

      if (Chancellor != null && Chancellor.Enabled)
        RookTypeEval.AddRookOn7thBonus(Chancellor, King, 2, 8);

      KingSafetyEvaluation kse = new KingSafetyEvaluation(King, Pawn);
      kse.AddTropism(Queen);
      kse.AddTropism(Chancellor);
      kse.AddTropism(Archbishop);
      AddEvaluation(kse);
    }
    #endregion


    // *** XBOARD ENGINE SUPPORT *** //

    #region TryCreateAdaptor
    public override EngineGameAdaptor TryCreateAdaptor(EngineConfiguration config)
    {
      if (config.SupportedVariants.Contains("capablanca") &&
        config.SupportedFeatures.Contains("setboard") &&
        Castling.Value == "Standard")
      {
        //	It is possible that this engine might be adaptable ...

        //	Verify the basics.  If pawn moves are different, forget it
        if (PawnDoubleMove == true && EnPassant == true)
        {
          EngineGameAdaptor adaptor = new EngineGameAdaptor("capablanca");
          adaptor.IssueSetboard = true;
          //	Do we need to translate piece notations?
          if (Chancellor.Notation[0] != "C")
            adaptor.TranslatePieceNotation(Chancellor.Notation[0], "C");
          if (Chancellor.Notation[1] != "c")
            adaptor.TranslatePieceNotation(Chancellor.Notation[1], "c");
          if (Archbishop.Notation[0] != "A")
            adaptor.TranslatePieceNotation(Archbishop.Notation[0], "A");
          if (Archbishop.Notation[1] != "a")
            adaptor.TranslatePieceNotation(Archbishop.Notation[0], "a");
          //	If the Kings are on the "f" file, we're good with what we have
          if (StartingPieces["f1"] == new GenericPiece(0, King))
            return adaptor;
          //	If the Kings are on the "e" file, we can do it, but we need 
          //	to mirror the board (make the engine think we are playing a 
          //	mirror image.)
          if (StartingPieces["e1"] == new GenericPiece(0, King))
          {
            adaptor.MirrorBoard = true;
            return adaptor;
          }
        }
      }
      return null;
    }
    #endregion

  }

  [Game("Bird's Chess", typeof(Geometry.Rectangular), 10, 8,
      Invented = "1874",
      InventedBy = "Henry Bird",
      Tags = "Chess Variant,Capablanca Variant,Historic",
      Definitions = "Array=rnbcqkabnr/pppppppppp/10/10/10/10/PPPPPPPPPP/RNBCQKABNR")]
  [Game("Carrera's Chess", typeof(Geometry.Rectangular), 10, 8,
      Invented = "1617",
      InventedBy = "Pietro Carrera",
      Tags = "Chess Variant,Capablanca Variant,Historic",
      Definitions = "Castling=None;Array=rcnbkqbnar/pppppppppp/10/10/10/10/PPPPPPPPPP/RCNBKQBNAR")]
  [Game("Embassy Chess", typeof(Geometry.Rectangular), 10, 8,
      Invented = "2005",
      InventedBy = "Kevin Hill",
      Tags = "Chess Variant,Capablanca Variant",
      Definitions = "Array=rnbqkcabnr/pppppppppp/10/10/10/10/PPPPPPPPPP/RNBQKCABNR")]
  [Game("Schoolbook Chess", typeof(Geometry.Rectangular), 10, 8,
      XBoardName = "schoolbook",
      Invented = "2006",
      InventedBy = "Sam Trenholme",
      Tags = "Chess Variant,Capablanca Variant",
      Definitions = "Castling=Flexible;Array=rqnbakbncr/pppppppppp/10/10/10/10/PPPPPPPPPP/RQNBAKBNCR")]
  [Game("Gothic Chess", typeof(Geometry.Rectangular), 10, 8,
      XBoardName = "gothic",
      Invented = "2002",
      InventedBy = "Ed Trice",
      Tags = "Chess Variant,Capablanca Variant",
      Definitions = "Castling=Standard;Array=rnbqckabnr/pppppppppp/10/10/10/10/PPPPPPPPPP/RNBQCKABNR")]
  [Game("Victorian Chess", typeof(Geometry.Rectangular), 10, 8,
      Invented = "2005",
      InventedBy = "David Paulowich;John Kipling Lewis",
      Tags = "Chess Variant,Capablanca Variant",
      Definitions = "Castling=Close-Rook;Array=crnbkabnrq/pppppppppp/10/10/10/10/PPPPPPPPPP/CRNBKABNRQ;PromotionTypes=QCA")]
  [Game("Opti Chess", typeof(Geometry.Rectangular), 10, 8,
      Invented = "2006",
      InventedBy = "Derek Nalls",
      Tags = "Chess Variant,Capablanca Variant",
      Definitions = "Castling=Close-Rook;Array=nrcbqkbarn/pppppppppp/10/10/10/10/PPPPPPPPPP/NRCBQKBARN")]
  [Game("Modern Carrera's Chess", typeof(Geometry.Rectangular), 10, 8,
      XBoardName = "moderncarrera",
      Invented = "1999",
      InventedBy = "Fergus Duniho;Sam Trenholme",
      Tags = "Chess Variant,Capablanca Variant",
      Definitions = "Castling=Standard;Array=ranbqkbncr/pppppppppp/10/10/10/10/PPPPPPPPPP/RANBQKBNCR")]
  [Game("Grotesque Chess", typeof(Geometry.Rectangular), 10, 8,
      Invented = "2004",
      InventedBy = "Fergus Duniho",
      Tags = "Chess Variant,Capablanca Variant",
      Definitions = "Castling=Flexible;Array=rbqnkcnabr/pppppppppp/10/10/10/10/PPPPPPPPPP/RBQNKCNABR")]
  [Game("Ladorean Chess", typeof(Geometry.Rectangular), 10, 8,
      Invented = "2005",
      InventedBy = "Bernhard U. Hermes",
      Tags = "Chess Variant,Capablanca Variant",
      Definitions = "Castling=Flexible;Array=rbqnkancbr/pppppppppp/10/10/10/10/PPPPPPPPPP/RBQNKANCBR")]
  [Game("Univers Chess", typeof(Geometry.Rectangular), 10, 8,
      Invented = "2006",
      InventedBy = "Fergus Duniho;Bruno Violet",
      Tags = "Chess Variant,Capablanca Variant",
      Definitions = "Castling=Flexible;Array=rbncqkanbr/pppppppppp/10/10/10/10/PPPPPPPPPP/RBNCQKANBR")]
  [Appearance(ColorScheme = "Sahara", Game = "Schoolbook Chess")]
  [Appearance(ColorScheme = "Buckingham Green", Game = "Modern Carrera's Chess")]
  [Appearance(ColorScheme = "Cinnamon", PieceSet = "Abstract", Game = "Grotesque Chess")]
  public class CapablancaVariants : CapablancaChess
  {
  }
}
