
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
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.IO;

namespace ChessV.Engine
{
	public class XBoard2Interface
	{
		#region Delegates
		//	The delegatetype communication event
		public delegate void CommunicationEvent( string message );

		//	The delegatetype standard event
		public delegate void StandardEvent();
		#endregion

		#region Public Events
		//	XBoard input event
		public static event CommunicationEvent XBoardInputEvent = ProcessInputEvent;

		//	XBoard output event
		public static event CommunicationEvent XBoardOutputEvent;

		//	XBoard quit event
		public static event StandardEvent XBoardQuitEvent;

		//	XBoard time updated event
		public static event StandardEvent XBoardTimeUpdatedEvent = ProcessTimeUpdatedEvent;
		#endregion

		#region Properties
		//	Gets a value indicating whether XBoard protocol is Active
		public static bool Active { get; private set; }

		//	Gets or sets the name of the variant to be played
		public static string Variant { get; set; }

		//	Get the current Game in progress (if any)
		public static Game Game { get; private set; }

		//	Get the TimeControl being used
		public static TimeControl TimeControl { get; private set; }

		//	Gets a value indicating whether to show the XBoard message GUI
		public static bool ShowGui { get; private set; }

		//	Gets or sets the thread that this class runs in
		public static Thread ThreadListener { get; private set; }

		//  Gets or sets the side (color) being played by this engine
		public static int MySide { get; private set; }
		#endregion

		#region Public Methods
		#region QueryAndSetXBoardActiveStatus
		public static void QueryAndSetXBoardActiveStatus()
		//	Checks if XBoard is present and sets the Active property if true
		{
			string strMessage = Console.ReadLine();
			Active = strMessage != null && strMessage.StartsWith( "xboard" );
			sendOutputMessage( "\n" );
		}
		#endregion

		#region ProcessTimeUpdatedEvent
		public static void ProcessTimeUpdatedEvent()
		{ 
		}
		#endregion

