using System;

namespace Snowball.Graphics
{
	public interface IRenderer
	{
		bool HasBegun { get; }

		void PushMatrix(Matrix matrix);

		void PopMatrix();

		void PushColor(Color color);

		void PopColor();

		void DrawLine(Vector2 v1, Vector2 v2, Color color);

		void DrawFilledRectangle(Rectangle rectangle, Color color);

		void DrawRectangle(Rectangle rectangle, Color color);
		
		void DrawRectangle(RotatableRectangle rectangle, Color color);

		void DrawTexture(Texture texture, Vector2 position, Color color);

		void DrawTexture(Texture texture, Rectangle destination, Rectangle? source, Color color);

		void DrawSprite(Sprite sprite);

		void DrawSprite(SpriteSheet spriteSheet, int frame, Vector2 position, Color color);
		
		void DrawSprite(SpriteSheet spriteSheet, int frame, Matrix transform, Color color);

		void DrawString(TextureFont textureFont, string text, Vector2 position, Color color);
	}
}
