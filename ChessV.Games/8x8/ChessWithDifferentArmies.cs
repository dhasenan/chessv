
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
using System.Collections.Generic;

namespace ChessV.Games
{
  [Game("Chess with Different Armies", typeof(Geometry.Rectangular), 8, 8,
      InventedBy = "Ralph Betza",
      Invented = "1996",
      Tags = "Chess Variant,Popular,Different Armies")]
  [Appearance(ColorScheme = "Orchid")]
  public class ChessWithDifferentArmies : Abstract.Generic8x8
  {
    // *** PIECE TYPES *** //

    //	Colorbound Clobberers
    public PieceType Archbishop;
    public PieceType WarElephant;
    public PieceType Phoenix;
    public PieceType Cleric;

    //	Remarkable Rookies
    public PieceType ShortRook;
    public PieceType Tower;
    public PieceType Lion;
    public PieceType Chancellor;

    //	Nutty Knights
    public PieceType ChargingRook;
    public PieceType NarrowKnight;
    public PieceType ChargingKnight;
    public PieceType Colonel;


    // *** GAME VARIABLES *** //

    [GameVariable] public ChoiceVariable WhiteArmy { get; set; }
    [GameVariable] public ChoiceVariable BlackArmy { get; set; }


    // *** CONSTRUCTION *** //

    public ChessWithDifferentArmies() :
      base
        ( /* symmetry = */ new MirrorSymmetry())
    {
    }


    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      Array = "#{BlackArray}/8/8/8/8/#{WhiteArray}";
      Castling.RemoveChoice("Flexible");
      WhiteArmy = new ChoiceVariable(new string[] { "Fabulous FIDEs", "Colorbound Clobberers", "Remarkable Rookies", "Nutty Knights" });
      BlackArmy = new ChoiceVariable(new string[] { "Fabulous FIDEs", "Colorbound Clobberers", "Remarkable Rookies", "Nutty Knights" });
      PawnDoubleMove = true;
      EnPassant = true;
      Castling.AddChoice("CwDA", "Standard castling with the extra exception to prevent color-bound pieces from changing square colors");
      Castling.Value = "CwDA";
    }
    #endregion

    #region SetOtherVariables
    public override void SetOtherVariables()
    {
      base.SetOtherVariables();
      //	Set BlackArmy array
      if (BlackArmy.Value == "Fabulous FIDEs")
        SetCustomProperty("BlackArray", "rnbqkbnr/pppppppp");
      else if (BlackArmy.Value == "Colorbound Clobberers")
        SetCustomProperty("BlackArray", "cxeakexc/pppppppp");
      else if (BlackArmy.Value == "Remarkable Rookies")
        SetCustomProperty("BlackArray", "stlcklts/pppppppp");
      else if (BlackArmy.Value == "Nutty Knights")
        SetCustomProperty("BlackArray", "rlncknlr/pppppppp");
      //	Set WhiteArmy array
      if (WhiteArmy.Value == "Fabulous FIDEs")
        SetCustomProperty("WhiteArray", "PPPPPPPP/RNBQKBNR");
      else if (WhiteArmy.Value == "Colorbound Clobberers")
        SetCustomProperty("WhiteArray", "PPPPPPPP/CXEAKEXC");
      else if (WhiteArmy.Value == "Remarkable Rookies")
        SetCustomProperty("WhiteArray", "PPPPPPPP/STLCKLTS");
      else if (WhiteArmy.Value == "Nutty Knights")
        SetCustomProperty("WhiteArray", "PPPPPPPP/RLNCKNLR");
      //	Set pawn promotion types
      PromotionTypes = "";
      if (WhiteArmy.Value == "Fabulous FIDEs")
        PromotionTypes += "QRBN";
      if (WhiteArmy.Value == "Colorbound Clobberers")
        PromotionTypes += "ACEX";
      if (WhiteArmy.Value == "Remarkable Rookies")
        PromotionTypes += "CSLT";
      if (WhiteArmy.Value == "Nutty Knights")
        PromotionTypes += "CRNL";
      if (BlackArmy.Value == "Fabulous FIDEs" && WhiteArmy.Value != "Fabulous FIDEs")
        PromotionTypes += "qrbn";
      if (BlackArmy.Value == "Colorbound Clobberers" && WhiteArmy.Value != "Colorbound Clobberers")
        PromotionTypes += "acex";
      if (BlackArmy.Value == "Remarkable Rookies" && WhiteArmy.Value != "Remarkable Rookies")
        PromotionTypes += "cslt";
      if (BlackArmy.Value == "Nutty Knights" && WhiteArmy.Value != "Nutty Knights")
        PromotionTypes += "crnl";
    }
    #endregion