		#region ProcessInputEvent
		public static void ProcessInputEvent( string strMessage )
		{
			try
			{
				#region xboard
				if( strMessage.StartsWith( "xboard" ) )
				{
					/*	Chess Engine Communication Protocol 
						This command will be sent once immediately after your engine process is started. 
						You can use it to put your engine into "xboard mode" if that is needed. 
						If your engine prints a prompt to ask for user input, you must turn off the prompt and output a 
						newline when the "xboard" command comes in.
						This will be a false 2nd "ghost" message, so ignore it.
					*/
				}
				#endregion

				#region protover
				else if( strMessage.StartsWith( "protover " ) )
				{
					/*
					 * Chess Engine Communication Protocol
						ping (boolean, default 0, recommended 1) 
						If ping=1, xboard may use the protocol's new "ping" command; if ping=0, xboard will not use the command. 
						setboard (boolean, default 0, recommended 1) 
						If setboard=1, xboard will use the protocol's new "setboard" command to set up positions; if setboard=0, it will use the older "edit" command. 
						playother (boolean, default 0, recommended 1) 
						If playother=1, xboard will use the protocol's new "playother" command when appropriate; if playother=0, it will not use the command. 
						san (boolean, default 0) 
						If san=1, xboard will send moves to the engine in standard algebraic notation (SAN); for example, Nf3. If san=0, xboard will send moves in coordinate notation; for example, g1f3. See MOVE in section 8 above for more details of both kinds of notation. 
						usermove (boolean, default 0) 
						If usermove=1, xboard will send moves to the engine with the command "usermove MOVE"; if usermove=0, xboard will send just the move, with no command name. 
						time (boolean, default 1, recommended 1) 
						If time=1, xboard will send the "time" and "otim" commands to update the engine's clocks; if time=0, it will not. 
						draw (boolean, default 1, recommended 1) 
						If draw=1, xboard will send the "draw" command if the engine's opponent offers a draw; if draw=0, xboard will not inform the engine about draw offers. Note that if draw=1, you may receive a draw offer while you are on move; if this will cause you to move immediately, you should set draw=0. 
						sigint (boolean, default 1) 
						If sigint=1, xboard may send SIGINT (the interrupt signal) to the engine as section 7 above; if sigint=0, it will not. 
						sigterm (boolean, default 1) 
						If sigterm=1, xboard may send SIGTERM (the termination signal) to the engine as section 7 above; if sigterm=0, it will not. 
						reuse (boolean, default 1, recommended 1) 
						If reuse=1, xboard may reuse your engine for multiple games. If reuse=0 (or if the user has set the -xreuse option on xboard's command line), xboard will kill the engine process after every game and start a fresh process for the next game. 
						analyze (boolean, default 1, recommended 1) 
						If analyze=0, xboard will not try to use the "analyze" command; it will pop up an error message if the user asks for analysis mode. If analyze=1, xboard will try to use the command if the user asks for analysis mode. 
						myname (string, default determined from engine filename) 
						This feature lets you set the name that xboard will use for your engine in window banners, in the PGN tags of saved game files, and when sending the "name" command to another engine. 
						variants (string, see text below) 
						This feature indicates which chess variants your engine accepts. It should be a comma-separated list of variant names. See the table under the "variant" command in section 8 above. If you do not set this feature, xboard will assume by default that your engine supports all variants. (However, the -zippyVariants command-line option still limits which variants will be accepted in Zippy mode.) It is recommended that you set this feature to the correct value for your engine (just "normal" in most cases) rather than leaving the default in place, so that the user will get an appropriate error message if he tries to play a variant that your engine does not support. 
						colors (boolean, default 1, recommended 0) 
						If colors=1, xboard uses the obsolete "white" and "black" commands in a stylized way that works with most older chess engines that require the commands. See the "Idioms" section below for details. If colors=0, xboard does not use the "white" and "black" commands at all. 
						ics (boolean, default 0) 
						If ics=1, xboard will use the protocol's new "ics" command to inform the engine of whether or not it is playing on a chess server; if ics=0, it will not. 
						name (boolean, see text below) 
						If name=1, xboard will use the protocol's "name" command to inform the engine of the opponent's name; if name=0, it will not. By default, name=1 if the engine is playing on a chess server; name=0 if not. 
						pause (boolean, default 0) 
						If pause=1, xboard may use the protocol's new "pause" command; if pause=0, xboard assumes that the engine does not support this command. 
						done (integer, no default) 
						If you set done=1 during the initial two-second timeout after xboard sends you the "xboard" command, the timeout will end and xboard will not look for any more feature commands before starting normal operation. If you set done=0, the initial timeout is increased to one hour; in this case, you must set done=1 before xboard will enter normal operation. 
					*/
					string strFeatures = string.Empty;
					strFeatures += "feature ping=1\n";
					strFeatures += "feature memory=1\n";
					strFeatures += "feature setboard=1\n";
					strFeatures += "feature usermove=1\n";
					strFeatures += "feature time=1\n";
					strFeatures += "feature reuse=1\n";
					strFeatures += "feature sigint=0\n";
					strFeatures += "feature sigterm=0\n";
					strFeatures += "feature draw=0\n";
					strFeatures += "feature option=\"Variation of Play\" -combo *None /// Small /// Medium /// Large\n";
					strFeatures += "feature option=\"Weakening\" -slider 0 0 15\n";
					strFeatures += "feature myname=\"ChessV 2.2\"\n";
					strFeatures += "feature colors=0\n";

					//	Determine all supported variants with an XBoard protocol name
					List<string> xboardVariants = Program.Manager.GetXBoardVariantList();
					StringBuilder variants = new StringBuilder( 400 );
					foreach( string variant in xboardVariants )
					{
						if( variants.Length > 0 )
							variants.Append( ',' );
						variants.Append( variant );
					}
					strFeatures += "feature variants=\"" + variants.ToString() + "\"\n";

					strFeatures += "feature done=1";
					sendOutputMessage( "feature" + strFeatures );
				}
				#endregion

				#region new
				else if( strMessage == "new" )
				{
					//	if there is a current game, stop tinking and erase it
					Game?.AbortSearch();
					Game = null;

					//  We don't start a new game here, because we still don't 
					//	know what variant we are playing.  The "variant" command is 
					//	sent after the "new" command (probably for historical reasons.)

					//	we will assume we are playing Orthodox Chess ("normal") until we are told 
					//	otherwise - in case we don't receive a "variant" command
					Variant = "normal";

					//	set the computer to play black
					MySide = 1;
					ForceModeOn = false;
				}
				#endregion

				#region load
				else if( strMessage.StartsWith( "load " ) )
				{
					// Load saved game
					string strPath = strMessage.Substring( 5 ).Trim();
					Active = false;
					try
					{
						TextReader reader = new StreamReader( strPath );
						Game = Program.Manager.LoadGame( reader );
						sendOutputMessage( "Loaded save game: " + strPath );
					}
					catch( Exception ex )
					{
						throw new XBoardInputException( "Unable to load save game: " + strPath, ex );
					}

					Active = true;
				}
				#endregion

				#region variant
				else if( strMessage.StartsWith( "variant " ) )
				{
					Variant = strMessage.Substring( 8 ).Trim();
					List<string> variants = Program.Manager.GetXBoardVariantList();
					if( !variants.Contains( Variant ) )
						throw new XBoardInputException( "Unsupported variant: " + Variant );
					if( Game != null )
					{
						Game = null;
						createGameIfNotYetCreated();
					}
				}
				#endregion

				#region quit
				else if( strMessage == "quit" )
				{
					//	terminate the program
					//XBoardQuitEvent();
				}
				#endregion

				#region force
				else if( strMessage == "force" )
				{
					// * Chess Engine Communication Protocol
					//	Set the engine to play neither color ("force mode"). Stop clocks. The engine should check that moves 
					//	received in force mode are legal and made in the proper turn, but should not think, ponder, or make 
					//	moves of its own.
					ForceModeOn = true;
					if( Game != null )
					{
						Game.ComputerControlled[0] = false;
						Game.ComputerControlled[1] = false;
					}
				}
				#endregion

				#region go
				else if( strMessage == "go" )
				{
					// * Chess Engine Communication Protocol
					//	Leave force mode and set the engine to play the color that is on move. 
					//	Associate the engine's clock with the color that is on move, the opponent's clock with the color that 
					//	is not on move. Start the engine's clock. Start thinking and eventually make a move. 

					createGameIfNotYetCreated();
					ForceModeOn = false;
					MySide = Game.CurrentSide;
					Game.ComputerControlled[MySide] = true;
					TimeControl.StartTimer();
					List<Movement> moves = Game.Think( TimeControl );
					foreach( Movement mv in moves )
						Game.MakeMove( mv, true );
					sendMove( moves );
				}
				#endregion

				#region playother
				else if( strMessage == "playother" )
				{
					/*
					   * Chess Engine Communication Protocol
						(This command is new in protocol version 2. It is not sent unless you enable it with the feature command.) 
						Leave force mode and set the engine to play the color that is not on move. Associate the opponent's 
						clock with the color that is on move, the engine's clock with the color that is not on move. Start the 
						opponent's clock. If pondering is enabled, the engine should begin pondering. If the engine later 
						receives a move, it should start thinking and eventually reply. 
					*/
					/*Game.SuspendPondering();
					Game.PlayerToPlay = Game.PlayerToPlay.OpposingPlayer;
					Game.PlayerToPlay.Intellegence = Player.PlayerIntellegenceNames.Computer;
					Game.PlayerToPlay.OpposingPlayer.Intellegence = Player.PlayerIntellegenceNames.Human;
					Game.PlayerToPlay.OpposingPlayer.Clock.Stop();
					Game.PlayerToPlay.Clock.Start();
					Game.ResumePondering();*/
				}
				#endregion

				#region st command
				else if( strMessage.StartsWith( "st " ) )
				{
					// * Chess Engine Communication Protocol
					//	Set time Absolute fixed time-per-move. No time is carried forward from one move to the next. 
					//	The commands "level" and "st" are not used together. 
					int seconds = Convert.ToInt32( strMessage.Substring( "st ".Length ) );
					TimeControl = new TimeControl();
					TimeControl.TimePerMove = seconds * 1000;
					XBoardTimeUpdatedEvent();
				}
				#endregion

				#region sd command
				else if( strMessage.StartsWith( "sd " ) )
				{
					int depth = Convert.ToInt32( strMessage.Substring( "sd ".Length ) );
					TimeControl = new TimeControl();
					TimeControl.Infinite = true;
					TimeControl.PlyLimit = depth;
					XBoardTimeUpdatedEvent();
				}
				#endregion

				#region depth command
				else if( strMessage.StartsWith( "depth " ) )
				{
					// * Chess Engine Communication Protocol
					//	The engine should limit its thinking to DEPTH ply. 
					TimeControl = new TimeControl();
					TimeControl.Infinite = true;
					TimeControl.PlyLimit = Convert.ToInt32( strMessage.Substring( "depth ".Length ) );
				}
				#endregion

				#region time command
				else if( strMessage.StartsWith( "time " ) )
				{
					//					Game.SuspendPondering();

					/* Chess Engine Communication Protocol
						Set a clock that always belongs to the engine. N is a number in centiseconds (units of 1/100 second). 
						Even if the engine changes to playing the opposite color, this clock remains with the engine.
					*/
					TimeControl.TimeLeft = Convert.ToInt64( strMessage.Substring( "time ".Length ) ) * 10;
					//					WinBoardTimeUpdatedEvent();
				}
				#endregion

				#region usermove command
				else if( strMessage.StartsWith( "usermove " ) )
				{
					/*
					   * Chess Engine Communication Protocol
						By default, moves are sent to the engine without a command name; the notation is just sent as a line 
						by itself. Beginning in protocol version 2, you can use the feature command to cause the command name
						"usermove" to be sent before the move. Example: "usermove e2e4". 
					*/
					createGameIfNotYetCreated();
					Game.MakeMove( Game.MoveFromDescription( strMessage.Substring( "usermove ".Length ), MoveNotation.XBoard ), true );
					if( Game.ComputerControlled[Game.CurrentSide] )
					{
						TimeControl.StartTimer();
						List<Movement> moves = Game.Think( TimeControl );
						foreach( Movement mv in moves )
							Game.MakeMove( mv, true );
						sendMove( moves );
					}
				}
				#endregion

				#region ?
				else if( strMessage == "?" )
				{
					/*	Move now. If your engine is thinking, it should move immediately; otherwise, the command should be 
						ignored (treated as a no-op). It is permissible for your engine to always ignore the ? command. 
						The only bad consequence is that xboard's Move Now menu command will do nothing. 
					*/
					Game.AbortSearch();
				}
				#endregion

				#region ping
				else if( strMessage.StartsWith( "ping " ) )
				{
					/*
					   * Chess Engine Communication Protocol
						In this command, N is a decimal number. When you receive the command, reply by sending the string 
						pong N, where N is the same number you received. Important: You must not reply to a "ping" command 
						until you have finished executing all commands that you received before it. Pondering does not count; 
						if you receive a ping while pondering, you should reply immediately and continue pondering. Because 
						of the way xboard uses the ping command, if you implement the other commands in this protocol, you 
						should never see a "ping" command when it is your move; however, if you do, you must not send the 
						"pong" reply to xboard until after you send your move. For example, xboard may send "?" immediately 
						followed by "ping". If you implement the "?" command, you will have moved by the time you see the 
						subsequent ping command. Similarly, xboard may send a sequence like "force", "new", "ping". You must 
						not send the pong response until after you have finished executing the "new" command and are ready 
						for the new game to start. The ping command is new in protocol version 2 and will not be sent unless 
						you enable it with the "feature" command. Its purpose is to allow several race conditions that could 
						occur in previous versions of the protocol to be fixed, so it is highly recommended that you implement 
						it. It is especially important in simple engines that do not ponder and do not poll for input while 
						thinking, but it is needed in all engines. 
					*/

					if( Game != null )
						while( Game.IsThinking )
							// Wait for thinking to finish
							Thread.Sleep( 100 );

					sendOutputMessage( "pong " + strMessage.Substring( 5 ) );
				}
				#endregion

				#region draw
				else if( strMessage == "draw" )
				{
					/*
						The engine's opponent offers the engine a draw. To accept the draw, send "offer draw". 
						To decline, ignore the offer (that is, send nothing). If you're playing on ICS, it's possible for the 
						draw offer to have been withdrawn by the time you accept it, so don't assume the game is over because 
						you accept a draw offer. Continue playing until xboard tells you the game is over. See also 
						"offer draw" below. 
						Ignore all draw offers for now.
					*/
				}
				#endregion

				#region result
				else if( strMessage.StartsWith( "result " ) )
				{
					/*
					   * Chess Engine Communication Protocol
						After the end of each game, xboard will send you a result command. You can use this command to trigger learning. RESULT is either 1-0, 0-1, 1/2-1/2, or *, indicating whether white won, black won, the game was a draw, or the game was unfinished. The COMMENT string is purely a human-readable comment; its content is unspecified and subject to change. In ICS mode, it is passed through from ICS uninterpreted. Example: 
						result 1-0 {White mates}
						Here are some notes on interpreting the "result" command. Some apply only to playing on ICS ("Zippy" mode). 

						If you won but did not just play a mate, your opponent must have resigned or forfeited. If you lost but were not just mated, you probably forfeited on time, or perhaps the operator resigned manually. If there was a draw for some nonobvious reason, perhaps your opponent called your flag when he had insufficient mating material (or vice versa), or perhaps the operator agreed to a draw manually. 

						You will get a result command even if you already know the game ended -- for example, after you just checkmated your opponent. In fact, if you send the "RESULT {COMMENT}" command (discussed below), you will simply get the same thing fed back to you with "result" tacked in front. You might not always get a "result *" command, however. In particular, you won't get one in local chess engine mode when the user stops playing by selecting Reset, Edit Game, Exit or the like. 
					*/
					//					Game.SuspendPondering();
					Game.AbortSearch();
				}
				#endregion

				#region setboard
				else if( strMessage.StartsWith( "setboard " ) )
				{
					/*
					   * Chess Engine Communication Protocol
						The setboard command is the new way to set up positions, beginning in protocol version 2. 
						It is not used unless it has been selected with the feature command. 
						Here FEN is a position in Forsythe-Edwards Notation, as defined in the PGN standard. 
						Illegal positions: N-o-t-e that either setboard or edit can be used to send an illegal position to the engine. 
						The user can create any position with xboard's Edit Position command (even, say, an empty board, or a 
						board with 64 white kings and no black ones). If your engine receives a position that it considers 
						illegal, I suggest that you send the response "tellusererror Illegal position", and then respond to 
						any attempted move with "Illegal move" until the next new, edit, or setboard command.
					*/
					try
					{
						if( Game == null )
							Game = Program.Manager.CreateGame( Program.Manager.XBoardNameToProperName( Variant ) );
						Game.ClearGameState();
						Game.LoadFEN( strMessage.Substring( 9 ) );
					}
					catch /*Fen.ValidationException x*/
					{
						throw;
						//sendOutputMessage( "tellusererror Illegal position: " + x.FenMessage );
					}
				}
				#endregion

				#region undo
				else if( strMessage == "undo" )
				{
					// * Chess Engine Communication Protocol
					// If the user asks to back up one move, xboard will send you the "undo" command. xboard will not send this command without putting you in "force" mode first, so you don't have to worry about what should happen if the user asks to undo a move your engine made. (GNU Chess 4 actually switches to playing the opposite color in this case.) 
					Game.UndoMove();
				}
				#endregion

				#region remove
				else if( strMessage == "remove" )
				{
					// * Chess Engine Communication Protocol
					// If the user asks to retract a move, xboard will send you the "remove" command. It sends this command only when the user is on move. 
					// Your engine should undo the last two moves (one for each player) and continue playing the same color.
					Game.UndoMove();
					Game.UndoMove();
				}
				#endregion

				#region post 
				else if( strMessage == "post" )
				{
					// * Chess Engine Communication Protocol
					// Turn on thinking output
					createGameIfNotYetCreated();
					Game.ThinkingCallback = XBoard2Interface.callbackObject.updateThinking;
				}
				#endregion

				#region nopost
				else if( strMessage == "nopost" )
				{
					// Turn off thinking output
					createGameIfNotYetCreated();
					Game.ThinkingCallback = null;
				}
				#endregion

				#region level
				else if( strMessage.StartsWith( "level " ) )
				{
					setLevel( strMessage.Substring( "level ".Length ) );
					XBoardTimeUpdatedEvent();
				}
				#endregion

				#region option
				else if( strMessage.StartsWith( "option " ) )
				{
					//	process setting of ChessV option (if recognized)
					string optionEqualsValueString = strMessage.Substring( 7 ).Trim();
					int splitIndex = optionEqualsValueString.IndexOf( '=' );
					if( splitIndex > 0 )
					{
						string option = optionEqualsValueString.Substring( 0, splitIndex ).Trim();
						string value = optionEqualsValueString.Substring( splitIndex + 1 ).Trim();
						if( option.ToUpper() == "VARIATION OF PLAY" )
						{
							if( value.ToUpper() == "NONE" )
								variationOfPlay = 0;
							else if( value.ToUpper() == "SMALL" )
								variationOfPlay = 1;
							else if( value.ToUpper() == "MEDIUM" )
								variationOfPlay = 2;
							else if( value.ToUpper() == "LARGE" )
								variationOfPlay = 3;
						}
						else if( option.ToUpper() == "WEAKENING" )
						{
							int weakeningVal;
							bool canParse = int.TryParse( value, out weakeningVal );
							if( canParse && weakeningVal >= 0 && weakeningVal <= 15 )
								weakening = weakeningVal;
						}
					}
				}
				#endregion

				#region memory
				else if( strMessage.StartsWith( "memory " ) )
				{
					string sizeStr = strMessage.Substring( 7 ).Trim();
					int size;
					if( int.TryParse( sizeStr, out size ) )
					{
						//	find the next smallest power of 2
						int ttsize = 4096;
						while( ttsize >= size )
							ttsize /= 2;
						ttSizeInMB = ttsize;
					}
				}
				#endregion


				#region *** analysis mode commands ***
				#region analyze
				else if( strMessage == "analyze" )
				{
					// * Chess Engine Communication Protocol
					// Enter analyze mode.

					//	analysis mode is not supported
					throw new XBoardInputException( "Unsupported command: " + strMessage );
				}
				#endregion

				#region .
				else if( strMessage == "." )
				{
					//	analysis mode is not supported
					throw new XBoardInputException( "Unsupported command: " + strMessage );

					// Status update
					/*					if( Game.PlayerToPlay.Brain.IsThinking )
										{
											SendAnalyzeStatus(
												Game.PlayerToPlay.Brain.ThinkingTimeElpased,
												Game.PlayerToPlay.Brain.Search.PositionsSearchedThisIteration,
												Game.PlayerToPlay.Brain.Search.SearchDepth,
												Game.PlayerToPlay.Brain.Search.TotalPositionsToSearch - Game.PlayerToPlay.Brain.Search.SearchPositionNo,
												Game.PlayerToPlay.Brain.Search.TotalPositionsToSearch,
												Game.PlayerToPlay.Brain.Search.CurrentMoveSearched );
										}*/
				}
				#endregion

				#region exit
				else if( strMessage == "exit" )
				{
					// Exit analyze mode.
					//					Game.SuspendPondering();

					//					if( Game.IsInAnalyseMode )
					//					{
					//						Game.IsInAnalyseMode = false;
					//					}
				}
				#endregion
				#endregion


				#region *** commands we ignore ***
				#region random
				else if( strMessage == "random" )
				{
					//	ignore this
				}
				#endregion

				#region white/back
				else if( strMessage == "white" || strMessage == "black" )
				{
					//	obsolete commands as of protocol version 2 - ignore 
				}
				#endregion

				#region hard
				else if( strMessage == "hard" )
				{
					/*
					   * Chess Engine Communication Protocol
						Turn on pondering (thinking on the opponent's time, also known as "permanent brain"). xboard will not 
						make any assumption about what your default is for pondering or whether "new" affects this setting. 
					*/

					//	Pondering is not supported, so do nothing
				}
				#endregion

				#region easy
				else if( strMessage == "easy" )
				{
					// * Chess Engine Communication Protocol
					// Turn off pondering

					//	Pondering is not supported, so do nothing				
				}
				#endregion

				#region otim
				else if( strMessage.StartsWith( "otim " ) )
				{
					//	do nothing - we don't do anything differently based on the opponent's time remaining
				}
				#endregion

				#region computer
				else if( strMessage == "computer" )
				{
					// * Chess Engine Communication Protocol
					// The opponent is also a computer chess engine. Some engines alter their playing style when they receive this command.
				}
				#endregion

				#region name
				else if( strMessage.StartsWith( "name " ) )
				{
					// * Chess Engine Communication Protocol
					// This command informs the engine of its opponent's name. When the engine is playing on a chess server, xboard obtains the 
					// opponent's name from the server. When the engine is playing locally against a human user, xboard obtains the user's login 
					// name from the local operating system. When the engine is playing locally against another engine, xboard uses either the other 
					// engine's filename or the name that the other engine supplied in the myname option to the feature command. By default, xboard 
					// uses the name command only when the engine is playing on a chess server. Beginning in protocol version 2, you can change this
					// with the name option to the feature command.
				}
				#endregion

				#region accepted
				else if( strMessage.StartsWith( "accepted " ) )
				{
					//	Feature request is accepted - no action necessary
				}
				#endregion

				#region rejected
				else if( strMessage.StartsWith( "rejected " ) )
				{
					//	Feature request is rejected - no action necessary
				}
				#endregion
				#endregion


				#region *** commands we do not support ***
				#region edit
				else if( strMessage == "edit" )
				{
					/*
					   * Chess Engine Communication Protocol
						The edit command is the old way to set up positions. For compatibility with old engines, it is still used by default, but new engines may prefer to use the feature command (see below) to cause xboard to use setboard instead. The edit command puts the chess engine into a special mode, where it accepts the following subcommands: c change current piece color, initially white  
						Pa4 (for example) place pawn of current color on a4  
						xa4 (for example) empty the square a4 (not used by xboard)  
						# clear board  
						. leave edit mode  
						See the Idioms section below for additional subcommands used in ChessBase's implementation of the protocol. 
						The edit command does not change the side to move. To set up a black-on-move position, xboard uses the following command sequence: 

							new
							force
							a2a3
							edit
							<edit commands>
							.

						This sequence is used to avoid the "black" command, which is now considered obsolete and which many engines never did implement as specified in this document. 

						After an edit command is complete, if a king and a rook are on their home squares, castling is assumed to be available to them. En passant capture is assumed to be illegal on the current move regardless of the positions of the pawns. The clock for the 50 move rule starts at zero, and for purposes of the draw by repetition rule, no prior positions are deemed to have occurred. 

					*/
					throw new XBoardInputException( "Unsupported command: " + strMessage );
				}
				#endregion

				#region hint
				else if( strMessage == "hint" )
				{
					// * Chess Engine Communication Protocol
					// If the user asks for a hint, xboard sends your engine the command "hint". Your engine should respond with "Hint: xxx", where xxx is a suggested move. If there is no move to suggest, you can ignore the hint command (that is, treat it as a no-op). 
					throw new XBoardInputException( "Unsupported command: " + strMessage );
				}
				#endregion

				#region bk
				else if( strMessage == "bk" )
				{
					// * Chess Engine Communication Protocol
					// If the user selects "Book" from the xboard menu, xboard will send your engine the command "bk". You can send any text you like as the response, as long as each line begins with a blank space or tab (\t) character, and you send an empty line at the end. The text pops up in a modal information dialog. 
					throw new XBoardInputException( "Unsupported command: " + strMessage );
				}
				#endregion

				#region resume
				else if( strMessage == "resume" )
				{
					// * Chess Engine Communication Protocol
					// (These commands are new in protocol version 2 and will not be sent unless feature pause=1 is set. 
					// At this writing, xboard actually does not use the commands at all, but it or other interfaces may use them in the future.) 
					// The "pause" command puts the engine into a special state where it does not think, ponder, or otherwise consume significant CPU time. 
					// The current thinking or pondering (if any) is suspended and both player's clocks are stopped. 
					// The only command that the interface may send to the engine while it is in the paused state is "resume". 
					// The paused thinking or pondering (if any) resumes from exactly where it left off, and the clock of the player 
					// on move resumes running from where it stopped. 
					throw new XBoardInputException( "Unsupported command: " + strMessage );
				}
				#endregion

				#region pause
				else if( strMessage == "pause" )
				{
					// * Chess Engine Communication Protocol
					// See resume
					throw new XBoardInputException( "Unsupported command: " + strMessage );
				}
				#endregion

				#region rating
				else if( strMessage == "rating" )
				{
					/*
					   * Chess Engine Communication Protocol
						In ICS mode, xboard obtains the ICS opponent's rating from the "Creating:" message that appears before each game. 
						(This message may not appear on servers using outdated versions of the FICS code.) In Zippy mode, it sends these ratings on
						to the chess engine using the "rating" command. The chess engine's own rating comes first, and if either opponent is not
						rated, his rating is given as 0. In the future this command may also be used in other modes, if ratings are known. 
						Example: rating 2600 1500
					*/
					throw new XBoardInputException( "Unsupported command: " + strMessage );
				}
				#endregion

				#region ics
				else if( strMessage.StartsWith( "ics " ) )
				{
					// * Chess Engine Communication Protocol
					// If HOSTNAME is "-", the engine is playing against a local opponent; otherwise, the engine is playing on an Internet Chess Server (ICS)
					//  with the given hostname. This command is new in protocol version 2 and is not sent unless the engine has enabled it with the "feature"
					//  command. Example: "ics freechess.org" 
					throw new XBoardInputException( "Unsupported command: " + strMessage );
				}
				#endregion
				#endregion


				#region *** other input - probably a move ***
				else
				{
					createGameIfNotYetCreated();
					Movement move = Game.MoveFromDescription( strMessage, MoveNotation.XBoard );
					if( move != null )
					{
						Game.MakeMove( move, true );
						if( Game.ComputerControlled[Game.CurrentSide] )
						{
							TimeControl.StartTimer();
							List<Movement> moves = Game.Think( TimeControl );
							foreach( Movement mv in moves )
								Game.MakeMove( mv, true );
							sendMove( moves );
						}
					}
					else
						throw new XBoardInputException( "Error (unknown command): " + strMessage );
				}
				#endregion
			}
			catch( XBoardInputException e )
			{
				sendOutputMessage( e.Message );
			}
		}
		#endregion

