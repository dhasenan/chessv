
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

namespace ChessV.Games
{
  #region Cannon
  [PieceType("Cannon", "Miscellaneous")]
  public class Cannon : PieceType
  {
    public Cannon(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Cannon", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);

      #region Customize piece-square-tables for the Cannon
      //	Midgame PST
      PSTMidgameInSmallCenter = 0;
      PSTMidgameInLargeCenter = 0;
      PSTMidgameSmallCenterAttacks = 2;
      PSTMidgameLargeCenterAttacks = 4;
      PSTMidgameForwardness = 0;
      PSTMidgameGlobalOffset = 0;
      //	Endgame PST
      PSTEndgameInSmallCenter = 0;
      PSTEndgameInLargeCenter = 0;
      PSTEndgameSmallCenterAttacks = 0;
      PSTEndgameLargeCenterAttacks = 0;
      PSTEndgameForwardness = 0;
      PSTEndgameGlobalOffset = 0;
      #endregion
    }

    public static new void AddMoves(PieceType type)
    {
      type.CannonMove(new Direction(0, 1));
      type.CannonMove(new Direction(0, -1));
      type.CannonMove(new Direction(1, 0));
      type.CannonMove(new Direction(-1, 0));
    }
  }
  #endregion

  #region Vao
  [PieceType("Vao", "Miscellaneous")]
  public class Vao : PieceType
  {
    public Vao(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Vao", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      type.CannonMove(new Direction(1, 1));
      type.CannonMove(new Direction(1, -1));
      type.CannonMove(new Direction(-1, 1));
      type.CannonMove(new Direction(-1, -1));
    }
  }
  #endregion

  #region Camel General
  [PieceType("Camel General", "Miscellaneous")]
  public class CamelGeneral : PieceType
  {
    public CamelGeneral(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Camel General", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      King.AddMoves(type);
      Camel.AddMoves(type);
    }
  }
  #endregion

  #region Jumping General
  [PieceType("Jumping General", "Miscellaneous")]
  public class JumpingGeneral : PieceType
  {
    public JumpingGeneral(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Jumping General", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      King.AddMoves(type);
      Elephant.AddMoves(type);
      Dabbabah.AddMoves(type);
    }
  }
  #endregion

  #region VerticalMoverGeneral
  [PieceType("Vertical Mover General", "Miscellaneous")]
  public class VerticalMoverGeneral : PieceType
  {
    public VerticalMoverGeneral(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Vertical Mover General", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      type.Slide(new Direction(1, 0));
      type.Slide(new Direction(-1, 0));
      type.Step(new Direction(0, 1));
      type.Step(new Direction(0, -1));
      Ferz.AddMoves(type);
    }
  }
  #endregion

  #region SideMoverGeneral
  [PieceType("Side Mover General", "Miscellaneous")]
  public class SideMoverGeneral : PieceType
  {
    public SideMoverGeneral(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Side Mover General", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      type.Slide(new Direction(0, 1));
      type.Slide(new Direction(0, -1));
      type.Step(new Direction(1, 0));
      type.Step(new Direction(-1, 0));
      Ferz.AddMoves(type);
    }
  }
  #endregion

  #region SquirrelGeneral
  [PieceType("Squirrel General", "Miscellaneous")]
  public class SquirrelGeneral : PieceType
  {
    public SquirrelGeneral(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Squirrel General", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Squirrel.AddMoves(type);
      King.AddMoves(type);
    }
  }
  #endregion
}
