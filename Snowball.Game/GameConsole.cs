using System;
using System.Collections.Generic;
using System.Text;
using Snowball.Graphics;
using System.IO;
using System.Reflection;

namespace Snowball
{
	/// <summary>
	/// Implements a drop down "quake style" console.
	/// </summary>
	public class GameConsole : IGameConsole
	{
		class GameConsoleLine
		{
			public string Text;
			public Color Color;
		}

		GameConsoleState state;
			
		TimeSpan animationElapsedTime;
		
		GameConsoleLine[] lines;
		int maxLines;
		int lineCount;
		int firstLine;

		StringBuilder input;
		int cursorPosition;

		GameConsoleInputEventArgs inputReceivedEventArgs;
		GameConsoleOutputEventArgs outputReceivedEventArgs;

		/// <summary>
		/// The window the console is listening to.
		/// </summary>
		public IGameWindow Window
		{
			get;
			private set;
		}

		/// <summary>
		/// The graphics device.
		/// </summary>
		public IGraphicsDevice GraphicsDevice
		{
			get;
			private set;
		}
		
		/// <summary>
		/// The current state of the console.
		/// </summary>
		public GameConsoleState State
		{
			get { return this.state; }

			private set
			{
				if (value != this.state)
				{
					bool isVisible = this.IsVisible;

					this.state = value;

					bool newIsVisible = this.IsVisible;

					if (newIsVisible != isVisible)
					{
						if (this.IsVisibleChanged != null)
							this.IsVisibleChanged(this, EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>
		/// If true, the console is currently visible.
		/// </summary>
		public bool IsVisible
		{
			get { return this.State != GameConsoleState.Hidden; }
		}

		/// <summary>
		/// The default Color used when not specifying a Color.
		/// </summary>
		public Color DefaultTextColor
		{
			get;
			set;
		}

		/// <summary>
		/// The background color of the console.
		/// </summary>
		public Color BackgroundColor
		{
			get;
			set;
		}

		/// <summary>
		/// The texture used as the background of the console.
		/// </summary>
		public Texture BackgroundTexture
		{
			get;
			set;
		}

		/// <summary>
		/// The height of the console.
		/// </summary>
		public int Height
		{
			get;
			set;
		}

		/// <summary>
		/// If true, the console will animate when hiding and showing.
		/// </summary>
		public bool Animate
		{
			get;
			set;
		}

		/// <summary>
		/// The amount of time animation transitions last.
		/// </summary>
		public TimeSpan AnimationTime
		{
			get;
			set;
		}

		/// <summary>
		/// The font used for rendering the text of the console.
		/// </summary>
		public TextureFont Font
		{
			get;
			set;
		}

		/// <summary>
		/// The amount of padding to apply at the sides of the console.
		/// </summary>
		public int Padding
		{
			get;
			set;
		}

		/// <summary>
		/// The maximum number of lines stored in the input buffer.
		/// </summary>
		public int MaxLines
		{
			get { return this.maxLines; }

			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException("MaxLines", "MaxLines must be > 0.");

				this.maxLines = value;
				this.UpdateLines();
			}
		}

		/// <summary>
		/// If true, an input prompt will be displayed.
		/// </summary>
		public bool InputEnabled
		{
			get;
			set;
		}

		/// <summary>
		/// The string used for the input prompt.
		/// </summary>
		public string InputPrompt
		{
			get;
			set;
		}

		/// <summary>
		/// The color used for text input.
		/// </summary>
		public Color InputColor
		{
			get;
			set;
		}

		/// <summary>
		/// The keycode which toggles the visibility of the console.
		/// </summary>
		public int ToggleKeyCode
		{
			get;
			set;
		}

		/// <summary>
		/// Triggered whenever the the IsVisible property changes.
		/// </summary>
		public event EventHandler IsVisibleChanged;

		/// <summary>
		/// Triggered whenever something is entered into the input prompt.
		/// </summary>
		public event EventHandler<GameConsoleInputEventArgs> InputReceived;

		/// <summary>
		/// Triggered whenever something is given to be displayed in the console.
		/// </summary>
		public event EventHandler<GameConsoleOutputEventArgs> OutputReceived;

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameConsole(IGameWindow window, IGraphicsDevice graphicsDevice)
			: base()
		{
			if (window == null)
				throw new ArgumentNullException("window");

			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");
						
			this.Window = window;
			this.Window.KeyPress += this.Window_KeyPress;

			this.GraphicsDevice = graphicsDevice;

			this.inputReceivedEventArgs = new GameConsoleInputEventArgs();
			this.outputReceivedEventArgs = new GameConsoleOutputEventArgs();

			this.DefaultTextColor = Color.Black;
			this.BackgroundColor = Color.White;
			this.Height = (int)(this.Window.DisplayHeight * 0.75);
			this.Animate = true;
			this.AnimationTime = TimeSpan.FromMilliseconds(500);
			this.animationElapsedTime = TimeSpan.Zero;

			this.Padding = 4;

			this.maxLines = 25;
			this.lines = new GameConsoleLine[this.maxLines];
			for (int i = 0; i < this.lines.Length; i++)
				this.lines[i] = new GameConsoleLine();

			this.lineCount = 0;
			this.firstLine = 0;

			this.InputEnabled = false;
			this.InputPrompt = "> ";
			this.InputColor = Color.Black;
			this.input = new StringBuilder(128);
			this.cursorPosition = 0;

			this.ToggleKeyCode = 192;
		}

		public void Initialize()
		{
			if (this.Font == null)
			{
				Stream xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Snowball.GameConsoleFont.xml");

				if (xmlStream == null)
					throw new FileNotFoundException("Failed to load GameConsoleFont.xml.");

				this.Font = this.GraphicsDevice.LoadTextureFont(xmlStream, (imageFileName, colorKey) =>
				{
					Stream imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Snowball." + imageFileName);

					if (imageStream == null)
						throw new FileNotFoundException("Failed to load GameConsoleFont.png.");

					return this.GraphicsDevice.LoadTexture(imageStream, colorKey);
				});
			}
		}

		public void Toggle()
		{
			if (this.State == GameConsoleState.Hidden || this.State == GameConsoleState.SlideUp)
			{
				if (this.Animate)
				{
					this.State = GameConsoleState.SlideDown;
				}
				else
				{
					this.State = GameConsoleState.Visible;
				}
			}
			else
			{
				if (this.Animate)
				{
					this.State = GameConsoleState.SlideUp;
				}
				else
				{
					this.State = GameConsoleState.Hidden;
				}
			}
		}

		public void Show()
		{
			this.State = GameConsoleState.Visible;
			this.animationElapsedTime = this.AnimationTime;
		}

		public void Hide()
		{
			this.State = GameConsoleState.Hidden;
			this.animationElapsedTime = TimeSpan.Zero;
		}

		public void Update(GameTime gameTime)
		{
			if (this.State == GameConsoleState.SlideDown)
			{
				this.animationElapsedTime += gameTime.ElapsedTime;

				if (this.animationElapsedTime >= this.AnimationTime)
				{
					this.animationElapsedTime = this.AnimationTime;
					this.State = GameConsoleState.Visible;
				}
			}
			else if (this.State == GameConsoleState.SlideUp)
			{
				this.animationElapsedTime -= gameTime.ElapsedTime;

				if (this.animationElapsedTime <= TimeSpan.Zero)
				{
					this.animationElapsedTime = TimeSpan.Zero;
					this.State = GameConsoleState.Hidden;
				}
			}
		}
		
		private void EnsureFont()
		{
			if (this.Font == null)
				throw new InvalidOperationException("Font is null.");
		}

		public void Draw(IGraphicsBatch graphics)
		{
			if (graphics == null)
				throw new ArgumentNullException("graphics");

			this.EnsureFont();

			if (this.IsVisible)
			{
				float top = 0.0f;
				
				if (this.State == GameConsoleState.SlideDown || this.State == GameConsoleState.SlideUp)
					top = this.Height - (this.Height * (float)(this.AnimationTime.TotalSeconds / this.animationElapsedTime.TotalSeconds));
				
				Rectangle rectangle = new Rectangle(0, (int)top, this.Window.DisplayWidth, this.Height);

				if (this.BackgroundTexture != null)
					graphics.DrawTexture(this.BackgroundTexture, rectangle, this.BackgroundColor);
				else
					graphics.DrawFilledRectangle(rectangle, this.BackgroundColor);

				float y = top + this.Height - this.Padding;

				if (this.InputEnabled)
				{
					y -= this.Font.LineHeight + this.Font.LineSpacing;

					graphics.DrawString(this.Font, this.InputPrompt, new Vector2(this.Padding, y), this.InputColor);

					Size promptSize = this.Font.MeasureString(this.InputPrompt);
					graphics.DrawString(this.Font, this.input.ToString(), new Vector2(this.Padding + promptSize.Width, y), this.InputColor);

					Size cursorLocation = Size.Zero;
					if (this.cursorPosition > 0 && this.input.Length > 0)
						cursorLocation = this.Font.MeasureString(this.input.ToString(), 0, this.cursorPosition);

					graphics.DrawString(this.Font, "_", new Vector2(this.Padding + promptSize.Width + cursorLocation.Width, y), this.InputColor);
				}

				for (int i = 0; i < this.lineCount; i++)
				{
					int index = this.firstLine - i;
					
					if (index < 0)
						index += this.lines.Length;

					y -= this.Font.LineHeight + this.Font.LineSpacing;
					graphics.DrawString(this.Font, this.lines[index].Text, new Vector2(this.Padding, y), this.lines[index].Color);
				}
			}
		}

		/// <summary>
		/// Called when the KeyPress event occurs on the game window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_KeyPress(object sender, GameWindowKeyPressEventArgs e)
		{
			this.EnsureFont();

			if (e.KeyCode == this.ToggleKeyCode)
			{
				this.Toggle();
			}

			if (this.IsVisible)
			{
				if (e.KeyChar == 8) // backspace
				{
					if (this.cursorPosition > 0)
					{
						this.input.Remove(this.cursorPosition - 1, 1);
						this.cursorPosition--;
					}
				}
				else if (e.KeyChar == 13)
				{
					if (this.InputReceived != null)
					{
						this.inputReceivedEventArgs.Text = this.input.ToString();
						this.InputReceived(this, this.inputReceivedEventArgs);
					}

					this.input.Clear();
					this.cursorPosition = 0;
				}
				else if (e.KeyChar != '`' && this.Font.ContainsCharacter(e.KeyChar))
				{
					this.input.Append(e.KeyChar);
					this.cursorPosition++;
				}
			}
		}

		private void UpdateLines()
		{
			if (this.lines.Length != this.maxLines)
			{
				GameConsoleLine[] temp = new GameConsoleLine[this.maxLines];

				for (int i = 0; i < temp.Length && i < this.lines.Length; i++)
				{
					int index = this.firstLine - i;
					if (index < 0)
						index += this.lines.Length;

					temp[temp.Length - i - 1] = this.lines[index];
				}

				for (int i = 0; i < temp.Length; i++)
					if (temp[i] == null)
						temp[i] = new GameConsoleLine();

				if (this.lineCount > this.maxLines)
					this.lineCount = this.maxLines;

				this.lines = temp;
				this.firstLine = temp.Length - 1;
			}
		}

		public void WriteLine(string text)
		{
			this.WriteLine(text, this.DefaultTextColor);
		}

		public void WriteLine(string text, Color color)
		{
			this.firstLine++;
			if (this.firstLine >= this.lines.Length)
				this.firstLine -= this.lines.Length;
						
			this.lines[this.firstLine].Text = text;
			this.lines[this.firstLine].Color = color;
						
			if (this.lineCount < this.maxLines)
				this.lineCount++;

			this.outputReceivedEventArgs.Text = text;
			this.outputReceivedEventArgs.Color = color;

			if (this.OutputReceived != null)
				this.OutputReceived(this, this.outputReceivedEventArgs);
		}
	}
}
