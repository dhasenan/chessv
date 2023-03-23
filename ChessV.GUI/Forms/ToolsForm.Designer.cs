
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
	partial class ToolsForm
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
			this.btnRunBatch = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnAddMoveToGames = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.btnCreateGameReferenceDocs = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.btnSaveGamesAsImages = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.btnTranspositionDetector = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnRunBatch
			// 
			this.btnRunBatch.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnRunBatch.Location = new System.Drawing.Point(23, 25);
			this.btnRunBatch.Name = "btnRunBatch";
			this.btnRunBatch.Size = new System.Drawing.Size(166, 32);
			this.btnRunBatch.TabIndex = 0;
			this.btnRunBatch.Text = "Run Games in Batch";
			this.btnRunBatch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnRunBatch.UseVisualStyleBackColor = true;
			this.btnRunBatch.Click += new System.EventHandler(this.btnRunBatch_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(220, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(277, 32);
			this.label1.TabIndex = 13;
			this.label1.Text = "Performs automated running of a batch of games as specified in a control file";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(201, 330);
			this.btnClose.MaximumSize = new System.Drawing.Size(123, 32);
			this.btnClose.MinimumSize = new System.Drawing.Size(123, 32);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(123, 32);
			this.btnClose.TabIndex = 5;
			this.btnClose.Text = "&Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnAddMoveToGames
			// 
			this.btnAddMoveToGames.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnAddMoveToGames.Location = new System.Drawing.Point(23, 82);
			this.btnAddMoveToGames.Name = "btnAddMoveToGames";
			this.btnAddMoveToGames.Size = new System.Drawing.Size(166, 32);
			this.btnAddMoveToGames.TabIndex = 1;
			this.btnAddMoveToGames.Text = "Add Move to Games";
			this.btnAddMoveToGames.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnAddMoveToGames.UseVisualStyleBackColor = true;
			this.btnAddMoveToGames.Click += new System.EventHandler(this.btnAddMoveToGames_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(220, 82);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(277, 32);
			this.label2.TabIndex = 16;
			this.label2.Text = "Opens every saved game in a folder and plays an additional move, logging the resu" +
    "lt";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnCreateGameReferenceDocs
			// 
			this.btnCreateGameReferenceDocs.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnCreateGameReferenceDocs.Location = new System.Drawing.Point(23, 253);
			this.btnCreateGameReferenceDocs.Name = "btnCreateGameReferenceDocs";
			this.btnCreateGameReferenceDocs.Size = new System.Drawing.Size(166, 32);
			this.btnCreateGameReferenceDocs.TabIndex = 4;
			this.btnCreateGameReferenceDocs.Text = "Create Game Reference Docs";
			this.btnCreateGameReferenceDocs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnCreateGameReferenceDocs.UseVisualStyleBackColor = true;
			this.btnCreateGameReferenceDocs.Click += new System.EventHandler(this.btnCreateGameReferenceDocs_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(220, 253);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(277, 32);
			this.label3.TabIndex = 18;
			this.label3.Text = "Creates HTML pages of reference materials for all currently defined games";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnSaveGamesAsImages
			// 
			this.btnSaveGamesAsImages.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSaveGamesAsImages.Location = new System.Drawing.Point(23, 139);
			this.btnSaveGamesAsImages.Name = "btnSaveGamesAsImages";
			this.btnSaveGamesAsImages.Size = new System.Drawing.Size(166, 32);
			this.btnSaveGamesAsImages.TabIndex = 2;
			this.btnSaveGamesAsImages.Text = "Save Games as Images";
			this.btnSaveGamesAsImages.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnSaveGamesAsImages.UseVisualStyleBackColor = true;
			this.btnSaveGamesAsImages.Click += new System.EventHandler(this.btnSaveGamesAsImages_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(220, 139);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(277, 32);
			this.label4.TabIndex = 20;
			this.label4.Text = "Opens every saved game in a folder and saves a JPG image of the position";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnTranspositionDetector
			// 
			this.btnTranspositionDetector.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnTranspositionDetector.Location = new System.Drawing.Point(23, 196);
			this.btnTranspositionDetector.Name = "btnTranspositionDetector";
			this.btnTranspositionDetector.Size = new System.Drawing.Size(166, 32);
			this.btnTranspositionDetector.TabIndex = 3;
			this.btnTranspositionDetector.Text = "Transposition Detector";
			this.btnTranspositionDetector.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnTranspositionDetector.UseVisualStyleBackColor = true;
			this.btnTranspositionDetector.Click += new System.EventHandler(this.btnTranspositionDetector_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(220, 196);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(277, 32);
			this.label5.TabIndex = 22;
			this.label5.Text = "Looks at all saved games in a folder to find the deepest places where the positio" +
    "n is the same";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ToolsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LemonChiffon;
			this.ClientSize = new System.Drawing.Size(524, 374);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.btnTranspositionDetector);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnSaveGamesAsImages);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnCreateGameReferenceDocs);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnAddMoveToGames);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnRunBatch);
			this.Name = "ToolsForm";
			this.Text = "ChessV Tools";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnRunBatch;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnAddMoveToGames;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnCreateGameReferenceDocs;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnSaveGamesAsImages;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnTranspositionDetector;
		private System.Windows.Forms.Label label5;
	}
}