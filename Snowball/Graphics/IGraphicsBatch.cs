using System;

namespace Snowball.Graphics
{
	public interface IGraphicsBatch
	{
		bool HasBegun { get; }

		void PushMatrix(Matrix matrix);

		void PopMatrix();
				
		void DrawFilledRectangle(Rectangle rectangle, Color color);

		void DrawFilledRectangle(Vector2 topLeft, Vector2 bottomRight, Color color);

		void DrawRectangle(Rectangle rectangle, Color color);
				
		void DrawTexture(Texture texture, Vector2 position, Color color);

		void DrawTexture(Texture texture, Vector2 position, Rectangle source, Color color);

		void DrawTexture(Texture texture, Rectangle destination, Color color);

		void DrawTexture(Texture texture, Rectangle destination, Rectangle source, Color color);

		void DrawTexture(Texture texture, Matrix transform, Color color);

		void DrawTexture(Texture texture, Rectangle source, Matrix transform, Color color);

		void DrawSprite(Sprite sprite);

		void DrawSprite(SpriteSheet spriteSheet, int frame, Vector2 position, Color color);
		
		void DrawSprite(SpriteSheet spriteSheet, int frame, Matrix transform, Color color);

		void DrawString(TextureFont textureFont, string text, Vector2 position, Color color);

		void Flush();
	}
}
