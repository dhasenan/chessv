namespace ChessV.GUI
{
	partial class AutomatedMatchesSettingsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutomatedMatchesSettingsForm));
			this.label1 = new System.Windows.Forms.Label();
			this.txtControlFile = new System.Windows.Forms.TextBox();
			this.btnBrowseControlFile = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnLoad = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnBrowseOutputFile = new System.Windows.Forms.Button();
			this.txtOutputFile = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.chkResume = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(32, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Control File:";
			// 
			// txtControlFile
			// 
			this.txtControlFile.Location = new System.Drawing.Point(100, 25);
			this.txtControlFile.Name = "txtControlFile";
			this.txtControlFile.Size = new System.Drawing.Size(322, 20);
			this.txtControlFile.TabIndex = 1;
			// 
			// btnBrowseControlFile
			// 
			this.btnBrowseControlFile.Image = global::ChessV.GUI.Properties.Resources.icon_open_folder;
			this.btnBrowseControlFile.Location = new System.Drawing.Point(432, 20);
			this.btnBrowseControlFile.Name = "btnBrowseControlFile";
			this.btnBrowseControlFile.Size = new System.Drawing.Size(32, 28);
			this.btnBrowseControlFile.TabIndex = 5;
			this.btnBrowseControlFile.UseVisualStyleBackColor = true;
			this.btnBrowseControlFile.Click += new System.EventHandler(this.btnBrowseControlFile_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
			this.btnCancel.Location = new System.Drawing.Point(299, 199);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(123, 32);
			this.btnCancel.TabIndex = 12;
			this.btnCancel.Text = "     &Cancel";
			this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnLoad
			// 
			this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoad.Image = global::ChessV.GUI.Properties.Resources.icon_ok;
			this.btnLoad.Location = new System.Drawing.Point(107, 199);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(123, 32);
			this.btnLoad.TabIndex = 11;
			this.btnLoad.Text = " Load Matches";
			this.btnLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnBrowseOutputFile);
			this.groupBox1.Controls.Add(this.txtOutputFile);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.chkResume);
			this.groupBox1.Location = new System.Drawing.Point(12, 72);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(513, 94);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			// 
			// btnBrowseOutputFile
			// 
			this.btnBrowseOutputFile.Enabled = false;
			this.btnBrowseOutputFile.Image = global::ChessV.GUI.Properties.Resources.icon_open_folder;
			this.btnBrowseOutputFile.Location = new System.Drawing.Point(420, 34);
			this.btnBrowseOutputFile.Name = "btnBrowseOutputFile";
			this.btnBrowseOutputFile.Size = new System.Drawing.Size(32, 28);
			this.btnBrowseOutputFile.TabIndex = 6;
			this.btnBrowseOutputFile.UseVisualStyleBackColor = true;
			this.btnBrowseOutputFile.Click += new System.EventHandler(this.btnBrowseOutputFile_Click);
			// 
			// txtOutputFile
			// 
			this.txtOutputFile.Enabled = false;
			this.txtOutputFile.Location = new System.Drawing.Point(88, 39);
			this.txtOutputFile.Name = "txtOutputFile";
			this.txtOutputFile.Size = new System.Drawing.Size(322, 20);
			this.txtOutputFile.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(21, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Output File:";
			// 
			// chkResume
			// 
			this.chkResume.AutoSize = true;
			this.chkResume.Location = new System.Drawing.Point(20, -1);
			this.chkResume.Name = "chkResume";
			this.chkResume.Size = new System.Drawing.Size(142, 17);
			this.chkResume.TabIndex = 0;
			this.chkResume.Text = "Resume A Previous Run";
			this.chkResume.UseVisualStyleBackColor = true;
			this.chkResume.CheckedChanged += new System.EventHandler(this.chkResume_CheckedChanged);
			// 
			// AutomatedMatchesSettingsForm
			// 
			this.AcceptButton = this.btnLoad;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LemonChiffon;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(537, 245);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnLoad);
			this.Controls.Add(this.btnBrowseControlFile);
			this.Controls.Add(this.txtControlFile);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AutomatedMatchesSettingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Run Automated Matches";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtControlFile;
		private System.Windows.Forms.Button btnBrowseControlFile;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnBrowseOutputFile;
		private System.Windows.Forms.TextBox txtOutputFile;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkResume;
	}
}