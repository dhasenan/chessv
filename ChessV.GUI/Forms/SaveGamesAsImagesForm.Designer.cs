
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
	partial class SaveGamesAsImagesForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveGamesAsImagesForm));
			this.btnBrowseFolder = new System.Windows.Forms.Button();
			this.txtFolder = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.label3 = new System.Windows.Forms.Label();
			this.lblCurrentFile = new System.Windows.Forms.Label();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// btnBrowseFolder
			// 
			this.btnBrowseFolder.Image = global::ChessV.GUI.Properties.Resources.icon_open_folder;
			this.btnBrowseFolder.Location = new System.Drawing.Point(435, 28);
			this.btnBrowseFolder.Name = "btnBrowseFolder";
			this.btnBrowseFolder.Size = new System.Drawing.Size(32, 28);
			this.btnBrowseFolder.TabIndex = 8;
			this.btnBrowseFolder.UseVisualStyleBackColor = true;
			this.btnBrowseFolder.Click += new System.EventHandler(this.btnBrowseFolder_Click);
			// 
			// txtFolder
			// 
			this.txtFolder.Location = new System.Drawing.Point(80, 33);
			this.txtFolder.Name = "txtFolder";
			this.txtFolder.Size = new System.Drawing.Size(345, 20);
			this.txtFolder.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(35, 36);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Folder:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
			this.btnCancel.Location = new System.Drawing.Point(283, 101);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(123, 32);
			this.btnCancel.TabIndex = 15;
			this.btnCancel.Text = "     &Cancel";
			this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnStart
			// 
			this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStart.Image = global::ChessV.GUI.Properties.Resources.icon_ok;
			this.btnStart.Location = new System.Drawing.Point(91, 101);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(123, 32);
			this.btnStart.TabIndex = 14;
			this.btnStart.Text = "    Start";
			this.btnStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(105, 66);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(63, 13);
			this.label3.TabIndex = 17;
			this.label3.Text = "Current File:";
			this.label3.Visible = false;
			// 
			// lblCurrentFile
			// 
			this.lblCurrentFile.AutoSize = true;
			this.lblCurrentFile.Location = new System.Drawing.Point(174, 66);
			this.lblCurrentFile.Name = "lblCurrentFile";
			this.lblCurrentFile.Size = new System.Drawing.Size(0, 13);
			this.lblCurrentFile.TabIndex = 18;
			this.lblCurrentFile.Visible = false;
			// 
			// timer
			// 
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// SaveGamesAsImagesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			// this.BackColor = System.Drawing.Color.LemonChiffon;
			this.ClientSize = new System.Drawing.Size(503, 151);
			this.Controls.Add(this.lblCurrentFile);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.btnBrowseFolder);
			this.Controls.Add(this.txtFolder);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "SaveGamesAsImagesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Save Games As Images";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnBrowseFolder;
		private System.Windows.Forms.TextBox txtFolder;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblCurrentFile;
		private System.Windows.Forms.Timer timer;
	}
}