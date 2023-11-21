
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
using System.IO;
using System.Windows.Forms;

namespace ChessV.GUI
{
  public partial class AddMoveToGamesProgressForm : Form
  {
    // *** PROPERTIES *** //

    public string Path { get; private set; }
    public int? FixedDepth { get; private set; }
    public int? FixedTimeMinutes { get; private set; }
    public int Variation { get; private set; }
    public Manager.Manager Manager { get; private set; }


    // *** CONSTRUCTION *** //

    #region Constructor
    public AddMoveToGamesProgressForm(string path, int? fixedDepth, int? fixedTimeMinutes, int variation)
    {
      Manager = Program.Manager;
      Path = path;
      FixedDepth = fixedDepth;
      FixedTimeMinutes = fixedTimeMinutes;
      Variation = variation;
      currentGame = -1;

      InitializeComponent();
    }
    #endregion


    // *** EVENT HANDLERS *** //

    #region Event Handlers
    #region AddMoveToGamesProgressForm Load event
    private void AddMoveToGamesProgressForm_Load(object sender, EventArgs e)
    {
      savedGames = Directory.GetFiles(Path, "*.sgf");
      foreach (string game in savedGames)
      {
        ListViewItem lvi = new ListViewItem(game.Substring(game.LastIndexOf('\\') + 1));
        lvi.SubItems.Add("(pending)");
        listGames.Items.Add(lvi);
      }
      timer.Start();
    }
    #endregion

    #region Timer Tick Event
    private void timer_Tick(object sender, EventArgs e)
    {
      timer.Stop();
      currentGame++;
      if (currentGame < savedGames.Length)
      {
        string filename = savedGames[currentGame];
        using (TextReader reader = new StreamReader(filename))
        {
          try
          {
            Game loadedGame = Manager.LoadGame(reader);
            reader.Close();
            loadedGame.Variation = Variation;
            loadedGame.TTSizeInMB = 1024;
            loadedGame.StartMatch();
            loadedGame.ComputerControlled[loadedGame.CurrentSide] = true;
            loadedGame.ComputerControlled[loadedGame.CurrentSide ^ 1] = false;
            loadedGame.AddInternalEngine(loadedGame.CurrentSide);
            loadedGame.AddHuman(loadedGame.CurrentSide ^ 1);
            TimeControl timeControl = new TimeControl();
            if (FixedDepth != null)
            {
              timeControl.Infinite = true;
              timeControl.PlyLimit = (int)FixedDepth;
            }
            else if (FixedTimeMinutes != null)
            {
              timeControl.TimePerMove = (int)FixedTimeMinutes * 1000 * 60;
            }
            loadedGame.Match.SetTimeControl(timeControl);
            loadedGame.MovePlayed += movePlayed;
            loadedGame.ThinkingCallback += updateThinking;
            //	Create and show the GameForm (the actual Game GUI)
            gameForm = new GameForm(loadedGame);
            gameForm.Show();
          }
          catch (Exception ex)
          {
            ExceptionForm exceptionForm = new ExceptionForm(ex, null);
            exceptionForm.ShowDialog();
          }
        }
      }
    }
    #endregion

    #region MovePlayed event handler
    protected void movePlayed(MoveInfo move)
    {
      if (gameForm != null)
      {
        string moveDescription = gameForm.Game.DescribeMove(move, MoveNotation.StandardAlgebraic);
        listGames.Items[currentGame].SubItems[1].Text = moveDescription + " (" + currentScore + ")";
        gameForm.Close();
        TextWriter output = new StreamWriter(savedGames[currentGame]);
        gameForm.Game.SaveGame(output);
        output.Close();
        gameForm = null;
      }
      else
      {
        Application.DoEvents();
        GC.Collect();
        timer.Start();
      }
    }
    #endregion

    #region updateThinking event handler
    protected void updateThinking(Dictionary<string, string> searchinfo)
    {
      currentScore = searchinfo["Score"];
    }
    #endregion
    #endregion


    // *** INTERNAL DATA *** //

    protected string[] savedGames;
    protected int currentGame;
    protected GameForm gameForm;
    protected string currentScore;
  }
}
