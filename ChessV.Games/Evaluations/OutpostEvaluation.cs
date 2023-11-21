
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

using ChessV.Games;
using System.Collections.Generic;

namespace ChessV.Evaluations
{
  public class OutpostEvaluation : Evaluation
  {
    // *** INITIALIZATION *** //

    #region Initialize
    public override void Initialize(Game game)
    {
      base.Initialize(game);
      chessGame = (Games.Abstract.GenericChess)game;

      //	determine outpost squares for each player
      int nSquares = game.Board.NumSquares;
      outpostSquares = new BitBoard[2] { new BitBoard(nSquares), new BitBoard(nSquares) };
      for (int sq = 0; sq < nSquares; sq++)
      {
        if (game.Board.InLargeCenter(sq) != 0)
        {
          if (game.Board.SquareToLocation(sq).Rank >= game.Board.NumRanks / 2)
            outpostSquares[0].SetBit(sq);
          if (game.Board.SquareToLocation(sq).Rank <= game.Board.NumRanks / 2)
            outpostSquares[1].SetBit(sq);
        }
      }
    }
    #endregion

    #region PostInitialize
    public override void PostInitialize()
    {
      base.PostInitialize();

      PieceType[] pieceTypes;
      int pieceTypeCount = game.GetPieceTypes(out pieceTypes);
      for (int nPieceType = 0; nPieceType < pieceTypeCount; nPieceType++)
        if (pieceTypes[nPieceType] is Pawn)
          pawnTypeNumber = pieceTypes[nPieceType].TypeNumber;

      pawnSupportDirectionNumbers = new int[2, 2];
      pawnSupportDirectionNumbers[0, 0] = game.GetDirectionNumber(new Direction(-1, 1));
      pawnSupportDirectionNumbers[0, 1] = game.GetDirectionNumber(new Direction(-1, -1));
      pawnSupportDirectionNumbers[1, 0] = game.PlayerDirection(1, pawnSupportDirectionNumbers[0, 0]);
      pawnSupportDirectionNumbers[1, 1] = game.PlayerDirection(1, pawnSupportDirectionNumbers[0, 1]);
    }
    #endregion

    #region AddOutpostBonus
    public void AddOutpostBonus(PieceType pieceType)
    {
      if (outpostPieceList == null)
        outpostPieceList = new List<OutpostPiece>();
      outpostPieceList.Add(new OutpostPiece(pieceType));
    }

    public void AddOutpostBonus
      (PieceType pieceType,
        int midgameBonus,
        int endgameBonus,
        int midgamePawnProtectedExtraBonus,
        int endgamePawnProtectedExtraBonus)
    {
      if (outpostPieceList == null)
        outpostPieceList = new List<OutpostPiece>();
      OutpostPiece outpost = new OutpostPiece(pieceType);
      outpost.MidgameBonus = midgameBonus;
      outpost.EndgameBonus = endgameBonus;
      outpost.MidgamePawnProtectedExtraBonus = midgamePawnProtectedExtraBonus;
      outpost.EndgamePawnProtectedExtraBonus = endgamePawnProtectedExtraBonus;
      outpostPieceList.Add(outpost);
    }
    #endregion


    // *** EVENT HANDLERS *** //

    #region ReleaseMemoryAllocations
    public override void ReleaseMemoryAllocations()
    {
      base.ReleaseMemoryAllocations();
      outpostPieceList = null;
      outpostSquares = null;
      pawnSupportDirectionNumbers = null;
    }
    #endregion