    #region AddPieceTypes
    public override void AddPieceTypes()
    {
      base.AddPieceTypes();

      //	White army
      if (WhiteArmy.Value == "Fabulous FIDEs")
      {
        AddPieceType(Queen = new Queen("Queen", BlackArmy.Value == "Fabulous FIDEs" ? "Q" : "Q/q'", 950, 1000));
        AddPieceType(Rook = new Rook("Rook", BlackArmy.Value == "Fabulous FIDEs" ? "R" : "R/r'", 500, 550));
        AddPieceType(Bishop = new Bishop("Bishop", BlackArmy.Value == "Fabulous FIDEs" ? "B" : "B/b'", 325, 350));
        AddPieceType(Knight = new Knight("Knight", BlackArmy.Value == "Fabulous FIDEs" ? "N" : "N/n'", 325, 325));
      }
      if (WhiteArmy.Value == "Colorbound Clobberers")
      {
        AddPieceType(Archbishop = new Archbishop("Archbishop", BlackArmy.Value == "Colorbound Clobberers" ? "A" : "A/a'", 875, 875));
        AddPieceType(WarElephant = new WarElephant("War Elephant", BlackArmy.Value == "Colorbound Clobberers" ? "E" : "E/e'", 475, 475));
        AddPieceType(Phoenix = new Phoenix("Phoenix", BlackArmy.Value == "Colorbound Clobberers" ? "X" : "X/x'", 315, 315));
        AddPieceType(Cleric = new Cleric("Cleric", BlackArmy.Value == "Colorbound Clobberers" ? "C" : "C/c'", 450, 500));
      }
      if (WhiteArmy.Value == "Remarkable Rookies")
      {
        AddPieceType(Chancellor = new Chancellor("Chancellor", BlackArmy.Value == "Remarkable Rookies" ? "C" : "C/c'", 950, 950));
        AddPieceType(ShortRook = new ShortRook("Short Rook", BlackArmy.Value == "Remarkable Rookies" ? "S" : "S/s'", 400, 425));
        AddPieceType(Tower = new Tower("Tower", BlackArmy.Value == "Remarkable Rookies" ? "T" : "T/t'", 325, 325));
        AddPieceType(Lion = new Lion("Lion", BlackArmy.Value == "Remarkable Rookies" ? "L" : "L/l'", 500, 500));
      }
      if (WhiteArmy.Value == "Nutty Knights")
      {
        AddPieceType(ChargingRook = new ChargingRook("Charging Rook", BlackArmy.Value == "Nutty Knights" ? "R" : "R/r'", 495, 530));
        AddPieceType(NarrowKnight = new NarrowKnight("Lancer", BlackArmy.Value == "Nutty Knights" ? "L" : "L/l'", 325, 325));
        AddPieceType(ChargingKnight = new ChargingKnight("Charging Knight", BlackArmy.Value == "Nutty Knights" ? "N" : "N/n'", 365, 365));
        AddPieceType(Colonel = new Colonel("Colonel", BlackArmy.Value == "Nutty Knights" ? "C" : "C/c'", 950, 950));
      }
      //	Black army
      if (BlackArmy.Value == "Fabulous FIDEs" && WhiteArmy.Value != "Fabulous FIDEs")
      {
        AddPieceType(Queen = new Queen("Queen", "Q'/q", 950, 1050));
        AddPieceType(Rook = new Rook("Rook", "R'/r", 500, 550));
        AddPieceType(Bishop = new Bishop("Bishop", "B'/b", 325, 350));
        AddPieceType(Knight = new Knight("Knight", "N'/n", 325, 325));
      }
      if (BlackArmy.Value == "Colorbound Clobberers" && WhiteArmy.Value != "Colorbound Clobberers")
      {
        AddPieceType(Archbishop = new Archbishop("Archbishop", "A'/a", 875, 875));
        AddPieceType(WarElephant = new WarElephant("War Elephant", "E'/e", 475, 475));
        AddPieceType(Phoenix = new Phoenix("Phoenix", "X'/x", 315, 315));
        AddPieceType(Cleric = new Cleric("Cleric", "C'/c", 450, 500));
      }
      if (BlackArmy.Value == "Remarkable Rookies" && WhiteArmy.Value != "Remarkable Rookies")
      {
        AddPieceType(Chancellor = new Chancellor("Chancellor", "C'/c", 950, 1000));
        AddPieceType(ShortRook = new ShortRook("Short Rook", "S'/s", 400, 425));
        AddPieceType(Tower = new Tower("Tower", "T'/t", 325, 325));
        AddPieceType(Lion = new Lion("Lion", "L'/l", 500, 500));
      }
      if (BlackArmy.Value == "Nutty Knights" && WhiteArmy.Value != "Nutty Knights")
      {
        AddPieceType(ChargingRook = new ChargingRook("Charging Rook", "R'/r", 495, 530));
        AddPieceType(NarrowKnight = new NarrowKnight("Lancer", "L'/l", 325, 325));
        AddPieceType(ChargingKnight = new ChargingKnight("Charging Knight", "N'/n", 365, 365));
        AddPieceType(Colonel = new Colonel("Colonel", "C'/c", 950, 950));
      }
      //	Army adjustment
      if ((WhiteArmy.Value == "Fabulous FIDEs" && BlackArmy.Value == "Remarkable Rookies") ||
        (BlackArmy.Value == "Fabulous FIDEs" && WhiteArmy.Value == "Remarkable Rookies"))
      {
        //	increase the value of the bishops since the Rookies have no piece that moves that way
        Bishop.MidgameValue += 35;
        Bishop.EndgameValue += 35;
      }
    }
    #endregion

