
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2019 BY GREG STRONG
  
  THIS FILE DERIVED FROM CUTE CHESS BY ILARI PIHLAJISTO AND ARTO JONSSON

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
	partial class EngineSettingsForm
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
			if (disposing && (components != null))
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EngineSettingsForm));
			this.tabControl = new System.Windows.Forms.TabControl();
			this.pageChessV = new System.Windows.Forms.TabPage();
			this.label6 = new System.Windows.Forms.Label();
			this.lblWeakening = new System.Windows.Forms.Label();
			this.trackWeakening = new System.Windows.Forms.TrackBar();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblVariation = new System.Windows.Forms.Label();
			this.trackVariation = new System.Windows.Forms.TrackBar();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblTTSize = new System.Windows.Forms.Label();
			this.trackTTSize = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.pageXBoard = new System.Windows.Forms.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.txtXBoardMemory = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.txtXBoardCores = new System.Windows.Forms.TextBox();
			this.tabControl.SuspendLayout();
			this.pageChessV.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackWeakening)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackVariation)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackTTSize)).BeginInit();
			this.pageXBoard.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.pageChessV);
			this.tabControl.Controls.Add(this.pageXBoard);
			this.tabControl.Location = new System.Drawing.Point(12, 12);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(610, 480);
			this.tabControl.TabIndex = 0;
			// 
			// pageChessV
			// 
			this.pageChessV.Controls.Add(this.label6);
			this.pageChessV.Controls.Add(this.lblWeakening);
			this.pageChessV.Controls.Add(this.trackWeakening);
			this.pageChessV.Controls.Add(this.label5);
			this.pageChessV.Controls.Add(this.label4);
			this.pageChessV.Controls.Add(this.lblVariation);
			this.pageChessV.Controls.Add(this.trackVariation);
			this.pageChessV.Controls.Add(this.label3);
			this.pageChessV.Controls.Add(this.label2);
			this.pageChessV.Controls.Add(this.lblTTSize);
			this.pageChessV.Controls.Add(this.trackTTSize);
			this.pageChessV.Controls.Add(this.label1);
			this.pageChessV.Location = new System.Drawing.Point(4, 22);
			this.pageChessV.Name = "pageChessV";
			this.pageChessV.Padding = new System.Windows.Forms.Padding(3);
			this.pageChessV.Size = new System.Drawing.Size(602, 454);
			this.pageChessV.TabIndex = 0;
			this.pageChessV.Text = "ChessV Settings";
			this.pageChessV.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(27, 334);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(549, 45);
			this.label6.TabIndex = 11;
			this.label6.Text = "Weakening deliberately reduces the strength of the engine through various means\r\n" +
    "to allow mere humans to have fun and maybe win";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblWeakening
			// 
			this.lblWeakening.AutoSize = true;
			this.lblWeakening.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWeakening.Location = new System.Drawing.Point(465, 286);
			this.lblWeakening.Name = "lblWeakening";
			this.lblWeakening.Size = new System.Drawing.Size(45, 16);
			this.lblWeakening.TabIndex = 10;
			this.lblWeakening.Text = "None";
			// 
			// trackWeakening
			// 
			this.trackWeakening.BackColor = System.Drawing.SystemColors.Window;
			this.trackWeakening.LargeChange = 4;
			this.trackWeakening.Location = new System.Drawing.Point(214, 286);
			this.trackWeakening.Maximum = 15;
			this.trackWeakening.Name = "trackWeakening";
			this.trackWeakening.Size = new System.Drawing.Size(234, 45);
			this.trackWeakening.TabIndex = 9;
			this.trackWeakening.Scroll += new System.EventHandler(this.trackWeakening_Scroll);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(118, 286);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(90, 16);
			this.label5.TabIndex = 8;
			this.label5.Text = "Weakening:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(27, 195);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(549, 45);
			this.label4.TabIndex = 7;
			this.label4.Text = resources.GetString("label4.Text");
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblVariation
			// 
			this.lblVariation.AutoSize = true;
			this.lblVariation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVariation.Location = new System.Drawing.Point(465, 156);
			this.lblVariation.Name = "lblVariation";
			this.lblVariation.Size = new System.Drawing.Size(47, 16);
			this.lblVariation.TabIndex = 6;
			this.lblVariation.Text = "Small";
			// 
			// trackVariation
			// 
			this.trackVariation.BackColor = System.Drawing.SystemColors.Window;
			this.trackVariation.LargeChange = 1;
			this.trackVariation.Location = new System.Drawing.Point(214, 156);
			this.trackVariation.Maximum = 3;
			this.trackVariation.Name = "trackVariation";
			this.trackVariation.Size = new System.Drawing.Size(234, 45);
			this.trackVariation.TabIndex = 5;
			this.trackVariation.Value = 1;
			this.trackVariation.Scroll += new System.EventHandler(this.trackVariation_Scroll);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(82, 156);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(126, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "Variation of Play:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(22, 75);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(549, 27);
			this.label2.TabIndex = 3;
			this.label2.Text = "The Transposition Table is a chunk of memory that allows the engine to be more ef" +
    "ficient\r\nby remembering positions seen previously.  (The same positions are ofte" +
    "n reached by different paths.)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblTTSize
			// 
			this.lblTTSize.AutoSize = true;
			this.lblTTSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTTSize.Location = new System.Drawing.Point(521, 37);
			this.lblTTSize.Name = "lblTTSize";
			this.lblTTSize.Size = new System.Drawing.Size(58, 16);
			this.lblTTSize.TabIndex = 2;
			this.lblTTSize.Text = "128 MB";
			// 
			// trackTTSize
			// 
			this.trackTTSize.BackColor = System.Drawing.SystemColors.Window;
			this.trackTTSize.LargeChange = 2;
			this.trackTTSize.Location = new System.Drawing.Point(214, 37);
			this.trackTTSize.Maximum = 8;
			this.trackTTSize.Name = "trackTTSize";
			this.trackTTSize.Size = new System.Drawing.Size(301, 45);
			this.trackTTSize.TabIndex = 1;
			this.trackTTSize.Value = 3;
			this.trackTTSize.Scroll += new System.EventHandler(this.trackTTSize_Scroll);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(22, 37);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(186, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Transposition Table Size:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
			this.btnCancel.Location = new System.Drawing.Point(338, 507);
			this.btnCancel.MaximumSize = new System.Drawing.Size(123, 32);
			this.btnCancel.MinimumSize = new System.Drawing.Size(123, 32);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(123, 32);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "    &Cancel";
			this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Image = global::ChessV.GUI.Properties.Resources.icon_ok;
			this.btnOK.Location = new System.Drawing.Point(174, 507);
			this.btnOK.MaximumSize = new System.Drawing.Size(123, 32);
			this.btnOK.MinimumSize = new System.Drawing.Size(123, 32);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(123, 32);
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "     &OK";
			this.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// pageXBoard
			// 
			this.pageXBoard.Controls.Add(this.txtXBoardCores);
			this.pageXBoard.Controls.Add(this.label9);
			this.pageXBoard.Controls.Add(this.label8);
			this.pageXBoard.Controls.Add(this.txtXBoardMemory);
			this.pageXBoard.Controls.Add(this.label7);
			this.pageXBoard.Location = new System.Drawing.Point(4, 22);
			this.pageXBoard.Name = "pageXBoard";
			this.pageXBoard.Size = new System.Drawing.Size(602, 454);
			this.pageXBoard.TabIndex = 1;
			this.pageXBoard.Text = "XBoard Engine Settings";
			this.pageXBoard.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(167, 55);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(139, 16);
			this.label7.TabIndex = 1;
			this.label7.Text = "Total Memory Use:";
			// 
			// txtXBoardMemory
			// 
			this.txtXBoardMemory.Location = new System.Drawing.Point(312, 54);
			this.txtXBoardMemory.Name = "txtXBoardMemory";
			this.txtXBoardMemory.Size = new System.Drawing.Size(37, 20);
			this.txtXBoardMemory.TabIndex = 2;
			this.txtXBoardMemory.Text = "32";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(355, 57);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(23, 13);
			this.label8.TabIndex = 3;
			this.label8.Text = "MB";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(178, 130);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(128, 16);
			this.label9.TabIndex = 4;
			this.label9.Text = "Number of Cores:";
			// 
			// txtXBoardCores
			// 
			this.txtXBoardCores.Location = new System.Drawing.Point(312, 129);
			this.txtXBoardCores.Name = "txtXBoardCores";
			this.txtXBoardCores.Size = new System.Drawing.Size(37, 20);
			this.txtXBoardCores.TabIndex = 5;
			this.txtXBoardCores.Text = "1";
			// 
			// EngineSettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LemonChiffon;
			this.ClientSize = new System.Drawing.Size(634, 547);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "EngineSettingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Engine Settings";
			this.Load += new System.EventHandler(this.EngineSettingsForm_Load);
			this.tabControl.ResumeLayout(false);
			this.pageChessV.ResumeLayout(false);
			this.pageChessV.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackWeakening)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackVariation)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackTTSize)).EndInit();
			this.pageXBoard.ResumeLayout(false);
			this.pageXBoard.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage pageChessV;
		private System.Windows.Forms.TrackBar trackTTSize;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblTTSize;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lblWeakening;
		private System.Windows.Forms.TrackBar trackWeakening;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblVariation;
		private System.Windows.Forms.TrackBar trackVariation;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TabPage pageXBoard;
		private System.Windows.Forms.TextBox txtXBoardCores;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtXBoardMemory;
		private System.Windows.Forms.Label label7;
	}
}