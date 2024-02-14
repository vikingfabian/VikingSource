using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.Maths;
using VikingEngine;
using System.Diagnostics;

namespace HardwareInstancing
{
    public struct ParticleSystem2Data
    {
        public Vector3 position;
        public int maxInstanceCount;
        public float initialLifetime;
        public float spawnHz;
        public float invSpawnHz;
        // TODO(Martin): Add texture + texture atlas dimension information?

        public ParticleSystem2Data(Vector3 position, int maxInstanceCount, float initialLifetime, float spawnHz)
        {
            this.position = position;
            this.maxInstanceCount = maxInstanceCount;
            this.initialLifetime = initialLifetime;
            this.spawnHz = spawnHz;
            invSpawnHz = 1f / spawnHz;
        }
    }

    class PerformanceTimer
    {
        Action<PerformanceTimer> onNotify;
        Stopwatch stopwatch;
        DateTime lastSecond;
        public long runningTotal;
        public int timesRan;

        public PerformanceTimer(Action<PerformanceTimer> onNotify)
        {
            this.onNotify = onNotify;
            stopwatch = new Stopwatch();
            lastSecond = DateTime.Now;
            runningTotal = 0;
            timesRan = 0;
        }

        public void StartTimedBlock()
        {
            stopwatch.Reset();
        }

        public void StopTimedBlock()
        {
            stopwatch.Stop();

            runningTotal += stopwatch.ElapsedTicks;
            ++timesRan;

            DateTime now = DateTime.Now;
            if ((now - lastSecond).TotalSeconds >= 1)
            {
                onNotify(this);
                timesRan = 0;
                runningTotal = 0;
                lastSecond = now;
            }
        }
    }

    public class ParticleSystem2
    {
        /* Nested structures */
        // size: 10 * 32 bits = 40 bytes
        // allocating 1.000 instances means allocating 0.04 megabytes
        // allocating 10.000 instances means allocating 0.4 megabytes
        // allocating 100.000 instances means allocating 4 megabytes
        struct InstanceInfo
        {
            public Vector4 world;
            public Vector4 color;
            public Vector2 atlasCoordinate;
        };

        // size: 16 * 32 bits = 64 bytes
        // allocating 1.000 particles means allocating 0.064 megabytes
        // allocating 10.000 particles means allocating 0.64 megabytes
        // allocating 100.000 particles means allocating 6.4 megabytes
        struct Particle
        {
            public Vector3 p;
            public Vector3 dp;
            public Vector3 ddp;
            public float timeLeft;
            public Vector2 atlasCoordinate;
            public Vector4 color;
            //public float scale;

            public IntVector3 ComputeGridIndex(Vector3 gridOrigin, float invGridScale, int gridDim)
            {
                Vector3 t = (p - gridOrigin) * invGridScale;
                IntVector3 index = new IntVector3((int)t.X, (int)t.Y, (int)t.Z);
                index.ClampAll(1, gridDim - 2);
                return index;
            }
        }

        // Summing up the numbers above gives a good estimate of the required memory allocation of using the particle system.
        // Add to this the physics memory - GRID_DIM.X^2 * GRID_DIM.Y * (32 bits or 4 bytes). A grid size of 256, 16 takes 4 megabytes
        // Note that the actual bandwidth used per frame will often be much lower. See the update method for details.

        struct ParticleCell
        {
            public float density;
            public Vector3 velocityTimesDensity;

            public void Add(float density, Vector3 velocity)
            {
                this.density += density;
                velocityTimesDensity += density * velocity;
            }
        }

        /* Constants */
        const int ATLAS_DIMENSION = 32;
        static readonly IntVector2 GRID_DIM = new IntVector2(256, 16);

        /* Fields */
        Texture2D texture;
        Effect effect;

        VertexDeclaration instanceVertexDeclaration;

        VertexBuffer instanceBuffer;
        VertexBuffer geometryBuffer;
        IndexBuffer indexBuffer;

        VertexBufferBinding[] bindings;
        InstanceInfo[] instances;