    #region AdjustEvaluation
    public override void AdjustEvaluation(ref int midgameEval, ref int endgameEval)
    {
      foreach (var bonus in outpostPieceList)
      {
        #region Player 0
        //	handle pieces of player 0
        BitBoard p0 = board.GetPieceTypeBitboard(0, bonus.PieceType.TypeNumber) & outpostSquares[0];
        while (p0)
        {
          int square = p0.ExtractLSB();
          Location location = board.SquareToLocation(square);
          if ((location.File == 0 || chessGame.PawnStructureInfo.GetBackPawnRank(1, location.File - 1) <= location.Rank) &&
            (location.File == board.NumFiles - 1 || chessGame.PawnStructureInfo.GetBackPawnRank(1, location.File + 1) <= location.Rank))
          {
            midgameEval += bonus.MidgameBonus;
            endgameEval += bonus.EndgameBonus;
            //	Is the piece protected by a friendly pawn?
            int sq1 = board.NextSquare(pawnSupportDirectionNumbers[0, 0], square);
            int sq2 = board.NextSquare(pawnSupportDirectionNumbers[0, 1], square);
            if ((sq1 >= 0 && board[sq1] != null && board[sq1].Player == 0 && board[sq1].PieceType.TypeNumber == pawnTypeNumber) ||
              (sq2 >= 0 && board[sq2] != null && board[sq2].Player == 0 && board[sq2].PieceType.TypeNumber == pawnTypeNumber))
            {
              midgameEval += bonus.MidgamePawnProtectedExtraBonus;
              endgameEval += bonus.EndgamePawnProtectedExtraBonus;
            }
          }
        }
        #endregion

        #region Player 1
        //	handle pieces of player 1
        BitBoard p1 = board.GetPieceTypeBitboard(1, bonus.PieceType.TypeNumber) & outpostSquares[1];
        while (p1)
        {
          int square = p1.ExtractLSB();
          Location location = board.SquareToLocation(square);
          if ((location.File == 0 || chessGame.PawnStructureInfo.GetBackPawnRank(0, location.File - 1) >= location.Rank) &&
            (location.File == board.NumFiles - 1 || chessGame.PawnStructureInfo.GetBackPawnRank(0, location.File + 1) >= location.Rank))
          {
            midgameEval -= bonus.MidgameBonus;
            endgameEval -= bonus.EndgameBonus;
            //	Is the piece protected by a friendly pawn?
            int sq1 = board.NextSquare(pawnSupportDirectionNumbers[1, 0], square);
            int sq2 = board.NextSquare(pawnSupportDirectionNumbers[1, 1], square);
            if ((sq1 >= 0 && board[sq1] != null && board[sq1].Player == 1 && board[sq1].PieceType.TypeNumber == pawnTypeNumber) ||
              (sq2 >= 0 && board[sq2] != null && board[sq2].Player == 1 && board[sq2].PieceType.TypeNumber == pawnTypeNumber))
            {
              midgameEval -= bonus.MidgamePawnProtectedExtraBonus;
              endgameEval -= bonus.EndgamePawnProtectedExtraBonus;
            }
          }
        }
        #endregion
      }
    }
    #endregion

    #region GetNotesForPieceType
    public override void GetNotesForPieceType(PieceType type, List<string> notes)
    {
      foreach (var outpost in outpostPieceList)
        if (outpost.PieceType == type)
          notes.Add("outpost bonus");
    }
    #endregion


    // *** PROTECTED MEMBER DATA *** //

    //	Store types and weights for types with outpost bonuses
    protected List<OutpostPiece> outpostPieceList;

    //	Squares that pieces must occupy to get the outpost bonus
    protected BitBoard[] outpostSquares;

    //	The type number of the Pawn piece type
    protected int pawnTypeNumber;

    //	Direction numbers, per player, of the pawn attack directions
    protected int[,] pawnSupportDirectionNumbers;

    //	The GenericChess Game object
    protected Games.Abstract.GenericChess chessGame;
  }

  #region OutpostPiece Class
  public class OutpostPiece
  {
    public PieceType PieceType { get; set; }
    public int MidgameBonus { get; set; }
    public int MidgamePawnProtectedExtraBonus { get; set; }
    public int EndgameBonus { get; set; }
    public int EndgamePawnProtectedExtraBonus { get; set; }

    public OutpostPiece(PieceType type)
    {
      PieceType = type;
      MidgameBonus = 15;
      EndgameBonus = 5;
      MidgamePawnProtectedExtraBonus = 10;
      EndgamePawnProtectedExtraBonus = 5;
    }
  }
  #endregion
}
