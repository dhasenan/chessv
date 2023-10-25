﻿
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2017 BY GREG STRONG

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
  partial class ApmwForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApmwForm));
      this.txtApmwOutput = new System.Windows.Forms.TextBox();
      this.timer = new System.Windows.Forms.Timer(this.components);
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.button1 = new System.Windows.Forms.Button();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.timer2 = new System.Windows.Forms.Timer(this.components);
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.button2 = new System.Windows.Forms.Button();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // txtApmwOutput
      // 
      this.txtApmwOutput.BackColor = System.Drawing.Color.White;
      this.txtApmwOutput.Location = new System.Drawing.Point(12, 63);
      this.txtApmwOutput.MaxLength = 8388352;
      this.txtApmwOutput.Multiline = true;
      this.txtApmwOutput.Name = "txtApmwOutput";
      this.txtApmwOutput.ReadOnly = true;
      this.txtApmwOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtApmwOutput.Size = new System.Drawing.Size(598, 316);
      this.txtApmwOutput.TabIndex = 0;
      // 
      // timer
      // 
      this.timer.Tick += new System.EventHandler(this.timer_Tick);
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(12, 24);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(132, 20);
      this.textBox1.TabIndex = 1;
      this.textBox1.Text = "archipelago.gg:";
      this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
      this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(94, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Archipelago Room";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(147, 8);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(109, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Archipelago User Slot";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(429, 8);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(73, 37);
      this.button1.TabIndex = 6;
      this.button1.Text = "Where\'s the \'Any\' key?";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // timer1
      // 
      this.timer1.Interval = 300;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // timer2
      // 
      this.timer2.Interval = 410;
      this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
      // 
      // textBox2
      // 
      this.textBox2.Location = new System.Drawing.Point(150, 24);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(132, 20);
      this.textBox2.TabIndex = 7;
      this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
      this.textBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
      this.label3.Location = new System.Drawing.Point(9, 47);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(165, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "to connect, press enter or any key";
      // 
      // button2
      // 
      this.button2.Enabled = false;
      this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.button2.Image = global::ChessV.GUI.Properties.Resources.icon_apmw;
      this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.button2.Location = new System.Drawing.Point(508, 8);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(102, 37);
      this.button2.TabIndex = 9;
      this.button2.Text = "        Start a game!";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // textBox3
      // 
      this.textBox3.Location = new System.Drawing.Point(288, 24);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new System.Drawing.Size(132, 20);
      this.textBox3.TabIndex = 10;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(285, 8);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(112, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Archipelago Password";
      // 
      // ApmwForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(622, 391);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.textBox3);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.textBox2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.txtApmwOutput);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "ApmwForm";
      this.Text = "ApmwForm";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ApmwForm_FormClosing);
      this.Load += new System.EventHandler(this.ApmwForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox txtApmwOutput;
    private System.Windows.Forms.Timer timer;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.Timer timer2;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.Label label4;
  }
}