        GraphicsDevice device;

        Random rnd;
        Particle[] particles;
        int nextSpawnIndex;
        int particlesToDraw;
        ParticleCell[, ,] grid;

        ParticleSystem2Data data;
        float timer;

        PerformanceTimer performanceTimer;

        public ParticleSystem2(ParticleSystem2Data data)
        {
            this.data = data;
        }

        public void Initialize(GraphicsDevice device)
        {
            this.device = device;

            rnd = new Random();

            GenerateInstanceVertexDeclaration();
            GenerateGeometry();
            GenerateInstanceInformation();

            bindings = new VertexBufferBinding[2];
            bindings[0] = new VertexBufferBinding(geometryBuffer);
            bindings[1] = new VertexBufferBinding(instanceBuffer, 0, 1);

            particles = new Particle[data.maxInstanceCount];

            performanceTimer = new PerformanceTimer(TimingNotification);

            grid = new ParticleCell[GRID_DIM.X, GRID_DIM.Y, GRID_DIM.X];
        }

        public void Load()
        {
            effect = LoadContent.Content.Load<Effect>("Shaders/InstancingTest");
            texture = LoadContent.Content.Load<Texture2D>("Lootfest/Lf3Tiles2");
        }

        private void GenerateInstanceVertexDeclaration()
        {
            VertexElement[] instanceStreamElements = new VertexElement[3];

            // TODO(Martin): Play with changing vector4 to vector3 and do performance testing! 
            instanceStreamElements[0] = new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position, 1);
            instanceStreamElements[1] = new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector4, VertexElementUsage.Color, 1);
            instanceStreamElements[2] = new VertexElement(sizeof(float) * 8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1);

