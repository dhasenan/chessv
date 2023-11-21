
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
  //                           Generic12x10
  //
  //    The Generic game classes make it easier to specify games by 
  //    providing functionality common to chess variants.  This class 
  //    extends the Generic__x10 class by adding support for castling 
  //    on an 12x10 board
  [Game("Generic 12x10", typeof(Geometry.Rectangular), 12, 10,
      Template = true)]
  public class Generic12x10 : Generic__x10
  {
    // *** GAME VARIABLES *** //

    [GameVariable] public ChoiceVariable Castling { get; set; }


    // *** CONSTRUCTION *** //

    public Generic12x10
      (Symmetry symmetry) :
        base
          ( /* num files = */ 12,
            /* symmetry = */ symmetry)
    {
    }


    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      Castling = new ChoiceVariable();
      Castling.AddChoice("3-3", "King starting on f or g file slides three squares either direction, " +
        "subject to the usual restrictions, to castle with the piece in the corner");
      Castling.AddChoice("3-4", "King starting on f or g file slides three squares when castling short " +
        "or four when castling long, subject to the usual restrictions, to castle with the piece in the corner");
      Castling.AddChoice("4-4", "King starting on f or g file slides four squares either direction, " +
        "subject to the usual restrictions, to castle with the piece in the corner");
      Castling.AddChoice("4-5", "King starting on f or g file slides four squares when castling short " +
        "or five when castling long, subject to the usual restrictions, to castle with the piece in the corner");
      Castling.AddChoice("Close-Rook 2-2", "King starting on f or g file slides two squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the b or k file");
      Castling.AddChoice("Close-Rook 2-3", "King starting on f or g file slides two squares when castling short " +
        "or three when castling long, subject to the usual restrictions, to castle with the piece on the b or k file");
      Castling.AddChoice("Close-Rook 3-3", "King starting on f or g file slides three squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the b or k file");
      Castling.AddChoice("Close-Rook 3-4", "King starting on f or g file slides three squares when castling short " +
        "or four when castling long, subject to the usual restrictions, to castle with the piece on the b or k file");
      Castling.AddChoice("Flexible", "King starting on f or g file slides two or more squares, subject to the usual " +
        "restrictions, to castle with the piece in the corner");
      Castling.AddChoice("Close-Rook Flexible", "King starting on f or g file slides two or more squares, subject to the usual " +
        "restrictions, to castle with the piece on the b or k file");
      Castling.AddChoice("2R 3-3", "King starting on f or g file of the second rank slides three squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the edge");
      Castling.AddChoice("2R 3-4", "King starting on f or g file of the second rank slides three squares when castling short " +
        "or four when castling long, subject to the usual restrictions, to castle with the piece on the edge");
      Castling.AddChoice("2R 4-4", "King starting on f or g file of the second rank slides four squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the edge");
      Castling.AddChoice("2R 4-5", "King starting on f or g file of the second rank slides four squares when castling short " +
        "or five when castling long, subject to the usual restrictions, to castle with the piece on the edge");
      Castling.AddChoice("2R Close-Rook 2-2", "King starting on f or g file of the second rank slides two squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the b or k file");
      Castling.AddChoice("2R Close-Rook 2-3", "King starting on f or g file of the second rank slides two squares when castling short " +
        "or three when castling long, subject to the usual restrictions, to castle with the piece on the b or k file");
      Castling.AddChoice("2R Close-Rook 3-3", "King starting on f or g file of the second rank slides three squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the b or k file");
      Castling.AddChoice("2R Close-Rook 3-4", "King starting on f or g file of the second rank slides three squares when castling short " +
        "or four when castling long, subject to the usual restrictions, to castle with the piece on the b or k file");
      Castling.AddChoice("2R Flexible", "King starting on f or g file of the second rank slides two or more squares, subject to the usual " +
        "restrictions, to castle with the piece on the edge");
      Castling.AddChoice("2R Close-Rook Flexible", "King starting on f or g file of the second rank slides two or more squares, subject to the usual " +
        "restrictions, to castle with the piece on the b or k file");
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
        //	find the king's start square (must be f1 or g1)
        GenericPiece WhiteKing = new GenericPiece(0, CastlingType);
        GenericPiece BlackKing = new GenericPiece(1, CastlingType);
        string whiteKingSquare = null;
        string blackKingSquare = null;
        if (StartingPieces["f1"] == WhiteKing)
          whiteKingSquare = "f1";
        else if (StartingPieces["g1"] == WhiteKing)
          whiteKingSquare = "g1";
        else if (StartingPieces["f2"] == WhiteKing)
          whiteKingSquare = "f2";
        else if (StartingPieces["g2"] == WhiteKing)
          whiteKingSquare = "g2";
        if (StartingPieces["f10"] == BlackKing)
          blackKingSquare = "f10";
        else if (StartingPieces["g10"] == BlackKing)
          blackKingSquare = "g10";
        else if (StartingPieces["f9"] == BlackKing)
          blackKingSquare = "f9";
        else if (StartingPieces["g9"] == BlackKing)
          blackKingSquare = "g9";
        else
          throw new Exception("Can't enable castling rule because King does not start on a supported square");

        //	FLEXIBLE CASTLING - King slides two or more squares (but must stop short of the 
        //	corner) and the corner piece jumps over to adjacent square
        if (Castling.Value == "Flexible")
        {
          AddFlexibleCastlingRule();
          if (whiteKingSquare == "g1")
          {
            FlexibleCastlingMove(0, "g1", "i1", "l1", 'L');
            FlexibleCastlingMove(0, "g1", "e1", "a1", 'A');
          }
          else if (whiteKingSquare == "f1")
          {
            FlexibleCastlingMove(0, "f1", "d1", "a1", 'A');
            FlexibleCastlingMove(0, "f1", "h1", "l1", 'L');
          }
          else
            throw new Exception("Can't enable castling rule with type 'Flexible' because King does not start on a supported square");
          if (blackKingSquare == "g10")
          {
            FlexibleCastlingMove(1, "g10", "i10", "l10", 'l');
            FlexibleCastlingMove(1, "g10", "e10", "a10", 'a');
          }
          else if (blackKingSquare == "f10")
          {
            FlexibleCastlingMove(1, "f10", "d10", "a10", 'a');
            FlexibleCastlingMove(1, "f10", "h10", "l10", 'l');
          }
          else
            throw new Exception("Can't enable castling rule with type 'Flexible' because King does not start on a supported square");
        }
        //	2R FLEXIBLE CASTLING - King slides two or more squares (but must stop short of the 
        //	corner) and the edge piece jumps over to adjacent square
        else if (Castling.Value == "2R Flexible")
        {
          AddFlexibleCastlingRule();
          if (whiteKingSquare == "g2")
          {
            FlexibleCastlingMove(0, "g2", "i2", "l2", 'L');
            FlexibleCastlingMove(0, "g2", "e2", "a2", 'A');
          }
          else if (whiteKingSquare == "f2")
          {
            FlexibleCastlingMove(0, "f2", "d2", "a2", 'A');
            FlexibleCastlingMove(0, "f2", "h2", "l2", 'L');
          }
          else
            throw new Exception("Can't enable castling rule with type '2R Flexible' because King does not start on a supported square");
          if (blackKingSquare == "g9")
          {
            FlexibleCastlingMove(1, "g9", "i9", "l9", 'l');
            FlexibleCastlingMove(1, "g9", "e9", "a9", 'a');
          }
          else if (blackKingSquare == "f9")
          {
            FlexibleCastlingMove(1, "f9", "d9", "a9", 'a');
            FlexibleCastlingMove(1, "f9", "h9", "l9", 'l');
          }
          else
            throw new Exception("Can't enable castling rule with type '2R Flexible' because King does not start on a supported square");
        }
        //	CLOSE-ROOK FLEXIBLE CASTLING - King slides two or more squares (but must stop short of the 
        //	b or k file) and the piece on that file jumps over to adjacent square
        else if (Castling.Value == "Close-Rook Flexible")
        {
          AddFlexibleCastlingRule();
          if (whiteKingSquare == "g1")
          {
            FlexibleCastlingMove(0, "g1", "i1", "k1", 'K');
            FlexibleCastlingMove(0, "g1", "e1", "b1", 'B');
          }
          else if (whiteKingSquare == "f1")
          {
            FlexibleCastlingMove(0, "f1", "d1", "b1", 'B');
            FlexibleCastlingMove(0, "f1", "h1", "k1", 'K');
          }
          else
            throw new Exception("Can't enable castling rule with type 'Close-Rook Flexible' because King does not start on a supported square");
          if (blackKingSquare == "g10")
          {
            FlexibleCastlingMove(1, "g10", "i10", "k10", 'k');
            FlexibleCastlingMove(1, "g10", "e10", "b10", 'b');
          }
          else if (blackKingSquare == "f10")
          {
            FlexibleCastlingMove(1, "f10", "d10", "b10", 'b');
            FlexibleCastlingMove(1, "f10", "h10", "k10", 'k');
          }
          else
            throw new Exception("Can't enable castling rule with type 'Close-Rook Flexible' because King does not start on a supported square");
        }
        //	2R CLOSE-ROOK FLEXIBLE CASTLING - King on the second rank slides two or more squares (but must stop 
        //	short of the b or k file) and the piece on that file jumps over to adjacent square
        else if (Castling.Value == "2R Close-Rook Flexible")
        {
          AddFlexibleCastlingRule();
          if (whiteKingSquare == "g2")
          {
            FlexibleCastlingMove(0, "g2", "i2", "k2", 'K');
            FlexibleCastlingMove(0, "g2", "e2", "b2", 'B');
          }
          else if (whiteKingSquare == "f2")
          {
            FlexibleCastlingMove(0, "f2", "d2", "b2", 'B');
            FlexibleCastlingMove(0, "f2", "h2", "k2", 'K');
          }
          else
            throw new Exception("Can't enable castling rule with type 'Close-Rook Flexible' because King does not start on a supported square");
          if (blackKingSquare == "g9")
          {
            FlexibleCastlingMove(1, "g9", "i9", "k9", 'k');
            FlexibleCastlingMove(1, "g9", "e9", "b9", 'b');
          }
          else if (blackKingSquare == "f9")
          {
            FlexibleCastlingMove(1, "f9", "d9", "b9", 'b');
            FlexibleCastlingMove(1, "f9", "h9", "k9", 'k');
          }
          else
            throw new Exception("Can't enable castling rule with type 'Close-Rook Flexible' because King does not start on a supported square");
        }
        //	ALL OTHER CASTLING TYPES
        else
        {
          //	handle implementation of all other castling options by algorithm
          string rankPlayer0 = Castling.Value.IndexOf("2R") >= 0 ? "2" : "1";
          string rankPlayer1 = Castling.Value.IndexOf("2R") >= 0 ? "9" : "10";
          char file0Player0 = Castling.Value.IndexOf("Close-Rook") >= 0 ? 'B' : 'A';
          char file1Player0 = Castling.Value.IndexOf("Close-Rook") >= 0 ? 'K' : 'L';
          char file0Player1 = Castling.Value.IndexOf("Close-Rook") >= 0 ? 'b' : 'a';
          char file1Player1 = Castling.Value.IndexOf("Close-Rook") >= 0 ? 'k' : 'l';
          int shortDistance = Convert.ToInt32(Castling.Value.Substring(Castling.Value.Length - 3, 1));
          int longDistance = Convert.ToInt32(Castling.Value.Substring(Castling.Value.Length - 1, 1));
          AddCastlingRule();
          if (whiteKingSquare[0] == 'g')
          {
            CastlingMove(0, "g" + rankPlayer0, (char)('g' + shortDistance) + rankPlayer0, file1Player1.ToString() + rankPlayer0, (char)('g' + shortDistance - 1) + rankPlayer0, file1Player0);
            CastlingMove(0, "g" + rankPlayer0, (char)('g' - longDistance) + rankPlayer0, file0Player1.ToString() + rankPlayer0, (char)('g' - longDistance + 1) + rankPlayer0, file0Player0);
          }
          else
          {
            CastlingMove(0, "f" + rankPlayer0, (char)('f' + longDistance) + rankPlayer0, file1Player1.ToString() + rankPlayer0, (char)('f' + longDistance - 1) + rankPlayer0, file1Player0);
            CastlingMove(0, "f" + rankPlayer0, (char)('f' - shortDistance) + rankPlayer0, file0Player1.ToString() + rankPlayer0, (char)('f' - shortDistance + 1) + rankPlayer0, file0Player0);
          }
          if (blackKingSquare[0] == 'g')
          {
            CastlingMove(1, "g" + rankPlayer1, (char)('g' + shortDistance) + rankPlayer1, file1Player1.ToString() + rankPlayer1, (char)('g' + shortDistance - 1) + rankPlayer1, file1Player1);
            CastlingMove(1, "g" + rankPlayer1, (char)('g' - longDistance) + rankPlayer1, file0Player1.ToString() + rankPlayer1, (char)('g' - longDistance + 1) + rankPlayer1, file0Player1);
          }
          else
          {
            CastlingMove(1, "f" + rankPlayer1, (char)('f' + longDistance) + rankPlayer1, file1Player1.ToString() + rankPlayer1, (char)('f' + longDistance - 1) + rankPlayer1, file1Player1);
            CastlingMove(1, "f" + rankPlayer1, (char)('f' - shortDistance) + rankPlayer1, file0Player1.ToString() + rankPlayer1, (char)('f' - shortDistance + 1) + rankPlayer1, file0Player1);
          }
        }
      }
      #endregion
    }
    #endregion


    // *** OPERATIONS *** //

    public void AddChessPieceTypes()
    {
      AddPieceType(Queen = new Queen("Queen", "Q", 1000, 1100));
      AddPieceType(Rook = new Rook("Rook", "R", 550, 600));
      AddPieceType(Bishop = new Bishop("Bishop", "B", 350, 400));
      AddPieceType(Knight = new Knight("Knight", "N", 275, 275));
    }
  }
}
