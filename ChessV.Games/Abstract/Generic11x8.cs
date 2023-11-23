
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
using ChessV.Games.Rules;
using System;

namespace ChessV.Games.Abstract
{
  //**********************************************************************
  //
  //                           Generic11x8
  //
  //    The Generic game classes make it easier to specify games by 
  //    providing functionality common to chess variants.  This class 
  //    extends the Generic__x8 class by adding support for a 
  //    variety of different castling rules commonly used on 11x8 board

  [Game("Generic 11x8", typeof(Geometry.Rectangular), 11, 8,
      Template = true)]
  public class Generic11x8 : Generic__x8
  {
    // *** GAME VARIABLES *** //

    [GameVariable] public ChoiceVariable Castling { get; set; }


    // *** CONSTRUCTION *** //

    public Generic11x8
      (Symmetry symmetry) :
        base
          ( /* num files = */ 11,
            /* symmetry = */ symmetry)
    {
    }


    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      Castling = new ChoiceVariable();
      Castling.AddChoice("Standard", "King starting on the f file slides three squares either direction, " +
        "subject to the usual restrictions, to castle with the piece in the corner");
      Castling.AddChoice("Long", "King starting on the f file slides four squares either direction, " +
        "subject to the usual restrictions, to castle with the piece in the corner");
      Castling.AddChoice("Flexible", "King starting on the f file slides two or more squares, subject to the usual " +
        "restrictions, to castle with the piece in the corner");
      Castling.AddChoice("Close-Rook", "King starting on the f file slides three squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the b or j file");
      Castling.AddChoice("Close-Rook Flexible", "King starting on the f file slides two or more squares, " +
        "subject to the usual restrictions, to castle with the piece on the b or j file");
      Castling.AddChoice("None", "No castling");
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
      if (Castling.Choices.IndexOf(Castling.Value) < Castling.Choices.IndexOf("None"))
      {
        //	find the king's start square, confirm the kings are centered on f1/f8
        GenericPiece WhiteKing = new GenericPiece(0, CastlingType);
        GenericPiece BlackKing = new GenericPiece(1, CastlingType);
        if (StartingPieces["f1"] != WhiteKing || StartingPieces["f8"] != BlackKing)
          throw new Exception("Can't enable castling rule because King does not start on a supported square");

        //	STANDARD CASTLING - King slides three squares and corner piece jumps over to adjacent square
        if (Castling.Value == "Standard")
        {
          AddCastlingRule();
          CastlingMove(0, "f1", "i1", "k1", "h1", 'K');
          CastlingMove(0, "f1", "c1", "a1", "d1", 'A');
          CastlingMove(1, "f8", "i8", "k8", "h8", 'k');
          CastlingMove(1, "f8", "c8", "a8", "d8", 'a');
        }
        //	LONG CASTLING - King slides four squares and the corner piece jumps over to adjacent square
        else if (Castling.Value == "Long")
        {
          AddCastlingRule();
          CastlingMove(0, "f1", "j1", "k1", "i1", 'K');
          CastlingMove(0, "f1", "b1", "a1", "c1", 'A');
          CastlingMove(1, "f8", "j8", "k8", "i8", 'k');
          CastlingMove(1, "f8", "b8", "a8", "c8", 'a');
        }
        //	FLEXIBLE CASTLING - King slides two or more squares (but must stop short of the 
        //	corner) and the corner piece jumps over to adjacent square
        else if (Castling.Value == "Flexible")
        {
          AddFlexibleCastlingRule();
          FlexibleCastlingMove(0, "f1", "h1", "k1", 'K');
          FlexibleCastlingMove(0, "f1", "d1", "a1", 'A');
          FlexibleCastlingMove(1, "f8", "h8", "k8", 'k');
          FlexibleCastlingMove(1, "f8", "d8", "a8", 'a');
        }
        //	CLOSE-ROOK CASTLING - Castling pieces are on b1/b8 and j1/j8 rather than in the 
        //	corners.  King slides three squares and castling piece jumps over to adjacent square.
        else if (Castling.Value == "Close-Rook")
        {
          AddCastlingRule();
          CastlingMove(0, "f1", "i1", "j1", "h1", 'J');
          CastlingMove(0, "f1", "c1", "b1", "d1", 'B');
          CastlingMove(1, "f8", "i8", "j8", "h8", 'j');
          CastlingMove(1, "f8", "c8", "b8", "d8", 'b');
        }
        //	CLOSE-ROOK FLEXIBLE - King slides two or more squares to castle with the piece on  
        //	the b or j file which then jumps over to adjacent square
        else if (Castling.Value == "Flexible")
        {
          AddFlexibleCastlingRule();
          FlexibleCastlingMove(0, "f1", "h1", "j1", 'J');
          FlexibleCastlingMove(0, "f1", "d1", "b1", 'B');
          FlexibleCastlingMove(1, "f8", "h8", "j8", 'j');
          FlexibleCastlingMove(1, "f8", "d8", "b8", 'b');
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
      if ((Knight != null && Knight.Enabled) ||
        (Bishop != null && Bishop.Enabled))
      {
        OutpostEval = new OutpostEvaluation();
        if (Knight != null && Knight.Enabled)
          OutpostEval.AddOutpostBonus(Knight);
        if (Bishop != null && Bishop.Enabled)
          OutpostEval.AddOutpostBonus(Bishop, 10, 2, 5, 5);
        AddEvaluation(OutpostEval);
      }

      //	Rook-type Evaluations (rook-mover on open file 
      //	and rook-mover on 7th ranks with enemy king on 8th)

      //	Do we have a royal king?
      CheckmateRule rule = (CheckmateRule)FindRule(typeof(CheckmateRule), true);
      bool royalKing = rule != null && King != null && King.Enabled && rule.RoyalPieceType == King;

      if ((Rook != null && Rook.Enabled) ||
        (Queen != null && Queen.Enabled && royalKing))
      {
        RookTypeEval = new RookTypeEvaluation();
        if (Rook != null && Rook.Enabled)
        {
          RookTypeEval.AddOpenFileBonus(Rook);
          if (royalKing)
            RookTypeEval.AddRookOn7thBonus(Rook, King);
        }
        if (Queen != null && Queen.Enabled && royalKing)
          RookTypeEval.AddRookOn7thBonus(Queen, King, 2, 8);
        AddEvaluation(RookTypeEval);
      }
    }
    #endregion


    // *** OPERATIONS *** //

    public void AddChessPieceTypes()
    {
      AddPieceType(Queen = new Queen("Queen", "Q", 1000, 1050));
      AddPieceType(Rook = new Rook("Rook", "R", 525, 550));
      AddPieceType(Bishop = new Bishop("Bishop", "B", 350, 350));
      AddPieceType(Knight = new Knight("Knight", "N", 285, 285));
    }
  }
}
