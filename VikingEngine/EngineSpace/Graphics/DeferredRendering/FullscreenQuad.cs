
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.EngineSpace.Graphics.DeferredRendering
{
    class FullscreenQuad
    {
        /* Fields */
        VertexBuffer vb;
        IndexBuffer ib;

        /* Constructors */
        public FullscreenQuad(GraphicsDevice device)
        {
            VertexPositionTexture[] vertices =
            {
                new VertexPositionTexture(new Vector3( 1, -1, 0), new Vector2(1, 1)),
                new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1)),
                new VertexPositionTexture(new Vector3(-1,  1, 0), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3( 1,  1, 0), new Vector2(1, 0)),
            };

            vb = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, vertices.Length, BufferUsage.None);
            vb.SetData<VertexPositionTexture>(vertices);

            ushort[] indices = { 0, 1, 2, 2, 3, 0 };

            ib = new IndexBuffer(device, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
            ib.SetData<ushort>(indices);
        }

        /* Methods */
        public void ReadyAndDraw(GraphicsDevice device)
        {
            ReadyBuffers(device);
            Draw(device);
        }

        public void ReadyBuffers(GraphicsDevice device)
        {
            device.SetVertexBuffer(vb);
            device.Indices = ib;
        }

        public void Draw(GraphicsDevice device)
        {
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
        }
    }
}
