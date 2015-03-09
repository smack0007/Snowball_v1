namespace Snowball.WinForms.Demo
{
	partial class MainForm
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
			this.graphicsDeviceControl = new Snowball.WinForms.GraphicsDeviceControl();
			this.SuspendLayout();
			// 
			// graphicsDeviceControl
			// 
			this.graphicsDeviceControl.BackColor = System.Drawing.Color.CornflowerBlue;
			this.graphicsDeviceControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphicsDeviceControl.Location = new System.Drawing.Point(0, 0);
			this.graphicsDeviceControl.Name = "graphicsDeviceControl";
			this.graphicsDeviceControl.Size = new System.Drawing.Size(284, 262);
			this.graphicsDeviceControl.TabIndex = 0;
			this.graphicsDeviceControl.Text = "graphicsDeviceControl";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.graphicsDeviceControl);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private GraphicsDeviceControl graphicsDeviceControl;
	}
}

