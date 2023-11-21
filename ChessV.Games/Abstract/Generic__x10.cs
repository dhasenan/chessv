
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

namespace ChessV.Games.Abstract
{
  //**********************************************************************
  //
  //                           Generic__x10
  //
  //    The Generic game classes make it easier to specify games by 
  //    providing functionality common to chess variants.  This class 
  //    is for chess variants on boards with 10 ranks and is used as a 
  //    base class for other generic boards such as 10x10.
  //
  //    It derives from the GenericChess class which provides the 
  //    rules for a game with Pawns and a Royal King, as well as the 
  //    50-move and draw-by-repetition rules.
  //
  //    This class adds optional support for a number of different 
  //    initial pawn multiple-move rules and En Passant.

  [Game("Generic x10", typeof(Geometry.Rectangular), 10,
      Template = true)]
  public class Generic__x10 : GenericChess
  {
    // *** GAME VARIABLES *** //

    [GameVariable] public ChoiceVariable PawnMultipleMove { get; set; }


    // *** CONSTRUCTION *** //

    public Generic__x10
      (int nFiles,               // number of files on the board
        Symmetry symmetry) :      // symmetry determining board mirroring/rotation
                base(nFiles, /* num ranks = */ 10, symmetry)
    {
    }


    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      PawnMultipleMove = new ChoiceVariable();
      PawnMultipleMove.AddChoice("None", "Pawns can never move more than a single space");
      PawnMultipleMove.AddChoice("Double", "Pawns on the second rank can move two spaces");
      PawnMultipleMove.AddChoice("Triple", "Pawns on the second rank can move up to three spaces");
      PawnMultipleMove.AddChoice("Great", "Pawns on the second or third rank can move two spaces");
      PawnMultipleMove.AddChoice("Grand", "Pawns on the third rank can move two spaces");
      PawnMultipleMove.AddChoice("Wildebeest", "Pawns on the second rank can move up to three spaces and pawns on the third rank can move two spaces");
      PawnMultipleMove.AddChoice("Unicorn", "Pawns on the second rank can move two spaces as well as pawns on the third rank of the centermost file(s)");
      PawnMultipleMove.AddChoice("Fast Pawn", "Pawns can move two spaces from any location");
      PawnMultipleMove.AddChoice("Custom", "Indicates a custom rule implemented by derived class");
      PawnMultipleMove.Value = "None";
    }
    #endregion

    #region AddPieceTypes
    public override void AddPieceTypes()
    {
      base.AddPieceTypes();
      Pawn.PSTMidgameInSmallCenter = 6;
      Pawn.PSTMidgameInLargeCenter = 0;
    }
    #endregion

    #region AddRules
    public override void AddRules()
    {
      base.AddRules();

      // *** PAWN MULTIPLE MOVE *** //
      if (PawnMultipleMove.Value == "Double")
      {
        MoveCapability doubleMove = new MoveCapability();
        doubleMove.MinSteps = 2;
        doubleMove.MaxSteps = 2;
        doubleMove.MustCapture = false;
        doubleMove.CanCapture = false;
        doubleMove.Direction = new Direction(1, 0);
        doubleMove.Condition = location => location.Rank == 1;
        Pawn.AddMoveCapability(doubleMove);
      }
      else if (PawnMultipleMove.Value == "Triple" || PawnMultipleMove.Value == "Wildebeest")
      {
        MoveCapability doubleMove = new MoveCapability();
        doubleMove.MinSteps = 2;
        doubleMove.MaxSteps = 3;
        doubleMove.MustCapture = false;
        doubleMove.CanCapture = false;
        doubleMove.Direction = new Direction(1, 0);
        doubleMove.Condition = location => location.Rank == 1;
        Pawn.AddMoveCapability(doubleMove);

        if (PawnMultipleMove.Value == "Wildebeest")
        {
          doubleMove = new MoveCapability();
          doubleMove.MinSteps = 2;
          doubleMove.MaxSteps = 2;
          doubleMove.MustCapture = false;
          doubleMove.CanCapture = false;
          doubleMove.Direction = new Direction(1, 0);
          doubleMove.Condition = location => location.Rank == 2;
          Pawn.AddMoveCapability(doubleMove);
        }
      }
      else if (PawnMultipleMove.Value == "Great")
      {
        MoveCapability doubleMove = new MoveCapability();
        doubleMove.MinSteps = 2;
        doubleMove.MaxSteps = 2;
        doubleMove.MustCapture = false;
        doubleMove.CanCapture = false;
        doubleMove.Direction = new Direction(1, 0);
        doubleMove.Condition = location => location.Rank == 1 || location.Rank == 2;
        Pawn.AddMoveCapability(doubleMove);
      }
      else if (PawnMultipleMove.Value == "Grand")
      {
        MoveCapability doubleMove = new MoveCapability();
        doubleMove.MinSteps = 2;
        doubleMove.MaxSteps = 2;
        doubleMove.MustCapture = false;
        doubleMove.CanCapture = false;
        doubleMove.Direction = new Direction(1, 0);
        doubleMove.Condition = location => location.Rank == 2;
        Pawn.AddMoveCapability(doubleMove);
      }
      else if (PawnMultipleMove.Value == "Unicorn")
      {
        MoveCapability doubleMove = new MoveCapability();
        doubleMove.MinSteps = 2;
        doubleMove.MaxSteps = 2;
        doubleMove.MustCapture = false;
        doubleMove.CanCapture = false;
        doubleMove.Direction = new Direction(1, 0);
        int nFiles = Board.NumFiles;
        int file1 = nFiles / 2;
        int file2 = (nFiles - 1) / 2;
        doubleMove.Condition = location => location.Rank == 1 ||
          (location.Rank == 2 && (location.File == file1 || location.File == file2));
        Pawn.AddMoveCapability(doubleMove);
      }
      else if (PawnMultipleMove.Value == "Fast Pawn")
      {
        //	Find the pawn's forward move capability and increase 
        //	the range to two spaces
        MoveCapability[] moves;
        int nMoves = Pawn.GetMoveCapabilities(out moves);
        foreach (MoveCapability move in moves)
          if (move.NDirection == PredefinedDirections.N && move.MaxSteps == 1)
          {
            move.MaxSteps = 2;
            break;
          }
      }
    }
    #endregion
  }
}
