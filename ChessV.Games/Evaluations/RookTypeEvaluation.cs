
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

using System.Collections.Generic;

namespace ChessV.Evaluations
{
  public class RookTypeEvaluation : Evaluation
  {
    // *** CONSTRUCTION *** //

    public RookTypeEvaluation()
    {
      openFileList = new List<OpenFileBonuses>();
      rookOn7thList = new List<RookOn7thBonuses>();
    }


    // *** INITIALIZATION *** //

    #region Initialize
    public override void Initialize(Game game)
    {
      base.Initialize(game);
      chessGame = (Games.Abstract.GenericChess)game;
    }
    #endregion

    #region AddOpenFileBonus
    public void AddOpenFileBonus(PieceType pieceType)
    {
      openFileList.Add(new OpenFileBonuses(pieceType));
    }

    public void AddOpenFileBonus
      (PieceType pieceType,
        int midgameSemiOpenBonus,
        int endgameSemiOpenBonus,
        int midgameFullOpenExtraBonus,
        int endgameFullOpenExtraBonus)
    {
      OpenFileBonuses openFileBonuses = new OpenFileBonuses(pieceType);
      openFileBonuses.MidgameSemiOpenBonus = midgameSemiOpenBonus;
      openFileBonuses.EndgameSemiOpenBonus = endgameSemiOpenBonus;
      openFileBonuses.MidgameFullOpenExtraBonus = midgameFullOpenExtraBonus;
      openFileBonuses.EndgameFullOpenExtraBonus = endgameFullOpenExtraBonus;
      openFileList.Add(openFileBonuses);
    }
    #endregion

    #region AddRookOn7thBonus
    public void AddRookOn7thBonus(PieceType rookType, PieceType kingType)
    {
      rookOn7thList.Add(new RookOn7thBonuses(rookType, kingType));
    }

    public void AddRookOn7thBonus
      (PieceType rookType,
        PieceType kingType,
        int midgameBonus,
        int endgameBonus)
    {
      RookOn7thBonuses bonus = new RookOn7thBonuses(rookType, kingType);
      bonus.MidgameBonus = midgameBonus;
      bonus.EndgameBonus = endgameBonus;
      rookOn7thList.Add(bonus);
    }
    #endregion


    // *** EVENT HANDLERS *** //

    #region ReleaseMemoryAllocations
    public override void ReleaseMemoryAllocations()
    {
      base.ReleaseMemoryAllocations();
      openFileList = null;
      rookOn7thList = null;
    }
    #endregion

