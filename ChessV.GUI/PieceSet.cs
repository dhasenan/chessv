
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2017 BY GREG STRONG

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
using System.Drawing;
using System.IO;

namespace ChessV.GUI
{
  public class PieceSet
  {
    public string Name { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public bool PreColored { get; private set; }
    public Dictionary<string, string> FilesByTypeName { get; private set; }

    public PieceSet(string name, string directory)
    {
      Name = name;
      FilesByTypeName = new Dictionary<string, string>();
      //	open the file for the King to determine the size of the bitmaps
      Bitmap kingBitmap;
      if (File.Exists(Path.Combine(directory, "King.bmp")))
      {
        PreColored = false;
        kingBitmap = new Bitmap(Path.Combine(directory, "King.bmp"));
      }
      else
      {
        PreColored = true;
        kingBitmap = new Bitmap(Path.Combine(directory, "WKing.bmp"));
      }
      Width = kingBitmap.Width;
      Height = kingBitmap.Height;

      string[] files = Directory.GetFiles(directory, "*.bmp");
      foreach (string file in files)
      {
        string pieceName = Path.GetFileName(file);
        if (PreColored && pieceName[0] == 'W')
        {
          pieceName = pieceName.Substring(1, pieceName.Length - 5);
          FilesByTypeName.Add(pieceName, file);
        }
        else if (!PreColored)
        {
          pieceName = pieceName.Substring(0, pieceName.Length - 4);
          FilesByTypeName.Add(pieceName, file);
        }
      }
    }

    public override string ToString()
    {
      return Name;
    }

    public string GetFilenameForPiece(PieceType piecetype)
    {
      foreach (string imageName in piecetype.ImagePreferenceList)
        if (FilesByTypeName.ContainsKey(imageName.Replace(" ", "")))
          return FilesByTypeName[imageName.Replace(" ", "")];
      return null;
    }
  }
}

