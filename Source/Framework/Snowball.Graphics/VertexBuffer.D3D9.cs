using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using D3D9 = SharpDX.Direct3D9;

namespace Snowball.Graphics
{
	public partial class VertexBuffer
	{
		internal D3D9.VertexDeclaration d3d9VertexDeclaration;
		internal D3D9.VertexBuffer d3d9VertexBuffer;
	}

	public partial class VertexBuffer<T>
	{
		private void Construct(GraphicsDevice graphicsDevice, int capacity, VertexBufferUsage usage)
		{
			Type vertexType = typeof(T);

			var fields = vertexType.GetFields(BindingFlags.Public | BindingFlags.Instance).OrderBy(x => x.MetadataToken).ToArray();

			D3D9.VertexElement[] vertexElements = new D3D9.VertexElement[fields.Length + 1];

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
					GetDeclarationUsage(attribute.Usage),
					(byte)attribute.Index);
			}

			vertexElements[vertexElements.Length - 1] = D3D9.VertexElement.VertexDeclarationEnd;

			this.d3d9VertexDeclaration = new D3D9.VertexDeclaration(graphicsDevice.d3d9Device, vertexElements);
			this.d3d9VertexBuffer = new D3D9.VertexBuffer(graphicsDevice.d3d9Device, Marshal.SizeOf(vertexType) * capacity, GetUsage(usage), D3D9.VertexFormat.None, GetPool(usage));
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

		private static D3D9.DeclarationUsage GetDeclarationUsage(VertexElementUsage usage)
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

			throw new GraphicsException(string.Format("Unable to determine D3D9 DeclarationUsage for VertexElementUsage.{0}.", usage));
		}

		private static D3D9.Usage GetUsage(VertexBufferUsage usage)
		{
			switch (usage)
			{
				case VertexBufferUsage.Static:
					return D3D9.Usage.WriteOnly;

				case VertexBufferUsage.Dynamic:
					return D3D9.Usage.WriteOnly | D3D9.Usage.Dynamic;
			}

			throw new GraphicsException(string.Format("Unable to determine D3D9 Usage for VertexBufferUsage.{0}.", usage));
		}

		private static D3D9.Pool GetPool(VertexBufferUsage usage)
		{
			switch (usage)
			{
				case VertexBufferUsage.Static:
					return D3D9.Pool.Managed;

				case VertexBufferUsage.Dynamic:
					return D3D9.Pool.Default;
			}

			throw new GraphicsException(string.Format("Unable to determine D3D9 Pool for VertexBufferUsage.{0}.", usage));
		}

		private void SetDataInternal(T[] data)
		{
			this.d3d9VertexBuffer.Lock(0, 0, D3D9.LockFlags.None).WriteRange(data);
			this.d3d9VertexBuffer.Unlock();
		}
	}
}
