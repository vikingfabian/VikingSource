

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using VikingEngine.Engine;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace VikingEngine.Graphics
{
    abstract class ParticleSystem : AbsParticleSystem
    {
        float prevInfoTime = 0;

        public ParticleSystem()
            : base()
        { }
        
        /// <summary>
        /// Adds a new particle to the system.
        /// </summary>
        public void AddParticle(Vector3 position, Vector3 velocity)
        {
            // Figure out where in the circular queue to allocate the new particle.
            int nextFreeParticle = firstFreeParticle + 1;

            if (nextFreeParticle >= settings.MaxParticles)
                nextFreeParticle = 0;

            // OLD: If there are no free particles, we just have to give up.
            if (nextFreeParticle == firstRetiredParticle)
                return;
                //// NEW: force retire
                //if (nextFreeParticle == firstRetiredParticle)
                //{
                //    firstRetiredParticle++;
                //    if (firstRetiredParticle >= settings.MaxParticles)
                //        firstRetiredParticle = 0;

                //    firstActiveParticle++;
                //    if (firstActiveParticle >= settings.MaxParticles)
                //        firstActiveParticle = 0;
                //}
                //return;

                // Adjust the input velocity based on how much
                // this particle system wants to be affected by it.
            velocity *= settings.EmitterVelocitySensitivity;

            // Add in some random amount of horizontal velocity.


           // float horizontalVelocity = 0;
            if (settings.MinHorizontalVelocity != 0 || settings.MaxHorizontalVelocity != 0)
            {
                //var rnd = Ref.peRnd.Float();
                float horizontalVelocity = MathHelper.Lerp(settings.MinHorizontalVelocity,
                                                       settings.MaxHorizontalVelocity,
                                                         Ref.peRnd.PercentF());

                double horizontalAngle = Ref.peRnd.PercentF() * MathHelper.TwoPi;

                velocity.X += horizontalVelocity * (float)Math.Cos(horizontalAngle);
                velocity.Z += horizontalVelocity * (float)Math.Sin(horizontalAngle);
            }


            if (settings.MinVerticalVelocity != 0 || settings.MaxVerticalVelocity != 0)
            {
                // Add in some random amount of vertical velocity.
                velocity.Y += MathHelper.Lerp(settings.MinVerticalVelocity,
                                          settings.MaxVerticalVelocity,
                                          Ref.peRnd.PercentF());
            }
            // Choose four random control values. These will be used by the vertex
            // shader to give each particle a different size, rotation, and color.
            Color randomValues = new Color(Ref.peRnd.Byte(),
                                           Ref.peRnd.Byte(),
                                           Ref.peRnd.Byte(),
                                           Ref.peRnd.Byte());

            
            // Fill in the particle vertex structure.
            for (int i = 0; i < GraphicsLib.PolygonIndicesCount; i++)
            {
                int indiceIx = firstFreeParticle * GraphicsLib.PolygonIndicesCount + i;
                particles_CPU[indiceIx].Position = position;
                particles_CPU[indiceIx].Velocity = velocity;
                particles_CPU[indiceIx].Random_Vcolor = randomValues;
                particles_CPU[indiceIx].Time = currentTime;

               
            }

            if (prevInfoTime != currentTime)
            {
                prevInfoTime = currentTime;
                Debug.Log("ADD particle");
                Debug.Log(particles_CPU[firstFreeParticle * GraphicsLib.PolygonIndicesCount].ToString());
            }
            //previousParticleIndex = firstFreeParticle;
            firstFreeParticle = nextFreeParticle;
        }
        protected override LoadedEffect loadedEffect
        {
            get { return LoadedEffect.ParticleEffect; }
        } 
    }


    /// <summary>
    /// The main component in charge of displaying particles.
    /// </summary>
    abstract class AbsParticleSystem
    {
        #region Fields


        // Settings class controls the appearance and animation of this particle system.
        protected ParticleSettings settings = new ParticleSettings();


        // For loading the effect and particle texture.
        //ContentManager content;


        // Custom effect for drawing point sprite particles. This computes the particle
        // animation entirely in the vertex shader: no per-particle CPU work required!
        protected Effect particleEffect;


        // Shortcuts for accessing frequently changed effect parameters.
        EffectParameter effectViewParameter;
        EffectParameter effectProjectionParameter;
        EffectParameter effectViewportScaleParameter;
        EffectParameter effectTimeParameter;
        
        // An array of particles, treated as a circular queue.
        protected ParticleVertex[] particles_CPU;
        

        // A vertex buffer holding our particles. This contains the same data as
        // the particles array, but copied across to where the GPU can access it.
        protected DynamicVertexBuffer vertexBuffer_GPU;


        //// Vertex declaration describes the format of our ParticleVertex structure.
        //VertexDeclaration vertexDeclaration;

        // Index buffer turns sets of four vertices into particle quads (pairs of triangles).
        IndexBuffer indexBuffer;

        // The particles array and vertex buffer are treated as a circular queue.
        // Initially, the entire contents of the array are free, because no particles
        // are in use. When a new particle is created, this is allocated from the
        // beginning of the array. If more than one particle is created, these will
        // always be stored in a consecutive block of array elements. Because all
        // particles last for the same amount of time, old particles will always be
        // removed in order from the start of this active particle region, so the
        // active and free regions will never be intermingled. Because the queue is
        // circular, there can be times when the active particle region wraps from the
        // end of the array back to the start. The queue uses modulo arithmetic to
        // handle these cases. For instance with a four entry queue we could have:
        //
        //      0
        //      1 - first active particle
        //      2 
        //      3 - first free particle
        //
        // In this case, particles 1 and 2 are active, while 3 and 4 are free.
        // Using modulo arithmetic we could also have:
        //
        //      0
        //      1 - first free particle
        //      2 
        //      3 - first active particle
        //
        // Here, 3 and 0 are active, while 1 and 2 are free.
        //
        // But wait! The full story is even more complex.
        //
        // When we create a new particle, we add them to our managed particles array.
        // We also need to copy this new data into the GPU vertex buffer, but we don't
        // want to do that straight away, because setting new data into a vertex buffer
        // can be an expensive operation. If we are going to be adding several particles
        // in a single frame, it is faster to initially just store them in our managed
        // array, and then later upload them all to the GPU in one single call. So our
        // queue also needs a region for storing new particles that have been added to
        // the managed array but not yet uploaded to the vertex buffer.
        //
        // Another issue occurs when old particles are retired. The CPU and GPU run
        // asynchronously, so the GPU will often still be busy drawing the previous
        // frame while the CPU is working on the next frame. This can cause a
        // synchronization problem if an old particle is retired, and then immediately
        // overwritten by a new one, because the CPU might try to change the contents
        // of the vertex buffer while the GPU is still busy drawing the old data from
        // it. Normally the graphics driver will take care of this by waiting until
        // the GPU has finished drawing inside the VertexBuffer.SetData call, but we
        // don't want to waste time waiting around every time we try to add a new
        // particle! To avoid this delay, we can specify the SetDataOptions.NoOverwrite
        // flag when we write to the vertex buffer. This basically means "I promise I
        // will never try to overwrite any data that the GPU might still be using, so
        // you can just go ahead and update the buffer straight away". To keep this
        // promise, we must avoid reusing vertices immediately after they are drawn.
        //
        // So in total, our queue contains four different regions:
        //
        // Vertices between firstActiveParticle and firstNewParticle are actively
        // being drawn, and exist in both the managed particles array and the GPU
        // vertex buffer.
        //
        // Vertices between firstNewParticle and firstFreeParticle are newly created,
        // and exist only in the managed particles array. These need to be uploaded
        // to the GPU at the start of the next draw call.
        //
        // Vertices between firstFreeParticle and firstRetiredParticle are free and
        // waiting to be allocated.
        //
        // Vertices between firstRetiredParticle and firstActiveParticle are no longer
        // being drawn, but were drawn recently enough that the GPU could still be
        // using them. These need to be kept around for a few more frames before they
        // can be reallocated.

        protected int firstActiveParticle;
        protected int firstNewParticle;
        protected int firstFreeParticle;
        protected int firstRetiredParticle;


        // Store the current time, in seconds.
        protected float currentTime;


        // Count how many times Draw has been called. This is used to know
        // when it is safe to retire old particles back into the free list.
        int drawCounter;


        // Shared random number generator.
        //protected static Random random = new Random();


        #endregion

        #region Initialization

        public AbsParticleSystem()
        {
            InitializeSettings(settings);

            particles_CPU = new ParticleVertex[settings.MaxParticles * GraphicsLib.PolygonIndicesCount];
            for (int i = 0; i < settings.MaxParticles; i++)
            {
                particles_CPU[i * GraphicsLib.PolygonIndicesCount + 0].Corner = new Short2(-1, -1);
                particles_CPU[i * GraphicsLib.PolygonIndicesCount + 1].Corner = new Short2(1, -1);
                particles_CPU[i * GraphicsLib.PolygonIndicesCount + 2].Corner = new Short2(1, 1);
                particles_CPU[i * GraphicsLib.PolygonIndicesCount + 3].Corner = new Short2(-1, 1);
            }
            // Create a dynamic vertex buffer.
            //int vertex count är fast
            //
            vertexBuffer_GPU = new DynamicVertexBuffer(Engine.Draw.graphicsDeviceManager.GraphicsDevice, ParticleVertex.VertexDeclaration,
                                                   particles_CPU.Length, BufferUsage.WriteOnly);

            particleEffect = Engine.LoadContent.Effect(loadedEffect);//Engine.LoadContent.LoadShader("ParticleEffect");
            Initialize();
            LoadContent();
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        virtual public void Initialize()
        {
            
            

            

            //base.Initialize();
        }


        /// <summary>
        /// Derived particle system classes should override this method
        /// and use it to initalize their tweakable settings.
        /// </summary>
        protected abstract void InitializeSettings(ParticleSettings settings);


        /// <summary>
        /// Loads graphics for the particle system.
        /// </summary>
        protected virtual void LoadContent()
        {
            LoadParticleEffect();

            // Create and populate the index buffer.
            ushort[] indices = new ushort[settings.MaxParticles * GraphicsLib.PolygonDrawOrderCount];

            for (int i = 0; i < settings.MaxParticles; i++)
            {
                indices[i * GraphicsLib.PolygonDrawOrderCount + 0] = (ushort)(i * GraphicsLib.PolygonIndicesCount + 0);
                indices[i * GraphicsLib.PolygonDrawOrderCount + 1] = (ushort)(i * GraphicsLib.PolygonIndicesCount + 1);
                indices[i * GraphicsLib.PolygonDrawOrderCount + 2] = (ushort)(i * GraphicsLib.PolygonIndicesCount + 2);

                indices[i * GraphicsLib.PolygonDrawOrderCount + 3] = (ushort)(i * GraphicsLib.PolygonIndicesCount + 0);
                indices[i * GraphicsLib.PolygonDrawOrderCount + 4] = (ushort)(i * GraphicsLib.PolygonIndicesCount + 2);
                indices[i * GraphicsLib.PolygonDrawOrderCount + 5] = (ushort)(i * GraphicsLib.PolygonIndicesCount + 3);
            }

            indexBuffer = new IndexBuffer(Engine.Draw.graphicsDeviceManager.GraphicsDevice,  IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);

            indexBuffer.SetData(indices);
        }


        /// <summary>
        /// Helper for loading and initializing the particle effect.
        /// </summary>
        void LoadParticleEffect()
        {
            //Effect effect = Engine.LoadContent.LoadShader("ParticleEffect"); //content.Load<Effect>("ParticleEffect");

            // If we have several particle systems, the content manager will return
            // a single shared effect instance to them all. But we want to preconfigure
            // the effect with parameters that are specific to this particular
            // particle system. By cloning the effect, we prevent one particle system
            // from stomping over the parameter settings of another.

            particleEffect = Engine.LoadContent.LoadShader("ParticleEffect"); //particleEffect.Clone();
            EffectParameterCollection parameters = particleEffect.Parameters;
           
            effectTimeParameter = parameters["CurrentTime"];
            updateParameters();
        }

        public void updateParameters()
        {

            EffectParameterCollection parameters = particleEffect.Parameters;



            // Look up shortcuts for parameters that change every frame.

            //effectTimeParameter = parameters["CurrentTime"];

            // Set the values of parameters that do not change.
           

            parameters["Duration"].SetValue((float)settings.Duration.TotalSeconds);
            parameters["DurationRandomness"].SetValue(settings.DurationRandomness);
            parameters["Gravity"]?.SetValue(settings.Gravity);
            parameters["EndVelocity"]?.SetValue(settings.EndVelocity);
            parameters["MinColor"].SetValue(settings.MinColor.ToVector4());
            parameters["MaxColor"].SetValue(settings.MaxColor.ToVector4());

            parameters["RotateSpeed"].SetValue(
                new Vector2(settings.MinRotateSpeed, settings.MaxRotateSpeed));

            parameters["StartSize"].SetValue(
                new Vector2(settings.MinStartSize, settings.MaxStartSize));

            parameters["EndSize"].SetValue(
                new Vector2(settings.MinEndSize, settings.MaxEndSize));

            // Load the particle texture, and set it onto the effect.
            Texture2D texture = Engine.LoadContent.Texture(settings.Texture);//content.Load<Texture2D>(settings.TextureName);

            parameters["Texture"].SetValue(texture);
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the particle system.
        /// </summary>
        virtual public void Update()
        {
            if (Ref.DeltaGameTimeSec > 0)
            {
                currentTime += Ref.TargetDeltaTimeSec;//Ref.DeltaGameTimeSec;

                RetireActiveParticles();
                FreeRetiredParticles();

                // If we let our timer go on increasing for ever, it would eventually
                // run out of floating point precision, at which point the particles
                // would render incorrectly. An easy way to prevent this is to notice
                // that the time value doesn't matter when no particles are being drawn,
                // so we can reset it back to zero any time the active queue is empty.

                //if (firstActiveParticle == firstFreeParticle)
                //    currentTime = 0;

                //if (firstRetiredParticle == firstActiveParticle)
                //    drawCounter = 0;

                bool noneActive = (firstActiveParticle == firstNewParticle);
                bool noneNew = (firstNewParticle == firstFreeParticle);
                bool noneRetired = (firstRetiredParticle == firstActiveParticle);

                if (noneActive && noneNew && noneRetired)
                {
                    currentTime = 0;
                    drawCounter = 0;
                }
            }
        }




        /// <summary>
        /// Helper for checking when active particles have reached the end of
        /// their life. It moves old particles from the active area of the queue
        /// to the retired section.
        /// </summary>
        void RetireActiveParticles()
        {
            float particleDuration = (float)settings.Duration.TotalSeconds;

            while (firstActiveParticle != firstNewParticle)
            {
                // Is this particle old enough to retire?
                // We multiply the active particle index by four, because each
                // particle consists of a quad that is made up of four vertices.
                float particleAge = currentTime - particles_CPU[firstActiveParticle * GraphicsLib.PolygonIndicesCount].Time;

                if (particleAge < particleDuration)
                    break;

                // Remember the time at which we retired this particle.
                particles_CPU[firstActiveParticle * GraphicsLib.PolygonIndicesCount].Time = drawCounter;

                // Move the particle from the active to the retired queue.
                firstActiveParticle++;

                if (firstActiveParticle >= settings.MaxParticles)
                    firstActiveParticle = 0;
            }
        }


        /// <summary>
        /// Helper for checking when retired particles have been kept around long
        /// enough that we can be sure the GPU is no longer using them. It moves
        /// old particles from the retired area of the queue to the free section.
        /// </summary>
        void FreeRetiredParticles()
        {
            while (firstRetiredParticle != firstActiveParticle)
            {
                // Has this particle been unused long enough that
                // the GPU is sure to be finished with it?
                int age = drawCounter - (int)particles_CPU[firstRetiredParticle * GraphicsLib.PolygonIndicesCount].Time;

                // The GPU is never supposed to get more than 2 frames behind the CPU.
                // We add 1 to that, just to be safe in case of buggy drivers that
                // might bend the rules and let the GPU get further behind.
                const int SafetyFrames = 3;
                if (age < SafetyFrames)
                    break;

                // Move the particle from the retired to the free queue.
                firstRetiredParticle++;

                if (firstRetiredParticle >= settings.MaxParticles)
                    firstRetiredParticle = 0;
            }
        }

        
        /// <summary>
        /// Draws the particle system.
        /// </summary>
        virtual public void Draw() //GraphicsDevice device)
        {
            
            // we'd better upload them to the GPU ready for drawing.
            if (firstNewParticle != firstFreeParticle)
            {
                AddNewParticlesToVertexBuffer();
            }
            
            drawParticleRange(firstActiveParticle, firstFreeParticle);
           
            drawCounter++;
           
        }
        /// <summary>
        /// Sets the camera view and projection matrices
        /// that will be used to draw this particle system.
        /// </summary>
        public void SetCamera(AbsCamera camera)//Matrix view, Matrix projection)
        {
            EffectParameterCollection parameters = particleEffect.Parameters;
            parameters["View"].SetValue(camera.ViewMatrix);
            parameters["Projection"].SetValue(camera.Projection);
            parameters["ViewportScale"].SetValue(new Vector2(0.5f / camera.aspectRatio, -0.5f));
            //effectViewParameter.SetValue(Ref.draw.Camera.ViewMatrix);
            //effectProjectionParameter.SetValue(Ref.draw.Camera.Projection);
        }
        protected void drawParticleRange(int start, int end)
        {
            
            // If there are any active particles, draw them now!
            if (start != end)
            {
                //Debug.Log($"startParticle {start}, endParticle {end}, currentTime {currentTime}");

                updateParameters();
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = settings.BlendState;


                Engine.Draw.graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

                // Set an effect parameter describing the viewport size. This is
                // needed to convert particle sizes into screen space point sizes.
                //effectViewportScaleParameter.SetValue(new Vector2(0.5f / Engine.Draw.graphicsDeviceManager.GraphicsDevice.Viewport.AspectRatio, -0.5f));

                // Set an effect parameter describing the current time. All the vertex
                // shader particle animation is keyed off this value.
                effectTimeParameter.SetValue(currentTime);

                // Set the particle vertex and index buffer.
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetVertexBuffer(vertexBuffer_GPU);
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.Indices = indexBuffer;

                // Activate the particle effect.
                foreach (EffectPass pass in particleEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    if (start < end)
                    {
                        // If the active particles are all in one consecutive range,
                        // we can draw them all in a single call.
                        Engine.Draw.graphicsDeviceManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                     //start * 4, (end - start) * 4,
                                                     start * 6, (end - start) * 2);
                    }
                    else
                    {
                        // If the active particle range wraps past the end of the queue
                        // back to the start, we must split them over two draw calls.
                        Engine.Draw.graphicsDeviceManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                     start * 6, (settings.MaxParticles - start) * 2);

                        if (end > 0)
                        {
                            Engine.Draw.graphicsDeviceManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                         //0, end * 4,
                                                         0, end * 2);
                        }
                    }
                }

                // Reset some of the renderstates that we changed,
                // so as not to mess up any other subsequent drawing.
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
        }


        /// <summary>
        /// Helper for uploading new particles from our managed
        /// array to the GPU vertex buffer.
        /// </summary>
        void AddNewParticlesToVertexBuffer()
        {

            updateVertexBuffer(firstNewParticle, firstFreeParticle);
            

            // Move the particles we just uploaded from the new to the active queue.
            firstNewParticle = firstFreeParticle;
        }

        protected void updateVertexBuffer(int startParticle, int endParticle)
        {
            

            int stride = ParticleVertex.SizeInBytes;
            if (startParticle < endParticle)
            {
                var opts = startParticle == 0 ? SetDataOptions.Discard : SetDataOptions.NoOverwrite;
                vertexBuffer_GPU.SetData(startParticle * stride * GraphicsLib.PolygonIndicesCount,
                                         particles_CPU,
                                         startParticle * GraphicsLib.PolygonIndicesCount,
                                         (endParticle - startParticle) * GraphicsLib.PolygonIndicesCount,
                                         stride, opts);
            }
            else
            {
                // tail
                vertexBuffer_GPU.SetData(startParticle * stride * GraphicsLib.PolygonIndicesCount,
                                         particles_CPU,
                                         startParticle * GraphicsLib.PolygonIndicesCount,
                                         (settings.MaxParticles - startParticle) * GraphicsLib.PolygonIndicesCount,
                                         stride, SetDataOptions.NoOverwrite);

                // head — start of buffer: prefer Discard
                if (endParticle > 0)
                {
                    vertexBuffer_GPU.SetData(0, particles_CPU, 0,
                                             endParticle * GraphicsLib.PolygonIndicesCount,
                                             stride, SetDataOptions.Discard);
                }
            }
        }



        #endregion

        #region Public Methods

    
        abstract protected LoadedEffect loadedEffect { get; }

        #endregion
    }
}