    #region AddRules
    public override void AddRules()
    {
      base.AddRules();

      #region Pawn Promotion

      // *** PAWN PROMOTION *** //
      List<PieceType> availablePromotionTypes = new List<PieceType>();
      if (WhiteArmy.Value == "Fabulous FIDEs" || BlackArmy.Value == "Fabulous FIDEs")
      {
        availablePromotionTypes.Add(Queen);
        availablePromotionTypes.Add(Rook);
        availablePromotionTypes.Add(Bishop);
        availablePromotionTypes.Add(Knight);
      }
      if (WhiteArmy.Value == "Colorbound Clobberers" || BlackArmy.Value == "Colorbound Clobberers")
      {
        availablePromotionTypes.Add(Archbishop);
        availablePromotionTypes.Add(WarElephant);
        availablePromotionTypes.Add(Phoenix);
        availablePromotionTypes.Add(Cleric);
      }
      if (WhiteArmy.Value == "Remarkable Rookies" || BlackArmy.Value == "Remarkable Rookies")
      {
        availablePromotionTypes.Add(ShortRook);
        availablePromotionTypes.Add(Tower);
        availablePromotionTypes.Add(Lion);
        availablePromotionTypes.Add(Chancellor);
      }
      if (WhiteArmy.Value == "Nutty Knights" || BlackArmy.Value == "Nutty Knights")
      {
        availablePromotionTypes.Add(ChargingRook);
        availablePromotionTypes.Add(NarrowKnight);
        availablePromotionTypes.Add(ChargingKnight);
        availablePromotionTypes.Add(Colonel);
      }
      AddBasicPromotionRule(Pawn, availablePromotionTypes, loc => loc.Rank == 7);
      #endregion

      #region Castling 

      // *** CASTLING *** //
      if (Castling.Value == "CwDA")
      {
        AddCastlingRule();
        CastlingMove(0, "e1", "g1", "h1", "f1", 'K');
        CastlingMove(1, "e8", "g8", "h8", "f8", 'k');
        if (WhiteArmy.Value == "Colorbound Clobberers")
          CastlingMove(0, "e1", "b1", "a1", "c1", 'Q');
        else
          CastlingMove(0, "e1", "c1", "a1", "d1", 'Q');
        if (BlackArmy.Value == "Colorbound Clobberers")
          CastlingMove(1, "e8", "b8", "a8", "c8", 'q');
        else
          CastlingMove(1, "e8", "c8", "a8", "d8", 'q');
      }
      #endregion
    }
    #endregion

    #region AddEvaluations
    public override void AddEvaluations()
    {
      base.AddEvaluations();

      if (RookTypeEval == null)
      {
        RookTypeEval = new RookTypeEvaluation();
        AddEvaluation(RookTypeEval);
      }
      if (ChargingRook != null)
        RookTypeEval.AddRookOn7thBonus(ChargingRook, King);
      if (ShortRook != null)
      {
        RookTypeEval.AddOpenFileBonus(ShortRook);
        RookTypeEval.AddRookOn7thBonus(ShortRook, King);
      }
      if (Colonel != null)
        RookTypeEval.AddRookOn7thBonus(Colonel, King, 2, 8);
      if (Chancellor != null)
        RookTypeEval.AddRookOn7thBonus(Chancellor, King, 2, 8);

      if (OutpostEval == null)
      {
        OutpostEval = new OutpostEvaluation();
        AddEvaluation(OutpostEval);
      }
      if (ChargingKnight != null)
        OutpostEval.AddOutpostBonus(ChargingKnight);
      if (NarrowKnight != null)
        OutpostEval.AddOutpostBonus(NarrowKnight);
      if (Phoenix != null)
        OutpostEval.AddOutpostBonus(Phoenix);
      if (WarElephant != null)
        OutpostEval.AddOutpostBonus(WarElephant, 10, 2, 5, 5);
      if (Cleric != null)
        OutpostEval.AddOutpostBonus(Cleric, 10, 2, 5, 5);
      if (Lion != null)
        OutpostEval.AddOutpostBonus(Lion, 10, 2, 5, 5);
      if (Bishop != null)
        OutpostEval.AddOutpostBonus(Bishop, 10, 2, 5, 5);
      if (Tower != null)
        OutpostEval.AddOutpostBonus(Tower, 10, 2, 5, 5);
    }
    #endregion

