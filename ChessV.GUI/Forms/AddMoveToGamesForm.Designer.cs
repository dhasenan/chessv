
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
	partial class AddMoveToGamesForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddMoveToGamesForm));
			this.btnBrowseFolder = new System.Windows.Forms.Button();
			this.txtFolder = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.optFixedDepth = new System.Windows.Forms.RadioButton();
			this.optFixedTime = new System.Windows.Forms.RadioButton();
			this.txtDepth = new System.Windows.Forms.TextBox();
			this.txtTimeMinutes = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.label3 = new System.Windows.Forms.Label();
			this.pickVariation = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
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
			// optFixedDepth
			// 
			this.optFixedDepth.AutoSize = true;
			this.optFixedDepth.Checked = true;
			this.optFixedDepth.Location = new System.Drawing.Point(133, 79);
			this.optFixedDepth.Name = "optFixedDepth";
			this.optFixedDepth.Size = new System.Drawing.Size(82, 17);
			this.optFixedDepth.TabIndex = 9;
			this.optFixedDepth.TabStop = true;
			this.optFixedDepth.Text = "Fixed Depth";
			this.optFixedDepth.UseVisualStyleBackColor = true;
			// 
			// optFixedTime
			// 
			this.optFixedTime.AutoSize = true;
			this.optFixedTime.Location = new System.Drawing.Point(133, 108);
			this.optFixedTime.Name = "optFixedTime";
			this.optFixedTime.Size = new System.Drawing.Size(76, 17);
			this.optFixedTime.TabIndex = 10;
			this.optFixedTime.Text = "Fixed Time";
			this.optFixedTime.UseVisualStyleBackColor = true;
			// 
			// txtDepth
			// 
			this.txtDepth.Location = new System.Drawing.Point(230, 78);
			this.txtDepth.Name = "txtDepth";
			this.txtDepth.Size = new System.Drawing.Size(64, 20);
			this.txtDepth.TabIndex = 11;
			this.txtDepth.Text = "10";
			// 
			// txtTimeMinutes
			// 
			this.txtTimeMinutes.Location = new System.Drawing.Point(230, 107);
			this.txtTimeMinutes.Name = "txtTimeMinutes";
			this.txtTimeMinutes.Size = new System.Drawing.Size(64, 20);
			this.txtTimeMinutes.TabIndex = 12;
			this.txtTimeMinutes.Text = "5";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(300, 110);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 13);
			this.label2.TabIndex = 13;
			this.label2.Text = "minutes";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
			this.btnCancel.Location = new System.Drawing.Point(284, 245);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(123, 32);
			this.btnCancel.TabIndex = 15;
			this.btnCancel.Text = "     &Cancel";
			this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnStart
			// 
			this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStart.Image = global::ChessV.GUI.Properties.Resources.icon_ok;
			this.btnStart.Location = new System.Drawing.Point(92, 245);
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
			this.label3.Location = new System.Drawing.Point(138, 158);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(86, 13);
			this.label3.TabIndex = 16;
			this.label3.Text = "Variation of Play:";
			// 
			// pickVariation
			// 
			this.pickVariation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.pickVariation.FormattingEnabled = true;
			this.pickVariation.Items.AddRange(new object[] {
            "None",
            "Small",
            "Medium",
            "Large"});
			this.pickVariation.Location = new System.Drawing.Point(230, 155);
			this.pickVariation.Name = "pickVariation";
			this.pickVariation.Size = new System.Drawing.Size(148, 21);
			this.pickVariation.TabIndex = 17;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(143, 191);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(81, 13);
			this.label4.TabIndex = 18;
			this.label4.Text = "Hashtable Size:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(230, 191);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(31, 13);
			this.label5.TabIndex = 19;
			this.label5.Text = "1 GB";
			// 
			// AddMoveToGamesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			// this.BackColor = System.Drawing.Color.LemonChiffon;
			this.ClientSize = new System.Drawing.Size(503, 295);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.pickVariation);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtTimeMinutes);
			this.Controls.Add(this.txtDepth);
			this.Controls.Add(this.optFixedTime);
			this.Controls.Add(this.optFixedDepth);
			this.Controls.Add(this.btnBrowseFolder);
			this.Controls.Add(this.txtFolder);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AddMoveToGamesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Add Move To Games";
			this.Load += new System.EventHandler(this.AddMoveToGamesForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnBrowseFolder;
		private System.Windows.Forms.TextBox txtFolder;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton optFixedDepth;
		private System.Windows.Forms.RadioButton optFixedTime;
		private System.Windows.Forms.TextBox txtDepth;
		private System.Windows.Forms.TextBox txtTimeMinutes;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox pickVariation;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
	}
}