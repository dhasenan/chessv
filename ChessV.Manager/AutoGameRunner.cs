
/***************************************************************************

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

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ChessV.Manager
{
	public class AutoGameRunner
	{
		public List<MatchRecord> GameList;
		public Queue<MatchRecord> PendingGames;
		public List<MatchRecord> FinishedGames;

		public Manager Manager { get; protected set; }
		public int ConcurrentGames { get; protected set; }
		public string CurrentPath { get; set; }

		public AutoGameRunner( Manager manager )
		{
			Manager = manager;
			GameList = new List<MatchRecord>();
			PendingGames = new Queue<MatchRecord>();
			FinishedGames = new List<MatchRecord>();
			ConcurrentGames = 1;
		}

		public IEnumerable<Game> Run()
		{
			foreach( MatchRecord gameRecord in GameList )
				PendingGames.Enqueue( gameRecord );
			while( PendingGames.Count > 0 )
			{
				MatchRecord currentGameRecord = PendingGames.Dequeue();
				if( currentGameRecord.SavedGameFile != null )
				{
					if( File.Exists( CurrentPath + Path.DirectorySeparatorChar + currentGameRecord.SavedGameFile ) )
					{
						System.GC.Collect();
						TextReader reader = new StreamReader( CurrentPath + Path.DirectorySeparatorChar + currentGameRecord.SavedGameFile );
						Game game = Manager.LoadGame( reader );
						reader.Close();
						game.StartMatch();
						game.ComputerControlled[0] = true;
						game.ComputerControlled[1] = true;
						game.AddInternalEngine( 0 );
						game.AddInternalEngine( 1 );
						TimeControl timeControl = new TimeControl( currentGameRecord.TimeControl );
						game.Match.SetTimeControl( timeControl );
						yield return game;
					}
					else
						throw new Exception( "Cannot find saved game: " + currentGameRecord.SavedGameFile );
				}
			}
		}

		protected KeyValuePair<MatchRecord, Game> gameInProgress;
	}
}
