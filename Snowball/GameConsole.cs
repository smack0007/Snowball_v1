using System;
using System.Collections.Generic;
using System.Text;
using Snowball.Graphics;
using Snowball.Input;

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

		IGameWindow window;
		IKeyboardDevice keyboard;
				
		TimeSpan animationElapsedTime;
		
		GameConsoleLine[] lines;
		int maxLines;
		int lineCount;
		int firstLine;

		StringBuilder input;
		int cursorPosition;

		GameConsoleCommandEventArgs commandEnteredEventArgs;

		/// <summary>
		/// The current state of the console.
		/// </summary>
		public GameConsoleState State
		{
			get;
			private set;
		}

		/// <summary>
		/// If true, the console is currently visible.
		/// </summary>
		public bool IsVisible
		{
			get { return this.State != GameConsoleState.Hidden; }
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
				if(value <= 0)
					throw new ArgumentOutOfRangeException("MaxLines", "MaxLines must be > 0.");

				this.maxLines = value;
				this.UpdateLines();
			}
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
		/// Triggered whenever a command is entered.
		/// </summary>
		public event EventHandler<GameConsoleCommandEventArgs> CommandEntered;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="window"></param>
		/// <param name="keyboard"></param>
		/// <param name="font"></param>
		public GameConsole(IGameWindow window, IKeyboardDevice keyboard, TextureFont font)
		{
			if(window == null)
				throw new ArgumentNullException("window");
						
			if(keyboard == null)
				throw new ArgumentNullException("keyboard");

			this.window = window;
			this.keyboard = keyboard;

			this.BackgroundColor = Color.White;
			this.BackgroundTexture = null;
			this.Height = (int)(this.window.ClientHeight * 0.75);
			this.Animate = true;
			this.AnimationTime = TimeSpan.FromMilliseconds(500);
			this.animationElapsedTime = TimeSpan.Zero;

			this.Font = font;
			this.Padding = 4;
			
			this.maxLines = 25;
			this.lines = new GameConsoleLine[this.maxLines];
			for(int i = 0; i < this.lines.Length; i++)
				this.lines[i] = new GameConsoleLine();

			this.lineCount = 0;
			this.firstLine = 0;

			this.InputPrompt = "> ";
			this.InputColor = Color.Black;
			this.input = new StringBuilder(128);
			this.cursorPosition = 0;

			this.commandEnteredEventArgs = new GameConsoleCommandEventArgs();
			
			this.window.KeyPress += this.Window_KeyPress;
		}

		public void Update(GameTime gameTime)
		{
			if(this.keyboard.IsKeyPressed(Keys.Backtick))
			{
				if(this.State == GameConsoleState.Hidden || this.State == GameConsoleState.SlideUp)
				{
					if(this.Animate)
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
					if(this.Animate)
					{
						this.State = GameConsoleState.SlideUp;
					}
					else
					{
						this.State = GameConsoleState.Hidden;
					}
				}
			}

			if(this.State == GameConsoleState.SlideDown)
			{
				this.animationElapsedTime += gameTime.ElapsedTime;

				if(this.animationElapsedTime >= this.AnimationTime)
				{
					this.animationElapsedTime = this.AnimationTime;
					this.State = GameConsoleState.Visible;
				}
			}
			else if(this.State == GameConsoleState.SlideUp)
			{
				this.animationElapsedTime -= gameTime.ElapsedTime;

				if(this.animationElapsedTime <= TimeSpan.Zero)
				{
					this.animationElapsedTime = TimeSpan.Zero;
					this.State = GameConsoleState.Hidden;
				}
			}
		}
				
		public void Draw(IRenderer renderer)
		{
			if(this.IsVisible)
			{
				float top = 0.0f;
				
				if(this.State == GameConsoleState.SlideDown || this.State == GameConsoleState.SlideUp)
					top = this.Height - (this.Height * (float)(this.AnimationTime.TotalSeconds / this.animationElapsedTime.TotalSeconds));
				
				Rectangle rectangle = new Rectangle(0, (int)top, this.window.ClientWidth, this.Height);

				if(this.BackgroundTexture != null)
					renderer.DrawTexture(this.BackgroundTexture, rectangle, null, this.BackgroundColor);
				else
					renderer.DrawFilledRectangle(rectangle, this.BackgroundColor);

				float y = top + this.Height - this.Padding - this.Font.LineHeight;

				renderer.DrawString(this.Font, this.InputPrompt, new Vector2(this.Padding, y), this.InputColor);

				Vector2 promptSize = this.Font.MeasureString(this.InputPrompt);
				renderer.DrawString(this.Font, this.input.ToString(), new Vector2(this.Padding + promptSize.X, y), this.InputColor);

				Vector2 cursorLocation = Vector2.Zero;
				if(this.cursorPosition > 0 && this.input.Length > 0)
					cursorLocation = this.Font.MeasureString(this.input.ToString(), 0, this.cursorPosition);
				
				renderer.DrawString(this.Font, "_", new Vector2(this.Padding + promptSize.X + cursorLocation.X, y), this.InputColor);

				for(int i = 0; i < this.lineCount; i++)
				{
					int index = this.firstLine - i;
					if(index < 0)
						index += this.lines.Length;

					y -= this.Font.LineHeight;
					renderer.DrawString(this.Font, this.lines[index].Text, new Vector2(this.Padding, y), this.lines[index].Color);
				}
			}
		}

		/// <summary>
		/// Called when the KeyPress event occurs on the game window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(this.IsVisible)
			{
				if(e.KeyChar == 8) // backspace
				{
					if(this.cursorPosition > 0)
					{
						this.input.Remove(this.cursorPosition - 1, 1);
						this.cursorPosition--;
					}
				}
				else if(e.KeyChar == 13)
				{
					if(this.CommandEntered != null)
					{
						this.commandEnteredEventArgs.Command = this.input.ToString();
						this.CommandEntered(this, this.commandEnteredEventArgs);
					}

					this.input.Clear();
					this.cursorPosition = 0;
				}
				else if(e.KeyChar != '`' && this.Font.ContainsCharacter(e.KeyChar))
				{
					this.input.Append(e.KeyChar);
					this.cursorPosition++;
				}
			}
		}

		private void UpdateLines()
		{
			if(this.lines.Length != this.maxLines)
			{
				GameConsoleLine[] temp = new GameConsoleLine[this.maxLines];

				for(int i = 0; i < temp.Length && i < this.lines.Length; i++)
				{
					int index = this.firstLine - i;
					if(index < 0)
						index += this.lines.Length;

					temp[temp.Length - i - 1] = this.lines[index];
				}

				for(int i = 0; i < temp.Length; i++)
					if(temp[i] == null)
						temp[i] = new GameConsoleLine();

				if(this.lineCount > this.maxLines)
					this.lineCount = this.maxLines;

				this.lines = temp;
				this.firstLine = temp.Length - 1;
			}
		}

		public void WriteLine(string text)
		{
			this.WriteLine(text, Color.White);
		}

		public void WriteLine(string text, Color color)
		{
			this.firstLine++;
			if(this.firstLine >= this.lines.Length)
				this.firstLine -= this.lines.Length;

			this.lines[this.firstLine].Text = text;
			this.lines[this.firstLine].Color = color;

			if(this.lineCount < this.maxLines)
				this.lineCount++;
		}
	}
}