    #region AdjustEvaluation
    public override void AdjustEvaluation(ref int midgameEval, ref int endgameEval)
    {
      #region Open File Bonuses
      if (openFileList != null)
        foreach (OpenFileBonuses bonus in openFileList)
        {
          #region Player 0
          //	handle pieces of player 0
          BitBoard p0 = board.GetPieceTypeBitboard(0, bonus.PieceType.TypeNumber);
          while (p0)
          {
            int square = p0.ExtractLSB();
            int file = board.GetFile(square);
            if (chessGame.PawnStructureInfo.GetBackPawnRank(0, file) == 15)
            {
              //	apply semi-open file bonus
              midgameEval += bonus.MidgameSemiOpenBonus;
              endgameEval += bonus.EndgameSemiOpenBonus;
              if (chessGame.PawnStructureInfo.GetBackPawnRank(1, file) == 0)
              {
                //	apply fully-open file extra bonus
                midgameEval += bonus.MidgameFullOpenExtraBonus;
                endgameEval += bonus.EndgameFullOpenExtraBonus;
              }
            }
          }
          #endregion

          #region Player 1
          //	handle pieces of player 1
          BitBoard p1 = board.GetPieceTypeBitboard(1, bonus.PieceType.TypeNumber);
          while (p1)
          {
            int square = p1.ExtractLSB();
            int file = board.GetFile(square);
            if (chessGame.PawnStructureInfo.GetBackPawnRank(1, file) == 0)
            {
              //	apply semi-open file bonus
              midgameEval -= bonus.MidgameSemiOpenBonus;
              endgameEval -= bonus.EndgameSemiOpenBonus;
              if (chessGame.PawnStructureInfo.GetBackPawnRank(0, file) == 15)
              {
                //	apply fully-open file extra bonus
                midgameEval -= bonus.MidgameFullOpenExtraBonus;
                endgameEval -= bonus.EndgameFullOpenExtraBonus;
              }
            }
          }
          #endregion
        }
      #endregion

      #region Rook On 7th Bonuses
      if (rookOn7thList != null)
        foreach (RookOn7thBonuses bonus in rookOn7thList)
        {
          #region Player 0
          //	handle pieces of player 0
          int kingRank = board.GetRank(board.GetPieceTypeBitboard(1, bonus.KingType.TypeNumber).LSB);
          if (kingRank == board.NumRanks - 1)
          {
            BitBoard p0 = board.GetPieceTypeBitboard(0, bonus.RookType.TypeNumber);
            while (p0)
            {
              int square = p0.ExtractLSB();
              int rank = board.GetRank(square);
              if (rank == board.NumRanks - 2)
              {
                midgameEval += bonus.MidgameBonus;
                endgameEval += bonus.EndgameBonus;
              }
            }
          }
          #endregion

          #region Player 1
          //	handle pieces of player 1
          kingRank = board.GetRank(board.GetPieceTypeBitboard(0, bonus.KingType.TypeNumber).LSB);
          if (kingRank == 0)
          {
            BitBoard p1 = board.GetPieceTypeBitboard(1, bonus.RookType.TypeNumber);
            while (p1)
            {
              int square = p1.ExtractLSB();
              int rank = board.GetRank(square);
              if (rank == 1)
              {
                midgameEval -= bonus.MidgameBonus;
                endgameEval -= bonus.EndgameBonus;
              }
            }
          }
          #endregion
        }
      #endregion
    }
    #endregion

    #region GetNotesForPieceType
    public override void GetNotesForPieceType(PieceType type, List<string> notes)
    {
      foreach (var filebonus in openFileList)
        if (filebonus.PieceType == type)
          notes.Add("open file bonus");

      foreach (var rankbonus in rookOn7thList)
        if (rankbonus.RookType == type)
          notes.Add("trap king on back rank bonus");
    }
    #endregion


    // *** PROTECTED MEMBER DATA *** //

    //	Store types and weights receiving open file bonuses
    protected List<OpenFileBonuses> openFileList;

    //	Store types and weights for rook-on-seventh bonuses
    protected List<RookOn7thBonuses> rookOn7thList;

    //	The GenericChess Game object
    protected Games.Abstract.GenericChess chessGame;
  }

  #region OpenFileBonuses class
  public class OpenFileBonuses
  {
    public PieceType PieceType { get; set; }
    public int MidgameSemiOpenBonus { get; set; }
    public int EndgameSemiOpenBonus { get; set; }
    public int MidgameFullOpenExtraBonus { get; set; }
    public int EndgameFullOpenExtraBonus { get; set; }

    public OpenFileBonuses(PieceType type)
    {
      PieceType = type;
      MidgameSemiOpenBonus = 8;
      EndgameSemiOpenBonus = 4;
      MidgameFullOpenExtraBonus = 8;
      EndgameFullOpenExtraBonus = 4;
    }
  }
  #endregion

  #region RookOn7thBonuses class
  public class RookOn7thBonuses
  {
    public PieceType RookType { get; set; }
    public PieceType KingType { get; set; }
    public int MidgameBonus { get; set; }
    public int EndgameBonus { get; set; }

    public RookOn7thBonuses(PieceType rookType, PieceType kingType)
    {
      RookType = rookType;
      KingType = kingType;
      MidgameBonus = 5;
      EndgameBonus = 10;
    }
  }
  #endregion
}
