using System;
using System.Windows.Forms;

namespace Snowball.Demo
{
	public class DemoGameWindow : GameWindow
	{
		MenuStrip menu;
				
		public override int ClientHeight
		{
			get { return base.ClientHeight - this.menu.Height; }
			set { base.ClientHeight = value + this.menu.Height; }
		}

		public DemoGameWindow()
			: base()
		{
			this.menu = new MenuStrip();
			ToolStripMenuItem file = (ToolStripMenuItem)this.menu.Items.Add("&File");

			file.DropDownItems.Add("E&xit");

			this.Form.Controls.Add(menu);
		}
	}
}
