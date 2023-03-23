
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

namespace ChessV.GUI
{
	partial class AutomatedMatchesProgressForm
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
			this.listMatches = new System.Windows.Forms.ListView();
			this.hdrID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.hdrSaveGameFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.hdrTimeControl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.hdrPlayer1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.hdrPlayer2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.hdrResult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.hdrWinner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnStart = new System.Windows.Forms.Button();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.lblPlayer1Name = new System.Windows.Forms.Label();
			this.lblPlayer2Name = new System.Windows.Forms.Label();
			this.lblPlayer1WinCount = new System.Windows.Forms.Label();
			this.lblPlayer2WinCount = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// listMatches
			// 
			this.listMatches.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrID,
            this.hdrSaveGameFile,
            this.hdrTimeControl,
            this.hdrPlayer1,
            this.hdrPlayer2,
            this.hdrResult,
            this.hdrWinner});
			this.listMatches.Location = new System.Drawing.Point(12, 12);
			this.listMatches.Name = "listMatches";
			this.listMatches.Size = new System.Drawing.Size(1030, 295);
			this.listMatches.TabIndex = 0;
			this.listMatches.UseCompatibleStateImageBehavior = false;
			this.listMatches.View = System.Windows.Forms.View.Details;
			// 
			// hdrID
			// 
			this.hdrID.Text = "ID";
			this.hdrID.Width = 80;
			// 
			// hdrSaveGameFile
			// 
			this.hdrSaveGameFile.Text = "Saved Game File";
			this.hdrSaveGameFile.Width = 120;
			// 
			// hdrTimeControl
			// 
			this.hdrTimeControl.Text = "Time Control";
			this.hdrTimeControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.hdrTimeControl.Width = 100;
			// 
			// hdrPlayer1
			// 
			this.hdrPlayer1.Text = "Player 1";
			this.hdrPlayer1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.hdrPlayer1.Width = 120;
			// 
			// hdrPlayer2
			// 
			this.hdrPlayer2.Text = "Player 2";
			this.hdrPlayer2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.hdrPlayer2.Width = 120;
			// 
			// hdrResult
			// 
			this.hdrResult.Text = "Game Result";
			this.hdrResult.Width = 300;
			// 
			// hdrWinner
			// 
			this.hdrWinner.Text = "Winner";
			this.hdrWinner.Width = 150;
			// 
			// btnStart
			// 
			this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStart.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnStart.Image = global::ChessV.GUI.Properties.Resources.icon_next;
			this.btnStart.Location = new System.Drawing.Point(469, 328);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(123, 32);
			this.btnStart.TabIndex = 12;
			this.btnStart.Text = "Start Matches";
			this.btnStart.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// timer
			// 
			this.timer.Interval = 1000;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// lblPlayer1Name
			// 
			this.lblPlayer1Name.Location = new System.Drawing.Point(12, 317);
			this.lblPlayer1Name.Name = "lblPlayer1Name";
			this.lblPlayer1Name.Size = new System.Drawing.Size(136, 18);
			this.lblPlayer1Name.TabIndex = 13;
			this.lblPlayer1Name.Text = "Player 1:";
			this.lblPlayer1Name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblPlayer1Name.Visible = false;
			// 
			// lblPlayer2Name
			// 
			this.lblPlayer2Name.Location = new System.Drawing.Point(15, 336);
			this.lblPlayer2Name.Name = "lblPlayer2Name";
			this.lblPlayer2Name.Size = new System.Drawing.Size(133, 18);
			this.lblPlayer2Name.TabIndex = 14;
			this.lblPlayer2Name.Text = "Player 2:";
			this.lblPlayer2Name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblPlayer2Name.Visible = false;
			// 
			// lblPlayer1WinCount
			// 
			this.lblPlayer1WinCount.AutoSize = true;
			this.lblPlayer1WinCount.Location = new System.Drawing.Point(154, 320);
			this.lblPlayer1WinCount.Name = "lblPlayer1WinCount";
			this.lblPlayer1WinCount.Size = new System.Drawing.Size(13, 13);
			this.lblPlayer1WinCount.TabIndex = 15;
			this.lblPlayer1WinCount.Text = "0";
			this.lblPlayer1WinCount.Visible = false;
			// 
			// lblPlayer2WinCount
			// 
			this.lblPlayer2WinCount.AutoSize = true;
			this.lblPlayer2WinCount.Location = new System.Drawing.Point(154, 339);
			this.lblPlayer2WinCount.Name = "lblPlayer2WinCount";
			this.lblPlayer2WinCount.Size = new System.Drawing.Size(13, 13);
			this.lblPlayer2WinCount.TabIndex = 16;
			this.lblPlayer2WinCount.Text = "0";
			this.lblPlayer2WinCount.Visible = false;
			// 
			// AutomatedMatchesProgressForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			// this.BackColor = System.Drawing.Color.LemonChiffon;
			this.ClientSize = new System.Drawing.Size(1054, 372);
			this.Controls.Add(this.lblPlayer2WinCount);
			this.Controls.Add(this.lblPlayer1WinCount);
			this.Controls.Add(this.lblPlayer2Name);
			this.Controls.Add(this.lblPlayer1Name);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.listMatches);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AutomatedMatchesProgressForm";
			this.Text = "AutomatedMatchesProgressForm";
			this.Load += new System.EventHandler(this.AutomatedMatchesProgressForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listMatches;
		private System.Windows.Forms.ColumnHeader hdrID;
		private System.Windows.Forms.ColumnHeader hdrSaveGameFile;
		private System.Windows.Forms.ColumnHeader hdrTimeControl;
		private System.Windows.Forms.ColumnHeader hdrPlayer1;
		private System.Windows.Forms.ColumnHeader hdrPlayer2;
		private System.Windows.Forms.ColumnHeader hdrResult;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.ColumnHeader hdrWinner;
		private System.Windows.Forms.Label lblPlayer1Name;
		private System.Windows.Forms.Label lblPlayer2Name;
		private System.Windows.Forms.Label lblPlayer1WinCount;
		private System.Windows.Forms.Label lblPlayer2WinCount;
	}
}