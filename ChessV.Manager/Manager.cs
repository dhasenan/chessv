
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

using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Threading;

namespace ChessV.Manager
{
	/************************************************************************

	                                 Manager
	
	The Manager class contains a number of indexes cataloging all the types 
	of Games that have been defined, all the PieceTypes, and all the XBoard 
	protocol engines.  It also contains the code creating new instances 
	of Games and Engines.
	
    ************************************************************************/

	public class Manager
	{
		// *** PUBLIC MEMBERS *** //

		//	A lookup table of all the Game-derived classes discovered that implement different variants
		public Dictionary<string, Type> GameClasses;

		//	A lookup table of all the GameAttribute attributes attached to game classes discovered indexed by game name
		public Dictionary<string, GameAttribute> GameAttributes;

		//	A lookup table of AppearanceAttributes attached to game classes
		public Dictionary<string, AppearanceAttribute> AppearanceAttributes;

		//	A library of all the pre-defined (built in) piece types
		static public PieceTypeLibrary PieceTypeLibrary;

		//	The main Environment (for running user scripts in the interpereter)
		public Compiler.Environment Environment;

		//	The library of all XBoard/Winboard engines discovered in the Engines\XBoard directory available for use
		public EngineLibrary EngineLibrary;

		//	A bogus record to designate the internal chess engine
		public EngineConfiguration InternalEngine;


		// *** CONSTRUCTION *** //

		#region Constructor
		public Manager()
		{
			#region Create Member Objects
			GameClasses = new Dictionary<string, Type>();
			GameAttributes = new Dictionary<string, GameAttribute>();
			AppearanceAttributes = new Dictionary<string, AppearanceAttribute>();
			Environment = new Compiler.Environment();
			PieceTypeLibrary = new PieceTypeLibrary();
			EngineLibrary = new EngineLibrary();
			InternalEngine = new EngineConfiguration();
			InternalEngine.FriendlyName = "ChessV";
			InternalEngine.InternalName = "ChessV 2.2 RC1";
			#endregion

			#region Add ChessV Data Types to Environment
			Environment.AddSymbol( "MoveCapability", typeof( MoveCapability ) );
			Environment.AddSymbol( "Direction", typeof( Direction ) );
			Environment.AddSymbol( "BitBoard", typeof( BitBoard ) );
			Environment.AddSymbol( "Rule", typeof( Rule ) );
			Environment.AddSymbol( "Game", typeof( Game ) );
			Environment.AddSymbol( "PieceType", typeof( PieceType ) );
			Environment.AddSymbol( "Piece", typeof( Piece ) );
			Environment.AddSymbol( "Location", typeof( Location ) );
			#endregion

			#region Load Internal Games

			// *** LOAD INTERNAL GAMES *** //

			//	Load games and piece types from the main ChessV.Base module
			Module module = typeof(Game).Module;
			loadPieceTypesFromModule( module );
			loadGamesFromModule( module );

			//	Load games and piece types from the ChessV.Games DLL
			string moduleName = module.FullyQualifiedName;
			string modulePath = moduleName.Substring( 0, Math.Max( moduleName.LastIndexOf( '\\' ), moduleName.LastIndexOf( '/' ) ) + 1 );
			string gamesDllName = modulePath + "ChessV.Games.dll";
			Assembly gamesAssembly = Assembly.UnsafeLoadFrom( gamesDllName );
			foreach( Module gamesModule in gamesAssembly.GetModules() )
			{
				loadPieceTypesFromModule( (Module) gamesModule );
				loadGamesFromModule( (Module) gamesModule );
				loadPieceTypePropertyAttributesFromModule( (Module) gamesModule );
				loadRulesFromModule( (Module) gamesModule );
				loadEvaluationsFromModule( (Module) gamesModule );
			}
			#endregion

			#region Load Games from Include Folder

			// *** LOAD GAMES FROM INCLUDE FOLDER *** //

			//	Search for the include folder.  We provide some flexibility 
			//	regarding where this path is located
			string currPath = Directory.GetCurrentDirectory();
			string includePath = Path.Combine( currPath, "Include" );
			if( !Directory.Exists( includePath ) )
			{
				int iIndex = currPath.LastIndexOf( "ChessV" );
				if( iIndex > 0 )
				{
					iIndex = currPath.IndexOf( Path.DirectorySeparatorChar, iIndex );
					if( iIndex > 0 )
					{
						currPath = currPath.Remove( iIndex );
						includePath = Path.Combine( currPath, "Include" );
						if( !Directory.Exists( includePath ) )
						{
							currPath = Directory.GetCurrentDirectory();
							iIndex = currPath.IndexOf( "ChessV" );
							if( iIndex > 0 )
							{
								iIndex = currPath.IndexOf( Path.DirectorySeparatorChar, iIndex );
								if( iIndex > 0 )
								{
									currPath = currPath.Remove( iIndex );
									includePath = Path.Combine( currPath, "Include" );
								}
							}
						}
					}
				}
			}
			
			if( Directory.Exists( includePath ) )
			{
				AppDomain myDomain = Thread.GetDomain();
				AssemblyName assemblyName = new AssemblyName();
				assemblyName.Name = "DynamicGamesAssembly";
				System.Reflection.Emit.AssemblyBuilder assemblyBuilder = myDomain.DefineDynamicAssembly( assemblyName, System.Reflection.Emit.AssemblyBuilderAccess.RunAndSave );
				System.Reflection.Emit.ModuleBuilder dynamicModule = assemblyBuilder.DefineDynamicModule( "ChessVDynamicGames" );
				Compiler.Compiler compiler = new Compiler.Compiler( assemblyBuilder, dynamicModule, Environment );
				string[] includeFiles = Directory.GetFiles( includePath, "*.cvc" );
				foreach( string file in includeFiles )
				{
					TextReader reader = new StreamReader( file );
					compiler.ProcessInput( reader );
					reader.Close();
				}
				foreach( KeyValuePair<string, Type> pair in compiler.GameTypes )
				{
					GameClasses.Add( pair.Key, pair.Value );
					GameAttributes.Add( pair.Key, compiler.GameAttributes[pair.Key] );
					if( compiler.GameAppearances.ContainsKey( pair.Key ) )
						AppearanceAttributes.Add( pair.Key, compiler.GameAppearances[pair.Key] );
						
				}
			}
			#endregion
		}
		#endregion


