
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace ChessV.GUI
{
  public partial class SaveGamesAsImagesForm : Form
  {
    public string GamePath { get; private set; }
    public Manager.Manager Manager { get; private set; }

    public SaveGamesAsImagesForm()
    {
      Manager = Program.Manager;
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
        GamePath = txtFolder.Text;
        string[] savedGameFiles = Directory.GetFiles(GamePath, "*.sgf");
        savedGameQueue = new Queue<string>();
        foreach (string game in savedGameFiles)
          savedGameQueue.Enqueue(game);
        timer.Start();
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      timer.Stop();
      if (savedGameQueue.Count == 0)
      {
        MessageBox.Show("Done!");
        Close();
        return;
      }
      string savedGameFile = savedGameQueue.Dequeue();
      label1.Visible = true;
      label3.Visible = true;
      lblCurrentFile.Visible = true;
      lblCurrentFile.Text = savedGameFile;
      using (TextReader reader = new StreamReader(savedGameFile))
      {
        try
        {
          Game game = Manager.LoadGame(reader);
          BoardPresentation presentation = PresentationFactory.CreatePresentation(game);
          Bitmap bitmap = presentation.Render();
          string imageFilename = savedGameFile.Replace(".sgf", ".jpg");
          bitmap.Save(imageFilename, ImageFormat.Jpeg);

        }
        catch (Exception ex)
        {
          MessageBox.Show("Failed to create game for: " + savedGameFile);
        }
        reader.Close();
      }
      timer.Start();
    }

    private Queue<string> savedGameQueue;
  }
}
