
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
using System.IO;

namespace ChessV.Engine
{
	static class Program
	{
		// *** GLOBALS *** //

		//	The ChessV Manager singleton object
		static public Manager.Manager Manager;

		//	Are we running a Microsoft Windows platform?
		static public bool RunningOnWindows;

		//	A Random object used for generating pseudo-random numbers throughout the program
		static public Random Random = new Random();


		static void Main( string[] args )
		{
			// *** INITIALIZATION *** //

			Manager = new Manager.Manager();

			//	Are we running on Windows?
			RunningOnWindows = Path.DirectorySeparatorChar == '\\';

			XBoard2Interface.QueryAndSetXBoardActiveStatus();
			if( XBoard2Interface.Active )
				XBoard2Interface.StartListener();
		}
	}
}
