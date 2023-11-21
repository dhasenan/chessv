
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
using System.IO;
using System.Windows.Forms;

namespace ChessV.GUI
{
  public partial class AutomatedMatchesSettingsForm : Form
  {
    public AutomatedMatchesSettingsForm()
    {
      InitializeComponent();
    }

    private void btnBrowseControlFile_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog() == DialogResult.OK)
        txtControlFile.Text = openFileDialog.FileName;
    }

    private void btnBrowseOutputFile_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog() == DialogResult.OK)
        txtOutputFile.Text = openFileDialog.FileName;
    }

    private void chkResume_CheckedChanged(object sender, EventArgs e)
    {
      txtOutputFile.Enabled = chkResume.Checked;
      btnBrowseOutputFile.Enabled = chkResume.Checked;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      if (!File.Exists(txtControlFile.Text))
      {
        MessageBox.Show("You must select a control file listing the matches");
        return;
      }
      ControlFile = txtControlFile.Text;
      if (chkResume.Checked)
      {
        if (!File.Exists(txtOutputFile.Text))
        {
          MessageBox.Show("To resume, you must select the output file for the previous run you wish to continue");
          return;
        }
        OutputFileToResume = txtOutputFile.Text;
      }
      DialogResult = DialogResult.OK;
      Close();
    }

    public string ControlFile { get; private set; }
    public string OutputFileToResume { get; private set; }
  }
}
