using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using D3D9 = SharpDX.Direct3D9;

namespace Snowball.Graphics
{
	public partial class VertexBuffer<T>
	{
		internal D3D9.VertexDeclaration d3d9VertexDeclaration;
		
		private void Construct(GraphicsDevice graphicsDevice)
		{
			Type vertexType = typeof(T);

			var fields = vertexType.GetFields(BindingFlags.Public | BindingFlags.Instance).OrderBy(x => x.MetadataToken).ToArray();

			D3D9.VertexElement[] vertexElements = new D3D9.VertexElement[fields.Length];

			for (int i = 0; i < fields.Length; i++)
			{
				var attribute = (VertexElementAttribute)fields[i].GetCustomAttributes(typeof(VertexElementAttribute), false).SingleOrDefault();

				if (attribute == null)
					throw new GraphicsException(string.Format("Vertex type {0} is missing the VertexElementAttribute for the field {1}.", vertexType, fields[i].Name));

				vertexElements[i] = new D3D9.VertexElement(
					0,
					(short)Marshal.OffsetOf(vertexType, fields[i].Name),
					GetDeclationType(fields[i].FieldType),
					D3D9.DeclarationMethod.Default,
					GetUsage(attribute.Usage),
					(byte)attribute.Index);
			}
		}

		private static D3D9.DeclarationType GetDeclationType(Type type)
		{
			if (type == typeof(Color))
			{
				return D3D9.DeclarationType.Float4;
			}
			else if (type == typeof(Vector2))
			{
				return D3D9.DeclarationType.Float2;
			}
			else if (type == typeof(Vector3))
			{
				return D3D9.DeclarationType.Float3;
			}

			throw new GraphicsException(string.Format("Unable to determine D3D9 declaration type for type {0}.", type));
		}

		private static D3D9.DeclarationUsage GetUsage(VertexElementUsage usage)
		{
			switch (usage)
			{
				case VertexElementUsage.Color:
					return D3D9.DeclarationUsage.Color;

				case VertexElementUsage.Position:
					return D3D9.DeclarationUsage.Position;

				case VertexElementUsage.TextureCoordinates:
					return D3D9.DeclarationUsage.TextureCoordinate;
			}

			throw new GraphicsException(string.Format("Unable to determine D3D9 usage for {0}.", usage));
		}
	}
}
