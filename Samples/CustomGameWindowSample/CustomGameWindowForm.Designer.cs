namespace CustomGameWindowSample
{
	partial class CustomGameWindowForm
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
			this.GameMenu = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuFileExit = new System.Windows.Forms.ToolStripMenuItem();
			this.GameBox = new System.Windows.Forms.PictureBox();
			this.GameMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.GameBox)).BeginInit();
			this.SuspendLayout();
			// 
			// Menu
			// 
			this.GameMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.GameMenu.Location = new System.Drawing.Point(0, 0);
			this.GameMenu.Name = "Menu";
			this.GameMenu.Size = new System.Drawing.Size(784, 24);
			this.GameMenu.TabIndex = 0;
			this.GameMenu.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFileExit});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// MenuFileExit
			// 
			this.MenuFileExit.Name = "MenuFileExit";
			this.MenuFileExit.Size = new System.Drawing.Size(152, 22);
			this.MenuFileExit.Text = "E&xit";
			// 
			// GameBox
			// 
			this.GameBox.BackColor = System.Drawing.Color.Black;
			this.GameBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GameBox.Location = new System.Drawing.Point(0, 24);
			this.GameBox.Name = "GameBox";
			this.GameBox.Size = new System.Drawing.Size(784, 538);
			this.GameBox.TabIndex = 1;
			this.GameBox.TabStop = false;
			// 
			// CustomGameWindowForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 562);
			this.Controls.Add(this.GameBox);
			this.Controls.Add(this.GameMenu);
			this.MainMenuStrip = this.GameMenu;
			this.Name = "CustomGameWindowForm";
			this.Text = "Snowball Custom Game Window Sample";
			this.GameMenu.ResumeLayout(false);
			this.GameMenu.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.GameBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		public System.Windows.Forms.PictureBox GameBox;
		public System.Windows.Forms.MenuStrip GameMenu;
		public System.Windows.Forms.ToolStripMenuItem MenuFileExit;
	}
}