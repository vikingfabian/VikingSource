using System;
using System.Collections.Generic;
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

        // Double-buffered dynamic instance vertex buffers
        private DynamicVertexBuffer[] _instanceBuffers;
        private int _activeBufferIndex = 0;
        private int _bufferCapacity = 16384;
        private VertexVoxelInstance[] _cpuInstanceData;

        public static Effect InstancedVoxelEffect;

        // Frame Telemetry Counters
        public int LastFrameStandardDrawCalls { get; private set; } = 0;
        public int LastFrameInstancedDrawCalls { get; private set; } = 0;
        public int LastFrameRenderedInstances { get; private set; } = 0;
        public int LastFrameBatchCount { get; private set; } = 0;
        public int LastFrameFrameSlices { get; private set; } = 0;
        public long LastFrameUploadedBytes { get; private set; } = 0;

        public DrawBatchCollection()
        {
            _cpuInstanceData = new VertexVoxelInstance[_bufferCapacity];
            _instanceBuffers = new DynamicVertexBuffer[2];

            var gd = Engine.Draw.graphicsDeviceManager.GraphicsDevice;
            for (int i = 0; i < 2; i++)
            {
                _instanceBuffers[i] = new DynamicVertexBuffer(
                    gd,
                    VertexVoxelInstance.VertexDeclaration,
                    _bufferCapacity,
                    BufferUsage.WriteOnly
                );
            }
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
                instance.OnDrawBatchAdd();
                _loadingQueue.Enqueue(instance);
            }
            else
            {
                Add(instance.master.modelIndex, instance);
            }
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

        public void DrawDepth(int cameraIndex, LightProjection light, Effect shader)
        {
            if (InstancedVoxelEffect != null && light != null)
            {
                // Set global light matrices
                InstancedVoxelEffect.CurrentTechnique = InstancedVoxelEffect.Techniques["InstancedDepthOnly"];
                InstancedVoxelEffect.Parameters["LightView"]?.SetValue(light.LightViewMatrix);
                InstancedVoxelEffect.Parameters["LightProjection"]?.SetValue(light.LightProjectionMatrix);
            }

            SwapAndDrawBatches(true, false, cameraIndex, null, light, shader);
        }

        public void RemoveAndDraw(bool shadow, int cameraIndex, AbsCamera camera, Effect shader, LightProjection light)
        {
            // Process async loaded models
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

            SwapAndDrawBatches(false, shadow, cameraIndex, camera, light, shader);
        }

        private void SwapAndDrawBatches(bool depthOnly, bool shadow, int cameraIndex, AbsCamera camera, LightProjection light, Effect fallbackShader)
        {
            int maxInstances = 0;
            foreach (var kv in _batches)
            {
                if (kv.Value.Count > maxInstances)
                {
                    maxInstances = kv.Value.Count;
                }
            }
            EnsureCapacity(maxInstances);

            _activeBufferIndex = 1 - _activeBufferIndex;
            var currentVbo = _instanceBuffers[_activeBufferIndex];
            var gd = Engine.Draw.graphicsDeviceManager.GraphicsDevice;

            Span<int> removeStack = stackalloc int[16];
            int removeCount = 0;

            _fallbackDrawList.Clear();

            int instancedDrawCalls = 0;
            int totalRenderedInstances = 0;
            int activeBatches = 0;
            int totalFrameSlices = 0;
            long totalUploadedBytes = 0;

            foreach (var kv in _batches)
            {
                var batch = kv.Value;
                int totalActive = batch.CollectAndPrune(cameraIndex, _cpuInstanceData, _fallbackDrawList, out var masterModel, out var frameGroups);

                if (totalActive == 0)
                {
                    if (batch.Count == 0 && removeCount < removeStack.Length)
                    {
                        removeStack[removeCount++] = kv.Key;
                    }
                    continue;
                }

                if (masterModel == null || masterModel.VB == null || InstancedVoxelEffect == null)
                {
                    continue;
                }

                // Upload instance stream
                currentVbo.SetData(_cpuInstanceData, 0, totalActive, SetDataOptions.Discard);
                totalUploadedBytes += totalActive * VertexVoxelInstance.VertexDeclaration.VertexStride;

                // Set textures
                if (!depthOnly && masterModel.texture != LoadedTexture.NO_TEXTURE)
                {
                    InstancedVoxelEffect.Parameters["MainTexture"]?.SetValue(Engine.LoadContent.Texture(masterModel.texture));
                }

                // Bind Stream 0 (Geometry) and Stream 1 (Instance Transforms)
                var bindings = new VertexBufferBinding[2];
                bindings[0] = new VertexBufferBinding(masterModel.VB.GetVertexBuffer(), 0, 0);
                bindings[1] = new VertexBufferBinding(currentVbo, 0, 1);

                gd.SetVertexBuffers(bindings);
                gd.Indices = masterModel.VB.GetIndexBuffer();

                activeBatches++;

                // Draw each active animation frame slice
                foreach (var pass in InstancedVoxelEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    for (int f = 0; f < frameGroups.Count; f++)
                    {
                        var group = frameGroups[f];
                        var frameData = masterModel.VB.GetFrame(group.FrameIndex);

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

            // Unbind vertex buffers before fallback drawing to prevent state corruption
            gd.SetVertexBuffers(null);
            gd.Indices = null;

            for (int i = 0; i < removeCount; i++)
            {
                _batches.Remove(removeStack[i]);
            }

            // Fallback Rendering
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

            LastFrameStandardDrawCalls = standardDrawCalls;
            LastFrameInstancedDrawCalls = instancedDrawCalls;
            LastFrameRenderedInstances = totalRenderedInstances;
            LastFrameBatchCount = activeBatches;
            LastFrameFrameSlices = totalFrameSlices;
            LastFrameUploadedBytes = totalUploadedBytes;
        }

        private void EnsureCapacity(int required)
        {
            if (required == 0 || required <= _bufferCapacity)
            {
                return;
            }

            int newCap = Math.Max(Math.Max(_bufferCapacity * 2, required), 256);
            _bufferCapacity = newCap;
            _cpuInstanceData = new VertexVoxelInstance[newCap];

            var gd = Engine.Draw.graphicsDeviceManager.GraphicsDevice;
            for (int i = 0; i < 2; i++)
            {
                _instanceBuffers[i]?.Dispose();
                _instanceBuffers[i] = new DynamicVertexBuffer(
                    gd,
                    VertexVoxelInstance.VertexDeclaration,
                    _bufferCapacity,
                    BufferUsage.WriteOnly
                );
            }
        }

        public void Remove(int masterId, AbsDraw model)
        {
            Debug.CrashIfThreaded();
            if (_batches.TryGetValue(masterId, out var batch))
            {
                if (batch.Count <= 1)
                {
                    _batches.Remove(masterId);
                }
                else
                {
                    batch.Remove(model);
                }
            }
        }
    }

    class InstancedDrawBatch : List<AbsDraw>
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

        public InstancedDrawBatch(int masterId) : base(32)
        {
            MasterId = masterId;
        }

        public int CollectAndPrune(
            int cameraIndex,
            VertexVoxelInstance[] outputBuffer,
            List<AbsDraw> fallbackList,
            out VoxelModel masterModel,
            out List<FrameGroup> frameGroups)
        {
            masterModel = null;
            _frameGroups.Clear();
            foreach (var list in _framePartitions.Values)
            {
                list.Clear();
            }

            int writeIndex = 0;

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
                    if (masterModel == null)
                    {
                        masterModel = voxInst.master;
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

            // Pack grouped instances into output buffer
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

                    Matrix world = Matrix.CreateScale(voxInst.scale) *
                                   Matrix.CreateFromQuaternion(voxInst.Rotation.QuadRotation) *
                                   Matrix.CreateTranslation(voxInst.position);

                    Vector4 customData = new Vector4(voxInst.ColorV3.X, voxInst.ColorV3.Y, voxInst.ColorV3.Z, 0f);
                    outputBuffer[writeIndex++].Set(ref world, customData);
                }

                _frameGroups.Add(new FrameGroup
                {
                    FrameIndex = frame,
                    InstanceStartIndex = start,
                    InstanceCount = instances.Count
                });
            }

            frameGroups = _frameGroups;
            return writeIndex;
        }
    }
}
