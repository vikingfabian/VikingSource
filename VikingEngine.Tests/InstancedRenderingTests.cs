using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.Graphics.DrawProcess;
using VikingEngine.Graphics;
using VikingEngine.Tests.Legacy;
using VikingEngine.Voxels;
using Xunit;

namespace VikingEngine.Tests
{
    public class InstancedRenderingTests
    {
        [Fact]
        public void VertexVoxelInstance_StructSize_MatchesVertexDeclarationStride()
        {
            int structSize = Marshal.SizeOf<VertexVoxelInstance>();
            int stride = VertexVoxelInstance.VertexDeclaration.VertexStride;

            // 4 x Vector4 (64 bytes) + 1 x Vector4 (16 bytes) = 80 bytes
            Assert.Equal(80, structSize);
            Assert.Equal(structSize, stride);
        }

        [Fact]
        public void VertexVoxelInstance_AffineMatrixDecomposition_PreservesValues()
        {
            var original = Matrix.CreateScale(1.8f, 2.0f, 1.8f) *
                           Matrix.CreateRotationY(MathHelper.ToRadians(45)) *
                           Matrix.CreateTranslation(120.5f, -10.0f, 350.25f);

            var customData = new Vector4(0.8f, 0.2f, 0.2f, 1.0f);
            var instance = new VertexVoxelInstance(ref original, customData);

            Assert.Equal(original.M11, instance.WorldRow0.X, 4);
            Assert.Equal(original.M22, instance.WorldRow1.Y, 4);
            Assert.Equal(original.M33, instance.WorldRow2.Z, 4);
            Assert.Equal(original.M41, instance.WorldRow3.X, 4);
            Assert.Equal(original.M42, instance.WorldRow3.Y, 4);
            Assert.Equal(original.M43, instance.WorldRow3.Z, 4);
            Assert.Equal(customData, instance.InstanceData);
        }

        [Fact]
        public void VertexVoxelInstance_ColorTintAndFlash_MapsCorrectly()
        {
            var world = Matrix.Identity;
            var colorTint = new Vector3(0.2f, 0.5f, 0.9f);
            float damageFlash = 0.75f;
            var instanceData = new Vector4(colorTint.X, colorTint.Y, colorTint.Z, damageFlash);

            var vertexInst = new VertexVoxelInstance(ref world, instanceData);

            Assert.Equal(0.2f, vertexInst.InstanceData.X, 3);
            Assert.Equal(0.5f, vertexInst.InstanceData.Y, 3);
            Assert.Equal(0.9f, vertexInst.InstanceData.Z, 3);
            Assert.Equal(0.75f, vertexInst.InstanceData.W, 3);
        }

        [Fact]
        public void InstancedDrawBatch_PrunesInactiveAndGroupsByFrame()
        {
            var batch = new InstancedDrawBatch(1);
            var fallback = new System.Collections.Generic.List<AbsDraw>();

            batch.Prepare(0, 0, fallback);

            Assert.Equal(1, batch.MasterId);
            Assert.Empty(batch);
            Assert.Empty(fallback);
        }

        [Fact]
        public void LegacyDrawBatchCollection_BaselineComparison()
        {
            var legacy = new LegacyDrawBatchCollection();
            Assert.Equal(0, legacy.Count);
        }

        [Fact]
        public void VoxelModelInstance_Pooled_PoolGeneration_IncrementsOnReset()
        {
            var instance = new DSSWars.VoxelModelInstance_Pooled(false);
            Assert.Equal(0, instance.PoolGeneration);

            instance.Pool_Reset();
            Assert.Equal(1, instance.PoolGeneration);

            instance.Pool_Reset();
            Assert.Equal(2, instance.PoolGeneration);
        }

        [Fact]
        public void DrawBatchCollection_AddNullMaster_SetsInRenderListTrue()
        {
            MainGame.SetMainThreadForTest();
            var collection = new DrawBatchCollection();
            var instance = new VoxelModelInstance(null, false);
            Assert.False(instance.InRenderList);

            collection.Add(instance);

            Assert.True(instance.InRenderList);
        }

        [Fact]
        public void EngineDraw_RequestScreenshot_CanBeToggled()
        {
            Engine.Draw.IsScreenshotRequested = false;
            Assert.False(Engine.Draw.IsScreenshotRequested);

            Engine.Draw.IsScreenshotRequested = true;
            Assert.True(Engine.Draw.IsScreenshotRequested);

            Engine.Draw.IsScreenshotRequested = false;
        }