		// *** OPERATIONS *** //

		#region CreateGame
		public Game CreateGame
			( string name,                                   //  name of game to create
			  Dictionary<string, string> definitions = null, //  optional dict of defined Game Variables
			  InitializationHelper initHelper = null )       //  optional init helper (null if non-interactive)
		{
			//	look up the Type of the requested Game class
			Type gameClass = GameClasses[name];

			//	look up the GameAttribute of the requested game
			GameAttribute gameAttribute = GameAttributes[name];

			//	use reflection to find the default constructor
			ConstructorInfo ci = gameClass.GetConstructor( new Type[] { } );
			if( ci == null )
				//	No appropriate constructor found.
				//	This is probably a template (abstract) Game
				return null;

			//	invoke the constructor to create the game object
			Game newgame;
			try
			{
				newgame = (Game) ci.Invoke( null );
			}
			catch( Exception ex )
			{
				throw new Exceptions.GameInitializationException( gameAttribute, "Exception in Manager.CreateGame: failed to create game", ex );
			}

			//	find the Game's Environment field and set it to a newly 
			//	constructed interpreter/compiler Environment with this Manager's 
			//	global Environment as the parent Environment.
			FieldInfo environmentField = newgame.GetType().GetField( "Environment" );
			if( environmentField != null )
				environmentField.SetValue( newgame, new Compiler.Environment( Environment ) );

			//	initialize the Game with the Initialize method - this is essential
			newgame.Initialize( gameAttribute, definitions, initHelper );

			//	return newly constructed Game object
			return newgame;
		}
		#endregion

