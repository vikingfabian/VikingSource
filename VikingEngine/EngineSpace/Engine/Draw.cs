using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VikingEngine.Graphics;
using HardwareInstancing;
using VikingEngine.LootFest;

namespace VikingEngine.Engine
{
    class Draw2D : Draw
    {
        public Draw2D()
            : base()
        { }

        protected override void drawEvent()
        {
            RenderBasic2D();
        } 
    }

    class Draw
    {
        /* Readonly & const */
        const int NUM_RENDERTARGETS = 1;
        static readonly BlendState StandardBlendState = BlendState.AlphaBlend;

        /* Static */
        public static string DebugUpdateTimeText = TextLib.EmptyString;
        public static RenderTargetImage RenderTargetImageBuffer;
        public static GraphicsDeviceManager graphicsDeviceManager;
        //public static GraphicsDevice GraphicsDevice;
        public static Viewport defaultViewport;
        public static bool horizontalSplit = true;
        public static Effect effectBR;
        //public static Effect PixelShader;
        public static Graphics.CustomEffect[] TextureEffects;
        static protected RenderTarget2D MainRenderTarget;
       
        static Vector2 FPSpos;
        static IntVector2 targetSize;
        public static int MaxScreenSplit = 4;

        public static void Init()
        {
            //Set the technique names
            //Graphics.TextureEffectLib.Init();

            const int ScreenDivide = 1;
            targetSize = new IntVector2(Engine.Screen.Width, Engine.Screen.Height) / ScreenDivide;
            

            //defaultViewport = defaultView;

            Vector3 cameraPosition = new Vector3(0, 0, 1000);
            Vector3 cameraLookAt = Vector3.Zero;

            //3d stuff

            effectBR = LoadContent.LoadShader("Effect");
           
            TextureEffects = new VikingEngine.Graphics.CustomEffect[(int)Graphics.TextureEffectType.NUM_NON];

            TextureEffects[(int)Graphics.TextureEffectType.Flat] = new Graphics.CustomEffect("Flat", false);
            TextureEffects[(int)Graphics.TextureEffectType.FlatNoOpacity] = new Graphics.CustomEffect("FlatNoOpacity", false);
            TextureEffects[(int)Graphics.TextureEffectType.Shadow] = new Graphics.CustomEffect("Shadow", false);
            TextureEffects[(int)Graphics.TextureEffectType.FixedLight] = new Graphics.CustomEffect("FixedLight", true);

            //PixelShader = LoadContent.LoadShader("PixelShader");

            //Post process
            FPSpos = Vector2.Zero;
            FPSpos = Engine.Screen.SafeArea.Position * 0.6f;

            AbsText.Init();
        }

        public CustomEffect getEffect(TextureEffectType type)
        {
            return TextureEffects[(int)type]; 
        }

        public static void ApplyScreenResolution()
        {
            graphicsDeviceManager.PreferredBackBufferWidth = Screen.MonitorTargetResolution.X; //Screen.RenderingResolution.X;
            graphicsDeviceManager.PreferredBackBufferHeight = Screen.MonitorTargetResolution.Y;//Screen.RenderingResolution.Y;
            graphicsDeviceManager.ApplyChanges();
            defaultViewport = Engine.Draw.graphicsDeviceManager.GraphicsDevice.Viewport;

            MainRenderTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, 
                Screen.RenderingResolution.X, Screen.RenderingResolution.Y, 
                false, SurfaceFormat.Color, DepthFormat.Depth24);
        }

        /* Properties */
       // public LFHeightMap Heightmap { set { heightmap = value; } }
        public bool IsSplitScreen { get { return ActivePlayerScreens != null && ActivePlayerScreens.Count > 1; } }
        public int ScreenWidth  { get { return defaultViewport.Width;  } }
        public int ScreenHeight { get { return defaultViewport.Height; } }

        virtual protected int renderLayerCount { get { return 4; } }

        /*Fields */
        public ParticleSystem2 instancing; // NOTE(Martin): Particle system is in test phase only.
        public SpriteBatch spriteBatch;
       // public Mesh SkyDome;
        public AbsCamera Camera;
        public StaticList<PlayerData> ActivePlayerScreens = new StaticList<PlayerData>(MaxScreenSplit);
        public RenderList[] renderList;
        public int CurrentRenderLayer = 0;
        public Color ClrColor = Color.Black;
        public Matrix worldMatrix;
        public Matrix wvpMatrix;
        public bool DrawGround = true;
        //public SamplerState SamplerState = null;
        /// <summary>
        /// Will override the normal render list and collect the images in the container instead
        /// </summary>
        public IDrawContainer AddToContainer = null;

        public Graphics.LFHeightMap heightmap;

