using System;
using System.IO;

using D3D = SharpDX.Direct3D9;

namespace Snowball.Graphics
{
	/// <summary>
	/// Contains helper methods for working with D3D.
	/// </summary>
	internal static class D3DHelper
	{
		public static D3D.Texture CreateTexture(D3D.Device device, int width, int height, TextureUsage usage)
		{
			D3D.Usage nativeUsage = D3D.Usage.None;
			D3D.Format nativeFormat = D3D.Format.A8R8G8B8;
			D3D.Pool nativePool = D3D.Pool.Managed;

			switch (usage)
			{
				case TextureUsage.RenderTarget:
					nativeUsage = D3D.Usage.RenderTarget;
					nativePool = D3D.Pool.Default;
					break;
			}

			return new D3D.Texture(
				device,
				width,
				height,
				1,
				nativeUsage,
				nativeFormat,
				nativePool);
		}

		public static D3D.Texture TextureFromStream(D3D.Device device, Stream stream, int width, int height, int colorKey)
		{
			return D3D.Texture.FromStream(
				device,
				stream,
				width,
				height,
				1,
				D3D.Usage.None,
				D3D.Format.A8R8G8B8,
				D3D.Pool.Managed,
				D3D.Filter.Point,
				D3D.Filter.Point,
				colorKey);
		}
	}
}