		public static void SendCheckmate()
		{
			if( Active )
				sendOutputMessage( Game.Match.Result.Winner == 0 ? "1-0 {White mates}" : "0-1 {Black mates}" );
		}

		public static void SendStalemate()
		{
			if( Active )
				sendOutputMessage( "1/2-1/2 {Stalemate}" );
		}

		public static void SendDrawByFiftyMoveRule()
		{
			if( Active )
				sendOutputMessage( "1/2-1/2 {Draw by 50 move rule}" );
		}

		public static void SendDrawByInsufficientMaterial()
		{
			if( Active )
				sendOutputMessage( "1/2-1/2 {Draw by insufficient material}" );
		}

		public static void SendDrawByRepetition()
		{
			if( Active )
				sendOutputMessage( "1/2-1/2 {Draw by repetition}" );
		}

		#region SendMoveTime
		public static void SendMoveTime( TimeSpan timeSpan )
		{
			if( Active )
			{
				sendOutputMessage( "movetime " + timeSpan.ToString() );
			}
		}
		#endregion

		#region UpdateThinking
		public static void UpdateThinking( Dictionary<string, string> searchinfo )
		{
			int currentEval = 0;
			if( searchinfo["Score"].IndexOf( "M" ) >= 0 )
				currentEval = searchinfo["Score"].IndexOf( "-" ) >= 0
			  ? -100000 - Convert.ToInt32( searchinfo["Score"].Substring( searchinfo["Score"].IndexOf( "M" ) + 1 ) )
			  : 100000 + Convert.ToInt32( searchinfo["Score"].Substring( searchinfo["Score"].IndexOf( "M" ) + 1 ) );
			else
			{
				Double d;
				if( Double.TryParse( searchinfo["Score"], out d ) )
					currentEval = Convert.ToInt32( d * 100.0 );
			}
			sendOutputMessage(
				searchinfo["Depth"] + " " +      // send search depth
				currentEval.ToString() + " " +   // send evaluation 
				searchinfo["TimeXB"] + " " +     // send time used
				searchinfo["NodesXB"] + " " +    // send node count
				searchinfo["PV"] );              // send current PV
		}
		#endregion

