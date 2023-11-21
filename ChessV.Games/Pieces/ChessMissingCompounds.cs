
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
  #region Archbishop
  [PieceType("Archbishop", "Chess Missing Compounds")]
  public class Archbishop : PieceType
  {
    public Archbishop(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Archbishop", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Bishop.AddMoves(type);
      Knight.AddMoves(type);
    }
  }
  #endregion

  #region Chancellor
  [PieceType("Chancellor", "Chess Missing Compounds")]
  public class Chancellor : PieceType
  {
    public Chancellor(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Chancellor", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Rook.AddMoves(type);
      Knight.AddMoves(type);
    }
  }
  #endregion

  #region Amazon
  [PieceType("Amazon", "Chess Missing Compounds")]
  public class Amazon : PieceType
  {
    public Amazon(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Amazon", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Queen.AddMoves(type);
      Knight.AddMoves(type);
    }
  }
  #endregion
}
