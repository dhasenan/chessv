
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
  //                           Generic10x10
  //
  //    The Generic game classes make it easier to specify games by 
  //    providing functionality common to chess variants.  This class 
  //    extends the Generic__x10 class by adding support for a 
  //    variety of different castling rules commonly used on 10x10 board

  [Game("Generic 10x10", typeof(Geometry.Rectangular), 10, 10,
      Template = true)]
  public class Generic10x10 : Generic__x10
  {
    // *** GAME VARIABLES *** //

    [GameVariable] public ChoiceVariable Castling { get; set; }


    // *** CONSTRUCTION *** //

    public Generic10x10
      (Symmetry symmetry) :
        base
          ( /* num files = */ 10,
            /* symmetry = */ symmetry)
    {
    }


    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      Castling = new ChoiceVariable();
      Castling.AddChoice("Standard", "King starting on e or f file slides three squares either direction, " +
        "subject to the usual restrictions, to castle with the piece in the corner");
      Castling.AddChoice("Long", "King starting on e or f file slides three squares when castling short " +
        "or four when castling long, subject to the usual restrictions, to castle with the piece in the corner");
      Castling.AddChoice("Flexible", "King starting on e or f file slides two or more squares, subject to the usual " +
        "restrictions, to castle with the piece in the corner");
      Castling.AddChoice("Close-Rook", "King starting on e or f file slides two squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the b or i file");
      Castling.AddChoice("Close-Rook Flexible", "King starting on e or f file slides two or more squares, " +
        "subject to the usual restrictions, to castle with the piece on the b or i file");
      Castling.AddChoice("2R Standard", "King starting on e or f file on the second rank slides three squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the edge");
      Castling.AddChoice("2R Long", "King starting on e or f file on the second rank slides three squares when castling short " +
        "or four when castling long, subject to the usual restrictions, to castle with the piece on the edge");
      Castling.AddChoice("2R Flexible", "King starting on e or f file on the second rank slides two or more squares, subject to the usual " +
        "restrictions, to castle with the piece on the edge");
      Castling.AddChoice("2R Close-Rook", "King starting on e or f file on the second rank slides two squares either direction, " +
        "subject to the usual restrictions, to castle with the piece on the b or i file on the second rank");
      Castling.AddChoice("2R Close-Rook Flexible", "King starting on e or f file on the second rank slides two or more squares, " +
        "subject to the usual restrictions, to castle with the piece on the b or i file");
      Castling.AddChoice("None", "No castling");
      Castling.Value = "None";
      PromotionRule.AddChoice("Grand");
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
        #region First rank castling rules
        if (Castling.Value[0] != '2')
        {
          //	Adding castling rule is somewhat complicated because there are a number of different forms 
          //	of castilng and we have to accomodate the King on either e1 or f1 as well as the fact that 
          //	the FEN notation, (typically KQkq), for King-side or Queen-side might be inappropriate,  
          //	in which case we use Shredder-FEN notation where the priv char is the file of the rook

          //	find the king's start square (must be e1 or f1)
          GenericPiece WhiteKing = new GenericPiece(0, CastlingType);
          GenericPiece BlackKing = new GenericPiece(1, CastlingType);
          string kingSquare;
          if (StartingPieces["e1"] == WhiteKing)
            kingSquare = "e1";
          else if (StartingPieces["f1"] == WhiteKing)
            kingSquare = "f1";
          else
            throw new Exception("Can't enable castling rule because King does not start on a supported square");

          //	Use Shredder-FEN notation?  We will use Shredder-FEN notation unless the Kings start on f1/f8 
          //	and a piece with a notation of Q starts on either e1 or d1 (for consistency with 10x8) 
          //	and the castling is with pieces in the corners (e.g., not Close-Rook castling.)
          bool shredderNotation = true;
          if (kingSquare == "f1" && StartingPieces["f10"] != null && StartingPieces["f10"] == BlackKing &&
            ((StartingPieces["d1"] != null && StartingPieces["d1"].PieceType.Notation[0] == "Q") ||
             (StartingPieces["e1"] != null && StartingPieces["e1"].PieceType.Notation[0] == "Q")))
            shredderNotation = false;

          //	STANDARD CASTLING - King slides three squares and corner piece jumps over to adjacent square
          if (Castling.Value == "Standard")
          {
            AddCastlingRule();
            if (kingSquare == "f1")
            {
              CastlingMove(0, "f1", "i1", "j1", "h1", shredderNotation ? 'J' : 'K');
              CastlingMove(0, "f1", "c1", "a1", "d1", shredderNotation ? 'A' : 'Q');
              CastlingMove(1, "f10", "i10", "j10", "h10", shredderNotation ? 'j' : 'k');
              CastlingMove(1, "f10", "c10", "a10", "d10", shredderNotation ? 'a' : 'q');
            }
            else
            {
              CastlingMove(0, "e1", "b1", "a1", "c1", 'A');
              CastlingMove(0, "e1", "h1", "j1", "i1", 'J');
              CastlingMove(1, "e10", "b10", "a10", "c10", 'a');
              CastlingMove(1, "e10", "h10", "j10", "i10", 'j');
            }
          }
          //	LONG CASTLING - King slides three squares to closer corner or four squares to 
          //	farther corner and the corner piece jumps over to adjacent square
          else if (Castling.Value == "Long")
          {
            AddCastlingRule();
            if (kingSquare == "f1")
            {
              CastlingMove(0, "f1", "i1", "j1", "h1", shredderNotation ? 'J' : 'K');
              CastlingMove(0, "f1", "b1", "a1", "c1", shredderNotation ? 'A' : 'Q');
              CastlingMove(1, "f10", "i10", "j10", "h10", shredderNotation ? 'j' : 'k');
              CastlingMove(1, "f10", "b10", "a10", "c10", shredderNotation ? 'a' : 'q');
            }
            else
            {
              CastlingMove(0, "e1", "b1", "a1", "c1", 'A');
              CastlingMove(0, "e1", "i1", "j1", "h1", 'J');
              CastlingMove(1, "e10", "b10", "a10", "c10", 'a');
              CastlingMove(1, "e10", "i10", "j10", "h10", 'j');
            }
          }
          //	FLEXIBLE CASTLING - King slides two or more squares (but must stop short of the 
          //	corner) and the corner piece jumps over to adjacent square
          else if (Castling.Value == "Flexible")
          {
            AddFlexibleCastlingRule();
            if (kingSquare == "f1")
            {
              FlexibleCastlingMove(0, "f1", "h1", "j1", shredderNotation ? 'J' : 'K');
              FlexibleCastlingMove(0, "f1", "d1", "a1", shredderNotation ? 'A' : 'Q');
              FlexibleCastlingMove(1, "f10", "h10", "j10", shredderNotation ? 'j' : 'k');
              FlexibleCastlingMove(1, "f10", "e10", "a10", shredderNotation ? 'a' : 'q');
            }
            else
            {
              FlexibleCastlingMove(0, "e1", "c1", "a1", 'A');
              FlexibleCastlingMove(0, "e1", "g1", "j1", 'J');
              FlexibleCastlingMove(1, "e10", "c10", "a10", 'a');
              FlexibleCastlingMove(1, "e10", "g10", "j10", 'j');
            }
          }
          //	CLOSE ROOK CASTLING - Castling pieces are on b1/b10 and i1/i10 rather than in the 
          //	corners.  King slides two squares and castling piece jumps over to adjacent square.
          else if (Castling.Value == "Close-Rook")
          {
            AddCastlingRule();
            if (kingSquare == "f1")
            {
              CastlingMove(0, "f1", "h1", "i1", "g1", 'I');
              CastlingMove(0, "f1", "d1", "b1", "e1", 'B');
              CastlingMove(1, "f10", "h10", "i10", "g10", 'i');
              CastlingMove(1, "f10", "d10", "b10", "e10", 'b');
            }
            else
            {
              CastlingMove(0, "e1", "c1", "b1", "d1", 'B');
              CastlingMove(0, "e1", "g1", "i1", "f1", 'I');
              CastlingMove(1, "e10", "c10", "b10", "d10", 'b');
              CastlingMove(1, "e10", "g10", "i10", "f10", 'i');
            }
          }
          //	CLOSE-ROOK FLEXIBLE - King slides two or more squares to castle with the  
          //	piece on the b or i file (which jumps to the other side)
          else if (Castling.Value == "Close-Rook Flexible")
          {
            AddFlexibleCastlingRule();
            if (kingSquare == "f1")
            {
              FlexibleCastlingMove(0, "f1", "h1", "i1", 'I');
              FlexibleCastlingMove(0, "f1", "d1", "b1", 'B');
              FlexibleCastlingMove(1, "f10", "h10", "i10", 'i');
              FlexibleCastlingMove(1, "f10", "e10", "b10", 'b');
            }
            else
            {
              FlexibleCastlingMove(0, "e1", "c1", "b1", 'B');
              FlexibleCastlingMove(0, "e1", "g1", "i1", 'I');
              FlexibleCastlingMove(1, "e10", "c10", "b10", 'b');
              FlexibleCastlingMove(1, "e10", "g10", "i10", 'i');
            }
          }
        }
        #endregion

        #region Second rank castling rules
        else    // *** SECOND-RANK CASTLING TYPES *** //
        {
          //	These are the eqivalents of the castling types above, but shifted onto the second rank 

          //	find the king's start square (must be e2 or f2)
          GenericPiece WhiteKing = new GenericPiece(0, CastlingType);
          GenericPiece BlackKing = new GenericPiece(1, CastlingType);
          string kingSquare;
          if (StartingPieces["e2"] == WhiteKing)
            kingSquare = "e2";
          else if (StartingPieces["f2"] == WhiteKing)
            kingSquare = "f2";
          else
            throw new Exception("Can't enable castling rule because King does not start on a supported square");

          //	Use Shredder-FEN notation?  We will use Shredder-FEN notation unless the Kings start on f1/f8 
          //	and a piece with a notation of Q starts on either e1 or d1 (for consistency with 10x8) 
          //	and the castling is with pieces in the corners (e.g., not Close-Rook castling.)
          bool shredderNotation = true;
          if (kingSquare == "f2" && StartingPieces["f9"] != null && StartingPieces["f9"] == BlackKing &&
            ((StartingPieces["d2"] != null && StartingPieces["d2"].PieceType.Notation[0] == "Q") ||
             (StartingPieces["e2"] != null && StartingPieces["e2"].PieceType.Notation[0] == "Q")))
            shredderNotation = false;

          //	2R STANDARD CASTLING - King on second rank slides three squares and corner piece jumps over to adjacent square
          if (Castling.Value == "2R Standard")
          {
            AddCastlingRule();
            if (kingSquare == "f2")
            {
              CastlingMove(0, "f2", "i2", "j2", "h2", shredderNotation ? 'J' : 'K');
              CastlingMove(0, "f2", "c2", "a2", "d2", shredderNotation ? 'A' : 'Q');
              CastlingMove(1, "f9", "i9", "j9", "h9", shredderNotation ? 'j' : 'k');
              CastlingMove(1, "f9", "c9", "a9", "d9", shredderNotation ? 'a' : 'q');
            }
            else
            {
              CastlingMove(0, "e2", "b2", "a2", "c2", 'A');
              CastlingMove(0, "e2", "h2", "j2", "i2", 'J');
              CastlingMove(1, "e9", "b9", "a9", "c9", 'a');
              CastlingMove(1, "e9", "h9", "j9", "i9", 'j');
            }
          }
          //	2R LONG CASTLING - King on second rank slides two squares to closer corner or three squares to 
          //	farther corner and the corner piece jumps over to adjacent square
          else if (Castling.Value == "2R Long")
          {
            AddCastlingRule();
            if (kingSquare == "f2")
            {
              CastlingMove(0, "f2", "i2", "j2", "h2", shredderNotation ? 'J' : 'K');
              CastlingMove(0, "f2", "b2", "a2", "c2", shredderNotation ? 'A' : 'Q');
              CastlingMove(1, "f9", "i9", "j9", "h9", shredderNotation ? 'j' : 'k');
              CastlingMove(1, "f9", "b9", "a9", "c9", shredderNotation ? 'a' : 'q');
            }
            else
            {
              CastlingMove(0, "e2", "b2", "a2", "c2", 'A');
              CastlingMove(0, "e2", "i2", "j2", "h2", 'J');
              CastlingMove(1, "e9", "b9", "a9", "c9", 'a');
              CastlingMove(1, "e9", "i9", "j9", "h9", 'j');
            }
          }
          //	2R FLEXIBLE CASTLING - King on second rank slides two or more squares (but must stop short of the 
          //	corner) and the corner piece jumps over to adjacent square
          else if (Castling.Value == "2R Flexible")
          {
            AddFlexibleCastlingRule();
            if (kingSquare == "f2")
            {
              FlexibleCastlingMove(0, "f2", "h2", "j2", shredderNotation ? 'J' : 'K');
              FlexibleCastlingMove(0, "f2", "d2", "a2", shredderNotation ? 'A' : 'Q');
              FlexibleCastlingMove(1, "f9", "h9", "j9", shredderNotation ? 'j' : 'k');
              FlexibleCastlingMove(1, "f9", "d9", "a9", shredderNotation ? 'a' : 'q');
            }
            else
            {
              FlexibleCastlingMove(0, "e2", "c2", "a2", 'A');
              FlexibleCastlingMove(0, "e2", "g2", "j2", 'J');
              FlexibleCastlingMove(1, "e9", "c9", "a9", 'a');
              FlexibleCastlingMove(1, "e9", "g9", "j9", 'j');
            }
          }
          //	2R CLOSE ROOK CASTLING - Castling pieces are on b1/b0 and i1/i0 rather than in the 
          //	corners.  King on second rank slides two squares and castling piece jumps over to adjacent square.
          else if (Castling.Value == "2R Close-Rook")
          {
            AddCastlingRule();
            if (kingSquare == "f2")
            {
              CastlingMove(0, "f2", "h2", "i2", "g2", 'I');
              CastlingMove(0, "f2", "d2", "b2", "e2", 'B');
              CastlingMove(1, "f9", "h9", "i9", "g9", 'i');
              CastlingMove(1, "f9", "d9", "b9", "e9", 'b');
            }
            else
            {
              CastlingMove(0, "e2", "c2", "b2", "d2", 'B');
              CastlingMove(0, "e2", "g2", "i2", "f2", 'I');
              CastlingMove(1, "e9", "c9", "b9", "d9", 'b');
              CastlingMove(1, "e9", "g9", "i9", "f9", 'i');
            }
          }
          //	2R CLOSE-ROOK FLEXIBLE - King on second rank slides two or more squares to castle with the  
          //	piece on the b or i file (which jumps to the other side)
          else if (Castling.Value == "2R Close-Rook Flexible")
          {
            AddFlexibleCastlingRule();
            if (kingSquare == "f2")
            {
              FlexibleCastlingMove(0, "f2", "h2", "i2", 'I');
              FlexibleCastlingMove(0, "f2", "d2", "b2", 'B');
              FlexibleCastlingMove(1, "f9", "h9", "i9", 'i');
              FlexibleCastlingMove(1, "f9", "e9", "b9", 'b');
            }
            else
            {
              FlexibleCastlingMove(0, "e2", "c2", "b2", 'B');
              FlexibleCastlingMove(0, "e2", "g2", "i2", 'I');
              FlexibleCastlingMove(1, "e9", "c9", "b9", 'b');
              FlexibleCastlingMove(1, "e9", "g9", "i9", 'i');
            }
          }
        }
        #endregion
      }
      #endregion

      // *** PAWN PROMOTION *** //

      if (PromotionRule.Value == "Grand")
        AddPromoteByReplacementRule(Pawn, loc => loc.Rank == 9 ? Rules.PromotionOption.MustPromote :
          (loc.Rank == 7 || loc.Rank == 8 ? Rules.PromotionOption.CanPromote : Rules.PromotionOption.CannotPromote));
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
        //				if( Bishop != null && Bishop.Enabled )
        //					OutpostEval.AddOutpostBonus( Bishop, 10, 2, 5, 5 );
        AddEvaluation(OutpostEval);
      }

      //	Rook-type Evaluations (rook-mover on open file 
      //	and rook-mover on 9th ranks with enemy king on 10th)

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
      AddPieceType(Queen = new Queen("Queen", "Q", 1000, 1100));
      AddPieceType(Rook = new Rook("Rook", "R", 550, 600));
      AddPieceType(Bishop = new Bishop("Bishop", "B", 400, 400));
      AddPieceType(Knight = new Knight("Knight", "N", 275, 275));
    }
  }
}
