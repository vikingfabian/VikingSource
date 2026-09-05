using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DebugExtensions;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.Graphics.DrawProcess;

namespace VikingEngine.Graphics
{
    class DrawBatchCollection
    {
        private readonly Queue<AbsVoxelModelInstance> _loadingQueue = new Queue<AbsVoxelModelInstance>();
        private readonly Dictionary<int, InstancedDrawBatch> _batches = new Dictionary<int, InstancedDrawBatch>(128);
        private readonly List<AbsDraw> _fallbackDrawList = new List<AbsDraw>(64);

        public static Effect InstancedVoxelEffect;

        private int _currentRenderFrame = 0;

        // Frame Telemetry Counters
        public int LastFrameStandardDrawCalls { get; private set; } = 0;
        public int LastFrameInstancedDrawCalls { get; private set; } = 0;
        public int LastFrameRenderedInstances { get; private set; } = 0;
        public int LastFrameBatchCount { get; private set; } = 0;
        public int LastFrameFrameSlices { get; private set; } = 0;
        public long LastFrameUploadedBytes { get; private set; } = 0;

        public float LastFramePrepBatchesTimeMs { get; private set; } = 0f;
        public float LastFrameDrawDepthTimeMs { get; private set; } = 0f;
        public float LastFrameDrawLitTimeMs { get; private set; } = 0f;
        private float _accumulatedPrepTimeMs = 0f;
        private bool _depthDrawnThisFrame = false;

        public DrawBatchCollection()
        {
        }

        public static void LoadContent()
        {
            InstancedVoxelEffect = Engine.LoadContent.LoadShader("InstancedVoxelShadow");
        }

        public void Add(AbsVoxelModelInstance instance)
        {
            Debug.CrashIfThreaded();

            if (instance.master == null)
            {
                _loadingQueue.Enqueue(instance);
                return;
            }

            Add(instance.master.modelIndex, instance);
        }

        public void Add(int masterId, AbsDraw model)
        {
            Debug.CrashIfThreaded();

            if (!_batches.TryGetValue(masterId, out var batch))
            {
                batch = new InstancedDrawBatch(masterId);
                _batches.Add(masterId, batch);
            }

            batch.Add(model);
            model.OnDrawBatchAdd();
        }

        private void ProcessLoadingQueue()
        {
            while (_loadingQueue.TryPeek(out var model) && model.master != null)
            {
                if (model.InRenderList)
                {
                    Add(model.master.modelIndex, _loadingQueue.Dequeue());
                }
                else
                {
                    _loadingQueue.Dequeue().OnDrawBatchRemove();
                }
            }
        }

        public void DrawDepth(int cameraIndex, LightProjection light, Effect shader)
        {
            var startTimestamp = Stopwatch.GetTimestamp();
            _depthDrawnThisFrame = true;
            ProcessLoadingQueue();

            if (InstancedVoxelEffect != null && light != null)
            {
                // Set global light matrices
                InstancedVoxelEffect.CurrentTechnique = InstancedVoxelEffect.Techniques["InstancedDepthOnly"];
                InstancedVoxelEffect.Parameters["LightView"]?.SetValue(light.LightViewMatrix);
                InstancedVoxelEffect.Parameters["LightProjection"]?.SetValue(light.LightProjectionMatrix);
            }

            RenderBatches(true, false, cameraIndex, null, light, shader);
            LastFrameDrawDepthTimeMs = (float)Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds;
        }