		#region LoadGame
		public Game LoadGame( TextReader reader, List<ulong> hashcodes = null )
		{
			//	create a SavedGameReader which does most of the work
			SavedGameReader savedGame = new SavedGameReader( reader );

			//	Find the name of the internally-defined Game we should be creating. 
			//	Usually, this is the name specified in the saved game file, but it 
			//	also supports creating new named Games derived from existing games. 
			//	In this case, we want the name of the existing base Game.
			string gameName;
			if( GameAttributes.ContainsKey( savedGame.GameName ) )
				gameName = savedGame.GameName;
			else if( savedGame.BaseGameName != null && GameAttributes.ContainsKey( savedGame.BaseGameName ) )
				gameName = savedGame.BaseGameName;
			else
				throw new Exception( "Saved game file specifies unknown variant: " + savedGame.GameName );

			//	create the Game object now that we have determined the name
			Game loadedGame = CreateGame( gameName, savedGame.VariableDefinitions );

			//	play out any moves saved with the game
			loadedGame.PlayMoves( savedGame.Moves, MoveNotation.StandardAlgebraic, hashcodes );

			//	return the new Game object
			return loadedGame;
		}
		#endregion

		#region LoadMatchesList
		public MatchSet LoadMatches( TextReader reader, string defaultPath )
		{
			Dictionary<string, int> dataFileColumnMap = new Dictionary<string, int>();
			MatchSet matchSet = new MatchSet();

			//	read the header row
			string input = reader.ReadLine();

			//	parse the headers
			string[] split = input.Split( '\t' );
			for( int x = 0; x < split.Length; x++ )
				dataFileColumnMap.Add( split[x].ToUpper(), x );
			if( !dataFileColumnMap.ContainsKey( "GAME" ) )
				throw new Exception( "LoadMatches: column 'Game' is required" );
			if( !dataFileColumnMap.ContainsKey( "ENGINE1" ) )
				throw new Exception( "LoadMatches: column 'Engine1' is required" );
			if( !dataFileColumnMap.ContainsKey( "ENGINE2" ) )
				throw new Exception( "LoadMatches: column 'Engine2' is required" );
			if( !dataFileColumnMap.ContainsKey( "TIME CONTROL" ) )
				throw new Exception( "LoadMatches: column 'Time Control' is required" );

			int gameNumber = 1;
			while( (input = reader.ReadLine()) != null )
			{
				string[] parts = input.Split( '\t' );
				if( parts.Length != dataFileColumnMap.Count )
					throw new Exception( "LoadMatches: Incorrect number of elements - expected " + 
						dataFileColumnMap.Count.ToString() + " but found " + parts.Length.ToString() );
				MatchRecord record = new MatchRecord();
				record.ID = dataFileColumnMap.ContainsKey( "ID" )
					? parts[dataFileColumnMap["ID"]]
					: gameNumber.ToString() + ".";
					
				string game = parts[dataFileColumnMap["GAME"]];
				record.SavedGameFile = Path.IsPathRooted( game ) ? game : Path.Combine( defaultPath, game );
				record.TimeControl = parts[dataFileColumnMap["TIME CONTROL"]];
				record.EngineNames[0] = parts[dataFileColumnMap["ENGINE1"]];
				record.EngineNames[1] = parts[dataFileColumnMap["ENGINE2"]];
				record.PlayerNames[0] = dataFileColumnMap.ContainsKey( "PLAYER1" ) ? parts[dataFileColumnMap["PLAYER1"]] : parts[dataFileColumnMap["ENGINE1"]];
				record.PlayerNames[1] = dataFileColumnMap.ContainsKey( "PLAYER2" ) ? parts[dataFileColumnMap["PLAYER2"]] : parts[dataFileColumnMap["ENGINE2"]];
				record.Variation = 0;
				if( dataFileColumnMap.ContainsKey( "VARIATION" ) )
				{
					if( parts[dataFileColumnMap["VARIATION"]].ToUpper() == "NONE" || parts[dataFileColumnMap["VARIATION"]] == "0" )
						record.Variation = 0;
					if( parts[dataFileColumnMap["VARIATION"]].ToUpper() == "SMALL" || parts[dataFileColumnMap["VARIATION"]] == "1" )
						record.Variation = 1;
					if( parts[dataFileColumnMap["VARIATION"]].ToUpper() == "MEDIUM" || parts[dataFileColumnMap["VARIATION"]] == "2" )
						record.Variation = 2;
					if( parts[dataFileColumnMap["VARIATION"]].ToUpper() == "LARGE" || parts[dataFileColumnMap["VARIATION"]] == "3" )
						record.Variation = 3;
				}
				matchSet.Add( record );
				gameNumber++;
			}
			return matchSet;
		}
		#endregion

