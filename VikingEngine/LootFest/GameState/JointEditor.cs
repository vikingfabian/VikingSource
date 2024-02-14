//using VikingEngine.Engine;
//using VikingEngine.HUD;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.LootFest.GameState
//{
//    /* Optimizing 3D draw calls - an exploration
//     * ------------------------------------------------------------------------
//     * There are several things which are of great importance, the main being
//     * that we want to send data to the graphics card as seldom as possible -
//     * meaning optimally zero or once per frame. If data changes, we will need
//     * to pass data once, but then we want to make the change as small as
//     * possible. For instance, if I have a buffer of lines, and one of them
//     * change during the frame, I will only want to upload the new vertex
//     * positions for the line that changed, and nothing else. This is very
//     * easy to accomplish, but things become difficult as soon as we change
//     * more than one line each frame - especially if they are not lying beside
//     * each other in memory. Should we then upload to all the memory that lies
//     * in between as well? If the actual calls are what's expensive, then yes,
//     * but it may be that the data transfer is slow, and then the answer is
//     * suddenly no instead.
//     * So, Q: When is transfer more expensive than call?
//     * If we notice that call is more expensive, then we may want to sort the
//     * data as it changes, according to some caching-algorithm. What's feasible
//     * depends on the situation; caching is hard.
//     * 
//     */

//    struct Line3D
//    {
//        /* Fields */
//        public Vector3 a;
//        public Vector3 b;

//        /* Constructors */
//        public Line3D(Vector3 a, Vector3 b)
//        {
//            this.a = a;
//            this.b = b;
//        }
//    }

//    class Cube
//    {
//        public Vector3 position;
//        public float side;
//        public Quaternion rotation;

//        public Cube(Vector3 position, float side)
//        {
//            this.position = position;
//            this.side = side;
//            rotation = new Quaternion(0, 1, 0, 0);
//        }

//        public void Draw(DebugDraw draw)
//        {
//            Matrix mat = Matrix.CreateFromQuaternion(rotation);
//            mat *= Matrix.CreateScale(side);
//            mat *= Matrix.CreateTranslation(position);

//            //Engine.Draw.graphicsDeviceManager.GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, )
//        }
//    }

//    class DebugDraw : Engine.Draw
//    {
//        /* Constants */
//        const int MAX_LINE_COUNT = 512;
//        const int VERTICES_PER_LINE = 2;

//        /* Fields */
//        public Line3D[] lines;

//        int lastLineIndex;

//        Effect effect;
//        GraphicsDevice device;
//        VertexBuffer linesBuffer;

//        public DebugDraw()
//        {
//            effect = LoadContent.Content.Load<Effect>("Shaders/DebugGeometry");
//            device = Engine.Draw.GraphicsDevice;
//            lines = new Line3D[MAX_LINE_COUNT];
//            lastLineIndex = 0;

//            VertexDeclaration vDecl = new VertexDeclaration(new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 1));
//            linesBuffer = new VertexBuffer(Engine.Draw.GraphicsDevice, vDecl, MAX_LINE_COUNT * VERTICES_PER_LINE, BufferUsage.WriteOnly);
//        }

//        public int GetLineHandle()
//        {
//            return lastLineIndex++;
//        }

//        public int GetLineHandles(int count)
//        {
//            int prevLast = lastLineIndex;
//            lastLineIndex += count;
//            return prevLast;
//        }

//        protected override void drawEvent()
//        {
//            device.DepthStencilState = DepthStencilState.Default;
//            device.Clear(Color.Black);

//            //linesBuffer.SetData(lines);
//            linesBuffer.SetData(lines, 0, lastLineIndex);

//            effect.CurrentTechnique = effect.Techniques["DebugGeometry"];
//            Matrix wvp = Matrix.CreateScale(1000);
//            wvp *= Matrix.CreatePerspective(Engine.Screen.Resolution.X, Engine.Screen.Resolution.Y, 0.1f, 1000f);
//            effect.Parameters["WVP"].SetValue(wvp);
//            effect.CurrentTechnique.Passes[0].Apply();
//            device.SetVertexBuffer(linesBuffer);
//            device.DrawPrimitives(PrimitiveType.LineList, 0, lastLineIndex);
//            device.SetVertexBuffers(null);
//        }
//    }

//    class JointEditor : Engine.GameState
//    {
//        /* Fields */
//        DebugDraw dbgDraw;
//        Gui gui;

//        int line;
//        bool prev;

//        /* Constructors */
//        public JointEditor(int index)
//             : base(false)
//        {
//            gui = new Gui(MenuSystem2.GuiStyle, Engine.Screen.SafeArea, ImageLayers.Lay1, index);
//            OpenMenu();
//        }

//        /* Methods */
//        public override void Time_Update(float time)
//        {
//            gui.Update();
//        }

//        public override void MouseEvent(MouseEventArg e)
//        {
//            base.MouseEvent(e);
//        }

//        public override void Button_Event(ButtonValue e)
//        {
//            if (e.Button == numBUTTON.A && e.KeyDown && !prev)
//            {
                
//            }
//            prev = e.KeyDown;
//        }

//        public void OpenMenu()
//        {
//            GuiLayout layout = new GuiLayout("Menu", gui);
//            {
//                new GuiTextButton("testbtn", null, OpenMenu, false, layout);
//            }
//            layout.End();
//        }

//        protected override void createDrawManager()
//        {
//            draw = dbgDraw = new DebugDraw();
//            int lineCount = 6;
//            line = dbgDraw.GetLineHandles(lineCount);
//            for (int i = 0; i < lineCount; ++i)
//            {
//                dbgDraw.lines[line + i].a = Ref.rnd.Vector3_Sq(Vector3.Zero, 1);
//                dbgDraw.lines[line + i].b = Ref.rnd.Vector3_Sq(Vector3.Zero, 1);
//            }
            
//        }
//    }
//}