        public void RemoveAndDraw(bool shadow, int cameraIndex, AbsCamera camera, Effect shader, LightProjection light)
        {
            var startTimestamp = Stopwatch.GetTimestamp();
            ProcessLoadingQueue();

            if (InstancedVoxelEffect != null)
            {
                if (camera != null)
                {
                    InstancedVoxelEffect.Parameters["View"]?.SetValue(camera.ViewMatrix);
                    InstancedVoxelEffect.Parameters["Projection"]?.SetValue(camera.Projection);
                }
                InstancedVoxelEffect.Parameters["AmbientColor"]?.SetValue(new Vector4(0.72f, 0.72f, 0.72f, 1f));
                InstancedVoxelEffect.Parameters["DiffuseColor"]?.SetValue(Vector4.One);

                if (shadow && light != null && shader != null)
                {
                    InstancedVoxelEffect.CurrentTechnique = InstancedVoxelEffect.Techniques["InstancedLitWithShadow"];
                    InstancedVoxelEffect.Parameters["LightView"]?.SetValue(light.LightViewMatrix);
                    InstancedVoxelEffect.Parameters["LightProjection"]?.SetValue(light.LightProjectionMatrix);
                    InstancedVoxelEffect.Parameters["LightDirection"]?.SetValue(light.lightDirection);

                    var shadowTex = shader.Parameters["ShadowMap"]?.GetValueTexture2D();
                    if (shadowTex != null)
                    {
                        InstancedVoxelEffect.Parameters["ShadowMap"]?.SetValue(shadowTex);
                    }
                }
                else
                {
                    InstancedVoxelEffect.CurrentTechnique = InstancedVoxelEffect.Techniques["InstancedLit"];
                }
            }

            RenderBatches(false, shadow, cameraIndex, camera, light, shader);
            LastFrameDrawLitTimeMs = (float)Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds;
        }

        private void RenderBatches(bool depthOnly, bool shadow, int cameraIndex, AbsCamera camera, LightProjection light, Effect fallbackShader)
        {
            var gd = Engine.Draw.graphicsDeviceManager.GraphicsDevice;

            // Step 1: Prepare all batches (runs only once per frame per camera)
            var prepStart = Stopwatch.GetTimestamp();
            _fallbackDrawList.Clear();
            long totalUploadedBytes = 0;

            Span<int> removeStack = stackalloc int[16];
            int removeCount = 0;

            foreach (var kv in _batches)
            {
                var batch = kv.Value;
                batch.Prepare(cameraIndex, _currentRenderFrame, _fallbackDrawList);
                totalUploadedBytes += batch.UploadedBytesThisFrame;

                if (batch.Count == 0 && removeCount < removeStack.Length)
                {
                    removeStack[removeCount++] = kv.Key;
                }
            }
            _accumulatedPrepTimeMs += (float)Stopwatch.GetElapsedTime(prepStart).TotalMilliseconds;

            // Step 2: Draw all prepared batches
            int instancedDrawCalls = 0;
            int totalRenderedInstances = 0;
            int activeBatches = 0;
            int totalFrameSlices = 0;

            foreach (var kv in _batches)
            {
                var batch = kv.Value;
                batch.Draw(depthOnly, gd, ref instancedDrawCalls, ref totalRenderedInstances, ref totalFrameSlices);
                if (batch.Count > 0)
                {
                    activeBatches++;
                }
            }

            // Unbind vertex buffers before fallback drawing to prevent state corruption
            gd.SetVertexBuffers(null);
            gd.Indices = null;

            for (int i = 0; i < removeCount; i++)
            {
                if (_batches.TryGetValue(removeStack[i], out var emptyBatch))
                {
                    emptyBatch.Dispose();
                    _batches.Remove(removeStack[i]);
                }
            }

            // Step 3: Fallback Rendering
            int standardDrawCalls = 0;
            if (_fallbackDrawList.Count > 0)
            {
                for (int i = 0; i < _fallbackDrawList.Count; i++)
                {
                    if (depthOnly)
                    {
                        (_fallbackDrawList[i] as Abs3DModel)?.DrawDepthOnly(true, fallbackShader, light, cameraIndex);
                    }
                    else if (shadow)
                    {
                        _fallbackDrawList[i].DrawWithShadow(cameraIndex, camera, fallbackShader, light);
                    }
                    else
                    {
                        _fallbackDrawList[i].Draw(cameraIndex);
                    }
                    standardDrawCalls++;
                }
            }

            if (!depthOnly)
            {
                if (!_depthDrawnThisFrame)
                {
                    LastFrameDrawDepthTimeMs = 0f;
                }
                _depthDrawnThisFrame = false;

                // Advance frame counter after lit pass completes
                _currentRenderFrame++;

                LastFrameStandardDrawCalls = standardDrawCalls;
                LastFrameInstancedDrawCalls = instancedDrawCalls;
                LastFrameRenderedInstances = totalRenderedInstances;
                LastFrameBatchCount = activeBatches;
                LastFrameFrameSlices = totalFrameSlices;
                LastFrameUploadedBytes = totalUploadedBytes;
                LastFramePrepBatchesTimeMs = _accumulatedPrepTimeMs;
                _accumulatedPrepTimeMs = 0f;
            }
        }