		#region LookupEngineByPartialName
		public EngineConfiguration LookupEngineByPartialName( string name )
		{
			if( name.ToUpper().Contains( "CHESSV" ) )
				return InternalEngine;
			return EngineLibrary.LookupEngineByPartialName( name );
		}
		#endregion

		#region GetXBoardVariantList
		public List<string> GetXBoardVariantList()
		{
			List<string> variantList = new List<string>();
			foreach( KeyValuePair<string, GameAttribute> pair in GameAttributes )
				if( pair.Value.XBoardName != null && pair.Value.XBoardName.Length > 0 )
					variantList.Add( pair.Value.XBoardName );
			return variantList;
		}
		#endregion

		#region XBoardNameToProperName
		public string XBoardNameToProperName( string xboardName )
		{
			foreach( KeyValuePair<string, GameAttribute> pair in GameAttributes )
				if( pair.Value.XBoardName == xboardName )
					return pair.Key;
			return null;
		}
		#endregion


		// *** HELPER FUNCTIONS *** //

		#region loadGamesFromModule
		protected void loadGamesFromModule( Module module )
		{
			Type[] types = module.GetTypes();
			foreach( Type type in types )
			{
				object[] customAttrs = type.GetCustomAttributes( typeof(GameAttribute), false );
				if( customAttrs != null && customAttrs.Length >= 1 )
					foreach( object attr in customAttrs )
					{
						GameAttribute gameAttribute = (GameAttribute) attr;
						GameClasses.Add( gameAttribute.GameName, type );
						GameAttributes.Add( gameAttribute.GameName, gameAttribute );
						Environment.AddSymbol( gameAttribute.GameName, type );
						object[] appearanceAttrs = type.GetCustomAttributes( typeof( AppearanceAttribute ), false );
						if( appearanceAttrs != null && customAttrs.Length >= 1 )
							foreach( object attr2 in appearanceAttrs )
							{
								AppearanceAttribute appearance = (AppearanceAttribute) attr2;
								if( appearance.Game != null )
								{
									if( AppearanceAttributes.ContainsKey( appearance.Game ) )
										AppearanceAttributes[appearance.Game] = appearance;
									else
										AppearanceAttributes.Add( appearance.Game, appearance );
								}
								else
									AppearanceAttributes.Add( gameAttribute.GameName, appearance );
							}
					}
			}
		}
		#endregion

		#region loadPieceTypesFromModule
		protected void loadPieceTypesFromModule( Module module )
		{
			Type[] types = module.GetTypes();
			foreach( Type type in types )
			{
				object[] customAttrs = type.GetCustomAttributes( typeof(PieceTypeAttribute), false );
				if( customAttrs != null && customAttrs.Length >= 1 )
					foreach( object attr in customAttrs )
					{
						PieceTypeAttribute pieceTypeAttribute = (PieceTypeAttribute) attr;
						PieceTypeLibrary.Add( pieceTypeAttribute.Name, type );
						Environment.AddSymbol( pieceTypeAttribute.Name, type );
					}
			}
		}
		#endregion

		#region loadPieceTypePropertyAttributesFromModule
		protected void loadPieceTypePropertyAttributesFromModule( Module module )
		{
			Type[] types = module.GetTypes();
			foreach( Type type in types )
			{
				if( type.IsSubclassOf( typeof(PieceTypePropertyAttribute) ) )
					Environment.AddSymbol( type.Name, type );
			}
		}
		#endregion

		#region loadRulesFromModule
		protected void loadRulesFromModule( Module module )
		{
			Type[] types = module.GetTypes();
			foreach( Type type in types )
			{
				if( type.IsSubclassOf( typeof( Rule ) ) )
					Environment.AddSymbol( type.Name, type );
			}
		}
		#endregion

		#region loadEvaluationsFromModule
		protected void loadEvaluationsFromModule( Module module )
		{
			Type[] types = module.GetTypes();
			foreach( Type type in types )
			{
				if( type.IsSubclassOf( typeof(Evaluation) ) )
					Environment.AddSymbol( type.Name, type );
			}
		}
		#endregion
	}
}