		#region StartListener
		public static void StartListener()
		{
			if( Active )
			{
				ThreadListener = new Thread( listen ) { Priority = ThreadPriority.Normal };
				ThreadListener.Start();
			}
		}
		#endregion

		#region StopListener
		public static void StopListener()
		{
			if( Active )
			{
				ThreadListener.Abort();
				ThreadListener.Join();
				ThreadListener = null;
			}
		}
		#endregion
		#endregion

		#region Private Functions
		private static void listen()
		{
			while( true )
			{
				string strMessage = Console.ReadLine();
				if( strMessage == null )
				{
					Thread.Sleep( 100 );
				}
				else
				{
					XBoardInputEvent( strMessage );
				}
			}
		}

		private static void makeMove( string strMove )
		{
			/*
					See below for the syntax of moves. If the move is illegal, print an error message; 
					see the section "Commands from the engine to xboard". If the move is legal and in turn, make it. 
					If not in force mode, stop the opponent's clock, start the engine's clock, start thinking, and eventually make a move. 
					When xboard sends your engine a move, it normally sends coordinate algebraic notation. Examples: 

					Normal moves: e2e4  
					Pawn promotion: e7e8q  
					Castling: e1g1, e1c1, e8g8, e8c8  
					Bughouse/crazyhouse drop: P@h3  
					ICS Wild 0/1 castling: d1f1, d1b1, d8f8, d8b8  
					FischerRandom castling: O-O, O-O-O (oh, not zero)  

					Beginning in protocol version 2, you can use the feature command to select SAN (standard algebraic notation) instead; 
					for example, e4, Nf3, exd5, Bxf7+, Qxf7#, e8=Q, O-O, or P@h3. Note that the last form, P@h3, is a extension to the PGN 
					standard's definition of SAN, which does not support bughouse or crazyhouse. 

					xboard doesn't reliably detect illegal moves, because it does not keep track of castling unavailability due to king 
					or rook moves, or en passant availability. If xboard sends an illegal move, send back an error message so that xboard 
					can retract it and inform the user; see the section "Commands from the engine to xboard". 
				*/
			//			Game.SuspendPondering();

			throw new XBoardInputException( "Illegal move: " + strMove );
		}