        public void Remove(int masterId, AbsDraw model)
        {
            Debug.CrashIfThreaded();
            if (_batches.TryGetValue(masterId, out var batch))
            {
                if (batch.Count <= 1)
                {
                    batch.Dispose();
                    _batches.Remove(masterId);
                }
                else
                {
                    batch.Remove(model);
                }
            }
        }
    }

    class InstancedDrawBatch : List<AbsDraw>, IDisposable
    {
        public readonly int MasterId;
        public struct FrameGroup
        {
            public int FrameIndex;
            public int InstanceStartIndex;
            public int InstanceCount;
        }

        private readonly List<FrameGroup> _frameGroups = new List<FrameGroup>(8);
        private readonly Dictionary<int, List<AbsVoxelModelInstance>> _framePartitions = new Dictionary<int, List<AbsVoxelModelInstance>>(8);

        // Double-buffered dynamic instance vertex buffer per batch
        private DynamicVertexBuffer _instanceBuffer;
        private int _bufferCapacity = 0;
        private VertexVoxelInstance[] _cpuData;
        private int _preparedCount = 0;
        private VoxelModel _masterModel = null;
        private int _lastPreparedFrame = -1;
        private int _lastPreparedCamera = -1;
        public long UploadedBytesThisFrame { get; private set; } = 0;

        public InstancedDrawBatch(int masterId) : base(32)
        {
            MasterId = masterId;
        }

        private void EnsureCapacity(int required)
        {
            if (required <= _bufferCapacity && _instanceBuffer != null)
            {
                return;
            }

            int newCap = Math.Max(Math.Max(_bufferCapacity * 2, required), 64);
            _bufferCapacity = newCap;
            _cpuData = new VertexVoxelInstance[newCap];

            var gd = Engine.Draw.graphicsDeviceManager.GraphicsDevice;
            _instanceBuffer?.Dispose();
            _instanceBuffer = new DynamicVertexBuffer(
                gd,
                VertexVoxelInstance.VertexDeclaration,
                _bufferCapacity,
                BufferUsage.WriteOnly
            );
        }

