
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

namespace ChessV.GUI
{
	partial class AddMoveToGamesProgressForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && (components != null) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.listGames = new System.Windows.Forms.ListView();
			this.hdrGameFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.hdrResult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// listGames
			// 
			this.listGames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrGameFile,
            this.hdrResult});
			this.listGames.Location = new System.Drawing.Point(12, 12);
			this.listGames.Name = "listGames";
			this.listGames.Size = new System.Drawing.Size(476, 238);
			this.listGames.TabIndex = 0;
			this.listGames.UseCompatibleStateImageBehavior = false;
			this.listGames.View = System.Windows.Forms.View.Details;
			// 
			// hdrGameFile
			// 
			this.hdrGameFile.Text = "Saved Game File";
			this.hdrGameFile.Width = 180;
			// 
			// hdrResult
			// 
			this.hdrResult.Text = "Result";
			this.hdrResult.Width = 140;
			// 
			// timer
			// 
			this.timer.Interval = 1000;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// AddMoveToGamesProgressForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LemonChiffon;
			this.ClientSize = new System.Drawing.Size(500, 262);
			this.Controls.Add(this.listGames);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AddMoveToGamesProgressForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Add Move To Games Progress";
			this.Load += new System.EventHandler(this.AddMoveToGamesProgressForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listGames;
		private System.Windows.Forms.ColumnHeader hdrGameFile;
		private System.Windows.Forms.ColumnHeader hdrResult;
		private System.Windows.Forms.Timer timer;
	}
}