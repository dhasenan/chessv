
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
  #region General
  [PieceType("General", "Miscellaneous Compounds")]
  public class General : PieceType
  {
    public General(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("General", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      King.AddMoves(type);
    }
  }
  #endregion

  #region ElephantFerz
  [PieceType("Elephant Ferz", "Miscellaneous Compounds")]
  public class ElephantFerz : PieceType
  {
    public ElephantFerz(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Elephant Ferz", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Elephant.AddMoves(type);
      Ferz.AddMoves(type);
    }
  }
  #endregion

  #region Unicorn
  [PieceType("Unicorn", "Miscellaneous Compounds")]
  public class Unicorn : PieceType
  {
    public Unicorn(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Unicorn", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Bishop.AddMoves(type);
      Nightrider.AddMoves(type);
    }
  }
  #endregion

  #region Squirrel
  [PieceType("Squirrel", "Miscellaneous Compounds")]
  public class Squirrel : PieceType
  {
    public Squirrel(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Squirrel", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Knight.AddMoves(type);
      Elephant.AddMoves(type);
      Dabbabah.AddMoves(type);
    }
  }
  #endregion

  #region Wildebeest
  [PieceType("Wildebeest", "Miscellaneous Compounds")]
  public class Wildebeest : PieceType
  {
    public Wildebeest(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Wildebeest", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Knight.AddMoves(type);
      Camel.AddMoves(type);
    }
  }
  #endregion

  #region Scout
  [PieceType("Scout", "Miscellaneous Compounds")]
  public class Scout : PieceType
  {
    public Scout(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Scout", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Wazir.AddMoves(type);
      Tribbabah.AddMoves(type);
    }
  }
  #endregion

  #region Wizard
  [PieceType("Wizard", "Miscellaneous Compounds")]
  public class Wizard : PieceType
  {
    public Wizard(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Wizard", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Ferz.AddMoves(type);
      Camel.AddMoves(type);
    }
  }
  #endregion

  #region Champion
  [PieceType("Champion", "Miscellaneous Compounds")]
  public class Champion : PieceType
  {
    public Champion(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Champion", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Wazir.AddMoves(type);
      Dabbabah.AddMoves(type);
      Elephant.AddMoves(type);
    }
  }
  #endregion

  #region Centaur
  [PieceType("Centaur", "Miscellaneous Compounds")]
  public class Centaur : PieceType
  {
    public Centaur(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Centaur", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      FallbackImage = "Knight General";
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      King.AddMoves(type);
      Knight.AddMoves(type);
    }
  }
  #endregion

  #region Caliph
  [PieceType("Caliph", "Miscellaneous Compounds")]
  public class Caliph : PieceType
  {
    public Caliph(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Caliph", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      FallbackImage = "Camel Bishop";
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Bishop.AddMoves(type);
      Camel.AddMoves(type);
    }
  }
  #endregion

  #region Dragon King
  [PieceType("Dragon King", "Miscellaneous Compounds")]
  public class DragonKing : PieceType
  {
    public DragonKing(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Dragon King", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Rook.AddMoves(type);
      Ferz.AddMoves(type);
    }
  }
  #endregion

  #region Dragon Horse
  [PieceType("Dragon Horse", "Miscellaneous Compounds")]
  public class DragonHorse : PieceType
  {
    public DragonHorse(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Dragon Horse", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Bishop.AddMoves(type);
      Wazir.AddMoves(type);
    }
  }
  #endregion

  #region Minister
  [PieceType("Ministor", "Miscellaneous Compounds")]
  public class Minister : PieceType
  {
    public Minister(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Minister", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Knight.AddMoves(type);
      Wazir.AddMoves(type);
      Dabbabah.AddMoves(type);
    }
  }
  #endregion

  #region High Priestess
  [PieceType("High Priestess", "Miscellaneous Compounds")]
  public class HighPriestess : PieceType
  {
    public HighPriestess(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("High Priestess", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Knight.AddMoves(type);
      Elephant.AddMoves(type);
      Ferz.AddMoves(type);
    }
  }
  #endregion

  #region Frog
  [PieceType("Frog", "Miscellaneous Compounts")]
  public class Frog : PieceType
  {
    public Frog(string name, string notation, int midgameValue, int endgameValue, string preferredImageName = null) :
      base("Frog", name, notation, midgameValue, endgameValue, preferredImageName)
    {
      AddMoves(this);
    }

    public static new void AddMoves(PieceType type)
    {
      Ferz.AddMoves(type);
      Tribbabah.AddMoves(type);
    }
  }
  #endregion
}
