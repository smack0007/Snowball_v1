﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowball.Graphics;
using Snowball.Content;
using Snowball.Input;

namespace Snowball.UI
{
	public class UIController : Control
	{		
		IGraphicsDevice graphicsDevice;
				
		IMouse mouse;

		public override Point Position
		{
			get { return Point.Zero; }
			set { throw new NotSupportedException("The Position property of the UIRoot control may not be set."); }
		}

		public override Size Size
		{
			get
			{
				if (this.graphicsDevice == null)
					return Size.Zero;

				return new Size(this.graphicsDevice.BackBufferWidth, this.graphicsDevice.BackBufferHeight);
			}

			set { throw new NotSupportedException("The Size property of the UIRoot control may not be set."); }
		}
					
		/// <summary>
		/// Constructor.
		/// </summary>
		public UIController()
			: base()
		{			
		}
				
		public void Initialize(IServiceProvider services)
		{											
			this.InitializeControlInternal(services);
		}
						
		public void Update(GameTime gameTime)
		{
			if (gameTime == null)
				throw new ArgumentNullException("gameTime");
						
			this.ProcessMouseInput(this.mouse);
		}

		public void Draw(IGraphicsBatch graphics)
		{
			if (graphics == null)
				throw new ArgumentNullException("graphics");

			this.DrawControlInternal(graphics);
		}
	
		protected override void InitializeControl()
		{
			this.graphicsDevice = this.Services.GetRequiredService<IGraphicsDevice>();

			this.mouse = this.Services.GetRequiredService<IMouse>();

			IUIContentLoader contentLoader = this.Services.GetRequiredService<IUIContentLoader>();
			this.Font = contentLoader.LoadFont();

			if (this.Font == null)
				throw new InvalidOperationException("IUIContentLoader failed to load font.");
		}

		protected override void DrawControl(IGraphicsBatch graphics)
		{
		}

		protected override void OnParentChanged(EventArgs e)
		{
			throw new InvalidOperationException("UIRoot may not be added as a child of a Control.");
		}
	}
}
