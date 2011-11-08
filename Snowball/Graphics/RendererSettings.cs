using System;

namespace Snowball.Graphics
{
	/// <summary>
	/// Used to instuct the Renderer class how to render.
	/// </summary>
	public class RendererSettings
	{
		public static readonly RendererSettings Default = new RendererSettings();

		/// <summary>
		/// The function to use when pushing and popping Color(s).
		/// </summary>
		public ColorFunction ColorStackFunction
		{
			get;
			set;
		}

		/// <summary>
		/// The type of texture filtering to use.
		/// </summary>
		public TextureFilter TextureFilter
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public RendererSettings()
		{
			this.ColorStackFunction = ColorFunction.Limit;
			this.TextureFilter = TextureFilter.Linear;
		}
	}
}