        /* Constructors */
        public Draw()
        {
            renderList = new RenderList[renderLayerCount];
            for (int listIx = 0; listIx < renderList.Length; listIx++)
            {
                renderList[listIx] = new RenderList();
            }
            Camera = new VikingEngine.Graphics.TopViewCamera();
            spriteBatch = new SpriteBatch(graphicsDeviceManager.GraphicsDevice);
            //spriteBatch.GraphicsDevice.DeviceLost = null;
            spriteBatch.GraphicsDevice.DeviceLost += this.onLostSpiteBatch;


            // TODO(Martin): This is test code
            if (DebugSett.Debug3DParticles)
            {
                instancing = new ParticleSystem2(new ParticleSystem2Data(Vector3.Zero, 6000, 0.5f, 500f));
                instancing.Initialize(graphicsDeviceManager.GraphicsDevice);
                instancing.Load();
            }
            // NOTE(Martin): Ends here
        }

        public void DeleteMe()
        {
            spriteBatch.GraphicsDevice.DeviceLost -= this.onLostSpiteBatch;
        }

        void onLostSpiteBatch(object sender, EventArgs e)
        {
            ClrColor = Color.Orange;
        }


        /* Methods */
        /// <returns>Screen Index</returns>
        public int AddPlayerScreen(PlayerData p)
        {
            int index = ActivePlayerScreens.Add(p);
            p.view.ScreenIndex = index;
            return index;
            //PlayerIndexToScreenIndex[(int)p.Index] = index;
        }

        public void ListMeshes(HUD.Gui menu)
        {
            VikingEngine.HUD.GuiLayout layout = new HUD.GuiLayout("Models", menu);
            {
                //DataLib.DocFile file = new DataLib.DocFile();
                SpottedArrayCounter<AbsDraw> drawList = new SpottedArrayCounter<AbsDraw>(renderList[0].GetList(Graphics.DrawObjType.Mesh));
                while (drawList.Next())
                {
                    new HUD.GuiLabel(drawList.sel.ToString(), true, layout.gui.style.textFormatDebug, layout);
                }
                //return file;
            }
            layout.End();
        }
        public void ListGeneratedModels(HUD.Gui menu)
        {
            VikingEngine.HUD.GuiLayout layout = new HUD.GuiLayout("Generated Models", menu);
            {
                SpottedArrayCounter<AbsDraw> drawList = new SpottedArrayCounter<AbsDraw>(renderList[0].GetList(Graphics.DrawObjType.MeshGenerated));
                while (drawList.Next())
                {
                    string text = drawList.sel.ToString();
                    new HUD.GuiLabel(text, true, layout.gui.style.textFormatDebug, layout);
                }
            }
            layout.End();
        }

        public void Set2DTranslation(int layer, Vector2 value)
        {
            renderList[layer].Transform2D = value;
        }
        public Vector2 Get2DTranslation(int layer)
        {
            return renderList[layer].Transform2D;
        }
        public void Set2DScale(int layer, float value)
        {
            renderList[layer].Scale2D = value;
        }
        public Vector2 LayerToScreenPos(RenderLayer layer, Vector2 position)
        {
            RenderList lay = renderList[(int)layer];
            return (position + lay.Transform2D) * lay.Scale2D;
        }

        public void AddToRenderList(Graphics.AbsDraw obj)
        {
            Debug.CrashIfThreaded();

            if (obj == null)
                throw new Exception("Draw object is null");

            if (AddToContainer == null)
            {
                obj.inRenderLayer = CurrentRenderLayer;
                renderList[CurrentRenderLayer].AddObj(obj);
            }
            else
            {
                AddToContainer.AddImage(obj);
            }
        }
        public void RemoveFromRenderList(Graphics.AbsDraw obj)
        {
            Debug.CrashIfThreaded();

            if (obj == null)
                throw new Exception("Draw object is null");

            if (AddToContainer == null)
            {
                renderList[obj.inRenderLayer].RemoveObj(obj);
            }
            else
            {
                AddToContainer.RemoveImage(obj);
            }
        }
        
        public void ClearRenderLayer(RenderLayer layer)
        {
            renderList[(int)layer].Clear();
        }

