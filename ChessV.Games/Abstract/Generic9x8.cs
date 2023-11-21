
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
  //                           Generic9x8
  //
  //    The Generic game classes make it easier to specify games by 
  //    providing functionality common to chess variants.  This class 
  //    extends the Generic__x8 class by adding support for a 
  //    variety of different castling rules commonly used on 9x8 board

  [Game("Generic 9x8", typeof(Geometry.Rectangular), 9, 8,
      Template = true)]
  public class Generic9x8 : Generic__x8
  {
    // *** GAME VARIABLES *** //

    [GameVariable] public ChoiceVariable Castling { get; set; }


    // *** CONSTRUCTION *** //

    public Generic9x8
      (Symmetry symmetry) :
        base
          ( /* num files = */ 9,
            /* symmetry = */ symmetry)
    {
    }


    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      Castling = new ChoiceVariable();
      Castling.AddChoice("Standard", "King starting on the e file slides two squares either direction, " +
        "subject to the usual restrictions, to castle with the piece in the corner");
      Castling.AddChoice("Long", "King starting on the e file slides three squares either direction, " +
        "subject to the usual restrictions, to castle with the piece in the corner");
      Castling.AddChoice("Flexible", "King starting on the e file slides two or more squares, subject to the usual " +
        "restrictions, to castle with the piece in the corner");
      Castling.AddChoice("Close-Rook", "King starting on the e file slides two squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the b or h file");
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
        //	The Kings must start on e1 and e10.  Ensure that is the case.
        GenericPiece WhiteKing = new GenericPiece(0, CastlingType);
        GenericPiece BlackKing = new GenericPiece(1, CastlingType);
        if (StartingPieces["e1"] != WhiteKing || StartingPieces["e8"] != BlackKing)
          throw new Exception("Can't enable castling rule because King does not start on a supported square");

        //	NOTE: We always use Shredder notation for castling privs.  With the King 
        //	in the center of the board, it no longer makes sense to say King-side 
        //	or Queen-side

        //	STANDARD CASTLING - King slides two squares and corner piece jumps over to adjacent square
        if (Castling.Value == "Standard")
        {
          AddCastlingRule();
          CastlingMove(0, "e1", "c1", "a1", "d1", 'A');
          CastlingMove(0, "e1", "g1", "i1", "f1", 'I');
          CastlingMove(1, "e8", "c8", "a8", "d8", 'a');
          CastlingMove(1, "e8", "g8", "i8", "f8", 'i');
        }
        //	LONG CASTLING - King slides three squares and corner piece jumps over to adjacent square
        else if (Castling.Value == "Long")
        {
          AddCastlingRule();
          CastlingMove(0, "e1", "b1", "a1", "c1", 'A');
          CastlingMove(0, "e1", "h1", "i1", "g1", 'I');
          CastlingMove(1, "e8", "b8", "a8", "c8", 'a');
          CastlingMove(1, "e8", "h8", "i8", "g8", 'i');
        }
        //	FLEXIBLE CASTLING - King slides two or more squares (but must stop short of the 
        //	corner) and the corner piece jumps over to adjacent square
        else if (Castling.Value == "Flexible")
        {
          AddFlexibleCastlingRule();
          FlexibleCastlingMove(0, "e1", "c1", "a1", 'A');
          FlexibleCastlingMove(0, "e1", "g1", "i1", 'I');
          FlexibleCastlingMove(1, "e8", "c8", "a8", 'a');
          FlexibleCastlingMove(1, "e8", "g8", "i8", 'i');
        }
        //	CLOSE-ROOK CASTLING - Castling pieces are on b1/b8 and h1/h8 rather than in the 
        //	corners.  King slides two squares and castling piece jumps over to adjacent square.
        else if (Castling.Value == "Close-Rook")
        {
          AddCastlingRule();
          CastlingMove(0, "e1", "c1", "b1", "d1", 'B');
          CastlingMove(0, "e1", "g1", "h1", "f1", 'H');
          CastlingMove(1, "e8", "c8", "b8", "d8", 'b');
          CastlingMove(1, "e8", "g8", "h8", "f8", 'h');
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
      CheckmateRule rule = (CheckmateRule)FindRule(typeof(CheckmateRule));
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
      AddPieceType(Rook = new Rook("Rook", "R", 500, 550));
      AddPieceType(Bishop = new Bishop("Bishop", "B", 325, 350));
      AddPieceType(Knight = new Knight("Knight", "N", 325, 325));
      AddPieceType(Queen = new Queen("Queen", "Q", 950, 1000));
    }
  }
}
