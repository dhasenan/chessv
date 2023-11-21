
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
using System.IO;
using System.Windows.Forms;

namespace ChessV.GUI
{
  public partial class TranspositionDetectorForm : Form
  {
    public TranspositionDetectorForm()
    {
      manager = Program.Manager;

      InitializeComponent();
    }

    private void TranspositionDetectorForm_Load(object sender, EventArgs e)
    {
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      if (!Directory.Exists(txtFolder.Text))
      {
        MessageBox.Show("You must select a valid folder containing saved games");
        return;
      }

      listResults.Items.Clear();
      hashcodes = new Dictionary<string, List<ulong>>();
      savedGames = Directory.GetFiles(txtFolder.Text, "*.sgf");
      foreach (string game in savedGames)
      {
        List<ulong> gameHashes = new List<ulong>();
        using (TextReader reader = new StreamReader(game))
        {
          try
          {
            Game loadedGame = manager.LoadGame(reader, gameHashes);
            reader.Close();
            hashcodes.Add(game, gameHashes);
          }
          catch (Exception ex)
          {
            ExceptionForm exceptionForm = new ExceptionForm(ex, null);
            exceptionForm.ShowDialog();
          }
        }
      }
      int total = 0;
      foreach (string game in savedGames)
      {
        //	find the position hashcodes for this game
        List<ulong> hashes = hashcodes[game];
        //	identify deepest match and with which game
        int deepestMatch = 0;
        string deepestMatchGame = "";
        //	iterate through every other game
        foreach (KeyValuePair<string, List<ulong>> otherGame in hashcodes)
        {
          //	don't compare with ourself
          if (otherGame.Key != game)
          {
            List<ulong> otherHashes = otherGame.Value;
            //	iterate through all hashcodes within game backwards
            for (int x = hashes.Count - 1; x >= 0; x--)
            {
              ulong thisHash = hashes[x];
              if (otherHashes.Contains(thisHash) && x >= deepestMatch)
              {
                deepestMatch = x;
                deepestMatchGame = otherGame.Key;
              }
            }
          }
        }
        ListViewItem lvi = new ListViewItem(game.Substring(game.LastIndexOf('\\') + 1));
        lvi.SubItems.Add(hashes.Count.ToString());
        lvi.SubItems.Add((deepestMatch + 1).ToString());
        lvi.SubItems.Add(deepestMatchGame.Substring(deepestMatchGame.LastIndexOf('\\') + 1));
        listResults.Items.Add(lvi);
        total += deepestMatch + 1;
      }
      lblTotal.Text = total.ToString();
    }

    Dictionary<string, List<ulong>> hashcodes;
    protected string[] savedGames;
    protected int currentGame;
    protected Manager.Manager manager;
  }
}
