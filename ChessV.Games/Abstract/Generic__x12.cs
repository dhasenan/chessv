
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
  //                           Generic__x12
  //
  //    The Generic game classes make it easier to specify games by 
  //    providing functionality common to chess variants.  This class 
  //    is for chess variants on boards with 12 ranks and is used as a 
  //    base class for other generic boards such as 12x12.
  //
  //    It derives from the GenericChess class which provides the 
  //    rules for a game with Pawns and a Royal King, as well as the 
  //    50-move and draw-by-repetition rules.
  //
  //    This class adds optional support for a number of different 
  //    initial pawn multiple-move rules and En Passant.

  [Game("Generic x12", typeof(Geometry.Rectangular), 12,
      Template = true)]
  public class Generic__x12 : GenericChess
  {
    // *** GAME VARIABLES *** //

    [GameVariable] public ChoiceVariable PawnMultipleMove { get; set; }


    // *** CONSTRUCTION *** //

    public Generic__x12
      (int nFiles,               // number of files on main part of board
        Symmetry symmetry) :      // symmetry determining board mirroring/rotation
        base(nFiles, /* num ranks = */ 12, symmetry)
    {
    }


    // *** INITIALIZATION *** //

    #region SetGameVariables
    public override void SetGameVariables()
    {
      base.SetGameVariables();
      PawnMultipleMove = new ChoiceVariable();
      PawnMultipleMove.AddChoice("None", "Pawns can never move more than a single space");
      PawnMultipleMove.AddChoice("@2(2)", "Pawns can move two spaces when on the second rank");
      PawnMultipleMove.AddChoice("@2(2,3)", "Pawns can move two or three spaces when on the second rank");
      PawnMultipleMove.AddChoice("@2(2,3,4)", "Pawns can move up to four spaces when on the second rank");
      PawnMultipleMove.AddChoice("@3(2)", "Pawns can move two spaces when on the third rank");
      PawnMultipleMove.AddChoice("@3(2,3)", "Pawns can move two or three spaces when on the third rank");
      PawnMultipleMove.AddChoice("@4(2)", "Pawns can move two spaces when on the fourth rank");
      PawnMultipleMove.AddChoice("Fast Pawn", "Pawns can move two spaces from any location");
      PawnMultipleMove.AddChoice("Custom", "Indicates a custom rule implemented by derived class");
      PawnMultipleMove.Value = "None";
    }
    #endregion

    #region AddRules
    public override void AddRules()
    {
      base.AddRules();

      // *** PAWN MULTIPLE MOVE *** //
      if (PawnMultipleMove.Value == "@2(2)" || PawnMultipleMove.Value == "@2(2,3)" || PawnMultipleMove.Value == "@2(2,3,4)")
      {
        MoveCapability doubleMove = new MoveCapability();
        doubleMove.MinSteps = 2;
        doubleMove.MaxSteps = Convert.ToInt32(PawnMultipleMove.Value.Substring(PawnMultipleMove.Value.Length - 2, 1));
        doubleMove.MustCapture = false;
        doubleMove.CanCapture = false;
        doubleMove.Direction = new Direction(1, 0);
        doubleMove.Condition = location => location.Rank == 1;
        Pawn.AddMoveCapability(doubleMove);
      }
      else if (PawnMultipleMove.Value == "@3(2)" || PawnMultipleMove.Value == "@3(2,3)")
      {
        MoveCapability doubleMove = new MoveCapability();
        doubleMove.MinSteps = 2;
        doubleMove.MaxSteps = Convert.ToInt32(PawnMultipleMove.Value.Substring(PawnMultipleMove.Value.Length - 2, 1));
        doubleMove.MustCapture = false;
        doubleMove.CanCapture = false;
        doubleMove.Direction = new Direction(1, 0);
        doubleMove.Condition = location => location.Rank == 2;
        Pawn.AddMoveCapability(doubleMove);
      }
      else if (PawnMultipleMove.Value == "@4(2)")
      {
        MoveCapability doubleMove = new MoveCapability();
        doubleMove.MinSteps = 2;
        doubleMove.MaxSteps = 2;
        doubleMove.MustCapture = false;
        doubleMove.CanCapture = false;
        doubleMove.Direction = new Direction(1, 0);
        doubleMove.Condition = location => location.Rank == 3;
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
