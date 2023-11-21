
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

using System;

namespace ChessV.Games.Abstract
{
  //**********************************************************************
  //
  //                           Generic11x10
  //
  //    The Generic game classes make it easier to specify games by 
  //    providing functionality common to chess variants.  This class 
  //    extends the Generic__x10 class by adding support for castling 
  //    on an 11x10 board
  [Game("Generic 11x10", typeof(Geometry.Rectangular), 11, 10,
      Template = true)]
  public class Generic11x10 : Generic__x10
  {
    // *** GAME VARIABLES *** //

    [GameVariable] public ChoiceVariable Castling { get; set; }


    // *** CONSTRUCTION *** //

    public Generic11x10
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
      Castling.AddChoice("Wildebeest", "King starting on the f file slides one or more squares, subject to the usual " +
        "restrictions, to castle with the piece in the corner");
      Castling.AddChoice("Close-Rook", "King starting on the f file slides three squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the b or j file");
      Castling.AddChoice("Close-Rook Flexible", "King starting on the f file slides two or more squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the b or j file");
      Castling.AddChoice("2R Standard", "King starting on the f file of the second rank slides three squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the edge");
      Castling.AddChoice("2R Long", "King starting on the f file of the second rank slides four squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the edge");
      Castling.AddChoice("2R Flexible", "King starting on the f file of the second rank slides two or more squares, subject to the usual " +
        "restrictions, to castle with the piece on the edge");
      Castling.AddChoice("2R Wildebeest", "King starting on the f file of the second rank slides one or more squares, subject to the usual " +
        "restrictions, to castle with the piece on the edge");
      Castling.AddChoice("2R Close-Rook", "King starting on the f file of the second rank slides three squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the b or j file");
      Castling.AddChoice("2R Close-Rook Flexible", "King starting on the f file of the second rank slides two or more squares either direction, " +
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
        //	find the king's start square (must be f1)
        GenericPiece WhiteKing = new GenericPiece(0, CastlingType);
        GenericPiece BlackKing = new GenericPiece(1, CastlingType);
        if (StartingPieces["f1"] != WhiteKing || StartingPieces["f10"] != BlackKing)
          throw new Exception("Can't enable castling rule because King does not start on a supported square");

        //	STANDARD CASTLING - King slides three squares and corner piece jumps over to adjacent square
        if (Castling.Value == "Standard")
        {
          AddCastlingRule();
          CastlingMove(0, "f1", "i1", "k1", "h1", 'K');
          CastlingMove(0, "f1", "c1", "a1", "d1", 'A');
          CastlingMove(1, "f10", "i10", "k10", "h10", 'k');
          CastlingMove(1, "f10", "c10", "a10", "d10", 'a');
        }
        //	LONG CASTLING - King slides four squares and the corner piece jumps over to adjacent square
        else if (Castling.Value == "Long")
        {
          AddCastlingRule();
          CastlingMove(0, "f1", "j1", "k1", "i1", 'K');
          CastlingMove(0, "f1", "b1", "a1", "c1", 'A');
          CastlingMove(1, "f10", "j10", "k10", "i10", 'k');
          CastlingMove(1, "f10", "b10", "a10", "c10", 'a');
        }
        //	FLEXIBLE Castilng - King starts in center and can slide 2, 3 or 4 squares 
        //	toward the castling piece on the corner square which hops to the other side
        else if (Castling.Value == "Flexible")
        {
          AddFlexibleCastlingRule();
          FlexibleCastlingMove(0, "f1", "h1", "k1", 'K');
          FlexibleCastlingMove(0, "f1", "d1", "a1", 'A');
          FlexibleCastlingMove(1, "f10", "h10", "k10", 'k');
          FlexibleCastlingMove(1, "f10", "d10", "a10", 'a');
        }
        //	CLOSE-ROOK CASTLING - Castling pieces are on b1/b10 and j1/j10 rather than in the 
        //	corners.  King slides three squares and castling piece jumps over to adjacent square.
        else if (Castling.Value == "Close-Rook")
        {
          AddCastlingRule();
          CastlingMove(0, "f1", "i1", "j1", "h1", 'J');
          CastlingMove(0, "f1", "c1", "b1", "d1", 'B');
          CastlingMove(1, "f10", "i10", "j10", "h10", 'j');
          CastlingMove(1, "f10", "c10", "b10", "d10", 'b');
        }
        //	WILDEBEEST Castling - the castling rule from Wildebeest Chess.  The King slides 
        //	one to four spaces toward the rook and the rook jumps over to the adjacent square
        else if (Castling.Value == "Wildebeest")
        {
          AddFlexibleCastlingRule();
          FlexibleCastlingMove(0, "f1", "g1", "k1", 'K');
          FlexibleCastlingMove(0, "f1", "e1", "a1", 'A');
          FlexibleCastlingMove(1, "f10", "g10", "k10", 'k');
          FlexibleCastlingMove(1, "f10", "e10", "a10", 'a');
        }
        //	CLOSE-ROOK FLEXIBLE - King starts in center and can slide 2 or 3 squares 
        //	toward the castling piece on the b or j file which hops to the other side
        else if (Castling.Value == "Close-Rook Flexible")
        {
          AddFlexibleCastlingRule();
          FlexibleCastlingMove(0, "f1", "h1", "j1", 'J');
          FlexibleCastlingMove(0, "f1", "d1", "b1", 'B');
          FlexibleCastlingMove(1, "f10", "h10", "j10", 'j');
          FlexibleCastlingMove(1, "f10", "d10", "b10", 'b');
        }

        //	2R STANDARD CASTLING - King slides three squares and edge piece jumps over to adjacent square
        else if (Castling.Value == "2R Standard")
        {
          AddCastlingRule();
          CastlingMove(0, "f2", "i2", "k2", "h2", 'K');
          CastlingMove(0, "f2", "c2", "a2", "d2", 'A');
          CastlingMove(1, "f9", "i9", "k9", "h9", 'k');
          CastlingMove(1, "f9", "c9", "a9", "d9", 'a');
        }
        //	2R LONG CASTLING - King slides four squares and the edge piece jumps over to adjacent square
        else if (Castling.Value == "2R Long")
        {
          AddCastlingRule();
          CastlingMove(0, "f2", "j2", "k2", "i2", 'K');
          CastlingMove(0, "f2", "b2", "a2", "c2", 'A');
          CastlingMove(1, "f9", "j9", "k9", "i9", 'k');
          CastlingMove(1, "f9", "b9", "a9", "c9", 'a');
        }
        //	2R FLEXIBLE Castilng - King starts in center and can slide 2, 3 or 4 squares 
        //	toward the castling piece on the edge square which hops to the other side
        else if (Castling.Value == "2R Flexible")
        {
          AddFlexibleCastlingRule();
          FlexibleCastlingMove(0, "f2", "h2", "k2", 'K');
          FlexibleCastlingMove(0, "f2", "d2", "a2", 'A');
          FlexibleCastlingMove(1, "f9", "h9", "k9", 'k');
          FlexibleCastlingMove(1, "f9", "d9", "a9", 'a');
        }
        //	2R CLOSE-ROOK CASTLING - Castling pieces are on b2/b9 and j2/j9 rather than in the 
        //	corners.  King slides three squares and castling piece jumps over to adjacent square.
        else if (Castling.Value == "2R Close-Rook")
        {
          AddCastlingRule();
          CastlingMove(0, "f2", "i2", "j2", "h2", 'J');
          CastlingMove(0, "f2", "c2", "b2", "d2", 'B');
          CastlingMove(1, "f9", "i9", "j9", "h9", 'j');
          CastlingMove(1, "f9", "c9", "b9", "d9", 'b');
        }
        //	2R WILDEBEEST Castling - the castling rule from Wildebeest Chess.  The King slides 
        //	one to four spaces toward the rook and the rook jumps over to the adjacent square
        else if (Castling.Value == "2R Wildebeest")
        {
          AddFlexibleCastlingRule();
          FlexibleCastlingMove(0, "f2", "g2", "k2", 'K');
          FlexibleCastlingMove(0, "f2", "e2", "a2", 'A');
          FlexibleCastlingMove(1, "f9", "g9", "k9", 'k');
          FlexibleCastlingMove(1, "f9", "e9", "a9", 'a');
        }
        //	2R CLOSE-ROOK FLEXIBLE - Castling pieces are on b2/b9 and j2/j9 rather than in the 
        //	corners.  King slides 2 or 3 squares and castling piece jumps over to adjacent square.
        else if (Castling.Value == "2R Close-Rook Flexible")
        {
          AddFlexibleCastlingRule();
          FlexibleCastlingMove(0, "f2", "h2", "j2", 'J');
          FlexibleCastlingMove(0, "f2", "d2", "b2", 'B');
          FlexibleCastlingMove(1, "f9", "h9", "j9", 'j');
          FlexibleCastlingMove(1, "f9", "d9", "b9", 'b');
        }
      }
      #endregion
    }
    #endregion


    // *** OPERATIONS *** //

    public void AddChessPieceTypes()
    {
      AddPieceType(Queen = new Queen("Queen", "Q", 1025, 1250));
      AddPieceType(Rook = new Rook("Rook", "R", 550, 600));
      AddPieceType(Bishop = new Bishop("Bishop", "B", 375, 375));
      AddPieceType(Knight = new Knight("Knight", "N", 275, 275));
    }
  }
}