            instanceVertexDeclaration = new VertexDeclaration(instanceStreamElements);
        }
        private void GenerateGeometry()
        {
            VertexPositionTexture[] vertices = new VertexPositionTexture[24];

            #region filling vertices
            vertices[0].Position = new Vector3(-1, 1, -1);
            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].Position = new Vector3(1, 1, -1);
            vertices[1].TextureCoordinate = new Vector2(1, 0);
            vertices[2].Position = new Vector3(-1, 1, 1);
            vertices[2].TextureCoordinate = new Vector2(0, 1);
            vertices[3].Position = new Vector3(1, 1, 1);
            vertices[3].TextureCoordinate = new Vector2(1, 1);

            vertices[4].Position = new Vector3(-1, -1, 1);
            vertices[4].TextureCoordinate = new Vector2(0, 0);
            vertices[5].Position = new Vector3(1, -1, 1);
            vertices[5].TextureCoordinate = new Vector2(1, 0);
            vertices[6].Position = new Vector3(-1, -1, -1);
            vertices[6].TextureCoordinate = new Vector2(0, 1);
            vertices[7].Position = new Vector3(1, -1, -1);
            vertices[7].TextureCoordinate = new Vector2(1, 1);

            vertices[8].Position = new Vector3(-1, 1, -1);
            vertices[8].TextureCoordinate = new Vector2(0, 0);
            vertices[9].Position = new Vector3(-1, 1, 1);
            vertices[9].TextureCoordinate = new Vector2(1, 0);
            vertices[10].Position = new Vector3(-1, -1, -1);
            vertices[10].TextureCoordinate = new Vector2(0, 1);
            vertices[11].Position = new Vector3(-1, -1, 1);
            vertices[11].TextureCoordinate = new Vector2(1, 1);

            vertices[12].Position = new Vector3(-1, 1, 1);
            vertices[12].TextureCoordinate = new Vector2(0, 0);
            vertices[13].Position = new Vector3(1, 1, 1);
            vertices[13].TextureCoordinate = new Vector2(1, 0);
            vertices[14].Position = new Vector3(-1, -1, 1);
            vertices[14].TextureCoordinate = new Vector2(0, 1);
            vertices[15].Position = new Vector3(1, -1, 1);
            vertices[15].TextureCoordinate = new Vector2(1, 1);

            vertices[16].Position = new Vector3(1, 1, 1);
            vertices[16].TextureCoordinate = new Vector2(0, 0);
            vertices[17].Position = new Vector3(1, 1, -1);
            vertices[17].TextureCoordinate = new Vector2(1, 0);
            vertices[18].Position = new Vector3(1, -1, 1);
            vertices[18].TextureCoordinate = new Vector2(0, 1);
            vertices[19].Position = new Vector3(1, -1, -1);
            vertices[19].TextureCoordinate = new Vector2(1, 1);

            vertices[20].Position = new Vector3(1, 1, -1);
            vertices[20].TextureCoordinate = new Vector2(0, 0);
            vertices[21].Position = new Vector3(-1, 1, -1);
            vertices[21].TextureCoordinate = new Vector2(1, 0);
            vertices[22].Position = new Vector3(1, -1, -1);
            vertices[22].TextureCoordinate = new Vector2(0, 1);
            vertices[23].Position = new Vector3(-1, -1, -1);
            vertices[23].TextureCoordinate = new Vector2(1, 1);
            #endregion

            for (int i = 0; i < 24; ++i)
            {
                float scale = 0.4f;
                vertices[i].Position *= scale;
            }

            geometryBuffer = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, 24, BufferUsage.WriteOnly);
            geometryBuffer.SetData(vertices);

            #region filling indices

            int[] indices = new int[36];
            indices[0] = 0; indices[1] = 1; indices[2] = 2;
            indices[3] = 1; indices[4] = 3; indices[5] = 2;

            indices[6] = 4; indices[7] = 5; indices[8] = 6;
            indices[9] = 5; indices[10] = 7; indices[11] = 6;

            indices[12] = 8; indices[13] = 9; indices[14] = 10;
            indices[15] = 9; indices[16] = 11; indices[17] = 10;

            indices[18] = 12; indices[19] = 13; indices[20] = 14;
            indices[21] = 13; indices[22] = 15; indices[23] = 14;

            indices[24] = 16; indices[25] = 17; indices[26] = 18;
            indices[27] = 17; indices[28] = 19; indices[29] = 18;

            indices[30] = 20; indices[31] = 21; indices[32] = 22;
            indices[33] = 21; indices[34] = 23; indices[35] = 22;

            #endregion

            indexBuffer = new IndexBuffer(device, typeof(int), 36, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }
        private void GenerateInstanceInformation()
        {
            instances = new InstanceInfo[data.maxInstanceCount];
            instanceBuffer = new VertexBuffer(device, instanceVertexDeclaration, data.maxInstanceCount, BufferUsage.WriteOnly);
            instanceBuffer.SetData(instances);
        }

        public void Update()
        {
            if (VikingEngine.LootFest.LfRef.LocalHeroes != null && VikingEngine.LootFest.LfRef.LocalHeroes.Count > 0)
            {
                if (data.position.Y <= 1)
                    data.position = VikingEngine.LootFest.LfRef.LocalHeroes[0].Position * 2;

                // TODO(Martin): Make timing precise.
                const float dt = 1f / 30;

                // Spawn particles!
                timer += dt;
                while (timer >= data.invSpawnHz)
                {
                    // TODO(Martin): Make class take more elaborate particle spawning parameters.
                    const float strength = 5;
                    particles[nextSpawnIndex].p = VikingEngine.LootFest.LfRef.LocalHeroes[0].Position * 2 - data.position;
                    //particles[nextSpawnIndex].dp = VikingEngine.LootFest.LfRef.LocalHeroes[0].Velocity.Value * 100;
                    //particles[nextSpawnIndex].p = new Vector3((float)rnd.NextDouble() - 0.5f, 0, (float)rnd.NextDouble() - 0.5f);
                    particles[nextSpawnIndex].dp = new Vector3(strength * (float)rnd.NextDouble(), strength * (float)rnd.NextDouble(), strength * (float)rnd.NextDouble());
                    //particles[nextSpawnIndex].dp += new Vector3(strength * ((float)rnd.NextDouble() - 0.5f), 2* strength * (float)rnd.NextDouble(), strength * ((float)rnd.NextDouble() - 0.5f));
                    particles[nextSpawnIndex].ddp = new Vector3(0, 0.1f, 0);
                    particles[nextSpawnIndex].timeLeft = data.initialLifetime;
                    //particles[nextSpawnIndex].atlasCoordinate = new Vector2(rnd.Next(ATLAS_DIMENSION), rnd.Next(ATLAS_DIMENSION));
                    particles[nextSpawnIndex].atlasCoordinate = Vector2.Zero;
                    //particles[nextSpawnIndex].color = new Vector4((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), 1f);
                    particles[nextSpawnIndex].color = new Vector4(0.3f + 0.3f * (float)rnd.NextDouble(), 0.3f * (float)rnd.NextDouble(), 0, 1.5f);

                    ++nextSpawnIndex;
                    if (nextSpawnIndex >= particles.Length)
                    {
                        nextSpawnIndex = 0;
                    }

                    timer -= data.invSpawnHz;
                }

                // Eulerian step
                //Array.Clear(grid, 0, (int)Math.Pow(GRID_DIM, 3));

                float gridScale = 0.5f;
                float invGridScale = 1f / gridScale;
                Vector3 gridOrigin = new Vector3(-0.5f * GRID_DIM.X * gridScale, 0, -0.5f * GRID_DIM.X * gridScale);

                for (int i = 0; i < data.maxInstanceCount; ++i)
                {
                    if (particles[i].timeLeft > 0)
                    {
                        float density = (float)Math.Pow(gridScale, 3); // TODO(Martin): replace with scale to 3rd power or something...

                        Vector3 gp = (particles[i].p - gridOrigin) * invGridScale;
                        IntVector3 index = new IntVector3((int)gp.X, (int)gp.Y, (int)gp.Z);
                        index.X = MathExt.Clamp(index.X, 1, GRID_DIM.X - 2);
                        index.Y = MathExt.Clamp(index.Y, 1, GRID_DIM.Y - 2);
                        index.Z = MathExt.Clamp(index.Z, 1, GRID_DIM.X - 2);

                        ParticleCell cell = grid[index.X, index.Y, index.Z];

                        cell.Add(density, particles[i].dp);
                        grid[index.X, index.Y, index.Z] = cell;
                    }
                }

                // Lagrangian step
                for (int i = 0; i < data.maxInstanceCount; ++i)
                {
                    // TODO(Martin): Make this method run from update instead, and make delta the actual frame delta
                    if (particles[i].timeLeft > 0)
                    {
                        Vector3 gp = (particles[i].p - gridOrigin) * invGridScale;
                        IntVector3 index = new IntVector3((int)gp.X, (int)gp.Y, (int)gp.Z);
                        index.X = MathExt.Clamp(index.X, 1, GRID_DIM.X - 2);
                        index.Y = MathExt.Clamp(index.Y, 1, GRID_DIM.Y - 2);
                        index.Z = MathExt.Clamp(index.Z, 1, GRID_DIM.X - 2);

                        ParticleCell cell = grid[index.X, index.Y, index.Z];
                        ParticleCell left = grid[index.X - 1, index.Y, index.Z];
                        ParticleCell right = grid[index.X + 1, index.Y, index.Z];
                        ParticleCell below = grid[index.X, index.Y - 1, index.Z];
                        ParticleCell above = grid[index.X, index.Y + 1, index.Z];
                        ParticleCell rear = grid[index.X, index.Y, index.Z - 1];
                        ParticleCell front = grid[index.X, index.Y, index.Z + 1];

                        Vector3 dispersion = Vector3.Zero;
                        float dc = 1f;
                        dispersion += dc * (cell.density - left.density) * new Vector3(-1f, 0, 0);
                        dispersion += dc * (cell.density - right.density) * new Vector3(1f, 0, 0);
                        dispersion += dc * (cell.density - below.density) * new Vector3(0, -1f, 0);
                        dispersion += dc * (cell.density - above.density) * new Vector3(0, 1f, 0);
                        dispersion += dc * (cell.density - rear.density) * new Vector3(0, 0, -1f);
                        dispersion += dc * (cell.density - front.density) * new Vector3(0, 0, 1f);
                        Vector3 ddp = particles[i].ddp + dispersion;

                        // Lagrangian step
                        particles[i].p += 0.5f * ddp * (float)Math.Pow(dt, 2) + particles[i].dp * dt;
                        particles[i].dp += ddp * dt;
                        particles[i].timeLeft = particles[i].timeLeft - dt;

                        if (particles[i].p.Y < 0f)
                        {
                            float restitution = 0.3f;
                            particles[i].p.Y = -particles[i].p.Y;
                            particles[i].dp.Y *= -restitution;
                            float friction = 0.7f;
                            particles[i].dp.X *= friction;
                            particles[i].dp.Z *= friction;
                        }
                        grid[index.X, index.Y, index.Z] = new ParticleCell();
                        //if (particles[i].p.X < -GRID_DIM / 2 || particles[i].p.X >= GRID_DIM / 2)
                        //{
                        //    particles[i].dp.X = -particles[i].dp.X;
                        //}
                        //if (particles[i].p.Z < -GRID_DIM / 2 || particles[i].p.Z >= GRID_DIM / 2)
                        //{
                        //    particles[i].dp.Z = -particles[i].dp.Z;
                        //}

                        // TODO(Martin): Explore the link to enable alpha blending
                        // http://casual-effects.blogspot.se/2015/03/implemented-weighted-blended-order.html
                        //particles[i].color.W = MathHelper.Clamp(particles[i].timeLeft / data.initialLifetime, 0f, 0.5f);

                        if (particles[i].timeLeft > 0)
                        {
                            instances[particlesToDraw].world = new Vector4(data.position + particles[i].p, 1);
                            //Vector3 col = (particles[i].p - gridOrigin) * invGridScale / GRID_DIM;
                            //particles[i].color = new Vector4(col.X, col.Y, col.Z, 1);
                            instances[particlesToDraw].color = particles[i].color;
                            instances[particlesToDraw].atlasCoordinate = particles[i].atlasCoordinate;
                            ++particlesToDraw;
                        }
                    }
                }

                if (particlesToDraw > 0)
                {
                    instanceBuffer.SetData(instances, 0, particlesToDraw);
                }
            }
        }

        void TimingNotification(PerformanceTimer data)
        {
            VikingEngine.Debug.Log("");
            VikingEngine.Debug.Log("Average ticks spent on particle system per frame: " + (data.runningTotal / data.timesRan).ToString());
            VikingEngine.Debug.Log("Average ms spent on particle system per frame : " + (1000 * data.runningTotal / (double)(data.timesRan * Stopwatch.Frequency)).ToString());
            VikingEngine.Debug.Log("Total ticks spent on particle system this second: " + data.runningTotal.ToString());
            VikingEngine.Debug.Log("Total ms spent on particle system this second: " + (1000 * data.runningTotal / (double)Stopwatch.Frequency).ToString());
        }

        public void Draw(ref Matrix view, ref Matrix projection)
        {
            performanceTimer.StartTimedBlock();

            // TODO(Martin): Move the update call to game update.
            Update();

            if (particlesToDraw > 0)
            {
                device.DepthStencilState = DepthStencilState.Default;

                effect.CurrentTechnique = effect.Techniques["Instancing"];
                effect.Parameters["WVP"].SetValue(view * projection);
                effect.Parameters["cubeTexture"].SetValue(texture);

                device.Indices = indexBuffer;
                effect.CurrentTechnique.Passes[0].Apply();

                device.BlendState = BlendState.Additive;
                //device.BlendState = BlendState.Opaque;
                device.SetVertexBuffers(bindings);
                device.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 24, 0, 12, particlesToDraw);
                device.SetVertexBuffers(null);
                particlesToDraw = 0;
            }

            performanceTimer.StopTimedBlock();
        }
    }
}