		private static void sendOutputMessage( string message )
		{
			Console.WriteLine( message );
		}

		private static void sendMove( List<Movement> moves )
		{
			if( moves.Count == 1 )
				sendOutputMessage( "move " + Game.DescribeMove( moves[0], MoveNotation.XBoard ) );
			else
			{
				//	multi-leg moves such as in marseillais chess
				string movedesc = Game.DescribeMove( moves[0], MoveNotation.XBoard );
				for( int x = 1; x < moves.Count; x++ )
					movedesc += "," + Game.DescribeMove( moves[1], MoveNotation.XBoard );
				sendOutputMessage( "move " + movedesc );
			}
		}

		private static void setLevel( string strLevel )
		{
			int intPos;
			string strMoves;
			string strTime;
			string strIncrement;
			int intMoves;
			int intMinutes;
			int intSeconds;
			int intIncrement;

			intPos = strLevel.IndexOf( " " );
			strMoves = strLevel.Substring( 0, intPos );
			strLevel = strLevel.Substring( intPos + 1 );

			intPos = strLevel.IndexOf( " " );
			strTime = strLevel.Substring( 0, intPos );
			strLevel = strLevel.Substring( intPos + 1 );

			strIncrement = strLevel;
			intIncrement = int.Parse( strIncrement );

			intMoves = int.Parse( strMoves );

			intPos = strTime.IndexOf( ':' );
			if( intPos >= 0 )
			{
				intMinutes = int.Parse( strTime.Substring( 0, intPos ) );
				intSeconds = int.Parse( strTime.Substring( intPos + 1 ) );
			}
			else
			{
				intMinutes = int.Parse( strTime );
				intSeconds = 0;
			}

			string tcString = intMinutes.ToString();
			if( intSeconds > 0 )
				tcString = tcString + ":" + intSeconds.ToString();
			if( intIncrement > 0 )
				tcString = tcString + "+" + intIncrement.ToString();
			if( intMoves > 0 )
				tcString = intMoves.ToString() + "/" + tcString;
			TimeControl = new TimeControl( tcString );

		}

		private static void setTime( string strTime )
		{
			int seconds = int.Parse( strTime );
			TimeControl = new TimeControl();
			TimeControl.TimePerMove = seconds * 1000;
		}

		private static void createGameIfNotYetCreated()
		{
			if( Game == null )
				Game = Program.Manager.CreateGame( Program.Manager.XBoardNameToProperName( Variant ) );
			Game.Variation = variationOfPlay;
			Game.Weakening = weakening;
			Game.TTSizeInMB = ttSizeInMB;
			Game.ComputerControlled[0] = ForceModeOn == false && MySide == 0;
			Game.ComputerControlled[1] = ForceModeOn == false && MySide == 1;
		}
		#endregion

		private static CallbackObject callbackObject = new CallbackObject();

		private static bool ForceModeOn;

		private static int variationOfPlay;

		private static int weakening;

		private static int ttSizeInMB = 64;
	}

	class CallbackObject
	{
		//	callback function from engine to post thinking info
		public void updateThinking( Dictionary<string, string> searchinfo )
		{
			XBoard2Interface.UpdateThinking( searchinfo );
		}
	}
}