    #region TryCreateAdaptor
    public override EngineGameAdaptor TryCreateAdaptor(EngineConfiguration config)
    {
      //	determine XBoard name based on armies selected
      string xboardname = "";
      if (WhiteArmy.Value == "Fabulous FIDEs")
        xboardname = "fide~";
      else if (WhiteArmy.Value == "Colorbound Clobberers")
        xboardname = "clobberers~";
      else if (WhiteArmy.Value == "Remarkable Rookies")
        xboardname = "rookies~";
      else if (WhiteArmy.Value == "Nutty Knights")
        xboardname = "nutters~";
      if (BlackArmy.Value == "Fabulous FIDEs")
        xboardname += "fide~(cwda)";
      else if (BlackArmy.Value == "Colorbound Clobberers")
        xboardname += "clobberers~(cwda)";
      else if (BlackArmy.Value == "Remarkable Rookies")
        xboardname += "rookies~(cwda)";
      else if (BlackArmy.Value == "Nutty Knights")
        xboardname += "nutters~(cwda)";

      //	FIDEs vs. FIDEs is just Chess
      if (WhiteArmy.Value == "Fabulous FIDEs" && BlackArmy.Value == "Fabulous FIDEs")
        xboardname = "normal";

      if (config.SupportedVariants.Contains(xboardname))
        return new EngineGameAdaptor(xboardname);

      return null;
    }
    #endregion
  }

  [Game("CwDA: FIDEs vs. Clobberers", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "fide~clobberers~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Fabulous FIDEs;BlackArmy=Colorbound Clobberers")]
  [Game("CwDA: Clobberers vs. FIDEs", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "clobberers~fide~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Colorbound Clobberers;BlackArmy=Fabulous FIDEs")]
  [Game("CwDA: FIDEs vs. Rookies", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "fide~rookies~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Fabulous FIDEs;BlackArmy=Remarkable Rookies")]
  [Game("CwDA: Rookies vs. FIDEs", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "rookies~fide~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Remarkable Rookies;BlackArmy=Fabulous FIDEs")]
  [Game("CwDA: FIDEs vs. Nutters", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "fide~nutters~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Fabulous FIDEs;BlackArmy=Nutty Knights")]
  [Game("CwDA: Nutters vs. FIDEs", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "nutters~fide~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Nutty Knights;BlackArmy=Fabulous FIDEs")]
  [Game("CwDA: Nutters vs. Clobberers", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "nutters~clobberers~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Nutty Knights;BlackArmy=Colorbound Clobberers")]
  [Game("CwDA: Clobberers vs. Nutters", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "clobberers~nutters~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Colorbound Clobberers;BlackArmy=Nutty Knights")]
  [Game("CwDA: Nutters vs. Rookies", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "nutters~rookies~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Nutty Knights;BlackArmy=Remarkable Rookies")]
  [Game("CwDA: Rookies vs. Nutters", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "rookies~nutters~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Remarkable Rookies;BlackArmy=Nutty Knights")]
  [Game("CwDA: Rookies vs. Clobberers", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "rookies~clobberers~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Remarkable Rookies;BlackArmy=Colorbound Clobberers")]
  [Game("CwDA: Clobberers vs. Rookies", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "clobberers~rookies~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Colorbound Clobberers;BlackArmy=Remarkable Rookies")]
  [Game("CwDA: Rookies vs. Rookies", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "rookies~rookies~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Remarkable Rookies;BlackArmy=Remarkable Rookies")]
  [Game("CwDA: Clobberers vs. Clobberers", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "clobberers~clobberers~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Colorbound Clobberers;BlackArmy=Colorbound Clobberers")]
  [Game("CwDA: Nutters vs. Nutters", typeof(Geometry.Rectangular), 8, 8,
      XBoardName = "nutters~nutters~(cwda)", Hidden = true,
      Definitions = "WhiteArmy=Nutty Knights;BlackArmy=Nutty Knights")]
  [Appearance(ColorScheme = "Orchid")]
  public class CwDAXBoard : ChessWithDifferentArmies
  {
  }
}
