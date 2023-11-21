
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
using System.Windows.Forms;

namespace ChessV.GUI
{
  public partial class AddMoveToGamesForm : Form
  {
    public string GamePath { get; private set; }
    public int? FixedDepth { get; private set; }
    public int? FixedTimeMinutes { get; private set; }
    public int Variation { get; private set; }

    public AddMoveToGamesForm()
    {
      InitializeComponent();
    }

    private void btnBrowseFolder_Click(object sender, EventArgs e)
    {
      if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
      {
        txtFolder.Text = folderBrowserDialog.SelectedPath;
      }
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      if (!Directory.Exists(txtFolder.Text))
      {
        MessageBox.Show("You must select a folder of ChessV saved game files (*.sgf)");
        return;
      }
      else
      {
        if (optFixedDepth.Checked)
          FixedDepth = Convert.ToInt32(txtDepth.Text);
        else
          FixedTimeMinutes = Convert.ToInt32(txtTimeMinutes.Text);
        GamePath = txtFolder.Text;
        Variation = pickVariation.SelectedIndex;
        DialogResult = DialogResult.OK;
        Close();
      }
    }

    private void AddMoveToGamesForm_Load(object sender, EventArgs e)
    {
      pickVariation.SelectedIndex = 0;
    }
  }
}
