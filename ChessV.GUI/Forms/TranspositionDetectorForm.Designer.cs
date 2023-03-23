
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

namespace ChessV.GUI
{
	partial class TranspositionDetectorForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TranspositionDetectorForm));
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnBrowseFolder = new System.Windows.Forms.Button();
			this.txtFolder = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.listResults = new System.Windows.Forms.ListView();
			this.chFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chNoMoves = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chMoveNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chOtherGame = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.label3 = new System.Windows.Forms.Label();
			this.lblTotal = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
			this.btnCancel.Location = new System.Drawing.Point(312, 307);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(123, 32);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "     &Cancel";
			this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnStart
			// 
			this.btnStart.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnStart.Image = global::ChessV.GUI.Properties.Resources.icon_ok;
			this.btnStart.Location = new System.Drawing.Point(120, 307);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(123, 32);
			this.btnStart.TabIndex = 3;
			this.btnStart.Text = "    Start";
			this.btnStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnBrowseFolder
			// 
			this.btnBrowseFolder.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnBrowseFolder.Image = global::ChessV.GUI.Properties.Resources.icon_open_folder;
			this.btnBrowseFolder.Location = new System.Drawing.Point(464, 20);
			this.btnBrowseFolder.Name = "btnBrowseFolder";
			this.btnBrowseFolder.Size = new System.Drawing.Size(32, 28);
			this.btnBrowseFolder.TabIndex = 1;
			this.btnBrowseFolder.UseVisualStyleBackColor = true;
			// 
			// txtFolder
			// 
			this.txtFolder.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.txtFolder.Location = new System.Drawing.Point(109, 25);
			this.txtFolder.Name = "txtFolder";
			this.txtFolder.Size = new System.Drawing.Size(345, 20);
			this.txtFolder.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(64, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "Folder:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 59);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 13);
			this.label2.TabIndex = 21;
			this.label2.Text = "Results:";
			// 
			// listResults
			// 
			this.listResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFile,
            this.chNoMoves,
            this.chMoveNo,
            this.chOtherGame});
			this.listResults.FullRowSelect = true;
			this.listResults.GridLines = true;
			this.listResults.HideSelection = false;
			this.listResults.Location = new System.Drawing.Point(15, 75);
			this.listResults.Name = "listResults";
			this.listResults.Size = new System.Drawing.Size(533, 207);
			this.listResults.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.listResults.TabIndex = 2;
			this.listResults.UseCompatibleStateImageBehavior = false;
			this.listResults.View = System.Windows.Forms.View.Details;
			// 
			// chFile
			// 
			this.chFile.Text = "Game File";
			this.chFile.Width = 192;
			// 
			// chNoMoves
			// 
			this.chNoMoves.Text = "# Moves";
			// 
			// chMoveNo
			// 
			this.chMoveNo.Text = "Move #";
			// 
			// chOtherGame
			// 
			this.chOtherGame.Text = "Transposes With";
			this.chOtherGame.Width = 192;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(474, 285);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(34, 13);
			this.label3.TabIndex = 22;
			this.label3.Text = "Total:";
			// 
			// lblTotal
			// 
			this.lblTotal.AutoSize = true;
			this.lblTotal.Location = new System.Drawing.Point(514, 285);
			this.lblTotal.Name = "lblTotal";
			this.lblTotal.Size = new System.Drawing.Size(13, 13);
			this.lblTotal.TabIndex = 23;
			this.lblTotal.Text = "0";
			// 
			// TranspositionDetectorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			// this.BackColor = System.Drawing.Color.LemonChiffon;
			this.ClientSize = new System.Drawing.Size(560, 355);
			this.Controls.Add(this.lblTotal);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.listResults);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.btnBrowseFolder);
			this.Controls.Add(this.txtFolder);
			this.Controls.Add(this.label1);
			this.Name = "TranspositionDetectorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Transposition Detector";
			this.Load += new System.EventHandler(this.TranspositionDetectorForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnBrowseFolder;
		private System.Windows.Forms.TextBox txtFolder;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListView listResults;
		private System.Windows.Forms.ColumnHeader chFile;
		private System.Windows.Forms.ColumnHeader chMoveNo;
		private System.Windows.Forms.ColumnHeader chOtherGame;
		private System.Windows.Forms.ColumnHeader chNoMoves;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblTotal;
	}
}