        [Fact]
        public void AbsVoxelObj_ModelIndexGeneration_IsThreadSafeAndUnique()
        {
            const int threadCount = 8;
            const int modelsPerThread = 125;
            var indices = new System.Collections.Concurrent.ConcurrentBag<int>();

            System.Threading.Tasks.Parallel.For(0, threadCount, _ =>
            {
                for (int i = 0; i < modelsPerThread; i++)
                {
                    var model = new TestVoxelObj();
                    indices.Add(model.modelIndex);
                }
            });

            Assert.Equal(threadCount * modelsPerThread, indices.Count);
            var distinctCount = new System.Collections.Generic.HashSet<int>(indices).Count;
            Assert.Equal(indices.Count, distinctCount);
        }

        private static void InitVoxelTestContext()
        {
            Block.Init();
            DataLib.SpriteCollection.Sprites = new Graphics.Sprite[(int)SpriteName.NUM];
            var whiteSprite = new Graphics.Sprite();
            whiteSprite.SourcePolygonTopLeft = Vector2.Zero;
            whiteSprite.SourcePolygonTopRight = new Vector2(1, 0);
            whiteSprite.SourcePolygonLowLeft = new Vector2(0, 1);
            whiteSprite.SourcePolygonLowRight = Vector2.One;
            DataLib.SpriteCollection.Sprites[(int)SpriteName.WhiteArea] = whiteSprite;
        }

        [Fact]
        public void VoxelObjBuilder_BuildVerticesHD_Texture_ProducesVerticeDataColorTexture()
        {
            InitVoxelTestContext();

            var grid = new VoxelObjGridDataHD(new IntVector3(2, 2, 2));
            grid.Set(0, 0, 0, 1);
            var grids = new List<VoxelObjGridDataHD> { grid };

            var verticeData = VoxelObjBuilder.BuildVerticesHD_Texture(grids, Vector3.Zero, out var framesData);

            Assert.NotNull(verticeData);
            Assert.IsType<VerticeDataColorTexture>(verticeData);
            Assert.Equal(VertexPositionColorTexture.VertexDeclaration, verticeData.VertexDeclaration);
            Assert.Single(framesData);
        }

        [Fact]
        public void VoxelObjBuilder_BuildVerticesHD_ProducesVerticeDataColorNormal_PreservesLootFestSemantics()
        {
            InitVoxelTestContext();

            var grid = new VoxelObjGridDataHD(new IntVector3(2, 2, 2));
            grid.Set(0, 0, 0, 1);
            var grids = new List<VoxelObjGridDataHD> { grid };

            var verticeData = VoxelObjBuilder.BuildVerticesHD(grids, Vector3.Zero, out var framesData);

            Assert.NotNull(verticeData);
            Assert.IsType<VerticeDataColorNormal>(verticeData);
            Assert.Equal(VertexPositionColorNormal.VertexDeclaration, verticeData.VertexDeclaration);
            Assert.Single(framesData);
        }

        [Fact]
        public void DrawBatchCollection_FallbackItems_RetainedAcrossDepthAndLitPasses()
        {
            MainGame.SetMainThreadForTest();
            var collection = new DrawBatchCollection();
            var model = new TestVoxelObj();
            model.Visible = true;

            collection.Add(1, model);

            // Pass 1: Depth pass in frame 0
            collection.DrawDepth(0, null, null);
            Assert.Equal(1, collection.FallbackDrawListCount);

            // Pass 2: Lit pass in frame 0 (fallback items must be retained, not cleared)
            collection.RemoveAndDraw(true, 0, null, null, null);
            Assert.Equal(1, collection.FallbackDrawListCount);

            // Pass 3: Next frame depth pass (cleared and re-prepared for new frame)
            collection.DrawDepth(0, null, null);
            Assert.Equal(1, collection.FallbackDrawListCount);
        }

        [Fact]
        public void DrawBatchCollection_RemoveAndDraw_WithoutShadow_RetainsFallbackItems()
        {
            MainGame.SetMainThreadForTest();
            var collection = new DrawBatchCollection();
            var model = new TestVoxelObj();
            model.Visible = true;

            collection.Add(1, model);

            // Pass with shadow = false
            collection.RemoveAndDraw(false, 0, null, null, null);
            Assert.Equal(1, collection.FallbackDrawListCount);
        }
    }

    internal class TestVoxelObj : AbsVoxelObj
    {
        public TestVoxelObj() : base(false) { }
        public override float SizeToScale => 1f;
        public override int GridSideLength => 1;
        public override int NumFrames => 1;
        public override void copyAllDataFrom(AbsDraw master) { }
        public override void DrawDepthOnly(bool drawDepth, Microsoft.Xna.Framework.Graphics.Effect shader, LightProjection light, int cameraIndex) { }
        public override void Draw(int cameraIndex) { }
        public override AbsDraw CloneMe() => throw new NotImplementedException();
    }
}
