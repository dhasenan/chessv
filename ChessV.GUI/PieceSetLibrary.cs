
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2020 BY GREG STRONG

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
using System.IO;

namespace ChessV.GUI
{
  public static class PieceSetLibrary
  {
    public static void Initialize()
    {
      //	Piece sets aren't stored in the Registry - we just check the Graphics\Piece Sets
      //	subdirectory on each run of the program to see what's available.
      //	Search for the directory - we allow some flexibility for where it is located
      var currPath = Directory.GetCurrentDirectory();
      var pieceSetRootDir = Path.Combine(currPath, "Graphics", "Piece Sets");
      while (!Directory.Exists(pieceSetRootDir))
      {
        var i = currPath.LastIndexOf(Path.DirectorySeparatorChar);
        if (i < 0)
        {
          // give up :(
          return;
        }
        currPath = currPath.Remove(i);
        pieceSetRootDir = Path.Combine(currPath, "Graphics", "Piece Sets");
      }
      //	Create the look-up table for PieceSets by name
      PieceSets = new Dictionary<string, PieceSet>();
      var pieceSetDirs = Directory.GetDirectories(pieceSetRootDir);
      foreach (var pieceSetDirectory in pieceSetDirs)
      {
        //	The directory must have either a King.bmp or both a WKing.bmp and BKing.bmp to be considered a
        //	piece set.  The size of this image determines the size of the piece set
        if (!File.Exists(pieceSetDirectory + Path.DirectorySeparatorChar + "King.bmp") &&
            (!File.Exists(pieceSetDirectory + Path.DirectorySeparatorChar + "WKing.bmp") ||
             !File.Exists(pieceSetDirectory + Path.DirectorySeparatorChar + "BKing.bmp"))) continue;
        var pieceSetName = pieceSetDirectory.Substring(pieceSetDirectory.LastIndexOf(Path.DirectorySeparatorChar) + 1);
        PieceSets.Add(pieceSetName, new PieceSet(pieceSetName, pieceSetDirectory));
      }
    }

    public static PieceSet Default => PieceSets["Standard"];

    public static PieceSet Lookup(string name)
    {
      return PieceSets.ContainsKey(name) ? PieceSets[name] : null;
    }

    public static bool Contains(string name)
    {
      return PieceSets.ContainsKey(name);
    }

    public static Dictionary<string, PieceSet> PieceSets { get; private set; }

    //	lookup table of all the graphics piece sets discovered in the Graphics directory
  }
}