        public void Prepare(int cameraIndex, int frameNumber, List<AbsDraw> fallbackList)
        {
            if (_lastPreparedFrame == frameNumber && _lastPreparedCamera == cameraIndex)
            {
                // Already prepared this frame for this camera
                return;
            }

            _lastPreparedFrame = frameNumber;
            _lastPreparedCamera = cameraIndex;
            UploadedBytesThisFrame = 0;
            _preparedCount = 0;
            _masterModel = null;
            _frameGroups.Clear();

            foreach (var list in _framePartitions.Values)
            {
                list.Clear();
            }

            for (int i = Count - 1; i >= 0; i--)
            {
                var item = this[i];
                if (!item.InRenderList)
                {
                    RemoveAt(i);
                    item.OnDrawBatchRemove();
                    continue;
                }

                if (item is AbsVoxelModelInstance voxInst)
                {
                    if (voxInst.master == null)
                    {
                        continue;
                    }

                    if (_masterModel == null)
                    {
                        _masterModel = voxInst.master;
                    }

                    if (voxInst.VisibleInCamera(cameraIndex))
                    {
                        int frame = voxInst.Frame;
                        if (!_framePartitions.TryGetValue(frame, out var partitionList))
                        {
                            partitionList = new List<AbsVoxelModelInstance>(32);
                            _framePartitions[frame] = partitionList;
                        }
                        partitionList.Add(voxInst);
                    }
                }
                else
                {
                    fallbackList.Add(item);
                }
            }

            int totalInstances = 0;
            foreach (var list in _framePartitions.Values)
            {
                totalInstances += list.Count;
            }

            if (totalInstances == 0 || _masterModel == null || _masterModel.VB == null)
            {
                return;
            }

            EnsureCapacity(totalInstances);

            int writeIndex = 0;
            foreach (var kv in _framePartitions)
            {
                int frame = kv.Key;
                var instances = kv.Value;
                if (instances.Count == 0)
                {
                    continue;
                }

                int start = writeIndex;
                for (int j = 0; j < instances.Count; j++)
                {
                    var voxInst = instances[j];
                    voxInst.UpdateWorldMatrix();
                    Vector4 customData = new Vector4(voxInst.ColorV3.X, voxInst.ColorV3.Y, voxInst.ColorV3.Z, 0f);
                    _cpuData[writeIndex++].Set(ref voxInst.WorldMatrix, customData);
                }

                _frameGroups.Add(new FrameGroup
                {
                    FrameIndex = frame,
                    InstanceStartIndex = start,
                    InstanceCount = instances.Count
                });
            }

            _preparedCount = writeIndex;
            _instanceBuffer.SetData(_cpuData, 0, _preparedCount, SetDataOptions.Discard);
            UploadedBytesThisFrame = _preparedCount * VertexVoxelInstance.VertexDeclaration.VertexStride;
        }

        public void Draw(bool depthOnly, GraphicsDevice gd, ref int instancedDrawCalls, ref int totalRenderedInstances, ref int totalFrameSlices)
        {
            if (_preparedCount == 0 || _masterModel == null || _masterModel.VB == null || DrawBatchCollection.InstancedVoxelEffect == null)
            {
                return;
            }

            if (!depthOnly)
            {
                Texture2D texture = null;
                if (_masterModel.texture != LoadedTexture.NO_TEXTURE)
                {
                    texture = Engine.LoadContent.Texture(_masterModel.texture);
                }

                if (texture == null)
                {
                    texture = Engine.LoadContent.Texture(LoadedTexture.WhiteArea);
                }

                DrawBatchCollection.InstancedVoxelEffect.Parameters["MainTexture"]?.SetValue(texture);
            }

            var bindings = new VertexBufferBinding[2];
            bindings[0] = new VertexBufferBinding(_masterModel.VB.GetVertexBuffer(), 0, 0);
            bindings[1] = new VertexBufferBinding(_instanceBuffer, 0, 1);

            gd.SetVertexBuffers(bindings);
            gd.Indices = _masterModel.VB.GetIndexBuffer();

            foreach (var pass in DrawBatchCollection.InstancedVoxelEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                for (int f = 0; f < _frameGroups.Count; f++)
                {
                    var group = _frameGroups[f];
                    var frameData = _masterModel.VB.GetFrame(group.FrameIndex);

                    if (frameData.numVertices > 0 && group.InstanceCount > 0)
                    {
                        gd.DrawInstancedPrimitives(
                            PrimitiveType.TriangleList,
                            0,
                            frameData.startDrawOrderIndex,
                            frameData.primitiveCount,
                            group.InstanceStartIndex,
                            group.InstanceCount
                        );

                        instancedDrawCalls++;
                        totalRenderedInstances += group.InstanceCount;
                        totalFrameSlices++;
                    }
                }
            }
        }

        public void Dispose()
        {
            _instanceBuffer?.Dispose();
            _instanceBuffer = null;
        }
    }
}
