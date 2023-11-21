
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

using ChessV.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ChessV.GUI
{
  public partial class AutomatedMatchesProgressForm : Form
  {
    // *** PUBLIC PROPERTIES *** //

    public MatchSet MatchSet { get; private set; }

    public string OutputFileName { get; set; }

    public string Player1Name { get; private set; }
    public string Player2Name { get; private set; }
    public double Player1Wins { get; private set; }
    public double Player2Wins { get; private set; }


    // *** CONSTRUCTION *** //

    public AutomatedMatchesProgressForm(MatchSet matchSet, string workingPath, string outputFileName)
    {
      this.workingPath = workingPath;
      OutputFileName = outputFileName;
      MatchSet = matchSet;
      inProgress = false;
      InitializeComponent();
    }


    // *** EVENT HANDLERS *** //

    private void AutomatedMatchesProgressForm_Load(object sender, EventArgs e)
    {
      foreach (MatchRecord match in MatchSet)
      {
        ListViewItem lvi = new ListViewItem(match.ID);
        lvi.SubItems.Add(
            match.SavedGameFile.IndexOf(Path.DirectorySeparatorChar) >= 0
          ? match.SavedGameFile.Substring(match.SavedGameFile.LastIndexOf(Path.DirectorySeparatorChar) + 1)
          : match.SavedGameFile);
        lvi.SubItems.Add(match.TimeControl);
        if (match.Engines[0] == null)
        {
          EngineConfiguration engine = Program.Manager.LookupEngineByPartialName(match.EngineNames[0]);
          if (engine == null)
            throw new Exception("Automated Matches: can't find an engine with a name containing '" + match.EngineNames[0] + "'");
          match.Engines[0] = engine;
        }
        lvi.SubItems.Add(match.EngineNames[0]);
        if (match.Engines[1] == null)
        {
          EngineConfiguration engine = Program.Manager.LookupEngineByPartialName(match.EngineNames[1]);
          if (engine == null)
            throw new Exception("Automated Matches: can't find an engine with a name containing '" + match.EngineNames[1] + "'");
          match.Engines[1] = engine;
        }
        lvi.SubItems.Add(match.EngineNames[1]);
        lvi.SubItems.Add("(pending)");
        lvi.SubItems.Add("(pending)");
        lvi.Tag = match;
        listMatches.Items.Add(lvi);
      }
      if (File.Exists(OutputFileName))
      {
        //	we are resuming a previous run - 
        //	load the results already completed
        TextReader log = new StreamReader(OutputFileName);
        //	skip header row
        string input = log.ReadLine();
        while ((input = log.ReadLine()) != null)
        {
          string[] split = input.Split('\t');
          string id = split[0];
          foreach (ListViewItem lvi in listMatches.Items)
          {
            MatchRecord match = (MatchRecord)lvi.Tag;
            if (match.ID == id)
            {
              match.EngineNames[0] = split[1];
              match.EngineNames[1] = split[2];
              match.PlayerNames[0] = split[3];
              match.PlayerNames[1] = split[4];
              match.Result = split[5];
              match.Winner = split[6];
              lvi.SubItems[5].Text = match.Result;
              lvi.SubItems[6].Text = match.Winner;
              updateResultCounts(match.Result, match.Winner);
              break;
            }
          }
        }
        log.Close();
      }
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      if (!inProgress)
      {
        currentMatches = new List<MatchRecord>();
        if (File.Exists(OutputFileName))
          outputFile = new StreamWriter(OutputFileName, true);
        else
        {
          outputFile = new StreamWriter(OutputFileName);
          outputFile.WriteLine("ID\tEngine1\tEngine2\tPlayer1\tPlayer2\tResult\tWinner");
          outputFile.Flush();
        }
        inProgress = true;
        timer.Start();
        nextGame();
      }
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      foreach (MatchRecord currentMatch in currentMatches)
      {
        Game currentGame = currentMatch.Game;
        if (!currentGame.Result.IsNone)
        {
          timer.Stop();
          //	find the list item for this match
          ListViewItem listitem = null;
          foreach (ListViewItem lvi in listMatches.Items)
          {
            if (lvi.Tag == currentMatch)
            {
              listitem = lvi;
              break;
            }
          }
          //	current game is over, record result
          listitem.SubItems[5].Text = currentGame.Result.VerboseString;
          string winner = currentGame.Result.IsDraw ? "-" : currentMatch.PlayerNames[currentGame.Result.Winner];
          listitem.SubItems[6].Text = winner;
          outputFile.WriteLine(currentMatch.ID + "\t" + currentMatch.EngineNames[0] + "\t" +
            currentMatch.EngineNames[1] + "\t" + currentMatch.PlayerNames[0] + "\t" +
            currentMatch.PlayerNames[1] + "\t" + currentGame.Result.VerboseString + "\t" + winner);
          outputFile.Flush();
          currentMatch.Result = currentGame.Result.VerboseString;
          currentMatches.Remove(currentMatch);
          Player p0 = currentGame.Match.GetPlayer(0);
          if (p0 is XBoardEngine)
            ((XBoardEngine)p0).sendQuit();
          Player p1 = currentGame.Match.GetPlayer(1);
          if (p1 is XBoardEngine)
            ((XBoardEngine)p1).sendQuit();
          updateResultCounts(currentGame.Result.ShortString, winner);
          currentGame.ReleaseMemoryAllocations();
          currentMatch.Game = null;
          //	perform garbage collection
          GC.Collect();
          //	start next game
          timer.Start();
          nextGame();
          break;
        }
      }
    }


    // *** HELPER FUNCTIONS *** //

    private void nextGame()
    {
      //	find next unfinished game to start
      MatchRecord nextMatch = null;
      foreach (MatchRecord match in MatchSet)
      {
        if (match.Result == null)
        {
          nextMatch = match;
          break;
        }
      }
      if (nextMatch != null)
      {
        StreamReader reader = new StreamReader(nextMatch.SavedGameFile);
        Game currentGame = Program.Manager.LoadGame(reader);
        currentGame.StartMatch();
        TimeControl timeControl = new TimeControl(nextMatch.TimeControl);
        currentGame.ComputerControlled[0] = true;
        currentGame.ComputerControlled[1] = true;
        if (nextMatch.Engines[0] == Program.Manager.InternalEngine)
          currentGame.AddInternalEngine(0);
        else
          currentGame.AddEngine(Program.Manager.EngineLibrary.AdaptEngine(currentGame, nextMatch.Engines[0]), 0);
        if (nextMatch.Engines[1] == Program.Manager.InternalEngine)
          currentGame.AddInternalEngine(1);
        else
          currentGame.AddEngine(Program.Manager.EngineLibrary.AdaptEngine(currentGame, nextMatch.Engines[1]), 1);
        currentGame.Match.SetTimeControl(timeControl);
        currentGame.IsAutomatedMatch = true;
        nextMatch.PlayerNames[0] = currentGame.PerformSymbolExpansion(nextMatch.PlayerNames[0]);
        nextMatch.PlayerNames[1] = currentGame.PerformSymbolExpansion(nextMatch.PlayerNames[1]);
        nextMatch.Game = currentGame;
        currentMatches.Add(nextMatch);
        GameForm gameForm = new GameForm(currentGame);
        gameForm.Show();
      }
      else
      {
        timer.Stop();
        outputFile.Close();
      }
    }

    protected void updateResultCounts(string result, string winner)
    {
      if (result.Length >= 3 && result.Substring(0, 3) == "1/2")
      {
        Player1Wins += 0.5;
        Player2Wins += 0.5;
      }
      else
      {
        if (Player1Name == winner)
          Player1Wins++;
        else if (Player2Name == winner)
          Player2Wins++;
        else
        {
          if (result.Length >= 3 && result.Substring(0, 3) == "1-0" && Player1Name == null)
          {
            Player1Name = winner;
            lblPlayer1Name.Text = Player1Name + ": ";
            Player1Wins++;
            lblPlayer1Name.Visible = true;
            lblPlayer1WinCount.Visible = true;
          }
          else if (result.Length >= 3 && result.Substring(0, 3) == "0-1" && Player2Name == null)
          {
            Player2Name = winner;
            lblPlayer2Name.Text = Player2Name + ": ";
            Player2Wins++;
            lblPlayer2Name.Visible = true;
            lblPlayer2WinCount.Visible = true;
          }
        }
        lblPlayer1WinCount.Text = Player1Wins.ToString();
        lblPlayer2WinCount.Text = Player2Wins.ToString();
      }
    }


    // *** PRIVATE DATA *** //

    private bool inProgress;
    private List<MatchRecord> currentMatches;
    private TextWriter outputFile;
    private string workingPath;
  }
}