        protected void SetRenderTarget(bool set, RenderTarget2D target, Color clrColor)
        {
            if (set)
            {
                graphicsDeviceManager.GraphicsDevice.SetRenderTarget(target);
                graphicsDeviceManager.GraphicsDevice.Clear(ClrColor);

            }
            else
            {
                graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);
            }
        }

        public void settingsChanged2dImagesRefresh() 
        {
            SpottedArrayCounter<AbsDraw> renderList2D = new SpottedArrayCounter<AbsDraw>(renderList[0].GetList(Graphics.DrawObjType.Texture2D));
            while (renderList2D.Next())
            {
                renderList2D.sel.settingsChangedRefresh();
            }
        }


        public void Draw2d(int layer)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, renderList[layer].TransformMatrix);

            KeepDraw2D(layer);

            spriteBatch.End();
        }

        public void Draw2d(int layer, BlendState blendState)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, blendState, null, null, null, null, renderList[layer].TransformMatrix);

            KeepDraw2D(layer);

            spriteBatch.End();
        }

        public void KeepDraw2D(int layer)
        {
            SpottedArrayCounter<AbsDraw> renderList2D = new SpottedArrayCounter<AbsDraw>(renderList[layer].GetList(Graphics.DrawObjType.Texture2D));

            while (renderList2D.Next())
            {
                renderList2D.sel.Draw(0);
            }
        }

        public void DrawGenerated(int layer, int cameraIndex)
        {
            graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
#if DEBUG
            int activeRenderCount = 0;
#endif
            SpottedArrayCounter<AbsDraw> drawList = new SpottedArrayCounter<AbsDraw>(renderList[layer].GetList(Graphics.DrawObjType.MeshGenerated));
            while (drawList.Next())
            {
                drawList.sel.Draw(cameraIndex);
#if DEBUG
                ++activeRenderCount;
#endif
            }
#if DEBUG
            ++activeRenderCount;
#endif
        }

        protected void Draw3d(int layer, int cameraIndex)
        {
            //Debug.TimeMeasure time = new Debug.TimeMeasure("Draw3d " + layer.ToString());

            //BeginDraw3D();

            SpottedArrayCounter<AbsDraw> drawList = new SpottedArrayCounter<AbsDraw>(renderList[layer].GetList(Graphics.DrawObjType.Mesh));
            while (drawList.Next())
            {
                drawList.sel.Draw(cameraIndex);
            }

           // time.EndMeasure();

            //System.Threading.Thread.Sleep(1);
        }
        
        public void RenderBasicOneLayer()
        {
            Ref.draw.AddToContainer = null;

            spriteBatch.GraphicsDevice.Clear(ClrColor);
            Draw2d((int)RenderLayer.Layer2);

            Camera.RecalculateMatrices();
            Camera.updateBillboard();
            DrawGenerated(0, 0);

            Draw3d(0, 0);

            ParticleHandler.Draw();
           
            Draw2d(0);
        }

        public void RenderBasic2D()
        {
            Ref.draw.AddToContainer = null;



            spriteBatch.GraphicsDevice.Clear(ClrColor);

            if (spriteBatch.IsDisposed)
            {
                ClrColor = Color.Green;
            }
            
            Draw2d(0);
        }

        public void MainDrawLoop()
        {
            if (Engine.Screen.ScreenIsReady && Engine.LoadContent.BaseContentLoaded)
            {
                StateHandler.RenderLoop();

                DateTime start = DateTime.Now;

                drawInContainersEvent();

                graphicsDeviceManager.GraphicsDevice.SetRenderTarget(MainRenderTarget);
                drawEvent();

                if (PlatformSettings.DebugPerformanceText)
                {
                    spriteBatch.Begin(SpriteSortMode.BackToFront, StandardBlendState);
                    spriteBatch.DrawString(LoadContent.Font(LoadedFont.Console), DebugUpdateTimeText, FPSpos,
                        Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

                    //if (Engine.Update.IsRunningSlow)
                    //    spriteBatch.Draw(Engine.LoadContent.Texture(LoadedTexture.WhiteArea), new Rectangle(10, 10, 20, 20), Color.Red);

                    spriteBatch.End();
                    DateTime end = DateTime.Now;
                    StateHandler.RenderTimePass(end.Subtract(start).TotalMilliseconds);
                }
                if (Update.SlowDownMarker > 0)
                {
                    spriteBatch.Begin(SpriteSortMode.BackToFront, StandardBlendState);
                    spriteBatch.Draw(Engine.LoadContent.Texture(LoadedTexture.WhiteArea), new Rectangle(20, 20, 40, 40), Color.Red);
                    spriteBatch.End();
                    --Update.SlowDownMarker;
                }


                graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);

                spriteBatch.Begin();
                spriteBatch.Draw(MainRenderTarget, Screen.MonitorTargetRect, Color.White);
                spriteBatch.End();
            }
        }

        protected void clearDepthBuffer()
        {
            MainRenderTarget.GraphicsDevice.Clear(ClearOptions.DepthBuffer, ColorExt.Empty, 1f, 0);
        }

        virtual protected void drawInContainersEvent()
        { }

        virtual protected void drawEvent()
        {
            RenderBasicOneLayer();
        }

        #region UNUSED
        public void AddToRenderList(Graphics.AbsDraw obj, bool add, int layer)
        {
#if PCGAME
            if (obj == null)
                throw new Exception("Draw object is null");
#endif

            if (add)
            {
                obj.inRenderLayer = layer;
                renderList[layer].AddObj(obj);
            }
            else
            {
                renderList[layer].RemoveObj(obj);
            }
        }

        public void RenderMultilayer2D()
        {
            spriteBatch.GraphicsDevice.Clear(ClrColor);
            Draw2d(2);
            Draw2d(1);
            Draw2d(0);
        }

        public Vector2 From3DToScreenPos(Vector3 objectPos)
        {
            Vector3 pos = graphicsDeviceManager.GraphicsDevice.Viewport.Project(
                Vector3.Zero, Camera.Projection,
                Camera.ViewMatrix, Matrix.CreateTranslation(objectPos));
            Vector2 ret = Vector2.Zero;
            ret.X = pos.X;
            ret.Y = pos.Y;
            return ret;
        }

        public void RenderDebug(List<AbsDraw2D> images)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (AbsDraw2D img in images)
            {
                img.Draw(0);
            }
            spriteBatch.End();
        }
        #endregion
    }
    
    class RenderList
    {
        public Matrix TransformMatrix { get; private set; }
        Vector2 store2Dtrans = Vector2.Zero;
        float scale = 1;

        public Vector2 Transform2D
        {
            set
            {
                store2Dtrans = value;
                updateTransformMatrix();
            }
            get
            {
                return store2Dtrans;
            }
        }
        public float Scale2D
        {
            set
            {
                scale = value;
                updateTransformMatrix();
            }
            get { return scale; }
        }

        void updateTransformMatrix()
        {
            Vector3 translate = Vector3.Zero;
            translate.X = store2Dtrans.X;
            translate.Y = store2Dtrans.Y;
            TransformMatrix = Matrix.CreateTranslation(translate) * Matrix.CreateScale(new Vector3(scale, scale, 1f));
        }

        SpottedArray<Graphics.AbsDraw>[] render = new SpottedArray<Graphics.AbsDraw>[(int)Graphics.DrawObjType.NUM];

        public RenderList()
        {
            TransformMatrix = Matrix.Identity;
            for (int i = 0; i < (int)Graphics.DrawObjType.NUM; i++)
            {
                render[i] = new SpottedArray<Graphics.AbsDraw>();
            }
        }
        public void AddObj(Graphics.AbsDraw obj)
        {
            render[(int)obj.DrawType].Add(obj);
        }
        public void RemoveObj(Graphics.AbsDraw obj)
        {
            render[(int)obj.DrawType].Remove(obj);
        }
        public SpottedArray<Graphics.AbsDraw> GetList(Graphics.DrawObjType type)
        { return render[(int)type]; }
        
        public void Clear()
        {
            for (int i = 0; i < (int)Graphics.DrawObjType.NUM; i++)
            {
                render[i].Clear();
            }

            store2Dtrans = Vector2.Zero;
            scale = 1;
            updateTransformMatrix();
        }
        public override string ToString()
        {
            string result = TextLib.EmptyString;
            for (Graphics.DrawObjType type = (Graphics.DrawObjType)0; type < Graphics.DrawObjType.NUM; type++)
            {
                result += type.ToString() + render[(int)type].Count.ToString() + ", ";
            }
            return result;
        }
    }
    struct GroundTexturePart
    {
        public List<Graphics.Image> Images;
        int myIndex;

        public GroundTexturePart(int index)
        {
            myIndex = index;
            Images = new List<VikingEngine.Graphics.Image>();
        }
        
    }
    class AddDrawObj : OneTimeTrigger
    {
        Graphics.AbsDraw obj; bool add;
        public AddDrawObj(Graphics.AbsDraw obj, bool add)
            : base(false)
        {
            Debug.LogError("Adding draw obj during thread:" + obj.ToString() + ", add:" + add.ToString());

            this.obj = obj;
            this.add = add;
            AddToOrRemoveFromUpdateList(true);

        }
        public override void Time_Update(float time)
        {
            if (add)
            {
                Ref.draw.AddToRenderList(obj);
            }
           else
            {
                Ref.draw.RemoveFromRenderList(obj);
            }
        }
    }
    struct AddToRenderObj
    {
        public AbsDraw img;
        public int layer;
        public bool add;
        public AddToRenderObj(AbsDraw img, bool add)
        {
            this.img = img;
            this.add = add;
            this.layer = Ref.draw.CurrentRenderLayer;
        }
    }
}
