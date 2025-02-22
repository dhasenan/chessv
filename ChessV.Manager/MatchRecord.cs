﻿/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2018 BY GREG STRONG
  
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

namespace ChessV.Manager
{
  public class MatchRecord
  {
    public MatchRecord()
    {
      PlayerNames = new string[2];
      EngineNames = new string[2];
      Engines = new EngineConfiguration[2];
    }

    public string ID;
    public string SavedGameFile;
    public string TimeControl;
    public string Result;
    public string[] PlayerNames;
    public string[] EngineNames;
    public EngineConfiguration[] Engines;
    public int Variation;
    public string Winner;
    public Game Game;
  }
}
